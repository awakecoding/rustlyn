param(
    [Parameter(Mandatory = $false)]
    [string]$Sample = "add"
)

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$sampleRoot = Join-Path $workspaceRoot (Join-Path "samples" $Sample)
$sourcePath = Join-Path $sampleRoot "src\lib.rs"

if (-not (Test-Path $sourcePath)) {
    throw "Sample source not found: $sourcePath"
}

$outputDirectory = Join-Path $workspaceRoot (Join-Path "artifacts\out" $Sample)
New-Item -ItemType Directory -Force -Path $outputDirectory | Out-Null

$outputPath = Join-Path $outputDirectory "$Sample.bc"

rustc $sourcePath `
    --crate-name $Sample `
    --crate-type lib `
    --edition 2024 `
    --emit llvm-bc `
    -C overflow-checks=off `
    -C panic=abort `
    -o $outputPath

if (-not (Test-Path $outputPath)) {
    throw "Expected bitcode output was not produced: $outputPath"
}

Write-Output $outputPath