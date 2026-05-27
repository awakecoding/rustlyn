// SPDX-License-Identifier: MIT
//
// AOT-friendly host that invokes the translated Rust library via reflection.
// Reflection is used (instead of a project reference) so that the host
// compiles even when the translated assembly is not present, which keeps
// the IDE/edit experience usable.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

internal static class Program
{
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "AOT smoke loads a known assembly we control.")]
    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "AOT smoke loads a known method by name.")]
    public static int Main()
    {
        var probe = Path.Combine(AppContext.BaseDirectory, "AotSmoke.Translated.dll");
        if (!File.Exists(probe))
        {
            Console.Error.WriteLine("AotSmoke.Translated.dll not found alongside host. Run scripts/Test-AotSmoke.ps1.");
            return 2;
        }

        var asm = Assembly.LoadFrom(probe);
        var module = asm.GetType("RustlynModule") ?? asm.GetType("aot_smoke") ?? FindModuleType(asm);
        if (module is null)
        {
            Console.Error.WriteLine("Translated module type not found in AotSmoke.Translated.dll.");
            return 3;
        }

        var add = module.GetMethod("aot_add", BindingFlags.Public | BindingFlags.Static);
        var meaning = module.GetMethod("aot_meaning", BindingFlags.Public | BindingFlags.Static);
        if (add is null || meaning is null)
        {
            Console.Error.WriteLine("Translated module missing aot_add or aot_meaning.");
            return 4;
        }

        var sum = (int)add.Invoke(null, new object[] { 19, 23 })!;
        var m = (int)meaning.Invoke(null, null)!;
        Console.WriteLine($"aot_smoke: sum={sum} meaning={m}");
        return sum == 42 && m == 42 ? 0 : 5;
    }

    private static Type? FindModuleType(Assembly asm)
    {
        foreach (var t in asm.GetTypes())
        {
            if (t.GetMethod("aot_add", BindingFlags.Public | BindingFlags.Static) is not null)
            {
                return t;
            }
        }
        return null;
    }
}
