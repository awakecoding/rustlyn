using Rustlyn.Interop;

namespace Rustlyn.Bindings;

public sealed record BindingSurface(
    IReadOnlyList<ManagedApiRequirement> Requirements,
    IReadOnlyList<ManagedGlueBinding> ManagedGlueBindings,
    IReadOnlyList<RustWrapperMethod> RustWrapperMethods,
    IReadOnlyList<RustEnumProjection> RustEnumProjections)
{
    public IReadOnlyList<RustExternBinding> Externs
        => ManagedGlueBindings.Select(RustExternBinding.FromManagedGlueBinding).ToArray();

    public static BindingSurface CreateTinyBclSurface()
    {
        var scalarMathBindings = BindingSurfaceScanner.CreateStaticScalarMethodBindings(
            new StaticScalarMethodBindingRequest(typeof(Math), nameof(Math.Abs), [typeof(double)], RustWrapperContainer.Math),
            new StaticScalarMethodBindingRequest(typeof(Math), nameof(Math.Sqrt), [typeof(double)], RustWrapperContainer.Math),
            new StaticScalarMethodBindingRequest(typeof(MathF), nameof(MathF.Abs), [typeof(float)], RustWrapperContainer.MathF),
            new StaticScalarMethodBindingRequest(typeof(MathF), nameof(MathF.Cos), [typeof(float)], RustWrapperContainer.MathF),
            new StaticScalarMethodBindingRequest(typeof(MathF), nameof(MathF.Max), [typeof(float), typeof(float)], RustWrapperContainer.MathF),
            new StaticScalarMethodBindingRequest(typeof(MathF), nameof(MathF.Min), [typeof(float), typeof(float)], RustWrapperContainer.MathF),
            new StaticScalarMethodBindingRequest(typeof(MathF), nameof(MathF.Sin), [typeof(float)], RustWrapperContainer.MathF),
            new StaticScalarMethodBindingRequest(typeof(MathF), nameof(MathF.Sqrt), [typeof(float)], RustWrapperContainer.MathF));
        var directoryGetFilesBinding = BindingSurfaceScanner.CreateStaticObjectHandleMethodBinding(
            new StaticObjectHandleMethodBindingRequest(typeof(Directory), nameof(Directory.GetFiles), [typeof(string), typeof(string)], RustWrapperContainer.IoDirectory));
        var fileReadAllLinesBinding = BindingSurfaceScanner.CreateStaticObjectHandleMethodBinding(
            new StaticObjectHandleMethodBindingRequest(typeof(File), nameof(File.ReadAllLines), [typeof(string)], RustWrapperContainer.IoFile));
        var arrayHandleBindings = CreateArrayHandleBindings();
        var stringUtf8AdapterBindings = CreateManagedStringUtf8AdapterBindings();

        return new BindingSurface(
            [
                ManagedApiRequirement.Method("System.Console.WriteLine(string)", typeof(Console), nameof(Console.WriteLine), [typeof(string)]),
                ManagedApiRequirement.Method("System.Environment.GetCommandLineArgs()", typeof(Environment), nameof(Environment.GetCommandLineArgs), []),
                ManagedApiRequirement.Property("System.Environment.CurrentDirectory", typeof(Environment), nameof(Environment.CurrentDirectory)),
                ManagedApiRequirement.Method("System.IO.Directory.GetCurrentDirectory()", typeof(Directory), nameof(Directory.GetCurrentDirectory), []),
                directoryGetFilesBinding.Requirement,
                fileReadAllLinesBinding.Requirement,
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
                .. scalarMathBindings.Select(static binding => binding.Requirement),
                ManagedApiRequirement.Method("System.String.Contains(string, StringComparison)", typeof(string), nameof(string.Contains), [typeof(string), typeof(StringComparison)]),
                ManagedApiRequirement.Method("System.String.IndexOf(string, StringComparison)", typeof(string), nameof(string.IndexOf), [typeof(string), typeof(StringComparison)]),
                ManagedApiRequirement.Method("System.String.StartsWith(string, StringComparison)", typeof(string), nameof(string.StartsWith), [typeof(string), typeof(StringComparison)]),
                ManagedApiRequirement.Method("System.String.EndsWith(string, StringComparison)", typeof(string), nameof(string.EndsWith), [typeof(string), typeof(StringComparison)]),
                ManagedApiRequirement.ForType("System.StringComparison", typeof(StringComparison)),
                ManagedApiRequirement.Method("System.String.Split(string, StringSplitOptions)", typeof(string), nameof(string.Split), [typeof(string), typeof(StringSplitOptions)]),
                ManagedApiRequirement.ForType("System.StringSplitOptions", typeof(StringSplitOptions)),
                ManagedApiRequirement.Method("System.String.Substring(int, int)", typeof(string), nameof(string.Substring), [typeof(int), typeof(int)]),
                ManagedApiRequirement.Method("System.String.Replace(string, string)", typeof(string), nameof(string.Replace), [typeof(string), typeof(string)]),
                ManagedApiRequirement.Property("System.String.Length", typeof(string), nameof(string.Length)),
                ManagedApiRequirement.ForType("System.String", typeof(string)),
                .. arrayHandleBindings.Requirements,
                ManagedApiRequirement.Method("System.TimeSpan.FromMilliseconds(double)", typeof(TimeSpan), nameof(TimeSpan.FromMilliseconds), [typeof(double)]),
                ManagedApiRequirement.Property("System.TimeSpan.Ticks", typeof(TimeSpan), nameof(TimeSpan.Ticks)),
                ManagedApiRequirement.Property("System.TimeSpan.TotalMilliseconds", typeof(TimeSpan), nameof(TimeSpan.TotalMilliseconds)),
                ManagedApiRequirement.Method("System.TimeSpan.ToString()", typeof(TimeSpan), nameof(TimeSpan.ToString), []),
                ManagedApiRequirement.ForType("System.TimeSpan", typeof(TimeSpan)),
                ManagedApiRequirement.Method("System.DateTimeOffset.FromUnixTimeMilliseconds(long)", typeof(DateTimeOffset), nameof(DateTimeOffset.FromUnixTimeMilliseconds), [typeof(long)]),
                ManagedApiRequirement.Method("System.DateTimeOffset.ToUnixTimeMilliseconds()", typeof(DateTimeOffset), nameof(DateTimeOffset.ToUnixTimeMilliseconds), []),
                ManagedApiRequirement.Method("System.DateTimeOffset.ToString()", typeof(DateTimeOffset), nameof(DateTimeOffset.ToString), []),
                ManagedApiRequirement.ForType("System.DateTimeOffset", typeof(DateTimeOffset)),
                ManagedApiRequirement.Method("System.Guid.Parse(string)", typeof(Guid), nameof(Guid.Parse), [typeof(string)]),
                ManagedApiRequirement.Method("System.Guid.ToString()", typeof(Guid), nameof(Guid.ToString), []),
                ManagedApiRequirement.ForType("System.Guid", typeof(Guid)),
                ManagedApiRequirement.Method("Rustlyn.Interop.ManagedCallbackBridge.InvokeI32(IntPtr, int)", typeof(ManagedCallbackBridge), nameof(ManagedCallbackBridge.InvokeI32), [typeof(IntPtr), typeof(int)]),
                ManagedApiRequirement.Method("Rustlyn.Interop.ManagedCallbackBridge.InvokeI32I32(IntPtr, int, int)", typeof(ManagedCallbackBridge), nameof(ManagedCallbackBridge.InvokeI32I32), [typeof(IntPtr), typeof(int), typeof(int)]),
                ManagedApiRequirement.Method("Rustlyn.Interop.ManagedCallbackBridge.InvokeObjectHandleTransform(IntPtr, int)", typeof(ManagedCallbackBridge), nameof(ManagedCallbackBridge.InvokeObjectHandleTransform), [typeof(IntPtr), typeof(int)])
            ],
            [
                Glue(
                    "rustlyn_bindgen_system_console_write_line_utf8",
                    "BindgenSystemConsoleWriteLineUtf8",
                    [Pointer("valuePointer"), I64("valueLength")],
                    ManagedGlueOperation.ReturnExceptionHandle(ManagedGlueResult.VoidCall(
                        StaticMethod(typeof(Console), nameof(Console.WriteLine), [typeof(string)], [Utf8String("valuePointer", "valueLength")])))),
                Glue(
                    "rustlyn_bindgen_system_console_write_line_string",
                    "BindgenSystemConsoleWriteLineString",
                    [I32("stringHandle")],
                    ManagedGlueOperation.ReturnExceptionHandle(ManagedGlueResult.VoidCall(
                        StaticMethod(typeof(Console), nameof(Console.WriteLine), [typeof(string)], [ManagedObject(typeof(string), "stringHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_environment_get_command_line_args",
                    "BindgenSystemEnvironmentGetCommandLineArgs",
                    [Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Environment), nameof(Environment.GetCommandLineArgs), [], [])))),
                Glue(
                    "rustlyn_bindgen_system_environment_current_directory",
                    "BindgenSystemEnvironmentCurrentDirectory",
                    [Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticProperty(typeof(Environment), nameof(Environment.CurrentDirectory))))),
                Glue(
                    "rustlyn_bindgen_system_io_directory_get_current_directory",
                    "BindgenSystemIoDirectoryGetCurrentDirectory",
                    [Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Directory), nameof(Directory.GetCurrentDirectory), [], [])))),
                directoryGetFilesBinding.ManagedGlueBinding,
                Glue(
                    "rustlyn_bindgen_system_io_file_read_all_lines_utf8",
                    "BindgenSystemIoFileReadAllLinesUtf8",
                    [Pointer("pathPointer"), I64("pathLength"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(File), nameof(File.ReadAllLines), [typeof(string)], [Utf8String("pathPointer", "pathLength")])))),
                fileReadAllLinesBinding.ManagedGlueBinding,
                Glue(
                    "rustlyn_bindgen_system_io_path_change_extension_string_string",
                    "BindgenSystemIoPathChangeExtensionStringString",
                    [I32("pathHandle"), I32("extensionHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.ChangeExtension), [typeof(string), typeof(string)], [ManagedObject(typeof(string), "pathHandle"), ManagedObject(typeof(string), "extensionHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_combine_string_string",
                    "BindgenSystemIoPathCombineStringString",
                    [I32("path1Handle"), I32("path2Handle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.Combine), [typeof(string), typeof(string)], [ManagedObject(typeof(string), "path1Handle"), ManagedObject(typeof(string), "path2Handle")])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_ends_in_directory_separator_string",
                    "BindgenSystemIoPathEndsInDirectorySeparatorString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        StaticMethod(typeof(Path), nameof(Path.EndsInDirectorySeparator), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_get_directory_name_string",
                    "BindgenSystemIoPathGetDirectoryNameString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetDirectoryName), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_get_extension_string",
                    "BindgenSystemIoPathGetExtensionString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetExtension), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_get_file_name_string",
                    "BindgenSystemIoPathGetFileNameString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetFileName), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_get_file_name_without_extension_string",
                    "BindgenSystemIoPathGetFileNameWithoutExtensionString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetFileNameWithoutExtension), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_get_full_path_string",
                    "BindgenSystemIoPathGetFullPathString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetFullPath), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_get_path_root_string",
                    "BindgenSystemIoPathGetPathRootString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetPathRoot), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_get_relative_path_string_string",
                    "BindgenSystemIoPathGetRelativePathStringString",
                    [I32("relativeToHandle"), I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetRelativePath), [typeof(string), typeof(string)], [ManagedObject(typeof(string), "relativeToHandle"), ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_get_temp_path",
                    "BindgenSystemIoPathGetTempPath",
                    [Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Path), nameof(Path.GetTempPath), [], [])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_has_extension_string",
                    "BindgenSystemIoPathHasExtensionString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        StaticMethod(typeof(Path), nameof(Path.HasExtension), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_is_path_fully_qualified_string",
                    "BindgenSystemIoPathIsPathFullyQualifiedString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        StaticMethod(typeof(Path), nameof(Path.IsPathFullyQualified), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_io_path_is_path_rooted_string",
                    "BindgenSystemIoPathIsPathRootedString",
                    [I32("pathHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        StaticMethod(typeof(Path), nameof(Path.IsPathRooted), [typeof(string)], [ManagedObject(typeof(string), "pathHandle")])))),
                .. scalarMathBindings.Select(static binding => binding.ManagedGlueBinding),
                .. stringUtf8AdapterBindings.InputManagedGlueBindings,
                Glue(
                    "rustlyn_bindgen_system_string_len",
                    "BindgenSystemStringLength",
                    [I32("stringHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        InstanceProperty(ManagedObject(typeof(string), "stringHandle"), typeof(string), nameof(string.Length))))),
                .. stringUtf8AdapterBindings.OutputManagedGlueBindings,
                Glue(
                    "rustlyn_bindgen_system_string_contains_utf8",
                    "BindgenSystemStringContainsUtf8",
                    [I32("stringHandle"), Pointer("needlePointer"), I64("needleLength"), I32("comparison"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        InstanceMethod(
                            ManagedObject(typeof(string), "stringHandle"),
                            typeof(string),
                            nameof(string.Contains),
                            [typeof(string), typeof(StringComparison)],
                            [Utf8String("needlePointer", "needleLength"), ManagedGlueExpression.EnumParameter(typeof(StringComparison), "comparison")])))),
                Glue(
                    "rustlyn_bindgen_system_string_contains_string",
                    "BindgenSystemStringContainsString",
                    [I32("stringHandle"), I32("needleHandle"), I32("comparison"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        InstanceMethod(
                            ManagedObject(typeof(string), "stringHandle"),
                            typeof(string),
                            nameof(string.Contains),
                            [typeof(string), typeof(StringComparison)],
                            [ManagedObject(typeof(string), "needleHandle"), ManagedGlueExpression.EnumParameter(typeof(StringComparison), "comparison")])))),
                Glue(
                    "rustlyn_bindgen_system_string_index_of_string",
                    "BindgenSystemStringIndexOfString",
                    [I32("stringHandle"), I32("valueHandle"), I32("comparison"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        InstanceMethod(
                            ManagedObject(typeof(string), "stringHandle"),
                            typeof(string),
                            nameof(string.IndexOf),
                            [typeof(string), typeof(StringComparison)],
                            [ManagedObject(typeof(string), "valueHandle"), ManagedGlueExpression.EnumParameter(typeof(StringComparison), "comparison")])))),
                Glue(
                    "rustlyn_bindgen_system_string_starts_with_string",
                    "BindgenSystemStringStartsWithString",
                    [I32("stringHandle"), I32("valueHandle"), I32("comparison"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        InstanceMethod(
                            ManagedObject(typeof(string), "stringHandle"),
                            typeof(string),
                            nameof(string.StartsWith),
                            [typeof(string), typeof(StringComparison)],
                            [ManagedObject(typeof(string), "valueHandle"), ManagedGlueExpression.EnumParameter(typeof(StringComparison), "comparison")])))),
                Glue(
                    "rustlyn_bindgen_system_string_ends_with_string",
                    "BindgenSystemStringEndsWithString",
                    [I32("stringHandle"), I32("valueHandle"), I32("comparison"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.BooleanAsInt(
                        InstanceMethod(
                            ManagedObject(typeof(string), "stringHandle"),
                            typeof(string),
                            nameof(string.EndsWith),
                            [typeof(string), typeof(StringComparison)],
                            [ManagedObject(typeof(string), "valueHandle"), ManagedGlueExpression.EnumParameter(typeof(StringComparison), "comparison")])))),
                Glue(
                    "rustlyn_bindgen_system_string_split_string_options",
                    "BindgenSystemStringSplitStringOptions",
                    [I32("stringHandle"), I32("separatorHandle"), I32("options"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        InstanceMethod(
                            ManagedObject(typeof(string), "stringHandle"),
                            typeof(string),
                            nameof(string.Split),
                            [typeof(string), typeof(StringSplitOptions)],
                            [ManagedObject(typeof(string), "separatorHandle"), ManagedGlueExpression.EnumParameter(typeof(StringSplitOptions), "options")])))),
                Glue(
                    "rustlyn_bindgen_system_string_substring_i32_i32",
                    "BindgenSystemStringSubstringI32I32",
                    [I32("stringHandle"), I32("startIndex"), I32("length"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        InstanceMethod(
                            ManagedObject(typeof(string), "stringHandle"),
                            typeof(string),
                            nameof(string.Substring),
                            [typeof(int), typeof(int)],
                            [ManagedGlueExpression.Parameter("startIndex"), ManagedGlueExpression.Parameter("length")])))),
                Glue(
                    "rustlyn_bindgen_system_string_replace_string_string",
                    "BindgenSystemStringReplaceStringString",
                    [I32("stringHandle"), I32("oldValueHandle"), I32("newValueHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        InstanceMethod(
                            ManagedObject(typeof(string), "stringHandle"),
                            typeof(string),
                            nameof(string.Replace),
                            [typeof(string), typeof(string)],
                            [ManagedObject(typeof(string), "oldValueHandle"), ManagedObject(typeof(string), "newValueHandle")])))),
                .. arrayHandleBindings.ManagedGlueBindings,
                Glue(
                    "rustlyn_bindgen_system_time_span_from_milliseconds_f64",
                    "BindgenSystemTimeSpanFromMillisecondsF64",
                    [F64("value"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(TimeSpan), nameof(TimeSpan.FromMilliseconds), [typeof(double)], [ManagedGlueExpression.Parameter("value")])))),
                Glue(
                    "rustlyn_bindgen_system_time_span_ticks",
                    "BindgenSystemTimeSpanTicks",
                    [I32("timeSpanHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Long(
                        InstanceProperty(ManagedObject(typeof(TimeSpan), "timeSpanHandle"), typeof(TimeSpan), nameof(TimeSpan.Ticks))))),
                Glue(
                    "rustlyn_bindgen_system_time_span_total_milliseconds",
                    "BindgenSystemTimeSpanTotalMilliseconds",
                    [I32("timeSpanHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Double(
                        InstanceProperty(ManagedObject(typeof(TimeSpan), "timeSpanHandle"), typeof(TimeSpan), nameof(TimeSpan.TotalMilliseconds))))),
                Glue(
                    "rustlyn_bindgen_system_time_span_to_string",
                    "BindgenSystemTimeSpanToString",
                    [I32("timeSpanHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        InstanceMethod(ManagedObject(typeof(TimeSpan), "timeSpanHandle"), typeof(TimeSpan), nameof(TimeSpan.ToString), [], [])))),
                Glue(
                    "rustlyn_bindgen_system_date_time_offset_from_unix_time_milliseconds_i64",
                    "BindgenSystemDateTimeOffsetFromUnixTimeMillisecondsI64",
                    [I64("milliseconds"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(DateTimeOffset), nameof(DateTimeOffset.FromUnixTimeMilliseconds), [typeof(long)], [ManagedGlueExpression.Parameter("milliseconds")])))),
                Glue(
                    "rustlyn_bindgen_system_date_time_offset_to_unix_time_milliseconds",
                    "BindgenSystemDateTimeOffsetToUnixTimeMilliseconds",
                    [I32("dateTimeOffsetHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Long(
                        InstanceMethod(ManagedObject(typeof(DateTimeOffset), "dateTimeOffsetHandle"), typeof(DateTimeOffset), nameof(DateTimeOffset.ToUnixTimeMilliseconds), [], [])))),
                Glue(
                    "rustlyn_bindgen_system_date_time_offset_to_string",
                    "BindgenSystemDateTimeOffsetToString",
                    [I32("dateTimeOffsetHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        InstanceMethod(ManagedObject(typeof(DateTimeOffset), "dateTimeOffsetHandle"), typeof(DateTimeOffset), nameof(DateTimeOffset.ToString), [], [])))),
                Glue(
                    "rustlyn_bindgen_system_guid_parse_string",
                    "BindgenSystemGuidParseString",
                    [I32("valueHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(typeof(Guid), nameof(Guid.Parse), [typeof(string)], [ManagedObject(typeof(string), "valueHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_guid_to_string",
                    "BindgenSystemGuidToString",
                    [I32("guidHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        InstanceMethod(ManagedObject(typeof(Guid), "guidHandle"), typeof(Guid), nameof(Guid.ToString), [], [])))),
                Glue(
                    "rustlyn_bindgen_system_callback_apply_i32",
                    "BindgenSystemCallbackApplyI32",
                    [Callback("i32CallbackPointer", "fn(i32) -> i32"), I32("value"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        StaticMethod(
                            typeof(ManagedCallbackBridge),
                            nameof(ManagedCallbackBridge.InvokeI32),
                            [typeof(IntPtr), typeof(int)],
                            [ManagedGlueExpression.Parameter("i32CallbackPointer"), ManagedGlueExpression.Parameter("value")])))),
                Glue(
                    "rustlyn_bindgen_system_callback_apply_i32_i32",
                    "BindgenSystemCallbackApplyI32I32",
                    [Callback("i32I32CallbackPointer", "fn(i32, i32) -> i32"), I32("left"), I32("right"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        StaticMethod(
                            typeof(ManagedCallbackBridge),
                            nameof(ManagedCallbackBridge.InvokeI32I32),
                            [typeof(IntPtr), typeof(int), typeof(int)],
                            [ManagedGlueExpression.Parameter("i32I32CallbackPointer"), ManagedGlueExpression.Parameter("left"), ManagedGlueExpression.Parameter("right")])))),
                Glue(
                    "rustlyn_bindgen_system_callback_transform_managed_string",
                    "BindgenSystemCallbackTransformManagedString",
                    [Callback("stringTransformCallbackPointer", "fn(i32) -> i32"), I32("stringHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        StaticMethod(
                            typeof(ManagedCallbackBridge),
                            nameof(ManagedCallbackBridge.InvokeObjectHandleTransform),
                            [typeof(IntPtr), typeof(int)],
                            [ManagedGlueExpression.Parameter("stringTransformCallbackPointer"), ManagedGlueExpression.Parameter("stringHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_object_release",
                    "BindgenSystemObjectRelease",
                    [I32("objectHandle")],
                    ManagedGlueOperation.ReturnExceptionHandle(ManagedGlueResult.ReleaseObject("objectHandle"))),
                Glue(
                    "rustlyn_bindgen_system_exception_release",
                    "BindgenSystemExceptionRelease",
                    [I32("exceptionHandle")],
                    ManagedGlueOperation.ReturnExceptionHandle(ManagedGlueResult.ReleaseException("exceptionHandle"))),
                Glue(
                    "rustlyn_bindgen_system_exception_get_type_name_utf8_len",
                    "BindgenSystemExceptionGetTypeNameUtf8Length",
                    [I32("exceptionHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        StaticMethod(typeof(ManagedInteropRuntime), nameof(ManagedInteropRuntime.GetExceptionTypeNameUtf8Length), [typeof(int)], [ManagedGlueExpression.Parameter("exceptionHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_exception_get_type_name_utf8",
                    "BindgenSystemExceptionGetTypeNameUtf8",
                    [I32("exceptionHandle"), Pointer("destinationPointer"), I64("destinationCapacity"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        StaticMethod(typeof(ManagedInteropRuntime), nameof(ManagedInteropRuntime.CopyExceptionTypeNameUtf8), [typeof(int), typeof(IntPtr), typeof(long)], [ManagedGlueExpression.Parameter("exceptionHandle"), ManagedGlueExpression.Parameter("destinationPointer"), ManagedGlueExpression.Parameter("destinationCapacity")])))),
                Glue(
                    "rustlyn_bindgen_system_exception_get_message_utf8_len",
                    "BindgenSystemExceptionGetMessageUtf8Length",
                    [I32("exceptionHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        StaticMethod(typeof(ManagedInteropRuntime), nameof(ManagedInteropRuntime.GetExceptionMessageUtf8Length), [typeof(int)], [ManagedGlueExpression.Parameter("exceptionHandle")])))),
                Glue(
                    "rustlyn_bindgen_system_exception_get_message_utf8",
                    "BindgenSystemExceptionGetMessageUtf8",
                    [I32("exceptionHandle"), Pointer("destinationPointer"), I64("destinationCapacity"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        StaticMethod(typeof(ManagedInteropRuntime), nameof(ManagedInteropRuntime.CopyExceptionMessageUtf8), [typeof(int), typeof(IntPtr), typeof(long)], [ManagedGlueExpression.Parameter("exceptionHandle"), ManagedGlueExpression.Parameter("destinationPointer"), ManagedGlueExpression.Parameter("destinationCapacity")]))))
            ],
            [
                new RustWrapperMethod(
                    RustWrapperContainer.Callback,
                    "pub fn apply_i32(callback: fn(i32) -> i32, value: i32) -> Result<i32, Exception>",
                    "rustlyn_bindgen_system_callback_apply_i32",
                    ["callback", "value"],
                    "result",
                    RustWrapperResult.Scalar("i32")),
                new RustWrapperMethod(
                    RustWrapperContainer.Callback,
                    "pub fn apply_i32_i32(callback: fn(i32, i32) -> i32, left: i32, right: i32) -> Result<i32, Exception>",
                    "rustlyn_bindgen_system_callback_apply_i32_i32",
                    ["callback", "left", "right"],
                    "result",
                    RustWrapperResult.Scalar("i32")),
                new RustWrapperMethod(
                    RustWrapperContainer.Callback,
                    "pub fn transform_managed_string(callback: fn(i32) -> i32, value: &ManagedString) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_callback_transform_managed_string",
                    ["callback", "value.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.Console,
                    "pub fn write_line_utf8_parts(value_ptr: *const u8, value_len: i64) -> Result<(), Exception>",
                    "rustlyn_bindgen_system_console_write_line_utf8",
                    ["value_ptr", "value_len"],
                    "unused",
                    RustWrapperResult.Void()),
                new RustWrapperMethod(
                    RustWrapperContainer.Console,
                    "pub fn write_line(value: &ManagedString) -> Result<(), Exception>",
                    "rustlyn_bindgen_system_console_write_line_string",
                    ["value.handle()"],
                    "unused",
                    RustWrapperResult.Void()),
                new RustWrapperMethod(
                    RustWrapperContainer.Environment,
                    "pub fn current_directory() -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_environment_current_directory",
                    [],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.Environment,
                    "pub fn get_command_line_args() -> Result<ManagedStringArray, Exception>",
                    "rustlyn_bindgen_system_environment_get_command_line_args",
                    [],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedStringArray")),
                new RustWrapperMethod(
                    RustWrapperContainer.Exception,
                    "pub fn release(self) -> Result<(), Self>",
                    "rustlyn_bindgen_system_exception_release",
                    ["self.handle"],
                    "unused",
                    RustWrapperResult.Void()),
                new RustWrapperMethod(
                    RustWrapperContainer.Exception,
                    "pub fn type_name_utf8_len(&self) -> Result<i32, Self>",
                    "rustlyn_bindgen_system_exception_get_type_name_utf8_len",
                    ["self.handle"],
                    "length",
                    RustWrapperResult.Scalar("i32")),
                new RustWrapperMethod(
                    RustWrapperContainer.Exception,
                    "pub fn copy_type_name_utf8_into(&self, buffer: &mut [u8]) -> Result<i32, Self>",
                    "rustlyn_bindgen_system_exception_get_type_name_utf8",
                    ["self.handle", "buffer.as_mut_ptr()", "buffer.len() as i64"],
                    "written",
                    RustWrapperResult.Scalar("i32")),
                new RustWrapperMethod(
                    RustWrapperContainer.Exception,
                    "pub fn message_utf8_len(&self) -> Result<i32, Self>",
                    "rustlyn_bindgen_system_exception_get_message_utf8_len",
                    ["self.handle"],
                    "length",
                    RustWrapperResult.Scalar("i32")),
                new RustWrapperMethod(
                    RustWrapperContainer.Exception,
                    "pub fn copy_message_utf8_into(&self, buffer: &mut [u8]) -> Result<i32, Self>",
                    "rustlyn_bindgen_system_exception_get_message_utf8",
                    ["self.handle", "buffer.as_mut_ptr()", "buffer.len() as i64"],
                    "written",
                    RustWrapperResult.Scalar("i32")),
                .. scalarMathBindings.Select(static binding => binding.RustWrapperMethod),
                new RustWrapperMethod(
                    RustWrapperContainer.IoDirectory,
                    "pub fn get_current_directory() -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_io_directory_get_current_directory",
                    [],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                directoryGetFilesBinding.RustWrapperMethod,
                new RustWrapperMethod(
                    RustWrapperContainer.IoFile,
                    "pub fn read_all_lines_utf8_parts(path_ptr: *const u8, path_len: i64) -> Result<ManagedStringArray, Exception>",
                    "rustlyn_bindgen_system_io_file_read_all_lines_utf8",
                    ["path_ptr", "path_len"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedStringArray")),
                fileReadAllLinesBinding.RustWrapperMethod,
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn change_extension(path: &ManagedString, extension: &ManagedString) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_io_path_change_extension_string_string",
                    ["path.handle()", "extension.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn combine(path1: &ManagedString, path2: &ManagedString) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_io_path_combine_string_string",
                    ["path1.handle()", "path2.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn ends_in_directory_separator(path: &ManagedString) -> Result<i32, Exception>",
                    "rustlyn_bindgen_system_io_path_ends_in_directory_separator_string",
                    ["path.handle()"],
                    "value",
                    RustWrapperResult.BooleanAsInt()),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_directory_name(path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_io_path_get_directory_name_string",
                    ["path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_extension(path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_io_path_get_extension_string",
                    ["path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_file_name(path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_io_path_get_file_name_string",
                    ["path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_file_name_without_extension(path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_io_path_get_file_name_without_extension_string",
                    ["path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_full_path(path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_io_path_get_full_path_string",
                    ["path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_path_root(path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_io_path_get_path_root_string",
                    ["path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_relative_path(relative_to: &ManagedString, path: &ManagedString) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_io_path_get_relative_path_string_string",
                    ["relative_to.handle()", "path.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn get_temp_path() -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_io_path_get_temp_path",
                    [],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn has_extension(path: &ManagedString) -> Result<i32, Exception>",
                    "rustlyn_bindgen_system_io_path_has_extension_string",
                    ["path.handle()"],
                    "value",
                    RustWrapperResult.BooleanAsInt()),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn is_path_fully_qualified(path: &ManagedString) -> Result<i32, Exception>",
                    "rustlyn_bindgen_system_io_path_is_path_fully_qualified_string",
                    ["path.handle()"],
                    "value",
                    RustWrapperResult.BooleanAsInt()),
                new RustWrapperMethod(
                    RustWrapperContainer.IoPath,
                    "pub fn is_path_rooted(path: &ManagedString) -> Result<i32, Exception>",
                    "rustlyn_bindgen_system_io_path_is_path_rooted_string",
                    ["path.handle()"],
                    "value",
                    RustWrapperResult.BooleanAsInt()),
                .. stringUtf8AdapterBindings.InputRustWrapperMethods,
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn len(&self) -> Result<i32, Exception>",
                    "rustlyn_bindgen_system_string_len",
                    ["self.handle"],
                    "length",
                    RustWrapperResult.Scalar("i32")),
                .. stringUtf8AdapterBindings.OutputRustWrapperMethods,
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn contains_utf8_parts(&self, needle_ptr: *const u8, needle_len: i64) -> Result<bool, Exception>",
                    "rustlyn_bindgen_system_string_contains_utf8",
                    ["self.handle", "needle_ptr", "needle_len", "StringComparison::Ordinal.as_i32()"],
                    "contains",
                    RustWrapperResult.Boolean()),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn contains_utf8_parts_with_comparison(&self, needle_ptr: *const u8, needle_len: i64, comparison: StringComparison) -> Result<bool, Exception>",
                    "rustlyn_bindgen_system_string_contains_utf8",
                    ["self.handle", "needle_ptr", "needle_len", "comparison.as_i32()"],
                    "contains",
                    RustWrapperResult.Boolean()),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn contains(&self, needle: &ManagedString) -> Result<bool, Exception>",
                    "rustlyn_bindgen_system_string_contains_string",
                    ["self.handle", "needle.handle()", "StringComparison::Ordinal.as_i32()"],
                    "contains",
                    RustWrapperResult.Boolean()),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn contains_with_comparison(&self, needle: &ManagedString, comparison: StringComparison) -> Result<bool, Exception>",
                    "rustlyn_bindgen_system_string_contains_string",
                    ["self.handle", "needle.handle()", "comparison.as_i32()"],
                    "contains",
                    RustWrapperResult.Boolean()),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn index_of(&self, value: &ManagedString) -> Result<i32, Exception>",
                    "rustlyn_bindgen_system_string_index_of_string",
                    ["self.handle", "value.handle()", "StringComparison::Ordinal.as_i32()"],
                    "index",
                    RustWrapperResult.Scalar("i32")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn index_of_with_comparison(&self, value: &ManagedString, comparison: StringComparison) -> Result<i32, Exception>",
                    "rustlyn_bindgen_system_string_index_of_string",
                    ["self.handle", "value.handle()", "comparison.as_i32()"],
                    "index",
                    RustWrapperResult.Scalar("i32")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn starts_with(&self, value: &ManagedString) -> Result<bool, Exception>",
                    "rustlyn_bindgen_system_string_starts_with_string",
                    ["self.handle", "value.handle()", "StringComparison::Ordinal.as_i32()"],
                    "starts",
                    RustWrapperResult.Boolean()),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn starts_with_comparison(&self, value: &ManagedString, comparison: StringComparison) -> Result<bool, Exception>",
                    "rustlyn_bindgen_system_string_starts_with_string",
                    ["self.handle", "value.handle()", "comparison.as_i32()"],
                    "starts",
                    RustWrapperResult.Boolean()),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn ends_with(&self, value: &ManagedString) -> Result<bool, Exception>",
                    "rustlyn_bindgen_system_string_ends_with_string",
                    ["self.handle", "value.handle()", "StringComparison::Ordinal.as_i32()"],
                    "ends",
                    RustWrapperResult.Boolean()),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn ends_with_comparison(&self, value: &ManagedString, comparison: StringComparison) -> Result<bool, Exception>",
                    "rustlyn_bindgen_system_string_ends_with_string",
                    ["self.handle", "value.handle()", "comparison.as_i32()"],
                    "ends",
                    RustWrapperResult.Boolean()),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn split(&self, separator: &ManagedString, options: StringSplitOptions) -> Result<ManagedStringArray, Exception>",
                    "rustlyn_bindgen_system_string_split_string_options",
                    ["self.handle", "separator.handle()", "options.as_i32()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedStringArray")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn substring(&self, start_index: i32, length: i32) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_string_substring_i32_i32",
                    ["self.handle", "start_index", "length"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn replace(&self, old_value: &ManagedString, new_value: &ManagedString) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_string_replace_string_string",
                    ["self.handle", "old_value.handle()", "new_value.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn release(self) -> Result<(), Exception>",
                    "rustlyn_bindgen_system_object_release",
                    ["self.handle"],
                    "unused",
                    RustWrapperResult.Void()),
                .. arrayHandleBindings.RustWrapperMethods,
                new RustWrapperMethod(
                    RustWrapperContainer.TimeSpan,
                    "pub fn from_milliseconds(value: f64) -> Result<ManagedTimeSpan, Exception>",
                    "rustlyn_bindgen_system_time_span_from_milliseconds_f64",
                    ["value"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedTimeSpan")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedTimeSpan,
                    "pub fn ticks(&self) -> Result<i64, Exception>",
                    "rustlyn_bindgen_system_time_span_ticks",
                    ["self.handle"],
                    "ticks",
                    RustWrapperResult.Scalar("i64")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedTimeSpan,
                    "pub fn total_milliseconds(&self) -> Result<f64, Exception>",
                    "rustlyn_bindgen_system_time_span_total_milliseconds",
                    ["self.handle"],
                    "milliseconds",
                    RustWrapperResult.Scalar("f64")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedTimeSpan,
                    "pub fn to_string(&self) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_time_span_to_string",
                    ["self.handle"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedTimeSpan,
                    "pub fn release(self) -> Result<(), Exception>",
                    "rustlyn_bindgen_system_object_release",
                    ["self.handle"],
                    "unused",
                    RustWrapperResult.Void()),
                new RustWrapperMethod(
                    RustWrapperContainer.DateTimeOffset,
                    "pub fn from_unix_time_milliseconds(milliseconds: i64) -> Result<ManagedDateTimeOffset, Exception>",
                    "rustlyn_bindgen_system_date_time_offset_from_unix_time_milliseconds_i64",
                    ["milliseconds"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedDateTimeOffset")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedDateTimeOffset,
                    "pub fn to_unix_time_milliseconds(&self) -> Result<i64, Exception>",
                    "rustlyn_bindgen_system_date_time_offset_to_unix_time_milliseconds",
                    ["self.handle"],
                    "milliseconds",
                    RustWrapperResult.Scalar("i64")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedDateTimeOffset,
                    "pub fn to_string(&self) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_date_time_offset_to_string",
                    ["self.handle"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedDateTimeOffset,
                    "pub fn release(self) -> Result<(), Exception>",
                    "rustlyn_bindgen_system_object_release",
                    ["self.handle"],
                    "unused",
                    RustWrapperResult.Void()),
                new RustWrapperMethod(
                    RustWrapperContainer.Guid,
                    "pub fn parse(value: &ManagedString) -> Result<ManagedGuid, Exception>",
                    "rustlyn_bindgen_system_guid_parse_string",
                    ["value.handle()"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedGuid")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedGuid,
                    "pub fn to_string(&self) -> Result<ManagedString, Exception>",
                    "rustlyn_bindgen_system_guid_to_string",
                    ["self.handle"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("ManagedString")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedGuid,
                    "pub fn release(self) -> Result<(), Exception>",
                    "rustlyn_bindgen_system_object_release",
                    ["self.handle"],
                    "unused",
                    RustWrapperResult.Void())
            ],
            [
                RustEnumProjection.FromManagedEnum(typeof(StringComparison)),
                RustEnumProjection.FromManagedEnum(typeof(StringSplitOptions))
            ]);
    }

    private static ArrayHandleBindingSet CreateArrayHandleBindings()
        => CreateArrayHandleBindings(
        [
            new ArrayHandleProjection(
                "System.String[]",
                typeof(string[]),
                typeof(string),
                RustWrapperContainer.ManagedStringArray,
                "rustlyn_bindgen_system_string_array",
                "BindgenSystemStringArray",
                "from_strings",
                "pub fn from_strings(first: &ManagedString, second: &ManagedString, third: &ManagedString) -> Result<Self, Exception>",
                ["first.handle()", "second.handle()", "third.handle()"],
                [I32("firstHandle"), I32("secondHandle"), I32("thirdHandle")],
                [ManagedObject(typeof(string), "firstHandle"), ManagedObject(typeof(string), "secondHandle"), ManagedObject(typeof(string), "thirdHandle")],
                nameof(ManagedInteropRuntime.CreateStringArray),
                [typeof(string), typeof(string), typeof(string)],
                "pub fn get(&self, index: i32) -> Result<ManagedString, Exception>",
                RustWrapperResult.ObjectHandle("ManagedString"),
                "object_handle",
                CreateGlueAfterAccessors: true),
            new ArrayHandleProjection(
                "System.Int32[]",
                typeof(int[]),
                typeof(int),
                RustWrapperContainer.ManagedIntArray,
                "rustlyn_bindgen_system_int_array",
                "BindgenSystemIntArray",
                "from_i32_triplet",
                "pub fn from_i32_triplet(first: i32, second: i32, third: i32) -> Result<Self, Exception>",
                ["first", "second", "third"],
                [I32("first"), I32("second"), I32("third")],
                [ManagedGlueExpression.Parameter("first"), ManagedGlueExpression.Parameter("second"), ManagedGlueExpression.Parameter("third")],
                nameof(ManagedInteropRuntime.CreateInt32Array),
                [typeof(int), typeof(int), typeof(int)],
                "pub fn get(&self, index: i32) -> Result<i32, Exception>",
                RustWrapperResult.Scalar("i32"),
                "value",
                "CopyInt32Array",
                nameof(ManagedInteropRuntime.CopyInt32Array),
                "*mut i32",
                "pub fn copy_into(&self, buffer: &mut [i32]) -> Result<i32, Exception>"),
            new ArrayHandleProjection(
                "System.Byte[]",
                typeof(byte[]),
                typeof(byte),
                RustWrapperContainer.ManagedByteArray,
                "rustlyn_bindgen_system_byte_array",
                "BindgenSystemByteArray",
                "from_u8_triplet",
                "pub fn from_u8_triplet(first: u8, second: u8, third: u8) -> Result<Self, Exception>",
                ["first as i32", "second as i32", "third as i32"],
                [I32("first"), I32("second"), I32("third")],
                [ManagedGlueExpression.Parameter("first"), ManagedGlueExpression.Parameter("second"), ManagedGlueExpression.Parameter("third")],
                nameof(ManagedInteropRuntime.CreateByteArray),
                [typeof(int), typeof(int), typeof(int)],
                "pub fn get(&self, index: i32) -> Result<i32, Exception>",
                RustWrapperResult.Scalar("i32"),
                "value",
                "CopyByteArray",
                nameof(ManagedInteropRuntime.CopyByteArray),
                "*mut u8",
                "pub fn copy_into(&self, buffer: &mut [u8]) -> Result<i32, Exception>")
        ]);

    private static ArrayHandleBindingSet CreateArrayHandleBindings(IReadOnlyList<ArrayHandleProjection> projections)
    {
        var requirements = new List<ManagedApiRequirement>();
        var managedGlueBindings = new List<ManagedGlueBinding>();
        var rustWrapperMethods = new List<RustWrapperMethod>();
        foreach (var projection in projections)
        {
            requirements.Add(ManagedApiRequirement.ForType(projection.DisplayName, projection.ArrayType));
            requirements.Add(ManagedApiRequirement.Method(
                $"Rustlyn.Interop.ManagedInteropRuntime.{projection.CreateMethodName}({FormatParameterTypes(projection.CreateParameterTypes)})",
                typeof(ManagedInteropRuntime),
                projection.CreateMethodName,
                projection.CreateParameterTypes));
            if (projection.CopyMethodName is not null)
            {
                requirements.Add(ManagedApiRequirement.Method(
                    $"Rustlyn.Interop.ManagedInteropRuntime.{projection.CopyMethodName}({FormatParameterTypes([projection.ArrayType, typeof(IntPtr), typeof(long)])})",
                    typeof(ManagedInteropRuntime),
                    projection.CopyMethodName,
                    [projection.ArrayType, typeof(IntPtr), typeof(long)]));
            }

            if (!projection.CreateGlueAfterAccessors)
            {
                AddArrayCreateGlueBinding(projection, managedGlueBindings);
            }

            if (!projection.CreateWrapperAfterAccessors)
            {
                AddArrayCreateWrapper(projection, rustWrapperMethods);
            }

            var lenSymbol = projection.BaseSymbol + "_len";
            managedGlueBindings.Add(Glue(
                lenSymbol,
                projection.HelperPrefix + "Length",
                [I32("arrayHandle"), Pointer("exceptionOutPointer")],
                ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                    ManagedGlueExpression.ArrayLength(ManagedObject(projection.ArrayType, "arrayHandle"))))));
            rustWrapperMethods.Add(new RustWrapperMethod(
                projection.Container,
                "pub fn len(&self) -> Result<i32, Exception>",
                lenSymbol,
                ["self.handle"],
                "length",
                RustWrapperResult.Scalar("i32")));

            var getSymbol = projection.BaseSymbol + "_get";
            var getElement = ManagedGlueExpression.ArrayElement(ManagedObject(projection.ArrayType, "arrayHandle"), "index");
            managedGlueBindings.Add(Glue(
                getSymbol,
                projection.HelperPrefix + "Get",
                [I32("arrayHandle"), I32("index"), Pointer("exceptionOutPointer")],
                ManagedGlueOperation.WriteExceptionOut(
                    "exceptionOutPointer",
                    projection.GetResult.Kind == RustWrapperResultKind.ObjectHandle
                        ? ManagedGlueResult.ObjectHandle(getElement)
                        : ManagedGlueResult.Int(getElement))));
            rustWrapperMethods.Add(new RustWrapperMethod(
                projection.Container,
                projection.GetWrapperSignature,
                getSymbol,
                ["self.handle", "index"],
                projection.GetResultVariableName,
                projection.GetResult));

            if (projection.CreateGlueAfterAccessors)
            {
                AddArrayCreateGlueBinding(projection, managedGlueBindings);
            }

            if (projection.CreateWrapperAfterAccessors)
            {
                AddArrayCreateWrapper(projection, rustWrapperMethods);
            }

            if (projection.CopyMethodName is not null && projection.CopyRustAbiType is not null && projection.CopyWrapperSignature is not null)
            {
                var copySymbol = projection.BaseSymbol + "_copy";
                managedGlueBindings.Add(Glue(
                    copySymbol,
                    projection.HelperPrefix + "Copy",
                    [I32("arrayHandle"), Pointer("destinationPointer", projection.CopyRustAbiType), I64("destinationCapacity"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        StaticMethod(
                            typeof(ManagedInteropRuntime),
                            projection.CopyMethodName,
                            [projection.ArrayType, typeof(IntPtr), typeof(long)],
                            [ManagedObject(projection.ArrayType, "arrayHandle"), ManagedGlueExpression.Parameter("destinationPointer"), ManagedGlueExpression.Parameter("destinationCapacity")])))));
                rustWrapperMethods.Add(new RustWrapperMethod(
                    projection.Container,
                    projection.CopyWrapperSignature,
                    copySymbol,
                    ["self.handle", "buffer.as_mut_ptr()", "buffer.len() as i64"],
                    "written",
                    RustWrapperResult.Scalar("i32")));
            }

            rustWrapperMethods.Add(new RustWrapperMethod(
                projection.Container,
                "pub fn release(self) -> Result<(), Exception>",
                "rustlyn_bindgen_system_object_release",
                ["self.handle"],
                "unused",
                RustWrapperResult.Void()));
        }

        return new ArrayHandleBindingSet(requirements, managedGlueBindings, rustWrapperMethods);
    }

    private static void AddArrayCreateGlueBinding(
        ArrayHandleProjection projection,
        ICollection<ManagedGlueBinding> managedGlueBindings)
    {
        var createSymbol = projection.BaseSymbol + CreateSymbolSuffix(projection.CreateWrapperName);
        managedGlueBindings.Add(Glue(
            createSymbol,
            projection.HelperPrefix + CreateHelperSuffix(projection.CreateWrapperName),
            [.. projection.CreateParameters, Pointer("exceptionOutPointer")],
            ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                StaticMethod(typeof(ManagedInteropRuntime), projection.CreateMethodName, projection.CreateParameterTypes, projection.CreateArguments)))));
    }

    private static void AddArrayCreateWrapper(
        ArrayHandleProjection projection,
        ICollection<RustWrapperMethod> rustWrapperMethods)
    {
        rustWrapperMethods.Add(new RustWrapperMethod(
            projection.Container,
            projection.CreateWrapperSignature,
            projection.BaseSymbol + CreateSymbolSuffix(projection.CreateWrapperName),
            projection.CreateWrapperArguments,
            "object_handle",
            RustWrapperResult.ObjectHandle("Self")));
    }

    private static string CreateSymbolSuffix(string wrapperName)
        => wrapperName switch
        {
            "from_strings" => "_from_strings",
            "from_i32_triplet" => "_from_i32_triplet",
            "from_u8_triplet" => "_from_u8_triplet",
            _ => $"_{wrapperName}"
        };

    private static string CreateHelperSuffix(string wrapperName)
        => string.Concat(wrapperName.Split('_').Select(static part => char.ToUpperInvariant(part[0]) + part[1..]));

    private static string FormatParameterTypes(IReadOnlyList<Type> parameterTypes)
        => string.Join(", ", parameterTypes.Select(FormatParameterType));

    private static string FormatParameterType(Type type)
    {
        if (type.IsArray)
        {
            var elementType = type.GetElementType()
                ?? throw new InvalidOperationException($"Array type '{type}' does not expose an element type.");
            return FormatParameterType(elementType) + "[]";
        }

        return type switch
        {
            _ when type == typeof(string) => "string",
            _ when type == typeof(int) => "int",
            _ when type == typeof(byte) => "byte",
            _ when type == typeof(long) => "long",
            _ when type == typeof(IntPtr) => "IntPtr",
            _ => type.Name
        };
    }

    private static Utf8AdapterBindingSet CreateManagedStringUtf8AdapterBindings()
    {
        var receiver = ManagedObject(typeof(string), "stringHandle");
        return new Utf8AdapterBindingSet(
            [
                Glue(
                    "rustlyn_bindgen_system_string_from_utf8",
                    "BindgenSystemStringFromUtf8",
                    [Pointer("valuePointer"), I64("valueLength"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.ObjectHandle(
                        Utf8String("valuePointer", "valueLength"))))
            ],
            [
                Glue(
                    "rustlyn_bindgen_system_string_utf8_len",
                    "BindgenSystemStringUtf8Length",
                    [I32("stringHandle"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        ManagedGlueExpression.Utf8ByteCount(receiver)))),
                Glue(
                    "rustlyn_bindgen_system_string_copy_utf8",
                    "BindgenSystemStringCopyUtf8",
                    [I32("stringHandle"), Pointer("destinationPointer"), I64("destinationCapacity"), Pointer("exceptionOutPointer")],
                    ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", ManagedGlueResult.Int(
                        ManagedGlueExpression.Utf8Copy(receiver, "destinationPointer", "destinationCapacity"))))
            ],
            [
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn from_utf8_parts(value_ptr: *const u8, value_len: i64) -> Result<Self, Exception>",
                    "rustlyn_bindgen_system_string_from_utf8",
                    ["value_ptr", "value_len"],
                    "object_handle",
                    RustWrapperResult.ObjectHandle("Self"))
            ],
            [
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn utf8_len(&self) -> Result<i32, Exception>",
                    "rustlyn_bindgen_system_string_utf8_len",
                    ["self.handle"],
                    "length",
                    RustWrapperResult.Scalar("i32")),
                new RustWrapperMethod(
                    RustWrapperContainer.ManagedString,
                    "pub fn copy_utf8_into(&self, buffer: &mut [u8]) -> Result<i32, Exception>",
                    "rustlyn_bindgen_system_string_copy_utf8",
                    ["self.handle", "buffer.as_mut_ptr()", "buffer.len() as i64"],
                    "written",
                    RustWrapperResult.Scalar("i32"))
            ]);
    }

    private static ManagedGlueBinding Glue(string symbol, string helperMethodName, IReadOnlyList<ManagedGlueParameter> parameters, ManagedGlueOperation operation)
        => new(symbol, helperMethodName, parameters, operation);

    private static ManagedGlueParameter I32(string name)
        => new("int", name);

    private static ManagedGlueParameter I64(string name)
        => new("long", name);

    private static ManagedGlueParameter F64(string name)
        => new("double", name);

    private static ManagedGlueParameter Callback(string name, string rustAbiType)
        => new("IntPtr", name, rustAbiType);

    private static ManagedGlueParameter Pointer(string name)
        => new("IntPtr", name);

    private static ManagedGlueParameter Pointer(string name, string rustAbiType)
        => new("IntPtr", name, rustAbiType);

    private static ManagedGlueExpression Utf8String(string pointerParameterName, string lengthParameterName)
        => ManagedGlueExpression.Utf8String(pointerParameterName, lengthParameterName);

    private static ManagedGlueExpression ManagedObject(Type type, string handleParameterName)
        => ManagedGlueExpression.ManagedObject(type, handleParameterName);

    private static ManagedGlueExpression EnumParameter(Type enumType, string parameterName)
        => ManagedGlueExpression.EnumParameter(enumType, parameterName);

    private static ManagedGlueExpression StaticMethod(Type declaringType, string methodName, IReadOnlyList<Type> parameterTypes, IReadOnlyList<ManagedGlueExpression> arguments)
        => ManagedGlueExpression.StaticMethod(declaringType, methodName, parameterTypes, arguments);

    private static ManagedGlueExpression StaticProperty(Type declaringType, string propertyName)
        => ManagedGlueExpression.StaticProperty(declaringType, propertyName);

    private static ManagedGlueExpression InstanceMethod(ManagedGlueExpression instance, Type declaringType, string methodName, IReadOnlyList<Type> parameterTypes, IReadOnlyList<ManagedGlueExpression> arguments)
        => ManagedGlueExpression.InstanceMethod(instance, declaringType, methodName, parameterTypes, arguments);

    private static ManagedGlueExpression InstanceProperty(ManagedGlueExpression instance, Type declaringType, string propertyName)
        => ManagedGlueExpression.InstanceProperty(instance, declaringType, propertyName);
}

internal sealed record ArrayHandleBindingSet(
    IReadOnlyList<ManagedApiRequirement> Requirements,
    IReadOnlyList<ManagedGlueBinding> ManagedGlueBindings,
    IReadOnlyList<RustWrapperMethod> RustWrapperMethods);

internal sealed record ArrayHandleProjection(
    string DisplayName,
    Type ArrayType,
    Type ElementType,
    RustWrapperContainer Container,
    string BaseSymbol,
    string HelperPrefix,
    string CreateWrapperName,
    string CreateWrapperSignature,
    IReadOnlyList<string> CreateWrapperArguments,
    IReadOnlyList<ManagedGlueParameter> CreateParameters,
    IReadOnlyList<ManagedGlueExpression> CreateArguments,
    string CreateMethodName,
    IReadOnlyList<Type> CreateParameterTypes,
    string GetWrapperSignature,
    RustWrapperResult GetResult,
    string GetResultVariableName,
    string? CopyRequirementName = null,
    string? CopyMethodName = null,
    string? CopyRustAbiType = null,
    string? CopyWrapperSignature = null,
    bool CreateGlueAfterAccessors = false,
    bool CreateWrapperAfterAccessors = false);

internal sealed record Utf8AdapterBindingSet(
    IReadOnlyList<ManagedGlueBinding> InputManagedGlueBindings,
    IReadOnlyList<ManagedGlueBinding> OutputManagedGlueBindings,
    IReadOnlyList<RustWrapperMethod> InputRustWrapperMethods,
    IReadOnlyList<RustWrapperMethod> OutputRustWrapperMethods);

public sealed record ManagedApiRequirement(string DisplayName, Type Type, ManagedApiRequirementKind Kind, string? MemberName, IReadOnlyList<Type> ParameterTypes)
{
    public static ManagedApiRequirement ForType(string displayName, Type type)
        => new(displayName, type, ManagedApiRequirementKind.Type, MemberName: null, []);

    public static ManagedApiRequirement Method(string displayName, Type type, string methodName, IReadOnlyList<Type> parameterTypes)
        => new(displayName, type, ManagedApiRequirementKind.Method, methodName, parameterTypes);

    public static ManagedApiRequirement Property(string displayName, Type type, string propertyName)
        => new(displayName, type, ManagedApiRequirementKind.Property, propertyName, []);

    public static ManagedApiRequirement Constructor(string displayName, Type type, IReadOnlyList<Type> parameterTypes)
        => new(displayName, type, ManagedApiRequirementKind.Constructor, ".ctor", parameterTypes);

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

        if (Kind == ManagedApiRequirementKind.Constructor)
        {
            _ = Type.GetConstructor(ParameterTypes.ToArray())
                ?? throw new InvalidOperationException($"Managed binding requirement '{DisplayName}' could not be resolved.");
            return;
        }

        throw new NotSupportedException($"Managed binding requirement kind '{Kind}' is not supported.");
    }
}

public enum ManagedApiRequirementKind
{
    Type,
    Method,
    Property,
    Event,
    Constructor
}

public sealed record RustEnumProjection(string RustName, Type ManagedType, bool IsFlags, IReadOnlyList<RustEnumVariant> Variants)
{
    public static RustEnumProjection FromManagedEnum(Type managedType)
    {
        ArgumentNullException.ThrowIfNull(managedType);

        if (!managedType.IsEnum)
        {
            throw new ArgumentException($"Managed type '{managedType.FullName}' is not an enum.", nameof(managedType));
        }

        var variants = Enum.GetNames(managedType)
            .Select(name => new RustEnumVariant(name, Convert.ToInt32(Enum.Parse(managedType, name), System.Globalization.CultureInfo.InvariantCulture)))
            .ToArray();
        return new RustEnumProjection(managedType.Name, managedType, managedType.IsDefined(typeof(FlagsAttribute), inherit: false), variants);
    }

    public void Validate()
    {
        if (!ManagedType.IsEnum)
        {
            throw new InvalidOperationException($"Rust enum projection '{RustName}' targets non-enum managed type '{ManagedType.FullName}'.");
        }

        if (Enum.GetUnderlyingType(ManagedType) != typeof(int))
        {
            throw new InvalidOperationException($"Rust enum projection '{RustName}' requires an int-backed managed enum.");
        }

        var expectedVariants = Enum.GetNames(ManagedType)
            .Select(name => new RustEnumVariant(name, Convert.ToInt32(Enum.Parse(ManagedType, name), System.Globalization.CultureInfo.InvariantCulture)))
            .ToArray();
        if (!Variants.SequenceEqual(expectedVariants))
        {
            throw new InvalidOperationException($"Rust enum projection '{RustName}' does not match managed enum '{ManagedType.FullName}'.");
        }
    }
}

public sealed record RustEnumVariant(string Name, int Value);

public sealed record RustExternBinding(string Symbol, IReadOnlyList<string> SignatureLines)
{
    public static RustExternBinding FromManagedGlueBinding(ManagedGlueBinding binding)
    {
        binding.Validate();

        var parameters = binding.Parameters
            .Select(static parameter => $"{ToRustParameterName(parameter.Name)}: {ToRustParameterType(parameter)}")
            .ToArray();
        var returnType = ToRustReturnType(binding.ReturnType);
        var singleLine = $"fn {binding.Symbol}({string.Join(", ", parameters)}) -> {returnType};";
        if (singleLine.Length <= 120)
        {
            return new RustExternBinding(binding.Symbol, [singleLine]);
        }

        var lines = new List<string>
        {
            $"fn {binding.Symbol}("
        };
        lines.AddRange(parameters.Select(static parameter => $"    {parameter},"));
        lines.Add($") -> {returnType};");
        return new RustExternBinding(binding.Symbol, lines);
    }

    private static string ToRustParameterType(ManagedGlueParameter parameter)
    {
        if (parameter.RustAbiType is not null)
        {
            return parameter.RustAbiType;
        }

        return parameter.TypeName switch
        {
            "int" => "i32",
            "long" => "i64",
            "float" => "f32",
            "double" => "f64",
            "IntPtr" when parameter.Name.EndsWith("OutPointer", StringComparison.Ordinal) => "*mut i32",
            "IntPtr" when parameter.Name.StartsWith("destination", StringComparison.Ordinal) => "*mut u8",
            "IntPtr" => "*const u8",
            _ => throw new NotSupportedException($"Managed glue parameter type '{parameter.TypeName}' is not supported in Rust extern bindings.")
        };
    }

    private static string ToRustReturnType(string managedReturnType)
    {
        return managedReturnType switch
        {
            "int" => "i32",
            "long" => "i64",
            "float" => "f32",
            "double" => "f64",
            _ => throw new NotSupportedException($"Managed glue return type '{managedReturnType}' is not supported in Rust extern bindings.")
        };
    }

    private static string ToRustParameterName(string managedName)
    {
        var snake = ToSnakeCase(managedName);
        if (string.Equals(snake, "exception_out_pointer", StringComparison.Ordinal))
        {
            return "exception_out";
        }

        if (snake.EndsWith("_pointer", StringComparison.Ordinal))
        {
            return snake[..^"_pointer".Length] + "_ptr";
        }

        return snake;
    }

    private static string ToSnakeCase(string value)
    {
        var chars = new List<char>(value.Length + 8);
        for (var index = 0; index < value.Length; index++)
        {
            var current = value[index];
            if (char.IsUpper(current))
            {
                if (index > 0)
                {
                    chars.Add('_');
                }

                chars.Add(char.ToLowerInvariant(current));
            }
            else
            {
                chars.Add(current);
            }
        }

        return new string(chars.ToArray());
    }
}

public sealed record RustWrapperMethod(
    RustWrapperContainer Container,
    string Signature,
    string ExternSymbol,
    IReadOnlyList<string> Arguments,
    string ResultVariableName,
    RustWrapperResult Result);

public enum RustWrapperContainer
{
    Callback,
    Console,
    Convert,
    Environment,
    Exception,
    Math,
    MathF,
    TimeSpan,
    DateTimeOffset,
    Gc,
    Guid,
    Task,
    IoDirectory,
    IoFile,
    IoPath,
    ManagedString,
    ManagedStringArray,
    ManagedIntArray,
    ManagedByteArray,
    ManagedTimeSpan,
    ManagedDateTimeOffset,
    ManagedGuid,
    OperatingSystem,
    Uri
}

public sealed record RustWrapperResult(RustWrapperResultKind Kind, string? RustType)
{
    public static RustWrapperResult Boolean()
        => new(RustWrapperResultKind.Boolean, RustType: null);

    public static RustWrapperResult BooleanAsInt()
        => new(RustWrapperResultKind.BooleanAsInt, RustType: null);

    public static RustWrapperResult ObjectHandle(string rustType)
        => new(RustWrapperResultKind.ObjectHandle, rustType);

    public static RustWrapperResult Scalar(string rustType)
        => new(RustWrapperResultKind.Scalar, rustType);

    public static RustWrapperResult Void()
        => new(RustWrapperResultKind.Void, RustType: null);
}

public enum RustWrapperResultKind
{
    Boolean,
    BooleanAsInt,
    ObjectHandle,
    Future,
    Scalar,
    Void
}

public sealed record ManagedGlueBinding(
    string Symbol,
    string RuntimeBridgeHelperMethodName,
    IReadOnlyList<ManagedGlueParameter> Parameters,
    ManagedGlueOperation Operation)
{
    public string ReturnType => Operation.Result switch
    {
        ManagedGlueLongResult => "long",
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

public sealed record ManagedGlueParameter(string TypeName, string Name, string? RustAbiType = null);

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

    public static ManagedGlueResult Long(ManagedGlueExpression valueExpression)
        => new ManagedGlueLongResult(valueExpression);

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

public sealed record ManagedGlueLongResult(ManagedGlueExpression ValueExpression) : ManagedGlueResult
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
