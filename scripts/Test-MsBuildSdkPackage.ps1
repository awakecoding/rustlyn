param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$sdkProject = Join-Path $workspaceRoot "dotnet\backend\src\Rustlyn.Sdk\Rustlyn.Sdk.csproj"
$toolProject = Join-Path $workspaceRoot "dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj"
$toolDll = Join-Path $workspaceRoot "dotnet\backend\src\Rustlyn.Tool\bin\$Configuration\net10.0\Rustlyn.Tool.dll"
$packageSource = Join-Path $workspaceRoot "artifacts\scratch\packages"
$packagePath = Join-Path $packageSource "Rustlyn.Sdk.0.1.0-local.nupkg"
$scratchProjectRoot = Join-Path $workspaceRoot "artifacts\scratch\msbuild-sdk-package"
$nugetPackages = Join-Path $scratchProjectRoot "nuget-packages"
$projectPath = Join-Path $scratchProjectRoot "msbuild_add_packaged.rsproj"
$binaryProjectPath = Join-Path $scratchProjectRoot "msbuild_bin_trivial_packaged.rsproj"
$workloadProjectPath = Join-Path $scratchProjectRoot "msbuild_generated_bindings_lousygrep_packaged.rsproj"
$nugetConfigPath = Join-Path $scratchProjectRoot "NuGet.config"
$outputAssembly = Join-Path $scratchProjectRoot "bin\$Configuration\net10.0\msbuild_add_packaged.dll"
$bitcodePath = Join-Path $scratchProjectRoot "obj\$Configuration\net10.0\msbuild_add_packaged.bc"
$binaryOutputAssembly = Join-Path $scratchProjectRoot "bin\$Configuration\net10.0\bin_trivial.dll"
$binaryRuntimeConfigPath = Join-Path $scratchProjectRoot "bin\$Configuration\net10.0\bin_trivial.runtimeconfig.json"
$binaryBitcodePath = Join-Path $scratchProjectRoot "obj\$Configuration\net10.0\bin_trivial.bc"
$workloadOutputAssembly = Join-Path $scratchProjectRoot "bin\$Configuration\net10.0\generated_bindings_lousygrep.dll"
$workloadRuntimeConfigPath = Join-Path $scratchProjectRoot "bin\$Configuration\net10.0\generated_bindings_lousygrep.runtimeconfig.json"
$workloadBitcodePath = Join-Path $scratchProjectRoot "obj\$Configuration\net10.0\generated_bindings_lousygrep.bc"
$addCratePath = Join-Path $workspaceRoot "samples\add"
$binTrivialCratePath = Join-Path $workspaceRoot "samples\bin_trivial"
$workloadCratePath = Join-Path $workspaceRoot "samples\generated_bindings_lousygrep"
$workloadFixtureDirectory = Join-Path $workspaceRoot "samples\generated_bindings_lousygrep\fixtures"
$supportAssemblyNames = @("Rustlyn.Backend.dll", "Rustlyn.Runtime.dll", "Rustlyn.Os.dll", "Rustlyn.Interop.dll")

if (Test-Path $scratchProjectRoot) {
    Remove-Item -Recurse -Force $scratchProjectRoot
}

if (Test-Path $packagePath) {
    Remove-Item -Force $packagePath
}

New-Item -ItemType Directory -Force -Path $packageSource, $scratchProjectRoot, $nugetPackages | Out-Null

dotnet build $toolProject -c $Configuration /nologo
if ($LASTEXITCODE -ne 0) {
    throw "Rustlyn.Tool build failed with exit code $LASTEXITCODE."
}

dotnet pack $sdkProject -c $Configuration -o $packageSource /nologo
if ($LASTEXITCODE -ne 0) {
    throw "Rustlyn.Sdk pack failed with exit code $LASTEXITCODE."
}

Add-Type -AssemblyName System.IO.Compression.FileSystem
$package = [System.IO.Compression.ZipFile]::OpenRead($packagePath)
try {
    $toolEntry = $package.Entries | Where-Object { $_.FullName -eq "tools/net10.0/Rustlyn.Tool.dll" } | Select-Object -First 1
    if ($null -eq $toolEntry) {
        throw "Expected packaged SDK to contain tools/net10.0/Rustlyn.Tool.dll."
    }
}
finally {
    $package.Dispose()
}

$projectXml = @"
<Project Sdk="Rustlyn.Sdk/0.1.0-local">
  <PropertyGroup>
    <AssemblyName>msbuild_add_packaged</AssemblyName>
    <RustlynCratePath>$addCratePath</RustlynCratePath>
  </PropertyGroup>
</Project>
"@
Set-Content -Path $projectPath -Value $projectXml -Encoding UTF8

$binaryProjectXml = @"
<Project Sdk="Rustlyn.Sdk/0.1.0-local">
    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <AssemblyName>bin_trivial</AssemblyName>
        <RustlynCratePath>$binTrivialCratePath</RustlynCratePath>
    </PropertyGroup>
</Project>
"@
Set-Content -Path $binaryProjectPath -Value $binaryProjectXml -Encoding UTF8

$workloadProjectXml = @"
<Project Sdk="Rustlyn.Sdk/0.1.0-local">
    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <AssemblyName>generated_bindings_lousygrep</AssemblyName>
        <RustlynCratePath>$workloadCratePath</RustlynCratePath>
    </PropertyGroup>
</Project>
"@
Set-Content -Path $workloadProjectPath -Value $workloadProjectXml -Encoding UTF8

$packageSourceUri = [System.Uri]::new($packageSource + [System.IO.Path]::DirectorySeparatorChar).AbsoluteUri
$nugetConfigXml = @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="rustlyn-scratch" value="$packageSourceUri" />
  </packageSources>
</configuration>
"@
Set-Content -Path $nugetConfigPath -Value $nugetConfigXml -Encoding UTF8

Push-Location $scratchProjectRoot
$previousNuGetPackages = $env:NUGET_PACKAGES
try {
    $env:NUGET_PACKAGES = $nugetPackages
    dotnet build $projectPath -c $Configuration /nologo
    if ($LASTEXITCODE -ne 0) {
        throw "Packaged MSBuild SDK sample build failed with exit code $LASTEXITCODE."
    }

    dotnet build $binaryProjectPath -c $Configuration /nologo
    if ($LASTEXITCODE -ne 0) {
        throw "Packaged MSBuild SDK binary sample build failed with exit code $LASTEXITCODE."
    }

    dotnet build $workloadProjectPath -c $Configuration /nologo
    if ($LASTEXITCODE -ne 0) {
        throw "Packaged MSBuild SDK generated-bindings workload build failed with exit code $LASTEXITCODE."
    }
}
finally {
    $env:NUGET_PACKAGES = $previousNuGetPackages
    Pop-Location
}

if (-not (Test-Path $outputAssembly)) {
    throw "Expected packaged-SDK translated assembly was not created: $outputAssembly"
}

if (-not (Test-Path $bitcodePath)) {
    throw "Expected packaged-SDK translated bitcode was not created: $bitcodePath"
}

if (-not (Test-Path $binaryOutputAssembly)) {
    throw "Expected packaged-SDK translated binary assembly was not created: $binaryOutputAssembly"
}

if (-not (Test-Path $binaryRuntimeConfigPath)) {
    throw "Expected packaged-SDK translated binary runtimeconfig was not created: $binaryRuntimeConfigPath"
}

if (-not (Test-Path $binaryBitcodePath)) {
    throw "Expected packaged-SDK translated binary bitcode was not created: $binaryBitcodePath"
}

if (-not (Test-Path $workloadOutputAssembly)) {
    throw "Expected packaged-SDK generated-bindings workload assembly was not created: $workloadOutputAssembly"
}

if (-not (Test-Path $workloadRuntimeConfigPath)) {
    throw "Expected packaged-SDK generated-bindings workload runtimeconfig was not created: $workloadRuntimeConfigPath"
}

if (-not (Test-Path $workloadBitcodePath)) {
    throw "Expected packaged-SDK generated-bindings workload bitcode was not created: $workloadBitcodePath"
}

$workloadOutputDirectory = Split-Path -Parent $workloadOutputAssembly
foreach ($supportAssemblyName in $supportAssemblyNames) {
    $supportAssemblyPath = Join-Path $workloadOutputDirectory $supportAssemblyName
    if (-not (Test-Path $supportAssemblyPath)) {
        throw "Expected packaged-SDK workload output to include copied support assembly: $supportAssemblyPath"
    }
}

$invokeOutput = & dotnet $toolDll invoke $bitcodePath --method add_i32 --arg i32:19 --arg i32:23
if ($LASTEXITCODE -ne 0) {
    throw "Packaged MSBuild SDK sample invoke failed with exit code $LASTEXITCODE."
}

$result = ($invokeOutput | Select-Object -Last 1).Trim()
if ($result -ne "42") {
    throw "Expected packaged MSBuild SDK sample to invoke add_i32(19, 23) => 42, got '$result'."
}

$binaryOutput = & dotnet $binaryOutputAssembly 2>&1
if ($LASTEXITCODE -ne 0) {
    throw "Packaged MSBuild SDK binary sample failed with exit code $LASTEXITCODE.`n$($binaryOutput | Out-String)"
}

$actualBinaryOutput = (($binaryOutput | ForEach-Object { $_.ToString() }) | Where-Object { $_.Trim().Length -gt 0 }) -join [Environment]::NewLine
if ($actualBinaryOutput -ne "") {
    throw "Expected packaged MSBuild SDK binary sample to produce no stdout/stderr, got '$actualBinaryOutput'."
}

$workloadOutput = & dotnet $workloadOutputAssembly runtime $workloadFixtureDirectory input.txt second.txt 2>&1
if ($LASTEXITCODE -ne 0) {
    throw "Packaged MSBuild SDK generated-bindings workload failed with exit code $LASTEXITCODE.`n$($workloadOutput | Out-String)"
}

$actualWorkloadOutput = (($workloadOutput | ForEach-Object { $_.ToString() }) | Where-Object { $_.Trim().Length -gt 0 }) -join [Environment]::NewLine
$expectedWorkloadOutput = @("alpha-runtime", "runtime-beta", "runtime-gamma") -join [Environment]::NewLine
if ($actualWorkloadOutput -ne $expectedWorkloadOutput) {
    throw "Expected packaged MSBuild SDK generated-bindings workload to print '$expectedWorkloadOutput', got '$actualWorkloadOutput'."
}

Push-Location $scratchProjectRoot
$previousNuGetPackages = $env:NUGET_PACKAGES
try {
    $env:NUGET_PACKAGES = $nugetPackages
    dotnet clean $workloadProjectPath -c $Configuration /nologo | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "Packaged MSBuild SDK generated-bindings workload clean failed with exit code $LASTEXITCODE."
    }
}
finally {
    $env:NUGET_PACKAGES = $previousNuGetPackages
    Pop-Location
}

foreach ($outputPath in @($workloadOutputAssembly, $workloadRuntimeConfigPath, $workloadBitcodePath) + ($supportAssemblyNames | ForEach-Object { Join-Path $workloadOutputDirectory $_ })) {
    if (Test-Path $outputPath) {
        throw "Expected packaged MSBuild SDK workload clean to remove generated output: $outputPath"
    }
}

Write-Host "PASS msbuild_add_packaged (nuget sdk) => 42"
Write-Host "PASS bin_trivial (nuget sdk inferred bin) => exit 0"
Write-Host "PASS generated_bindings_lousygrep (nuget sdk support assemblies + clean)"
