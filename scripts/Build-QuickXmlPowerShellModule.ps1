param(
    [string]$OutDir = 'artifacts\out\quick_xml_powershell',
    [string]$Toolchain = 'nightly',
    [string]$BuildStd = 'std,panic_abort'
)

$ErrorActionPreference = 'Stop'
& (Join-Path $PSScriptRoot 'Build-RustFormatPowerShellModule.ps1') `
    -Sample quick_xml `
    -EngineAssemblyName quick_xml_engine.dll `
    -CmdletsToExport @('ConvertTo-RustXml', 'ConvertFrom-RustXml') `
    -ManifestFileName Rustlyn.QuickXml.PowerShell.psd1 `
    -ModuleGuid 'd619c7f8-72c4-492e-9d2e-0a80508ee3ac' `
    -OutDir $OutDir `
    -Toolchain $Toolchain `
    -BuildStd $BuildStd
