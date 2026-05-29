function Resolve-RustlynCli {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$RepoRoot,

        [Parameter(Mandatory = $false)]
        [ValidateSet("Debug", "Release")]
        [string]$Configuration = "Release",

        [Parameter(Mandatory = $false)]
        [string]$ToolPath,

        [Parameter(Mandatory = $false)]
        [string]$ToolDll,

        [Parameter(Mandatory = $false)]
        [switch]$SkipBuild
    )

    $resolvedRepoRoot = (Resolve-Path -LiteralPath $RepoRoot).ProviderPath

    if (-not [string]::IsNullOrWhiteSpace($ToolPath)) {
        $resolvedToolPath = Resolve-RustlynCliPath -RepoRoot $resolvedRepoRoot -Path $ToolPath
        return [pscustomobject]@{
            FileName = $resolvedToolPath
            PrefixArguments = @()
            DisplayName = $resolvedToolPath
        }
    }

    if (-not [string]::IsNullOrWhiteSpace($ToolDll)) {
        $resolvedToolDll = Resolve-RustlynCliPath -RepoRoot $resolvedRepoRoot -Path $ToolDll
        return [pscustomobject]@{
            FileName = "dotnet"
            PrefixArguments = @($resolvedToolDll)
            DisplayName = "dotnet $resolvedToolDll"
        }
    }

    $toolProject = Join-Path $resolvedRepoRoot "dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj"
    $toolOutputDirectory = Join-Path $resolvedRepoRoot "dotnet\backend\src\Rustlyn.Tool\bin\$Configuration\net10.0"
    $toolExeName = if ($IsWindows -or $env:OS -eq "Windows_NT") { "rustlyn.exe" } else { "rustlyn" }
    $toolExe = Join-Path $toolOutputDirectory $toolExeName

    if (-not $SkipBuild -and -not (Test-Path -LiteralPath $toolExe -PathType Leaf)) {
        dotnet build $toolProject -c $Configuration /nologo
        if ($LASTEXITCODE -ne 0) {
            throw "rustlyn build failed with exit code $LASTEXITCODE."
        }
    }

    if (Test-Path -LiteralPath $toolExe -PathType Leaf) {
        return [pscustomobject]@{
            FileName = (Resolve-Path -LiteralPath $toolExe).ProviderPath
            PrefixArguments = @()
            DisplayName = $toolExe
        }
    }

    $toolDll = Join-Path $toolOutputDirectory "rustlyn.dll"
    if (Test-Path -LiteralPath $toolDll -PathType Leaf) {
        return [pscustomobject]@{
            FileName = "dotnet"
            PrefixArguments = @((Resolve-Path -LiteralPath $toolDll).ProviderPath)
            DisplayName = "dotnet $toolDll"
        }
    }

    $buildHint = if ($SkipBuild) { " Run without -SkipBuild to build it first." } else { "" }
    throw "rustlyn output was not produced under '$toolOutputDirectory'.$buildHint"
}

function Invoke-RustlynCli {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)]
        [pscustomobject]$Command,

        [Parameter(ValueFromRemainingArguments = $true)]
        [string[]]$Arguments
    )

    $toolArguments = @()
    foreach ($argument in $Command.PrefixArguments) {
        $toolArguments += $argument
    }
    foreach ($argument in $Arguments) {
        $toolArguments += $argument
    }

    & $Command.FileName @toolArguments
}

function Resolve-RustlynCliPath {
    param(
        [Parameter(Mandatory = $true)]
        [string]$RepoRoot,

        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    $candidatePath = if ([System.IO.Path]::IsPathRooted($Path)) {
        $Path
    }
    else {
        Join-Path $RepoRoot $Path
    }

    if (-not (Test-Path -LiteralPath $candidatePath -PathType Leaf)) {
        throw "rustlyn tool not found: $candidatePath"
    }

    return (Resolve-Path -LiteralPath $candidatePath).ProviderPath
}
