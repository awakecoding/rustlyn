param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$sdkRoot = Join-Path $workspaceRoot "dotnet\backend\src"
$projectPath = Join-Path $workspaceRoot "samples\msbuild_build_std_core\msbuild_build_std_core.rsproj"
$outputAssembly = Join-Path $workspaceRoot "samples\msbuild_build_std_core\bin\$Configuration\net10.0\msbuild_build_std_core.dll"
$bitcodePath = Join-Path $workspaceRoot "samples\msbuild_build_std_core\obj\$Configuration\net10.0\msbuild_build_std_core.bc"
$toolProject = Join-Path $workspaceRoot "dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj"
$toolDll = Join-Path $workspaceRoot "dotnet\backend\src\Rustlyn.Tool\bin\$Configuration\net10.0\Rustlyn.Tool.dll"

dotnet build $toolProject -c $Configuration /nologo
if ($LASTEXITCODE -ne 0) {
    throw "Rustlyn.Tool build failed with exit code $LASTEXITCODE."
}

$previousMsBuildSdksPath = $env:MSBuildSDKsPath
try {
    $env:MSBuildSDKsPath = $sdkRoot
    dotnet build $projectPath -c $Configuration "/p:RustlynToolDll=$toolDll" /nologo
    if ($LASTEXITCODE -ne 0) {
        throw "MSBuild SDK build-std sample build failed with exit code $LASTEXITCODE."
    }
}
finally {
    $env:MSBuildSDKsPath = $previousMsBuildSdksPath
}

if (-not (Test-Path $outputAssembly)) {
    throw "Expected build-std translated assembly was not created: $outputAssembly"
}

if (-not (Test-Path $bitcodePath)) {
    throw "Expected build-std translated bitcode was not created: $bitcodePath"
}

$invokeOutput = & dotnet $toolDll invoke $bitcodePath --method add_i32 --arg i32:19 --arg i32:23
if ($LASTEXITCODE -ne 0) {
    throw "MSBuild SDK build-std sample invoke failed with exit code $LASTEXITCODE."
}

$result = ($invokeOutput | Select-Object -Last 1).Trim()
if ($result -ne "42") {
    throw "Expected MSBuild SDK build-std sample to invoke add_i32(19, 23) => 42, got '$result'."
}

Write-Host "PASS msbuild_build_std_core (msbuild sdk build-std) => 42"