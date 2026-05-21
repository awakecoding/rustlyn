param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$sdkProject = Join-Path $workspaceRoot "dotnet\backend\src\RustMcil.Sdk\RustMcil.Sdk.csproj"
$toolProject = Join-Path $workspaceRoot "dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj"
$toolDll = Join-Path $workspaceRoot "dotnet\backend\src\RustMcil.Tool\bin\$Configuration\net10.0\RustMcil.Tool.dll"
$packageSource = Join-Path $workspaceRoot "artifacts\scratch\packages"
$scratchProjectRoot = Join-Path $workspaceRoot "artifacts\scratch\msbuild-sdk-package"
$projectPath = Join-Path $scratchProjectRoot "msbuild_add_packaged.rsproj"
$nugetConfigPath = Join-Path $scratchProjectRoot "NuGet.config"
$outputAssembly = Join-Path $scratchProjectRoot "bin\$Configuration\net10.0\msbuild_add_packaged.dll"
$bitcodePath = Join-Path $scratchProjectRoot "obj\$Configuration\net10.0\msbuild_add_packaged.bc"
$addCratePath = Join-Path $workspaceRoot "samples\add"

New-Item -ItemType Directory -Force -Path $packageSource, $scratchProjectRoot | Out-Null

dotnet build $toolProject -c $Configuration /nologo
if ($LASTEXITCODE -ne 0) {
    throw "RustMcil.Tool build failed with exit code $LASTEXITCODE."
}

dotnet pack $sdkProject -c $Configuration -o $packageSource /nologo
if ($LASTEXITCODE -ne 0) {
    throw "RustMcil.Sdk pack failed with exit code $LASTEXITCODE."
}

$projectXml = @"
<Project Sdk="RustMcil.Sdk/0.1.0-local">
  <PropertyGroup>
    <AssemblyName>msbuild_add_packaged</AssemblyName>
    <RustMcilCratePath>$addCratePath</RustMcilCratePath>
  </PropertyGroup>
</Project>
"@
Set-Content -Path $projectPath -Value $projectXml -Encoding UTF8

$packageSourceUri = [System.Uri]::new($packageSource + [System.IO.Path]::DirectorySeparatorChar).AbsoluteUri
$nugetConfigXml = @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="rust-msil-scratch" value="$packageSourceUri" />
  </packageSources>
</configuration>
"@
Set-Content -Path $nugetConfigPath -Value $nugetConfigXml -Encoding UTF8

Push-Location $scratchProjectRoot
try {
    dotnet build $projectPath -c $Configuration "/p:RustMcilToolDll=$toolDll" /nologo
    if ($LASTEXITCODE -ne 0) {
        throw "Packaged MSBuild SDK sample build failed with exit code $LASTEXITCODE."
    }
}
finally {
    Pop-Location
}

if (-not (Test-Path $outputAssembly)) {
    throw "Expected packaged-SDK translated assembly was not created: $outputAssembly"
}

if (-not (Test-Path $bitcodePath)) {
    throw "Expected packaged-SDK translated bitcode was not created: $bitcodePath"
}

$invokeOutput = & dotnet $toolDll invoke $bitcodePath --method add_i32 --arg i32:19 --arg i32:23
if ($LASTEXITCODE -ne 0) {
    throw "Packaged MSBuild SDK sample invoke failed with exit code $LASTEXITCODE."
}

$result = ($invokeOutput | Select-Object -Last 1).Trim()
if ($result -ne "42") {
    throw "Expected packaged MSBuild SDK sample to invoke add_i32(19, 23) => 42, got '$result'."
}

Write-Host "PASS msbuild_add_packaged (nuget sdk) => 42"