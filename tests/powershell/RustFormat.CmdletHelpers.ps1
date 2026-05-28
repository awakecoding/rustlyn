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
            ConvertTo-Json -InputObject $items[0] -Depth 32 -Compress
            return
        }

        ConvertTo-Json -InputObject $items.ToArray() -Depth 32 -Compress
    }
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
