using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Rustlyn.Interop;

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

    public static IDisposable SubscribeButtonClick(Button button, int handlerId, int stateHandle)
    {
        ArgumentNullException.ThrowIfNull(button);

        void Handler(object? sender, Avalonia.Interactivity.RoutedEventArgs args)
            => InvokeRustClick(handlerId, stateHandle);

        CurrentSession.RegisterClick(stateHandle, () => InvokeRustClick(handlerId, stateHandle));
        CurrentSession.SmokeTextBlockHandle = stateHandle;
        button.Click += Handler;
        return new AvaloniaEventSubscription(() => button.Click -= Handler);
    }

    internal static Window BuildMainWindowFromRust()
    {
        var windowHandle = InvokeRustBuildUi();
        return ManagedInteropRuntime.GetObject<Window>(windowHandle);
    }

    private static int RunSmoke()
    {
        try
        {
            BuildAvaloniaApp().SetupWithoutStarting();
            _ = InvokeRustBuildUi();

            var textBlock = ManagedInteropRuntime.GetObject<TextBlock>(CurrentSession.SmokeTextBlockHandle);
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

    private static AvaloniaSession CurrentSession
        => currentSession ?? throw new InvalidOperationException("Avalonia bridge session has not been initialized.");

    private sealed class AvaloniaSession
    {
        private readonly Dictionary<int, Action> clickHandlers = new();

        public int SmokeTextBlockHandle { get; set; }

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

    private sealed class AvaloniaEventSubscription(Action unsubscribe) : IDisposable
    {
        private Action? unsubscribe = unsubscribe;

        public void Dispose()
        {
            var action = Interlocked.Exchange(ref unsubscribe, null);
            action?.Invoke();
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