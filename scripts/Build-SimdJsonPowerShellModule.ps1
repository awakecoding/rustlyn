param(
    [string]$OutDir = 'artifacts\out\simd_json_powershell',
    [string]$Toolchain = 'nightly',
    [string]$BuildStd = 'std,panic_abort'
)

$ErrorActionPreference = 'Stop'
& (Join-Path $PSScriptRoot 'Build-RustFormatPowerShellModule.ps1') `
    -Sample simd_json `
    -EngineAssemblyName simd_json_engine.dll `
    -CmdletsToExport @('ConvertTo-RustJson', 'ConvertFrom-RustJson') `
    -ManifestFileName Rustlyn.SimdJson.PowerShell.psd1 `
    -ModuleGuid 'e4ba0c7a-fbb1-495f-96e8-9febd90a212c' `
    -OutDir $OutDir `
    -Toolchain $Toolchain `
    -BuildStd $BuildStd
