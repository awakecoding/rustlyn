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
$generatedProjectPath = Join-Path $workspaceRoot "samples\msbuild_generated_cargo\msbuild_generated_cargo.rsproj"
$generatedOutputAssembly = Join-Path $workspaceRoot "samples\msbuild_generated_cargo\bin\$Configuration\net10.0\msbuild_generated_cargo.dll"
$generatedBitcodePath = Join-Path $workspaceRoot "samples\msbuild_generated_cargo\obj\$Configuration\net10.0\msbuild_generated_cargo.bc"
$generatedCargoManifestPath = Join-Path $workspaceRoot "samples\msbuild_generated_cargo\obj\$Configuration\net10.0\rs\Cargo.toml"
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

    dotnet build $generatedProjectPath -c $Configuration "/p:RustMcilToolDll=$toolDll" /nologo
    if ($LASTEXITCODE -ne 0) {
        throw "MSBuild SDK generated Cargo sample build failed with exit code $LASTEXITCODE."
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

if (-not (Test-Path $generatedOutputAssembly)) {
    throw "Expected generated-Cargo translated assembly was not created: $generatedOutputAssembly"
}

if (-not (Test-Path $generatedBitcodePath)) {
    throw "Expected generated-Cargo translated bitcode was not created: $generatedBitcodePath"
}

if (-not (Test-Path $generatedCargoManifestPath)) {
    throw "Expected generated Cargo manifest was not created: $generatedCargoManifestPath"
}

$generatedCargoManifest = Get-Content $generatedCargoManifestPath -Raw
if ($generatedCargoManifest -notmatch "profile_probe = \{ path = '.+profile_probe' \}") {
    throw "Expected generated Cargo manifest to contain a profile_probe RustReference dependency."
}

if ($generatedCargoManifest -notmatch "cfg-if = \{ version = '1\.0\.0', default-features = false \}") {
    throw "Expected generated Cargo manifest to contain a cfg-if RustCrateReference dependency."
}

if ($generatedCargoManifest -notmatch "bitflags = \{ version = '2\.9\.4', default-features = false, features = \['std','serde'\] \}") {
    throw "Expected generated Cargo manifest to contain a bitflags RustCrateReference dependency."
}

if ($generatedCargoManifest -notmatch 'default-features = false') {
    throw "Expected generated Cargo manifest to preserve RustCrateReference DefaultFeatures metadata."
}

if ($generatedCargoManifest -notmatch "features = \['std','serde'\]") {
    throw "Expected generated Cargo manifest to preserve RustCrateReference Features metadata."
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

$generatedInvokeOutput = & dotnet $toolDll invoke $generatedBitcodePath --method generated_cargo_score
if ($LASTEXITCODE -ne 0) {
    throw "MSBuild SDK generated Cargo sample invoke failed with exit code $LASTEXITCODE."
}

$generatedResult = ($generatedInvokeOutput | Select-Object -Last 1).Trim()
if ($generatedResult -ne "83") {
    throw "Expected generated Cargo sample to invoke generated_cargo_score() => 83, got '$generatedResult'."
}

Write-Host "PASS msbuild_add (msbuild sdk) => 42"
Write-Host "PASS msbuild_sourcegear_aliases (sourcegear property aliases) => 11"
Write-Host "PASS msbuild_generated_cargo (generated Cargo manifest + RustReference/RustCrateReference features) => 83"
