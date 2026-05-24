using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Themes.Fluent;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Rustlyn.AvaloniaSupport;

public static class AvaloniaBridge
{
    private const string GeneratedModuleTypeName = "Rustlyn.GeneratedModule";
    private const string BuildUiMethodName = "avalonia_build_ui";
    private const string OnClickMethodName = "avalonia_on_click";
    private const string InitialSmokeText = "hello world";
    private const string ClickedSmokeText = "hello world from Rust click";

    private static AvaloniaSession? currentSession;

    public static int RunApp()
    {
        var args = Environment.GetCommandLineArgs().Skip(1).ToArray();
        currentSession = new AvaloniaSession();

        if (args.Any(static arg => string.Equals(arg, "--smoke", StringComparison.Ordinal)))
        {
            try
            {
                return RunSmoke();
            }
            finally
            {
                currentSession = null;
            }
        }

        try
        {
            return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        finally
        {
            currentSession = null;
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<RustAvaloniaApp>().UsePlatformDetect();

    public static int CreateWindow()
        => CurrentSession.Add(new Window());

    public static int CreateStackPanel()
        => CurrentSession.Add(new StackPanel { Orientation = Orientation.Vertical });

    public static int CreateTextBlock()
        => CurrentSession.Add(new TextBlock());

    public static int CreateButton()
        => CurrentSession.Add(new Button());

    public static void SetWindowTitleUtf8(int windowHandle, IntPtr titlePointer, long titleLength)
    {
        CurrentSession.Get<Window>(windowHandle).Title = ReadUtf8String(titlePointer, titleLength);
    }

    public static void SetWindowSize(int windowHandle, int width, int height)
    {
        var window = CurrentSession.Get<Window>(windowHandle);
        window.Width = width;
        window.Height = height;
    }

    public static void SetWindowContent(int windowHandle, int contentHandle)
    {
        CurrentSession.Get<Window>(windowHandle).Content = CurrentSession.Get<Control>(contentHandle);
    }

    public static void SetStackPanelSpacing(int stackPanelHandle, int spacing)
    {
        CurrentSession.Get<StackPanel>(stackPanelHandle).Spacing = spacing;
    }

    public static void SetStackPanelMargin(int stackPanelHandle, int margin)
    {
        CurrentSession.Get<StackPanel>(stackPanelHandle).Margin = new Thickness(margin);
    }

    public static void AddStackPanelChild(int stackPanelHandle, int childHandle)
    {
        CurrentSession.Get<StackPanel>(stackPanelHandle).Children.Add(CurrentSession.Get<Control>(childHandle));
    }

    public static void SetTextBlockTextUtf8(int textBlockHandle, IntPtr textPointer, long textLength)
    {
        CurrentSession.Get<TextBlock>(textBlockHandle).Text = ReadUtf8String(textPointer, textLength);
    }

    public static void SetButtonContentUtf8(int buttonHandle, IntPtr contentPointer, long contentLength)
    {
        CurrentSession.Get<Button>(buttonHandle).Content = ReadUtf8String(contentPointer, contentLength);
    }

    public static void SetButtonOnClick(int buttonHandle, int handlerId, int stateHandle)
    {
        var button = CurrentSession.Get<Button>(buttonHandle);
        CurrentSession.RegisterClick(buttonHandle, () => InvokeRustClick(handlerId, stateHandle));
        button.Click += (_, _) => InvokeRustClick(handlerId, stateHandle);
    }

    internal static Window BuildMainWindowFromRust()
    {
        var windowHandle = InvokeRustBuildUi();
        return CurrentSession.Get<Window>(windowHandle);
    }

    private static int RunSmoke()
    {
        try
        {
            BuildAvaloniaApp().SetupWithoutStarting();
            _ = InvokeRustBuildUi();

            var textBlock = CurrentSession.FindFirst<TextBlock>();
            if (!string.Equals(textBlock.Text, InitialSmokeText, StringComparison.Ordinal))
            {
                return SmokeFail($"Expected initial text '{InitialSmokeText}', but found '{textBlock.Text}'.");
            }

            CurrentSession.InvokeFirstClick();
            if (!string.Equals(textBlock.Text, ClickedSmokeText, StringComparison.Ordinal))
            {
                return SmokeFail($"Expected clicked text '{ClickedSmokeText}', but found '{textBlock.Text}'.");
            }

            Console.WriteLine("avalonia:rust-ui:ok");
            return 0;
        }
        catch (Exception ex)
        {
            return SmokeFail(ex.ToString());
        }
    }

    private static int SmokeFail(string message)
    {
        Console.Error.WriteLine(message);
        Console.WriteLine("avalonia:rust-ui:fail");
        return 2;
    }

    private static int InvokeRustBuildUi()
    {
        var method = GetGeneratedMethod(BuildUiMethodName, typeof(int), Type.EmptyTypes);
        return (int)InvokeGeneratedMethod(method, null)!;
    }

    private static void InvokeRustClick(int handlerId, int stateHandle)
    {
        var method = GetGeneratedMethod(OnClickMethodName, typeof(void), [typeof(int), typeof(int)]);
        _ = InvokeGeneratedMethod(method, [handlerId, stateHandle]);
    }

    private static MethodInfo GetGeneratedMethod(string name, Type returnType, Type[] parameterTypes)
    {
        var generatedType = GetGeneratedModuleType();
        var method = generatedType.GetMethod(name, BindingFlags.Public | BindingFlags.Static, binder: null, types: parameterTypes, modifiers: null)
            ?? throw new MissingMethodException(generatedType.FullName, name);

        if (method.ReturnType != returnType)
        {
            throw new InvalidOperationException($"Generated method '{name}' returned '{method.ReturnType}', but '{returnType}' was expected.");
        }

        return method;
    }

    private static Type GetGeneratedModuleType()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly?.GetType(GeneratedModuleTypeName) is { } entryType)
        {
            return entryType;
        }

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.GetType(GeneratedModuleTypeName) is { } generatedType)
            {
                return generatedType;
            }
        }

        throw new InvalidOperationException($"Generated module type '{GeneratedModuleTypeName}' could not be found.");
    }

    private static object? InvokeGeneratedMethod(MethodInfo method, object?[]? arguments)
    {
        try
        {
            return method.Invoke(null, arguments);
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            throw;
        }
    }

    private static string ReadUtf8String(IntPtr pointer, long length)
    {
        if (length == 0)
        {
            return string.Empty;
        }

        var bytes = new byte[checked((int)length)];
        Marshal.Copy(pointer, bytes, 0, bytes.Length);
        return Encoding.UTF8.GetString(bytes);
    }

    private static AvaloniaSession CurrentSession
        => currentSession ?? throw new InvalidOperationException("Avalonia bridge session has not been initialized.");

    private sealed class AvaloniaSession
    {
        private readonly Dictionary<int, object> objects = new();
        private readonly Dictionary<int, Action> clickHandlers = new();
        private int nextHandle = 1;

        public int Add(object value)
        {
            var handle = nextHandle++;
            objects.Add(handle, value);
            return handle;
        }

        public T Get<T>(int handle)
            where T : class
        {
            if (!objects.TryGetValue(handle, out var value))
            {
                throw new KeyNotFoundException($"Avalonia object handle {handle} was not found.");
            }

            return value as T
                ?? throw new InvalidOperationException($"Avalonia object handle {handle} is '{value.GetType().FullName}', not '{typeof(T).FullName}'.");
        }

        public T FindFirst<T>()
            where T : class
        {
            return objects.Values.OfType<T>().FirstOrDefault()
                ?? throw new InvalidOperationException($"No Avalonia object of type '{typeof(T).FullName}' has been created.");
        }

        public void RegisterClick(int buttonHandle, Action handler)
        {
            clickHandlers[buttonHandle] = handler;
        }

        public void InvokeFirstClick()
        {
            var handler = clickHandlers.OrderBy(static pair => pair.Key).Select(static pair => pair.Value).FirstOrDefault()
                ?? throw new InvalidOperationException("No Avalonia button click handler has been registered.");
            handler();
        }
    }
}

public sealed class RustAvaloniaApp : Application
{
    public override void Initialize()
    {
        Styles.Add(new FluentTheme(null));
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = AvaloniaBridge.BuildMainWindowFromRust();
        }

        base.OnFrameworkInitializationCompleted();
    }
}