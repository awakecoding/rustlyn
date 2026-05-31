param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('quick_xml', 'simd_json', 'marked_yaml', 'csv', 'toml', 'bson', 'cbor', 'powershell_cmdlets')]
    [string]$Sample,

    [Parameter(Mandatory = $true)]
    [string]$EngineAssemblyName,

    [Parameter(Mandatory = $true)]
    [string[]]$CmdletsToExport,

    [Parameter(Mandatory = $true)]
    [string]$ManifestFileName,

    [Parameter(Mandatory = $true)]
    [guid]$ModuleGuid,

    [Parameter(Mandatory = $true)]
    [string]$OutDir,

    [string]$Toolchain = 'nightly',
    [string]$BuildStd = 'std,panic_abort'
)

$ErrorActionPreference = 'Stop'
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$outPath = if ([System.IO.Path]::IsPathRooted($OutDir)) { $OutDir } else { Join-Path $repoRoot $OutDir }
$outPath = [System.IO.Path]::GetFullPath($outPath)
$cmdletProject = Join-Path $repoRoot 'dotnet\backend\src\Rustlyn.PowerShellCmdlets\Rustlyn.PowerShellCmdlets.csproj'
$toolProject = Join-Path $repoRoot 'dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj'
$samplePath = Join-Path $repoRoot "samples\$Sample"
$cmdletBuildDir = Join-Path $repoRoot 'dotnet\backend\src\Rustlyn.PowerShellCmdlets\bin\Release\net10.0'
. (Join-Path $PSScriptRoot 'Rustlyn.Cli.ps1')

if (Test-Path $outPath) {
    Remove-Item -LiteralPath $outPath -Recurse -Force
}
New-Item -ItemType Directory -Path $outPath | Out-Null

if ($Sample -eq 'powershell_cmdlets') {
    if ($EngineAssemblyName -ne 'rustlyn_powershell_format_cmdlets.dll') {
        throw "The unified generated PowerShell cmdlet runtime must be packaged as 'rustlyn_powershell_format_cmdlets.dll' because generated cmdlet shims load that engine name."
    }
}

dotnet build $cmdletProject -c Release
if ($LASTEXITCODE -ne 0) {
    throw "Failed to build Rustlyn.PowerShellCmdlets."
}

dotnet build $toolProject -c Release /nologo
if ($LASTEXITCODE -ne 0) {
    throw "Failed to build rustlyn."
}

$previousCargoIncremental = $env:CARGO_INCREMENTAL
$previousCargoTargetDir = $env:CARGO_TARGET_DIR
$env:CARGO_INCREMENTAL = '0'
$env:CARGO_TARGET_DIR = Join-Path ([System.IO.Path]::GetTempPath()) "rustlyn-cargo-target-$Sample"
try {
    $rustlyn = Resolve-RustlynCli -RepoRoot $repoRoot -Configuration Release
    Invoke-RustlynCli $rustlyn translate $samplePath --out (Join-Path $outPath $EngineAssemblyName) --bitcode-out (Join-Path $outPath ([System.IO.Path]::ChangeExtension($EngineAssemblyName, '.bc'))) --toolchain $Toolchain --build-std $BuildStd --powershell-cmdlet-bindings
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to translate sample '$Sample'."
    }
}
finally {
    $env:CARGO_INCREMENTAL = $previousCargoIncremental
    $env:CARGO_TARGET_DIR = $previousCargoTargetDir
}

Copy-Item -Path (Join-Path $cmdletBuildDir '*.dll') -Destination $outPath -Force
Copy-Item -Path (Join-Path $cmdletBuildDir '*.deps.json') -Destination $outPath -Force
Copy-Item -Path (Join-Path $cmdletBuildDir '*.pdb') -Destination $outPath -Force -ErrorAction SilentlyContinue

$runtimeSupportAssemblies = @(
    'dotnet\backend\src\Rustlyn.Backend\bin\Release\net10.0\Rustlyn.Backend.dll',
    'dotnet\backend\src\Rustlyn.Runtime\bin\Release\net10.0\Rustlyn.Runtime.dll',
    'dotnet\backend\src\Rustlyn.Interop\bin\Release\net10.0\Rustlyn.Interop.dll',
    'dotnet\backend\src\Rustlyn.Os\bin\Release\net10.0\Rustlyn.Os.dll'
)

foreach ($relativeAssembly in $runtimeSupportAssemblies) {
    $assemblyPath = Join-Path $repoRoot $relativeAssembly
    if (!(Test-Path $assemblyPath)) {
        throw "Required runtime support assembly '$assemblyPath' was not built."
    }

    Copy-Item -LiteralPath $assemblyPath -Destination $outPath -Force
    $pdbPath = [System.IO.Path]::ChangeExtension($assemblyPath, '.pdb')
    if (Test-Path $pdbPath) {
        Copy-Item -LiteralPath $pdbPath -Destination $outPath -Force
    }
}

$manifestPath = Join-Path $outPath $ManifestFileName
New-ModuleManifest `
    -Path $manifestPath `
    -RootModule 'Rustlyn.PowerShellCmdlets.dll' `
    -ModuleVersion '0.1.0' `
    -Guid $ModuleGuid `
    -Author 'Rustlyn' `
    -CompanyName 'Rustlyn' `
    -Copyright '(c) Rustlyn contributors' `
    -Description "Rust-backed PowerShell cmdlets for $Sample." `
    -PowerShellVersion '7.5' `
    -CmdletsToExport $CmdletsToExport `
    -FunctionsToExport @() `
    -AliasesToExport @()

Write-Output $manifestPath
