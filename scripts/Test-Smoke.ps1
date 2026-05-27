param(
    [Parameter(Mandatory = $false)]
    [string[]]$Sample = @("add"),

    [Parameter(Mandatory = $false)]
    [ValidateSet("Bitcode", "Cargo")]
    [string]$Mode = "Bitcode",

    [Parameter(Mandatory = $false)]
    [ValidateSet("Full", "Inspect")]
    [string]$Stage = "Full",

    [Parameter(Mandatory = $false)]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",

    [Parameter(Mandatory = $false)]
    [string]$ToolDll
)

$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$toolProject = Join-Path $workspaceRoot "dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj"
$defaultToolDll = Join-Path $workspaceRoot "dotnet\backend\src\Rustlyn.Tool\bin\$Configuration\net10.0\Rustlyn.Tool.dll"
$script:RustlynToolDll = $null

function Resolve-RustlynToolDll {
    param(
        [Parameter(Mandatory = $true)]
        [string]$CandidatePath
    )

    $resolvedPath = if ([System.IO.Path]::IsPathRooted($CandidatePath)) {
        $CandidatePath
    }
    else {
        Join-Path $workspaceRoot $CandidatePath
    }

    if (-not (Test-Path -LiteralPath $resolvedPath -PathType Leaf)) {
        throw "Rustlyn.Tool DLL not found: $resolvedPath"
    }

    return (Resolve-Path -LiteralPath $resolvedPath).ProviderPath
}

function Invoke-RustlynTool {
    if (-not $script:RustlynToolDll) {
        throw "Rustlyn.Tool DLL has not been resolved."
    }

    & dotnet $script:RustlynToolDll @args
}

$sampleChecks = @{
    add = @{
        Method = "add_i32"
        Arguments = @(3, 4)
        Expected = 7
    }
    build_std_core_probe = @{
        CratePath = Join-Path $workspaceRoot "samples\add"
        Method = "add_i32"
        Arguments = @(19, 23)
        Expected = 42
        SupportedModes = @("Cargo")
        Toolchain = "nightly"
        BuildStd = "core"
    }
    build_std_alloc_probe = @{
        CratePath = Join-Path $workspaceRoot "samples\alloc_only_probe"
        Method = "alloc_vec_capacity_score"
        Arguments = @()
        Expected = 4
        SupportedModes = @("Cargo")
        Toolchain = "nightly"
        BuildStd = "core,alloc"
    }
    build_std_std_probe = @{
        CratePath = Join-Path $workspaceRoot "samples\std_fs"
        Method = "std_fs_line_count"
        Arguments = @()
        Expected = 3
        SupportedModes = @("Cargo")
        Toolchain = "nightly"
        BuildStd = "std,panic_abort"
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
    alloc_probe = @{
        Method = "alloc_string_len"
        Arguments = @()
        Expected = 12
        SupportedModes = @("Cargo")
    }
    std_fs = @{
        Method = "std_fs_line_count"
        Arguments = @()
        Expected = 3
        SupportedModes = @("Cargo")
    }
    std_console = @{
        Method = "std_console_probe"
        Arguments = @()
        Expected = 0
        SupportedModes = @("Cargo")
        Toolchain = "nightly"
        BuildStd = "std,panic_abort"
    }
    std_console_runtime = @{
        CratePath = Join-Path $workspaceRoot "samples\std_console"
        Method = "std_console_runtime_value_probe"
        Arguments = @(17)
        Expected = 0
        SupportedModes = @("Cargo")
        Toolchain = "nightly"
        BuildStd = "std,panic_abort"
    }
    std_time = @{
        Method = "std_time_probe"
        Arguments = @()
        Expected = 1
        SupportedModes = @("Cargo")
        Toolchain = "nightly"
        BuildStd = "std,panic_abort"
    }
    std_env = @{
        Method = "std_env_probe"
        Arguments = @()
        ExpectedMinimum = 1
        SupportedModes = @("Cargo")
        Toolchain = "nightly"
        BuildStd = "std,panic_abort"
    }
    std_path = @{
        Method = "std_path_probe"
        Arguments = @()
        Expected = 4
        SupportedModes = @("Cargo")
        Toolchain = "nightly"
        BuildStd = "std,panic_abort"
    }
    marked_yaml = @{
        Method = "marked_yaml_serde_parse_fails_coerce_score"
        Arguments = @()
        Expected = 7
        SupportedModes = @("Cargo")
        Toolchain = "nightly"
        BuildStd = "std,panic_abort"
    }
    trait_object_probe = @{
        Method = "trait_object_score"
        Arguments = @(0)
        Expected = 12
        SupportedModes = @("Cargo")
    }
    trait_object_pair_probe = @{
        CratePath = Join-Path $workspaceRoot "samples\trait_object_probe"
        Method = "trait_object_pair_score"
        Arguments = @(0)
        Expected = 507
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_api = @{
        Method = "dotnet_runtime_score"
        Arguments = @()
        Expected = 253
        SupportedModes = @("Cargo")
    }
    generated_bindings_hello = @{
        Method = "generated_bindings_score"
        Arguments = @()
        Expected = 100
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_loop = @{
        Method = "dotnet_runtime_loop"
        Arguments = @(2, [uint32]45)
        Expected = 27
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_text = @{
        Method = "dotnet_runtime_text_score"
        Arguments = @()
        Expected = 189
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_args = @{
        Arguments = @("alpha-runtime", "runtime")
        ExpectedOutput = "alpha-runtime"
        BinaryTarget = "dotnet_runtime_args"
        SupportedModes = @("Cargo")
    }
    avalonia_hello = @{
        Arguments = @("--smoke")
        ExpectedOutput = "avalonia:rust-ui:ok"
        BinaryTarget = "avalonia_hello"
        SupportedModes = @("Cargo")
    }
    lousygrep = @{
        CratePath = Join-Path $workspaceRoot "samples\generated_bindings_lousygrep"
        Arguments = @(
            "runtime",
            (Join-Path $workspaceRoot "samples\generated_bindings_lousygrep\fixtures"),
            "input.txt",
            "second.txt"
        )
        ExpectedOutput = @(
            "alpha-runtime",
            "runtime-beta",
            "runtime-gamma"
        )
        BinaryTarget = "generated_bindings_lousygrep"
        SupportedModes = @("Cargo")
    }
    lousygrep_primitive = @{
        Arguments = @(
            "runtime",
            (Join-Path $workspaceRoot "samples\lousygrep_primitive\fixtures\input.txt"),
            (Join-Path $workspaceRoot "samples\lousygrep_primitive\fixtures\second.txt")
        )
        ExpectedOutput = @(
            "$((Join-Path $workspaceRoot 'samples\lousygrep_primitive\fixtures\input.txt')):alpha-runtime",
            "$((Join-Path $workspaceRoot 'samples\lousygrep_primitive\fixtures\input.txt')):runtime-beta",
            "$((Join-Path $workspaceRoot 'samples\lousygrep_primitive\fixtures\second.txt')):runtime-gamma"
        )
        BinaryTarget = "lousygrep_primitive"
        SupportedModes = @("Cargo")
    }
    generated_bindings_lousygrep = @{
        Arguments = @(
            "runtime",
            (Join-Path $workspaceRoot "samples\generated_bindings_lousygrep\fixtures"),
            "input.txt",
            "second.txt"
        )
        ExpectedOutput = @(
            "alpha-runtime",
            "runtime-beta",
            "runtime-gamma"
        )
        BinaryTarget = "generated_bindings_lousygrep"
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_roundtrip = @{
        Method = "dotnet_runtime_roundtrip_score"
        Arguments = @()
        Expected = 196
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_transform = @{
        Method = "dotnet_runtime_transform_score"
        Arguments = @()
        Expected = 335
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_transform = @{
        Method = "dotnet_runtime_path_transform_score"
        Arguments = @()
        Expected = 1563
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_change = @{
        Method = "dotnet_runtime_path_change_score"
        Arguments = @()
        Expected = 10163
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_combine = @{
        Method = "dotnet_runtime_path_combine_score"
        Arguments = @()
        Expected = 9573
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_combine3 = @{
        Method = "dotnet_runtime_path_combine3_score"
        Arguments = @()
        Expected = 9595
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_env_path = @{
        Method = "dotnet_runtime_env_path_score"
        Arguments = @()
        Expected = 10595
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_user_profile = @{
        Method = "dotnet_runtime_user_profile_score"
        Arguments = @()
        Expected = 12717
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_dual_root = @{
        Method = "dotnet_runtime_dual_root_score"
        Arguments = @()
        Expected = 11145
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_triple_root = @{
        Method = "dotnet_runtime_triple_root_score"
        Arguments = @()
        Expected = 1018583
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_relative_path = @{
        Method = "dotnet_runtime_relative_path_score"
        Arguments = @()
        Expected = 34095
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_directory_name = @{
        Method = "dotnet_runtime_directory_name_score"
        Arguments = @()
        Expected = 239012
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_full_path = @{
        Method = "dotnet_runtime_full_path_score"
        Arguments = @()
        Expected = 524012
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_root = @{
        Method = "dotnet_runtime_path_root_score"
        Arguments = @()
        Expected = 4738012
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_file_name = @{
        Method = "dotnet_runtime_path_file_name_score"
        Arguments = @()
        Expected = 47379812
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_recompose = @{
        Method = "dotnet_runtime_path_recompose_score"
        Arguments = @()
        Expected = 50499812
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_recompose_change = @{
        Method = "dotnet_runtime_path_recompose_change_score"
        Arguments = @()
        Expected = 51110812
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_cross_root = @{
        Method = "dotnet_runtime_path_cross_root_score"
        Arguments = @()
        Expected = 52116812
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_branch_root = @{
        Method = "dotnet_runtime_path_branch_root_score"
        Arguments = @()
        Expected = 152116812
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_triple_branch = @{
        Method = "dotnet_runtime_path_triple_branch_score"
        Arguments = @()
        Expected = 351105611
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_double_select = @{
        Method = "dotnet_runtime_path_double_select_score"
        Arguments = @()
        Expected = 342006812
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_ranked_select = @{
        Method = "dotnet_runtime_path_ranked_select_score"
        Arguments = @()
        Expected = 342006812
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_combo_rank = @{
        Method = "dotnet_runtime_path_combo_rank_score"
        Arguments = @()
        Expected = 333121499
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_two_stage_rank = @{
        Method = "dotnet_runtime_path_two_stage_rank_score"
        Arguments = @()
        Expected = 332311999
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_three_stage_rank = @{
        Method = "dotnet_runtime_path_three_stage_rank_score"
        Arguments = @()
        Expected = 332231220
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_four_stage_rank = @{
        Method = "dotnet_runtime_path_four_stage_rank_score"
        Arguments = @()
        Expected = 332233109
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_five_stage_rank = @{
        Method = "dotnet_runtime_path_five_stage_rank_score"
        Arguments = @()
        Expected = 332232319
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_six_stage_rank = @{
        Method = "dotnet_runtime_path_six_stage_rank_score"
        Arguments = @()
        Expected = 332232209
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_seven_stage_rank = @{
        Method = "dotnet_runtime_path_seven_stage_rank_score"
        Arguments = @()
        Expected = 332232210
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eight_stage_rank = @{
        Method = "dotnet_runtime_path_eight_stage_rank_score"
        Arguments = @()
        Expected = 332232212
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_nine_stage_rank = @{
        Method = "dotnet_runtime_path_nine_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_ten_stage_rank = @{
        Method = "dotnet_runtime_path_ten_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eleven_stage_rank = @{
        Method = "dotnet_runtime_path_eleven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_twelve_stage_rank = @{
        Method = "dotnet_runtime_path_twelve_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_thirteen_stage_rank = @{
        Method = "dotnet_runtime_path_thirteen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_fourteen_stage_rank = @{
        Method = "dotnet_runtime_path_fourteen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_fifteen_stage_rank = @{
        Method = "dotnet_runtime_path_fifteen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_sixteen_stage_rank = @{
        Method = "dotnet_runtime_path_sixteen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_seventeen_stage_rank = @{
        Method = "dotnet_runtime_path_seventeen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eighteen_stage_rank = @{
        Method = "dotnet_runtime_path_eighteen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_nineteen_stage_rank = @{
        Method = "dotnet_runtime_path_nineteen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_twenty_stage_rank = @{
        Method = "dotnet_runtime_path_twenty_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_twenty_one_stage_rank = @{
        Method = "dotnet_runtime_path_twenty_one_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_twenty_two_stage_rank = @{
        Method = "dotnet_runtime_path_twenty_two_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_twenty_three_stage_rank = @{
        Method = "dotnet_runtime_path_twenty_three_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_twenty_four_stage_rank = @{
        Method = "dotnet_runtime_path_twenty_four_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_twenty_five_stage_rank = @{
        Method = "dotnet_runtime_path_twenty_five_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_twenty_six_stage_rank = @{
        Method = "dotnet_runtime_path_twenty_six_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_twenty_seven_stage_rank = @{
        Method = "dotnet_runtime_path_twenty_seven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_twenty_eight_stage_rank = @{
        Method = "dotnet_runtime_path_twenty_eight_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_twenty_nine_stage_rank = @{
        Method = "dotnet_runtime_path_twenty_nine_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_thirty_stage_rank = @{
        Method = "dotnet_runtime_path_thirty_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_thirty_one_stage_rank = @{
        Method = "dotnet_runtime_path_thirty_one_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_thirty_two_stage_rank = @{
        Method = "dotnet_runtime_path_thirty_two_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_thirty_three_stage_rank = @{
        Method = "dotnet_runtime_path_thirty_three_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_thirty_four_stage_rank = @{
        Method = "dotnet_runtime_path_thirty_four_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_thirty_five_stage_rank = @{
        Method = "dotnet_runtime_path_thirty_five_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_thirty_six_stage_rank = @{
        Method = "dotnet_runtime_path_thirty_six_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_thirty_seven_stage_rank = @{
        Method = "dotnet_runtime_path_thirty_seven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_thirty_eight_stage_rank = @{
        Method = "dotnet_runtime_path_thirty_eight_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_thirty_nine_stage_rank = @{
        Method = "dotnet_runtime_path_thirty_nine_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_forty_stage_rank = @{
        Method = "dotnet_runtime_path_forty_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_forty_one_stage_rank = @{
        Method = "dotnet_runtime_path_forty_one_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_forty_two_stage_rank = @{
        Method = "dotnet_runtime_path_forty_two_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_forty_three_stage_rank = @{
        Method = "dotnet_runtime_path_forty_three_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_forty_four_stage_rank = @{
        Method = "dotnet_runtime_path_forty_four_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_forty_five_stage_rank = @{
        Method = "dotnet_runtime_path_forty_five_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_forty_six_stage_rank = @{
        Method = "dotnet_runtime_path_forty_six_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_forty_seven_stage_rank = @{
        Method = "dotnet_runtime_path_forty_seven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_forty_eight_stage_rank = @{
        Method = "dotnet_runtime_path_forty_eight_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_forty_nine_stage_rank = @{
        Method = "dotnet_runtime_path_forty_nine_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_fifty_stage_rank = @{
        Method = "dotnet_runtime_path_fifty_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_fifty_one_stage_rank = @{
        Method = "dotnet_runtime_path_fifty_one_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_fifty_two_stage_rank = @{
        Method = "dotnet_runtime_path_fifty_two_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_fifty_three_stage_rank = @{
        Method = "dotnet_runtime_path_fifty_three_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_fifty_four_stage_rank = @{
        Method = "dotnet_runtime_path_fifty_four_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_fifty_five_stage_rank = @{
        Method = "dotnet_runtime_path_fifty_five_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_fifty_six_stage_rank = @{
        Method = "dotnet_runtime_path_fifty_six_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_fifty_seven_stage_rank = @{
        Method = "dotnet_runtime_path_fifty_seven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_fifty_eight_stage_rank = @{
        Method = "dotnet_runtime_path_fifty_eight_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_fifty_nine_stage_rank = @{
        Method = "dotnet_runtime_path_fifty_nine_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_sixty_stage_rank = @{
        Method = "dotnet_runtime_path_sixty_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_sixty_one_stage_rank = @{
        Method = "dotnet_runtime_path_sixty_one_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_sixty_two_stage_rank = @{
        Method = "dotnet_runtime_path_sixty_two_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_sixty_three_stage_rank = @{
        Method = "dotnet_runtime_path_sixty_three_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_sixty_four_stage_rank = @{
        Method = "dotnet_runtime_path_sixty_four_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_sixty_five_stage_rank = @{
        Method = "dotnet_runtime_path_sixty_five_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_sixty_six_stage_rank = @{
        Method = "dotnet_runtime_path_sixty_six_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_sixty_seven_stage_rank = @{
        Method = "dotnet_runtime_path_sixty_seven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_sixty_eight_stage_rank = @{
        Method = "dotnet_runtime_path_sixty_eight_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_sixty_nine_stage_rank = @{
        Method = "dotnet_runtime_path_sixty_nine_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_seventy_stage_rank = @{
        Method = "dotnet_runtime_path_seventy_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_seventy_one_stage_rank = @{
        Method = "dotnet_runtime_path_seventy_one_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_seventy_two_stage_rank = @{
        Method = "dotnet_runtime_path_seventy_two_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_seventy_three_stage_rank = @{
        Method = "dotnet_runtime_path_seventy_three_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_seventy_four_stage_rank = @{
        Method = "dotnet_runtime_path_seventy_four_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_seventy_five_stage_rank = @{
        Method = "dotnet_runtime_path_seventy_five_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_seventy_six_stage_rank = @{
        Method = "dotnet_runtime_path_seventy_six_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_seventy_seven_stage_rank = @{
        Method = "dotnet_runtime_path_seventy_seven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_seventy_eight_stage_rank = @{
        Method = "dotnet_runtime_path_seventy_eight_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_seventy_nine_stage_rank = @{
        Method = "dotnet_runtime_path_seventy_nine_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eighty_stage_rank = @{
        Method = "dotnet_runtime_path_eighty_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eighty_one_stage_rank = @{
        Method = "dotnet_runtime_path_eighty_one_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eighty_two_stage_rank = @{
        Method = "dotnet_runtime_path_eighty_two_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eighty_three_stage_rank = @{
        Method = "dotnet_runtime_path_eighty_three_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eighty_four_stage_rank = @{
        Method = "dotnet_runtime_path_eighty_four_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eighty_five_stage_rank = @{
        Method = "dotnet_runtime_path_eighty_five_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eighty_six_stage_rank = @{
        Method = "dotnet_runtime_path_eighty_six_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eighty_seven_stage_rank = @{
        Method = "dotnet_runtime_path_eighty_seven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eighty_eight_stage_rank = @{
        Method = "dotnet_runtime_path_eighty_eight_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_eighty_nine_stage_rank = @{
        Method = "dotnet_runtime_path_eighty_nine_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_ninety_stage_rank = @{
        Method = "dotnet_runtime_path_ninety_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_ninety_one_stage_rank = @{
        Method = "dotnet_runtime_path_ninety_one_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_ninety_two_stage_rank = @{
        Method = "dotnet_runtime_path_ninety_two_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_ninety_three_stage_rank = @{
        Method = "dotnet_runtime_path_ninety_three_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_ninety_four_stage_rank = @{
        Method = "dotnet_runtime_path_ninety_four_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_ninety_five_stage_rank = @{
        Method = "dotnet_runtime_path_ninety_five_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_ninety_six_stage_rank = @{
        Method = "dotnet_runtime_path_ninety_six_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_ninety_seven_stage_rank = @{
        Method = "dotnet_runtime_path_ninety_seven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_ninety_eight_stage_rank = @{
        Method = "dotnet_runtime_path_ninety_eight_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_ninety_nine_stage_rank = @{
        Method = "dotnet_runtime_path_ninety_nine_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_one_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_one_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_two_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_two_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_three_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_three_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_four_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_four_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_five_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_five_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_six_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_six_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_seven_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_seven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_eight_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_eight_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_nine_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_nine_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_ten_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_ten_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_eleven_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_eleven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_twelve_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_twelve_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_thirteen_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_thirteen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_fourteen_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_fourteen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_fifteen_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_fifteen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_sixteen_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_sixteen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_seventeen_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_seventeen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_eighteen_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_eighteen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_nineteen_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_nineteen_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_twenty_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_twenty_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_twenty_one_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_twenty_one_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_twenty_two_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_twenty_two_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_twenty_three_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_twenty_three_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_twenty_four_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_twenty_four_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_twenty_five_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_twenty_five_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_twenty_six_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_twenty_six_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_twenty_seven_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_twenty_seven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_twenty_eight_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_twenty_eight_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_twenty_nine_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_twenty_nine_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_thirty_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_thirty_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_thirty_one_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_thirty_one_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_thirty_two_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_thirty_two_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_thirty_three_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_thirty_three_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_thirty_four_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_thirty_four_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_thirty_five_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_thirty_five_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_thirty_six_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_thirty_six_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_thirty_seven_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_thirty_seven_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_thirty_eight_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_thirty_eight_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_thirty_nine_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_thirty_nine_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_forty_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_forty_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_forty_one_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_forty_one_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_forty_two_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_forty_two_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
    }
    dotnet_runtime_path_one_hundred_forty_three_stage_rank = @{
        Method = "dotnet_runtime_path_one_hundred_forty_three_stage_rank_score"
        Arguments = @()
        Expected = 332232222
        SupportedModes = @("Cargo")
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
    async_state_machine = @{
        Method = "retry_backoff"
        Arguments = @(0, 5, 3)
        Expected = 3
    }
    enum_complex = @{
        Method = "enum_complex_nested_option"
        Arguments = @(0)
        Expected = 0
    }
    error_propagation = @{
        Method = "error_prop_success"
        Arguments = @()
        Expected = 0
    }
    generic_collections = @{
        Method = "pair_swap_sum"
        Arguments = @(3, 4)
        Expected = 7
    }
    global_ptr_pass = @{
        Method = "global_ptr_pass_probe"
        Arguments = @(1)
        Expected = 43
    }
    iterator_chain = @{
        Method = "iter_map_filter_sum"
        Arguments = @(5)
        Expected = 30
    }
    string_vec_ops = @{
        Method = "vec_push_sum"
        Arguments = @(4)
        Expected = 10
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

    Invoke-RustlynTool inspect $ArtifactPath
    if ($LASTEXITCODE -ne 0) {
        throw "rustlyn inspect failed for '$CurrentSample' with exit code $LASTEXITCODE"
    }

    Invoke-RustlynTool lower $ArtifactPath | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "rustlyn lower failed for '$CurrentSample' with exit code $LASTEXITCODE"
    }

    $emittedAssemblyPath = Join-Path ([System.IO.Path]::GetTempPath()) ("rustlyn-smoke-{0}-{1}.dll" -f $CurrentSample, [Guid]::NewGuid().ToString("N"))
    try {
        Invoke-RustlynTool emit $ArtifactPath --out $emittedAssemblyPath | Out-Null
        if ($LASTEXITCODE -ne 0) {
            throw "rustlyn emit failed for '$CurrentSample' with exit code $LASTEXITCODE"
        }

        # Prefer invoking through Rustlyn.Tool when the argument shape is supported.
        # This avoids loading the emitted .NET 10 assembly into the host pwsh process,
        # which may run on an older .NET runtime (e.g. pwsh 7.4 on .NET 8) and reject
        # the System.Runtime 10.0.0.0 reference. The in-process path below remains as
        # a fallback for arg shapes the invoke tool does not yet support.
        $invokeToolArgs = @()
        $invokeToolEligible = $true
        foreach ($candidate in $arguments) {
            if ($candidate -is [int] -or $candidate -is [Int32]) {
                $invokeToolArgs += "i32:$([int]$candidate)"
            }
            elseif ($candidate -is [long] -or $candidate -is [Int64]) {
                $invokeToolArgs += "i64:$([long]$candidate)"
            }
            elseif ($candidate -is [uint] -or $candidate -is [UInt32]) {
                $invokeToolArgs += "u32:$([uint]$candidate)"
            }
            elseif ($candidate -is [ulong] -or $candidate -is [UInt64]) {
                $invokeToolArgs += "u64:$([ulong]$candidate)"
            }
            else {
                $invokeToolEligible = $false
                break
            }
        }

        if ($invokeToolEligible) {
            $invokeCli = @('invoke', $ArtifactPath, '--method', $Check.Method)
            foreach ($encoded in $invokeToolArgs) {
                $invokeCli += '--arg'
                $invokeCli += $encoded
            }

            $invokeOutput = Invoke-RustlynTool @invokeCli
            if ($LASTEXITCODE -ne 0) {
                throw "rustlyn invoke failed for '$CurrentSample' with exit code $LASTEXITCODE"
            }

            $actualText = if ($invokeOutput -is [array]) { ($invokeOutput | Where-Object { $_ -ne $null } | Select-Object -Last 1) } else { $invokeOutput }
            $actualText = "$actualText".Trim()
            if ($Check.ContainsKey("ExpectedMinimum")) {
                if ([Int64]$actualText -lt [Int64]$Check.ExpectedMinimum) {
                    throw "Generated method '$($Check.Method)' returned '$actualText' for '$CurrentSample', expected at least '$($Check.ExpectedMinimum)'."
                }
            }
            else {
                $expectedText = "$($Check.Expected)".Trim()
                if ($actualText -ne $expectedText) {
                    throw "Generated method '$($Check.Method)' returned '$actualText' for '$CurrentSample', expected '$expectedText'."
                }
            }

            Write-Host ("PASS {0} => {1}" -f $CurrentSample, $actualText)
            return
        }

        $loadContext = [System.Runtime.Loader.AssemblyLoadContext]::new("rustlyn-smoke-$CurrentSample-$([Guid]::NewGuid().ToString('N'))", $true)
        try {
            $assemblyBytes = [System.IO.File]::ReadAllBytes($emittedAssemblyPath)
            $assemblyStream = [System.IO.MemoryStream]::new($assemblyBytes)
            try {
                $assembly = $loadContext.LoadFromStream($assemblyStream)
            }
            finally {
                $assemblyStream.Dispose()
            }

            $generatedType = $assembly.GetType("Rustlyn.GeneratedModule", $true)
            $method = $generatedType.GetMethod($Check.Method, [System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::Static)
            if ($null -eq $method) {
                throw "Generated method '$($Check.Method)' was not found for '$CurrentSample'."
            }

            $actual = $method.Invoke($null, $arguments)
            if ($Check.ContainsKey("ExpectedMinimum")) {
                if ([Int64]$actual -lt [Int64]$Check.ExpectedMinimum) {
                    throw "Generated method '$($Check.Method)' returned '$actual' for '$CurrentSample', expected at least '$($Check.ExpectedMinimum)'."
                }
            }
            elseif ($actual -ne $Check.Expected) {
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

    $translatedOutputDirectory = Join-Path ([System.IO.Path]::GetTempPath()) ("rustlyn-translate-smoke-{0}-{1}" -f $CurrentSample, [Guid]::NewGuid().ToString("N"))
    $translatedBitcodePath = Join-Path $translatedOutputDirectory ("{0}.bc" -f $CurrentSample)
    $translatedAssemblyPath = Join-Path $translatedOutputDirectory ("{0}.dll" -f $CurrentSample)
    $translatedRuntimeConfigPath = [System.IO.Path]::ChangeExtension($translatedAssemblyPath, ".runtimeconfig.json")
    $translatedBackendAssemblyPath = Join-Path ([System.IO.Path]::GetDirectoryName($translatedAssemblyPath)) "Rustlyn.Backend.dll"
    try {
        New-Item -ItemType Directory -Path $translatedOutputDirectory -Force | Out-Null

        $translateCommand = @(
            'translate',
            $CratePath,
            '--out',
            $translatedAssemblyPath,
            '--bitcode-out',
            $translatedBitcodePath
        )

        if ($Check.ContainsKey("BinaryTarget")) {
            $translateCommand += '--bin'
            $translateCommand += $Check.BinaryTarget
        }

        if ($Check.ContainsKey("Toolchain")) {
            $translateCommand += '--toolchain'
            $translateCommand += $Check.Toolchain
        }

        if ($Check.ContainsKey("Target")) {
            $translateCommand += '--target'
            $translateCommand += $Check.Target
        }

        if ($Check.ContainsKey("BuildStd")) {
            $translateCommand += '--build-std'
            $translateCommand += $Check.BuildStd
        }

        if ($Check.ContainsKey("BuildStdFeatures")) {
            $translateCommand += '--build-std-features'
            $translateCommand += $Check.BuildStdFeatures
        }

        Invoke-RustlynTool @translateCommand | Out-Null
        if ($LASTEXITCODE -ne 0) {
            throw "rustlyn translate failed for '$CurrentSample' with exit code $LASTEXITCODE"
        }

        Invoke-RustlynTool inspect $translatedBitcodePath
        if ($LASTEXITCODE -ne 0) {
            throw "rustlyn inspect failed for translated '$CurrentSample' with exit code $LASTEXITCODE"
        }

        Invoke-RustlynTool lower $translatedBitcodePath | Out-Null
        if ($LASTEXITCODE -ne 0) {
            throw "rustlyn lower failed for translated '$CurrentSample' with exit code $LASTEXITCODE"
        }

        if ($Check.ContainsKey("ExpectedOutput")) {
            if (-not (Test-Path $translatedRuntimeConfigPath)) {
                throw "Translated console sample '$CurrentSample' did not produce runtimeconfig '$translatedRuntimeConfigPath'."
            }

            if (-not (Test-Path $translatedBackendAssemblyPath)) {
                throw "Translated console sample '$CurrentSample' did not copy runtime support assembly '$translatedBackendAssemblyPath'."
            }

            $consoleOutput = & dotnet $translatedAssemblyPath @arguments 2>&1
            if ($LASTEXITCODE -ne 0) {
                throw "dotnet run translated console failed for '$CurrentSample' with exit code $LASTEXITCODE`n$($consoleOutput | Out-String)"
            }

            $actual = (($consoleOutput | ForEach-Object { $_.ToString() }) | Where-Object { $_.Trim().Length -gt 0 }) -join [Environment]::NewLine
            $expectedOutput = if ($Check.ExpectedOutput -is [System.Array]) {
                (($Check.ExpectedOutput | ForEach-Object { $_.ToString() }) | Where-Object { $_.Trim().Length -gt 0 }) -join [Environment]::NewLine
            }
            else {
                [string]$Check.ExpectedOutput
            }

            if ($actual -ne $expectedOutput) {
                throw "Translated console sample '$CurrentSample' printed '$actual', expected '$expectedOutput'."
            }

            Write-Host ("PASS {0} (cargo console) => {1}" -f $CurrentSample, $actual)
        }
        else {
            $invokeCommand = @(
                'invoke',
                $translatedBitcodePath,
                '--method',
                $Check.Method
            )

            foreach ($argument in $arguments) {
                $invokeCommand += '--arg'
                $invokeCommand += (Format-InvokeArgument -Value $argument)
            }

            $invokeOutput = Invoke-RustlynTool @invokeCommand 2>&1
            if ($LASTEXITCODE -ne 0) {
                throw "rustlyn invoke failed for translated '$CurrentSample' with exit code $LASTEXITCODE`n$($invokeOutput | Out-String)"
            }

            $actual = (($invokeOutput | ForEach-Object { $_.ToString() }) | Where-Object { $_.Trim().Length -gt 0 } | Select-Object -Last 1)
            if ($Check.ContainsKey("ExpectedMinimum")) {
                if ([Int64]$actual -lt [Int64]$Check.ExpectedMinimum) {
                    throw "Translated method '$($Check.Method)' returned '$actual' for '$CurrentSample', expected at least '$($Check.ExpectedMinimum)'."
                }
            }
            elseif ($actual -ne [string]$Check.Expected) {
                throw "Translated method '$($Check.Method)' returned '$actual' for '$CurrentSample', expected '$($Check.Expected)'."
            }

            Write-Host ("PASS {0} (cargo) => {1}" -f $CurrentSample, $actual)
        }
    }
    finally {
        if ($Check.ContainsKey("Cleanup")) {
            & $Check.Cleanup $cleanupState
        }

        if (Test-Path $translatedOutputDirectory) {
            Remove-Item -Recurse -Force $translatedOutputDirectory
        }
    }
}

function Format-InvokeArgument {
    param(
        [Parameter(Mandatory = $true)]
        [object]$Value
    )

    if ($Value -is [int]) {
        return "i32:$Value"
    }

    if ($Value -is [long]) {
        return "i64:$Value"
    }

    if ($Value -is [uint32]) {
        return "u32:$Value"
    }

    if ($Value -is [uint64]) {
        return "u64:$Value"
    }

    if ($Value -is [bool]) {
        return "i1:$([int]$Value)"
    }

    if ($Value -is [float]) {
        return "f32:$Value"
    }

    if ($Value -is [double]) {
        return "f64:$Value"
    }

    throw "Smoke invoke argument type '$($Value.GetType().FullName)' is not supported."
}

Push-Location $workspaceRoot
try {
    if ($ToolDll) {
        $script:RustlynToolDll = Resolve-RustlynToolDll -CandidatePath $ToolDll
    }
    else {
        dotnet build -c $Configuration $toolProject -p:UseSharedCompilation=false
        if ($LASTEXITCODE -ne 0) {
            throw "dotnet build failed with exit code $LASTEXITCODE"
        }

        $script:RustlynToolDll = Resolve-RustlynToolDll -CandidatePath $defaultToolDll
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
            $cratePath = if ($check.ContainsKey("CratePath")) {
                $check.CratePath
            }
            else {
                Join-Path $workspaceRoot (Join-Path "samples" $currentSample)
            }
            Invoke-TranslateSmokeCheck -CurrentSample $currentSample -CratePath $cratePath -Check $check
            continue
        }

        $artifactPath = & (Join-Path $workspaceRoot "scripts\Build-SampleBitcode.ps1") -Sample $currentSample
        if ($Stage -eq "Inspect") {
            Invoke-RustlynTool inspect $artifactPath
            if ($LASTEXITCODE -ne 0) {
                throw "rustlyn inspect failed for '$currentSample' with exit code $LASTEXITCODE"
            }

            Write-Host ("PASS {0} (inspect)" -f $currentSample)
            continue
        }

        Invoke-SmokeCheck -CurrentSample $currentSample -ArtifactPath $artifactPath -Check $check
    }
}
finally {
    Pop-Location
}
