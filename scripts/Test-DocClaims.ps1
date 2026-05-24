<#
.SYNOPSIS
Audits public documentation claims against the executable evidence enumerated in
docs/support-matrix.md.

.DESCRIPTION
This is the first tier of the release-conformance gate. It walks every Markdown
file under the repository (excluding generated/scratch areas), extracts capability
claim sentences, and flags ones that mention features whose support-matrix status
is "Planned" or "Unsupported" without an explicit qualifier (Planned / Roadmap /
Not yet / Coming soon / TODO). Use this to catch README/roadmap text that grew
ahead of fixtures and CI evidence.

Exits 0 when all flagged claims are qualified, 1 otherwise.
#>

[CmdletBinding()]
param(
    [string] $Root = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
)

$ErrorActionPreference = 'Stop'

$matrixPath = Join-Path $Root 'docs\support-matrix.md'
if (-not (Test-Path $matrixPath)) {
    Write-Error "Missing support matrix: $matrixPath"
    exit 2
}

# Pull all areas marked Planned or Unsupported out of the matrix table rows.
$matrixText = Get-Content $matrixPath -Raw
$rowRegex = [regex] '\|\s*([^\|]+?)\s*\|\s*(Planned|Unsupported)\s*\|'
$planned = New-Object System.Collections.Generic.List[string]
foreach ($m in $rowRegex.Matches($matrixText)) {
    $area = $m.Groups[1].Value.Trim()
    if (-not [string]::IsNullOrWhiteSpace($area)) { [void]$planned.Add($area) }
}

if ($planned.Count -eq 0) {
    Write-Host "No Planned/Unsupported areas in support matrix; nothing to audit."
    exit 0
}

$skipDirs = @('target','bin','obj','artifacts','node_modules','.git')
$skipFiles = @('docs\original-eric-sink-design.md')
$qualifiers = @('planned','roadmap','not yet','coming soon','todo','future','unsupported','tracked in','will support','intend to')

$violations = New-Object System.Collections.Generic.List[psobject]
$mdFiles = Get-ChildItem -Path $Root -Filter *.md -Recurse -File | Where-Object {
    $rel = $_.FullName.Substring($Root.Length).TrimStart('\','/')
    foreach ($d in $skipDirs) { if ($rel -like "$d*") { return $false } }
    foreach ($s in $skipFiles) { if ($rel -ieq $s) { return $false } }
    return $true
}

foreach ($file in $mdFiles) {
    if ($file.FullName -eq $matrixPath) { continue }
    $lineNum = 0
    foreach ($line in [System.IO.File]::ReadAllLines($file.FullName)) {
        $lineNum++
        $lower = $line.ToLowerInvariant()
        foreach ($area in $planned) {
            $needle = $area.ToLowerInvariant()
            if ($needle.Length -lt 4) { continue }
            # Word-boundary match so "Templates" doesn't match "method templates".
            $escaped = [regex]::Escape($needle)
            if ($lower -match "\b$escaped\b") {
                # Allow specific harmless contexts (e.g., "method templates" referring to C# code templates, not project templates).
                $contextNoiseTokens = @('method templates','code templates','source templates','t4 templates')
                $isContextNoise = $false
                foreach ($ctx in $contextNoiseTokens) { if ($lower.Contains($ctx)) { $isContextNoise = $true; break } }
                if ($isContextNoise) { continue }

                $hasQualifier = $false
                foreach ($q in $qualifiers) { if ($lower.Contains($q)) { $hasQualifier = $true; break } }
                if (-not $hasQualifier) {
                    [void]$violations.Add([pscustomobject]@{
                        File = $file.FullName.Substring($Root.Length).TrimStart('\','/')
                        Line = $lineNum
                        Area = $area
                        Text = $line.Trim()
                    })
                }
            }
        }
    }
}

if ($violations.Count -eq 0) {
    Write-Host "Documentation claim audit passed ($($mdFiles.Count) files, $($planned.Count) Planned/Unsupported areas checked)."
    exit 0
}

Write-Host "Documentation claim audit found $($violations.Count) unqualified references to Planned/Unsupported features:" -ForegroundColor Yellow
$violations | Sort-Object File, Line | Format-Table -AutoSize
exit 1
