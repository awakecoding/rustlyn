param(
    [Parameter(Mandatory = $false)]
    [string[]]$Sample = @("add"),

    [Parameter(Mandatory = $false)]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release",

    [Parameter(Mandatory = $false)]
    [string]$LlvmRoot = $env:RUSTLYN_LLVM_ROOT,

    [Parameter(Mandatory = $false)]
    [string]$LegacyToolDll,

    [Parameter(Mandatory = $false)]
    [string]$NativeHostPath,

    [Parameter(Mandatory = $false)]
    [string]$ScratchRoot,

    [Parameter(Mandatory = $false)]
    [switch]$SkipBuild,

    [Parameter(Mandatory = $false)]
    [switch]$SkipSdkPackage
)

$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
. (Join-Path $PSScriptRoot "Rustlyn.Cli.ps1")

if ([string]::IsNullOrWhiteSpace($LlvmRoot)) {
    throw "LlvmRoot is required for the legacy managed CLI. Pass -LlvmRoot or set RUSTLYN_LLVM_ROOT."
}

if ([string]::IsNullOrWhiteSpace($LegacyToolDll)) {
    $LegacyToolDll = Join-Path $workspaceRoot "dotnet\backend\src\Rustlyn.Tool\bin\$Configuration\net10.0\rustlyn.dll"
}

if ([string]::IsNullOrWhiteSpace($NativeHostPath)) {
    $nativeHostName = if ($IsWindows -or $env:OS -eq "Windows_NT") { "rustlyn.exe" } else { "rustlyn" }
    $NativeHostPath = Join-Path $workspaceRoot "native\rustlyn\target\release\$nativeHostName"
}

if ([string]::IsNullOrWhiteSpace($ScratchRoot)) {
    $ScratchRoot = Join-Path $workspaceRoot "artifacts\scratch\native-host-parity"
}

$toolProject = Join-Path $workspaceRoot "dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj"
$nativeManifest = Join-Path $workspaceRoot "native\rustlyn\Cargo.toml"

if (-not $SkipBuild) {
    dotnet build $toolProject -c $Configuration /nologo
    if ($LASTEXITCODE -ne 0) {
        throw "legacy rustlyn build failed with exit code $LASTEXITCODE."
    }

    if (-not (Test-Path -LiteralPath $NativeHostPath -PathType Leaf)) {
        if ([string]::IsNullOrWhiteSpace($env:RUSTLYN_NATIVEAOT_LIB_DIR)) {
            throw "Native host is missing and RUSTLYN_NATIVEAOT_LIB_DIR is not set; publish Rustlyn.NativeAot first or pass -SkipBuild with an existing host."
        }

        cargo build --manifest-path $nativeManifest --release
        if ($LASTEXITCODE -ne 0) {
            throw "native rustlyn build failed with exit code $LASTEXITCODE."
        }
    }
}

$legacyRustlyn = Resolve-RustlynCli -RepoRoot $workspaceRoot -Configuration $Configuration -ToolDll $LegacyToolDll
$nativeRustlyn = Resolve-RustlynCli -RepoRoot $workspaceRoot -Configuration $Configuration -ToolPath $NativeHostPath

if (Test-Path -LiteralPath $ScratchRoot) {
    Remove-Item -Recurse -Force $ScratchRoot
}
New-Item -ItemType Directory -Force -Path $ScratchRoot | Out-Null

function Invoke-CapturedRustlyn {
    param(
        [Parameter(Mandatory = $true)]
        [pscustomobject]$Command,

        [Parameter(Mandatory = $true)]
        [string[]]$Arguments,

        [Parameter(Mandatory = $false)]
        [int[]]$ExpectedExitCode = @(0)
    )

    $toolArguments = @()
    foreach ($argument in $Command.PrefixArguments) {
        $toolArguments += $argument
    }
    foreach ($argument in $Arguments) {
        $toolArguments += $argument
    }

    $output = & $Command.FileName @toolArguments 2>&1
    $exitCode = if ($null -eq $LASTEXITCODE) { 0 } else { $LASTEXITCODE }
    $text = ($output | ForEach-Object { $_.ToString() }) -join [Environment]::NewLine
    if ($ExpectedExitCode -notcontains $exitCode) {
        throw "Command '$($Command.DisplayName) $($Arguments -join ' ')' exited with $exitCode. Output:$([Environment]::NewLine)$text"
    }

    [pscustomobject]@{
        ExitCode = $exitCode
        Text = $text
        Lines = @($output | ForEach-Object { $_.ToString() })
    }
}

function Assert-Contains {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Text,

        [Parameter(Mandatory = $true)]
        [string]$Expected,

        [Parameter(Mandatory = $true)]
        [string]$Context
    )

    if ($Text.IndexOf($Expected, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        throw "$Context did not contain '$Expected'. Output:$([Environment]::NewLine)$Text"
    }
}

function Assert-ContainsAny {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Text,

        [Parameter(Mandatory = $true)]
        [string[]]$Expected,

        [Parameter(Mandatory = $true)]
        [string]$Context
    )

    foreach ($value in $Expected) {
        if ($Text.IndexOf($value, [System.StringComparison]::OrdinalIgnoreCase) -ge 0) {
            return
        }
    }

    throw "$Context did not contain any expected text ($($Expected -join ', ')). Output:$([Environment]::NewLine)$Text"
}

function Assert-FileCreated {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path,

        [Parameter(Mandatory = $true)]
        [string]$Context
    )

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "$Context did not create expected file: $Path"
    }

    $length = (Get-Item -LiteralPath $Path).Length
    if ($length -le 0) {
        throw "$Context created an empty file: $Path"
    }
}

function Get-PackageId {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Text,

        [Parameter(Mandatory = $true)]
        [string]$Context
    )

    $match = [regex]::Match($Text, '(?m)^Package ID:\s*(?<id>\S+)\s*$')
    if (-not $match.Success) {
        throw "$Context did not report a package ID. Output:$([Environment]::NewLine)$Text"
    }

    $match.Groups['id'].Value
}

function Get-InspectValue {
    param(
        [Parameter(Mandatory = $true)]
        [pscustomobject]$Result,

        [Parameter(Mandatory = $true)]
        [string]$Key
    )

    $prefix = "$Key`: "
    foreach ($line in $Result.Lines) {
        if ($line.StartsWith($prefix, [System.StringComparison]::OrdinalIgnoreCase)) {
            return $line.Substring($prefix.Length).Trim()
        }
    }

    throw "inspect output did not include '$Key'. Output:$([Environment]::NewLine)$($Result.Text)"
}

function Normalize-Text {
    param([Parameter(Mandatory = $true)][string]$Text)
    return ($Text -replace "`r`n", "`n").Trim()
}

function Measure-VersionCommand {
    param([Parameter(Mandatory = $true)][pscustomobject]$Command)

    $watch = [System.Diagnostics.Stopwatch]::StartNew()
    $result = Invoke-CapturedRustlyn -Command $Command -Arguments @("--version")
    $watch.Stop()

    [pscustomobject]@{
        Text = $result.Text.Trim()
        ElapsedMilliseconds = $watch.ElapsedMilliseconds
    }
}

function Test-SdkPackageUsesNativeHost {
    param(
        [Parameter(Mandatory = $true)]
        [string]$CratePath
    )

    $packageSource = Join-Path $ScratchRoot "packages"
    $sdkProject = Join-Path $workspaceRoot "dotnet\backend\src\Rustlyn.Sdk\Rustlyn.Sdk.csproj"
    New-Item -ItemType Directory -Force -Path $packageSource | Out-Null

    dotnet pack $sdkProject -c $Configuration -o $packageSource /nologo -m:1 | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw "Rustlyn.Sdk pack failed with exit code $LASTEXITCODE."
    }

    $packagePath = Join-Path $packageSource "Rustlyn.Sdk.0.1.0-local.nupkg"
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $package = [System.IO.Compression.ZipFile]::OpenRead((Resolve-Path -LiteralPath $packagePath).ProviderPath)
    try {
        foreach ($entryName in @(
            "tools/win-x64/rustlyn.exe",
            "tools/win-x64/Rustlyn.Backend.dll",
            "tools/win-x64/Rustlyn.Runtime.dll",
            "tools/win-x64/Rustlyn.Os.dll",
            "tools/win-x64/Rustlyn.Interop.dll",
            "tools/win-x64/Rustlyn.BindingContracts.dll"
        )) {
            $entry = $package.Entries | Where-Object { $_.FullName -eq $entryName } | Select-Object -First 1
            if ($null -eq $entry) {
                throw "packaged SDK is missing $entryName"
            }
        }
    }
    finally {
        $package.Dispose()
    }

    $sdkScratch = Join-Path $ScratchRoot "sdk-native"
    $nugetPackages = Join-Path $sdkScratch "nuget-packages"
    New-Item -ItemType Directory -Force -Path $sdkScratch, $nugetPackages | Out-Null

    $projectPath = Join-Path $sdkScratch "native_host_sdk.rsproj"
    $packageSourceUri = [System.Uri]::new($packageSource + [System.IO.Path]::DirectorySeparatorChar).AbsoluteUri
    @"
<Project Sdk="Rustlyn.Sdk/0.1.0-local">
  <PropertyGroup>
    <AssemblyName>native_host_sdk</AssemblyName>
    <RustlynCratePath>$CratePath</RustlynCratePath>
  </PropertyGroup>
</Project>
"@ | Set-Content -Path $projectPath -Encoding UTF8

    @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="rustlyn-scratch" value="$packageSourceUri" />
  </packageSources>
</configuration>
"@ | Set-Content -Path (Join-Path $sdkScratch "NuGet.config") -Encoding UTF8

    Push-Location $sdkScratch
    $previousNuGetPackages = $env:NUGET_PACKAGES
    try {
        $env:NUGET_PACKAGES = $nugetPackages
        dotnet build $projectPath -c $Configuration /nologo
        if ($LASTEXITCODE -ne 0) {
            throw "packaged SDK native-host sample build failed with exit code $LASTEXITCODE."
        }
    }
    finally {
        $env:NUGET_PACKAGES = $previousNuGetPackages
        Pop-Location
    }

    Assert-FileCreated (Join-Path $sdkScratch "bin\$Configuration\net10.0\native_host_sdk.dll") "packaged SDK native-host build"
}

$legacyVersion = Measure-VersionCommand -Command $legacyRustlyn
$nativeVersion = Measure-VersionCommand -Command $nativeRustlyn
$nativeHostSize = (Get-Item -LiteralPath (Resolve-Path -LiteralPath $NativeHostPath).ProviderPath).Length

Write-Host "Legacy version: $($legacyVersion.Text) ($($legacyVersion.ElapsedMilliseconds) ms)"
Write-Host "Native version: $($nativeVersion.Text) ($($nativeVersion.ElapsedMilliseconds) ms, $nativeHostSize bytes)"

$firstCratePath = $null
foreach ($sampleName in $Sample) {
    $samplePath = Join-Path $workspaceRoot "samples\$sampleName"
    if (-not (Test-Path -LiteralPath $samplePath -PathType Container)) {
        throw "sample not found: $samplePath"
    }
    if ($null -eq $firstCratePath) {
        $firstCratePath = $samplePath
    }

    & (Join-Path $PSScriptRoot "Build-SampleBitcode.ps1") -Sample $sampleName | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw "sample bitcode build failed for '$sampleName' with exit code $LASTEXITCODE."
    }

    $bitcodePath = Join-Path $workspaceRoot "artifacts\out\$sampleName\$sampleName.bc"
    Assert-FileCreated $bitcodePath "sample bitcode build"

    Write-Host "Comparing inspect/lower/emit for $sampleName"
    $legacyInspect = Invoke-CapturedRustlyn -Command $legacyRustlyn -Arguments @("inspect", $bitcodePath, "--llvm-root", $LlvmRoot)
    $nativeInspect = Invoke-CapturedRustlyn -Command $nativeRustlyn -Arguments @("inspect", $bitcodePath)
    foreach ($key in @("FunctionCount", "AliasCount", "GlobalCount", "BasicBlockCount", "InstructionCount")) {
        $legacyValue = Get-InspectValue $legacyInspect $key
        $nativeValue = Get-InspectValue $nativeInspect $key
        if ($legacyValue -ne $nativeValue) {
            throw "inspect mismatch for $sampleName/$key`: legacy=$legacyValue native=$nativeValue"
        }
    }
    Assert-Contains $nativeInspect.Text "linked-in-process" "native inspect"

    $legacyLower = Invoke-CapturedRustlyn -Command $legacyRustlyn -Arguments @("lower", $bitcodePath, "--llvm-root", $LlvmRoot)
    $nativeLower = Invoke-CapturedRustlyn -Command $nativeRustlyn -Arguments @("lower", $bitcodePath)
    if ((Normalize-Text $legacyLower.Text) -ne (Normalize-Text $nativeLower.Text)) {
        throw "lower output mismatch for $sampleName."
    }

    $sampleScratch = Join-Path $ScratchRoot $sampleName
    New-Item -ItemType Directory -Force -Path $sampleScratch | Out-Null

    $legacyEmitPath = Join-Path $sampleScratch "$sampleName.legacy.emit.dll"
    $nativeEmitPath = Join-Path $sampleScratch "$sampleName.native.emit.dll"
    Invoke-CapturedRustlyn -Command $legacyRustlyn -Arguments @("emit", $bitcodePath, "--out", $legacyEmitPath, "--pdb", "--llvm-root", $LlvmRoot) | Out-Null
    Invoke-CapturedRustlyn -Command $nativeRustlyn -Arguments @("emit", $bitcodePath, "--out", $nativeEmitPath, "--pdb") | Out-Null
    Assert-FileCreated $legacyEmitPath "legacy emit"
    Assert-FileCreated $nativeEmitPath "native emit"
    Assert-FileCreated ([System.IO.Path]::ChangeExtension($legacyEmitPath, ".pdb")) "legacy emit"
    Assert-FileCreated ([System.IO.Path]::ChangeExtension($nativeEmitPath, ".pdb")) "native emit"

    $legacyTranslatePath = Join-Path $sampleScratch "$sampleName.legacy.translate.dll"
    $nativeTranslatePath = Join-Path $sampleScratch "$sampleName.native.translate.dll"
    $legacyTranslateBc = Join-Path $sampleScratch "$sampleName.legacy.translate.bc"
    $nativeTranslateBc = Join-Path $sampleScratch "$sampleName.native.translate.bc"
    $legacyTranslate = Invoke-CapturedRustlyn -Command $legacyRustlyn -Arguments @("translate", $samplePath, "--out", $legacyTranslatePath, "--bitcode-out", $legacyTranslateBc, "--llvm-root", $LlvmRoot)
    $nativeTranslate = Invoke-CapturedRustlyn -Command $nativeRustlyn -Arguments @("translate", $samplePath, "--out", $nativeTranslatePath, "--bitcode-out", $nativeTranslateBc)
    Assert-Contains $legacyTranslate.Text "Bitcode:" "legacy translate"
    Assert-Contains $nativeTranslate.Text "Bitcode:" "native translate"
    Assert-FileCreated $legacyTranslatePath "legacy translate"
    Assert-FileCreated $nativeTranslatePath "native translate"
    Assert-FileCreated $legacyTranslateBc "legacy translate"
    Assert-FileCreated $nativeTranslateBc "native translate"

    $legacyCargo = Invoke-CapturedRustlyn -Command $legacyRustlyn -Arguments @("cargo", "build", "--manifest-path", (Join-Path $samplePath "Cargo.toml"), "--llvm-root", $LlvmRoot)
    $nativeCargo = Invoke-CapturedRustlyn -Command $nativeRustlyn -Arguments @("cargo", "build", "--manifest-path", (Join-Path $samplePath "Cargo.toml"))
    Assert-Contains $legacyCargo.Text "Assembly:" "legacy cargo build"
    Assert-Contains $nativeCargo.Text "Assembly:" "native cargo build"

    $legacyPackDir = Join-Path $sampleScratch "legacy-pack"
    $nativePackDir = Join-Path $sampleScratch "native-pack"
    $legacyPack = Invoke-CapturedRustlyn -Command $legacyRustlyn -Arguments @("pack", $samplePath, "--out", $legacyPackDir, "--version", "0.1.0-parity", "--llvm-root", $LlvmRoot)
    $nativePack = Invoke-CapturedRustlyn -Command $nativeRustlyn -Arguments @("pack", $samplePath, "--out", $nativePackDir, "--version", "0.1.0-parity")
    Assert-Contains $legacyPack.Text "Package ID:" "legacy pack"
    Assert-Contains $nativePack.Text "Package ID:" "native pack"
    $legacyPackageId = Get-PackageId $legacyPack.Text "legacy pack"
    $nativePackageId = Get-PackageId $nativePack.Text "native pack"
    if (-not [string]::Equals($legacyPackageId, $nativePackageId, [System.StringComparison]::Ordinal)) {
        throw "pack package ID mismatch for ${sampleName}: legacy=$legacyPackageId native=$nativePackageId"
    }
    Assert-FileCreated (Join-Path $legacyPackDir "$legacyPackageId.0.1.0-parity.nupkg") "legacy pack"
    Assert-FileCreated (Join-Path $nativePackDir "$nativePackageId.0.1.0-parity.nupkg") "native pack"
}

$missingPath = Join-Path $ScratchRoot "missing.bc"
$legacyMissing = Invoke-CapturedRustlyn -Command $legacyRustlyn -Arguments @("emit", $missingPath, "--out", (Join-Path $ScratchRoot "missing.legacy.dll"), "--llvm-root", $LlvmRoot) -ExpectedExitCode @(1)
$nativeMissing = Invoke-CapturedRustlyn -Command $nativeRustlyn -Arguments @("emit", $missingPath, "--out", (Join-Path $ScratchRoot "missing.native.dll")) -ExpectedExitCode @(2)
Assert-ContainsAny $legacyMissing.Text @("not found", "Configured LLVM toolchain", "missing") "legacy missing-artifact diagnostic"
Assert-Contains $nativeMissing.Text "missing-artifact" "native missing-artifact diagnostic"

if (-not $SkipSdkPackage -and $null -ne $firstCratePath) {
    Test-SdkPackageUsesNativeHost $firstCratePath
}

Write-Host "Native host parity validation passed."
$global:LASTEXITCODE = 0
