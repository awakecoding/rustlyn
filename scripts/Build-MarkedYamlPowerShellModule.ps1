param(
    [string]$OutDir = 'artifacts\out\marked_yaml_powershell',
    [string]$Toolchain = 'nightly',
    [string]$BuildStd = 'std,panic_abort'
)

$ErrorActionPreference = 'Stop'
& (Join-Path $PSScriptRoot 'Build-RustFormatPowerShellModule.ps1') `
    -Sample marked_yaml `
    -EngineAssemblyName marked_yaml_engine.dll `
    -CmdletsToExport @('ConvertTo-RustYaml', 'ConvertFrom-RustYaml') `
    -ManifestFileName Rustlyn.MarkedYaml.PowerShell.psd1 `
    -ModuleGuid '3d71fc1e-8296-41d3-a384-51c8f0fdff47' `
    -OutDir $OutDir `
    -Toolchain $Toolchain `
    -BuildStd $BuildStd
