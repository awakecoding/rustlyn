Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')

Describe 'Rust toml PowerShell cmdlets' {
    BeforeAll {
        . (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')
        $manifest = Build-RustlynPowerShellModule `
            -BuildScriptName 'Build-TomlPowerShellModule.ps1' `
            -ManifestRelativePath 'artifacts\out\toml_powershell\Rustlyn.Toml.PowerShell.psd1'
        Import-RustlynPowerShellModule -ManifestPath $manifest
    }

    It 'exports TOML conversion cmdlets' {
        Get-Command ConvertTo-RustToml -ErrorAction Stop | Should -Not -BeNullOrEmpty
        Get-Command ConvertFrom-RustToml -ErrorAction Stop | Should -Not -BeNullOrEmpty
    }

    It 'roundtrips TOML-compatible maps through the Rust engine' {
        $value = [ordered]@{
            title = 'Rustlyn'
            format = 'toml'
        }

        $toml = ConvertTo-RustToml -InputObject $value -Depth 8
        $toml | Should -Match 'title = "Rustlyn"'
        $toml | Should -Match 'format = "toml"'

        $actual = ConvertFrom-RustToml -InputObject $toml
        Assert-ObjectJsonEquivalent $actual $value
    }

    It 'rejects malformed TOML' {
        { ConvertFrom-RustToml -InputObject 'title = [unclosed' -ErrorAction Stop } | Should -Throw
    }
}
