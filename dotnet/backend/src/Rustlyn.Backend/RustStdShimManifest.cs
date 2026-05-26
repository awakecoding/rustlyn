namespace Rustlyn.Backend;

internal static class RustStdShimManifest
{
    public static IReadOnlyList<RustStdShimEntry> Aliases { get; } =
    [
        new("rustlyn_std_pathbuf_to_owned", nameof(RuntimeBridgeHelpers.InitializeWtf8PathBuffer), "std-abi-specialized"),
        new("rustlyn_std_path_file_name", nameof(RuntimeBridgeHelpers.StdPathFileName), "std-abi-specialized"),
        new("rustlyn_std_path_file_stem", nameof(RuntimeBridgeHelpers.StdPathFileStem), "std-abi-specialized"),
        new("rustlyn_std_path_extension", nameof(RuntimeBridgeHelpers.StdPathExtension), "std-abi-specialized"),
        new("rustlyn_std_path_parent", nameof(RuntimeBridgeHelpers.StdPathParent), "std-abi-specialized"),
        new("rustlyn_std_path_components", nameof(RuntimeBridgeHelpers.StdPathComponents), "std-abi-specialized"),
        new("rustlyn_std_path_is_absolute", nameof(RuntimeBridgeHelpers.StdPathIsAbsolute), "std-abi-specialized"),
        new("rustlyn_std_path_components_eq", nameof(RuntimeBridgeHelpers.StdPathComponentsEq), "std-abi-specialized"),
        new("rustlyn_std_path_eq", nameof(RuntimeBridgeHelpers.StdPathEq), "std-abi-specialized"),
        new("rustlyn_std_path_starts_with", nameof(RuntimeBridgeHelpers.StdPathStartsWith), "std-abi-specialized"),
        new("rustlyn_std_path_ends_with", nameof(RuntimeBridgeHelpers.StdPathEndsWith), "std-abi-specialized"),
        new("rustlyn_std_path_join", nameof(RuntimeBridgeHelpers.StdPathJoin), "std-abi-specialized"),
        new("rustlyn_std_path_with_extension", nameof(RuntimeBridgeHelpers.StdPathWithExtension), "std-abi-specialized"),
        new("rustlyn_std_path_with_file_name", nameof(RuntimeBridgeHelpers.StdPathWithFileName), "std-abi-specialized"),
        new("_ZN4core4iter6traits8iterator8Iterator4fold17hbc92b954ca3f68c4E", nameof(RuntimeBridgeHelpers.StdPathComponentCount), "std-abi-specialized"),
        new("rustlyn_std_pathbuf_push", nameof(RuntimeBridgeHelpers.AppendPathSegment), "std-abi-specialized"),
        new("rustlyn_std_fs_read_to_string_inner", nameof(RuntimeBridgeHelpers.ReadFileToRustString), "std-abi-specialized"),
        new("rustlyn_core_slice_memchr_aligned", nameof(RuntimeBridgeHelpers.MemchrAligned), "runtime-specialized"),
        new("rustlyn_std_io_print", nameof(RuntimeBridgeHelpers.StdIoPrint), "std-abi-specialized"),
        new("rustlyn_std_io_eprint", nameof(RuntimeBridgeHelpers.StdIoEPrint), "std-abi-specialized"),
        new("rustlyn_std_io_stdout", nameof(RuntimeBridgeHelpers.StdIoStdout), "std-abi-specialized"),
        new("rustlyn_std_io_stdout_write_all", nameof(RuntimeBridgeHelpers.StdIoStdoutWriteAll), "std-abi-specialized"),
        new("rustlyn_std_io_stderr_write_all", nameof(RuntimeBridgeHelpers.StdIoStderrWriteAll), "std-abi-specialized"),
        new("rustlyn_std_io_stdout_flush", nameof(RuntimeBridgeHelpers.StdIoStdoutFlush), "std-abi-specialized"),
        new("rustlyn_std_time_instant_now", nameof(RuntimeBridgeHelpers.StdTimeInstantNow), "std-abi-specialized"),
        new("rustlyn_std_time_instant_elapsed", nameof(RuntimeBridgeHelpers.StdTimeInstantElapsed), "std-abi-specialized"),
        new("rustlyn_std_env_current_dir", nameof(RuntimeBridgeHelpers.StdEnvCurrentDir), "std-abi-specialized"),
        new("rustlyn_std_env_temp_dir", nameof(RuntimeBridgeHelpers.StdEnvTempDir), "std-abi-specialized"),
        new("rustlyn_std_env_var", nameof(RuntimeBridgeHelpers.StdEnvVar), "std-abi-specialized")
    ];
}

internal sealed record RustStdShimEntry(string Symbol, string HelperMethodName, string Classification);
