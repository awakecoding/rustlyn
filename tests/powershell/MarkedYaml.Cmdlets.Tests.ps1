Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')

BeforeDiscovery {
    $yamlBaselineAvailable = [bool](Get-Command ConvertTo-Yaml -ErrorAction SilentlyContinue) -and
        [bool](Get-Command ConvertFrom-Yaml -ErrorAction SilentlyContinue)
}

Describe 'Rust marked-yaml PowerShell cmdlets' {
    BeforeAll {
        . (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')
        $script:yamlBaselineAvailable = [bool](Get-Command ConvertTo-Yaml -ErrorAction SilentlyContinue) -and
            [bool](Get-Command ConvertFrom-Yaml -ErrorAction SilentlyContinue)
        if ($script:yamlBaselineAvailable) {
            $manifest = Build-RustlynPowerShellModule `
                -BuildScriptName 'Build-MarkedYamlPowerShellModule.ps1' `
                -ManifestRelativePath 'artifacts\out\marked_yaml_powershell\Rustlyn.MarkedYaml.PowerShell.psd1'
            Import-RustlynPowerShellModule -ManifestPath $manifest
        }
    }

    It 'exports YAML conversion cmdlets' -Skip:(-not $yamlBaselineAvailable) {
        Get-Command ConvertTo-RustYaml -ErrorAction Stop | Should -Not -BeNullOrEmpty
        Get-Command ConvertFrom-RustYaml -ErrorAction Stop | Should -Not -BeNullOrEmpty
    }

    It 'matches ConvertTo-Yaml after baseline parse normalization for diverse values' -Skip:(-not $yamlBaselineAvailable) {
        $cases = @(
            @{ Name = 'string'; Value = 'Tom "quoted": value' },
            @{ Name = 'int'; Value = 42 },
            @{ Name = 'bool'; Value = $true },
            @{ Name = 'array'; Value = @('one', 2, $false) },
            @{ Name = 'ordered-map'; Value = [ordered]@{ name = 'rustlyn'; count = 3; active = $true } },
            @{ Name = 'object'; Value = [pscustomobject]@{ name = 'rustlyn'; nested = [pscustomobject]@{ active = $true } } },
            @{ Name = 'unicode'; Value = [ordered]@{ text = 'café naïve résumé'; colon = 'a:b'; quote = 'he said "yes"' } },
            @{ Name = 'multiline'; Value = [ordered]@{ text = "line1`nline2`nline3" } }
        )

        foreach ($case in $cases) {
            $expected = ConvertTo-Yaml -Data $case.Value
            $actual = ConvertTo-RustYaml -InputObject $case.Value

            ConvertTo-CanonicalYamlObjectJson (ConvertFrom-Yaml -Yaml $actual) |
                Should -Be (ConvertTo-CanonicalYamlObjectJson (ConvertFrom-Yaml -Yaml $expected))
        }
    }

    It 'matches ConvertFrom-Yaml after normalization for maps, sequences, scalars, and markers' -Skip:(-not $yamlBaselineAvailable) {
        $documents = @(
            "name: rustlyn`ncount: 3`nitems:`n- one`n- two`n",
            "---`nname: rustlyn`nnested:`n  active: true`n",
            "items:`n- 1`n- 2`n- 3`n",
            "text: |`n  line1`n  line2`n",
            "quoted: `"a:b # not comment`"`nunicode: `"café naïve résumé`"`n",
            "empty:`nnullValue: null`nbool: false`n"
        )

        foreach ($yaml in $documents) {
            $expected = ConvertFrom-Yaml -Yaml $yaml
            $actual = ConvertFrom-RustYaml -InputObject $yaml

            ConvertTo-CanonicalYamlObjectJson $actual |
                Should -Be (ConvertTo-CanonicalYamlObjectJson $expected)
        }
    }

    It 'roundtrips through Rust and baseline YAML cmdlets in both directions' -Skip:(-not $yamlBaselineAvailable) {
        $value = [ordered]@{
            name = 'rustlyn'
            items = @('yaml', 'marked', 'powershell')
            nested = [ordered]@{ active = $true; note = 'roundtrip' }
        }

        $rustYaml = ConvertTo-RustYaml -InputObject $value
        $baselineYaml = ConvertTo-Yaml -Data $value

        ConvertTo-CanonicalYamlObjectJson (ConvertFrom-Yaml -Yaml $rustYaml) |
            Should -Be (ConvertTo-CanonicalYamlObjectJson (ConvertFrom-Yaml -Yaml $baselineYaml))
        ConvertTo-CanonicalYamlObjectJson (ConvertFrom-RustYaml -InputObject $baselineYaml) |
            Should -Be (ConvertTo-CanonicalYamlObjectJson (ConvertFrom-Yaml -Yaml $baselineYaml))
        ConvertTo-CanonicalYamlObjectJson (ConvertFrom-RustYaml -InputObject $rustYaml) |
            Should -Be (ConvertTo-CanonicalYamlObjectJson (ConvertFrom-Yaml -Yaml $baselineYaml))
    }

    It 'rejects malformed YAML variants' -Skip:(-not $yamlBaselineAvailable) {
        $badInputs = @(
            'root: [unclosed',
            'root: { bad',
            "items:`n- one`n- [unterminated"
        )

        foreach ($yaml in $badInputs) {
            { ConvertFrom-RustYaml -InputObject $yaml -ErrorAction Stop } | Should -Throw
        }
    }
}
