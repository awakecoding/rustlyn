Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')

Describe 'Rust simd-json PowerShell cmdlets' {
    BeforeAll {
        . (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')
        $manifest = Build-RustlynPowerShellModule `
            -BuildScriptName 'Build-SimdJsonPowerShellModule.ps1' `
            -ManifestRelativePath 'artifacts\out\simd_json_powershell\Rustlyn.SimdJson.PowerShell.psd1'
        Import-RustlynPowerShellModule -ManifestPath $manifest
    }

    It 'exports JSON conversion cmdlets' {
        Get-Command ConvertTo-RustJson -ErrorAction Stop | Should -Not -BeNullOrEmpty
        Get-Command ConvertFrom-RustJson -ErrorAction Stop | Should -Not -BeNullOrEmpty
    }

    It 'matches ConvertTo-Json compressed output for scalar and object matrices' {
        $cases = @(
            @{ Name = 'string'; Value = 'Tom "quoted" \ slash'; Depth = 4 },
            @{ Name = 'int'; Value = 42; Depth = 4 },
            @{ Name = 'long'; Value = [long]1234567890123; Depth = 4 },
            @{ Name = 'double'; Value = 3.14159; Depth = 4 },
            @{ Name = 'bool'; Value = $true; Depth = 4 },
            @{ Name = 'null'; Value = $null; Depth = 4 },
            @{ Name = 'array'; Value = @(1, 'two', $false, $null); Depth = 5 },
            @{ Name = 'ordered-map'; Value = [ordered]@{ name = 'rustlyn'; count = 3; active = $true }; Depth = 5 },
            @{ Name = 'object'; Value = [pscustomobject]@{ name = 'rustlyn'; count = 3; nested = [pscustomobject]@{ active = $true } }; Depth = 5 },
            @{ Name = 'unicode'; Value = [pscustomobject]@{ text = "emoji 🚀 café"; controls = "line`nbreak`t tab" }; Depth = 5 }
        )

        foreach ($case in $cases) {
            ConvertTo-RustJson -InputObject $case.Value -Depth $case.Depth -Compress |
                Should -Be (ConvertTo-Json -InputObject $case.Value -Depth $case.Depth -Compress)
        }
    }

    It 'matches ConvertTo-Json semantic output for formatted JSON and pipeline input' {
        $value = [pscustomobject]@{
            name = 'rustlyn'
            items = @(1, 2, 3)
            nested = [pscustomobject]@{ active = $true; note = 'formatted' }
        }

        Assert-ObjectJsonEquivalent `
            (ConvertFrom-Json -InputObject (ConvertTo-RustJson -InputObject $value -Depth 5)) `
            (ConvertFrom-Json -InputObject (ConvertTo-Json -InputObject $value -Depth 5))

        $pipeline = @(
            [pscustomobject]@{ name = 'one'; count = 1 },
            [pscustomobject]@{ name = 'two'; count = 2 }
        )
        Assert-ObjectJsonEquivalent `
            (ConvertFrom-Json -InputObject ([string]::Concat(@($pipeline | ConvertTo-RustJson -Depth 5 -Compress)))) `
            (ConvertFrom-Json -InputObject ([string]::Concat(@($pipeline | ConvertTo-Json -Depth 5 -Compress))))
    }

    It 'matches ConvertTo-Json depth and enum handling' {
        $deep = [pscustomobject]@{
            level1 = [pscustomobject]@{
                level2 = [pscustomobject]@{
                    level3 = [pscustomobject]@{ value = 'deep' }
                }
            }
        }

        foreach ($depth in 1, 2, 3, 4) {
            ConvertTo-RustJson -InputObject $deep -Depth $depth -Compress |
                Should -Be (ConvertTo-Json -InputObject $deep -Depth $depth -Compress)
        }

        ConvertTo-RustJson -InputObject ([System.DayOfWeek]::Friday) -EnumsAsStrings -Compress |
            Should -Be (ConvertTo-Json -InputObject ([System.DayOfWeek]::Friday) -EnumsAsStrings -Compress)
    }

    It 'matches ConvertFrom-Json object shape after normalization for diverse documents' {
        $documents = @(
            'null',
            'true',
            '42',
            '-3.5',
            '"hello"',
            '["one",2,false,null]',
            '{"name":"rustlyn","count":3,"items":[1,2,3],"nested":{"active":true}}',
            '{"unicode":"emoji 🚀 café","escaped":"quote \" slash \\ newline\n"}',
            '[{"name":"one"},{"name":"two","items":[1,2]}]'
        )

        foreach ($json in $documents) {
            $expected = ConvertFrom-Json -InputObject $json
            $actual = ConvertFrom-RustJson -InputObject $json

            Assert-ObjectJsonEquivalent $actual $expected
        }
    }

    It 'honors AsHashtable for ConvertFrom-RustJson including nested maps' {
        $actual = ConvertFrom-RustJson -InputObject '{"name":"rustlyn","nested":{"active":true},"items":[1,2]}' -AsHashtable
        $expected = ConvertFrom-Json -InputObject '{"name":"rustlyn","nested":{"active":true},"items":[1,2]}' -AsHashtable

        $actual | Should -BeOfType ([hashtable])
        $actual['nested'] | Should -BeOfType ([hashtable])
        Assert-ObjectJsonEquivalent $actual $expected
    }

    It 'honors NoEnumerate for single-item arrays' {
        $actual = ConvertFrom-RustJson -InputObject '[1]' -NoEnumerate
        $expected = ConvertFrom-Json -InputObject '[1]' -NoEnumerate

        Assert-ObjectJsonEquivalent $actual $expected
    }

    It 'roundtrips through Rust and built-in JSON cmdlets in both directions' {
        $value = [pscustomobject]@{
            name = 'rustlyn'
            numbers = @(1, 2, 3)
            nested = [pscustomobject]@{ active = $true; text = 'roundtrip' }
        }

        $rustJson = ConvertTo-RustJson -InputObject $value -Depth 6 -Compress
        $builtInJson = ConvertTo-Json -InputObject $value -Depth 6 -Compress

        Assert-ObjectJsonEquivalent (ConvertFrom-Json -InputObject $rustJson) (ConvertFrom-Json -InputObject $builtInJson)
        Assert-ObjectJsonEquivalent (ConvertFrom-RustJson -InputObject $builtInJson) (ConvertFrom-Json -InputObject $builtInJson)
        Assert-ObjectJsonEquivalent (ConvertFrom-RustJson -InputObject $rustJson) (ConvertFrom-Json -InputObject $builtInJson)
    }

    It 'rejects malformed JSON variants' {
        $badInputs = @(
            '{"name":',
            '[1,]',
            '{"a":true',
            '{"a":tru}',
            '"unterminated',
            '{]'
        )

        foreach ($json in $badInputs) {
            { ConvertFrom-RustJson -InputObject $json -ErrorAction Stop } | Should -Throw
        }
    }
}
