param(
    [Parameter(Mandatory = $false)]
    [string[]]$Sample = @("add"),

    [Parameter(Mandatory = $false)]
    [ValidateSet("Bitcode", "Cargo")]
    [string]$Mode = "Bitcode"
)

$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$sampleChecks = @{
    add = @{
        Method = "add_i32"
        Arguments = @(3, 4)
        Expected = 7
    }
    and = @{
        Method = "and_i32"
        Arguments = @(6, 3)
        Expected = 2
    }
    shl = @{
        Method = "shl_i32"
        Arguments = @(3, 1)
        Expected = 6
    }
    ashr = @{
        Method = "ashr_i32"
        Arguments = @(-8, 1)
        Expected = -4
    }
    lshr = @{
        Method = "lshr_i32"
        Arguments = @(8, 1)
        Expected = 4
    }
    or = @{
        Method = "or_i32"
        Arguments = @(6, 3)
        Expected = 7
    }
    xor = @{
        Method = "xor_i32"
        Arguments = @(6, 3)
        Expected = 5
    }
    mul = @{
        Method = "mul_i32"
        Arguments = @(3, 4)
        Expected = 12
    }
    div = @{
        Method = "div_i32"
        Arguments = @(8, 2)
        Expected = 4
    }
    div_u32 = @{
        Method = "div_u32"
        Arguments = @(8, 2)
        Expected = 4
    }
    div_i64 = @{
        Method = "div_i64"
        Arguments = @(8L, 2L)
        Expected = 4L
    }
    rem = @{
        Method = "rem_i32"
        Arguments = @(9, 4)
        Expected = 1
    }
    rem_u32 = @{
        Method = "rem_u32"
        Arguments = @(9, 4)
        Expected = 1
    }
    rem_i64 = @{
        Method = "rem_i64"
        Arguments = @(9L, 4L)
        Expected = 1L
    }
    rem_u64 = @{
        Method = "rem_u64"
        Arguments = @(9L, 4L)
        Expected = 1L
    }
    sext_i32_to_i64 = @{
        Method = "sext_i32_to_i64"
        Arguments = @(-7)
        Expected = -7L
    }
    select = @{
        Method = "select_i32"
        Arguments = @(0, 11, 22)
        Expected = 11
    }
    mask_lt_i64 = @{
        Method = "mask_lt_i64"
        Arguments = @(2L, 7L)
        Expected = -1L
    }
    phi_merge = @{
        Method = "merge_call_i32"
        Arguments = @(0, 5)
        Expected = 12
    }
    xor_fold_loop = @{
        Method = "xor_fold_i32"
        Arguments = @(5)
        Expected = 4
    }
    xor_fold_vectorized = @{
        Method = "xor_fold_i32"
        Arguments = @(5)
        Expected = 4
    }
    sum_xor_u8_vectorized = @{
        Method = "sum_xor_u8"
        Arguments = @(33)
        Expected = 19
    }
    sum_xor_i8_vectorized = @{
        Method = "sum_xor_i8"
        Arguments = @(33)
        Expected = 19
    }
    max_xor_i8_vectorized = @{
        Method = "max_xor_i8"
        Arguments = @(33)
        Expected = 35
    }
    max_add_sub_i8_vectorized = @{
        Method = "max_add_sub_i8"
        Arguments = @(100)
        Expected = 127
    }
    max_xor_sub_i8_vectorized = @{
        Method = "max_xor_sub_i8"
        Arguments = @(100)
        Expected = 120
    }
    max_xor_u8_vectorized = @{
        Method = "max_xor_u8"
        Arguments = @(255)
        Expected = 255
    }
    min_xor_u8_vectorized = @{
        Method = "min_xor_u8"
        Arguments = @(255)
        Expected = 0
    }
    min_xor_sub_u8_vectorized = @{
        Method = "min_xor_sub_u8"
        Arguments = @(255)
        Expected = 0
    }
    min_xor_i8_vectorized = @{
        Method = "min_xor_i8"
        Arguments = @(33)
        Expected = 0
    }
    min_xor_sub_i8_vectorized = @{
        Method = "min_xor_sub_i8"
        Arguments = @(64)
        Expected = 57
    }
    min_add_sub_i8_vectorized = @{
        Method = "min_add_sub_i8"
        Arguments = @(64)
        Expected = 38
    }
    or_fold_vectorized = @{
        Method = "or_fold_i32"
        Arguments = @(5)
        Expected = 7
    }
    and_fold_vectorized = @{
        Method = "and_fold_i32"
        Arguments = @(5)
        Expected = 0
    }
    and_fold_i8_vectorized = @{
        Method = "and_fold_i8"
        Arguments = @(33)
        Expected = 0
    }
    max_xor_vectorized = @{
        Method = "max_xor_i32"
        Arguments = @(5)
        Expected = 7
    }
    min_xor_vectorized = @{
        Method = "min_xor_i32"
        Arguments = @(5)
        Expected = 0
    }
    max_xor_u32_vectorized = @{
        Method = "max_xor_u32"
        Arguments = @(5)
        Expected = 7
    }
    min_xor_u32_vectorized = @{
        Method = "min_xor_u32"
        Arguments = @(5)
        Expected = 0
    }
    max_xor_u64_vectorized = @{
        Method = "max_xor_u64"
        Arguments = @(5L)
        Expected = 7L
    }
    min_xor_u64_vectorized = @{
        Method = "min_xor_u64"
        Arguments = @(5L)
        Expected = 0L
    }
    min_xor_i64_vectorized = @{
        Method = "min_xor_i64"
        Arguments = @(5L)
        Expected = 0L
    }
    max_xor_i64_vectorized = @{
        Method = "max_xor_i64"
        Arguments = @(5L)
        Expected = 7L
    }
    sum_fold_loop = @{
        Method = "sum_fold_i32"
        Arguments = @(5)
        Expected = 10
    }
    sum_xor_vectorized = @{
        Method = "sum_xor_i32"
        Arguments = @(5)
        Expected = 13
    }
    xor_fold_u32_vectorized = @{
        Method = "xor_fold_u32"
        Arguments = @(17)
        Expected = 16
    }
    sum_xor_u32_vectorized = @{
        Method = "sum_xor_u32"
        Arguments = @(17)
        Expected = 139
    }
    or_fold_u32_vectorized = @{
        Method = "or_fold_u32"
        Arguments = @(17)
        Expected = 31
    }
    and_fold_u32_vectorized = @{
        Method = "and_fold_u32"
        Arguments = @(17)
        Expected = 0
    }
    xor_fold_u64_vectorized = @{
        Method = "xor_fold_u64"
        Arguments = @(17L)
        Expected = 16L
    }
    sum_xor_u64_vectorized = @{
        Method = "sum_xor_u64"
        Arguments = @(17L)
        Expected = 139L
    }
    or_fold_u64_vectorized = @{
        Method = "or_fold_u64"
        Arguments = @(17L)
        Expected = 31L
    }
    and_fold_u64_vectorized = @{
        Method = "and_fold_u64"
        Arguments = @(17L)
        Expected = 0L
    }
    max_xor_i16_vectorized = @{
        Method = "max_xor_i16"
        Arguments = @([Int16]5)
        Expected = [Int16]7
    }
    max_xor_u16_vectorized = @{
        Method = "max_xor_u16"
        Arguments = @([UInt16]5)
        Expected = [UInt16]7
    }
    min_xor_u16_vectorized = @{
        Method = "min_xor_u16"
        Arguments = @([UInt16]5)
        Expected = [UInt16]0
    }
    xor_fold_u8_vectorized = @{
        Method = "xor_fold_u8"
        Arguments = @(33)
        Expected = 32
    }
    xor_fold_i8_vectorized = @{
        Method = "xor_fold_i8"
        Arguments = @(33)
        Expected = 32
    }
    or_fold_u8_vectorized = @{
        Method = "or_fold_u8"
        Arguments = @(33)
        Expected = 63
    }
    or_fold_i8_vectorized = @{
        Method = "or_fold_i8"
        Arguments = @(33)
        Expected = 63
    }
    and_fold_u8_vectorized = @{
        Method = "and_fold_u8"
        Arguments = @(33)
        Expected = 0
    }
    xor_fold_u16_vectorized = @{
        Method = "xor_fold_u16"
        Arguments = @([UInt16]17)
        Expected = [UInt16]16
    }
    sum_xor_u16_vectorized = @{
        Method = "sum_xor_u16"
        Arguments = @([UInt16]17)
        Expected = [UInt16]139
    }
    or_fold_u16_vectorized = @{
        Method = "or_fold_u16"
        Arguments = @([UInt16]17)
        Expected = [UInt16]31
    }
    and_fold_u16_vectorized = @{
        Method = "and_fold_u16"
        Arguments = @([UInt16]17)
        Expected = [UInt16]0
    }
    min_xor_i16_vectorized = @{
        Method = "min_xor_i16"
        Arguments = @([Int16]5)
        Expected = [Int16]0
    }
    xor_fold_i16_vectorized = @{
        Method = "xor_fold_i16"
        Arguments = @([Int16]17)
        Expected = [Int16]16
    }
    sum_xor_i16_vectorized = @{
        Method = "sum_xor_i16"
        Arguments = @([Int16]17)
        Expected = [Int16]139
    }
    or_fold_i16_vectorized = @{
        Method = "or_fold_i16"
        Arguments = @([Int16]17)
        Expected = [Int16]31
    }
    and_fold_i16_vectorized = @{
        Method = "and_fold_i16"
        Arguments = @([Int16]17)
        Expected = [Int16]0
    }
    sum_xor_i64_vectorized = @{
        Method = "sum_xor_i64"
        Arguments = @(5L)
        Expected = 13L
    }
    xor_fold_i64_vectorized = @{
        Method = "xor_fold_i64"
        Arguments = @(5L)
        Expected = 4L
    }
    or_fold_i64_vectorized = @{
        Method = "or_fold_i64"
        Arguments = @(5L)
        Expected = 7L
    }
    and_fold_i64_vectorized = @{
        Method = "and_fold_i64"
        Arguments = @(5L)
        Expected = 0L
    }
    eq = @{
        Method = "eq_i32"
        Arguments = @(7, 7)
        Expected = 1
    }
    ne = @{
        Method = "ne_i32"
        Arguments = @(2, 7)
        Expected = 1
    }
    max_u32 = @{
        Method = "max_u32"
        Arguments = @(2, 7)
        Expected = 7
    }
    max_eq_u32 = @{
        Method = "max_eq_u32"
        Arguments = @(7, 7)
        Expected = 7
    }
    sub = @{
        Method = "sub_i64"
        Arguments = @(10L, 3L)
        Expected = 7L
    }
    max = @{
        Method = "max_i32"
        Arguments = @(2, 7)
        Expected = 7
    }
    max_eq = @{
        Method = "max_eq_i32"
        Arguments = @(7, 7)
        Expected = 7
    }
    min_u32 = @{
        Method = "min_u32"
        Arguments = @(2, 7)
        Expected = 2
    }
    min_eq_u32 = @{
        Method = "min_eq_u32"
        Arguments = @(7, 7)
        Expected = 7
    }
    min = @{
        Method = "min_i32"
        Arguments = @(2, 7)
        Expected = 2
    }
    min_eq = @{
        Method = "min_eq_i32"
        Arguments = @(7, 7)
        Expected = 7
    }
    call_chain = @{
        Method = "call_chain_i32"
        Arguments = @(5)
        Expected = 6
    }
    global_init = @{
        Method = "read_global_i32"
        Arguments = @()
        Expected = 7
    }
    global_array = @{
        Method = "read_second_i32"
        Arguments = @()
        Expected = 20
    }
    pair_right = @{
        Method = "second_field_i32"
        Arguments = @(38654705667L)
        Expected = 9
    }
    pair_i64 = @{
        Method = "second_field_i64"
        Expected = 9L
        SupportedModes = @("Bitcode")
        CreateArguments = {
            $memory = [System.Runtime.InteropServices.Marshal]::AllocHGlobal(16)
            try {
                [System.Runtime.InteropServices.Marshal]::WriteInt64($memory, 0, 3L)
                [System.Runtime.InteropServices.Marshal]::WriteInt64($memory, 8, 9L)
                return @{
                    Arguments = @([IntPtr]$memory)
                    State = $memory
                }
            }
            catch {
                [System.Runtime.InteropServices.Marshal]::FreeHGlobal($memory)
                throw
            }
        }
        Cleanup = {
            param($state)

            if ($null -ne $state -and $state -ne [IntPtr]::Zero) {
                [System.Runtime.InteropServices.Marshal]::FreeHGlobal($state)
            }
        }
    }
    local_array = @{
        Method = "second_of_pair_i32"
        Arguments = @(3, 9)
        Expected = 9
    }
}

function Invoke-SmokeCheck {
    param(
        [Parameter(Mandatory = $true)]
        [string]$CurrentSample,

        [Parameter(Mandatory = $true)]
        [string]$ArtifactPath,

        [Parameter(Mandatory = $true)]
        [hashtable]$Check
    )

    $arguments = $Check.Arguments
    $cleanupState = $null
    if ($Check.ContainsKey("CreateArguments")) {
        $prepared = & $Check.CreateArguments
        $arguments = $prepared.Arguments
        $cleanupState = $prepared.State
    }

    dotnet run --project ".\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj" -- inspect $ArtifactPath
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet run inspect failed for '$CurrentSample' with exit code $LASTEXITCODE"
    }

    dotnet run --project ".\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj" -- lower $ArtifactPath | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet run lower failed for '$CurrentSample' with exit code $LASTEXITCODE"
    }

    $emittedAssemblyPath = Join-Path ([System.IO.Path]::GetTempPath()) ("rust-msil-smoke-{0}-{1}.dll" -f $CurrentSample, [Guid]::NewGuid().ToString("N"))
    try {
        dotnet run --project ".\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj" -- emit $ArtifactPath --out $emittedAssemblyPath | Out-Null
        if ($LASTEXITCODE -ne 0) {
            throw "dotnet run emit failed for '$CurrentSample' with exit code $LASTEXITCODE"
        }

        $loadContext = [System.Runtime.Loader.AssemblyLoadContext]::new("rust-msil-smoke-$CurrentSample-$([Guid]::NewGuid().ToString('N'))", $true)
        try {
            $assemblyBytes = [System.IO.File]::ReadAllBytes($emittedAssemblyPath)
            $assemblyStream = [System.IO.MemoryStream]::new($assemblyBytes)
            try {
                $assembly = $loadContext.LoadFromStream($assemblyStream)
            }
            finally {
                $assemblyStream.Dispose()
            }

            $generatedType = $assembly.GetType("RustMcil.GeneratedModule", $true)
            $method = $generatedType.GetMethod($Check.Method, [System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::Static)
            if ($null -eq $method) {
                throw "Generated method '$($Check.Method)' was not found for '$CurrentSample'."
            }

            $actual = $method.Invoke($null, $arguments)
            if ($actual -ne $Check.Expected) {
                throw "Generated method '$($Check.Method)' returned '$actual' for '$CurrentSample', expected '$($Check.Expected)'."
            }

            Write-Host ("PASS {0} => {1}" -f $CurrentSample, $actual)
        }
        finally {
            $loadContext.Unload()
            [System.GC]::Collect()
            [System.GC]::WaitForPendingFinalizers()
            [System.GC]::Collect()
        }
    }
    finally {
        if ($Check.ContainsKey("Cleanup")) {
            & $Check.Cleanup $cleanupState
        }

        if (Test-Path $emittedAssemblyPath) {
            Remove-Item -Force $emittedAssemblyPath
        }
    }
}

function Invoke-TranslateSmokeCheck {
    param(
        [Parameter(Mandatory = $true)]
        [string]$CurrentSample,

        [Parameter(Mandatory = $true)]
        [string]$CratePath,

        [Parameter(Mandatory = $true)]
        [hashtable]$Check
    )

    $arguments = $Check.Arguments
    $cleanupState = $null
    if ($Check.ContainsKey("CreateArguments")) {
        $prepared = & $Check.CreateArguments
        $arguments = $prepared.Arguments
        $cleanupState = $prepared.State
    }

    $translatedBitcodePath = Join-Path ([System.IO.Path]::GetTempPath()) ("rust-msil-translate-smoke-{0}-{1}.bc" -f $CurrentSample, [Guid]::NewGuid().ToString("N"))
    $translatedAssemblyPath = Join-Path ([System.IO.Path]::GetTempPath()) ("rust-msil-translate-smoke-{0}-{1}.dll" -f $CurrentSample, [Guid]::NewGuid().ToString("N"))
    try {
        dotnet run --project ".\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj" -- translate $CratePath --out $translatedAssemblyPath --bitcode-out $translatedBitcodePath | Out-Null
        if ($LASTEXITCODE -ne 0) {
            throw "dotnet run translate failed for '$CurrentSample' with exit code $LASTEXITCODE"
        }

        dotnet run --project ".\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj" -- inspect $translatedBitcodePath
        if ($LASTEXITCODE -ne 0) {
            throw "dotnet run inspect failed for translated '$CurrentSample' with exit code $LASTEXITCODE"
        }

        dotnet run --project ".\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj" -- lower $translatedBitcodePath | Out-Null
        if ($LASTEXITCODE -ne 0) {
            throw "dotnet run lower failed for translated '$CurrentSample' with exit code $LASTEXITCODE"
        }

        $loadContext = [System.Runtime.Loader.AssemblyLoadContext]::new("rust-msil-translate-smoke-$CurrentSample-$([Guid]::NewGuid().ToString('N'))", $true)
        try {
            $assemblyBytes = [System.IO.File]::ReadAllBytes($translatedAssemblyPath)
            $assemblyStream = [System.IO.MemoryStream]::new($assemblyBytes)
            try {
                $assembly = $loadContext.LoadFromStream($assemblyStream)
            }
            finally {
                $assemblyStream.Dispose()
            }

            $generatedType = $assembly.GetType("RustMcil.GeneratedModule", $true)
            $method = $generatedType.GetMethod($Check.Method, [System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::Static)
            if ($null -eq $method) {
                throw "Translated method '$($Check.Method)' was not found for '$CurrentSample'."
            }

            $actual = $method.Invoke($null, $arguments)
            if ($actual -ne $Check.Expected) {
                throw "Translated method '$($Check.Method)' returned '$actual' for '$CurrentSample', expected '$($Check.Expected)'."
            }

            Write-Host ("PASS {0} (cargo) => {1}" -f $CurrentSample, $actual)
        }
        finally {
            $loadContext.Unload()
            [System.GC]::Collect()
            [System.GC]::WaitForPendingFinalizers()
            [System.GC]::Collect()
        }
    }
    finally {
        if ($Check.ContainsKey("Cleanup")) {
            & $Check.Cleanup $cleanupState
        }

        if (Test-Path $translatedAssemblyPath) {
            Remove-Item -Force $translatedAssemblyPath
        }

        if (Test-Path $translatedBitcodePath) {
            Remove-Item -Force $translatedBitcodePath
        }
    }
}

Push-Location $workspaceRoot
try {
    dotnet build ".\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj"
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet build failed with exit code $LASTEXITCODE"
    }

    foreach ($currentSample in $Sample) {
        if (-not $sampleChecks.ContainsKey($currentSample)) {
            throw "Unknown smoke sample '$currentSample'. Supported samples: $($sampleChecks.Keys -join ', ')"
        }

        $check = $sampleChecks[$currentSample]
        if ($check.ContainsKey("SupportedModes") -and $check.SupportedModes -notcontains $Mode) {
            throw "Smoke sample '$currentSample' does not support mode '$Mode'."
        }

        if ($Mode -eq "Cargo") {
            $cratePath = Join-Path $workspaceRoot (Join-Path "samples" $currentSample)
            Invoke-TranslateSmokeCheck -CurrentSample $currentSample -CratePath $cratePath -Check $check
            continue
        }

        $artifactPath = & (Join-Path $workspaceRoot "scripts\Build-SampleBitcode.ps1") -Sample $currentSample
        Invoke-SmokeCheck -CurrentSample $currentSample -ArtifactPath $artifactPath -Check $check
    }
}
finally {
    Pop-Location
}
