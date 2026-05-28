param(
    [string]$OutDir = 'artifacts\out\toml_powershell',
    [string]$Toolchain = 'nightly',
    [string]$BuildStd = 'std,panic_abort'
)

$ErrorActionPreference = 'Stop'
& (Join-Path $PSScriptRoot 'Build-RustFormatPowerShellModule.ps1') `
    -Sample toml `
    -EngineAssemblyName toml_engine.dll `
    -CmdletsToExport @('ConvertTo-RustToml', 'ConvertFrom-RustToml') `
    -ManifestFileName Rustlyn.Toml.PowerShell.psd1 `
    -ModuleGuid 'f402491f-21cb-4478-83e0-dcfeecbd8348' `
    -OutDir $OutDir `
    -Toolchain $Toolchain `
    -BuildStd $BuildStd
