param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release",

    [Parameter(Mandatory = $false)]
    [string]$RuntimeIdentifier = "win-x64",

    [Parameter(Mandatory = $false)]
    [string]$OutputPath,

    [Parameter(Mandatory = $false)]
    [ValidateSet("Shared", "Static")]
    [string]$NativeAotLinkMode = "Shared",

    [Parameter(Mandatory = $false)]
    [switch]$SkipBuild
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$nativeAotProject = Join-Path $repoRoot "dotnet\backend\src\Rustlyn.NativeAot\Rustlyn.NativeAot.csproj"
$nativeHostManifest = Join-Path $repoRoot "native\rustlyn\Cargo.toml"
$nativeAotPublishDir = Join-Path $repoRoot "dotnet\backend\src\Rustlyn.NativeAot\bin\$Configuration\net10.0\$RuntimeIdentifier\publish"
$nativeHostExeName = if ($RuntimeIdentifier.StartsWith("win-", [System.StringComparison]::OrdinalIgnoreCase)) { "rustlyn.exe" } else { "rustlyn" }
$nativeHostPath = Join-Path $repoRoot "native\rustlyn\target\release\$nativeHostExeName"
$supportAssemblyDir = Join-Path $repoRoot "dotnet\backend\src\Rustlyn.Backend\bin\$Configuration\net10.0"
$stagingRoot = Join-Path $repoRoot "artifacts\scratch\rustlyn-native-distro\$RuntimeIdentifier"

if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path $repoRoot "artifacts\out\rustlyn-$RuntimeIdentifier.zip"
}

if (-not $SkipBuild) {
    if ($NativeAotLinkMode -eq "Static" -and [string]::IsNullOrWhiteSpace($env:RUSTLYN_NATIVEAOT_RUNTIME_LIB_DIR)) {
        throw "RUSTLYN_NATIVEAOT_RUNTIME_LIB_DIR must point at the NativeAOT runtime pack native library directory."
    }
    if ([string]::IsNullOrWhiteSpace($env:RUSTLYN_LLVM_ROOT)) {
        throw "RUSTLYN_LLVM_ROOT must point at the static LLVM build used to link the native host."
    }

    dotnet publish $nativeAotProject -c $Configuration -r $RuntimeIdentifier --self-contained -p:PublishAot=true -p:NativeLib=$NativeAotLinkMode -m:1
    if ($LASTEXITCODE -ne 0) {
        throw "NativeAOT $NativeAotLinkMode publish failed with exit code $LASTEXITCODE."
    }

    $env:RUSTLYN_NATIVEAOT_LIB_DIR = $nativeAotPublishDir
    $env:RUSTLYN_NATIVEAOT_LINK_MODE = $NativeAotLinkMode.ToLowerInvariant()
    cargo build --manifest-path $nativeHostManifest --release
    if ($LASTEXITCODE -ne 0) {
        throw "native rustlyn host build failed with exit code $LASTEXITCODE."
    }
}

if (-not (Test-Path -LiteralPath $nativeHostPath -PathType Leaf)) {
    throw "native rustlyn host was not found: $nativeHostPath"
}

if (Test-Path -LiteralPath $stagingRoot) {
    Remove-Item -Recurse -Force $stagingRoot
}
New-Item -ItemType Directory -Force -Path $stagingRoot | Out-Null

Copy-Item -LiteralPath $nativeHostPath -Destination (Join-Path $stagingRoot $nativeHostExeName)
if ($NativeAotLinkMode -eq "Shared") {
    $nativeAotDllPath = Join-Path (Split-Path -Parent $nativeHostPath) "rustlyn_nativeaot.dll"
    if (-not (Test-Path -LiteralPath $nativeAotDllPath -PathType Leaf)) {
        throw "shared NativeAOT DLL was not found beside the native host: $nativeAotDllPath"
    }
    Copy-Item -LiteralPath $nativeAotDllPath -Destination (Join-Path $stagingRoot "rustlyn_nativeaot.dll")
}

$supportAssemblyNames = @(
    "Rustlyn.Backend.dll",
    "Rustlyn.BindingContracts.dll",
    "Rustlyn.Runtime.dll",
    "Rustlyn.Os.dll",
    "Rustlyn.Interop.dll"
)

foreach ($assemblyName in $supportAssemblyNames) {
    $assemblyPath = Join-Path $supportAssemblyDir $assemblyName
    if (-not (Test-Path -LiteralPath $assemblyPath -PathType Leaf)) {
        throw "required support assembly was not found: $assemblyPath"
    }
    Copy-Item -LiteralPath $assemblyPath -Destination (Join-Path $stagingRoot $assemblyName)
}

foreach ($rootFileName in @("README.md", "LICENSE")) {
    $rootFilePath = Join-Path $repoRoot $rootFileName
    if (Test-Path -LiteralPath $rootFilePath -PathType Leaf) {
        Copy-Item -LiteralPath $rootFilePath -Destination (Join-Path $stagingRoot $rootFileName)
    }
}

$outputDirectory = Split-Path -Parent $OutputPath
if (-not [string]::IsNullOrWhiteSpace($outputDirectory)) {
    New-Item -ItemType Directory -Force -Path $outputDirectory | Out-Null
}
if (Test-Path -LiteralPath $OutputPath) {
    Remove-Item -Force $OutputPath
}

Compress-Archive -Path (Join-Path $stagingRoot "*") -DestinationPath $OutputPath
Write-Host "Rustlyn native distribution: $OutputPath"
