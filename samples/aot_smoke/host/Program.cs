// SPDX-License-Identifier: MIT
//
using System;

internal static class Program
{
    public static int Main()
    {
#if AOT_SMOKE_TRANSLATED
        var sum = Rustlyn.GeneratedModule.aot_add(19, 23);
        var m = Rustlyn.GeneratedModule.aot_meaning();
        Console.WriteLine($"aot_smoke: sum={sum} meaning={m}");
        return sum == 42 && m == 42 ? 0 : 5;
#else
        Console.Error.WriteLine("AotSmoke.Translated.dll not found at build time. Run scripts/Test-AotSmoke.ps1.");
        return 2;
#endif
    }
}
