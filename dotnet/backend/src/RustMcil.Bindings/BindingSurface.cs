using RustMcil.Interop;

namespace RustMcil.Bindings;

public sealed record BindingSurface(
    IReadOnlyList<ManagedApiRequirement> Requirements,
    IReadOnlyList<RustExternBinding> Externs,
    IReadOnlyList<ManagedGlueBinding> ManagedGlueBindings,
    IReadOnlyList<RustWrapperMethod> RustWrapperMethods)
{
    public static BindingSurface CreateTinyBclSurface()
    {
        return new BindingSurface(
            [
                ManagedApiRequirement.Method("System.Console.WriteLine(string)", typeof(Console), nameof(Console.WriteLine), [typeof(string)]),
                ManagedApiRequirement.Method("System.Environment.GetCommandLineArgs()", typeof(Environment), nameof(Environment.GetCommandLineArgs), []),
                ManagedApiRequirement.Property("System.Environment.CurrentDirectory", typeof(Environment), nameof(Environment.CurrentDirectory)),
                ManagedApiRequirement.Method("System.IO.Directory.GetCurrentDirectory()", typeof(Directory), nameof(Directory.GetCurrentDirectory), []),
                ManagedApiRequirement.Method("System.IO.File.ReadAllLines(string)", typeof(File), nameof(File.ReadAllLines), [typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.ChangeExtension(string, string)", typeof(Path), nameof(Path.ChangeExtension), [typeof(string), typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.Combine(string, string)", typeof(Path), nameof(Path.Combine), [typeof(string), typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.EndsInDirectorySeparator(string)", typeof(Path), nameof(Path.EndsInDirectorySeparator), [typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.GetDirectoryName(string)", typeof(Path), nameof(Path.GetDirectoryName), [typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.GetExtension(string)", typeof(Path), nameof(Path.GetExtension), [typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.GetFileName(string)", typeof(Path), nameof(Path.GetFileName), [typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.GetFileNameWithoutExtension(string)", typeof(Path), nameof(Path.GetFileNameWithoutExtension), [typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.GetFullPath(string)", typeof(Path), nameof(Path.GetFullPath), [typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.GetPathRoot(string)", typeof(Path), nameof(Path.GetPathRoot), [typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.GetRelativePath(string, string)", typeof(Path), nameof(Path.GetRelativePath), [typeof(string), typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.GetTempPath()", typeof(Path), nameof(Path.GetTempPath), []),
                ManagedApiRequirement.Method("System.IO.Path.HasExtension(string)", typeof(Path), nameof(Path.HasExtension), [typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.IsPathFullyQualified(string)", typeof(Path), nameof(Path.IsPathFullyQualified), [typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.IsPathRooted(string)", typeof(Path), nameof(Path.IsPathRooted), [typeof(string)]),
                ManagedApiRequirement.Method("System.MathF.Sqrt(float)", typeof(MathF), nameof(MathF.Sqrt), [typeof(float)]),
                ManagedApiRequirement.Method("System.String.Contains(string, StringComparison)", typeof(string), nameof(string.Contains), [typeof(string), typeof(StringComparison)]),
                ManagedApiRequirement.Property("System.String.Length", typeof(string), nameof(string.Length)),
                ManagedApiRequirement.ForType("System.String", typeof(string)),
                ManagedApiRequirement.ForType("System.String[]", typeof(string[]))
            ],
            [
                new RustExternBinding(
                    "rust_mcil_bindgen_system_console_write_line_utf8",
                    ["fn rust_mcil_bindgen_system_console_write_line_utf8(value_ptr: *const u8, value_len: i64) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_console_write_line_string",
                    ["fn rust_mcil_bindgen_system_console_write_line_string(handle: i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_environment_get_command_line_args",
                    ["fn rust_mcil_bindgen_system_environment_get_command_line_args(exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_environment_current_directory",
                    ["fn rust_mcil_bindgen_system_environment_current_directory(exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_directory_get_current_directory",
                    ["fn rust_mcil_bindgen_system_io_directory_get_current_directory(exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_file_read_all_lines_utf8",
                    [
                        "fn rust_mcil_bindgen_system_io_file_read_all_lines_utf8(",
                        "    path_ptr: *const u8,",
                        "    path_len: i64,",
                        "    exception_out: *mut i32,",
                        ") -> i32;"
                    ]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_file_read_all_lines_string",
                    ["fn rust_mcil_bindgen_system_io_file_read_all_lines_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_change_extension_string_string",
                    ["fn rust_mcil_bindgen_system_io_path_change_extension_string_string(path_handle: i32, extension_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_combine_string_string",
                    ["fn rust_mcil_bindgen_system_io_path_combine_string_string(path1_handle: i32, path2_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_ends_in_directory_separator_string",
                    ["fn rust_mcil_bindgen_system_io_path_ends_in_directory_separator_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_get_directory_name_string",
                    ["fn rust_mcil_bindgen_system_io_path_get_directory_name_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_get_extension_string",
                    ["fn rust_mcil_bindgen_system_io_path_get_extension_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_get_file_name_string",
                    ["fn rust_mcil_bindgen_system_io_path_get_file_name_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_get_file_name_without_extension_string",
                    ["fn rust_mcil_bindgen_system_io_path_get_file_name_without_extension_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_get_full_path_string",
                    ["fn rust_mcil_bindgen_system_io_path_get_full_path_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_get_path_root_string",
                    ["fn rust_mcil_bindgen_system_io_path_get_path_root_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_get_relative_path_string_string",
                    ["fn rust_mcil_bindgen_system_io_path_get_relative_path_string_string(relative_to_handle: i32, path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_get_temp_path",
                    ["fn rust_mcil_bindgen_system_io_path_get_temp_path(exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_has_extension_string",
                    ["fn rust_mcil_bindgen_system_io_path_has_extension_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_is_path_fully_qualified_string",
                    ["fn rust_mcil_bindgen_system_io_path_is_path_fully_qualified_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_is_path_rooted_string",
                    ["fn rust_mcil_bindgen_system_io_path_is_path_rooted_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_mathf_sqrt_f32",
                    ["fn rust_mcil_bindgen_system_mathf_sqrt_f32(value: f32, exception_out: *mut i32) -> f32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_string_from_utf8",
                    ["fn rust_mcil_bindgen_system_string_from_utf8(value_ptr: *const u8, value_len: i64, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_string_len",
                    ["fn rust_mcil_bindgen_system_string_len(handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_string_utf8_len",
                    ["fn rust_mcil_bindgen_system_string_utf8_len(handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_string_copy_utf8",
                    [
                        "fn rust_mcil_bindgen_system_string_copy_utf8(",
                        "    handle: i32,",
                        "    destination_ptr: *mut u8,",
                        "    destination_capacity: i64,",
                        "    exception_out: *mut i32,",
                        ") -> i32;"
                    ]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_string_contains_utf8",
                    [
                        "fn rust_mcil_bindgen_system_string_contains_utf8(",
                        "    handle: i32,",
                        "    needle_ptr: *const u8,",
                        "    needle_len: i64,",
                        "    exception_out: *mut i32,",
                        ") -> i32;"
                    ]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_string_contains_string",
                    [
                        "fn rust_mcil_bindgen_system_string_contains_string(",
                        "    handle: i32,",
                        "    needle_handle: i32,",
                        "    exception_out: *mut i32,",
                        ") -> i32;"
                    ]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_string_array_len",
                    ["fn rust_mcil_bindgen_system_string_array_len(handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_string_array_get",
                    [
                        "fn rust_mcil_bindgen_system_string_array_get(",
                        "    handle: i32,",
                        "    index: i32,",
                        "    exception_out: *mut i32,",
                        ") -> i32;"
                    ]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_object_release",
                    ["fn rust_mcil_bindgen_system_object_release(handle: i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_exception_release",
                    ["fn rust_mcil_bindgen_system_exception_release(handle: i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_exception_get_type_name_utf8_len",
                    ["fn rust_mcil_bindgen_system_exception_get_type_name_utf8_len(handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_exception_get_type_name_utf8",
                    [
                        "fn rust_mcil_bindgen_system_exception_get_type_name_utf8(",
                        "    handle: i32,",
                        "    destination_ptr: *mut u8,",
                        "    destination_capacity: i64,",
                        "    exception_out: *mut i32,",
                        ") -> i32;"
                    ]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_exception_get_message_utf8_len",
                    ["fn rust_mcil_bindgen_system_exception_get_message_utf8_len(handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_exception_get_message_utf8",
                    [
                        "fn rust_mcil_bindgen_system_exception_get_message_utf8(",
                        "    handle: i32,",
                        "    destination_ptr: *mut u8,",
                        "    destination_capacity: i64,",
                        "    exception_out: *mut i32,",
                        ") -> i32;"
                    ])
            ],
            [
                Glue(
                    "rust_mcil_bindgen_system_console_write_line_utf8",
                    "BindgenSystemConsoleWriteLineUtf8",
                    [Pointer("valuePointer"), I64("valueLength")],
                    ManagedGlueOperation.ReturnExceptionHandle(ManagedGlueResult.VoidCall(
                        StaticMethod(typeof(Console), nameof(Console.WriteLine), [typeof(string)], [Utf8String("valuePointer", "valueLength")])))),
                Glue(
                    "rust_mcil_bindgen_system_console_write_line_string",
                    "BindgenSystemConsoleWriteLineString",
                    [I32("stringHandle")],
                    ManagedGlueOperation.ReturnExceptionHandle(ManagedGlueResult.VoidCall(
                        StaticMethod(typeof(Console), nameof(Console.WriteLine), [typeof(string)], [ManagedObject(typeof(string), "stringHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_environment_get_command_line_args",
                    "BindgenSystemEnvironmentGetCommandLineArgs",
                    [Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Environment), nameof(Environment.GetCommandLineArgs), [], [])))),
                Glue(
                    "rust_mcil_bindgen_system_environment_current_directory",
                    "BindgenSystemEnvironmentCurrentDirectory",
                    [Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticProperty(typeof(Environment), nameof(Environment.CurrentDirectory))))),
                Glue(
                    "rust_mcil_bindgen_system_io_directory_get_current_directory",
                    "BindgenSystemIoDirectoryGetCurrentDirectory",
                    [Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Directory), nameof(Directory.GetCurrentDirectory), [], [])))),
                Glue(
                    "rust_mcil_bindgen_system_io_file_read_all_lines_utf8",
                    "BindgenSystemIoFileReadAllLinesUtf8",
                    [Pointer("pathPointer"), I64("pathLength"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(File), nameof(File.ReadAllLines), [typeof(string)], [Utf8String("pathPointer", "pathLength")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_file_read_all_lines_string",
                    "BindgenSystemIoFileReadAllLinesString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(File), nameof(File.ReadAllLines), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_change_extension_string_string",
                    "BindgenSystemIoPathChangeExtensionStringString",
                    [I32("pathHandle"), I32("extensionHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.ChangeExtension), [typeof(string), typeof(string)], [ManagedObject(typeof(string), "pathHandle"), ManagedObject(typeof(string), "extensionHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_combine_string_string",
                    "BindgenSystemIoPathCombineStringString",
                    [I32("path1Handle"), I32("path2Handle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.Combine), [typeof(string), typeof(string)], [ManagedObject(typeof(string), "path1Handle"), ManagedObject(typeof(string), "path2Handle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_ends_in_directory_separator_string",
                    "BindgenSystemIoPathEndsInDirectorySeparatorString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        StaticMethod(typeof(Path), nameof(Path.EndsInDirectorySeparator), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_get_directory_name_string",
                    "BindgenSystemIoPathGetDirectoryNameString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetDirectoryName), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_get_extension_string",
                    "BindgenSystemIoPathGetExtensionString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetExtension), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_get_file_name_string",
                    "BindgenSystemIoPathGetFileNameString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetFileName), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_get_file_name_without_extension_string",
                    "BindgenSystemIoPathGetFileNameWithoutExtensionString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetFileNameWithoutExtension), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_get_full_path_string",
                    "BindgenSystemIoPathGetFullPathString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetFullPath), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_get_path_root_string",
                    "BindgenSystemIoPathGetPathRootString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetPathRoot), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_get_relative_path_string_string",
                    "BindgenSystemIoPathGetRelativePathStringString",
                    [I32("relativeToHandle"), I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetRelativePath), [typeof(string), typeof(string)], [ManagedObject(typeof(string), "relativeToHandle"), ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_get_temp_path",
                    "BindgenSystemIoPathGetTempPath",
                    [Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetTempPath), [], [])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_has_extension_string",
                    "BindgenSystemIoPathHasExtensionString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        StaticMethod(typeof(Path), nameof(Path.HasExtension), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_is_path_fully_qualified_string",
                    "BindgenSystemIoPathIsPathFullyQualifiedString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        StaticMethod(typeof(Path), nameof(Path.IsPathFullyQualified), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_io_path_is_path_rooted_string",
                    "BindgenSystemIoPathIsPathRootedString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        StaticMethod(typeof(Path), nameof(Path.IsPathRooted), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_mathf_sqrt_f32",
                    "BindgenSystemMathfSqrtF32",
                    [F32("value"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Float(
                        StaticMethod(typeof(MathF), nameof(MathF.Sqrt), [typeof(float)], [ManagedGlueExpression.Parameter("value")])))),
                Glue(
                    "rust_mcil_bindgen_system_string_from_utf8",
                    "BindgenSystemStringFromUtf8",
                    [Pointer("valuePointer"), I64("valueLength"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        Utf8String("valuePointer", "valueLength")))),
                Glue(
                    "rust_mcil_bindgen_system_string_len",
                    "BindgenSystemStringLength",
                    [I32("stringHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        InstanceProperty(ManagedObject(typeof(string), "stringHandle"), typeof(string), nameof(string.Length))))),
                Glue(
                    "rust_mcil_bindgen_system_string_utf8_len",
                    "BindgenSystemStringUtf8Length",
                    [I32("stringHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        ManagedGlueExpression.Utf8ByteCount(ManagedObject(typeof(string), "stringHandle"))))),
                Glue(
                    "rust_mcil_bindgen_system_string_copy_utf8",
                    "BindgenSystemStringCopyUtf8",
                    [I32("stringHandle"), Pointer("destinationPointer"), I64("destinationCapacity"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        ManagedGlueExpression.Utf8Copy(ManagedObject(typeof(string), "stringHandle"), "destinationPointer", "destinationCapacity")))),
                Glue(
                    "rust_mcil_bindgen_system_string_contains_utf8",
                    "BindgenSystemStringContainsUtf8",
                    [I32("stringHandle"), Pointer("needlePointer"), I64("needleLength"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        InstanceMethod(
                            ManagedObject(typeof(string), "stringHandle"),
                            typeof(string),
                            nameof(string.Contains),
                            [typeof(string), typeof(StringComparison)],
                            [Utf8String("needlePointer", "needleLength"), ManagedGlueExpression.EnumValue(typeof(StringComparison), nameof(StringComparison.Ordinal))])))),
                Glue(
                    "rust_mcil_bindgen_system_string_contains_string",
                    "BindgenSystemStringContainsString",
                    [I32("stringHandle"), I32("needleHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        InstanceMethod(
                            ManagedObject(typeof(string), "stringHandle"),
                            typeof(string),
                            nameof(string.Contains),
                            [typeof(string), typeof(StringComparison)],
                            [ManagedObject(typeof(string), "needleHandle"), ManagedGlueExpression.EnumValue(typeof(StringComparison), nameof(StringComparison.Ordinal))])))),
                Glue(
                    "rust_mcil_bindgen_system_string_array_len",
                    "BindgenSystemStringArrayLength",
                    [I32("arrayHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        ManagedGlueExpression.ArrayLength(ManagedObject(typeof(string[]), "arrayHandle"))))),
                Glue(
                    "rust_mcil_bindgen_system_string_array_get",
                    "BindgenSystemStringArrayGet",
                    [I32("arrayHandle"), I32("index"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        ManagedGlueExpression.ArrayElement(ManagedObject(typeof(string[]), "arrayHandle"), "index")))),
                Glue(
                    "rust_mcil_bindgen_system_object_release",
                    "BindgenSystemObjectRelease",
                    [I32("objectHandle")],
                    ManagedGlueOperation.ReturnExceptionHandle(ManagedGlueResult.ReleaseObject("objectHandle"))),
                Glue(
                    "rust_mcil_bindgen_system_exception_release",
                    "BindgenSystemExceptionRelease",
                    [I32("exceptionHandle")],
                    ManagedGlueOperation.ReturnExceptionHandle(ManagedGlueResult.ReleaseException("exceptionHandle"))),
                Glue(
                    "rust_mcil_bindgen_system_exception_get_type_name_utf8_len",
                    "BindgenSystemExceptionGetTypeNameUtf8Length",
                    [I32("exceptionHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        StaticMethod(typeof(ManagedInteropRuntime), nameof(ManagedInteropRuntime.GetExceptionTypeNameUtf8Length), [typeof(int)], [ManagedGlueExpression.Parameter("exceptionHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_exception_get_type_name_utf8",
                    "BindgenSystemExceptionGetTypeNameUtf8",
                    [I32("exceptionHandle"), Pointer("destinationPointer"), I64("destinationCapacity"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        StaticMethod(typeof(ManagedInteropRuntime), nameof(ManagedInteropRuntime.CopyExceptionTypeNameUtf8), [typeof(int), typeof(IntPtr), typeof(long)], [ManagedGlueExpression.Parameter("exceptionHandle"), ManagedGlueExpression.Parameter("destinationPointer"), ManagedGlueExpression.Parameter("destinationCapacity")])))),
                Glue(
                    "rust_mcil_bindgen_system_exception_get_message_utf8_len",
                    "BindgenSystemExceptionGetMessageUtf8Length",
                    [I32("exceptionHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        StaticMethod(typeof(ManagedInteropRuntime), nameof(ManagedInteropRuntime.GetExceptionMessageUtf8Length), [typeof(int)], [ManagedGlueExpression.Parameter("exceptionHandle")])))),
                Glue(
                    "rust_mcil_bindgen_system_exception_get_message_utf8",
                    "BindgenSystemExceptionGetMessageUtf8",
                    [I32("exceptionHandle"), Pointer("destinationPointer"), I64("destinationCapacity"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        StaticMethod(typeof(ManagedInteropRuntime), nameof(ManagedInteropRuntime.CopyExceptionMessageUtf8), [typeof(int), typeof(IntPtr), typeof(long)], [ManagedGlueExpression.Parameter("exceptionHandle"), ManagedGlueExpression.Parameter("destinationPointer"), ManagedGlueExpression.Parameter("destinationCapacity")]))))
            ],
            [
                new RustWrapperMethod(
                    RustWrapperContainer.MathF,
                    "pub fn sqrt(value: f32) -> Result<f32, Exception>",
                    "rust_mcil_bindgen_system_mathf_sqrt_f32",
                    ["value"],
                    "value",
                    RustWrapperResult.Scalar("f32")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn change_extension(path: &ManagedString, extension: &ManagedString) -> Result<ManagedString, Exception>",
                    "rust_mcil_bindgen_system_io_path_change_extension_string_string",
                    ["path.handle()", "extension.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn combine(path1: &ManagedString, path2: &ManagedString) -> Result<ManagedString, Exception>",
                    "rust_mcil_bindgen_system_io_path_combine_string_string",
                    ["path1.handle()", "path2.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn ends_in_directory_separator(path: &ManagedString) -> Result<i32, Exception>",
                    "rust_mcil_bindgen_system_io_path_ends_in_directory_separator_string",
                    ["path.handle()"],
                    "value",
                    RustWrapperResult.BooleanAsInt()),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_directory_name(path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rust_mcil_bindgen_system_io_path_get_directory_name_string",
                    ["path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_extension(path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rust_mcil_bindgen_system_io_path_get_extension_string",
                    ["path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_file_name(path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rust_mcil_bindgen_system_io_path_get_file_name_string",
                    ["path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_file_name_without_extension(path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rust_mcil_bindgen_system_io_path_get_file_name_without_extension_string",
                    ["path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_full_path(path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rust_mcil_bindgen_system_io_path_get_full_path_string",
                    ["path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_path_root(path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rust_mcil_bindgen_system_io_path_get_path_root_string",
                    ["path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_relative_path(relative_to: &ManagedString, path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rust_mcil_bindgen_system_io_path_get_relative_path_string_string",
                    ["relative_to.handle()", "path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_temp_path() -> Result<ManagedString, Exception>",
                    "rust_mcil_bindgen_system_io_path_get_temp_path",
                    [],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn has_extension(path: &ManagedString) -> Result<i32, Exception>",
                    "rust_mcil_bindgen_system_io_path_has_extension_string",
                    ["path.handle()"],
                    "value",
                    RustWrapperResult.BooleanAsInt()),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn is_path_fully_qualified(path: &ManagedString) -> Result<i32, Exception>",
                    "rust_mcil_bindgen_system_io_path_is_path_fully_qualified_string",
                    ["path.handle()"],
                    "value",
                    RustWrapperResult.BooleanAsInt()),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn is_path_rooted(path: &ManagedString) -> Result<i32, Exception>",
                    "rust_mcil_bindgen_system_io_path_is_path_rooted_string",
                    ["path.handle()"],
                    "value",
                    RustWrapperResult.BooleanAsInt())
            ]);
    }

    private static ManagedGlueBinding Glue(string symbol, string helperMethodName, IReadOnlyList<ManagedGlueParameter> parameters, ManagedGlueOperation operation)
        => new(symbol, helperMethodName, parameters, operation);

    private static ManagedGlueParameter I32(string name)
        => new("int", name);

    private static ManagedGlueParameter I64(string name)
        => new("long", name);

    private static ManagedGlueParameter F32(string name)
        => new("float", name);

    private static ManagedGlueParameter Pointer(string name)
        => new("IntPtr", name);

    private static ManagedGlueExpression Utf8String(string pointerParameterName, string lengthParameterName)
        => ManagedGlueExpression.Utf8String(pointerParameterName, lengthParameterName);

    private static ManagedGlueExpression ManagedObject(Type type, string handleParameterName)
        => ManagedGlueExpression.ManagedObject(type, handleParameterName);

    private static ManagedGlueExpression StaticMethod(Type declaringType, string methodName, IReadOnlyList<Type> parameterTypes, IReadOnlyList<ManagedGlueExpression> arguments)
        => ManagedGlueExpression.StaticMethod(declaringType, methodName, parameterTypes, arguments);

    private static ManagedGlueExpression StaticProperty(Type declaringType, string propertyName)
        => ManagedGlueExpression.StaticProperty(declaringType, propertyName);

    private static ManagedGlueExpression InstanceMethod(ManagedGlueExpression instance, Type declaringType, string methodName, IReadOnlyList<Type> parameterTypes, IReadOnlyList<ManagedGlueExpression> arguments)
        => ManagedGlueExpression.InstanceMethod(instance, declaringType, methodName, parameterTypes, arguments);

    private static ManagedGlueExpression InstanceProperty(ManagedGlueExpression instance, Type declaringType, string propertyName)
        => ManagedGlueExpression.InstanceProperty(instance, declaringType, propertyName);
}

public sealed record ManagedApiRequirement(string DisplayName, Type Type, ManagedApiRequirementKind Kind, string? MemberName, IReadOnlyList<Type> ParameterTypes)
{
    public static ManagedApiRequirement ForType(string displayName, Type type)
        => new(displayName, type, ManagedApiRequirementKind.Type, MemberName: null, []);

    public static ManagedApiRequirement Method(string displayName, Type type, string methodName, IReadOnlyList<Type> parameterTypes)
        => new(displayName, type, ManagedApiRequirementKind.Method, methodName, parameterTypes);

    public static ManagedApiRequirement Property(string displayName, Type type, string propertyName)
        => new(displayName, type, ManagedApiRequirementKind.Property, propertyName, []);

    public void Validate()
    {
        if (Kind == ManagedApiRequirementKind.Type)
        {
            if (Type.FullName is null)
            {
                throw new InvalidOperationException($"Managed binding requirement '{DisplayName}' does not resolve to a named type.");
            }

            return;
        }

        if (Kind == ManagedApiRequirementKind.Method)
        {
            _ = Type.GetMethod(MemberName ?? string.Empty, ParameterTypes.ToArray())
                ?? throw new InvalidOperationException($"Managed binding requirement '{DisplayName}' could not be resolved.");
            return;
        }

        if (Kind == ManagedApiRequirementKind.Property)
        {
            var property = Type.GetProperty(MemberName ?? string.Empty)
                ?? throw new InvalidOperationException($"Managed binding requirement '{DisplayName}' could not be resolved.");
            if (property.GetMethod is null)
            {
                throw new InvalidOperationException($"Managed binding requirement '{DisplayName}' does not expose a getter.");
            }

            return;
        }

        throw new NotSupportedException($"Managed binding requirement kind '{Kind}' is not supported.");
    }
}

public enum ManagedApiRequirementKind
{
    Type,
    Method,
    Property
}

public sealed record RustExternBinding(string Symbol, IReadOnlyList<string> SignatureLines);

public sealed record RustWrapperMethod(
    RustWrapperContainer Container,
    string Signature,
    string ExternSymbol,
    IReadOnlyList<string> Arguments,
    string ResultVariableName,
    RustWrapperResult Result);

public enum RustWrapperContainer
{
    MathF,
    IoPath
}

public sealed record RustWrapperResult(RustWrapperResultKind Kind, string? RustType)
{
    public static RustWrapperResult BooleanAsInt()
        => new(RustWrapperResultKind.BooleanAsInt, RustType: null);

    public static RustWrapperResult ObjectHandle(string rustType)
        => new(RustWrapperResultKind.ObjectHandle, rustType);

    public static RustWrapperResult Scalar(string rustType)
        => new(RustWrapperResultKind.Scalar, rustType);
}

public enum RustWrapperResultKind
{
    BooleanAsInt,
    ObjectHandle,
    Scalar
}

public sealed record ManagedGlueBinding(
    string Symbol,
    string RuntimeBridgeHelperMethodName,
    IReadOnlyList<ManagedGlueParameter> Parameters,
    ManagedGlueOperation Operation)
{
    public string ReturnType => Operation.Result switch
    {
        ManagedGlueFloatResult => "float",
        ManagedGlueDoubleResult => "double",
        _ => "int"
    };

    public void Validate()
    {
        if (Operation.ExceptionConvention == ManagedGlueExceptionConvention.WriteExceptionOut)
        {
            if (Operation.ExceptionOutParameterName is null)
            {
                throw new InvalidOperationException($"Managed glue binding '{Symbol}' uses the exception-out convention without an exception-out parameter name.");
            }

            if (!Parameters.Any(parameter => string.Equals(parameter.Name, Operation.ExceptionOutParameterName, StringComparison.Ordinal)))
            {
                throw new InvalidOperationException($"Managed glue binding '{Symbol}' does not declare exception-out parameter '{Operation.ExceptionOutParameterName}'.");
            }
        }

        Operation.Result.Validate();
    }
}

public sealed record ManagedGlueParameter(string TypeName, string Name);

public sealed record ManagedGlueOperation(
    ManagedGlueExceptionConvention ExceptionConvention,
    string? ExceptionOutParameterName,
    ManagedGlueResult Result)
{
    public static ManagedGlueOperation ReturnExceptionHandle(ManagedGlueResult result)
        => new(ManagedGlueExceptionConvention.ReturnExceptionHandle, ExceptionOutParameterName: null, result);

    public static ManagedGlueOperation WriteExceptionOut(string exceptionOutParameterName, ManagedGlueResult result)
        => new(ManagedGlueExceptionConvention.WriteExceptionOut, exceptionOutParameterName, result);
}

public enum ManagedGlueExceptionConvention
{
    ReturnExceptionHandle,
    WriteExceptionOut
}

public abstract record ManagedGlueResult
{
    public abstract void Validate();

    public static ManagedGlueResult VoidCall(ManagedGlueExpression callExpression)
        => new ManagedGlueVoidCallResult(callExpression);

    public static ManagedGlueResult ObjectHandle(ManagedGlueExpression valueExpression)
        => new ManagedGlueObjectHandleResult(valueExpression);

    public static ManagedGlueResult Int(ManagedGlueExpression valueExpression)
        => new ManagedGlueIntResult(valueExpression);

    public static ManagedGlueResult Float(ManagedGlueExpression valueExpression)
        => new ManagedGlueFloatResult(valueExpression);

    public static ManagedGlueResult Double(ManagedGlueExpression valueExpression)
        => new ManagedGlueDoubleResult(valueExpression);

    public static ManagedGlueResult BooleanAsInt(ManagedGlueExpression valueExpression)
        => new ManagedGlueBooleanAsIntResult(valueExpression);

    public static ManagedGlueResult ReleaseObject(string handleExpression)
        => new ManagedGlueReleaseObjectResult(handleExpression);

    public static ManagedGlueResult ReleaseException(string handleExpression)
        => new ManagedGlueReleaseExceptionResult(handleExpression);
}

public sealed record ManagedGlueVoidCallResult(ManagedGlueExpression CallExpression) : ManagedGlueResult
{
    public override void Validate()
        => CallExpression.Validate();
}

public sealed record ManagedGlueObjectHandleResult(ManagedGlueExpression ValueExpression) : ManagedGlueResult
{
    public override void Validate()
        => ValueExpression.Validate();
}

public sealed record ManagedGlueIntResult(ManagedGlueExpression ValueExpression) : ManagedGlueResult
{
    public override void Validate()
        => ValueExpression.Validate();
}

public sealed record ManagedGlueFloatResult(ManagedGlueExpression ValueExpression) : ManagedGlueResult
{
    public override void Validate()
        => ValueExpression.Validate();
}

public sealed record ManagedGlueDoubleResult(ManagedGlueExpression ValueExpression) : ManagedGlueResult
{
    public override void Validate()
        => ValueExpression.Validate();
}

public sealed record ManagedGlueBooleanAsIntResult(ManagedGlueExpression ValueExpression) : ManagedGlueResult
{
    public override void Validate()
        => ValueExpression.Validate();
}

public sealed record ManagedGlueReleaseObjectResult(string HandleExpression) : ManagedGlueResult
{
    public override void Validate()
    {
    }
}

public sealed record ManagedGlueReleaseExceptionResult(string HandleExpression) : ManagedGlueResult
{
    public override void Validate()
    {
    }
}
