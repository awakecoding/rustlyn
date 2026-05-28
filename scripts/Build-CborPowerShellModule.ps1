param(
    [string]$OutDir = 'artifacts\out\cbor_powershell',
    [string]$Toolchain = 'nightly',
    [string]$BuildStd = 'std,panic_abort'
)

$ErrorActionPreference = 'Stop'
& (Join-Path $PSScriptRoot 'Build-RustFormatPowerShellModule.ps1') `
    -Sample cbor `
    -EngineAssemblyName cbor_engine.dll `
    -CmdletsToExport @('ConvertTo-RustCbor', 'ConvertFrom-RustCbor') `
    -ManifestFileName Rustlyn.Cbor.PowerShell.psd1 `
    -ModuleGuid '4a237bdd-bdc4-48c9-a3aa-d274c87bb14c' `
    -OutDir $OutDir `
    -Toolchain $Toolchain `
    -BuildStd $BuildStd
