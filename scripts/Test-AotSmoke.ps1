#requires -Version 7.0
<#
.SYNOPSIS
    Drives the AOT smoke sample end-to-end.

.DESCRIPTION
    1. Translates samples/aot_smoke into a managed assembly using rustlyn.
    2. Copies that assembly into samples/aot_smoke/host as AotSmoke.Translated.dll.
    3. Publishes the host with PublishAot=true.
    4. Executes the published native binary and asserts the expected stdout.

.PARAMETER LlvmRoot
    Path to the LLVM toolchain root. Defaults to the RUSTLYN_LLVM_ROOT env var.

.PARAMETER Runtime
    Runtime identifier passed to dotnet publish (default: detected from $PSVersionTable.OS).
#>
param(
    [string]$LlvmRoot = $env:RUSTLYN_LLVM_ROOT,
    [string]$Runtime,
    [string]$ToolPath,
    [string]$ToolDll
)

$ErrorActionPreference = 'Stop'
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$sample = Join-Path $repoRoot 'samples\aot_smoke'
$hostProject = Join-Path $sample 'host'
$translatedDll = Join-Path $hostProject 'AotSmoke.Translated.dll'
$translatedBc = Join-Path $hostProject 'AotSmoke.Translated.bc'
. (Join-Path $PSScriptRoot 'Rustlyn.Cli.ps1')
$rustlyn = Resolve-RustlynCli -RepoRoot $repoRoot -Configuration Release -ToolPath $ToolPath -ToolDll $ToolDll

if (-not $Runtime) {
    $Runtime = if ($IsWindows) { 'win-x64' } elseif ($IsLinux) { 'linux-x64' } elseif ($IsMacOS) { 'osx-x64' } else { 'linux-x64' }
}

Write-Host "Translating $sample -> $translatedDll"
$translateArgs = @('translate', $sample, '--out', $translatedDll, '--bitcode-out', $translatedBc)
if ($LlvmRoot) { $translateArgs += @('--llvm-root', $LlvmRoot) }
Invoke-RustlynCli $rustlyn @translateArgs
if ($LASTEXITCODE -ne 0) { throw "rustlyn translate failed with exit code $LASTEXITCODE." }

Write-Host "Publishing host for $Runtime with PublishAot=true"
& dotnet publish $hostProject -c Release -r $Runtime --self-contained true
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed with exit code $LASTEXITCODE." }

$publishDir = Join-Path $hostProject "bin\Release\net10.0\$Runtime\publish"
$exeName = if ($IsWindows) { 'AotSmokeHost.exe' } else { 'AotSmokeHost' }
$exe = Join-Path $publishDir $exeName
if (-not (Test-Path $exe)) { throw "Published binary not found: $exe" }

$translatedAssemblies = Get-ChildItem -Path $hostProject -Filter '*.dll' -File
foreach ($assembly in $translatedAssemblies) {
    Copy-Item -Path $assembly.FullName -Destination (Join-Path $publishDir $assembly.Name) -Force
}

Write-Host "Running $exe"
$output = & $exe
if ($LASTEXITCODE -ne 0) { throw "AOT binary exited with code $LASTEXITCODE. Output: $output" }

$expected = 'aot_smoke: sum=42 meaning=42'
if ($output -ne $expected) { throw "Unexpected output:`nExpected: $expected`nActual:   $output" }

Write-Host "AOT smoke passed."
