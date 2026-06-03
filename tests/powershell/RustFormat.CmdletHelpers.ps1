Set-StrictMode -Version Latest

function Get-RustlynRepoRoot {
    $root = Resolve-Path (Join-Path $PSScriptRoot '..\..')
    return $root.Path
}

function Build-RustlynPowerShellModule {
    param(
        [Parameter(Mandatory)]
        [string]$BuildScriptName,

        [Parameter(Mandatory)]
        [string]$ManifestRelativePath
    )

    $repoRoot = Get-RustlynRepoRoot
    $buildScript = Join-Path $repoRoot "scripts\$BuildScriptName"
    if (!(Test-Path $buildScript)) {
        throw "Build script '$buildScript' was not found."
    }

    & $buildScript | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw "Build script '$buildScript' failed with exit code $LASTEXITCODE."
    }

    $manifestPath = Join-Path $repoRoot $ManifestRelativePath
    if (!(Test-Path $manifestPath)) {
        throw "Module manifest '$manifestPath' was not produced."
    }

    return (Resolve-Path $manifestPath).Path
}

function Import-RustlynPowerShellModule {
    param(
        [Parameter(Mandatory)]
        [string]$ManifestPath
    )

    Import-Module $ManifestPath -Force -ErrorAction Stop
}

function Test-CommandParameter {
    param(
        [Parameter(Mandatory)]
        [string]$CommandName,

        [Parameter(Mandatory)]
        [string]$ParameterName
    )

    $command = Get-Command $CommandName -ErrorAction Stop
    return $command.Parameters.ContainsKey($ParameterName)
}

function ConvertTo-CanonicalXml {
    param(
        [Parameter(Mandatory)]
        [string]$Xml
    )

    [xml]$document = $Xml
    return $document.OuterXml
}

function ConvertTo-XmlString {
    param(
        [Parameter(Mandatory, ValueFromPipeline)]
        [object]$InputObject
    )

    process {
        if ($InputObject -is [xml]) {
            return $InputObject.OuterXml
        }

        if ($InputObject -is [System.IO.Stream]) {
            $position = $InputObject.Position
            $InputObject.Position = 0
            try {
                $reader = [System.IO.StreamReader]::new($InputObject, [System.Text.Encoding]::UTF8, $true, 1024, $true)
                try {
                    return $reader.ReadToEnd()
                }
                finally {
                    $reader.Dispose()
                }
            }
            finally {
                if ($InputObject.CanSeek) {
                    $InputObject.Position = $position
                }
            }
        }

        return [string]$InputObject
    }
}

function ConvertTo-CanonicalJson {
    param(
        [Parameter(ValueFromPipeline)]
        [AllowNull()]
        [object]$InputObject
    )

    begin {
        $items = [System.Collections.Generic.List[object]]::new()
    }

    process {
        $items.Add($InputObject)
    }

    end {
        if ($items.Count -eq 1) {
            ConvertTo-Json -InputObject (ConvertTo-NormalizedJsonValue $items[0]) -Depth 32 -Compress
            return
        }

        ConvertTo-Json -InputObject (ConvertTo-NormalizedJsonValue $items.ToArray()) -Depth 32 -Compress
    }
}

function ConvertTo-NormalizedJsonValue {
    param(
        [AllowNull()]
        [object]$InputObject
    )

    if ($null -eq $InputObject) {
        return $null
    }

    if ($InputObject -is [string] -or $InputObject -is [char] -or $InputObject -is [bool] -or
        $InputObject -is [byte] -or $InputObject -is [sbyte] -or $InputObject -is [short] -or
        $InputObject -is [ushort] -or $InputObject -is [int] -or $InputObject -is [uint] -or
        $InputObject -is [long] -or $InputObject -is [ulong] -or $InputObject -is [float] -or
        $InputObject -is [double] -or $InputObject -is [decimal] -or $InputObject -is [datetime] -or
        $InputObject -is [datetimeoffset] -or $InputObject -is [guid]) {
        return $InputObject
    }

    if ($InputObject -is [System.Collections.IDictionary]) {
        $normalized = [ordered]@{}
        foreach ($key in @($InputObject.Keys | Sort-Object)) {
            $normalized[$key] = ConvertTo-NormalizedJsonValue $InputObject[$key]
        }
        return $normalized
    }

    if ($InputObject -is [System.Collections.IEnumerable] -and $InputObject -isnot [string]) {
        $items = [System.Collections.Generic.List[object]]::new()
        foreach ($item in $InputObject) {
            $items.Add((ConvertTo-NormalizedJsonValue $item))
        }
        return $items.ToArray()
    }

    if ($InputObject -is [psobject] -and $InputObject.PSObject.BaseObject -is [pscustomobject]) {
        $normalized = [ordered]@{}
        foreach ($property in @($InputObject.PSObject.Properties | Sort-Object Name)) {
            if ($property.IsGettable) {
                $normalized[$property.Name] = ConvertTo-NormalizedJsonValue $property.Value
            }
        }
        return [pscustomobject]$normalized
    }

    return $InputObject
}

function ConvertTo-CanonicalYamlObjectJson {
    param(
        [Parameter(Mandatory)]
        [object]$InputObject
    )

    $InputObject | ConvertTo-Json -Depth 32 -Compress
}

function Assert-ObjectJsonEquivalent {
    param(
        [AllowNull()]
        [object]$Actual,

        [AllowNull()]
        [object]$Expected
    )

    ConvertTo-CanonicalJson $Actual | Should -Be (ConvertTo-CanonicalJson $Expected)
}

function Assert-StringSequenceEquivalent {
    param(
        [Parameter(Mandatory)]
        [object[]]$Actual,

        [Parameter(Mandatory)]
        [object[]]$Expected
    )

    @($Actual).Count | Should -Be @($Expected).Count
    for ($index = 0; $index -lt @($Expected).Count; $index++) {
        [string]@($Actual)[$index] | Should -Be ([string]@($Expected)[$index])
    }
}
