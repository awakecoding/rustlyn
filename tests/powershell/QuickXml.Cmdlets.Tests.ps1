Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')

Describe 'Rust quick-xml PowerShell cmdlets' {
    BeforeAll {
        . (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')
        $manifest = Build-RustlynPowerShellModule `
            -BuildScriptName 'Build-QuickXmlPowerShellModule.ps1' `
            -ManifestRelativePath 'artifacts\out\quick_xml_powershell\Rustlyn.QuickXml.PowerShell.psd1'
        Import-RustlynPowerShellModule -ManifestPath $manifest
    }

    It 'exports XML conversion cmdlets' {
        Get-Command ConvertTo-RustXml -ErrorAction Stop | Should -Not -BeNullOrEmpty
        Get-Command ConvertFrom-RustXml -ErrorAction Stop | Should -Not -BeNullOrEmpty
    }

    It 'matches ConvertTo-Xml for diverse object graphs after canonicalization' {
        $punctuatedObject = [pscustomobject]::new()
        $punctuatedObject | Add-Member -NotePropertyName 'two words' -NotePropertyValue 'space'
        $punctuatedObject | Add-Member -NotePropertyName '🚀' -NotePropertyValue 'rocket'
        $punctuatedObject | Add-Member -NotePropertyName 'name:colon' -NotePropertyValue 'value'

        $cases = @(
            @{ Name = 'string'; Value = 'Tom & Jerry < 3'; Depth = 2 },
            @{ Name = 'integer'; Value = 42; Depth = 2 },
            @{ Name = 'boolean'; Value = $true; Depth = 2 },
            @{ Name = 'null'; Value = $null; Depth = 2 },
            @{ Name = 'guid'; Value = [pscustomobject]@{ Id = [guid]'11111111-2222-3333-4444-555555555555'; Name = 'rustlyn' }; Depth = 4 },
            @{ Name = 'char'; Value = [pscustomobject]@{ Letter = [char]0x41; Text = 'A&B' }; Depth = 4 },
            @{ Name = 'array'; Value = @(1, 'two', $false); Depth = 4 },
            @{ Name = 'ordered-map'; Value = [ordered]@{ Name = 'rustlyn'; Count = 3; Active = $true }; Depth = 4 },
            @{ Name = 'object'; Value = [pscustomobject]@{ Name = 'rustlyn'; Count = 3; Nested = [pscustomobject]@{ Active = $true } }; Depth = 4 },
            @{ Name = 'unicode'; Value = [pscustomobject]@{ Text = "emoji 🚀 𐐷 café"; Symbols = '<>&"'' 𝄞' }; Depth = 4 },
            @{ Name = 'property-name-punctuation'; Value = $punctuatedObject; Depth = 4 },
            @{ Name = 'scalar-mix'; Value = [pscustomobject]@{ Empty = ''; Decimal = [decimal]'12.3400'; When = [datetime]'2024-01-02T03:04:05.120Z'; Bytes = [byte[]](0, 1, 255) }; Depth = 4 },
            @{ Name = 'array-with-null-and-empty'; Value = [pscustomobject]@{ Items = @($null, '', 'text') }; Depth = 4 }
        )

        foreach ($case in $cases) {
            $expected = ConvertTo-Xml -InputObject $case.Value -As String -Depth $case.Depth -NoTypeInformation
            $actual = ConvertTo-RustXml -InputObject $case.Value -As String -Depth $case.Depth -NoTypeInformation

            ConvertTo-CanonicalXml $actual | Should -Be (ConvertTo-CanonicalXml $expected)
        }
    }

    It 'matches ConvertTo-Xml depth truncation behavior' {
        $value = [pscustomobject]@{
            Level1 = [pscustomobject]@{
                Level2 = [pscustomobject]@{
                    Level3 = [pscustomobject]@{ Name = 'deep' }
                }
            }
        }

        foreach ($depth in 1, 2, 3, 4) {
            $expected = ConvertTo-Xml -InputObject $value -As String -Depth $depth -NoTypeInformation
            $actual = ConvertTo-RustXml -InputObject $value -As String -Depth $depth -NoTypeInformation

            ConvertTo-CanonicalXml $actual | Should -Be (ConvertTo-CanonicalXml $expected)
        }
    }

    It 'matches type-information output when requested by omission' {
        $value = [pscustomobject]@{ Name = 'typed'; Count = 1 }

        $expected = ConvertTo-Xml -InputObject $value -As String -Depth 3
        $actual = ConvertTo-RustXml -InputObject $value -As String -Depth 3

        ConvertTo-CanonicalXml $actual | Should -Be (ConvertTo-CanonicalXml $expected)
    }

    It 'matches Document and Stream output modes semantically' {
        $value = [pscustomobject]@{ Name = 'rustlyn'; Count = 3 }
        $expected = ConvertTo-Xml -InputObject $value -As String -Depth 3 -NoTypeInformation

        $document = ConvertTo-RustXml -InputObject $value -As Document -Depth 3 -NoTypeInformation
        $stream = ConvertTo-RustXml -InputObject $value -As Stream -Depth 3 -NoTypeInformation

        $document | Should -BeOfType ([xml])
        ConvertTo-CanonicalXml (ConvertTo-XmlString $document) | Should -Be (ConvertTo-CanonicalXml $expected)
        ConvertTo-CanonicalXml (ConvertTo-XmlString $stream) | Should -Be (ConvertTo-CanonicalXml $expected)
    }

    It 'accepts pipeline input with the same canonical XML as ConvertTo-Xml' {
        $items = @(
            [pscustomobject]@{ Name = 'one'; Count = 1 },
            [pscustomobject]@{ Name = 'two'; Count = 2 }
        )

        $expected = [string]::Concat(@($items | ConvertTo-Xml -As String -Depth 3 -NoTypeInformation))
        $actual = [string]::Concat(@($items | ConvertTo-RustXml -As String -Depth 3 -NoTypeInformation))

        ConvertTo-CanonicalXml $actual | Should -Be (ConvertTo-CanonicalXml $expected)
    }

    It 'roundtrips built-in ConvertTo-Xml output through ConvertFrom-RustXml' {
        $cases = @(
            [pscustomobject]@{ Name = 'rustlyn' },
            [pscustomobject]@{ Text = 'Tom & Jerry'; Nested = [pscustomobject]@{ Active = $true } },
            [ordered]@{ Numbers = @(1, 2, 3); Empty = $null }
        )

        foreach ($case in $cases) {
            $xml = ConvertTo-Xml -InputObject $case -As String -Depth 4 -NoTypeInformation
            $document = ConvertFrom-RustXml -InputObject $xml

            $document | Should -BeOfType ([xml])
            ConvertTo-CanonicalXml $document.OuterXml | Should -Be (ConvertTo-CanonicalXml $xml)
        }
    }

    It 'accepts XML declaration, comments, CDATA, attributes, empty elements, and unicode text' {
        $xml = "<?xml version=`"1.0`" encoding=`"utf-8`"?><root attr=`"value`"><empty /><!--note--><item><![CDATA[emoji 🚀 <tag>]]></item></root>"
        $document = ConvertFrom-RustXml -InputObject $xml

        $document.root.attr | Should -Be 'value'
        $document.root.item.'#cdata-section' | Should -Be 'emoji 🚀 <tag>'
    }

    It 'rejects malformed XML variants' {
        $badInputs = @(
            '<root><item></root>',
            "<root attr='unterminated></root>",
            '<root><item></item>',
            '<root>&not-an-entity;</root>'
        )

        foreach ($xml in $badInputs) {
            { ConvertFrom-RustXml -InputObject $xml -ErrorAction Stop } | Should -Throw
        }
    }
}
