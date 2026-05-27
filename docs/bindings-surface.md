# Bindings surface inventory

This document enumerates every managed API requirement currently exposed by
`Rustlyn.Bindings.BindingSurface.CreateTinyBclSurface()`. It is the
contract that the bindings generator targets and the regression test
`BindingSurfaceMatchesDocumentedInventory` keeps in sync — if the live
surface grows or shrinks, the test will fail until this doc is updated.

The surface intentionally starts as a small, well-understood slice. Add
new requirements only when you also (a) update this doc, (b) update the
regression test if scoping changes, and (c) record the rationale in the
PR description.

## Inventory (62 requirements)

| # | Signature |
| -:| --- |
|  1 | `Rustlyn.Interop.ManagedCallbackBridge.InvokeI32(IntPtr, int)` |
|  2 | `Rustlyn.Interop.ManagedCallbackBridge.InvokeI32I32(IntPtr, int, int)` |
|  3 | `Rustlyn.Interop.ManagedCallbackBridge.InvokeObjectHandleTransform(IntPtr, int)` |
|  4 | `Rustlyn.Interop.ManagedInteropRuntime.CopyByteArray(byte[], IntPtr, long)` |
|  5 | `Rustlyn.Interop.ManagedInteropRuntime.CopyInt32Array(int[], IntPtr, long)` |
|  6 | `Rustlyn.Interop.ManagedInteropRuntime.CreateByteArray(int, int, int)` |
|  7 | `Rustlyn.Interop.ManagedInteropRuntime.CreateInt32Array(int, int, int)` |
|  8 | `Rustlyn.Interop.ManagedInteropRuntime.CreateStringArray(string, string, string)` |
|  9 | `System.Byte[]` |
| 10 | `System.Console.WriteLine(string)` |
| 11 | `System.DateTimeOffset` |
| 12 | `System.DateTimeOffset.FromUnixTimeMilliseconds(long)` |
| 13 | `System.DateTimeOffset.ToString()` |
| 14 | `System.DateTimeOffset.ToUnixTimeMilliseconds()` |
| 15 | `System.Environment.CurrentDirectory` |
| 16 | `System.Environment.GetCommandLineArgs()` |
| 17 | `System.Guid` |
| 18 | `System.Guid.Parse(string)` |
| 19 | `System.Guid.ToString()` |
| 20 | `System.IO.Directory.GetCurrentDirectory()` |
| 21 | `System.IO.Directory.GetFiles(string, string)` |
| 22 | `System.IO.File.ReadAllLines(string)` |
| 23 | `System.IO.Path.ChangeExtension(string, string)` |
| 24 | `System.IO.Path.Combine(string, string)` |
| 25 | `System.IO.Path.EndsInDirectorySeparator(string)` |
| 26 | `System.IO.Path.GetDirectoryName(string)` |
| 27 | `System.IO.Path.GetExtension(string)` |
| 28 | `System.IO.Path.GetFileName(string)` |
| 29 | `System.IO.Path.GetFileNameWithoutExtension(string)` |
| 30 | `System.IO.Path.GetFullPath(string)` |
| 31 | `System.IO.Path.GetPathRoot(string)` |
| 32 | `System.IO.Path.GetRelativePath(string, string)` |
| 33 | `System.IO.Path.GetTempPath()` |
| 34 | `System.IO.Path.HasExtension(string)` |
| 35 | `System.IO.Path.IsPathFullyQualified(string)` |
| 36 | `System.IO.Path.IsPathRooted(string)` |
| 37 | `System.Int32[]` |
| 38 | `System.Math.Abs(double)` |
| 39 | `System.Math.Sqrt(double)` |
| 40 | `System.MathF.Abs(float)` |
| 41 | `System.MathF.Cos(float)` |
| 42 | `System.MathF.Max(float, float)` |
| 43 | `System.MathF.Min(float, float)` |
| 44 | `System.MathF.Sin(float)` |
| 45 | `System.MathF.Sqrt(float)` |
| 46 | `System.String` |
| 47 | `System.String.Contains(string, StringComparison)` |
| 48 | `System.String.EndsWith(string, StringComparison)` |
| 49 | `System.String.IndexOf(string, StringComparison)` |
| 50 | `System.String.Length` |
| 51 | `System.String.Replace(string, string)` |
| 52 | `System.String.Split(string, StringSplitOptions)` |
| 53 | `System.String.StartsWith(string, StringComparison)` |
| 54 | `System.String.Substring(int, int)` |
| 55 | `System.StringComparison` |
| 56 | `System.StringSplitOptions` |
| 57 | `System.String[]` |
| 58 | `System.TimeSpan` |
| 59 | `System.TimeSpan.FromMilliseconds(double)` |
| 60 | `System.TimeSpan.Ticks` |
| 61 | `System.TimeSpan.ToString()` |
| 62 | `System.TimeSpan.TotalMilliseconds` |

## How to update this doc

1. Edit `dotnet/backend/src/Rustlyn.Bindings/BindingSurface.cs` —
   typically `CreateTinyBclSurface()`.
2. Run the regression test:
   `dotnet run -c Release --project ./dotnet/backend/tests/Rustlyn.Backend.Tests/Rustlyn.Backend.Tests.csproj -- BindingSurfaceMatchesDocumentedInventory`
   It will fail and report the exact additions/removals.
3. Sync the table here, re-run the test, then commit both changes
   together.
