Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')

Describe 'Rust bson PowerShell cmdlets' {
    BeforeAll {
        . (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')
        $manifest = Build-RustlynPowerShellModule `
            -BuildScriptName 'Build-BsonPowerShellModule.ps1' `
            -ManifestRelativePath 'artifacts\out\bson_powershell\Rustlyn.Bson.PowerShell.psd1'
        Import-RustlynPowerShellModule -ManifestPath $manifest
    }

    It 'exports BSON conversion cmdlets' {
        Get-Command ConvertTo-RustBson -ErrorAction Stop | Should -Not -BeNullOrEmpty
        Get-Command ConvertFrom-RustBson -ErrorAction Stop | Should -Not -BeNullOrEmpty
    }

    It 'roundtrips a document through BSON bytes using the Rust engine' {
        $value = [ordered]@{
            name = 'rustlyn'
            count = 3
            active = $true
            nested = [ordered]@{ format = 'bson' }
            items = @(1, 2, 3)
        }

        $bytes = ConvertTo-RustBson -InputObject $value -Depth 8
        $bytes.GetType() | Should -Be ([byte[]])
        $bytes.Length | Should -BeGreaterThan 0

        $actual = ConvertFrom-RustBson -InputObject $bytes
        Assert-ObjectJsonEquivalent $actual $value
    }

    It 'accepts pipeline byte input and rejects malformed BSON' {
        $bytes = ConvertTo-RustBson -InputObject ([ordered]@{ name = 'pipeline'; count = 1 })
        $actual = $bytes | ConvertFrom-RustBson
        $actual.name | Should -Be 'pipeline'

        { ConvertFrom-RustBson -InputObject ([byte[]](0, 1, 2)) -ErrorAction Stop } | Should -Throw
    }
}
