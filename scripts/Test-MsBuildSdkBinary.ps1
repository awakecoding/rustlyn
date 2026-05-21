param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$sdkRoot = Join-Path $workspaceRoot "dotnet\backend\src"
$projectPath = Join-Path $workspaceRoot "samples\msbuild_bin_trivial\msbuild_bin_trivial.rsproj"
$outputAssembly = Join-Path $workspaceRoot "samples\msbuild_bin_trivial\bin\$Configuration\net10.0\msbuild_bin_trivial.dll"
$runtimeConfigPath = Join-Path $workspaceRoot "samples\msbuild_bin_trivial\bin\$Configuration\net10.0\msbuild_bin_trivial.runtimeconfig.json"
$bitcodePath = Join-Path $workspaceRoot "samples\msbuild_bin_trivial\obj\$Configuration\net10.0\msbuild_bin_trivial.bc"
$toolProject = Join-Path $workspaceRoot "dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj"
$toolDll = Join-Path $workspaceRoot "dotnet\backend\src\RustMcil.Tool\bin\$Configuration\net10.0\RustMcil.Tool.dll"

dotnet build $toolProject -c $Configuration /nologo
if ($LASTEXITCODE -ne 0) {
    throw "RustMcil.Tool build failed with exit code $LASTEXITCODE."
}

$previousMsBuildSdksPath = $env:MSBuildSDKsPath
try {
    $env:MSBuildSDKsPath = $sdkRoot
    dotnet build $projectPath -c $Configuration "/p:RustMcilToolDll=$toolDll" /nologo
    if ($LASTEXITCODE -ne 0) {
        throw "MSBuild SDK binary sample build failed with exit code $LASTEXITCODE."
    }
}
finally {
    $env:MSBuildSDKsPath = $previousMsBuildSdksPath
}

if (-not (Test-Path $outputAssembly)) {
    throw "Expected translated binary assembly was not created: $outputAssembly"
}

if (-not (Test-Path $runtimeConfigPath)) {
    throw "Expected translated binary runtimeconfig was not created: $runtimeConfigPath"
}

if (-not (Test-Path $bitcodePath)) {
    throw "Expected translated binary bitcode was not created: $bitcodePath"
}

$consoleOutput = & dotnet $outputAssembly 2>&1
if ($LASTEXITCODE -ne 0) {
    throw "Translated MSBuild SDK binary sample failed with exit code $LASTEXITCODE.`n$($consoleOutput | Out-String)"
}

$actualOutput = (($consoleOutput | ForEach-Object { $_.ToString() }) | Where-Object { $_.Trim().Length -gt 0 }) -join [Environment]::NewLine
if ($actualOutput -ne "") {
    throw "Expected translated MSBuild SDK binary sample to produce no stdout/stderr, got '$actualOutput'."
}

$previousMsBuildSdksPath = $env:MSBuildSDKsPath
try {
    $env:MSBuildSDKsPath = $sdkRoot
    dotnet clean $projectPath -c $Configuration "/p:RustMcilToolDll=$toolDll" /nologo | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "MSBuild SDK binary sample clean failed with exit code $LASTEXITCODE."
    }
}
finally {
    $env:MSBuildSDKsPath = $previousMsBuildSdksPath
}

foreach ($outputPath in @($outputAssembly, $runtimeConfigPath, $bitcodePath)) {
    if (Test-Path $outputPath) {
        throw "Expected clean to remove translated binary output: $outputPath"
    }
}

Write-Host "PASS msbuild_bin_trivial (msbuild sdk bin) => exit 0"