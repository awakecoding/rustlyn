param(
    [Parameter(Mandatory = $false)]
    [string]$Sample = "add",

    [Parameter(Mandatory = $false)]
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = 'Stop'

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$sampleRoot = Join-Path $workspaceRoot (Join-Path "samples" $Sample)
$sourcePath = Join-Path $sampleRoot "src\lib.rs"

if (-not (Test-Path $sourcePath)) {
    throw "Sample source not found: $sourcePath"
}

$outputDirectory = Join-Path $workspaceRoot (Join-Path "artifacts\out" $Sample)
New-Item -ItemType Directory -Force -Path $outputDirectory | Out-Null

. (Join-Path $PSScriptRoot 'Rustlyn.Cli.ps1')

$rustlyn = Resolve-RustlynCli -RepoRoot $workspaceRoot -Configuration $Configuration
Invoke-RustlynCli $rustlyn rustc $sourcePath `
    --crate-name $Sample `
    --out-dir $outputDirectory `
    --emit 'bc,ll' | Out-Null

$outputPath = Join-Path $outputDirectory "$Sample.bc"
if (-not (Test-Path $outputPath)) {
    throw "Expected bitcode output was not produced: $outputPath"
}

Write-Output $outputPath