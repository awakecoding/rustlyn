[CmdletBinding()]
param(
    [string]$Sample = 'add',
    [string]$Passes = 'mem2reg,sroa,simplifycfg',
    [string]$LlvmDevRoot = $env:RUSTLYN_LLVM_ROOT,
    [string]$ToolDll,
    [switch]$SkipBuild
)

$ErrorActionPreference = 'Stop'
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
Push-Location $repoRoot
try {
    $helperName = if ($IsWindows -or $env:OS -eq 'Windows_NT') { 'rustlyn-llvm.exe' } else { 'rustlyn-llvm' }
    $helper = Join-Path $repoRoot (Join-Path 'native\rustlyn-llvm\target\release' $helperName)
    if (-not $SkipBuild -or -not (Test-Path $helper)) {
        & (Join-Path $PSScriptRoot 'Build-RustlynLlvmHelper.ps1') -LlvmDevRoot $LlvmDevRoot | Out-Null
    }
    if (-not (Test-Path $helper)) {
        throw "rustlyn-llvm helper not found at '$helper'"
    }

    & (Join-Path $PSScriptRoot 'Build-SampleBitcode.ps1') -Sample $Sample | Out-Null
    $bitcode = Join-Path $repoRoot ("artifacts\out\{0}\{0}.bc" -f $Sample)
    if (-not (Test-Path $bitcode)) {
        throw "sample bitcode not found at '$bitcode'"
    }

    $optBc = Join-Path $repoRoot ("artifacts\out\{0}\{0}.opt.bc" -f $Sample)
    Write-Host "[rustlyn-llvm opt] $bitcode -> $optBc (passes: $Passes)"
    & $helper opt $bitcode --passes $Passes --output $optBc
    if ($LASTEXITCODE -ne 0) { throw "rustlyn-llvm opt failed with exit code $LASTEXITCODE" }
    if (-not (Test-Path $optBc)) { throw "expected optimized bitcode at '$optBc'" }

    $beforeIr = & $helper print-ir $bitcode --disable-verify --output -
    $afterIr  = & $helper print-ir $optBc  --disable-verify --output -
    $beforeAllocas = ([regex]::Matches($beforeIr, '\balloca\b')).Count
    $afterAllocas  = ([regex]::Matches($afterIr,  '\balloca\b')).Count
    Write-Host "alloca count: before=$beforeAllocas after=$afterAllocas"
    if ($beforeAllocas -gt 0 -and $afterAllocas -ge $beforeAllocas) {
        Write-Warning "opt did not reduce alloca count for sample '$Sample'."
    }

    # End-to-end: invoke through Rustlyn with RUSTLYN_LLVM_OPT_PASSES enabled.
    $env:RUSTLYN_LLVM_ROOT = Join-Path $repoRoot 'native\rustlyn-llvm\target\release'
    $env:RUSTLYN_LLVM_OPT_PASSES = $Passes
    if ($Sample -eq 'add') {
        Write-Host "[rustlyn invoke] add_i32(2,3) with opt pre-pass"
        if ($ToolDll) {
            $resolvedToolDll = if ([System.IO.Path]::IsPathRooted($ToolDll)) { $ToolDll } else { Join-Path $repoRoot $ToolDll }
            if (-not (Test-Path -LiteralPath $resolvedToolDll -PathType Leaf)) {
                throw "Rustlyn.Tool DLL not found: $resolvedToolDll"
            }

            $result = dotnet $resolvedToolDll invoke $bitcode --method add_i32 --arg i32:2 --arg i32:3
        }
        else {
            $result = dotnet run -c Release --no-build --project (Join-Path $repoRoot 'dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj') -- invoke $bitcode --method add_i32 --arg i32:2 --arg i32:3
        }
        if ($LASTEXITCODE -ne 0) { throw "rustlyn invoke failed (exit $LASTEXITCODE): $result" }
        $value = ($result | Select-Object -Last 1).Trim()
        if ($value -ne '5') { throw "expected 5 from add_i32(2,3), got '$value'" }
        Write-Host "OK add_i32(2,3) = $value"
    }
}
finally {
    Pop-Location
}
