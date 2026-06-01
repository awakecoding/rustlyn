Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')

Describe 'Rust csv PowerShell cmdlets' {
    BeforeAll {
        . (Join-Path $PSScriptRoot 'RustFormat.CmdletHelpers.ps1')
        $manifest = Build-RustlynPowerShellModule `
            -BuildScriptName 'Build-CsvPowerShellModule.ps1' `
            -ManifestRelativePath 'artifacts\out\csv_powershell\Rustlyn.Csv.PowerShell.psd1'
        Import-RustlynPowerShellModule -ManifestPath $manifest
    }

    It 'exports CSV conversion cmdlets' {
        Get-Command ConvertTo-RustCsv -ErrorAction Stop | Should -Not -BeNullOrEmpty
        Get-Command ConvertFrom-RustCsv -ErrorAction Stop | Should -Not -BeNullOrEmpty
    }

    It 'matches ConvertTo-Csv for scalar-like object property values' {
        $cases = @(
            [pscustomobject]@{ Name = 'rustlyn'; Count = 3; Active = $true },
            [pscustomobject]@{ Name = 'comma,value'; Quote = 'he said "yes"'; Empty = '' },
            [pscustomobject]@{ Name = 'unicode'; Text = "emoji 🚀 café"; NullValue = $null },
            [pscustomobject]@{ Name = 'astral'; Text = "emoji 🚀 𐐷"; Note = '𝄞' },
            [pscustomobject]@{ Name = 'multiline'; Text = "line1`nline2" }
        )

        foreach ($case in $cases) {
            $expected = @(ConvertTo-Csv -InputObject $case -NoTypeInformation)
            $actual = @(ConvertTo-RustCsv -InputObject $case -NoTypeInformation)

            Assert-StringSequenceEquivalent $actual $expected
        }
    }

    It 'matches ConvertTo-Csv for pipeline input rows' {
        $rows = @(
            [pscustomobject]@{ Name = 'one'; Count = 1 },
            [pscustomobject]@{ Name = 'two'; Count = 2 },
            [pscustomobject]@{ Name = 'three'; Count = 3 }
        )

        $expected = @($rows | ConvertTo-Csv -NoTypeInformation)
        $actual = @($rows | ConvertTo-RustCsv -NoTypeInformation)

        Assert-StringSequenceEquivalent $actual $expected
    }

    It 'matches ConvertTo-Csv for mixed-schema pipeline rows' {
        $cases = @(
            @(
                [pscustomobject]@{ Name = 'one'; Count = 1 },
                [pscustomobject]@{ Name = 'two'; Count = 2; Extra = 'later' }
            ),
            @(
                [pscustomobject]@{ Name = 'one'; Count = 1; Note = 'a' },
                [pscustomobject]@{ Name = 'two'; Count = 2 }
            ),
            @(
                [pscustomobject]@{ Name = 'one'; Note = $null },
                [pscustomobject]@{ Name = 'two'; Note = '' },
                [pscustomobject]@{ Name = 'three'; Note = '🚀' }
            )
        )

        foreach ($rows in $cases) {
            $expected = @($rows | ConvertTo-Csv -NoTypeInformation)
            $actual = @($rows | ConvertTo-RustCsv -NoTypeInformation)

            Assert-StringSequenceEquivalent $actual $expected
        }
    }

    It 'matches ConvertTo-Csv formatting parameters exposed by the host' {
        $row = [pscustomobject]@{ Name = 'rustlyn'; Note = 'comma,value'; Count = 3 }
        $astralRow = [pscustomobject]@{ Name = 'emoji 🚀'; Note = 'he said "go, now"'; Glyph = '𝄞' }

        Assert-StringSequenceEquivalent `
            @(ConvertTo-RustCsv -InputObject $row -NoTypeInformation -Delimiter ';') `
            @(ConvertTo-Csv -InputObject $row -NoTypeInformation -Delimiter ';')

        Assert-StringSequenceEquivalent `
            @(ConvertTo-RustCsv -InputObject $row -IncludeTypeInformation) `
            @(ConvertTo-Csv -InputObject $row -IncludeTypeInformation)

        Assert-StringSequenceEquivalent `
            @(ConvertTo-RustCsv -InputObject $row -NoTypeInformation -UseCulture) `
            @(ConvertTo-Csv -InputObject $row -NoTypeInformation -UseCulture)

        if (Test-CommandParameter 'ConvertTo-Csv' 'NoHeader') {
            Assert-StringSequenceEquivalent `
                @(ConvertTo-RustCsv -InputObject $row -NoTypeInformation -NoHeader) `
                @(ConvertTo-Csv -InputObject $row -NoTypeInformation -NoHeader)

            Assert-StringSequenceEquivalent `
                @(ConvertTo-RustCsv -InputObject $astralRow -NoTypeInformation -NoHeader) `
                @(ConvertTo-Csv -InputObject $astralRow -NoTypeInformation -NoHeader)
        }

        if (Test-CommandParameter 'ConvertTo-Csv' 'QuoteFields') {
            Assert-StringSequenceEquivalent `
                @(ConvertTo-RustCsv -InputObject $row -NoTypeInformation -QuoteFields Name) `
                @(ConvertTo-Csv -InputObject $row -NoTypeInformation -QuoteFields Name)

            Assert-StringSequenceEquivalent `
                @(ConvertTo-RustCsv -InputObject $astralRow -NoTypeInformation -QuoteFields Name) `
                @(ConvertTo-Csv -InputObject $astralRow -NoTypeInformation -QuoteFields Name)
        }

        if (Test-CommandParameter 'ConvertTo-Csv' 'UseQuotes') {
            foreach ($mode in 'Always', 'AsNeeded', 'Never') {
                $simple = [pscustomobject]@{ Name = 'rustlyn'; Count = 3 }
                Assert-StringSequenceEquivalent `
                    @(ConvertTo-RustCsv -InputObject $simple -NoTypeInformation -UseQuotes $mode) `
                    @(ConvertTo-Csv -InputObject $simple -NoTypeInformation -UseQuotes $mode)
            }

            foreach ($mode in 'AsNeeded', 'Never') {
                Assert-StringSequenceEquivalent `
                    @(ConvertTo-RustCsv -InputObject $astralRow -NoTypeInformation -UseQuotes $mode) `
                    @(ConvertTo-Csv -InputObject $astralRow -NoTypeInformation -UseQuotes $mode)
            }

            $literalQuote = [pscustomobject]@{ Name = 'rustlyn'; Note = 'literal"quote' }
            Assert-StringSequenceEquivalent `
                @(ConvertTo-RustCsv -InputObject $literalQuote -NoTypeInformation -UseQuotes Never) `
                @(ConvertTo-Csv -InputObject $literalQuote -NoTypeInformation -UseQuotes Never)

            $startsWithQuote = [pscustomobject]@{ Name = 'rustlyn'; Note = ([char]34 + 'starts') }
            Assert-StringSequenceEquivalent `
                @(ConvertTo-RustCsv -InputObject $startsWithQuote -NoTypeInformation -UseQuotes Never) `
                @(ConvertTo-Csv -InputObject $startsWithQuote -NoTypeInformation -UseQuotes Never)
        }
    }

    It 'matches ConvertFrom-Csv for standard, delimiter, header, unicode, empty, and multiline cases' {
        $cases = @(
            @{ Csv = "`"Name`",`"Count`"`n`"rustlyn`",`"3`""; Parameters = @{} },
            @{ Csv = "Name;Count`nrustlyn;3"; Parameters = @{ Delimiter = ';' } },
            @{ Csv = "rustlyn,3"; Parameters = @{ Header = @('Name', 'Count') } },
            @{ Csv = "`"Name`",`"Text`"`n`"unicode`",`"emoji 🚀 café`""; Parameters = @{} },
            @{ Csv = "`"Name`",`"Empty`"`n`"rustlyn`",`"`""; Parameters = @{} },
            @{ Csv = "`"Name`",`"Text`"`n`"multi`",`"line1`nline2`""; Parameters = @{} },
            @{ Csv = "`"Name`",`"Quote`"`n`"rustlyn`",`"he said `"`"yes`"`"`""; Parameters = @{} }
        )

        foreach ($case in $cases) {
            $expectedParameters = @{ InputObject = $case.Csv }
            $actualParameters = @{ InputObject = $case.Csv }
            foreach ($key in $case.Parameters.Keys) {
                $expectedParameters[$key] = $case.Parameters[$key]
                $actualParameters[$key] = $case.Parameters[$key]
            }

            $expected = ConvertFrom-Csv @expectedParameters
            $actual = ConvertFrom-RustCsv @actualParameters

            Assert-ObjectJsonEquivalent -Actual @($actual) -Expected @($expected)
        }
    }

    It 'matches ConvertFrom-Csv for pipeline text input' {
        $lines = @('"Name","Count"', '"one","1"', '"two","2"')

        $expected = $lines | ConvertFrom-Csv
        $actual = $lines | ConvertFrom-RustCsv

        Assert-ObjectJsonEquivalent -Actual @($actual) -Expected @($expected)
    }

    It 'matches ConvertFrom-Csv for blank pipeline records' {
        $lines = @('Name', '', 'value')

        $expected = $lines | ConvertFrom-Csv
        $actual = $lines | ConvertFrom-RustCsv

        Assert-ObjectJsonEquivalent -Actual @($actual) -Expected @($expected)
    }

    It 'matches ConvertFrom-Csv for empty input' {
        Assert-ObjectJsonEquivalent `
            -Actual @(ConvertFrom-RustCsv -InputObject '') `
            -Expected @(ConvertFrom-Csv -InputObject '')
    }

    It 'matches ConvertFrom-Csv for explicit string-array input and literal quotes in unquoted fields' {
        $lines = @('"Name","Count"', '"one","1"', '"two","2"')
        Assert-ObjectJsonEquivalent `
            -Actual @(ConvertFrom-RustCsv -InputObject $lines) `
            -Expected @(ConvertFrom-Csv -InputObject $lines)

        $literalQuoteCsv = "Name,Note`nrustlyn,literal`"quote"
        Assert-ObjectJsonEquivalent `
            -Actual @(ConvertFrom-RustCsv -InputObject $literalQuoteCsv) `
            -Expected @(ConvertFrom-Csv -InputObject $literalQuoteCsv)
    }

    It 'matches ConvertFrom-Csv for PowerShell-tolerated unbalanced quote forms' {
        $documents = @(
            "Name`n`"unterminated",
            "Name,Note`nrustlyn,`"starts"
        )

        foreach ($csv in $documents) {
            Assert-ObjectJsonEquivalent `
                -Actual @(ConvertFrom-RustCsv -InputObject $csv) `
                -Expected @(ConvertFrom-Csv -InputObject $csv)
        }
    }

    It 'roundtrips through Rust and built-in CSV cmdlets in both directions' {
        $rows = @(
            [pscustomobject]@{ Name = 'one'; Count = 1; Note = 'plain' },
            [pscustomobject]@{ Name = 'two'; Count = 2; Note = 'comma,value' },
            [pscustomobject]@{ Name = 'three'; Count = 3; Note = "line1`nline2" }
        )

        $rustCsv = @($rows | ConvertTo-RustCsv -NoTypeInformation)
        $baselineCsv = @($rows | ConvertTo-Csv -NoTypeInformation)

        Assert-StringSequenceEquivalent $rustCsv $baselineCsv
        Assert-ObjectJsonEquivalent -Actual @($rustCsv | ConvertFrom-Csv) -Expected @($baselineCsv | ConvertFrom-Csv)
        Assert-ObjectJsonEquivalent -Actual @($baselineCsv | ConvertFrom-RustCsv) -Expected @($baselineCsv | ConvertFrom-Csv)
        Assert-ObjectJsonEquivalent -Actual @($rustCsv | ConvertFrom-RustCsv) -Expected @($baselineCsv | ConvertFrom-Csv)
    }

}
