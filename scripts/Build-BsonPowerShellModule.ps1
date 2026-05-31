param(
    [string]$OutDir = 'artifacts\out\bson_powershell',
    [string]$Toolchain = 'nightly',
    [string]$BuildStd = 'std,panic_abort'
)

$ErrorActionPreference = 'Stop'
& (Join-Path $PSScriptRoot 'Build-RustFormatPowerShellModule.ps1') `
    -Sample powershell_cmdlets `
    -EngineAssemblyName rustlyn_powershell_format_cmdlets.dll `
    -CmdletsToExport @('ConvertTo-RustBson', 'ConvertFrom-RustBson') `
    -ManifestFileName Rustlyn.Bson.PowerShell.psd1 `
    -ModuleGuid '3adf6b12-aa30-4bdf-8688-a2dcf8366a7e' `
    -OutDir $OutDir `
    -Toolchain $Toolchain `
    -BuildStd $BuildStd
