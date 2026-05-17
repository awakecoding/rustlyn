using System.Collections.Generic;
using System.IO;
using Microsoft.FSharp.Core;

[CompilationMapping(SourceConstructFlags.Module)]
public static class Program
{
	[EntryPoint]
	public static int main(string[] args)
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		string text2 = default(string);
		for (int i = 0; i < args.Length; i++)
		{
			string text = args[i];
			if (string.Equals(text, "-L"))
			{
				i++;
			}
			else if (string.Equals(text, "-o"))
			{
				i++;
				text2 = args[i];
			}
			else if (text.EndsWith(".o"))
			{
				list2.Add(text);
			}
			else if (text.EndsWith(".rlib"))
			{
				list.Add(text);
			}
			else if (!text.StartsWith("-Wl,") && !string.Equals(text, "-shared") && !string.Equals(text, "-nodefaultlibs") && !text.StartsWith("-l") && !string.Equals(text, "-lgcc_s") && !string.Equals(text, "-lutil") && !string.Equals(text, "-lrt") && !string.Equals(text, "-lc"))
			{
				FSharpFunc<string, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, Unit>, Unit, string, Unit, string>("TODO: %s"));
				string func = text;
				fSharpFunc.Invoke(func);
			}
		}
		if (string.Equals(text2, null))
		{
			string message = "no dest";
			throw Operators.Failure(message);
		}
		if (list.Count == 0 && list2.Count == 1)
		{
			string sourceFileName = list2[0];
			File.Copy(sourceFileName, text2, overwrite: true);
			return 0;
		}
		string message2 = "TODO";
		throw Operators.Failure(message2);
	}
}
