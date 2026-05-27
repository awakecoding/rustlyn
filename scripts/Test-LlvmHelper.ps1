param(
    [Parameter(Mandatory = $false)]
    [string]$LlvmDevRoot = $env:RUSTLYN_LLVM_DEV_ROOT,

    [Parameter(Mandatory = $false)]
    [string]$Sample = "add"
)

$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$helperPath = & (Join-Path $PSScriptRoot "Build-RustlynLlvmHelper.ps1") -LlvmDevRoot $LlvmDevRoot -Configuration release | Select-Object -Last 1
$bitcodePath = & (Join-Path $PSScriptRoot "Build-SampleBitcode.ps1") -Sample $Sample | Select-Object -Last 1

& $helperPath print-ir $bitcodePath --disable-verify --output - | Out-Null
if ($LASTEXITCODE -ne 0) {
    throw "rustlyn-llvm print-ir failed with exit code $LASTEXITCODE."
}

$helperRoot = Split-Path -Parent $helperPath
& dotnet run -c Release --project (Join-Path $workspaceRoot "dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj") -- lower $bitcodePath --llvm-root $helperRoot | Out-Null
if ($LASTEXITCODE -ne 0) {
    throw "Rustlyn lower failed with helper root '$helperRoot' and exit code $LASTEXITCODE."
}

Write-Host "rustlyn-llvm helper smoke passed for sample '$Sample'."
