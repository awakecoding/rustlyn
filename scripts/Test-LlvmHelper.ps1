param(
    [Parameter(Mandatory = $false)]
    [string]$LlvmDevRoot = $env:RUSTLYN_LLVM_ROOT,

    [Parameter(Mandatory = $false)]
    [string]$Sample = "add",

    [Parameter(Mandatory = $false)]
    [string]$ToolPath,

    [Parameter(Mandatory = $false)]
    [string]$ToolDll
)

$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$helperPath = & (Join-Path $PSScriptRoot "Build-RustlynLlvmHelper.ps1") -LlvmDevRoot $LlvmDevRoot -Configuration release | Select-Object -Last 1
$bitcodePath = & (Join-Path $PSScriptRoot "Build-SampleBitcode.ps1") -Sample $Sample | Select-Object -Last 1
$helperRoot = Split-Path -Parent $helperPath
. (Join-Path $PSScriptRoot "Rustlyn.Cli.ps1")
$rustlyn = Resolve-RustlynCli -RepoRoot $workspaceRoot -Configuration Release -ToolPath $ToolPath -ToolDll $ToolDll

Invoke-RustlynCli $rustlyn llvm print-ir $bitcodePath --llvm-root $helperRoot --disable-verify --output - | Out-Null
if ($LASTEXITCODE -ne 0) {
    throw "rustlyn llvm print-ir failed with exit code $LASTEXITCODE."
}

Invoke-RustlynCli $rustlyn lower $bitcodePath --llvm-root $helperRoot | Out-Null
if ($LASTEXITCODE -ne 0) {
    throw "Rustlyn lower failed with helper root '$helperRoot' and exit code $LASTEXITCODE."
}

Write-Host "rustlyn llvm helper smoke passed for sample '$Sample'."
