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
                ManagedApiRequirement.Method("System.IO.Path.GetFileName(string)", typeof(Path), nameof(Path.GetFileName), [typeof(string)]),
                ManagedApiRequirement.Method("System.IO.Path.GetFileNameWithoutExtension(string)", typeof(Path), nameof(Path.GetFileNameWithoutExtension), [typeof(string)]),
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
                    "rust_mcil_bindgen_system_io_path_get_file_name_string",
                    ["fn rust_mcil_bindgen_system_io_path_get_file_name_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
                new RustExternBinding(
                    "rust_mcil_bindgen_system_io_path_get_file_name_without_extension_string",
                    ["fn rust_mcil_bindgen_system_io_path_get_file_name_without_extension_string(path_handle: i32, exception_out: *mut i32) -> i32;"]),
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
                    ["fn rust_mcil_bindgen_system_object_release(handle: i32) -> i32;"])
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
                    ManagedGlueOperation.ReturnExceptionHandle(ManagedGlueResult.ReleaseObject("objectHandle")))
            ],
            [
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
                    RustWrapperResult.ObjectHandle("ManagedString"))
            ]);
    }

    private static ManagedGlueBinding Glue(string symbol, string helperMethodName, IReadOnlyList<ManagedGlueParameter> parameters, ManagedGlueOperation operation)
        => new(symbol, helperMethodName, parameters, operation);

    private static ManagedGlueParameter I32(string name)
        => new("int", name);

    private static ManagedGlueParameter I64(string name)
        => new("long", name);

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
    IoPath
}

public sealed record RustWrapperResult(RustWrapperResultKind Kind, string? RustType)
{
    public static RustWrapperResult ObjectHandle(string rustType)
        => new(RustWrapperResultKind.ObjectHandle, rustType);
}

public enum RustWrapperResultKind
{
    ObjectHandle
}

public sealed record ManagedGlueBinding(
    string Symbol,
    string RuntimeBridgeHelperMethodName,
    IReadOnlyList<ManagedGlueParameter> Parameters,
    ManagedGlueOperation Operation)
{
    public string ReturnType => "int";

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

    public static ManagedGlueResult BooleanAsInt(ManagedGlueExpression valueExpression)
        => new ManagedGlueBooleanAsIntResult(valueExpression);

    public static ManagedGlueResult ReleaseObject(string handleExpression)
        => new ManagedGlueReleaseObjectResult(handleExpression);
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