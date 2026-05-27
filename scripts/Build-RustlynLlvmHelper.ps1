param(
    [Parameter(Mandatory = $false)]
    [string]$LlvmDevRoot = $env:RUSTLYN_LLVM_ROOT,

    [Parameter(Mandatory = $false)]
    [ValidateSet("debug", "release")]
    [string]$Configuration = "release"
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($LlvmDevRoot) -and -not [string]::IsNullOrWhiteSpace($env:RUSTLYN_LLVM_DEV_ROOT)) {
    $LlvmDevRoot = $env:RUSTLYN_LLVM_DEV_ROOT
}

if ([string]::IsNullOrWhiteSpace($LlvmDevRoot)) {
    throw "LlvmDevRoot is required. Pass -LlvmDevRoot or set RUSTLYN_LLVM_ROOT to an LLVM development root with bin\llvm-config.exe and static libraries."
}

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$manifestPath = Join-Path $workspaceRoot "native\rustlyn-llvm\Cargo.toml"

$env:RUSTLYN_LLVM_ROOT = $LlvmDevRoot
$env:RUSTLYN_LLVM_DEV_ROOT = $LlvmDevRoot

$args = @("build", "--manifest-path", $manifestPath)
if ($Configuration -eq "release") {
    $args += "--release"
}

& cargo @args
if ($LASTEXITCODE -ne 0) {
    throw "cargo build failed with exit code $LASTEXITCODE."
}

$targetDir = Join-Path $workspaceRoot "native\rustlyn-llvm\target\$Configuration"
$exeName = if ($IsWindows -or $env:OS -eq "Windows_NT") { "rustlyn-llvm.exe" } else { "rustlyn-llvm" }
$helperPath = Join-Path $targetDir $exeName
if (-not (Test-Path $helperPath)) {
    throw "Expected helper was not produced: $helperPath"
}

Write-Output $helperPath
