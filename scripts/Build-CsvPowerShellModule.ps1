param(
    [string]$OutDir = 'artifacts\out\csv_powershell',
    [string]$Toolchain = 'nightly',
    [string]$BuildStd = 'std,panic_abort'
)

$ErrorActionPreference = 'Stop'
& (Join-Path $PSScriptRoot 'Build-RustFormatPowerShellModule.ps1') `
    -Sample powershell_cmdlets `
    -EngineAssemblyName rustlyn_powershell_format_cmdlets.dll `
    -CmdletsToExport @('ConvertTo-RustCsv', 'ConvertFrom-RustCsv') `
    -ManifestFileName Rustlyn.Csv.PowerShell.psd1 `
    -ModuleGuid '21a467a7-5dbe-4543-a0ac-8377cf8e7c85' `
    -OutDir $OutDir `
    -Toolchain $Toolchain `
    -BuildStd $BuildStd
