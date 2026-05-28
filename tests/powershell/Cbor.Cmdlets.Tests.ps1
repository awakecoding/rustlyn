Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')

Describe 'Rust cbor PowerShell cmdlets' {
    BeforeAll {
        . (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')
        $manifest = Build-RustlynPowerShellModule `
            -BuildScriptName 'Build-CborPowerShellModule.ps1' `
            -ManifestRelativePath 'artifacts\out\cbor_powershell\Rustlyn.Cbor.PowerShell.psd1'
        Import-RustlynPowerShellModule -ManifestPath $manifest
    }

    It 'exports CBOR conversion cmdlets' {
        Get-Command ConvertTo-RustCbor -ErrorAction Stop | Should -Not -BeNullOrEmpty
        Get-Command ConvertFrom-RustCbor -ErrorAction Stop | Should -Not -BeNullOrEmpty
    }

    It 'roundtrips values through CBOR bytes using the Rust engine' {
        $value = [ordered]@{
            name = 'rustlyn'
            count = 3
            active = $true
            nested = [ordered]@{ format = 'cbor' }
            items = @(1, 'two', $false)
        }

        $bytes = ConvertTo-RustCbor -InputObject $value -Depth 8
        $bytes.GetType() | Should -Be ([byte[]])
        $bytes.Length | Should -BeGreaterThan 0

        $actual = ConvertFrom-RustCbor -InputObject $bytes
        Assert-ObjectJsonEquivalent $actual $value
    }

    It 'accepts pipeline byte input and rejects malformed CBOR' {
        $bytes = ConvertTo-RustCbor -InputObject @('one', 2, $true)
        $actual = $bytes | ConvertFrom-RustCbor
        Assert-ObjectJsonEquivalent $actual @('one', 2, $true)

        { ConvertFrom-RustCbor -InputObject ([byte[]](0xff)) -ErrorAction Stop } | Should -Throw
    }
}
