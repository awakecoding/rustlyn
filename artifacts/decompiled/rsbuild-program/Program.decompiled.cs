using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using LLVMSharp.Interop;
using Microsoft.FSharp.Core;
using Mono.Cecil;
using llvm_stuff;

[CompilationMapping(SourceConstructFlags.Module)]
public static class Program
{
	[Serializable]
	internal sealed class a@58 : FSharpFunc<string, string>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal a@58()
		{
		}

		public override string Invoke(string s)
		{
			return s.Trim();
		}
	}

	[Serializable]
	internal sealed class a@58-1 : FSharpFunc<string, string>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal a@58-1()
		{
		}

		public override string Invoke(string s)
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, string>("\"%P()\"", new object[1] { s }, null));
		}
	}

	[Serializable]
	internal sealed class load_ref@74-1 : FSharpFunc<string, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_ref@74-1(FSharpFunc<string, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(string arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class load_ref@74 : FSharpFunc<string, FSharpFunc<string, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, FSharpFunc<string, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_ref@74(FSharpFunc<string, FSharpFunc<string, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<string, Unit> Invoke(string arg10)
		{
			FSharpFunc<string, Unit> clo = clo1.Invoke(arg10);
			return new load_ref@74-1(clo);
		}
	}

	[Serializable]
	internal sealed class cpref_dotnet@194-1 : FSharpFunc<string, string>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, string> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal cpref_dotnet@194-1(FSharpFunc<string, string> clo2)
		{
			this.clo2 = clo2;
		}

		public override string Invoke(string arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class cpref_dotnet@194 : FSharpFunc<string, FSharpFunc<string, string>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, FSharpFunc<string, string>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal cpref_dotnet@194(FSharpFunc<string, FSharpFunc<string, string>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<string, string> Invoke(string arg10)
		{
			FSharpFunc<string, string> clo = clo1.Invoke(arg10);
			return new cpref_dotnet@194-1(clo);
		}
	}

	[Serializable]
	internal sealed class ref_win32@210-1 : FSharpFunc<string, string>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, string> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal ref_win32@210-1(FSharpFunc<string, string> clo2)
		{
			this.clo2 = clo2;
		}

		public override string Invoke(string arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class ref_win32@210 : FSharpFunc<string, FSharpFunc<string, string>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, FSharpFunc<string, string>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal ref_win32@210(FSharpFunc<string, FSharpFunc<string, string>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<string, string> Invoke(string arg10)
		{
			FSharpFunc<string, string> clo = clo1.Invoke(arg10);
			return new ref_win32@210-1(clo);
		}
	}

	[Serializable]
	internal sealed class ref_m@215-1 : FSharpFunc<string, string>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, string> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal ref_m@215-1(FSharpFunc<string, string> clo2)
		{
			this.clo2 = clo2;
		}

		public override string Invoke(string arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class ref_m@215 : FSharpFunc<string, FSharpFunc<string, string>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, FSharpFunc<string, string>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal ref_m@215(FSharpFunc<string, FSharpFunc<string, string>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<string, string> Invoke(string arg10)
		{
			FSharpFunc<string, string> clo = clo1.Invoke(arg10);
			return new ref_m@215-1(clo);
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1, 1, 1, 1 })]
	public static void write_cargo_toml(string path_dest, string name, string version, string edition, string path_src, string path_dotnet, string items_RustCrateReference, string items_RustReference)
	{
		StreamWriter streamWriter = new StreamWriter(path_dest);
		try
		{
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("[package]"));
			FSharpFunc<string, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("name = \"%s\""));
			fSharpFunc.Invoke(name);
			FSharpFunc<string, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("version = \"%s\""));
			fSharpFunc2.Invoke(version);
			FSharpFunc<string, Unit> fSharpFunc3 = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("edition = \"%s\""));
			fSharpFunc3.Invoke(edition);
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("autobins = false"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("autoexamples = false"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("autotests = false"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("autobenches = false"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>(""));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("[lib]"));
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("crate-type = [\"cdylib\"]"));
			FSharpFunc<string, Unit> fSharpFunc4 = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("path = \"%s\""));
			fSharpFunc4.Invoke(path_src);
			ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>(""));
			if (!string.Equals(path_dotnet, null))
			{
				ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("[dependencies.dotnet]"));
				FSharpFunc<string, Unit> fSharpFunc5 = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("path = \"%s\""));
				fSharpFunc5.Invoke(path_dotnet);
				ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>(""));
			}
			string[] array = items_RustReference.Split(';');
			foreach (string text in array)
			{
				string text2 = text.Trim();
				if (text2.Length > 0)
				{
					string[] array2 = text2.Split('=');
					string text3 = array2[0].Replace('\\', '/');
					string text4 = array2[1];
					FSharpFunc<string, Unit> fSharpFunc6 = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("[dependencies.%s]"));
					string func = text4;
					fSharpFunc6.Invoke(func);
					FSharpFunc<string, Unit> fSharpFunc7 = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("path = \"%s\""));
					string func2 = text3;
					fSharpFunc7.Invoke(func2);
					ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>(""));
				}
			}
			string[] array3 = items_RustCrateReference.Split(';');
			foreach (string text5 in array3)
			{
				string text6 = text5.Trim();
				if (text6.Length <= 0)
				{
					continue;
				}
				string[] array4 = text6.Split('=');
				string text7 = array4[0];
				string text8 = array4[1];
				FSharpFunc<string, Unit> fSharpFunc8 = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("[dependencies.%s]"));
				string func3 = text7;
				fSharpFunc8.Invoke(func3);
				FSharpFunc<string, Unit> fSharpFunc9 = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("version = \"%s\""));
				string func4 = text8;
				fSharpFunc9.Invoke(func4);
				if (array4.Length > 2)
				{
					string text9 = array4[2];
					if (text9.Trim().Length > 0)
					{
						FSharpFunc<string, Unit> fSharpFunc10 = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("default-features = %s"));
						string func5 = text9;
						fSharpFunc10.Invoke(func5);
					}
				}
				if (array4.Length > 3)
				{
					string text10 = array4[3];
					if (text10.Trim().Length > 0)
					{
						string[] array5 = text10.Split(',');
						FSharpFunc<string, string> fSharpFunc11 = new a@58();
						string[] array6 = array5;
						if (array6 == null)
						{
							throw new ArgumentNullException("array");
						}
						string[] array7 = new string[array6.Length];
						for (int k = 0; k < array7.Length; k++)
						{
							array7[k] = fSharpFunc11.Invoke(array6[k]);
						}
						string[] array8 = array7;
						FSharpFunc<string, string> fSharpFunc12 = new a@58-1();
						string[] array9 = array8;
						if (array9 == null)
						{
							throw new ArgumentNullException("array");
						}
						string[] array10 = new string[array9.Length];
						for (int l = 0; l < array10.Length; l++)
						{
							array10[l] = fSharpFunc12.Invoke(array9[l]);
						}
						string[] value = array10;
						string text11 = string.Join(",", value);
						FSharpFunc<string, Unit> fSharpFunc13 = ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("features = [%s]"));
						string func6 = text11;
						fSharpFunc13.Invoke(func6);
					}
				}
				ExtraTopLevelOperators.PrintFormatLineToTextWriter(streamWriter, new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>(""));
			}
			Unit unit = null;
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

	internal static TypeDefinition load_ref(string r)
	{
		string[] array = r.Split(',');
		if (array.Length != 2)
		{
			FSharpFunc<string, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, Unit>, Unit, string, Unit, string>("invalid reference (assembly+classname): %s"));
			fSharpFunc.Invoke(r);
		}
		string text = array[0];
		string text2 = array[1];
		string fullPath = Path.GetFullPath(text);
		ModuleDefinition moduleDefinition = ModuleDefinition.ReadModule(fullPath);
		TypeDefinition type = moduleDefinition.GetType(text2);
		if (LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(type, null))
		{
			FSharpFunc<string, FSharpFunc<string, Unit>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, FSharpFunc<string, Unit>>, Unit, string, Unit, Tuple<string, string>>("type %s not found in %s"));
			FSharpFunc<string, string>.InvokeFast(new load_ref@74(clo), text2, text);
		}
		return type;
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 })]
	public static void write_cil(string path_bc, string assembly_name, string class_name, string str_version, string ref_dotnet, string ref_rt, string ref_win32, string ref_m, string outputdir, cecil.OutputTypeSetting output_type)
	{
		LLVMMemoryBufferRef mb = sgllvm.read_file(path_bc);
		LLVMModuleRef m = sgllvm.parse_bitcode(mb);
		object obj;
		if (!string.Equals(ref_dotnet, null))
		{
			TypeDefinition typeDefinition = load_ref(ref_dotnet);
			TypeDefinition value = typeDefinition;
			obj = FSharpOption<TypeDefinition>.Some(value);
		}
		else
		{
			obj = null;
		}
		FSharpOption<TypeDefinition> cpref = (FSharpOption<TypeDefinition>)obj;
		TypeDefinition typeDefinition2 = load_ref(ref_rt);
		TypeDefinition typeDefinition3 = load_ref(ref_win32);
		TypeDefinition typeDefinition4 = load_ref(ref_m);
		MemoryStream memoryStream = new MemoryStream();
		byte[] array;
		try
		{
			Version ver = new Version(str_version);
			TypeDefinition typeDefinition5 = typeDefinition2;
			TypeDefinition item = typeDefinition5;
			cecil.RtSetting ref_rt2 = cecil.RtSetting.NewCopy(item);
			cecil.CompilerSettings settings = new cecil.CompilerSettings(new TypeDefinition[2] { typeDefinition3, typeDefinition4 }, cpref, ref_rt2, output_type);
			cecil.gen_assembly(settings, m, assembly_name, class_name, ver, memoryStream);
			array = memoryStream.ToArray();
		}
		finally
		{
			if (memoryStream is IDisposable disposable)
			{
				disposable.Dispose();
				_ = null;
			}
			else
			{
				_ = null;
			}
		}
		byte[] bytes = array;
		string path = assembly_name + ".dll";
		string path2 = Path.Combine(outputdir, path);
		File.WriteAllBytes(path2, bytes);
	}

	[EntryPoint]
	public static int main(string[] argv)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		string text = argv[0];
		string text2 = argv[1];
		string path = argv[2];
		string path_rsfakelink = argv[3];
		string class_name = argv[4];
		string path2 = argv[5];
		string outputdir = argv[6];
		string text3 = argv[7];
		string text4 = argv[8];
		string edition = argv[9];
		string text5 = argv[10];
		string text6 = argv[11];
		string text7 = argv[12];
		string text8 = argv[13];
		FSharpFunc<string, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("RustCrateReference: %s"));
		string func = text7;
		fSharpFunc.Invoke(func);
		FSharpFunc<string, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("RustReference: %s"));
		string func2 = text8;
		fSharpFunc2.Invoke(func2);
		string path3 = Path.GetFullPath(path).Replace("\\", "/");
		string fullPath = Path.GetFullPath(path2);
		DirectoryInfo directoryInfo = Directory.CreateDirectory(fullPath);
		DirectoryInfo directoryInfo2 = directoryInfo;
		build_sysroot.write_custom_target(fullPath, path_rsfakelink);
		cecil.OutputTypeSetting outputTypeSetting = ((!string.Equals(text3.ToLower().Trim(), "exe")) ? cecil.OutputTypeSetting.Library : cecil.OutputTypeSetting.Exe);
		string text9 = text.Replace(".", "_");
		string path_src = ((outputTypeSetting.Tag == 0) ? "../../src/lib.rs" : "../../src/main.rs");
		string text10 = text5.ToLower().Trim();
		if (!string.Equals(text10, "debug") && !string.Equals(text10, "release"))
		{
			FSharpFunc<string, Unit> fSharpFunc3 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, Unit>, Unit, string, Unit, string>("config must be debug or release: %s"));
			string func3 = text10;
			fSharpFunc3.Invoke(func3);
		}
		string text11 = text10;
		string text12 = Path.Combine(fullPath, "sr");
		DirectoryInfo directoryInfo3 = Directory.CreateDirectory(text12);
		DirectoryInfo directoryInfo4 = directoryInfo3;
		string text13 = Path.Combine(fullPath, "sysroot");
		Stopwatch stopwatch2 = Stopwatch.StartNew();
		build_sysroot.build_std_sysroot_with_cargo(text12, text4, build_sysroot.custom_target_base_name, text13, fullPath);
		stopwatch2.Stop();
		string text14 = Path.Combine(fullPath, "rs");
		DirectoryInfo directoryInfo5 = Directory.CreateDirectory(text14);
		DirectoryInfo directoryInfo6 = directoryInfo5;
		string text15 = Path.Combine(path3, "rs_dotnet", "dotnet").Replace("\\", "/");
		string path_dotnet = ((!string.Equals(text6.ToLower().Trim(), "true")) ? null : text15);
		string text16 = Path.Combine(text14, "Cargo.toml");
		write_cargo_toml(text16, text9, text2, edition, path_src, path_dotnet, text7, text8);
		object obj;
		if (string.Equals(text6.ToLower().Trim(), "true"))
		{
			string arg = Path.Combine(text15, "dotnet.dll");
			string arg2 = "__glue";
			FSharpFunc<string, FSharpFunc<string, string>> clo = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, FSharpFunc<string, string>>, Unit, string, string, Tuple<string, string>>("%s,%s"));
			obj = FSharpFunc<string, string>.InvokeFast(new cpref_dotnet@194(clo), arg, arg2);
		}
		else
		{
			obj = null;
		}
		string ref_dotnet = (string)obj;
		FSharpFunc<string, string> fSharpFunc4 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("%s,sgrt"));
		string location = typeof(sgrt).Assembly.Location;
		string ref_rt = fSharpFunc4.Invoke(location);
		string arg3 = Path.Combine(path3, "sgwin32.dll");
		string arg4 = "sgwin32";
		FSharpFunc<string, FSharpFunc<string, string>> clo2 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, FSharpFunc<string, string>>, Unit, string, string, Tuple<string, string>>("%s,%s"));
		string ref_win = FSharpFunc<string, string>.InvokeFast(new ref_win32@210(clo2), arg3, arg4);
		string arg5 = Path.Combine(path3, "sgm.dll");
		string arg6 = "sgm";
		FSharpFunc<string, FSharpFunc<string, string>> clo3 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, FSharpFunc<string, string>>, Unit, string, string, Tuple<string, string>>("%s,%s"));
		string ref_m = FSharpFunc<string, string>.InvokeFast(new ref_m@215(clo3), arg5, arg6);
		string path_bc = Path.Combine(text14, "target", build_sysroot.custom_target_base_name, text11, text9 + ".bc");
		Stopwatch stopwatch3 = Stopwatch.StartNew();
		List<string> list = new List<string>();
		list.Add(text4);
		list.Add("-vv");
		list.Add("build");
		list.Add("--verbose");
		if (string.Equals(text5, "release"))
		{
			list.Add("--release");
		}
		list.Add("--manifest-path");
		list.Add(text16);
		list.Add("--target");
		FSharpFunc<string, string> fSharpFunc5 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("./%s.json"));
		string custom_target_base_name = build_sysroot.custom_target_base_name;
		list.Add(fSharpFunc5.Invoke(custom_target_base_name));
		string args = string.Join(" ", list);
		FSharpFunc<string, string> fSharpFunc6 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("--sysroot %s"));
		string fullPath2 = Path.GetFullPath(text13);
		string env_v = fSharpFunc6.Invoke(fullPath2);
		exec.exec_env("cargo", args, "RUSTFLAGS", env_v, fullPath);
		stopwatch3.Stop();
		Stopwatch stopwatch4 = Stopwatch.StartNew();
		write_cil(path_bc, text, class_name, text2, ref_dotnet, ref_rt, ref_win, ref_m, outputdir, outputTypeSetting);
		stopwatch4.Stop();
		stopwatch.Stop();
		FSharpFunc<long, Unit> fSharpFunc7 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<long, Unit>, TextWriter, Unit, Unit, long>("Build sysroot time: %A milliseconds"));
		long elapsedMilliseconds = stopwatch2.ElapsedMilliseconds;
		fSharpFunc7.Invoke(elapsedMilliseconds);
		FSharpFunc<long, Unit> fSharpFunc8 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<long, Unit>, TextWriter, Unit, Unit, long>("Build rust project time: %A milliseconds"));
		long elapsedMilliseconds2 = stopwatch3.ElapsedMilliseconds;
		fSharpFunc8.Invoke(elapsedMilliseconds2);
		FSharpFunc<long, Unit> fSharpFunc9 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<long, Unit>, TextWriter, Unit, Unit, long>("Build cil time: %A milliseconds"));
		long elapsedMilliseconds3 = stopwatch4.ElapsedMilliseconds;
		fSharpFunc9.Invoke(elapsedMilliseconds3);
		FSharpFunc<long, Unit> fSharpFunc10 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<long, Unit>, TextWriter, Unit, Unit, long>("Overall build time: %A milliseconds"));
		long elapsedMilliseconds4 = stopwatch.ElapsedMilliseconds;
		fSharpFunc10.Invoke(elapsedMilliseconds4);
		return 0;
	}
}
