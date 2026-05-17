using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.FSharp.Core;

[CompilationMapping(SourceConstructFlags.Module)]
public static class build_sysroot
{
	public static string custom_target_base_name
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return "aarch64-sourcegear-windows";
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1 })]
	public static void build_std_sysroot_with_cargo(string path_bld_sysroot, string toolchain, string custom_target_name, string sysroot_dest, string wd)
	{
		DirectoryInfo directoryInfo = Directory.CreateDirectory(path_bld_sysroot);
		DirectoryInfo directoryInfo2 = directoryInfo;
		FSharpFunc<string, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("%s --print sysroot"));
		Tuple<string, string> tuple = exec.exec_capture("rustc", fSharpFunc.Invoke(toolchain), wd);
		string item = tuple.Item1;
		StringBuilder stringBuilder = new StringBuilder();
		string path = Path.Combine(item, "lib", "rustlib", "src", "rust", "library");
		string func = Path.Combine(path, "std").Replace("\\", "/");
		string func2 = Path.Combine(path, "rustc-std-workspace-alloc").Replace("\\", "/");
		string func3 = Path.Combine(path, "rustc-std-workspace-core").Replace("\\", "/");
		string func4 = Path.Combine(path, "rustc-std-workspace-std").Replace("\\", "/");
		StringBuilder stringBuilder2 = stringBuilder.AppendLine("[package]");
		StringBuilder stringBuilder3 = stringBuilder2;
		StringBuilder stringBuilder4 = stringBuilder.AppendLine("authors = [\"The Rust Project Developers\"]");
		StringBuilder stringBuilder5 = stringBuilder4;
		StringBuilder stringBuilder6 = stringBuilder.AppendLine("name = \"sysroot\"");
		StringBuilder stringBuilder7 = stringBuilder6;
		StringBuilder stringBuilder8 = stringBuilder.AppendLine("version = \"0.0.0\"");
		StringBuilder stringBuilder9 = stringBuilder8;
		StringBuilder stringBuilder10 = stringBuilder.AppendLine("[dependencies.std]");
		StringBuilder stringBuilder11 = stringBuilder10;
		StringBuilder stringBuilder12 = stringBuilder.AppendLine(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("path = \"%s\"")).Invoke(func));
		StringBuilder stringBuilder13 = stringBuilder12;
		StringBuilder stringBuilder14 = stringBuilder.AppendLine("[patch.crates-io.rustc-std-workspace-alloc]");
		StringBuilder stringBuilder15 = stringBuilder14;
		StringBuilder stringBuilder16 = stringBuilder.AppendLine(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("path = \"%s\"")).Invoke(func2));
		StringBuilder stringBuilder17 = stringBuilder16;
		StringBuilder stringBuilder18 = stringBuilder.AppendLine("[patch.crates-io.rustc-std-workspace-core]");
		StringBuilder stringBuilder19 = stringBuilder18;
		StringBuilder stringBuilder20 = stringBuilder.AppendLine(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("path = \"%s\"")).Invoke(func3));
		StringBuilder stringBuilder21 = stringBuilder20;
		StringBuilder stringBuilder22 = stringBuilder.AppendLine("[patch.crates-io.rustc-std-workspace-std]");
		StringBuilder stringBuilder23 = stringBuilder22;
		StringBuilder stringBuilder24 = stringBuilder.AppendLine(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("path = \"%s\"")).Invoke(func4));
		StringBuilder stringBuilder25 = stringBuilder24;
		StringBuilder stringBuilder26 = stringBuilder.AppendLine("[dependencies.compiler_builtins]");
		StringBuilder stringBuilder27 = stringBuilder26;
		StringBuilder stringBuilder28 = stringBuilder.AppendLine("features = ['mem']");
		StringBuilder stringBuilder29 = stringBuilder28;
		string path2 = Path.Combine(path_bld_sysroot, "Cargo.toml");
		File.WriteAllText(path2, stringBuilder.ToString());
		string item2 = Path.GetRelativePath(wd, path2).Replace("\\", "/");
		DirectoryInfo directoryInfo3 = Directory.CreateDirectory(Path.Combine(path_bld_sysroot, "src"));
		DirectoryInfo directoryInfo4 = directoryInfo3;
		File.WriteAllText(Path.Combine(path_bld_sysroot, "src", "lib.rs"), "");
		FSharpFunc<string, string> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("--sysroot \"%s\""));
		Environment.SetEnvironmentVariable("RUSTFLAGS", fSharpFunc2.Invoke(sysroot_dest));
		List<string> list = new List<string>();
		list.Add(toolchain);
		list.Add("build");
		list.Add("--release");
		list.Add("--manifest-path");
		list.Add(item2);
		list.Add("--target");
		FSharpFunc<string, string> fSharpFunc3 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("./%s.json"));
		list.Add(fSharpFunc3.Invoke(custom_target_name));
		list.Add("--verbose");
		list.Add("-p");
		list.Add("std");
		string args = string.Join(" ", list);
		exec.exec("cargo", args, wd);
		string text = Path.Combine(sysroot_dest, "lib", "rustlib", custom_target_name, "lib");
		DirectoryInfo directoryInfo5 = Directory.CreateDirectory(text);
		DirectoryInfo directoryInfo6 = directoryInfo5;
		string path3 = Path.Combine(path_bld_sysroot, "target", custom_target_name, "release", "deps");
		string[] files = Directory.GetFiles(path3);
		foreach (string text2 in files)
		{
			string fileName = Path.GetFileName(text2);
			string destFileName = Path.Combine(text, fileName);
			File.Copy(text2, destFileName, overwrite: true);
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static void write_custom_target(string dir, string path_rsfakelink)
	{
		string path = custom_target_base_name + ".json";
		string path2 = Path.Combine(dir, path);
		StreamWriter streamWriter = new StreamWriter(path2);
		try
		{
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("{"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"arch\": \"aarch64\","));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"data-layout\": \"e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128\","));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"dynamic-linking\": true,"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"dll-prefix\" : \"\","));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"dll-suffix\" : \".bc\","));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"only-cdylib\": true,"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"no-builtins\": true,"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"allow_asm\": false,"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"requires-lto\": true,"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"executables\": false,"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"is-builtin\": false,"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"linker\": \"dotnet\","));
			FSharpFunc<string, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("\"pre-link-args\": { \"gcc\" : [ \"%s\" ] },"));
			fSharpFunc.Invoke(path_rsfakelink);
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"linker-flavor\": \"gcc\","));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"linker-is-gnu\": false,"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"llvm-target\": \"aarch64-pc-windows-msvc\","));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"max-atomic-width\": 64,"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"obj-is-bitcode\": true,"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"os\": \"windows\","));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"panic-strategy\" : \"abort\","));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"target-c-int-width\": \"32\","));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"target-endian\": \"little\","));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"target-family\": \"windows\","));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"target-pointer-width\": \"64\","));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("\"vendor\": \"sourcegear\""));
			Unit unit = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("}"));
		}
		finally
		{
			if (streamWriter is IDisposable disposable)
			{
				disposable.Dispose();
				_ = null;
			}
			else
			{
				_ = null;
			}
		}
	}
}
