param(
    [ValidateSet('Json', 'Yaml', 'Toml', 'Bson', 'Cbor', 'Csv', 'Xml')]
    [string[]]$Format = @('Json', 'Yaml', 'Toml', 'Bson', 'Cbor', 'Csv', 'Xml'),

    [string]$OutRoot = 'artifacts\scratch\rust_format_benchmarks',

    [string]$Toolchain = 'nightly',

    [string]$BuildStd = 'std,panic_abort',

    [int]$WarmupIterations = 1,

    [int]$MeasureIterations = 5,

    [int]$FlatRowCount = 128,

    [int]$DeepRowCount = 32,

    [int]$DeepDepth = 10,

    [switch]$SkipBuild,

    [string]$ChildManifestPath,

    [string]$ChildFormatName,

    [string]$ChildScenario
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
        ToCommand = 'ConvertTo-RustJson'
        FromCommand = 'ConvertFrom-RustJson'
        ToArgs = @{ Compress = $true }
        DeepToArgs = @{ Compress = $true }
    }
    Yaml = @{
        BuildScript = 'Build-MarkedYamlPowerShellModule.ps1'
        OutDir = 'marked_yaml'
        Manifest = 'Rustlyn.MarkedYaml.PowerShell.psd1'
        ToCommand = 'ConvertTo-RustYaml'
        FromCommand = 'ConvertFrom-RustYaml'
        ToArgs = @{}
        DeepToArgs = @{}
    }
    Toml = @{
        BuildScript = 'Build-TomlPowerShellModule.ps1'
        OutDir = 'toml'
        Manifest = 'Rustlyn.Toml.PowerShell.psd1'
        ToCommand = 'ConvertTo-RustToml'
        FromCommand = 'ConvertFrom-RustToml'
        ToArgs = @{}
        DeepToArgs = @{}
    }
    Bson = @{
        BuildScript = 'Build-BsonPowerShellModule.ps1'
        OutDir = 'bson'
        Manifest = 'Rustlyn.Bson.PowerShell.psd1'
        ToCommand = 'ConvertTo-RustBson'
        FromCommand = 'ConvertFrom-RustBson'
        ToArgs = @{}
        DeepToArgs = @{}
    }
    Cbor = @{
        BuildScript = 'Build-CborPowerShellModule.ps1'
        OutDir = 'cbor'
        Manifest = 'Rustlyn.Cbor.PowerShell.psd1'
        ToCommand = 'ConvertTo-RustCbor'
        FromCommand = 'ConvertFrom-RustCbor'
        ToArgs = @{}
        DeepToArgs = @{}
    }
    Csv = @{
        BuildScript = 'Build-CsvPowerShellModule.ps1'
        OutDir = 'csv'
        Manifest = 'Rustlyn.Csv.PowerShell.psd1'
        ToCommand = 'ConvertTo-RustCsv'
        FromCommand = 'ConvertFrom-RustCsv'
        ToArgs = @{ NoTypeInformation = $true }
        DeepToArgs = @{ NoTypeInformation = $true }
    }
    Xml = @{
        BuildScript = 'Build-QuickXmlPowerShellModule.ps1'
        OutDir = 'quick_xml'
        Manifest = 'Rustlyn.QuickXml.PowerShell.psd1'
        ToCommand = 'ConvertTo-RustXml'
        FromCommand = 'ConvertFrom-RustXml'
        ToArgs = @{ As = 'String'; NoTypeInformation = $true }
        DeepToArgs = @{ As = 'String'; NoTypeInformation = $true }
    }
}

function New-FlatRows {
    param([int]$Count)

    $rows = [System.Collections.Generic.List[object]]::new()
    for ($index = 0; $index -lt $Count; $index++) {
        $rows.Add([pscustomobject][ordered]@{
            Id = $index
            Name = "item$index"
            Active = (($index % 2) -eq 0)
            Score = [math]::Round(($index + 1) * 1.25, 2)
            Note = "row-$index"
        })
    }

    return $rows.ToArray()
}

function New-DeepNode {
    param(
        [int]$Depth,
        [int]$Seed
    )

    if ($Depth -le 0) {
        return [pscustomobject][ordered]@{
            Seed = $Seed
            Label = "leaf-$Seed"
            Flags = @($true, $false, ($Seed % 2) -eq 0)
        }
    }

    return [pscustomobject][ordered]@{
        Level = $Depth
        Seed = $Seed
        Meta = [ordered]@{
            Name = "node-$Depth-$Seed"
            Weight = [math]::Round(($Depth * 10) + ($Seed / 10.0), 2)
            Tags = @("d$Depth", "s$Seed", 'rust')
        }
        Items = @(
            [pscustomobject][ordered]@{ Kind = 'left'; Value = $Depth + $Seed },
            [pscustomobject][ordered]@{ Kind = 'right'; Value = ($Depth * 2) + $Seed }
        )
        Child = New-DeepNode -Depth ($Depth - 1) -Seed ($Seed + 1)
    }
}

function New-DeepRows {
    param(
        [int]$Count,
        [int]$Depth
    )

    $rows = [System.Collections.Generic.List[object]]::new()
    for ($index = 0; $index -lt $Count; $index++) {
        $rows.Add([pscustomobject][ordered]@{
            Row = $index
            Root = New-DeepNode -Depth $Depth -Seed $index
        })
    }

    return $rows.ToArray()
}

function New-TomlFlatDocument {
    param([int]$Count)

    $document = [ordered]@{}
    for ($index = 0; $index -lt $Count; $index++) {
        $document["item$index"] = "value-$index"
    }

    return $document
}

function New-TomlDeepValue {
    param(
        [int]$Depth,
        [int]$Seed
    )

    if ($Depth -le 0) {
        return "leaf-$Seed"
    }

    return @((New-TomlDeepValue -Depth ($Depth - 1) -Seed $Seed))
}

function New-TomlDeepDocument {
    param(
        [int]$Count,
        [int]$Depth
    )

    $document = [ordered]@{}
    for ($index = 0; $index -lt $Count; $index++) {
        $document["item$index"] = New-TomlDeepValue -Depth $Depth -Seed $index
    }

    return $document
}

function New-YamlFlatDocument {
    param([int]$Count)

    $document = [ordered]@{}
    for ($index = 0; $index -lt $Count; $index++) {
        $document["item$index"] = "value-$index"
    }

    return $document
}

function New-YamlDeepNode {
    param(
        [int]$Depth,
        [int]$Seed
    )

    if ($Depth -le 0) {
        return [ordered]@{ label = "leaf-$Seed" }
    }

    return [ordered]@{
        name = "node-$Depth-$Seed"
        child = New-YamlDeepNode -Depth ($Depth - 1) -Seed $Seed
    }
}

function New-YamlDeepDocument {
    param(
        [int]$Count,
        [int]$Depth
    )

    $document = [ordered]@{}
    for ($index = 0; $index -lt $Count; $index++) {
        $document["item$index"] = New-YamlDeepNode -Depth $Depth -Seed $index
    }

    return $document
}

function New-BinaryFlatDocument {
    param([int]$Count)

    $document = [ordered]@{}
    for ($index = 0; $index -lt $Count; $index++) {
        switch ($index % 3) {
            0 { $document["item$index"] = $index }
            1 { $document["item$index"] = "value-$index" }
            default { $document["item$index"] = (($index % 2) -eq 0) }
        }
    }

    return $document
}

function New-BinaryDeepNode {
    param(
        [int]$Depth,
        [int]$Seed
    )

    if ($Depth -le 0) {
        return [ordered]@{ value = "leaf-$Seed" }
    }

    return [ordered]@{
        name = "node-$Depth-$Seed"
        child = New-BinaryDeepNode -Depth ($Depth - 1) -Seed $Seed
    }
}

function New-BinaryDeepDocument {
    param(
        [int]$Count,
        [int]$Depth
    )

    $document = [ordered]@{}
    for ($index = 0; $index -lt $Count; $index++) {
        $document["item$index"] = New-BinaryDeepNode -Depth $Depth -Seed $index
    }

    return $document
}

function Invoke-RustBenchmark {
    param(
        [string]$FormatName,
        [string]$Scenario,
        [scriptblock]$Action,
        [int]$WarmupCount,
        [int]$IterationCount,
        [int]$InputCount,
        [int]$Depth
    )

    for ($index = 0; $index -lt $WarmupCount; $index++) {
        & $Action | Out-Null
    }

    [System.GC]::Collect()
    [System.GC]::WaitForPendingFinalizers()
    [System.GC]::Collect()

    $elapsed = [System.Collections.Generic.List[double]]::new()
    for ($index = 0; $index -lt $IterationCount; $index++) {
        $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
        & $Action | Out-Null
        $stopwatch.Stop()
        $elapsed.Add($stopwatch.Elapsed.TotalMilliseconds)
    }

    $samples = $elapsed.ToArray()
    $average = ($samples | Measure-Object -Average).Average
    $minimum = ($samples | Measure-Object -Minimum).Minimum
    $maximum = ($samples | Measure-Object -Maximum).Maximum

    [pscustomobject]@{
        Format = $FormatName
        Scenario = $Scenario
        InputCount = $InputCount
        Depth = $Depth
        Iterations = $IterationCount
        AverageMs = [math]::Round($average, 3)
        MinMs = [math]::Round($minimum, 3)
        MaxMs = [math]::Round($maximum, 3)
        Status = 'Ok'
        Detail = ''
    }
}

function Invoke-RustBenchmarkSafely {
    param(
        [string]$FormatName,
        [string]$Scenario,
        [scriptblock]$Action,
        [int]$WarmupCount,
        [int]$IterationCount,
        [int]$InputCount,
        [int]$Depth
    )

    try {
        return Invoke-RustBenchmark `
            -FormatName $FormatName `
            -Scenario $Scenario `
            -Action $Action `
            -WarmupCount $WarmupCount `
            -IterationCount $IterationCount `
            -InputCount $InputCount `
            -Depth $Depth
    }
    catch {
        return [pscustomobject]@{
            Format = $FormatName
            Scenario = $Scenario
            InputCount = $InputCount
            Depth = $Depth
            Iterations = $IterationCount
            AverageMs = $null
            MinMs = $null
            MaxMs = $null
            Status = 'Failed'
            Detail = $_.Exception.Message
        }
    }
}

function New-RustFormatDocument {
    param(
        [string]$FormatName,
        [hashtable]$FormatInfo,
        [object]$InputValue,
        [int]$Depth
    )

    $toCommand = Get-Command $FormatInfo.ToCommand -ErrorAction Stop
    $parameters = @{}
    foreach ($key in $FormatInfo.ToArgs.Keys) {
        $parameters[$key] = $FormatInfo.ToArgs[$key]
    }
    if ($toCommand.Parameters.ContainsKey('Depth')) {
        $parameters['Depth'] = $Depth
    }

    switch ($FormatName) {
        'Bson' { Write-Output -NoEnumerate (& $toCommand -InputObject $InputValue @parameters); return }
        'Cbor' { Write-Output -NoEnumerate (& $toCommand -InputObject $InputValue @parameters); return }
        'Yaml' { return (& $toCommand -InputObject $InputValue @parameters) }
        'Toml' { return (& $toCommand -InputObject $InputValue @parameters) }
        default { return @($InputValue | & $toCommand @parameters) }
    }
}

function New-BenchmarkInputValue {
    param(
        [string]$FormatName,
        [ValidateSet('Flat', 'Deep')]
        [string]$Shape,
        [object[]]$FlatRows,
        [object[]]$DeepRows,
        [int]$RequestedDeepDepth
    )

    switch ($FormatName) {
        'Yaml' {
            if ($Shape -eq 'Flat') {
                return (New-YamlFlatDocument -Count $FlatRows.Count)
            }

            return (New-YamlDeepDocument -Count $DeepRows.Count -Depth $RequestedDeepDepth)
        }
        'Toml' {
            if ($Shape -eq 'Flat') {
                return (New-TomlFlatDocument -Count $FlatRows.Count)
            }

            return (New-TomlDeepDocument -Count $DeepRows.Count -Depth $RequestedDeepDepth)
        }
        'Bson' {
            if ($Shape -eq 'Flat') {
                return (New-BinaryFlatDocument -Count $FlatRows.Count)
            }

            return (New-BinaryDeepDocument -Count $DeepRows.Count -Depth $RequestedDeepDepth)
        }
        'Cbor' {
            if ($Shape -eq 'Flat') {
                return (New-BinaryFlatDocument -Count $FlatRows.Count)
            }

            return (New-BinaryDeepDocument -Count $DeepRows.Count -Depth $RequestedDeepDepth)
        }
        default {
            if ($Shape -eq 'Flat') {
                return $FlatRows
            }

            return $DeepRows
        }
    }
}

function Invoke-RustFormatConvertTo {
    param(
        [string]$FormatName,
        [System.Management.Automation.CommandInfo]$Command,
        [hashtable]$Parameters,
        [object]$InputValue
    )

    switch ($FormatName) {
        'Yaml' { return (& $Command -InputObject $InputValue @Parameters) }
        'Toml' { return (& $Command -InputObject $InputValue @Parameters) }
        'Bson' { return (& $Command -InputObject $InputValue @Parameters) }
        'Cbor' { return (& $Command -InputObject $InputValue @Parameters) }
        default { return ($InputValue | & $Command @Parameters) }
    }
}

function Invoke-RustFormatConvertFrom {
    param(
        [string]$FormatName,
        [System.Management.Automation.CommandInfo]$Command,
        [object]$InputValue
    )

    switch ($FormatName) {
        'Bson' { return (& $Command -InputObject $InputValue) }
        'Cbor' { return (& $Command -InputObject $InputValue) }
        default { return ($InputValue | & $Command) }
    }
}

function Get-DepthForCommand {
    param(
        [System.Management.Automation.CommandInfo]$Command,
        [int]$RequestedDepth
    )

    if ($Command.Parameters.ContainsKey('Depth')) {
        return $RequestedDepth
    }

    return 0
}

function Invoke-ChildBenchmarkScenario {
    param(
        [string]$ManifestPath,
        [string]$FormatName,
        [string]$Scenario,
        [int]$WarmupCount,
        [int]$IterationCount,
        [int]$FlatCount,
        [int]$DeepCount,
        [int]$RequestedDeepDepth
    )

    $formatInfo = $formats[$FormatName]
    Import-Module $ManifestPath -Force -ErrorAction Stop

    $toCommand = Get-Command $formatInfo.ToCommand -ErrorAction Stop
    $fromCommand = Get-Command $formatInfo.FromCommand -ErrorAction Stop
    $flatRows = New-FlatRows -Count $FlatCount
    $deepRows = New-DeepRows -Count $DeepCount -Depth $RequestedDeepDepth
    $flatInputValue = New-BenchmarkInputValue -FormatName $FormatName -Shape Flat -FlatRows $flatRows -DeepRows $deepRows -RequestedDeepDepth $RequestedDeepDepth
    $deepInputValue = New-BenchmarkInputValue -FormatName $FormatName -Shape Deep -FlatRows $flatRows -DeepRows $deepRows -RequestedDeepDepth $RequestedDeepDepth
    $flatDepth = [Math]::Max(4, [Math]::Min($RequestedDeepDepth, 8))

    $flatToArgs = @{}
    foreach ($key in $formatInfo.ToArgs.Keys) {
        $flatToArgs[$key] = $formatInfo.ToArgs[$key]
    }
    if ($toCommand.Parameters.ContainsKey('Depth')) {
        $flatToArgs['Depth'] = $flatDepth
    }

    $deepToArgs = @{}
    foreach ($key in $formatInfo.DeepToArgs.Keys) {
        $deepToArgs[$key] = $formatInfo.DeepToArgs[$key]
    }
    if ($toCommand.Parameters.ContainsKey('Depth')) {
        $deepToArgs['Depth'] = $RequestedDeepDepth
    }

    switch ($Scenario) {
        'ConvertTo-Flat' {
            return Invoke-RustBenchmarkSafely -FormatName $FormatName -Scenario $Scenario -Action {
                Invoke-RustFormatConvertTo -FormatName $FormatName -Command $toCommand -Parameters $flatToArgs -InputValue $flatInputValue | Out-Null
            } -WarmupCount $WarmupCount -IterationCount $IterationCount -InputCount $FlatCount -Depth (Get-DepthForCommand -Command $toCommand -RequestedDepth $flatDepth)
        }
        'ConvertFrom-Flat' {
            $flatDocument = New-RustFormatDocument -FormatName $FormatName -FormatInfo $formatInfo -InputValue $flatInputValue -Depth $flatDepth
            return Invoke-RustBenchmarkSafely -FormatName $FormatName -Scenario $Scenario -Action {
                Invoke-RustFormatConvertFrom -FormatName $FormatName -Command $fromCommand -InputValue $flatDocument | Out-Null
            } -WarmupCount $WarmupCount -IterationCount $IterationCount -InputCount $FlatCount -Depth (Get-DepthForCommand -Command $toCommand -RequestedDepth $flatDepth)
        }
        'ConvertTo-Deep' {
            return Invoke-RustBenchmarkSafely -FormatName $FormatName -Scenario $Scenario -Action {
                Invoke-RustFormatConvertTo -FormatName $FormatName -Command $toCommand -Parameters $deepToArgs -InputValue $deepInputValue | Out-Null
            } -WarmupCount $WarmupCount -IterationCount $IterationCount -InputCount $DeepCount -Depth (Get-DepthForCommand -Command $toCommand -RequestedDepth $RequestedDeepDepth)
        }
        'ConvertFrom-Deep' {
            $deepDocument = New-RustFormatDocument -FormatName $FormatName -FormatInfo $formatInfo -InputValue $deepInputValue -Depth $RequestedDeepDepth
            return Invoke-RustBenchmarkSafely -FormatName $FormatName -Scenario $Scenario -Action {
                Invoke-RustFormatConvertFrom -FormatName $FormatName -Command $fromCommand -InputValue $deepDocument | Out-Null
            } -WarmupCount $WarmupCount -IterationCount $IterationCount -InputCount $DeepCount -Depth (Get-DepthForCommand -Command $toCommand -RequestedDepth $RequestedDeepDepth)
        }
        default {
            throw "Unknown child benchmark scenario '$Scenario'."
        }
    }
}

function Invoke-IsolatedRustBenchmark {
    param(
        [string]$ManifestPath,
        [string]$FormatName,
        [string]$Scenario,
        [int]$WarmupCount,
        [int]$IterationCount,
        [int]$FlatCount,
        [int]$DeepCount,
        [int]$RequestedDeepDepth
    )

    $pwsh = (Get-Command pwsh -CommandType Application -ErrorAction Stop | Select-Object -First 1 -ExpandProperty Source)
    $output = @(
        & $pwsh -NoLogo -NoProfile -File $PSCommandPath `
            -SkipBuild `
            -WarmupIterations $WarmupCount `
            -MeasureIterations $IterationCount `
            -FlatRowCount $FlatCount `
            -DeepRowCount $DeepCount `
            -DeepDepth $RequestedDeepDepth `
            -ChildManifestPath $ManifestPath `
            -ChildFormatName $FormatName `
            -ChildScenario $Scenario 2>&1
    )
    $exitCode = $LASTEXITCODE

    if ($exitCode -eq 0 -and $output.Count -gt 0) {
        try {
            return $output[-1] | ConvertFrom-Json -ErrorAction Stop
        }
        catch {
            return [pscustomobject]@{
                Format = $FormatName
                Scenario = $Scenario
                InputCount = if ($Scenario -like '*Deep*') { $DeepCount } else { $FlatCount }
                Depth = if ($Scenario -like '*Deep*') { $RequestedDeepDepth } else { [Math]::Max(4, [Math]::Min($RequestedDeepDepth, 8)) }
                Iterations = $IterationCount
                AverageMs = $null
                MinMs = $null
                MaxMs = $null
                Status = 'Failed'
                Detail = ($output -join "`n")
            }
        }
    }

    return [pscustomobject]@{
        Format = $FormatName
        Scenario = $Scenario
        InputCount = if ($Scenario -like '*Deep*') { $DeepCount } else { $FlatCount }
        Depth = if ($Scenario -like '*Deep*') { $RequestedDeepDepth } else { [Math]::Max(4, [Math]::Min($RequestedDeepDepth, 8)) }
        Iterations = $IterationCount
        AverageMs = $null
        MinMs = $null
        MaxMs = $null
        Status = 'Failed'
        Detail = if ($output.Count -gt 0) { $output -join "`n" } else { "Child benchmark process exited with code $exitCode." }
    }
}

if ($ChildScenario) {
    $result = Invoke-ChildBenchmarkScenario `
        -ManifestPath $ChildManifestPath `
        -FormatName $ChildFormatName `
        -Scenario $ChildScenario `
        -WarmupCount $WarmupIterations `
        -IterationCount $MeasureIterations `
        -FlatCount $FlatRowCount `
        -DeepCount $DeepRowCount `
        -RequestedDeepDepth $DeepDepth
    $result | ConvertTo-Json -Compress -Depth 6
    return
}

New-Item -ItemType Directory -Path $outRootPath -Force | Out-Null
$results = [System.Collections.Generic.List[object]]::new()

foreach ($formatName in $Format) {
    $formatInfo = $formats[$formatName]
    $moduleOutDir = Join-Path $outRootPath $formatInfo.OutDir

    if (-not $SkipBuild) {
        if (Test-Path $moduleOutDir) {
            Remove-Item -LiteralPath $moduleOutDir -Recurse -Force
        }

        & (Join-Path $PSScriptRoot $formatInfo.BuildScript) -OutDir $moduleOutDir -Toolchain $Toolchain -BuildStd $BuildStd | Out-Host
        if ($LASTEXITCODE -ne 0) {
            throw "Failed to build $formatName PowerShell module."
        }
    }

    $manifestPath = Join-Path $moduleOutDir $formatInfo.Manifest
    if (-not (Test-Path $manifestPath)) {
        throw "Expected module manifest '$manifestPath' to exist."
    }

    $results.Add((Invoke-IsolatedRustBenchmark -ManifestPath $manifestPath -FormatName $formatName -Scenario 'ConvertTo-Flat' -WarmupCount $WarmupIterations -IterationCount $MeasureIterations -FlatCount $FlatRowCount -DeepCount $DeepRowCount -RequestedDeepDepth $DeepDepth))
    $results.Add((Invoke-IsolatedRustBenchmark -ManifestPath $manifestPath -FormatName $formatName -Scenario 'ConvertFrom-Flat' -WarmupCount $WarmupIterations -IterationCount $MeasureIterations -FlatCount $FlatRowCount -DeepCount $DeepRowCount -RequestedDeepDepth $DeepDepth))
    $results.Add((Invoke-IsolatedRustBenchmark -ManifestPath $manifestPath -FormatName $formatName -Scenario 'ConvertTo-Deep' -WarmupCount $WarmupIterations -IterationCount $MeasureIterations -FlatCount $FlatRowCount -DeepCount $DeepRowCount -RequestedDeepDepth $DeepDepth))
    $results.Add((Invoke-IsolatedRustBenchmark -ManifestPath $manifestPath -FormatName $formatName -Scenario 'ConvertFrom-Deep' -WarmupCount $WarmupIterations -IterationCount $MeasureIterations -FlatCount $FlatRowCount -DeepCount $DeepRowCount -RequestedDeepDepth $DeepDepth))
}

$outputPath = Join-Path $outRootPath 'rust-format-benchmark-results.json'
$results | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $outputPath -Encoding UTF8
$results | Sort-Object Format, Scenario | Format-Table Format, Scenario, InputCount, Depth, AverageMs, MinMs, MaxMs, Status -AutoSize
Write-Host "Saved benchmark results to $outputPath"