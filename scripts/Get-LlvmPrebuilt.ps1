param(
    [Parameter(Mandatory = $false)]
    [string]$Version = "20.1.8",

    [Parameter(Mandatory = $false)]
    [string]$ReleaseTag = "v2025.3.0",

    [Parameter(Mandatory = $false)]
    [string]$Architecture = "x86_64",

    [Parameter(Mandatory = $false)]
    [string]$OperatingSystem = "windows",

    [Parameter(Mandatory = $false)]
    [switch]$Force
)

$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$toolchainRoot = Join-Path $workspaceRoot "artifacts\toolchains\llvm"
$downloadRoot = Join-Path $workspaceRoot "artifacts\toolchains\downloads"
$archiveName = "clang+llvm-$Version-$Architecture-$OperatingSystem.tar.xz"
$archivePath = Join-Path $downloadRoot $archiveName
$installRoot = Join-Path $toolchainRoot ([System.IO.Path]::GetFileNameWithoutExtension([System.IO.Path]::GetFileNameWithoutExtension($archiveName)))
$downloadUrl = "https://github.com/awakecoding/llvm-prebuilt/releases/download/$ReleaseTag/$archiveName"
$expectedToolPath = Join-Path $installRoot "bin\llvm-opt.exe"

if ($Force -and (Test-Path $installRoot)) {
    Remove-Item -Recurse -Force $installRoot
}

if ((-not $Force) -and (Test-Path $expectedToolPath)) {
    Write-Output $installRoot
    return
}

New-Item -ItemType Directory -Force -Path $toolchainRoot | Out-Null
New-Item -ItemType Directory -Force -Path $downloadRoot | Out-Null

if ($Force -or (-not (Test-Path $archivePath))) {
    Invoke-WebRequest -Uri $downloadUrl -OutFile $archivePath
}

tar -xf $archivePath -C $toolchainRoot
if ($LASTEXITCODE -ne 0) {
    throw "Failed to extract llvm-prebuilt archive: $archivePath"
}

if (-not (Test-Path $expectedToolPath)) {
    throw "Expected llvm-opt.exe was not found after extraction: $expectedToolPath"
}

Write-Output $installRoot
