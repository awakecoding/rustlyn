param(
    [ValidateSet('Json', 'Yaml', 'Toml', 'Bson', 'Cbor', 'Csv')]
    [string[]]$Format = @('Json', 'Yaml', 'Toml', 'Bson', 'Cbor', 'Csv'),

    [string]$OutRoot = 'artifacts\scratch\rust_format_powershell_modules',

    [string]$Toolchain = 'nightly',

    [string]$BuildStd = 'std,panic_abort',

    [switch]$SkipBuild
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$outRootPath = if ([System.IO.Path]::IsPathRooted($OutRoot)) { $OutRoot } else { Join-Path $repoRoot $OutRoot }
$outRootPath = [System.IO.Path]::GetFullPath($outRootPath)

$formats = @{
    Json = @{
        BuildScript = 'Build-SimdJsonPowerShellModule.ps1'
        OutDir = 'simd_json'
        Manifest = 'Rustlyn.SimdJson.PowerShell.psd1'
    }
    Yaml = @{
        BuildScript = 'Build-MarkedYamlPowerShellModule.ps1'
        OutDir = 'marked_yaml'
        Manifest = 'Rustlyn.MarkedYaml.PowerShell.psd1'
    }
    Toml = @{
        BuildScript = 'Build-TomlPowerShellModule.ps1'
        OutDir = 'toml'
        Manifest = 'Rustlyn.Toml.PowerShell.psd1'
    }
    Bson = @{
        BuildScript = 'Build-BsonPowerShellModule.ps1'
        OutDir = 'bson'
        Manifest = 'Rustlyn.Bson.PowerShell.psd1'
    }
    Cbor = @{
        BuildScript = 'Build-CborPowerShellModule.ps1'
        OutDir = 'cbor'
        Manifest = 'Rustlyn.Cbor.PowerShell.psd1'
    }
    Csv = @{
        BuildScript = 'Build-CsvPowerShellModule.ps1'
        OutDir = 'csv'
        Manifest = 'Rustlyn.Csv.PowerShell.psd1'
    }
}

$smokeScript = Join-Path $outRootPath 'Invoke-RustFormatPowerShellModuleSmoke.ps1'
New-Item -ItemType Directory -Path $outRootPath -Force | Out-Null
@'
param(
    [Parameter(Mandatory = $true)]
    [string]$ManifestPath,

    [Parameter(Mandatory = $true)]
    [ValidateSet('Json', 'Yaml', 'Toml', 'Bson', 'Cbor', 'Csv')]
    [string]$Format
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module $ManifestPath -Force

function Assert-Condition {
    param(
        [bool]$Condition,
        [string]$Message
    )

    if (-not $Condition) {
        throw $Message
    }
}

function Get-RustFormatValue {
    param(
        [object]$InputObject,
        [string]$Name
    )

    if ($InputObject -is [System.Collections.IDictionary]) {
        return $InputObject[$Name]
    }

    return $InputObject.$Name
}

function Assert-Throws {
    param(
        [scriptblock]$ScriptBlock,
        [string]$Message
    )

    try {
        & $ScriptBlock | Out-Null
    }
    catch {
        return
    }

    throw $Message
}

$sample = [pscustomobject]@{
    name = 'rustlyn'
    count = 3
}

switch ($Format) {
    'Json' {
        $text = $sample | ConvertTo-RustJson -Compress
        Assert-Condition $text.Contains('"name":"rustlyn"') "Unexpected JSON output: $text"
        $largeJson = 1..64 | ForEach-Object { [pscustomobject]@{ name = "item$_"; count = $_ } } | ConvertTo-RustJson -Compress
        Assert-Condition $largeJson.Contains('"name":"item64"') "Unexpected large JSON output: $largeJson"
        $decoded = '{"name":"rustlyn","count":3}' | ConvertFrom-RustJson -AsHashtable
        Assert-Condition ((Get-RustFormatValue $decoded 'name') -eq 'rustlyn') 'Unexpected JSON decode result.'
        $array = '[{"name":"rustlyn"}]' | ConvertFrom-RustJson -AsHashtable -NoEnumerate
        Assert-Condition (($array.Count -eq 1) -and ((Get-RustFormatValue $array[0] 'name') -eq 'rustlyn')) 'Unexpected JSON NoEnumerate result.'
        Assert-Throws { '{' | ConvertFrom-RustJson -ErrorAction Stop } 'Malformed JSON did not fail.'
    }
    'Yaml' {
        $text = $sample | ConvertTo-RustYaml
        Assert-Condition $text.Contains('rustlyn') "Unexpected YAML output: $text"
        $decoded = "name: rustlyn`ncount: 3" | ConvertFrom-RustYaml -AsHashtable
        Assert-Condition ((Get-RustFormatValue $decoded 'name') -eq 'rustlyn') 'Unexpected YAML decode result.'
        Assert-Throws { 'name: [unterminated' | ConvertFrom-RustYaml -ErrorAction Stop } 'Malformed YAML did not fail.'
    }
    'Toml' {
        $text = $sample | ConvertTo-RustToml
        Assert-Condition $text.Contains('name = "rustlyn"') "Unexpected TOML output: $text"
        $decoded = 'name = "rustlyn"' | ConvertFrom-RustToml
        Assert-Condition ((Get-RustFormatValue $decoded 'name') -eq 'rustlyn') 'Unexpected TOML decode result.'
        Assert-Throws { "[nested]`nname = `"rustlyn`"" | ConvertFrom-RustToml -ErrorAction Stop } 'Unsupported TOML table input did not fail.'
    }
    'Bson' {
        $bytes = $sample | ConvertTo-RustBson
        Assert-Condition ($bytes -is [byte[]]) "BSON output was not a single byte array: $($bytes.GetType().FullName)"
        $decoded = ConvertFrom-RustBson -InputObject $bytes -AsHashtable
        Assert-Condition ((Get-RustFormatValue $decoded 'name') -eq 'rustlyn') 'Unexpected BSON decode result.'
        Assert-Throws { ConvertFrom-RustBson -InputObject ([byte[]](1, 2, 3)) -ErrorAction Stop } 'Truncated BSON did not fail.'
    }
    'Cbor' {
        $bytes = $sample | ConvertTo-RustCbor
        Assert-Condition ($bytes -is [byte[]]) "CBOR output was not a single byte array: $($bytes.GetType().FullName)"
        $decoded = ConvertFrom-RustCbor -InputObject $bytes -AsHashtable
        Assert-Condition ((Get-RustFormatValue $decoded 'name') -eq 'rustlyn') 'Unexpected CBOR decode result.'
        Assert-Throws { ConvertFrom-RustCbor -InputObject ([byte[]](255)) -ErrorAction Stop } 'Malformed CBOR did not fail.'
    }
    'Csv' {
        $lines = @($sample | ConvertTo-RustCsv)
        Assert-Condition (($lines -join "`n").Contains('rustlyn')) "Unexpected CSV output: $($lines -join '|')"
        $decoded = ConvertFrom-RustCsv -InputObject @('name,count', 'rustlyn,3')
        Assert-Condition ((Get-RustFormatValue $decoded 'name') -eq 'rustlyn') 'Unexpected CSV decode result.'
        $semicolonLines = @($sample | ConvertTo-RustCsv -Delimiter ';')
        Assert-Condition ((($semicolonLines -join "`n").Contains('rustlyn')) -and (($semicolonLines -join "`n").Contains(';'))) "Unexpected semicolon CSV output: $($semicolonLines -join '|')"
        $headerDecoded = ConvertFrom-RustCsv -InputObject 'rustlyn;3' -Header @('name', 'count') -Delimiter ';'
        Assert-Condition ((Get-RustFormatValue $headerDecoded 'name') -eq 'rustlyn') 'Unexpected CSV Header decode result.'
        $noHeaderLines = @($sample | ConvertTo-RustCsv -NoHeader)
        Assert-Condition (($noHeaderLines.Count -eq 1) -and $noHeaderLines[0].Contains('rustlyn')) "Unexpected CSV NoHeader output: $($noHeaderLines -join '|')"
    }
}
'@ | Set-Content -LiteralPath $smokeScript -Encoding UTF8

foreach ($formatName in $Format) {
    $formatInfo = $formats[$formatName]
    $moduleOutDir = Join-Path $outRootPath $formatInfo.OutDir
    if (-not $SkipBuild) {
        if (Test-Path $moduleOutDir) {
            Remove-Item -LiteralPath $moduleOutDir -Recurse -Force
        }

        Write-Host "Building $formatName PowerShell module..."
        & (Join-Path $PSScriptRoot $formatInfo.BuildScript) -OutDir $moduleOutDir -Toolchain $Toolchain -BuildStd $BuildStd | Out-Host
        if ($LASTEXITCODE -ne 0) {
            throw "Failed to build $formatName PowerShell module."
        }
    }

    $manifestPath = Join-Path $moduleOutDir $formatInfo.Manifest
    if (-not (Test-Path $manifestPath)) {
        throw "Expected module manifest '$manifestPath' to exist."
    }

    $enginePath = Join-Path $moduleOutDir 'rustlyn_powershell_format_cmdlets.dll'
    if (-not (Test-Path $enginePath)) {
        throw "Expected unified Rust format engine '$enginePath' to exist."
    }

    Write-Host "Smoking $formatName PowerShell module..."
    pwsh -NoLogo -NoProfile -File $smokeScript -ManifestPath $manifestPath -Format $formatName
    if ($LASTEXITCODE -ne 0) {
        throw "Smoke test failed for $formatName PowerShell module."
    }
}

Write-Output "Rust format PowerShell module smokes passed: $($Format -join ', ')"
