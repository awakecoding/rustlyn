using Avalonia;
using Avalonia.Controls;
using Rustlyn.AvaloniaSupport;
using Rustlyn.Interop;

namespace Rustlyn.Bindings;

public static class ExternalPackageBindingSurfaces
{
    private const string PowerShellSdkPackageVersion = "7.5.0";

    public static BindingManifestDocument CreateAvaloniaHelloManifest()
        => BindingManifestFactory.FromExternalPackageSurface(
            CreateAvaloniaHelloSurface(),
            CreateAvaloniaHelloPackageSurface());

    public static BindingManifestPackageSurface CreateAvaloniaHelloPackageSurface()
    {
        return new BindingManifestPackageSurface(
            "Avalonia",
            "12.0.3",
            "net10.0",
            [
                PackageAssembly(typeof(Window).Assembly, "compile"),
                PackageAssembly(typeof(Application).Assembly, "compile"),
                PackageAssembly(typeof(AvaloniaBridge).Assembly, "bootstrap")
            ],
            [
                new BindingManifestPackageDependency("Avalonia.Desktop", "12.0.3"),
                new BindingManifestPackageDependency("Avalonia.Themes.Fluent", "12.0.3")
            ],
            [
                "runtimes/**/native/*",
                "*.dll",
                "*.json"
            ]);
    }

    public static BindingSurface CreateAvaloniaHelloSurface()
    {
        return new BindingSurface(
            [
                ManagedApiRequirement.ForType("Avalonia.Controls.Window", typeof(Window)),
                ManagedApiRequirement.ForType("Avalonia.Controls.StackPanel", typeof(StackPanel)),
                ManagedApiRequirement.ForType("Avalonia.Controls.TextBlock", typeof(TextBlock)),
                ManagedApiRequirement.ForType("Avalonia.Controls.Button", typeof(Button)),
                ManagedApiRequirement.ForType("Avalonia.Controls.Control", typeof(Control)),
                ManagedApiRequirement.ForType("Avalonia.Thickness", typeof(Thickness)),
                ManagedApiRequirement.ForType("Rustlyn.AvaloniaSupport.AvaloniaBridge", typeof(AvaloniaBridge))
            ],
            CreateAvaloniaHelloBindings(),
            [],
            []);
    }

    public static BindingManifestDocument CreatePowerShellCmdletManifest()
        => BindingManifestFactory.FromExternalPackageSurface(
            CreatePowerShellCmdletSurface(),
            CreatePowerShellCmdletPackageSurface());

    public static BindingManifestPackageSurface CreatePowerShellCmdletPackageSurface()
    {
        return new BindingManifestPackageSurface(
            "Microsoft.PowerShell.SDK",
            PowerShellSdkPackageVersion,
            "net10.0",
            [
                new BindingManifestPackageAssembly(
                    "System.Management.Automation",
                    new BindingManifestAssemblyIdentity("System.Management.Automation", PowerShellSdkPackageVersion, null, "31bf3856ad364e35"),
                    "compile",
                    string.Empty),
                new BindingManifestPackageAssembly(
                    "Rustlyn.PowerShellSupport",
                    new BindingManifestAssemblyIdentity("Rustlyn.PowerShellSupport", null, null, string.Empty),
                    "bootstrap",
                    string.Empty),
                PackageAssembly(typeof(ManagedInteropRuntime).Assembly, "bootstrap"),
                new BindingManifestPackageAssembly(
                    "Rustlyn.PowerShellCmdlets.Generated",
                    new BindingManifestAssemblyIdentity("Rustlyn.PowerShellCmdlets.Generated", null, null, string.Empty),
                    "generated-shim",
                    string.Empty)
            ],
            [
                new BindingManifestPackageDependency("System.Management.Automation", PowerShellSdkPackageVersion)
            ],
            [
                "*.dll",
                "*.psd1",
                "*.psm1",
                "runtimes/**/native/*"
            ]);
    }

    public static BindingSurface CreatePowerShellCmdletSurface()
    {
        return new BindingSurface(
            [],
            CreatePowerShellCmdletBindings(),
            [],
            []);
    }

    private static ManagedGlueBinding[] CreateAvaloniaHelloBindings()
    {
        return
        [
            ObjectBinding(
                "rustlyn_bindgen_avalonia_controls_window_new",
                "BindgenAvaloniaControlsWindowNew",
                [],
                ManagedGlueExpression.Constructor(typeof(Window), [], [])),
            ObjectBinding(
                "rustlyn_bindgen_avalonia_controls_stack_panel_new",
                "BindgenAvaloniaControlsStackPanelNew",
                [],
                ManagedGlueExpression.Raw("new Avalonia.Controls.StackPanel { Orientation = Avalonia.Layout.Orientation.Vertical }")),
            ObjectBinding(
                "rustlyn_bindgen_avalonia_controls_text_block_new",
                "BindgenAvaloniaControlsTextBlockNew",
                [],
                ManagedGlueExpression.Constructor(typeof(TextBlock), [], [])),
            ObjectBinding(
                "rustlyn_bindgen_avalonia_controls_button_new",
                "BindgenAvaloniaControlsButtonNew",
                [],
                ManagedGlueExpression.Constructor(typeof(Button), [], [])),
            ObjectBinding(
                "rustlyn_bindgen_avalonia_thickness_new_f64",
                "BindgenAvaloniaThicknessNewF64",
                [F64("value")],
                ManagedGlueExpression.Constructor(typeof(Thickness), [typeof(double)], [ManagedGlueExpression.Parameter("value")])),
            VoidBinding(
                "rustlyn_bindgen_avalonia_controls_window_set_title_utf8",
                "BindgenAvaloniaControlsWindowSetTitleUtf8",
                [I32("windowHandle"), Pointer("titlePointer"), I64("titleLength")],
                "ManagedInteropRuntime.GetObject<Avalonia.Controls.Window>(windowHandle).Title = InteropUtf8.ReadString(titlePointer, titleLength)"),
            VoidBinding(
                "rustlyn_bindgen_avalonia_controls_window_set_width_f64",
                "BindgenAvaloniaControlsWindowSetWidthF64",
                [I32("windowHandle"), F64("width")],
                "ManagedInteropRuntime.GetObject<Avalonia.Controls.Window>(windowHandle).Width = width"),
            VoidBinding(
                "rustlyn_bindgen_avalonia_controls_window_set_height_f64",
                "BindgenAvaloniaControlsWindowSetHeightF64",
                [I32("windowHandle"), F64("height")],
                "ManagedInteropRuntime.GetObject<Avalonia.Controls.Window>(windowHandle).Height = height"),
            VoidBinding(
                "rustlyn_bindgen_avalonia_controls_window_set_content_control",
                "BindgenAvaloniaControlsWindowSetContentControl",
                [I32("windowHandle"), I32("contentHandle")],
                "ManagedInteropRuntime.GetObject<Avalonia.Controls.Window>(windowHandle).Content = ManagedInteropRuntime.GetObject<Avalonia.Controls.Control>(contentHandle)"),
            VoidBinding(
                "rustlyn_bindgen_avalonia_controls_stack_panel_set_spacing_f64",
                "BindgenAvaloniaControlsStackPanelSetSpacingF64",
                [I32("stackPanelHandle"), F64("spacing")],
                "ManagedInteropRuntime.GetObject<Avalonia.Controls.StackPanel>(stackPanelHandle).Spacing = spacing"),
            VoidBinding(
                "rustlyn_bindgen_avalonia_controls_stack_panel_set_margin_thickness",
                "BindgenAvaloniaControlsStackPanelSetMarginThickness",
                [I32("stackPanelHandle"), I32("marginHandle")],
                "ManagedInteropRuntime.GetObject<Avalonia.Controls.StackPanel>(stackPanelHandle).Margin = ManagedInteropRuntime.GetObject<Avalonia.Thickness>(marginHandle)"),
            VoidBinding(
                "rustlyn_bindgen_avalonia_controls_panel_children_add_control",
                "BindgenAvaloniaControlsPanelChildrenAddControl",
                [I32("panelHandle"), I32("childHandle")],
                "ManagedInteropRuntime.GetObject<Avalonia.Controls.Panel>(panelHandle).Children.Add(ManagedInteropRuntime.GetObject<Avalonia.Controls.Control>(childHandle))"),
            VoidBinding(
                "rustlyn_bindgen_avalonia_controls_text_block_set_text_utf8",
                "BindgenAvaloniaControlsTextBlockSetTextUtf8",
                [I32("textBlockHandle"), Pointer("textPointer"), I64("textLength")],
                "ManagedInteropRuntime.GetObject<Avalonia.Controls.TextBlock>(textBlockHandle).Text = InteropUtf8.ReadString(textPointer, textLength)"),
            VoidBinding(
                "rustlyn_bindgen_avalonia_controls_button_set_content_utf8",
                "BindgenAvaloniaControlsButtonSetContentUtf8",
                [I32("buttonHandle"), Pointer("contentPointer"), I64("contentLength")],
                "ManagedInteropRuntime.GetObject<Avalonia.Controls.Button>(buttonHandle).Content = InteropUtf8.ReadString(contentPointer, contentLength)"),
            ObjectBinding(
                "rustlyn_bindgen_avalonia_controls_button_subscribe_click",
                "BindgenAvaloniaControlsButtonSubscribeClick",
                [I32("buttonHandle"), I32("handlerId"), I32("stateHandle")],
                ManagedGlueExpression.Raw("Rustlyn.AvaloniaSupport.AvaloniaBridge.SubscribeButtonClick(ManagedInteropRuntime.GetObject<Avalonia.Controls.Button>(buttonHandle), handlerId, stateHandle)"))
        ];
    }

    private static ManagedGlueBinding[] CreatePowerShellCmdletBindings()
    {
        return
        [
            ObjectBinding(
                "rustlyn_bindgen_powershell_string_from_utf8",
                "BindgenPowerShellStringFromUtf8",
                [Pointer("valuePointer"), I64("valueLength")],
                ManagedGlueExpression.Utf8String("valuePointer", "valueLength")),
            Int32Binding(
                "rustlyn_bindgen_powershell_string_utf8_len",
                "BindgenPowerShellStringUtf8Length",
                [I32("stringHandle")],
                "InteropUtf8.GetByteCount(ManagedInteropRuntime.GetObject<string>(stringHandle))"),
            Int32Binding(
                "rustlyn_bindgen_powershell_string_copy_utf8",
                "BindgenPowerShellStringCopyUtf8",
                [I32("stringHandle"), Pointer("destinationPointer"), I64("destinationCapacity")],
                "InteropUtf8.CopyString(ManagedInteropRuntime.GetObject<string>(stringHandle), destinationPointer, destinationCapacity)"),
            VoidBinding(
                "rustlyn_bindgen_powershell_cmdlet_write_object_string",
                "BindgenPowerShellCmdletWriteObjectString",
                [I32("cmdletContextHandle"), I32("valueHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.WriteObject(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(valueHandle))"),
            VoidBinding(
                "rustlyn_bindgen_powershell_cmdlet_write_object_handle",
                "BindgenPowerShellCmdletWriteObjectHandle",
                [I32("cmdletContextHandle"), I32("valueHandle"), I32("enumerateCollection")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.WriteObject(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<object>(valueHandle), enumerateCollection != 0)"),
            VoidBinding(
                "rustlyn_bindgen_powershell_cmdlet_write_object_bytes",
                "BindgenPowerShellCmdletWriteObjectBytes",
                [I32("cmdletContextHandle"), Pointer("bytesPointer"), I64("byteLength")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.WriteObjectBytes(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), bytesPointer, byteLength)"),
            VoidBinding(
                "rustlyn_bindgen_powershell_cmdlet_write_json_string",
                "BindgenPowerShellCmdletWriteJsonString",
                [I32("cmdletContextHandle"), I32("jsonHandle"), I32("asHashtable"), I32("noEnumerate")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.WriteJson(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(jsonHandle), asHashtable != 0, noEnumerate != 0)"),
            VoidBinding(
                "rustlyn_bindgen_powershell_cmdlet_write_verbose_string",
                "BindgenPowerShellCmdletWriteVerboseString",
                [I32("cmdletContextHandle"), I32("messageHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.WriteVerbose(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(messageHandle))"),
            VoidBinding(
                "rustlyn_bindgen_powershell_cmdlet_write_warning_string",
                "BindgenPowerShellCmdletWriteWarningString",
                [I32("cmdletContextHandle"), I32("messageHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.WriteWarning(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(messageHandle))"),
            VoidBinding(
                "rustlyn_bindgen_powershell_cmdlet_write_error_string",
                "BindgenPowerShellCmdletWriteErrorString",
                [I32("cmdletContextHandle"), I32("messageHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.WriteErrorString(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(messageHandle))"),
            VoidBinding(
                "rustlyn_bindgen_powershell_cmdlet_write_error_record_string",
                "BindgenPowerShellCmdletWriteErrorRecordString",
                [I32("cmdletContextHandle"), I32("messageHandle"), I32("fullyQualifiedErrorIdHandle"), I32("category"), I32("targetObjectHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.WriteErrorRecordString(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(messageHandle), ManagedInteropRuntime.GetObject<string>(fullyQualifiedErrorIdHandle), category, targetObjectHandle == 0 ? null : ManagedInteropRuntime.GetObject<object>(targetObjectHandle))"),
            VoidBinding(
                "rustlyn_bindgen_powershell_cmdlet_throw_terminating_error_record_string",
                "BindgenPowerShellCmdletThrowTerminatingErrorRecordString",
                [I32("cmdletContextHandle"), I32("messageHandle"), I32("fullyQualifiedErrorIdHandle"), I32("category"), I32("targetObjectHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.ThrowTerminatingErrorRecordString(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(messageHandle), ManagedInteropRuntime.GetObject<string>(fullyQualifiedErrorIdHandle), category, targetObjectHandle == 0 ? null : ManagedInteropRuntime.GetObject<object>(targetObjectHandle))"),
            ObjectBinding(
                "rustlyn_bindgen_powershell_cmdlet_get_parameter_string",
                "BindgenPowerShellCmdletGetParameterString",
                [I32("cmdletContextHandle"), I32("nameHandle")],
                ManagedGlueExpression.Raw("Rustlyn.PowerShellSupport.PowerShellCmdletBridge.GetBoundParameterString(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(nameHandle))")),
            BooleanBinding(
                "rustlyn_bindgen_powershell_cmdlet_has_parameter",
                "BindgenPowerShellCmdletHasParameter",
                [I32("cmdletContextHandle"), I32("nameHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.HasBoundParameter(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(nameHandle))"),
            BooleanBinding(
                "rustlyn_bindgen_powershell_cmdlet_get_parameter_bool",
                "BindgenPowerShellCmdletGetParameterBool",
                [I32("cmdletContextHandle"), I32("nameHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.GetBoundParameterBoolean(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(nameHandle))"),
            Int32Binding(
                "rustlyn_bindgen_powershell_cmdlet_get_parameter_i32",
                "BindgenPowerShellCmdletGetParameterI32",
                [I32("cmdletContextHandle"), I32("nameHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.GetBoundParameterInt32(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(nameHandle))"),
            Int32Binding(
                "rustlyn_bindgen_powershell_cmdlet_get_parameter_char",
                "BindgenPowerShellCmdletGetParameterChar",
                [I32("cmdletContextHandle"), I32("nameHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.GetBoundParameterChar(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(nameHandle))"),
            ObjectBinding(
                "rustlyn_bindgen_powershell_cmdlet_get_parameter_snapshot_json",
                "BindgenPowerShellCmdletGetParameterSnapshotJson",
                [I32("cmdletContextHandle"), I32("nameHandle")],
                ManagedGlueExpression.Raw("Rustlyn.PowerShellSupport.PowerShellCmdletBridge.GetBoundParameterSnapshotJson(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(nameHandle))")),
            ObjectBinding(
                "rustlyn_bindgen_powershell_cmdlet_get_input_string",
                "BindgenPowerShellCmdletGetInputString",
                [I32("cmdletContextHandle")],
                ManagedGlueExpression.Raw("Rustlyn.PowerShellSupport.PowerShellCmdletBridge.GetInputString(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle))")),
            ObjectBinding(
                "rustlyn_bindgen_powershell_cmdlet_get_current_culture_list_separator",
                "BindgenPowerShellCmdletGetCurrentCultureListSeparator",
                [I32("cmdletContextHandle")],
                ManagedGlueExpression.Raw("Rustlyn.PowerShellSupport.PowerShellCmdletBridge.GetCurrentCultureListSeparator(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle))")),
            ObjectBinding(
                "rustlyn_bindgen_powershell_cmdlet_get_input_snapshot_json",
                "BindgenPowerShellCmdletGetInputSnapshotJson",
                [I32("cmdletContextHandle")],
                ManagedGlueExpression.Raw("Rustlyn.PowerShellSupport.PowerShellCmdletBridge.GetInputSnapshotJson(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle))")),
            BooleanBinding(
                "rustlyn_bindgen_powershell_cmdlet_should_process_string",
                "BindgenPowerShellCmdletShouldProcessString",
                [I32("cmdletContextHandle"), I32("targetHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.ShouldProcess(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(targetHandle))"),
            BooleanBinding(
                "rustlyn_bindgen_powershell_cmdlet_should_process_action_string",
                "BindgenPowerShellCmdletShouldProcessActionString",
                [I32("cmdletContextHandle"), I32("targetHandle"), I32("actionHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.ShouldProcess(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle), ManagedInteropRuntime.GetObject<string>(targetHandle), ManagedInteropRuntime.GetObject<string>(actionHandle))"),
            BooleanBinding(
                "rustlyn_bindgen_powershell_cmdlet_is_cancellation_requested",
                "BindgenPowerShellCmdletIsCancellationRequested",
                [I32("cmdletContextHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.IsCancellationRequested(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle))"),
            Int32Binding(
                "rustlyn_bindgen_powershell_cmdlet_get_lifecycle_state_handle",
                "BindgenPowerShellCmdletGetLifecycleStateHandle",
                [I32("cmdletContextHandle")],
                "Rustlyn.PowerShellSupport.PowerShellCmdletBridge.GetLifecycleStateHandle(ManagedInteropRuntime.GetObject<Rustlyn.PowerShellSupport.PowerShellCmdletContext>(cmdletContextHandle))"),
            BooleanBinding(
                "rustlyn_bindgen_powershell_object_release",
                "BindgenPowerShellObjectRelease",
                [I32("objectHandle")],
                "ManagedInteropRuntime.ReleaseObject(objectHandle)")
        ];
    }

    private static ManagedGlueBinding ObjectBinding(string symbol, string helperMethodName, IReadOnlyList<ManagedGlueParameter> parameters, ManagedGlueExpression value)
        => new(
            symbol,
            helperMethodName,
            [.. parameters, Pointer("exceptionOutPointer")],
            ManagedGlueOperation.WriteExceptionOut(
                "exceptionOutPointer",
                ManagedGlueResult.ObjectHandle(value)));

    private static ManagedGlueBinding VoidBinding(string symbol, string helperMethodName, IReadOnlyList<ManagedGlueParameter> parameters, string statement)
        => new(
            symbol,
            helperMethodName,
            [.. parameters, Pointer("exceptionOutPointer")],
            ManagedGlueOperation.WriteExceptionOut(
                "exceptionOutPointer",
                ManagedGlueResult.VoidCall(ManagedGlueExpression.Raw(statement))));

    private static ManagedGlueBinding BooleanBinding(string symbol, string helperMethodName, IReadOnlyList<ManagedGlueParameter> parameters, string expression)
        => new(
            symbol,
            helperMethodName,
            [.. parameters, Pointer("exceptionOutPointer")],
            ManagedGlueOperation.WriteExceptionOut(
                "exceptionOutPointer",
                ManagedGlueResult.BooleanAsInt(ManagedGlueExpression.Raw(expression))));

    private static ManagedGlueBinding Int32Binding(string symbol, string helperMethodName, IReadOnlyList<ManagedGlueParameter> parameters, string expression)
        => new(
            symbol,
            helperMethodName,
            [.. parameters, Pointer("exceptionOutPointer")],
            ManagedGlueOperation.WriteExceptionOut(
                "exceptionOutPointer",
                ManagedGlueResult.Int(ManagedGlueExpression.Raw(expression))));

    private static ManagedGlueParameter I32(string name)
        => new("int", name);

    private static ManagedGlueParameter I64(string name)
        => new("long", name);

    private static ManagedGlueParameter F64(string name)
        => new("double", name);

    private static ManagedGlueParameter Pointer(string name)
        => new("IntPtr", name);

    private static BindingManifestPackageAssembly PackageAssembly(System.Reflection.Assembly assembly, string role)
    {
        var name = assembly.GetName();
        return new BindingManifestPackageAssembly(
            name.Name ?? string.Empty,
            BindingManifestAssemblyIdentity.From(name),
            role,
            assembly.Location);
    }
}
