param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$sdkRoot = Join-Path $workspaceRoot "dotnet\backend\src"
$projectPath = Join-Path $workspaceRoot "samples\msbuild_add\msbuild_add.rsproj"
$outputAssembly = Join-Path $workspaceRoot "samples\msbuild_add\bin\$Configuration\net10.0\msbuild_add.dll"
$bitcodePath = Join-Path $workspaceRoot "samples\msbuild_add\obj\$Configuration\net10.0\msbuild_add.bc"
$aliasProjectPath = Join-Path $workspaceRoot "samples\msbuild_sourcegear_aliases\msbuild_sourcegear_aliases.rsproj"
$aliasOutputAssembly = Join-Path $workspaceRoot "samples\msbuild_sourcegear_aliases\bin\$Configuration\net10.0\msbuild_sourcegear_aliases.dll"
$aliasBitcodePath = Join-Path $workspaceRoot "samples\msbuild_sourcegear_aliases\obj\$Configuration\net10.0\msbuild_sourcegear_aliases.bc"
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
        throw "MSBuild SDK sample build failed with exit code $LASTEXITCODE."
    }

    dotnet build $aliasProjectPath -c $Configuration "/p:RustMcilToolDll=$toolDll" /nologo
    if ($LASTEXITCODE -ne 0) {
        throw "MSBuild SDK SourceGear-alias sample build failed with exit code $LASTEXITCODE."
    }
}
finally {
    $env:MSBuildSDKsPath = $previousMsBuildSdksPath
}

if (-not (Test-Path $outputAssembly)) {
    throw "Expected translated assembly was not created: $outputAssembly"
}

if (-not (Test-Path $bitcodePath)) {
    throw "Expected translated bitcode was not created: $bitcodePath"
}

if (-not (Test-Path $aliasOutputAssembly)) {
    throw "Expected SourceGear-alias translated assembly was not created: $aliasOutputAssembly"
}

if (-not (Test-Path $aliasBitcodePath)) {
    throw "Expected SourceGear-alias translated bitcode was not created: $aliasBitcodePath"
}

$invokeOutput = & dotnet $toolDll invoke $bitcodePath --method add_i32 --arg i32:19 --arg i32:23
if ($LASTEXITCODE -ne 0) {
    throw "MSBuild SDK sample invoke failed with exit code $LASTEXITCODE."
}

$result = ($invokeOutput | Select-Object -Last 1).Trim()
if ($result -ne "42") {
    throw "Expected MSBuild SDK sample to invoke add_i32(19, 23) => 42, got '$result'."
}

$aliasInvokeOutput = & dotnet $toolDll invoke $aliasBitcodePath --method profile_probe_score
if ($LASTEXITCODE -ne 0) {
    throw "MSBuild SDK SourceGear-alias sample invoke failed with exit code $LASTEXITCODE."
}

$aliasResult = ($aliasInvokeOutput | Select-Object -Last 1).Trim()
if ($aliasResult -ne "11") {
    throw "Expected SourceGear RustDebugOrRelease=debug alias to invoke profile_probe_score() => 11, got '$aliasResult'."
}

Write-Host "PASS msbuild_add (msbuild sdk) => 42"
Write-Host "PASS msbuild_sourcegear_aliases (sourcegear property aliases) => 11"