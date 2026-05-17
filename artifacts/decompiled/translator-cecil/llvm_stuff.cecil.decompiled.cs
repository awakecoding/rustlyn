using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using <StartupCode$llvm2cil>.$llvm_stuff;
using LLVMSharp.Interop;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace llvm_stuff;

[CompilationMapping(SourceConstructFlags.Module)]
public static class cecil
{
	[Serializable]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[DebuggerDisplay("{__DebugDisplay(),nq}")]
	[CompilationMapping(SourceConstructFlags.SumType)]
	public abstract class InstructionValue : IEquatable<InstructionValue>, IStructuralEquatable
	{
		public static class Tags
		{
			public const int Local = 0;

			public const int Temp = 1;

			public const int Immed = 2;
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Local@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Local : InstructionValue
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Variable item;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Local(cil.Variable item)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Temp@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Temp : InstructionValue
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Variable item;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Temp(cil.Variable item)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Immed@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Immed : InstructionValue
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly LLVMValueRef item;

			[CompilationMapping(SourceConstructFlags.Field, 2, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public LLVMValueRef Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Immed(LLVMValueRef item)
			{
				this.item = item;
			}
		}

		[SpecialName]
		internal class Local@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Local _obj;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Local@DebugTypeProxy(Local obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Temp@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Temp _obj;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Temp@DebugTypeProxy(Temp obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Immed@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Immed _obj;

			[CompilationMapping(SourceConstructFlags.Field, 2, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public LLVMValueRef Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Immed@DebugTypeProxy(Immed obj)
			{
				_obj = obj;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Tag
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return (this is Immed) ? 2 : ((this is Temp) ? 1 : 0);
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLocal
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is Local;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsTemp
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is Temp;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsImmed
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is Immed;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal InstructionValue()
		{
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
		public static InstructionValue NewLocal(cil.Variable item)
		{
			return new Local(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
		public static InstructionValue NewTemp(cil.Variable item)
		{
			return new Temp(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 2)]
		public static InstructionValue NewImmed(LLVMValueRef item)
		{
			return new Immed(item);
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<InstructionValue, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<InstructionValue, string>, Unit, string, string, InstructionValue>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public virtual sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				if (!(this is Local))
				{
					if (this is Temp)
					{
						Temp temp = (Temp)this;
						num = 1;
						return -1640531527 + (temp.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
					}
					if (this is Immed)
					{
						Immed immed = (Immed)this;
						num = 2;
						return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, immed.item) + ((num << 6) + (num >> 2)));
					}
				}
				Local local = (Local)this;
				num = 0;
				return -1640531527 + (local.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is InstructionValue instructionValue)
				{
					InstructionValue instructionValue2 = instructionValue;
					int num = ((this is Immed) ? 2 : ((this is Temp) ? 1 : 0));
					InstructionValue instructionValue3 = instructionValue2;
					int num2 = ((instructionValue3 is Immed) ? 2 : ((instructionValue3 is Temp) ? 1 : 0));
					if (num == num2)
					{
						if (!(this is Local))
						{
							if (this is Temp)
							{
								Temp temp = (Temp)this;
								Temp temp2 = (Temp)instructionValue2;
								cil.Variable variable = temp.item;
								cil.Variable obj2 = temp2.item;
								return variable.Equals(obj2, comp);
							}
							if (this is Immed)
							{
								Immed immed = (Immed)this;
								Immed immed2 = (Immed)instructionValue2;
								return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, immed.item, immed2.item);
							}
						}
						Local local = (Local)this;
						Local local2 = (Local)instructionValue2;
						cil.Variable variable2 = local.item;
						cil.Variable obj3 = local2.item;
						return variable2.Equals(obj3, comp);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(InstructionValue obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = ((this is Immed) ? 2 : ((this is Temp) ? 1 : 0));
					int num2 = ((obj is Immed) ? 2 : ((obj is Temp) ? 1 : 0));
					if (num == num2)
					{
						if (!(this is Local))
						{
							if (this is Temp)
							{
								Temp temp = (Temp)this;
								Temp temp2 = (Temp)obj;
								return temp.item.Equals(temp2.item);
							}
							if (this is Immed)
							{
								Immed immed = (Immed)this;
								Immed immed2 = (Immed)obj;
								return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(immed.item, immed2.item);
							}
						}
						Local local = (Local)this;
						Local local2 = (Local)obj;
						return local.item.Equals(local2.item);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is InstructionValue obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[DebuggerDisplay("{__DebugDisplay(),nq}")]
	[CompilationMapping(SourceConstructFlags.SumType)]
	public abstract class InstructionDest : IEquatable<InstructionDest>, IStructuralEquatable
	{
		public static class Tags
		{
			public const int InTemp = 0;

			public const int OnStack = 1;

			public const int Void = 2;
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(InTemp@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class InTemp : InstructionDest
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Variable item;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal InTemp(cil.Variable item)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(_OnStack@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		internal class _OnStack : InstructionDest
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal _OnStack()
			{
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(_Void@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		internal class _Void : InstructionDest
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal _Void()
			{
			}
		}

		[SpecialName]
		internal class InTemp@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal InTemp _obj;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public InTemp@DebugTypeProxy(InTemp obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class _OnStack@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal _OnStack _obj;

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public _OnStack@DebugTypeProxy(_OnStack obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class _Void@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal _Void _obj;

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public _Void@DebugTypeProxy(_Void obj)
			{
				_obj = obj;
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly InstructionDest _unique_OnStack = new _OnStack();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly InstructionDest _unique_Void = new _Void();

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Tag
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return (this is _Void) ? 2 : ((this is _OnStack) ? 1 : 0);
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsInTemp
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is InTemp;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static InstructionDest OnStack
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
			get
			{
				return _unique_OnStack;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsOnStack
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is _OnStack;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static InstructionDest Void
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 2)]
			get
			{
				return _unique_Void;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsVoid
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is _Void;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal InstructionDest()
		{
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
		public static InstructionDest NewInTemp(cil.Variable item)
		{
			return new InTemp(item);
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<InstructionDest, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<InstructionDest, string>, Unit, string, string, InstructionDest>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public virtual sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				if (this is InTemp)
				{
					InTemp inTemp = (InTemp)this;
					num = 0;
					return -1640531527 + (inTemp.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				return (this is _Void) ? 2 : ((this is _OnStack) ? 1 : 0);
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is InstructionDest instructionDest)
				{
					InstructionDest instructionDest2 = instructionDest;
					int num = ((this is _Void) ? 2 : ((this is _OnStack) ? 1 : 0));
					InstructionDest instructionDest3 = instructionDest2;
					int num2 = ((instructionDest3 is _Void) ? 2 : ((instructionDest3 is _OnStack) ? 1 : 0));
					if (num == num2)
					{
						if (this is InTemp)
						{
							InTemp inTemp = (InTemp)this;
							InTemp inTemp2 = (InTemp)instructionDest2;
							cil.Variable variable = inTemp.item;
							cil.Variable obj2 = inTemp2.item;
							return variable.Equals(obj2, comp);
						}
						return true;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(InstructionDest obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = ((this is _Void) ? 2 : ((this is _OnStack) ? 1 : 0));
					int num2 = ((obj is _Void) ? 2 : ((obj is _OnStack) ? 1 : 0));
					if (num == num2)
					{
						if (this is InTemp)
						{
							InTemp inTemp = (InTemp)this;
							InTemp inTemp2 = (InTemp)obj;
							return inTemp.item.Equals(inTemp2.item);
						}
						return true;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is InstructionDest obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[DebuggerDisplay("{__DebugDisplay(),nq}")]
	[CompilationMapping(SourceConstructFlags.SumType)]
	public abstract class ResultLocal : IEquatable<ResultLocal>, IStructuralEquatable
	{
		public static class Tags
		{
			public const int Dest = 0;

			public const int TempResult = 1;
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Dest@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Dest : ResultLocal
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Variable item;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Dest(cil.Variable item)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(TempResult@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class TempResult : ResultLocal
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Variable item;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal TempResult(cil.Variable item)
			{
				this.item = item;
			}
		}

		[SpecialName]
		internal class Dest@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Dest _obj;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Dest@DebugTypeProxy(Dest obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class TempResult@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal TempResult _obj;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TempResult@DebugTypeProxy(TempResult obj)
			{
				_obj = obj;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Tag
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return (this is TempResult) ? 1 : 0;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsDest
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is Dest;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsTempResult
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is TempResult;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal ResultLocal()
		{
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
		public static ResultLocal NewDest(cil.Variable item)
		{
			return new Dest(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
		public static ResultLocal NewTempResult(cil.Variable item)
		{
			return new TempResult(item);
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<ResultLocal, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<ResultLocal, string>, Unit, string, string, ResultLocal>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public virtual sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				if (this is Dest)
				{
					Dest dest = (Dest)this;
					num = 0;
					return -1640531527 + (dest.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				TempResult tempResult = (TempResult)this;
				num = 1;
				return -1640531527 + (tempResult.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is ResultLocal resultLocal)
				{
					ResultLocal resultLocal2 = resultLocal;
					int num = ((this is TempResult) ? 1 : 0);
					ResultLocal resultLocal3 = resultLocal2;
					int num2 = ((resultLocal3 is TempResult) ? 1 : 0);
					if (num == num2)
					{
						if (this is Dest)
						{
							Dest dest = (Dest)this;
							Dest dest2 = (Dest)resultLocal2;
							cil.Variable variable = dest.item;
							cil.Variable obj2 = dest2.item;
							return variable.Equals(obj2, comp);
						}
						TempResult tempResult = (TempResult)this;
						TempResult tempResult2 = (TempResult)resultLocal2;
						cil.Variable variable2 = tempResult.item;
						cil.Variable obj3 = tempResult2.item;
						return variable2.Equals(obj3, comp);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(ResultLocal obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = ((this is TempResult) ? 1 : 0);
					int num2 = ((obj is TempResult) ? 1 : 0);
					if (num == num2)
					{
						if (this is Dest)
						{
							Dest dest = (Dest)this;
							Dest dest2 = (Dest)obj;
							return dest.item.Equals(dest2.item);
						}
						TempResult tempResult = (TempResult)this;
						TempResult tempResult2 = (TempResult)obj;
						return tempResult.item.Equals(tempResult2.item);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is ResultLocal obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[DebuggerDisplay("{__DebugDisplay(),nq}")]
	[CompilationMapping(SourceConstructFlags.SumType)]
	public abstract class BlockInstructionValue : IEquatable<BlockInstructionValue>, IStructuralEquatable
	{
		public static class Tags
		{
			public const int Direct = 0;

			public const int ForwardTo = 1;
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Direct@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Direct : BlockInstructionValue
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly InstructionValue item;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public InstructionValue Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Direct(InstructionValue item)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(ForwardTo@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class ForwardTo : BlockInstructionValue
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly LLVMValueRef item;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public LLVMValueRef Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal ForwardTo(LLVMValueRef item)
			{
				this.item = item;
			}
		}

		[SpecialName]
		internal class Direct@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Direct _obj;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public InstructionValue Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Direct@DebugTypeProxy(Direct obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class ForwardTo@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal ForwardTo _obj;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public LLVMValueRef Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public ForwardTo@DebugTypeProxy(ForwardTo obj)
			{
				_obj = obj;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Tag
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return (this is ForwardTo) ? 1 : 0;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsDirect
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is Direct;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsForwardTo
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is ForwardTo;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal BlockInstructionValue()
		{
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
		public static BlockInstructionValue NewDirect(InstructionValue item)
		{
			return new Direct(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
		public static BlockInstructionValue NewForwardTo(LLVMValueRef item)
		{
			return new ForwardTo(item);
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<BlockInstructionValue, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<BlockInstructionValue, string>, Unit, string, string, BlockInstructionValue>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public virtual sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				if (this is Direct)
				{
					Direct direct = (Direct)this;
					num = 0;
					return -1640531527 + (direct.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				ForwardTo forwardTo = (ForwardTo)this;
				num = 1;
				return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, forwardTo.item) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is BlockInstructionValue blockInstructionValue)
				{
					BlockInstructionValue blockInstructionValue2 = blockInstructionValue;
					int num = ((this is ForwardTo) ? 1 : 0);
					BlockInstructionValue blockInstructionValue3 = blockInstructionValue2;
					int num2 = ((blockInstructionValue3 is ForwardTo) ? 1 : 0);
					if (num == num2)
					{
						if (this is Direct)
						{
							Direct direct = (Direct)this;
							Direct direct2 = (Direct)blockInstructionValue2;
							InstructionValue instructionValue = direct.item;
							InstructionValue obj2 = direct2.item;
							return instructionValue.Equals(obj2, comp);
						}
						ForwardTo forwardTo = (ForwardTo)this;
						ForwardTo forwardTo2 = (ForwardTo)blockInstructionValue2;
						return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, forwardTo.item, forwardTo2.item);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(BlockInstructionValue obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = ((this is ForwardTo) ? 1 : 0);
					int num2 = ((obj is ForwardTo) ? 1 : 0);
					if (num == num2)
					{
						if (this is Direct)
						{
							Direct direct = (Direct)this;
							Direct direct2 = (Direct)obj;
							return direct.item.Equals(direct2.item);
						}
						ForwardTo forwardTo = (ForwardTo)this;
						ForwardTo forwardTo2 = (ForwardTo)obj;
						return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(forwardTo.item, forwardTo2.item);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is BlockInstructionValue obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class GenSyms
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal MethodReference ctor_exception@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<LLVMValueRef, FieldReference> d_globals@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<LLVMValueRef, MethodReference> d_methods@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal FSharpFunc<string, MethodReference> f_get_rt@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public MethodReference ctor_exception => ctor_exception@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public Dictionary<LLVMValueRef, FieldReference> d_globals => d_globals@;

		[CompilationMapping(SourceConstructFlags.Field, 2)]
		public Dictionary<LLVMValueRef, MethodReference> d_methods => d_methods@;

		[CompilationMapping(SourceConstructFlags.Field, 3)]
		public FSharpFunc<string, MethodReference> f_get_rt => f_get_rt@;

		public GenSyms(MethodReference ctor_exception, Dictionary<LLVMValueRef, FieldReference> d_globals, Dictionary<LLVMValueRef, MethodReference> d_methods, FSharpFunc<string, MethodReference> f_get_rt)
		{
			ctor_exception@ = ctor_exception;
			d_globals@ = d_globals;
			d_methods@ = d_methods;
			f_get_rt@ = f_get_rt;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<GenSyms, string>, Unit, string, string, GenSyms>("%+A")).Invoke(this);
		}
	}

	[Serializable]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[DebuggerDisplay("{__DebugDisplay(),nq}")]
	[CompilationMapping(SourceConstructFlags.SumType)]
	public sealed class OutputTypeSetting : IEquatable<OutputTypeSetting>, IStructuralEquatable, IComparable<OutputTypeSetting>, IComparable, IStructuralComparable
	{
		public static class Tags
		{
			public const int Library = 0;

			public const int Exe = 1;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly OutputTypeSetting _unique_Library = new OutputTypeSetting(0);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly OutputTypeSetting _unique_Exe = new OutputTypeSetting(1);

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[field: DebuggerNonUserCode]
		public int Tag
		{
			[DebuggerNonUserCode]
			get;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static OutputTypeSetting Library
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
			get
			{
				return _unique_Library;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLibrary
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 0;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static OutputTypeSetting Exe
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
			get
			{
				return _unique_Exe;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsExe
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 1;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal OutputTypeSetting(int _tag)
		{
			this._tag = _tag;
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<OutputTypeSetting, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<OutputTypeSetting, string>, Unit, string, string, OutputTypeSetting>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(OutputTypeSetting obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = _tag;
					int num2 = obj._tag;
					if (num == num2)
					{
						return 0;
					}
					return num - num2;
				}
				return 1;
			}
			if (obj != null)
			{
				return -1;
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj)
		{
			return CompareTo((OutputTypeSetting)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			OutputTypeSetting outputTypeSetting = (OutputTypeSetting)obj;
			if (this != null)
			{
				if ((OutputTypeSetting)obj != null)
				{
					int num = _tag;
					int num2 = outputTypeSetting._tag;
					if (num == num2)
					{
						return 0;
					}
					return num - num2;
				}
				return 1;
			}
			if ((OutputTypeSetting)obj != null)
			{
				return -1;
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				return _tag;
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is OutputTypeSetting outputTypeSetting)
				{
					OutputTypeSetting outputTypeSetting2 = outputTypeSetting;
					int num = _tag;
					int num2 = outputTypeSetting2._tag;
					return num == num2;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(OutputTypeSetting obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = _tag;
					int num2 = obj._tag;
					return num == num2;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is OutputTypeSetting obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[DebuggerDisplay("{__DebugDisplay(),nq}")]
	[CompilationMapping(SourceConstructFlags.SumType)]
	public abstract class RtSetting : IEquatable<RtSetting>, IStructuralEquatable
	{
		public static class Tags
		{
			public const int Reference = 0;

			public const int Copy = 1;
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Reference@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Reference : RtSetting
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly TypeDefinition item;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TypeDefinition Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Reference(TypeDefinition item)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Copy@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Copy : RtSetting
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly TypeDefinition item;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TypeDefinition Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Copy(TypeDefinition item)
			{
				this.item = item;
			}
		}

		[SpecialName]
		internal class Reference@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Reference _obj;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TypeDefinition Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Reference@DebugTypeProxy(Reference obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Copy@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Copy _obj;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TypeDefinition Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Copy@DebugTypeProxy(Copy obj)
			{
				_obj = obj;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Tag
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return (this is Copy) ? 1 : 0;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsReference
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is Reference;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsCopy
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is Copy;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal RtSetting()
		{
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
		public static RtSetting NewReference(TypeDefinition item)
		{
			return new Reference(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
		public static RtSetting NewCopy(TypeDefinition item)
		{
			return new Copy(item);
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<RtSetting, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<RtSetting, string>, Unit, string, string, RtSetting>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public virtual sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				if (this is Reference)
				{
					Reference reference = (Reference)this;
					num = 0;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, reference.item) + ((num << 6) + (num >> 2)));
				}
				Copy copy = (Copy)this;
				num = 1;
				return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, copy.item) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is RtSetting rtSetting)
				{
					RtSetting rtSetting2 = rtSetting;
					int num = ((this is Copy) ? 1 : 0);
					RtSetting rtSetting3 = rtSetting2;
					int num2 = ((rtSetting3 is Copy) ? 1 : 0);
					if (num == num2)
					{
						if (this is Reference)
						{
							Reference reference = (Reference)this;
							Reference reference2 = (Reference)rtSetting2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, reference.item, reference2.item);
						}
						Copy copy = (Copy)this;
						Copy copy2 = (Copy)rtSetting2;
						return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, copy.item, copy2.item);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(RtSetting obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = ((this is Copy) ? 1 : 0);
					int num2 = ((obj is Copy) ? 1 : 0);
					if (num == num2)
					{
						if (this is Reference)
						{
							Reference reference = (Reference)this;
							Reference reference2 = (Reference)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(reference.item, reference2.item);
						}
						Copy copy = (Copy)this;
						Copy copy2 = (Copy)obj;
						return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(copy.item, copy2.item);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is RtSetting obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class CompilerSettings : IEquatable<CompilerSettings>, IStructuralEquatable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal TypeDefinition[] references@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal FSharpOption<TypeDefinition> cpref@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal RtSetting ref_rt@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal OutputTypeSetting output_type@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public TypeDefinition[] references => references@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public FSharpOption<TypeDefinition> cpref => cpref@;

		[CompilationMapping(SourceConstructFlags.Field, 2)]
		public RtSetting ref_rt => ref_rt@;

		[CompilationMapping(SourceConstructFlags.Field, 3)]
		public OutputTypeSetting output_type => output_type@;

		public CompilerSettings(TypeDefinition[] references, FSharpOption<TypeDefinition> cpref, RtSetting ref_rt, OutputTypeSetting output_type)
		{
			references@ = references;
			cpref@ = cpref;
			ref_rt@ = ref_rt;
			output_type@ = output_type;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<CompilerSettings, string>, Unit, string, string, CompilerSettings>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				num = -1640531527 + (output_type@.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (ref_rt@.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, cpref@) + ((num << 6) + (num >> 2)));
				return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, references@) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is CompilerSettings compilerSettings)
				{
					CompilerSettings compilerSettings2 = compilerSettings;
					if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, references@, compilerSettings2.references@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, cpref@, compilerSettings2.cpref@))
						{
							RtSetting rtSetting = ref_rt@;
							RtSetting obj2 = compilerSettings2.ref_rt@;
							if (rtSetting.Equals(obj2, comp))
							{
								OutputTypeSetting outputTypeSetting = output_type@;
								OutputTypeSetting obj3 = compilerSettings2.output_type@;
								return outputTypeSetting.Equals(obj3, comp);
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(CompilerSettings obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(references@, obj.references@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(cpref@, obj.cpref@))
						{
							if (ref_rt@.Equals(obj.ref_rt@))
							{
								return output_type@.Equals(obj.output_type@);
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is CompilerSettings obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class MethodRefInternal : IEquatable<MethodRefInternal>, IStructuralEquatable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal LLVMValueRef func@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal MethodDefinition method@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<LLVMValueRef, ParameterDefinition> args@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal ParameterDefinition extra@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public LLVMValueRef func => func@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public MethodDefinition method => method@;

		[CompilationMapping(SourceConstructFlags.Field, 2)]
		public Dictionary<LLVMValueRef, ParameterDefinition> args => args@;

		[CompilationMapping(SourceConstructFlags.Field, 3)]
		public ParameterDefinition extra => extra@;

		public MethodRefInternal(LLVMValueRef func, MethodDefinition method, Dictionary<LLVMValueRef, ParameterDefinition> args, ParameterDefinition extra)
		{
			func@ = func;
			method@ = method;
			args@ = args;
			extra@ = extra;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<MethodRefInternal, string>, Unit, string, string, MethodRefInternal>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, extra@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, args@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, method@) + ((num << 6) + (num >> 2)));
				return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, func@) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is MethodRefInternal methodRefInternal)
				{
					MethodRefInternal methodRefInternal2 = methodRefInternal;
					if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, func@, methodRefInternal2.func@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, method@, methodRefInternal2.method@))
						{
							if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, args@, methodRefInternal2.args@))
							{
								return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, extra@, methodRefInternal2.extra@);
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(MethodRefInternal obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(func@, obj.func@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(method@, obj.method@))
						{
							if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(args@, obj.args@))
							{
								return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(extra@, obj.extra@);
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is MethodRefInternal obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class DataStuff : IEquatable<DataStuff>, IStructuralEquatable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal byte[] data@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal string name@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal EmbeddedResource resource@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public byte[] data => data@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public string name => name@;

		[CompilationMapping(SourceConstructFlags.Field, 2)]
		public EmbeddedResource resource => resource@;

		public DataStuff(byte[] data, string name, EmbeddedResource resource)
		{
			data@ = data;
			name@ = name;
			resource@ = resource;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<DataStuff, string>, Unit, string, string, DataStuff>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, resource@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + ((name@?.GetHashCode() ?? 0) + ((num << 6) + (num >> 2)));
				return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, data@) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is DataStuff dataStuff)
				{
					DataStuff dataStuff2 = dataStuff;
					if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, data@, dataStuff2.data@))
					{
						if (string.Equals(name@, dataStuff2.name@))
						{
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, resource@, dataStuff2.resource@);
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(DataStuff obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(data@, obj.data@))
					{
						if (string.Equals(name@, obj.name@))
						{
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(resource@, obj.resource@);
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is DataStuff obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[DebuggerDisplay("{__DebugDisplay(),nq}")]
	[CompilationMapping(SourceConstructFlags.SumType)]
	public abstract class Block : IEquatable<Block>, IStructuralEquatable
	{
		public static class Tags
		{
			public const int Regular = 0;

			public const int Phi = 1;
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Regular@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Regular : Block
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Label item;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Label Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Regular(cil.Label item)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Phi@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Phi : Block
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Dictionary<LLVMBasicBlockRef, cil.Label> item;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Dictionary<LLVMBasicBlockRef, cil.Label> Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Phi(Dictionary<LLVMBasicBlockRef, cil.Label> item)
			{
				this.item = item;
			}
		}

		[SpecialName]
		internal class Regular@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Regular _obj;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Label Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Regular@DebugTypeProxy(Regular obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Phi@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Phi _obj;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Dictionary<LLVMBasicBlockRef, cil.Label> Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Phi@DebugTypeProxy(Phi obj)
			{
				_obj = obj;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Tag
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return (this is Phi) ? 1 : 0;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsRegular
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is Regular;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsPhi
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is Phi;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal Block()
		{
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
		public static Block NewRegular(cil.Label item)
		{
			return new Regular(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
		public static Block NewPhi(Dictionary<LLVMBasicBlockRef, cil.Label> item)
		{
			return new Phi(item);
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<Block, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<Block, string>, Unit, string, string, Block>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public virtual sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				if (this is Regular)
				{
					Regular regular = (Regular)this;
					num = 0;
					return -1640531527 + (regular.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				Phi phi = (Phi)this;
				num = 1;
				return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, phi.item) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is Block block)
				{
					Block block2 = block;
					int num = ((this is Phi) ? 1 : 0);
					Block block3 = block2;
					int num2 = ((block3 is Phi) ? 1 : 0);
					if (num == num2)
					{
						if (this is Regular)
						{
							Regular regular = (Regular)this;
							Regular regular2 = (Regular)block2;
							cil.Label label = regular.item;
							cil.Label obj2 = regular2.item;
							return label.Equals(obj2, comp);
						}
						Phi phi = (Phi)this;
						Phi phi2 = (Phi)block2;
						return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, phi.item, phi2.item);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(Block obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = ((this is Phi) ? 1 : 0);
					int num2 = ((obj is Phi) ? 1 : 0);
					if (num == num2)
					{
						if (this is Regular)
						{
							Regular regular = (Regular)this;
							Regular regular2 = (Regular)obj;
							return regular.item.Equals(regular2.item);
						}
						Phi phi = (Phi)this;
						Phi phi2 = (Phi)obj;
						return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(phi.item, phi2.item);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is Block obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class PhiBlockInfo : IEquatable<PhiBlockInfo>, IStructuralEquatable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal LLVMValueRef[] phis@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<LLVMBasicBlockRef, Dictionary<LLVMValueRef, LLVMValueRef>> incoming_phi_val@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<LLVMValueRef, Dictionary<LLVMBasicBlockRef, LLVMValueRef>> phi_incoming_val@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public LLVMValueRef[] phis => phis@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public Dictionary<LLVMBasicBlockRef, Dictionary<LLVMValueRef, LLVMValueRef>> incoming_phi_val => incoming_phi_val@;

		[CompilationMapping(SourceConstructFlags.Field, 2)]
		public Dictionary<LLVMValueRef, Dictionary<LLVMBasicBlockRef, LLVMValueRef>> phi_incoming_val => phi_incoming_val@;

		public PhiBlockInfo(LLVMValueRef[] phis, Dictionary<LLVMBasicBlockRef, Dictionary<LLVMValueRef, LLVMValueRef>> incoming_phi_val, Dictionary<LLVMValueRef, Dictionary<LLVMBasicBlockRef, LLVMValueRef>> phi_incoming_val)
		{
			phis@ = phis;
			incoming_phi_val@ = incoming_phi_val;
			phi_incoming_val@ = phi_incoming_val;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<PhiBlockInfo, string>, Unit, string, string, PhiBlockInfo>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, phi_incoming_val@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, incoming_phi_val@) + ((num << 6) + (num >> 2)));
				return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, phis@) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is PhiBlockInfo phiBlockInfo)
				{
					PhiBlockInfo phiBlockInfo2 = phiBlockInfo;
					if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, phis@, phiBlockInfo2.phis@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, incoming_phi_val@, phiBlockInfo2.incoming_phi_val@))
						{
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, phi_incoming_val@, phiBlockInfo2.phi_incoming_val@);
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(PhiBlockInfo obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(phis@, obj.phis@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(incoming_phi_val@, obj.incoming_phi_val@))
						{
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(phi_incoming_val@, obj.phi_incoming_val@);
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is PhiBlockInfo obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	internal sealed class get_integer_constant@222-2 : FSharpFunc<string, long>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, long> clo3;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_integer_constant@222-2(FSharpFunc<string, long> clo3)
		{
			this.clo3 = clo3;
		}

		public override long Invoke(string arg30)
		{
			return clo3.Invoke(arg30);
		}
	}

	[Serializable]
	internal sealed class get_integer_constant@222-1 : FSharpFunc<LLVMValueRef, FSharpFunc<string, long>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueRef, FSharpFunc<string, long>> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_integer_constant@222-1(FSharpFunc<LLVMValueRef, FSharpFunc<string, long>> clo2)
		{
			this.clo2 = clo2;
		}

		public override FSharpFunc<string, long> Invoke(LLVMValueRef arg20)
		{
			FSharpFunc<string, long> clo = clo2.Invoke(arg20);
			return new get_integer_constant@222-2(clo);
		}
	}

	[Serializable]
	internal sealed class get_integer_constant@222 : FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, FSharpFunc<string, long>>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, FSharpFunc<string, long>>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_integer_constant@222(FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, FSharpFunc<string, long>>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<LLVMValueRef, FSharpFunc<string, long>> Invoke(LLVMValueKind arg10)
		{
			FSharpFunc<LLVMValueRef, FSharpFunc<string, long>> clo = clo1.Invoke(arg10);
			return new get_integer_constant@222-1(clo);
		}
	}

	[Serializable]
	internal sealed class do_vec_into_local@340 : FSharpFunc<cil.CilWriter, Unit>
	{
		public cil.Variable result;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal do_vec_into_local@340(cil.Variable result)
		{
			this.result = result;
		}

		public override Unit Invoke(cil.CilWriter il)
		{
			il.Append(cil.MyInstruction.NewLdloca(result));
			return null;
		}
	}

	[Serializable]
	internal sealed class do_ptrvec_into_local@355 : FSharpFunc<cil.CilWriter, Unit>
	{
		public cil.Variable result;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal do_ptrvec_into_local@355(cil.Variable result)
		{
			this.result = result;
		}

		public override Unit Invoke(cil.CilWriter il)
		{
			il.Append(cil.MyInstruction.NewLdloca(result));
			return null;
		}
	}

	[Serializable]
	internal sealed class load_value_constant_ptr@399-1 : FSharpFunc<LLVMValueRef, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueRef, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_value_constant_ptr@399-1(FSharpFunc<LLVMValueRef, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(LLVMValueRef arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class load_value_constant_ptr@399 : FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_value_constant_ptr@399(FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<LLVMValueRef, Unit> Invoke(string arg10)
		{
			FSharpFunc<LLVMValueRef, Unit> clo = clo1.Invoke(arg10);
			return new load_value_constant_ptr@399-1(clo);
		}
	}

	[Serializable]
	internal sealed class indexes@424 : FSharpFunc<LLVMValueRef, int>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal indexes@424()
		{
		}

		public override int Invoke(LLVMValueRef v)
		{
			return (int)get_integer_constant(v);
		}
	}

	[Serializable]
	internal sealed class cpblk_into@490-1 : FSharpFunc<int, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal cpblk_into@490-1(FSharpFunc<int, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(int arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class cpblk_into@490 : FSharpFunc<int, FSharpFunc<int, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, FSharpFunc<int, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal cpblk_into@490(FSharpFunc<int, FSharpFunc<int, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<int, Unit> Invoke(int arg10)
		{
			FSharpFunc<int, Unit> clo = clo1.Invoke(arg10);
			return new cpblk_into@490-1(clo);
		}
	}

	[Serializable]
	internal sealed class f_load_src@505 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public cil.Variable from;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_load_src@505(cil.CilWriter il, cil.Variable from)
		{
			this.il = il;
			this.from = from;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			il.Append(cil.MyInstruction.NewLdloc(from));
			return null;
		}
	}

	[Serializable]
	internal sealed class f_load_src@509-1 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public cil.Variable from;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_load_src@509-1(cil.CilWriter il, cil.Variable from)
		{
			this.il = il;
			this.from = from;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			il.Append(cil.MyInstruction.NewLdloca(from));
			return null;
		}
	}

	[Serializable]
	internal sealed class cpblk_into_from_ldloca@516-1 : FSharpFunc<int, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal cpblk_into_from_ldloca@516-1(FSharpFunc<int, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(int arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class cpblk_into_from_ldloca@516 : FSharpFunc<int, FSharpFunc<int, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, FSharpFunc<int, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal cpblk_into_from_ldloca@516(FSharpFunc<int, FSharpFunc<int, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<int, Unit> Invoke(int arg10)
		{
			FSharpFunc<int, Unit> clo = clo1.Invoke(arg10);
			return new cpblk_into_from_ldloca@516-1(clo);
		}
	}

	[Serializable]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[DebuggerDisplay("{__DebugDisplay(),nq}")]
	[CompilationMapping(SourceConstructFlags.SumType)]
	public sealed class TraceInstruction : IEquatable<TraceInstruction>, IStructuralEquatable, IComparable<TraceInstruction>, IComparable, IStructuralComparable
	{
		public static class Tags
		{
			public const int Yes = 0;

			public const int No = 1;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly TraceInstruction _unique_Yes = new TraceInstruction(0);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly TraceInstruction _unique_No = new TraceInstruction(1);

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[field: DebuggerNonUserCode]
		public int Tag
		{
			[DebuggerNonUserCode]
			get;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static TraceInstruction Yes
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
			get
			{
				return _unique_Yes;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsYes
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 0;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static TraceInstruction No
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
			get
			{
				return _unique_No;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsNo
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 1;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal TraceInstruction(int _tag)
		{
			this._tag = _tag;
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<TraceInstruction, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<TraceInstruction, string>, Unit, string, string, TraceInstruction>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(TraceInstruction obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = _tag;
					int num2 = obj._tag;
					if (num == num2)
					{
						return 0;
					}
					return num - num2;
				}
				return 1;
			}
			if (obj != null)
			{
				return -1;
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj)
		{
			return CompareTo((TraceInstruction)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			TraceInstruction traceInstruction = (TraceInstruction)obj;
			if (this != null)
			{
				if ((TraceInstruction)obj != null)
				{
					int num = _tag;
					int num2 = traceInstruction._tag;
					if (num == num2)
					{
						return 0;
					}
					return num - num2;
				}
				return 1;
			}
			if ((TraceInstruction)obj != null)
			{
				return -1;
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				return _tag;
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is TraceInstruction traceInstruction)
				{
					TraceInstruction traceInstruction2 = traceInstruction;
					int num = _tag;
					int num2 = traceInstruction2._tag;
					return num == num2;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(TraceInstruction obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = _tag;
					int num2 = obj._tag;
					return num == num2;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is TraceInstruction obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	internal sealed class outer_load_arg@640-1 : FSharpFunc<LLVMValueRef, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueRef, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal outer_load_arg@640-1(FSharpFunc<LLVMValueRef, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(LLVMValueRef arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class outer_load_arg@640 : FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal outer_load_arg@640(FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<LLVMValueRef, Unit> Invoke(LLVMValueKind arg10)
		{
			FSharpFunc<LLVMValueRef, Unit> clo = clo1.Invoke(arg10);
			return new outer_load_arg@640-1(clo);
		}
	}

	[Serializable]
	internal sealed class store_constant_at@698-1 : FSharpFunc<LLVMValueRef, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueRef, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal store_constant_at@698-1(FSharpFunc<LLVMValueRef, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(LLVMValueRef arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class store_constant_at@698 : FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal store_constant_at@698(FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<LLVMValueRef, Unit> Invoke(string arg10)
		{
			FSharpFunc<LLVMValueRef, Unit> clo = clo1.Invoke(arg10);
			return new store_constant_at@698-1(clo);
		}
	}

	[Serializable]
	internal sealed class sub_get_addr@708 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<Unit, Unit> f_get_addr;

		public cil.StructType styp;

		public int i;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal sub_get_addr@708(cil.CilWriter il, FSharpFunc<Unit, Unit> f_get_addr, cil.StructType styp, int i)
		{
			this.il = il;
			this.f_get_addr = f_get_addr;
			this.styp = styp;
			this.i = i;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			f_get_addr.Invoke(null);
			il.Append(cil.MyInstruction.NewLdc_I4((int)styp.items[i].off));
			il.Append(cil.MyInstruction.Add);
			return null;
		}
	}

	[Serializable]
	internal sealed class my_get_addr@744 : FSharpTypeFunc
	{
		public FSharpFunc<Unit, Unit> f_get_addr;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_get_addr@744(FSharpFunc<Unit, Unit> f_get_addr)
		{
			this.f_get_addr = f_get_addr;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new my_get_addr@744T<a>(f_get_addr, this);
		}
	}

	[Serializable]
	internal sealed class my_get_addr@744T<a> : FSharpFunc<a, Unit>
	{
		public FSharpFunc<Unit, Unit> f_get_addr;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public my_get_addr@744 self1@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_get_addr@744T(FSharpFunc<Unit, Unit> f_get_addr, my_get_addr@744 self1@)
		{
			this.f_get_addr = f_get_addr;
			this.self1@ = self1@;
		}

		public override Unit Invoke(a _arg1)
		{
			my_get_addr@744 my_get_addr@745 = self1@;
			return my_get_addr@745.f_get_addr.Invoke(null);
		}
	}

	[Serializable]
	internal sealed class my_get_nth@745 : FSharpTypeFunc
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest;

		public LLVMValueRef v;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_get_nth@745(GenSyms syms, cil.CilWriter il, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, LLVMValueRef v)
		{
			this.syms = syms;
			this.il = il;
			this.f_load_instr_value_to_dest = f_load_instr_value_to_dest;
			this.v = v;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new my_get_nth@745T<a>(syms, il, f_load_instr_value_to_dest, v, this);
		}
	}

	[Serializable]
	internal sealed class my_get_nth@745T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest;

		public LLVMValueRef v;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public my_get_nth@745 self4@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_get_nth@745T(GenSyms syms, cil.CilWriter il, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, LLVMValueRef v, my_get_nth@745 self4@)
		{
			this.syms = syms;
			this.il = il;
			this.f_load_instr_value_to_dest = f_load_instr_value_to_dest;
			this.v = v;
			this.self4@ = self4@;
		}

		public override Unit Invoke(a _arg2, int i)
		{
			my_get_nth@745 my_get_nth@746 = self4@;
			LLVMValueRef operand = my_get_nth@746.v.GetOperand((uint)i);
			outer_load_constant_value(my_get_nth@746.syms, my_get_nth@746.il, my_get_nth@746.f_load_instr_value_to_dest, operand);
			return null;
		}
	}

	[Serializable]
	internal sealed class my_get_addr@756-1 : FSharpTypeFunc
	{
		public FSharpFunc<Unit, Unit> f_get_addr;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_get_addr@756-1(FSharpFunc<Unit, Unit> f_get_addr)
		{
			this.f_get_addr = f_get_addr;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new my_get_addr@756-1T<a>(f_get_addr, this);
		}
	}

	[Serializable]
	internal sealed class my_get_addr@756-1T<a> : FSharpFunc<a, Unit>
	{
		public FSharpFunc<Unit, Unit> f_get_addr;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public my_get_addr@756-1 self1@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_get_addr@756-1T(FSharpFunc<Unit, Unit> f_get_addr, my_get_addr@756-1 self1@)
		{
			this.f_get_addr = f_get_addr;
			this.self1@ = self1@;
		}

		public override Unit Invoke(a _arg3)
		{
			my_get_addr@756-1 my_get_addr@756-2 = self1@;
			return my_get_addr@756-2.f_get_addr.Invoke(null);
		}
	}

	[Serializable]
	internal sealed class my_get_nth@757-1 : FSharpTypeFunc
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest;

		public LLVMValueRef v;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_get_nth@757-1(GenSyms syms, cil.CilWriter il, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, LLVMValueRef v)
		{
			this.syms = syms;
			this.il = il;
			this.f_load_instr_value_to_dest = f_load_instr_value_to_dest;
			this.v = v;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new my_get_nth@757-1T<a>(syms, il, f_load_instr_value_to_dest, v, this);
		}
	}

	[Serializable]
	internal sealed class my_get_nth@757-1T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest;

		public LLVMValueRef v;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public my_get_nth@757-1 self4@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_get_nth@757-1T(GenSyms syms, cil.CilWriter il, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, LLVMValueRef v, my_get_nth@757-1 self4@)
		{
			this.syms = syms;
			this.il = il;
			this.f_load_instr_value_to_dest = f_load_instr_value_to_dest;
			this.v = v;
			this.self4@ = self4@;
		}

		public override Unit Invoke(a _arg4, int i)
		{
			my_get_nth@757-1 my_get_nth@757-2 = self4@;
			LLVMValueRef operand = my_get_nth@757-2.v.GetOperand((uint)i);
			outer_load_constant_value(my_get_nth@757-2.syms, my_get_nth@757-2.il, my_get_nth@757-2.f_load_instr_value_to_dest, operand);
			return null;
		}
	}

	[Serializable]
	internal sealed class dest_offset@777 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		public int elem_siz;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal dest_offset@777(cil.CilWriter il, int elem_siz)
		{
			this.il = il;
			this.elem_siz = elem_siz;
		}

		public override Unit Invoke(int n)
		{
			if (n > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(n));
				il.Append(cil.MyInstruction.NewLdc_I4(elem_siz));
				il.Append(cil.MyInstruction.Mul);
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class f_dest_addr@786 : FSharpFunc<Unit, Unit>
	{
		public FSharpFunc<Unit, Unit> f_get_addr;

		public FSharpFunc<int, Unit> dest_offset;

		public int i;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_dest_addr@786(FSharpFunc<Unit, Unit> f_get_addr, FSharpFunc<int, Unit> dest_offset, int i)
		{
			this.f_get_addr = f_get_addr;
			this.dest_offset = dest_offset;
			this.i = i;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			f_get_addr.Invoke(null);
			return dest_offset.Invoke(i);
		}
	}

	[Serializable]
	internal sealed class my_get_addr@800-2 : FSharpTypeFunc
	{
		public FSharpFunc<Unit, Unit> f_get_addr;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_get_addr@800-2(FSharpFunc<Unit, Unit> f_get_addr)
		{
			this.f_get_addr = f_get_addr;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new my_get_addr@800-2T<a>(f_get_addr, this);
		}
	}

	[Serializable]
	internal sealed class my_get_addr@800-2T<a> : FSharpFunc<a, Unit>
	{
		public FSharpFunc<Unit, Unit> f_get_addr;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public my_get_addr@800-2 self1@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_get_addr@800-2T(FSharpFunc<Unit, Unit> f_get_addr, my_get_addr@800-2 self1@)
		{
			this.f_get_addr = f_get_addr;
			this.self1@ = self1@;
		}

		public override Unit Invoke(a _arg5)
		{
			my_get_addr@800-2 my_get_addr@800-3 = self1@;
			return my_get_addr@800-3.f_get_addr.Invoke(null);
		}
	}

	[Serializable]
	internal sealed class my_get_nth@801-2 : FSharpTypeFunc
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest;

		public LLVMValueRef v;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_get_nth@801-2(GenSyms syms, cil.CilWriter il, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, LLVMValueRef v)
		{
			this.syms = syms;
			this.il = il;
			this.f_load_instr_value_to_dest = f_load_instr_value_to_dest;
			this.v = v;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new my_get_nth@801-2T<a>(syms, il, f_load_instr_value_to_dest, v, this);
		}
	}

	[Serializable]
	internal sealed class my_get_nth@801-2T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest;

		public LLVMValueRef v;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public my_get_nth@801-2 self4@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_get_nth@801-2T(GenSyms syms, cil.CilWriter il, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, LLVMValueRef v, my_get_nth@801-2 self4@)
		{
			this.syms = syms;
			this.il = il;
			this.f_load_instr_value_to_dest = f_load_instr_value_to_dest;
			this.v = v;
			this.self4@ = self4@;
		}

		public override Unit Invoke(a _arg6, int i)
		{
			my_get_nth@801-2 my_get_nth@801-3 = self4@;
			LLVMValueRef elementAsConstant = my_get_nth@801-3.v.GetElementAsConstant((uint)i);
			outer_load_constant_value(my_get_nth@801-3.syms, my_get_nth@801-3.il, my_get_nth@801-3.f_load_instr_value_to_dest, elementAsConstant);
			return null;
		}
	}

	[Serializable]
	internal sealed class sub_get_addr@818-1 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<Unit, Unit> f_get_addr;

		public cil.FirstClassType elemtype;

		public int i;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal sub_get_addr@818-1(cil.CilWriter il, FSharpFunc<Unit, Unit> f_get_addr, cil.FirstClassType elemtype, int i)
		{
			this.il = il;
			this.f_get_addr = f_get_addr;
			this.elemtype = elemtype;
			this.i = i;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			f_get_addr.Invoke(null);
			int num = cil.get_sizeof(elemtype) * i;
			if (num != 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(num));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class sub_get_addr@837-2 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<Unit, Unit> f_get_addr;

		public cil.FirstClassType elemtype;

		public int i;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal sub_get_addr@837-2(cil.CilWriter il, FSharpFunc<Unit, Unit> f_get_addr, cil.FirstClassType elemtype, int i)
		{
			this.il = il;
			this.f_get_addr = f_get_addr;
			this.elemtype = elemtype;
			this.i = i;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			f_get_addr.Invoke(null);
			int num = cil.get_sizeof(elemtype) * i;
			if (num != 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(num));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class store_constant_at@847-3 : FSharpFunc<LLVMValueRef, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueRef, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal store_constant_at@847-3(FSharpFunc<LLVMValueRef, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(LLVMValueRef arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class store_constant_at@847-2 : FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal store_constant_at@847-2(FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<LLVMValueRef, Unit> Invoke(LLVMValueKind arg10)
		{
			FSharpFunc<LLVMValueRef, Unit> clo = clo1.Invoke(arg10);
			return new store_constant_at@847-3(clo);
		}
	}

	[Serializable]
	internal sealed class outer_load_constant_value@909 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public cil.Variable tmp;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal outer_load_constant_value@909(cil.CilWriter il, cil.Variable tmp)
		{
			this.il = il;
			this.tmp = tmp;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			il.Append(cil.MyInstruction.NewLdloca(tmp));
			return null;
		}
	}

	[Serializable]
	internal sealed class outer_load_constant_value@950-1 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public cil.Variable tmp_strange;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal outer_load_constant_value@950-1(cil.CilWriter il, cil.Variable tmp_strange)
		{
			this.il = il;
			this.tmp_strange = tmp_strange;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			il.Append(cil.MyInstruction.NewLdloca(tmp_strange));
			return null;
		}
	}

	[Serializable]
	internal sealed class outer_load_constant_value@1019-3 : FSharpFunc<LLVMValueRef, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueRef, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal outer_load_constant_value@1019-3(FSharpFunc<LLVMValueRef, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(LLVMValueRef arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class outer_load_constant_value@1019-2 : FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal outer_load_constant_value@1019-2(FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<LLVMValueRef, Unit> Invoke(LLVMValueKind arg10)
		{
			FSharpFunc<LLVMValueRef, Unit> clo = clo1.Invoke(arg10);
			return new outer_load_constant_value@1019-3(clo);
		}
	}

	[Serializable]
	internal sealed class outer_load_value@1055-1 : FSharpFunc<LLVMValueRef, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueRef, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal outer_load_value@1055-1(FSharpFunc<LLVMValueRef, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(LLVMValueRef arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class outer_load_value@1055 : FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal outer_load_value@1055(FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<LLVMValueRef, Unit> Invoke(LLVMValueKind arg10)
		{
			FSharpFunc<LLVMValueRef, Unit> clo = clo1.Invoke(arg10);
			return new outer_load_value@1055-1(clo);
		}
	}

	[Serializable]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[DebuggerDisplay("{__DebugDisplay(),nq}")]
	[CompilationMapping((SourceConstructFlags)33)]
	internal abstract class AddressOfValue : IEquatable<AddressOfValue>, IStructuralEquatable
	{
		internal static class Tags
		{
			public const int Arg = 0;

			public const int AlreadyVar = 1;

			public const int TempVar = 2;
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Arg@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		internal class Arg : AddressOfValue
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly ParameterDefinition item;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal ParameterDefinition Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Arg(ParameterDefinition item)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(AlreadyVar@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		internal class AlreadyVar : AddressOfValue
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Variable item;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal AlreadyVar(cil.Variable item)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(TempVar@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		internal class TempVar : AddressOfValue
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Variable item;

			[CompilationMapping(SourceConstructFlags.Field, 2, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal TempVar(cil.Variable item)
			{
				this.item = item;
			}
		}

		[SpecialName]
		internal class Arg@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Arg _obj;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public ParameterDefinition Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Arg@DebugTypeProxy(Arg obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class AlreadyVar@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal AlreadyVar _obj;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public AlreadyVar@DebugTypeProxy(AlreadyVar obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class TempVar@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal TempVar _obj;

			[CompilationMapping(SourceConstructFlags.Field, 2, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.Variable Item
			{
				[CompilerGenerated]
				[DebuggerNonUserCode]
				get
				{
					return _obj.item;
				}
			}

			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TempVar@DebugTypeProxy(TempVar obj)
			{
				_obj = obj;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal int Tag
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return (this is TempVar) ? 2 : ((this is AlreadyVar) ? 1 : 0);
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal bool IsArg
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is Arg;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal bool IsAlreadyVar
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is AlreadyVar;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal bool IsTempVar
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is TempVar;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal AddressOfValue()
		{
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
		internal static AddressOfValue NewArg(ParameterDefinition item)
		{
			return new Arg(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
		internal static AddressOfValue NewAlreadyVar(cil.Variable item)
		{
			return new AlreadyVar(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 2)]
		internal static AddressOfValue NewTempVar(cil.Variable item)
		{
			return new TempVar(item);
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<AddressOfValue, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<AddressOfValue, string>, Unit, string, string, AddressOfValue>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public virtual sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				if (!(this is Arg))
				{
					if (this is AlreadyVar)
					{
						AlreadyVar alreadyVar = (AlreadyVar)this;
						num = 1;
						return -1640531527 + (alreadyVar.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
					}
					if (this is TempVar)
					{
						TempVar tempVar = (TempVar)this;
						num = 2;
						return -1640531527 + (tempVar.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
					}
				}
				Arg arg = (Arg)this;
				num = 0;
				return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, arg.item) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is AddressOfValue addressOfValue)
				{
					AddressOfValue addressOfValue2 = addressOfValue;
					int num = ((this is TempVar) ? 2 : ((this is AlreadyVar) ? 1 : 0));
					AddressOfValue addressOfValue3 = addressOfValue2;
					int num2 = ((addressOfValue3 is TempVar) ? 2 : ((addressOfValue3 is AlreadyVar) ? 1 : 0));
					if (num == num2)
					{
						if (!(this is Arg))
						{
							if (this is AlreadyVar)
							{
								AlreadyVar alreadyVar = (AlreadyVar)this;
								AlreadyVar alreadyVar2 = (AlreadyVar)addressOfValue2;
								cil.Variable variable = alreadyVar.item;
								cil.Variable obj2 = alreadyVar2.item;
								return variable.Equals(obj2, comp);
							}
							if (this is TempVar)
							{
								TempVar tempVar = (TempVar)this;
								TempVar tempVar2 = (TempVar)addressOfValue2;
								cil.Variable variable2 = tempVar.item;
								cil.Variable obj3 = tempVar2.item;
								return variable2.Equals(obj3, comp);
							}
						}
						Arg arg = (Arg)this;
						Arg arg2 = (Arg)addressOfValue2;
						return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, arg.item, arg2.item);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(AddressOfValue obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = ((this is TempVar) ? 2 : ((this is AlreadyVar) ? 1 : 0));
					int num2 = ((obj is TempVar) ? 2 : ((obj is AlreadyVar) ? 1 : 0));
					if (num == num2)
					{
						if (!(this is Arg))
						{
							if (this is AlreadyVar)
							{
								AlreadyVar alreadyVar = (AlreadyVar)this;
								AlreadyVar alreadyVar2 = (AlreadyVar)obj;
								return alreadyVar.item.Equals(alreadyVar2.item);
							}
							if (this is TempVar)
							{
								TempVar tempVar = (TempVar)this;
								TempVar tempVar2 = (TempVar)obj;
								return tempVar.item.Equals(tempVar2.item);
							}
						}
						Arg arg = (Arg)this;
						Arg arg2 = (Arg)obj;
						return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(arg.item, arg2.item);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is AddressOfValue obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	internal sealed class prep_address_of_value@1091 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public cil.Variable result;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal prep_address_of_value@1091(cil.CilWriter il, cil.Variable result)
		{
			this.il = il;
			this.result = result;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			il.Append(cil.MyInstruction.NewLdloca(result));
			return null;
		}
	}

	[Serializable]
	internal sealed class prep_address_of_value@1118-2 : FSharpFunc<LLVMValueRef, AddressOfValue>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueRef, AddressOfValue> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal prep_address_of_value@1118-2(FSharpFunc<LLVMValueRef, AddressOfValue> clo2)
		{
			this.clo2 = clo2;
		}

		public override AddressOfValue Invoke(LLVMValueRef arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class prep_address_of_value@1118-1 : FSharpFunc<string, FSharpFunc<LLVMValueRef, AddressOfValue>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, FSharpFunc<LLVMValueRef, AddressOfValue>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal prep_address_of_value@1118-1(FSharpFunc<string, FSharpFunc<LLVMValueRef, AddressOfValue>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<LLVMValueRef, AddressOfValue> Invoke(string arg10)
		{
			FSharpFunc<LLVMValueRef, AddressOfValue> clo = clo1.Invoke(arg10);
			return new prep_address_of_value@1118-2(clo);
		}
	}

	[Serializable]
	internal sealed class load_aggorvec_into_specific_local@1134 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public cil.Variable var_into;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_aggorvec_into_specific_local@1134(cil.CilWriter il, cil.Variable var_into)
		{
			this.il = il;
			this.var_into = var_into;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			il.Append(cil.MyInstruction.NewLdloca(var_into));
			return null;
		}
	}

	[Serializable]
	internal sealed class load_aggorvec_into_specific_local@1149-2 : FSharpFunc<LLVMValueRef, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueRef, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_aggorvec_into_specific_local@1149-2(FSharpFunc<LLVMValueRef, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(LLVMValueRef arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class load_aggorvec_into_specific_local@1149-1 : FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_aggorvec_into_specific_local@1149-1(FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<LLVMValueRef, Unit> Invoke(string arg10)
		{
			FSharpFunc<LLVMValueRef, Unit> clo = clo1.Invoke(arg10);
			return new load_aggorvec_into_specific_local@1149-2(clo);
		}
	}

	[Serializable]
	internal sealed class gen_dump_str@1156 : FSharpFunc<string, Unit>
	{
		public GenSyms syms;

		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_dump_str@1156(GenSyms syms, cil.CilWriter il)
		{
			this.syms = syms;
			this.il = il;
		}

		public override Unit Invoke(string s)
		{
			il.Append(cil.MyInstruction.NewLdstr(s));
			il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("dump_str")));
			return null;
		}
	}

	[Serializable]
	internal sealed class comment@1166 : FSharpFunc<string, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal comment@1166(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(string s)
		{
			il_comment(il, s);
			return null;
		}
	}

	[Serializable]
	internal sealed class todo@1168 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public MethodReference ref_typ_e;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal todo@1168(cil.CilWriter il, MethodReference ref_typ_e)
		{
			this.il = il;
			this.ref_typ_e = ref_typ_e;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new todo@1168T<a>(il, ref_typ_e, this);
		}
	}

	[Serializable]
	internal sealed class todo@1168T<a> : FSharpFunc<a, Unit>
	{
		public cil.CilWriter il;

		public MethodReference ref_typ_e;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public todo@1168 self2@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal todo@1168T(cil.CilWriter il, MethodReference ref_typ_e, todo@1168 self2@)
		{
			this.il = il;
			this.ref_typ_e = ref_typ_e;
			this.self2@ = self2@;
		}

		public override Unit Invoke(a q)
		{
			todo@1168 todo@1169 = self2@;
			FSharpFunc<a, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<a, string>, Unit, string, string, a>("TODO %A"));
			string text = fSharpFunc.Invoke(q);
			FSharpFunc<string, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("%s"));
			string func = text;
			fSharpFunc2.Invoke(func);
			todo@1169.il.Append(cil.MyInstruction.NewLdstr(text));
			todo@1169.il.Append(cil.MyInstruction.NewNewobj(todo@1169.ref_typ_e));
			todo@1169.il.Append(cil.MyInstruction.Throw);
			return null;
		}
	}

	[Serializable]
	internal sealed class load_value@1176 : FSharpFunc<LLVMValueRef, Unit>
	{
		public cil.GenTypes typs;

		public GenSyms syms;

		public cil.CilWriter il;

		public Dictionary<LLVMValueRef, ParameterDefinition> args;

		public FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value;

		public FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_value@1176(cil.GenTypes typs, GenSyms syms, cil.CilWriter il, Dictionary<LLVMValueRef, ParameterDefinition> args, FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest)
		{
			this.typs = typs;
			this.syms = syms;
			this.il = il;
			this.args = args;
			this.f_get_instr_value = f_get_instr_value;
			this.f_load_instr_value_to_dest = f_load_instr_value_to_dest;
		}

		public override Unit Invoke(LLVMValueRef v)
		{
			outer_load_value(typs, syms, il, args, f_get_instr_value, f_load_instr_value_to_dest, v);
			return null;
		}
	}

	[Serializable]
	internal sealed class my_prep_address_of_value@1179 : FSharpFunc<LLVMValueRef, AddressOfValue>
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public Dictionary<LLVMValueRef, ParameterDefinition> args;

		public FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value;

		public FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_prep_address_of_value@1179(GenSyms syms, cil.CilWriter il, Dictionary<LLVMValueRef, ParameterDefinition> args, FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest)
		{
			this.syms = syms;
			this.il = il;
			this.args = args;
			this.f_get_instr_value = f_get_instr_value;
			this.f_load_instr_value_to_dest = f_load_instr_value_to_dest;
		}

		public override AddressOfValue Invoke(LLVMValueRef v)
		{
			return prep_address_of_value(syms, il, args, f_get_instr_value, f_load_instr_value_to_dest, v);
		}
	}

	[Serializable]
	internal sealed class get_result_local@1183 : FSharpFunc<Unit, ResultLocal>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public InstructionDest result_dest;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_result_local@1183(cil.CilWriter il, LLVMValueRef op, InstructionDest result_dest)
		{
			this.il = il;
			this.op = op;
			this.result_dest = result_dest;
		}

		public override ResultLocal Invoke(Unit unitVar0)
		{
			InstructionDest instructionDest = result_dest;
			InstructionDest instructionDest2 = instructionDest;
			if (!(instructionDest2 is InstructionDest.InTemp))
			{
				if (instructionDest2 is InstructionDest._OnStack)
				{
					cil.FirstClassType tr = llvm_type_to_firstclass_type(op.TypeOf);
					cil.Variable tempVariable = il.GetTempVariable(tr);
					cil.Variable item = tempVariable;
					return ResultLocal.NewTempResult(item);
				}
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 1183, 18);
			}
			InstructionDest.InTemp inTemp = (InstructionDest.InTemp)instructionDest;
			cil.Variable item2 = inTemp.item;
			return ResultLocal.NewDest(item2);
		}
	}

	[Serializable]
	internal sealed class ldloca_result_local@1191 : FSharpFunc<ResultLocal, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal ldloca_result_local@1191(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(ResultLocal v)
		{
			cil.Variable item = ((v is ResultLocal.TempResult) ? ((ResultLocal.TempResult)v).item : ((ResultLocal.Dest)v).item);
			il.Append(cil.MyInstruction.NewLdloca(item));
			return null;
		}
	}

	[Serializable]
	internal sealed class init_result_local@1198 : FSharpFunc<ResultLocal, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal init_result_local@1198(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(ResultLocal v)
		{
			init_local(dest: (v is ResultLocal.TempResult) ? ((ResultLocal.TempResult)v).item : ((ResultLocal.Dest)v).item, il: il);
			return null;
		}
	}

	[Serializable]
	internal sealed class grab_result_local@1206 : FSharpFunc<ResultLocal, cil.Variable>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal grab_result_local@1206()
		{
		}

		public override cil.Variable Invoke(ResultLocal v)
		{
			if (!(v is ResultLocal.TempResult))
			{
				return ((ResultLocal.Dest)v).item;
			}
			return ((ResultLocal.TempResult)v).item;
		}
	}

	[Serializable]
	internal sealed class finish_result_local@1213 : FSharpFunc<ResultLocal, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal finish_result_local@1213(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(ResultLocal v)
		{
			if (!(v is ResultLocal.TempResult))
			{
				ResultLocal.Dest dest = (ResultLocal.Dest)v;
				return null;
			}
			ResultLocal.TempResult tempResult = (ResultLocal.TempResult)v;
			cil.Variable variable = tempResult.item;
			il.Append(cil.MyInstruction.NewLdloc(variable));
			il.ReleaseTempVariable(variable);
			return null;
		}
	}

	[Serializable]
	internal sealed class walk_indexes@1221 : OptimizedClosures.FSharpFunc<cil.FirstClassType, LLVMValueRef[], int, cil.FirstClassType>
	{
		public cil.CilWriter il;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal walk_indexes@1221(cil.CilWriter il, FSharpFunc<LLVMValueRef, Unit> load_value)
		{
			this.il = il;
			this.load_value = load_value;
		}

		public override cil.FirstClassType Invoke(cil.FirstClassType typ, LLVMValueRef[] indexes, int cur)
		{
			cil.FirstClassType firstClassType3;
			while (true)
			{
				cil.FirstClassType firstClassType = typ;
				cil.FirstClassType firstClassType2;
				switch (firstClassType.Tag)
				{
				case 2:
				{
					cil.FirstClassType.StructType structType = (cil.FirstClassType.StructType)firstClassType;
					cil.StructType item3 = structType.Item;
					long num8 = get_integer_constant(indexes[cur]);
					long num9 = num8;
					int num10 = (int)num9;
					il.Append(cil.MyInstruction.NewLdc_I4((int)item3.items[num10].off));
					il.Append(cil.MyInstruction.Add);
					firstClassType2 = item3.items[num10].typ;
					break;
				}
				case 3:
				{
					cil.FirstClassType.ArrayType arrayType = (cil.FirstClassType.ArrayType)firstClassType;
					cil.ArrayType item2 = arrayType.Item;
					FSharpOption<long> fSharpOption3 = get_maybe_integer_constant(indexes[cur]);
					if (fSharpOption3 != null)
					{
						FSharpOption<long> fSharpOption4 = fSharpOption3;
						long value2 = fSharpOption4.Value;
						int num5 = (int)value2;
						int num6 = cil.get_sizeof(item2.elemtype) * num5;
						if (num6 != 0)
						{
							il.Append(cil.MyInstruction.NewLdc_I4(num6));
							il.Append(cil.MyInstruction.Conv_I);
							il.Append(cil.MyInstruction.Add);
						}
						firstClassType2 = item2.elemtype;
						break;
					}
					load_value.Invoke(indexes[cur]);
					int num7 = cil.get_sizeof(item2.elemtype);
					if (num7 == 0)
					{
						string message2 = "TODO";
						throw Operators.Failure(message2);
					}
					il.Append(cil.MyInstruction.NewLdc_I4(num7));
					il.Append(cil.MyInstruction.Mul);
					il.Append(cil.MyInstruction.Conv_I);
					il.Append(cil.MyInstruction.Add);
					firstClassType2 = item2.elemtype;
					break;
				}
				case 1:
				{
					cil.FirstClassType.VectorType vectorType = (cil.FirstClassType.VectorType)firstClassType;
					cil.VectorType item = vectorType.Item;
					FSharpOption<long> fSharpOption = get_maybe_integer_constant(indexes[cur]);
					if (fSharpOption != null)
					{
						FSharpOption<long> fSharpOption2 = fSharpOption;
						long value = fSharpOption2.Value;
						int num = (int)value;
						int num2 = cil.sizeof_vec_elem(item.elemtype);
						int num3 = num * num2;
						if (num3 != 0)
						{
							il.Append(cil.MyInstruction.NewLdc_I4(num3));
							il.Append(cil.MyInstruction.Add);
						}
						firstClassType2 = cil.vec_elem_type_to_first_class(item.elemtype);
						break;
					}
					load_value.Invoke(indexes[cur]);
					int num4 = cil.sizeof_vec_elem(item.elemtype);
					if (num4 == 0)
					{
						string message = "TODO";
						throw Operators.Failure(message);
					}
					il.Append(cil.MyInstruction.NewLdc_I4(num4));
					il.Append(cil.MyInstruction.Mul);
					il.Append(cil.MyInstruction.Conv_I);
					il.Append(cil.MyInstruction.Add);
					firstClassType2 = cil.vec_elem_type_to_first_class(item.elemtype);
					break;
				}
				default:
				{
					FSharpFunc<cil.FirstClassType, cil.FirstClassType> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, cil.FirstClassType>, Unit, string, cil.FirstClassType, cil.FirstClassType>("walk_indexes for %A"));
					cil.FirstClassType func = typ;
					firstClassType2 = fSharpFunc.Invoke(func);
					break;
				}
				}
				firstClassType3 = firstClassType2;
				if (cur == indexes.Length - 1)
				{
					break;
				}
				LLVMValueRef[] array = indexes;
				cur++;
				indexes = array;
				typ = firstClassType3;
			}
			return firstClassType3;
		}
	}

	[Serializable]
	internal sealed class store@1284 : OptimizedClosures.FSharpFunc<cil.FirstClassType, LLVMValueRef, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal store@1284(cil.CilWriter il, FSharpFunc<LLVMValueRef, Unit> load_value, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value)
		{
			this.il = il;
			this.load_value = load_value;
			this.my_prep_address_of_value = my_prep_address_of_value;
		}

		public override Unit Invoke(cil.FirstClassType typ, LLVMValueRef v)
		{
			switch (typ.Tag)
			{
			case 1:
			{
				cil.FirstClassType.VectorType vectorType = (cil.FirstClassType.VectorType)typ;
				break;
			}
			case 2:
			{
				cil.FirstClassType.StructType structType = (cil.FirstClassType.StructType)typ;
				break;
			}
			case 3:
			{
				cil.FirstClassType.ArrayType arrayType = (cil.FirstClassType.ArrayType)typ;
				break;
			}
			case 4:
			{
				cil.FirstClassType.FunnyIntegerType funnyIntegerType = (cil.FirstClassType.FunnyIntegerType)typ;
				break;
			}
			default:
				load_value.Invoke(v);
				il.Append(cil.MyInstruction.Stind_I);
				return null;
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)typ;
				cil.PrimitiveType item = primitiveType.Item;
				load_value.Invoke(v);
				store_mem_primitive_from_stack(il, item);
				return null;
			}
			}
			AddressOfValue addr = my_prep_address_of_value.Invoke(v);
			emit_address_of_value(il, addr);
			il.Append(cil.MyInstruction.NewLdc_I4(cil.get_sizeof(typ)));
			il.Append(cil.MyInstruction.Cpblk);
			release_address_of_value(il, addr);
			return null;
		}
	}

	[Serializable]
	internal sealed class load_vecprim_to_stack@1313 : FSharpFunc<cil.VecPrimitiveType, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_vecprim_to_stack@1313(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(cil.VecPrimitiveType pt)
		{
			switch (pt.Tag)
			{
			default:
				il.Append(cil.MyInstruction.Ldind_I1);
				return null;
			case 1:
				il.Append(cil.MyInstruction.Ldind_I2);
				return null;
			case 2:
				il.Append(cil.MyInstruction.Ldind_I4);
				return null;
			case 3:
				il.Append(cil.MyInstruction.Ldind_I8);
				return null;
			case 4:
				il.Append(cil.MyInstruction.Ldind_R4);
				return null;
			case 5:
				il.Append(cil.MyInstruction.Ldind_R8);
				return null;
			}
		}
	}

	[Serializable]
	internal sealed class load_mem_primitive_to_stack@1329 : FSharpFunc<cil.PrimitiveType, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_mem_primitive_to_stack@1329(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(cil.PrimitiveType pt)
		{
			switch (pt.Tag)
			{
			default:
				il.Append(cil.MyInstruction.Ldind_U1);
				il.Append(cil.MyInstruction.NewLdc_I4(1));
				il.Append(cil.MyInstruction.And);
				return null;
			case 1:
				il.Append(cil.MyInstruction.Ldind_I1);
				return null;
			case 2:
				il.Append(cil.MyInstruction.Ldind_I2);
				return null;
			case 3:
				il.Append(cil.MyInstruction.Ldind_I4);
				return null;
			case 4:
				il.Append(cil.MyInstruction.Ldind_I8);
				return null;
			case 5:
				il.Append(cil.MyInstruction.Ldind_R4);
				return null;
			case 6:
				il.Append(cil.MyInstruction.Ldind_R8);
				return null;
			}
		}
	}

	[Serializable]
	internal sealed class save_op_result_from_stack@1342 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public InstructionDest result_dest;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal save_op_result_from_stack@1342(cil.CilWriter il, InstructionDest result_dest)
		{
			this.il = il;
			this.result_dest = result_dest;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			InstructionDest instructionDest = result_dest;
			InstructionDest instructionDest2 = instructionDest;
			if (!(instructionDest2 is InstructionDest._OnStack))
			{
				if (!(instructionDest2 is InstructionDest._Void))
				{
					InstructionDest.InTemp inTemp = (InstructionDest.InTemp)instructionDest;
					cil.Variable item = inTemp.item;
					il.Append(cil.MyInstruction.NewStloc(item));
					return null;
				}
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class load_mem_and_save_op_result@1355 : OptimizedClosures.FSharpFunc<cil.FirstClassType, LLVMValueRef, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, cil.Variable> grab_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		public FSharpFunc<cil.PrimitiveType, Unit> load_mem_primitive_to_stack;

		public FSharpFunc<Unit, Unit> save_op_result_from_stack;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_mem_and_save_op_result@1355(cil.CilWriter il, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, cil.Variable> grab_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local, FSharpFunc<cil.PrimitiveType, Unit> load_mem_primitive_to_stack, FSharpFunc<Unit, Unit> save_op_result_from_stack)
		{
			this.il = il;
			this.get_result_local = get_result_local;
			this.grab_result_local = grab_result_local;
			this.finish_result_local = finish_result_local;
			this.load_mem_primitive_to_stack = load_mem_primitive_to_stack;
			this.save_op_result_from_stack = save_op_result_from_stack;
		}

		public override Unit Invoke(cil.FirstClassType vt, LLVMValueRef op)
		{
			switch (vt.Tag)
			{
			case 3:
			{
				cil.FirstClassType.ArrayType arrayType = (cil.FirstClassType.ArrayType)vt;
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType = (cil.FirstClassType.VectorType)vt;
				break;
			}
			case 2:
			{
				cil.FirstClassType.StructType structType = (cil.FirstClassType.StructType)vt;
				break;
			}
			case 4:
			{
				cil.FirstClassType.FunnyIntegerType funnyIntegerType = (cil.FirstClassType.FunnyIntegerType)vt;
				break;
			}
			default:
				il.Append(cil.MyInstruction.Ldind_I);
				return save_op_result_from_stack.Invoke(null);
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)vt;
				cil.PrimitiveType item = primitiveType.Item;
				load_mem_primitive_to_stack.Invoke(item);
				return save_op_result_from_stack.Invoke(null);
			}
			}
			cil.Variable tempVariable = il.GetTempVariable(cil.FirstClassType.Ptr);
			il.Append(cil.MyInstruction.NewStloc(tempVariable));
			ResultLocal func = get_result_local.Invoke(null);
			cpblk_into_from_ldloc(il, grab_result_local.Invoke(func), tempVariable, cil.get_sizeof(vt));
			il.ReleaseTempVariable(tempVariable);
			return finish_result_local.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class find_label@1383 : OptimizedClosures.FSharpFunc<LLVMValueRef, LLVMValueRef, cil.Label>
	{
		public Dictionary<LLVMBasicBlockRef, Block> labels;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal find_label@1383(Dictionary<LLVMBasicBlockRef, Block> labels)
		{
			this.labels = labels;
		}

		public override cil.Label Invoke(LLVMValueRef op, LLVMValueRef v)
		{
			LLVMBasicBlockRef key = v.AsBasicBlock();
			Block value;
			Tuple<bool, Block> tuple = new Tuple<bool, Block>(labels.TryGetValue(key, out value), value);
			if (tuple.Item1)
			{
				if (!(tuple.Item2 is Block.Phi))
				{
					Block.Regular regular = (Block.Regular)tuple.Item2;
					return regular.item;
				}
				Block.Phi phi = (Block.Phi)tuple.Item2;
				Dictionary<LLVMBasicBlockRef, cil.Label> dictionary = phi.item;
				LLVMBasicBlockRef instructionParent = op.InstructionParent;
				return dictionary[instructionParent];
			}
			FSharpFunc<LLVMValueRef, cil.Label> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, cil.Label>, Unit, string, cil.Label, LLVMValueRef>("label not found: %A"));
			LLVMValueRef func = op;
			return fSharpFunc.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class get_opcode@1395 : FSharpFunc<LLVMValueRef, LLVMOpcode>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_opcode@1395()
		{
		}

		public override LLVMOpcode Invoke(LLVMValueRef op)
		{
			return sgllvm.get_value_kind(op) switch
			{
				LLVMValueKind.LLVMConstantExprValueKind => op.ConstOpcode, 
				LLVMValueKind.LLVMInstructionValueKind => op.InstructionOpcode, 
				_ => throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 1395, 18), 
			};
		}
	}

	[Serializable]
	internal sealed class simple_binop_primitive_opcode@1402 : FSharpFunc<LLVMValueRef, cil.MyInstruction>
	{
		public FSharpFunc<LLVMValueRef, LLVMOpcode> get_opcode;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal simple_binop_primitive_opcode@1402(FSharpFunc<LLVMValueRef, LLVMOpcode> get_opcode)
		{
			this.get_opcode = get_opcode;
		}

		public override cil.MyInstruction Invoke(LLVMValueRef op)
		{
			switch (get_opcode.Invoke(op))
			{
			case LLVMOpcode.LLVMAdd:
				return cil.MyInstruction.Add;
			case LLVMOpcode.LLVMMul:
				return cil.MyInstruction.Mul;
			case LLVMOpcode.LLVMSDiv:
				return cil.MyInstruction.Div;
			case LLVMOpcode.LLVMSRem:
				return cil.MyInstruction.Rem;
			case LLVMOpcode.LLVMAnd:
				return cil.MyInstruction.And;
			case LLVMOpcode.LLVMOr:
				return cil.MyInstruction.Or;
			case LLVMOpcode.LLVMXor:
				return cil.MyInstruction.Xor;
			case LLVMOpcode.LLVMSub:
				return cil.MyInstruction.Sub;
			case LLVMOpcode.LLVMFAdd:
				return cil.MyInstruction.Add;
			case LLVMOpcode.LLVMFMul:
				return cil.MyInstruction.Mul;
			case LLVMOpcode.LLVMFDiv:
				return cil.MyInstruction.Div;
			case LLVMOpcode.LLVMFSub:
				return cil.MyInstruction.Sub;
			case LLVMOpcode.LLVMFRem:
				return cil.MyInstruction.Rem;
			case LLVMOpcode.LLVMAShr:
				return cil.MyInstruction.Shr;
			case LLVMOpcode.LLVMShl:
				return cil.MyInstruction.Shl;
			case LLVMOpcode.LLVMUDiv:
				return cil.MyInstruction.Div_Un;
			case LLVMOpcode.LLVMURem:
				return cil.MyInstruction.Rem_Un;
			case LLVMOpcode.LLVMLShr:
				return cil.MyInstruction.Shr_Un;
			default:
			{
				FSharpFunc<LLVMValueRef, cil.MyInstruction> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, cil.MyInstruction>, Unit, string, cil.MyInstruction, LLVMValueRef>("simple_binop unknown op: %A"));
				return fSharpFunc.Invoke(op);
			}
			}
		}
	}

	[Serializable]
	internal sealed class get_funny_method_ref@1424 : FSharpFunc<LLVMValueRef, MethodReference>
	{
		public GenSyms syms;

		public FSharpFunc<LLVMValueRef, LLVMOpcode> get_opcode;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_funny_method_ref@1424(GenSyms syms, FSharpFunc<LLVMValueRef, LLVMOpcode> get_opcode)
		{
			this.syms = syms;
			this.get_opcode = get_opcode;
		}

		public override MethodReference Invoke(LLVMValueRef op)
		{
			switch (get_opcode.Invoke(op))
			{
			case LLVMOpcode.LLVMShl:
				return syms.f_get_rt@.Invoke("strange_shl");
			case LLVMOpcode.LLVMAShr:
				return syms.f_get_rt@.Invoke("strange_ashr");
			case LLVMOpcode.LLVMLShr:
				return syms.f_get_rt@.Invoke("strange_lshr");
			case LLVMOpcode.LLVMAdd:
				return syms.f_get_rt@.Invoke("strange_add");
			case LLVMOpcode.LLVMMul:
				return syms.f_get_rt@.Invoke("strange_mul");
			case LLVMOpcode.LLVMSub:
				return syms.f_get_rt@.Invoke("strange_sub");
			case LLVMOpcode.LLVMSDiv:
				return syms.f_get_rt@.Invoke("strange_sdiv");
			case LLVMOpcode.LLVMSRem:
				return syms.f_get_rt@.Invoke("strange_srem");
			case LLVMOpcode.LLVMUDiv:
				return syms.f_get_rt@.Invoke("strange_udiv");
			case LLVMOpcode.LLVMURem:
				return syms.f_get_rt@.Invoke("strange_urem");
			case LLVMOpcode.LLVMAnd:
				return syms.f_get_rt@.Invoke("strange_and");
			case LLVMOpcode.LLVMOr:
				return syms.f_get_rt@.Invoke("strange_or");
			case LLVMOpcode.LLVMXor:
				return syms.f_get_rt@.Invoke("strange_xor");
			default:
			{
				FSharpFunc<LLVMValueRef, MethodReference> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, MethodReference>, Unit, string, MethodReference, LLVMValueRef>("get_funny_method_ref unknown op: %A"));
				return fSharpFunc.Invoke(op);
			}
			}
		}
	}

	[Serializable]
	internal sealed class simple_binop_funny@1445 : OptimizedClosures.FSharpFunc<cil.FunnyIntegerType, LLVMValueRef, Unit>
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public FSharpTypeFunc todo;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, Unit> ldloca_result_local;

		public FSharpFunc<ResultLocal, cil.Variable> grab_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		public FSharpFunc<LLVMValueRef, LLVMOpcode> get_opcode;

		public FSharpFunc<LLVMValueRef, cil.MyInstruction> simple_binop_primitive_opcode;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal simple_binop_funny@1445(GenSyms syms, cil.CilWriter il, FSharpTypeFunc todo, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, Unit> ldloca_result_local, FSharpFunc<ResultLocal, cil.Variable> grab_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local, FSharpFunc<LLVMValueRef, LLVMOpcode> get_opcode, FSharpFunc<LLVMValueRef, cil.MyInstruction> simple_binop_primitive_opcode)
		{
			this.syms = syms;
			this.il = il;
			this.todo = todo;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.get_result_local = get_result_local;
			this.ldloca_result_local = ldloca_result_local;
			this.grab_result_local = grab_result_local;
			this.finish_result_local = finish_result_local;
			this.get_opcode = get_opcode;
			this.simple_binop_primitive_opcode = simple_binop_primitive_opcode;
		}

		public override Unit Invoke(cil.FunnyIntegerType t, LLVMValueRef op)
		{
			ResultLocal func = get_result_local.Invoke(null);
			int num = cil.sizeof_funnyinteger_rounded_up(t);
			AddressOfValue addr = my_prep_address_of_value.Invoke(op.GetOperand(0u));
			AddressOfValue addr2 = my_prep_address_of_value.Invoke(op.GetOperand(1u));
			if (t.bits < 64)
			{
				MethodReference methodReference;
				switch (get_opcode.Invoke(op))
				{
				case LLVMOpcode.LLVMUDiv:
				case LLVMOpcode.LLVMURem:
				case LLVMOpcode.LLVMAnd:
				case LLVMOpcode.LLVMOr:
				case LLVMOpcode.LLVMXor:
					methodReference = syms.f_get_rt@.Invoke("part_to_u64");
					break;
				default:
					methodReference = syms.f_get_rt@.Invoke("part_to_i64");
					break;
				}
				MethodReference item = methodReference;
				il.Append(cil.MyInstruction.NewLdc_I4(t.bits));
				emit_address_of_value(il, addr);
				il.Append(cil.MyInstruction.NewCall(item));
				il.Append(cil.MyInstruction.NewLdc_I4(t.bits));
				emit_address_of_value(il, addr2);
				il.Append(cil.MyInstruction.NewCall(item));
				cil.MyInstruction instr = simple_binop_primitive_opcode.Invoke(op);
				il.Append(instr);
				long item2 = make_mask_64(t);
				il.Append(cil.MyInstruction.NewLdc_I8(item2));
				il.Append(cil.MyInstruction.And);
				cil.Variable tempVariable = il.GetTempVariable(cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.I64));
				il.Append(cil.MyInstruction.NewStloc(tempVariable));
				cpblk_into_from_ldloca(il, grab_result_local.Invoke(func), tempVariable, num);
				il.ReleaseTempVariable(tempVariable);
			}
			else
			{
				il.Append(cil.MyInstruction.NewLdc_I4(num));
				emit_address_of_value(il, addr);
				emit_address_of_value(il, addr2);
				ldloca_result_local.Invoke(func);
				switch (get_opcode.Invoke(op))
				{
				case LLVMOpcode.LLVMAdd:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_add")));
					break;
				case LLVMOpcode.LLVMMul:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_mul")));
					break;
				case LLVMOpcode.LLVMSub:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_sub")));
					break;
				case LLVMOpcode.LLVMSDiv:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_sdiv")));
					break;
				case LLVMOpcode.LLVMSRem:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_srem")));
					break;
				case LLVMOpcode.LLVMUDiv:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_udiv")));
					break;
				case LLVMOpcode.LLVMURem:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_urem")));
					break;
				case LLVMOpcode.LLVMAnd:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_and")));
					break;
				case LLVMOpcode.LLVMOr:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_or")));
					break;
				case LLVMOpcode.LLVMXor:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_xor")));
					break;
				default:
				{
					FSharpTypeFunc fSharpTypeFunc = todo;
					LLVMValueRef func2 = op;
					((FSharpFunc<LLVMValueRef, Unit>)fSharpTypeFunc.Specialize<LLVMValueRef>()).Invoke(func2);
					break;
				}
				}
			}
			release_address_of_value(il, addr);
			release_address_of_value(il, addr2);
			return finish_result_local.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class funny_shift@1511 : OptimizedClosures.FSharpFunc<cil.FunnyIntegerType, LLVMValueRef, Unit>
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public FSharpTypeFunc todo;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, Unit> ldloca_result_local;

		public FSharpFunc<ResultLocal, cil.Variable> grab_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		public FSharpFunc<LLVMValueRef, LLVMOpcode> get_opcode;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal funny_shift@1511(GenSyms syms, cil.CilWriter il, FSharpTypeFunc todo, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, Unit> ldloca_result_local, FSharpFunc<ResultLocal, cil.Variable> grab_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local, FSharpFunc<LLVMValueRef, LLVMOpcode> get_opcode)
		{
			this.syms = syms;
			this.il = il;
			this.todo = todo;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.get_result_local = get_result_local;
			this.ldloca_result_local = ldloca_result_local;
			this.grab_result_local = grab_result_local;
			this.finish_result_local = finish_result_local;
			this.get_opcode = get_opcode;
		}

		public override Unit Invoke(cil.FunnyIntegerType t, LLVMValueRef op)
		{
			ResultLocal func = get_result_local.Invoke(null);
			int num = cil.sizeof_funnyinteger_rounded_up(t);
			AddressOfValue addr = my_prep_address_of_value.Invoke(op.GetOperand(0u));
			AddressOfValue addr2 = my_prep_address_of_value.Invoke(op.GetOperand(1u));
			if (t.bits < 64)
			{
				MethodReference methodReference;
				switch (get_opcode.Invoke(op))
				{
				case LLVMOpcode.LLVMShl:
				case LLVMOpcode.LLVMLShr:
					methodReference = syms.f_get_rt@.Invoke("part_to_u64");
					break;
				default:
					methodReference = syms.f_get_rt@.Invoke("part_to_i64");
					break;
				}
				MethodReference item = methodReference;
				il.Append(cil.MyInstruction.NewLdc_I4(t.bits));
				emit_address_of_value(il, addr);
				il.Append(cil.MyInstruction.NewCall(item));
				if (op.GetOperand(1u).IsConstant)
				{
					il.Append(cil.MyInstruction.NewLdc_I4(t.bits));
					emit_address_of_value(il, addr2);
					il.Append(cil.MyInstruction.NewCall(item));
					il.Append(cil.MyInstruction.Conv_I4);
				}
				else
				{
					il.Append(cil.MyInstruction.NewLdc_I4(t.bits));
					emit_address_of_value(il, addr2);
					il.Append(cil.MyInstruction.NewCall(item));
					il.Append(cil.MyInstruction.Conv_I4);
				}
				cil.MyInstruction instr = get_opcode.Invoke(op) switch
				{
					LLVMOpcode.LLVMAShr => cil.MyInstruction.Shr, 
					LLVMOpcode.LLVMShl => cil.MyInstruction.Shl, 
					LLVMOpcode.LLVMLShr => cil.MyInstruction.Shr_Un, 
					_ => throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 1543, 26), 
				};
				il.Append(instr);
				long item2 = make_mask_64(t);
				il.Append(cil.MyInstruction.NewLdc_I8(item2));
				il.Append(cil.MyInstruction.And);
				cil.Variable variable = il.NewVariable(cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.I64));
				il.Append(cil.MyInstruction.NewStloc(variable));
				cpblk_into_from_ldloca(il, grab_result_local.Invoke(func), variable, num);
			}
			else
			{
				il.Append(cil.MyInstruction.NewLdc_I4(num));
				emit_address_of_value(il, addr);
				emit_address_of_value(il, addr2);
				ldloca_result_local.Invoke(func);
				switch (get_opcode.Invoke(op))
				{
				case LLVMOpcode.LLVMShl:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_shl")));
					break;
				case LLVMOpcode.LLVMAShr:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_ashr")));
					break;
				case LLVMOpcode.LLVMLShr:
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_lshr")));
					break;
				default:
				{
					FSharpTypeFunc fSharpTypeFunc = todo;
					LLVMValueRef func2 = op;
					((FSharpFunc<LLVMValueRef, Unit>)fSharpTypeFunc.Specialize<LLVMValueRef>()).Invoke(func2);
					break;
				}
				}
			}
			release_address_of_value(il, addr);
			release_address_of_value(il, addr2);
			return finish_result_local.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class add_offset@1631 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@1631(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@1637 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public cil.VecPrimitiveType elemtype;

		public int elem_size;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public FSharpFunc<int, Unit> add_offset;

		public cil.MyInstruction opcode;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@1637(cil.CilWriter il, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, cil.VecPrimitiveType elemtype, int elem_size, AddressOfValue v1, AddressOfValue v2, FSharpFunc<int, Unit> add_offset, cil.MyInstruction opcode)
		{
			this.il = il;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.elemtype = elemtype;
			this.elem_size = elem_size;
			this.v1 = v1;
			this.v2 = v2;
			this.add_offset = add_offset;
			this.opcode = opcode;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@1637T<a>(il, load_vecprim_to_stack, elemtype, elem_size, v1, v2, add_offset, opcode, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@1637T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public cil.VecPrimitiveType elemtype;

		public int elem_size;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public FSharpFunc<int, Unit> add_offset;

		public cil.MyInstruction opcode;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@1637 self8@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@1637T(cil.CilWriter il, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, cil.VecPrimitiveType elemtype, int elem_size, AddressOfValue v1, AddressOfValue v2, FSharpFunc<int, Unit> add_offset, cil.MyInstruction opcode, get_nth_val@1637 self8@)
		{
			this.il = il;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.elemtype = elemtype;
			this.elem_size = elem_size;
			this.v1 = v1;
			this.v2 = v2;
			this.add_offset = add_offset;
			this.opcode = opcode;
			this.self8@ = self8@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@1637 get_nth_val@1638 = self8@;
			int func = i * get_nth_val@1638.elem_size;
			emit_address_of_value(get_nth_val@1638.il, get_nth_val@1638.v1);
			get_nth_val@1638.add_offset.Invoke(func);
			get_nth_val@1638.load_vecprim_to_stack.Invoke(get_nth_val@1638.elemtype);
			emit_address_of_value(get_nth_val@1638.il, get_nth_val@1638.v2);
			get_nth_val@1638.add_offset.Invoke(func);
			get_nth_val@1638.load_vecprim_to_stack.Invoke(get_nth_val@1638.elemtype);
			get_nth_val@1638.il.Append(get_nth_val@1638.opcode);
			return null;
		}
	}

	[Serializable]
	internal sealed class add_offset@1664-1 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@1664-1(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@1670-1 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.PrimitiveType, Unit> load_mem_primitive_to_stack;

		public int elem_size;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public FSharpFunc<int, Unit> add_offset;

		public cil.MyInstruction opcode;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@1670-1(cil.CilWriter il, FSharpFunc<cil.PrimitiveType, Unit> load_mem_primitive_to_stack, int elem_size, AddressOfValue v1, AddressOfValue v2, FSharpFunc<int, Unit> add_offset, cil.MyInstruction opcode)
		{
			this.il = il;
			this.load_mem_primitive_to_stack = load_mem_primitive_to_stack;
			this.elem_size = elem_size;
			this.v1 = v1;
			this.v2 = v2;
			this.add_offset = add_offset;
			this.opcode = opcode;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@1670-1T<a>(il, load_mem_primitive_to_stack, elem_size, v1, v2, add_offset, opcode, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@1670-1T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.PrimitiveType, Unit> load_mem_primitive_to_stack;

		public int elem_size;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public FSharpFunc<int, Unit> add_offset;

		public cil.MyInstruction opcode;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@1670-1 self7@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@1670-1T(cil.CilWriter il, FSharpFunc<cil.PrimitiveType, Unit> load_mem_primitive_to_stack, int elem_size, AddressOfValue v1, AddressOfValue v2, FSharpFunc<int, Unit> add_offset, cil.MyInstruction opcode, get_nth_val@1670-1 self7@)
		{
			this.il = il;
			this.load_mem_primitive_to_stack = load_mem_primitive_to_stack;
			this.elem_size = elem_size;
			this.v1 = v1;
			this.v2 = v2;
			this.add_offset = add_offset;
			this.opcode = opcode;
			this.self7@ = self7@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@1670-1 get_nth_val@1670-2 = self7@;
			int func = i * get_nth_val@1670-2.elem_size;
			emit_address_of_value(get_nth_val@1670-2.il, get_nth_val@1670-2.v1);
			get_nth_val@1670-2.add_offset.Invoke(func);
			get_nth_val@1670-2.load_mem_primitive_to_stack.Invoke(cil.PrimitiveType.I1);
			emit_address_of_value(get_nth_val@1670-2.il, get_nth_val@1670-2.v2);
			get_nth_val@1670-2.add_offset.Invoke(func);
			get_nth_val@1670-2.load_mem_primitive_to_stack.Invoke(cil.PrimitiveType.I1);
			get_nth_val@1670-2.il.Append(get_nth_val@1670-2.opcode);
			return null;
		}
	}

	[Serializable]
	internal sealed class add_offset@1697-2 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@1697-2(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class simple_binop_vector@1578 : FSharpFunc<LLVMValueRef, Unit>
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, Unit> ldloca_result_local;

		public FSharpFunc<ResultLocal, cil.Variable> grab_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public FSharpFunc<cil.PrimitiveType, Unit> load_mem_primitive_to_stack;

		public FSharpFunc<Unit, Unit> save_op_result_from_stack;

		public FSharpFunc<LLVMValueRef, LLVMOpcode> get_opcode;

		public FSharpFunc<LLVMValueRef, cil.MyInstruction> simple_binop_primitive_opcode;

		public FSharpFunc<LLVMValueRef, MethodReference> get_funny_method_ref;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal simple_binop_vector@1578(GenSyms syms, cil.CilWriter il, FSharpFunc<LLVMValueRef, Unit> load_value, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, Unit> ldloca_result_local, FSharpFunc<ResultLocal, cil.Variable> grab_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, FSharpFunc<cil.PrimitiveType, Unit> load_mem_primitive_to_stack, FSharpFunc<Unit, Unit> save_op_result_from_stack, FSharpFunc<LLVMValueRef, LLVMOpcode> get_opcode, FSharpFunc<LLVMValueRef, cil.MyInstruction> simple_binop_primitive_opcode, FSharpFunc<LLVMValueRef, MethodReference> get_funny_method_ref)
		{
			this.syms = syms;
			this.il = il;
			this.load_value = load_value;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.get_result_local = get_result_local;
			this.ldloca_result_local = ldloca_result_local;
			this.grab_result_local = grab_result_local;
			this.finish_result_local = finish_result_local;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.load_mem_primitive_to_stack = load_mem_primitive_to_stack;
			this.save_op_result_from_stack = save_op_result_from_stack;
			this.get_opcode = get_opcode;
			this.simple_binop_primitive_opcode = simple_binop_primitive_opcode;
			this.get_funny_method_ref = get_funny_method_ref;
		}

		public override Unit Invoke(LLVMValueRef op)
		{
			cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(op.TypeOf);
			cil.FirstClassType firstClassType2 = firstClassType;
			if (firstClassType2.Tag == 1)
			{
				cil.FirstClassType.VectorType vectorType = (cil.FirstClassType.VectorType)firstClassType2;
				if (vectorType.Item.elemtype.Tag == 2)
				{
					cil.VectorElementType.VecPrim vecPrim = (cil.VectorElementType.VecPrim)vectorType.Item.elemtype;
					ulong llvm_size_in_bits = vectorType.Item.llvm_size_in_bits;
					cil.VecPrimitiveType item = vecPrim.Item;
					uint count = vectorType.Item.count;
					if (llvm_size_in_bits <= 256 && llvm_size_in_bits % 64 == 0)
					{
						ulong llvm_size_in_bits2 = vectorType.Item.llvm_size_in_bits;
						cil.VecPrimitiveType item2 = vecPrim.Item;
						uint count2 = vectorType.Item.count;
						object obj;
						switch (get_opcode.Invoke(op))
						{
						case LLVMOpcode.LLVMAdd:
							obj = "add";
							break;
						case LLVMOpcode.LLVMMul:
							obj = "mul";
							break;
						case LLVMOpcode.LLVMSDiv:
							obj = "div";
							break;
						case LLVMOpcode.LLVMSRem:
							obj = "rem";
							break;
						case LLVMOpcode.LLVMAnd:
							obj = "and";
							break;
						case LLVMOpcode.LLVMOr:
							obj = "or";
							break;
						case LLVMOpcode.LLVMXor:
							obj = "xor";
							break;
						case LLVMOpcode.LLVMSub:
							obj = "sub";
							break;
						case LLVMOpcode.LLVMFAdd:
							obj = "add";
							break;
						case LLVMOpcode.LLVMFMul:
							obj = "mul";
							break;
						case LLVMOpcode.LLVMFDiv:
							obj = "div";
							break;
						case LLVMOpcode.LLVMFSub:
							obj = "sub";
							break;
						case LLVMOpcode.LLVMFRem:
							obj = "rem";
							break;
						case LLVMOpcode.LLVMAShr:
							obj = "ashr";
							break;
						case LLVMOpcode.LLVMShl:
							obj = "shl";
							break;
						case LLVMOpcode.LLVMUDiv:
							obj = "udiv";
							break;
						case LLVMOpcode.LLVMURem:
							obj = "urem";
							break;
						case LLVMOpcode.LLVMLShr:
							obj = "lshr";
							break;
						default:
						{
							FSharpFunc<LLVMValueRef, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, string>, Unit, string, string, LLVMValueRef>("simple_binop unknown op: %A"));
							LLVMValueRef func = op;
							obj = fSharpFunc.Invoke(func);
							break;
						}
						}
						string text = (string)obj;
						string text2 = item2.Tag switch
						{
							1 => "i16", 
							2 => "i32", 
							3 => "i64", 
							4 => "f32", 
							5 => "f64", 
							_ => "i8", 
						};
						load_value.Invoke(op.GetOperand(0u));
						load_value.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<uint, string, string>>("v%P()%P()_%P()", new object[3] { count2, text2, text }, null)))));
						return save_op_result_from_stack.Invoke(null);
					}
				}
			}
			if (firstClassType2.Tag == 1)
			{
				cil.FirstClassType.VectorType vectorType2 = (cil.FirstClassType.VectorType)firstClassType2;
				switch (vectorType2.Item.elemtype.Tag)
				{
				case 2:
				{
					cil.VectorElementType.VecPrim vecPrim2 = (cil.VectorElementType.VecPrim)vectorType2.Item.elemtype;
					cil.VecPrimitiveType item5 = vecPrim2.Item;
					uint count5 = vectorType2.Item.count;
					FSharpFunc<LLVMValueRef, Unit> fSharpFunc4 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, TextWriter, Unit, Unit, LLVMValueRef>("SAD %A"));
					LLVMValueRef func6 = op;
					fSharpFunc4.Invoke(func6);
					int elem_size2 = cil.sizeof_vecprim(item5);
					AddressOfValue addressOfValue3 = my_prep_address_of_value.Invoke(op.GetOperand(0u));
					AddressOfValue addressOfValue4 = my_prep_address_of_value.Invoke(op.GetOperand(1u));
					ResultLocal func7 = get_result_local.Invoke(null);
					FSharpFunc<int, Unit> add_offset2 = new add_offset@1631(il);
					cil.MyInstruction opcode2 = simple_binop_primitive_opcode.Invoke(op);
					FSharpTypeFunc fSharpTypeFunc2 = new get_nth_val@1637(il, load_vecprim_to_stack, item5, elem_size2, addressOfValue3, addressOfValue4, add_offset2, opcode2);
					do_vec_into_local(il, cil.vecprim_to_prim(item5), count5, grab_result_local.Invoke(func7), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc2.Specialize<cil.CilWriter>());
					release_address_of_value(il, addressOfValue3);
					release_address_of_value(il, addressOfValue4);
					return finish_result_local.Invoke(func7);
				}
				case 3:
				{
					cil.VectorElementType.VecFunny vecFunny = (cil.VectorElementType.VecFunny)vectorType2.Item.elemtype;
					cil.FunnyIntegerType item3 = vecFunny.Item;
					uint count4 = vectorType2.Item.count;
					int num = cil.sizeof_funnyinteger_rounded_up(item3);
					AddressOfValue addr = my_prep_address_of_value.Invoke(op.GetOperand(0u));
					AddressOfValue addr2 = my_prep_address_of_value.Invoke(op.GetOperand(1u));
					ResultLocal func4 = get_result_local.Invoke(null);
					FSharpFunc<int, Unit> fSharpFunc3 = new add_offset@1697-2(il);
					MethodReference item4 = get_funny_method_ref.Invoke(op);
					int num2 = 0;
					int num3 = (int)(count4 - 1);
					if (num3 >= num2)
					{
						do
						{
							int func5 = num2 * num;
							il.Append(cil.MyInstruction.NewLdc_I4(num));
							emit_address_of_value(il, addr);
							fSharpFunc3.Invoke(func5);
							emit_address_of_value(il, addr2);
							fSharpFunc3.Invoke(func5);
							ldloca_result_local.Invoke(func4);
							fSharpFunc3.Invoke(func5);
							il.Append(cil.MyInstruction.NewCall(item4));
							num2++;
						}
						while (num2 != num3 + 1);
					}
					release_address_of_value(il, addr);
					release_address_of_value(il, addr2);
					return finish_result_local.Invoke(func4);
				}
				case 0:
				{
					uint count3 = vectorType2.Item.count;
					FSharpFunc<LLVMValueRef, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, TextWriter, Unit, Unit, LLVMValueRef>("BIT %A"));
					LLVMValueRef func2 = op;
					fSharpFunc2.Invoke(func2);
					int elem_size = 1;
					AddressOfValue addressOfValue = my_prep_address_of_value.Invoke(op.GetOperand(0u));
					AddressOfValue addressOfValue2 = my_prep_address_of_value.Invoke(op.GetOperand(1u));
					ResultLocal func3 = get_result_local.Invoke(null);
					FSharpFunc<int, Unit> add_offset = new add_offset@1664-1(il);
					cil.MyInstruction opcode = simple_binop_primitive_opcode.Invoke(op);
					FSharpTypeFunc fSharpTypeFunc = new get_nth_val@1670-1(il, load_mem_primitive_to_stack, elem_size, addressOfValue, addressOfValue2, add_offset, opcode);
					do_vec_into_local(il, cil.PrimitiveType.I1, count3, grab_result_local.Invoke(func3), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc.Specialize<cil.CilWriter>());
					release_address_of_value(il, addressOfValue);
					release_address_of_value(il, addressOfValue2);
					return finish_result_local.Invoke(func3);
				}
				}
			}
			FSharpFunc<LLVMValueRef, Unit> fSharpFunc5 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, Unit, string, Unit, LLVMValueRef>("match case : %A"));
			LLVMValueRef func8 = op;
			return fSharpFunc5.Invoke(func8);
		}
	}

	[Serializable]
	internal sealed class simple_binop_prim@1727 : FSharpFunc<LLVMValueRef, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		public FSharpFunc<Unit, Unit> save_op_result_from_stack;

		public FSharpFunc<LLVMValueRef, cil.MyInstruction> simple_binop_primitive_opcode;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal simple_binop_prim@1727(cil.CilWriter il, FSharpFunc<LLVMValueRef, Unit> load_value, FSharpFunc<Unit, Unit> save_op_result_from_stack, FSharpFunc<LLVMValueRef, cil.MyInstruction> simple_binop_primitive_opcode)
		{
			this.il = il;
			this.load_value = load_value;
			this.save_op_result_from_stack = save_op_result_from_stack;
			this.simple_binop_primitive_opcode = simple_binop_primitive_opcode;
		}

		public override Unit Invoke(LLVMValueRef op)
		{
			cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(op.TypeOf);
			cil.FirstClassType firstClassType2 = firstClassType;
			if (firstClassType2.Tag == 0)
			{
				cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)firstClassType2;
				cil.MyInstruction instr = simple_binop_primitive_opcode.Invoke(op);
				load_value.Invoke(op.GetOperand(0u));
				load_value.Invoke(op.GetOperand(1u));
				il.Append(instr);
				return save_op_result_from_stack.Invoke(null);
			}
			FSharpFunc<LLVMValueRef, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, Unit, string, Unit, LLVMValueRef>("match case : %A"));
			LLVMValueRef func = op;
			return fSharpFunc.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class throw_if_not_prim@1790 : FSharpFunc<LLVMTypeRef, Unit>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal throw_if_not_prim@1790()
		{
		}

		public override Unit Invoke(LLVMTypeRef t)
		{
			cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(t);
			if (firstClassType.Tag == 0)
			{
				cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)firstClassType;
				return null;
			}
			FSharpFunc<LLVMTypeRef, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMTypeRef, Unit>, Unit, string, Unit, LLVMTypeRef>("Cannot handle %A"));
			return fSharpFunc.Invoke(t);
		}
	}

	[Serializable]
	internal sealed class gen@2484 : FSharpFunc<MethodReference, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		public FSharpFunc<ResultLocal, Unit> ldloca_result_local;

		public ResultLocal resloc;

		public cil.StructType styp;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen@2484(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, Unit> load_value, FSharpFunc<ResultLocal, Unit> ldloca_result_local, ResultLocal resloc, cil.StructType styp)
		{
			this.il = il;
			this.op = op;
			this.load_value = load_value;
			this.ldloca_result_local = ldloca_result_local;
			this.resloc = resloc;
			this.styp = styp;
		}

		public override Unit Invoke(MethodReference mref)
		{
			LLVMValueRef operand = op.GetOperand(0u);
			LLVMValueRef operand2 = op.GetOperand(1u);
			ldloca_result_local.Invoke(resloc);
			il.Append(cil.MyInstruction.NewLdc_I4((int)styp.items[0].off));
			il.Append(cil.MyInstruction.Add);
			ldloca_result_local.Invoke(resloc);
			il.Append(cil.MyInstruction.NewLdc_I4((int)styp.items[1].off));
			il.Append(cil.MyInstruction.Add);
			load_value.Invoke(operand);
			load_value.Invoke(operand2);
			il.Append(cil.MyInstruction.NewCall(mref));
			return null;
		}
	}

	[Serializable]
	internal sealed class gen_strange@2506 : FSharpFunc<MethodReference, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public FSharpFunc<ResultLocal, Unit> ldloca_result_local;

		public ResultLocal resloc;

		public cil.StructType styp;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_strange@2506(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, FSharpFunc<ResultLocal, Unit> ldloca_result_local, ResultLocal resloc, cil.StructType styp)
		{
			this.il = il;
			this.op = op;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.ldloca_result_local = ldloca_result_local;
			this.resloc = resloc;
			this.styp = styp;
		}

		public override Unit Invoke(MethodReference mref)
		{
			LLVMValueRef operand = op.GetOperand(0u);
			LLVMValueRef operand2 = op.GetOperand(1u);
			ldloca_result_local.Invoke(resloc);
			il.Append(cil.MyInstruction.NewLdc_I4((int)styp.items[0].off));
			il.Append(cil.MyInstruction.Add);
			ldloca_result_local.Invoke(resloc);
			il.Append(cil.MyInstruction.NewLdc_I4((int)styp.items[1].off));
			il.Append(cil.MyInstruction.Add);
			AddressOfValue addr = my_prep_address_of_value.Invoke(operand);
			AddressOfValue addr2 = my_prep_address_of_value.Invoke(operand2);
			emit_address_of_value(il, addr);
			emit_address_of_value(il, addr2);
			il.Append(cil.MyInstruction.NewCall(mref));
			release_address_of_value(il, addr);
			release_address_of_value(il, addr2);
			return null;
		}
	}

	[Serializable]
	internal sealed class gen@2577-1 : FSharpFunc<MethodReference, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, Unit> ldloca_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen@2577-1(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, Unit> load_value, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, Unit> ldloca_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local)
		{
			this.il = il;
			this.op = op;
			this.load_value = load_value;
			this.get_result_local = get_result_local;
			this.ldloca_result_local = ldloca_result_local;
			this.finish_result_local = finish_result_local;
		}

		public override Unit Invoke(MethodReference mref)
		{
			LLVMValueRef operand = op.GetOperand(0u);
			LLVMValueRef operand2 = op.GetOperand(1u);
			ResultLocal func = get_result_local.Invoke(null);
			ldloca_result_local.Invoke(func);
			load_value.Invoke(operand);
			load_value.Invoke(operand2);
			il.Append(cil.MyInstruction.NewCall(mref));
			return finish_result_local.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class gen_vec@2588 : FSharpFunc<MethodReference, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		public FSharpFunc<Unit, Unit> save_op_result_from_stack;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_vec@2588(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, Unit> load_value, FSharpFunc<Unit, Unit> save_op_result_from_stack)
		{
			this.il = il;
			this.op = op;
			this.load_value = load_value;
			this.save_op_result_from_stack = save_op_result_from_stack;
		}

		public override Unit Invoke(MethodReference mref)
		{
			LLVMValueRef operand = op.GetOperand(0u);
			LLVMValueRef operand2 = op.GetOperand(1u);
			load_value.Invoke(operand);
			load_value.Invoke(operand2);
			il.Append(cil.MyInstruction.NewCall(mref));
			return save_op_result_from_stack.Invoke(null);
		}
	}

	[Serializable]
	internal sealed class do_bswap@2702 : OptimizedClosures.FSharpFunc<int, FSharpFunc<Unit, Unit>, FSharpFunc<Unit, Unit>, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal do_bswap@2702(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int num_bytes, FSharpFunc<Unit, Unit> f_load_src, FSharpFunc<Unit, Unit> f_load_dst)
		{
			int num = 0;
			int num2 = num_bytes - 1;
			if (num2 >= num)
			{
				do
				{
					f_load_dst.Invoke(null);
					int num3 = num_bytes - 1 - num;
					int num4 = num;
					if (num3 != 0)
					{
						il.Append(cil.MyInstruction.NewLdc_I4(num3));
						il.Append(cil.MyInstruction.Add);
					}
					f_load_src.Invoke(null);
					if (num4 != 0)
					{
						il.Append(cil.MyInstruction.NewLdc_I4(num4));
						il.Append(cil.MyInstruction.Add);
					}
					il.Append(cil.MyInstruction.Ldind_I1);
					il.Append(cil.MyInstruction.Stind_I1);
					num++;
				}
				while (num != num2 + 1);
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class f_load_dst@2740 : FSharpFunc<Unit, Unit>
	{
		public FSharpFunc<ResultLocal, Unit> ldloca_result_local;

		public ResultLocal resloc;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_load_dst@2740(FSharpFunc<ResultLocal, Unit> ldloca_result_local, ResultLocal resloc)
		{
			this.ldloca_result_local = ldloca_result_local;
			this.resloc = resloc;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			return ldloca_result_local.Invoke(resloc);
		}
	}

	[Serializable]
	internal sealed class f_load_src@2743-2 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public AddressOfValue v;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_load_src@2743-2(cil.CilWriter il, AddressOfValue v)
		{
			this.il = il;
			this.v = v;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			emit_address_of_value(il, v);
			return null;
		}
	}

	[Serializable]
	internal sealed class f_load_dst@2750-1 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<ResultLocal, Unit> ldloca_result_local;

		public ResultLocal resloc;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_load_dst@2750-1(cil.CilWriter il, FSharpFunc<ResultLocal, Unit> ldloca_result_local, ResultLocal resloc)
		{
			this.il = il;
			this.ldloca_result_local = ldloca_result_local;
			this.resloc = resloc;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			ldloca_result_local.Invoke(resloc);
			il.Append(cil.MyInstruction.NewLdc_I4(4));
			il.Append(cil.MyInstruction.Add);
			return null;
		}
	}

	[Serializable]
	internal sealed class f_load_src@2755-3 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public AddressOfValue v;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_load_src@2755-3(cil.CilWriter il, AddressOfValue v)
		{
			this.il = il;
			this.v = v;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			emit_address_of_value(il, v);
			il.Append(cil.MyInstruction.NewLdc_I4(4));
			il.Append(cil.MyInstruction.Add);
			return null;
		}
	}

	[Serializable]
	internal sealed class save_call_result@2803 : FSharpFunc<Unit, Unit>
	{
		public FSharpFunc<Unit, Unit> save_op_result_from_stack;

		public FSharpOption<LLVMTypeRef> return_value;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal save_call_result@2803(FSharpFunc<Unit, Unit> save_op_result_from_stack, FSharpOption<LLVMTypeRef> return_value)
		{
			this.save_op_result_from_stack = save_op_result_from_stack;
			this.return_value = return_value;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			FSharpOption<LLVMTypeRef> fSharpOption = return_value;
			if (fSharpOption == null)
			{
				return null;
			}
			FSharpOption<LLVMTypeRef> fSharpOption2 = fSharpOption;
			return save_op_result_from_stack.Invoke(null);
		}
	}

	[Serializable]
	internal sealed class push_params@2819 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		public int count_fixed_params;

		public int count_extra_params;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal push_params@2819(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, Unit> load_value, int count_fixed_params, int count_extra_params)
		{
			this.il = il;
			this.op = op;
			this.load_value = load_value;
			this.count_fixed_params = count_fixed_params;
			this.count_extra_params = count_extra_params;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			if (count_fixed_params > 0)
			{
				int num = 0;
				int num2 = count_fixed_params - 1;
				if (num2 >= num)
				{
					do
					{
						LLVMValueRef operand = op.GetOperand((uint)num);
						load_value.Invoke(operand);
						num++;
					}
					while (num != num2 + 1);
				}
			}
			if (count_extra_params > 0)
			{
				cil.Variable variable = il.NewVariable_VarArg(count_extra_params);
				init_local(il, variable);
				il.Append(cil.MyInstruction.NewLdloca(variable));
				il.Append(cil.MyInstruction.NewLdc_I8(count_extra_params));
				il.Append(cil.MyInstruction.Stind_I8);
				int num3 = 0;
				int num4 = count_extra_params - 1;
				if (num4 >= num3)
				{
					do
					{
						il.Append(cil.MyInstruction.NewLdloca(variable));
						il.Append(cil.MyInstruction.NewLdc_I4((num3 + 1) * 8));
						il.Append(cil.MyInstruction.Add);
						LLVMValueRef operand2 = op.GetOperand((uint)(num3 + count_fixed_params));
						load_value.Invoke(operand2);
						cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(operand2.TypeOf);
						switch (firstClassType.Tag)
						{
						case 0:
						{
							cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)firstClassType;
							cil.PrimitiveType item = primitiveType.Item;
							store_mem_primitive_from_stack(il, item);
							break;
						}
						case 5:
							il.Append(cil.MyInstruction.Stind_I);
							break;
						default:
							throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 2842, 38);
						}
						num3++;
					}
					while (num3 != num4 + 1);
				}
				il.Append(cil.MyInstruction.NewLdloca(variable));
				return null;
			}
			il.Append(cil.MyInstruction.NewLdc_I4(0));
			il.Append(cil.MyInstruction.Conv_I);
			return null;
		}
	}

	[Serializable]
	internal sealed class do_regular_call@2856 : FSharpFunc<LLVMValueRef, Unit>
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public FSharpFunc<Unit, Unit> save_call_result;

		public FSharpFunc<Unit, Unit> push_params;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal do_regular_call@2856(GenSyms syms, cil.CilWriter il, FSharpFunc<Unit, Unit> save_call_result, FSharpFunc<Unit, Unit> push_params)
		{
			this.syms = syms;
			this.il = il;
			this.save_call_result = save_call_result;
			this.push_params = push_params;
		}

		public override Unit Invoke(LLVMValueRef f)
		{
			MethodReference methodReference = syms.d_methods@[f];
			MethodReference x = methodReference;
			MethodReference y = default(MethodReference);
			if (!LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(x, y))
			{
				push_params.Invoke(null);
				il.Append(cil.MyInstruction.NewCall(methodReference));
				return save_call_result.Invoke(null);
			}
			FSharpFunc<string, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("Call Missing method a: %s"));
			string name = f.Name;
			string text = fSharpFunc.Invoke(name);
			FSharpFunc<string, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("%s"));
			string func = text;
			fSharpFunc2.Invoke(func);
			il.Append(cil.MyInstruction.NewLdstr(text));
			il.Append(cil.MyInstruction.NewNewobj(syms.ctor_exception@));
			il.Append(cil.MyInstruction.Throw);
			return null;
		}
	}

	[Serializable]
	internal sealed class do_indirect_call@2868 : FSharpFunc<LLVMValueRef, Unit>
	{
		public cil.GenTypes typs;

		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		public FSharpOption<LLVMTypeRef> return_value;

		public FSharpFunc<Unit, Unit> save_call_result;

		public int count_fixed_params;

		public int count_extra_params;

		public FSharpFunc<Unit, Unit> push_params;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal do_indirect_call@2868(cil.GenTypes typs, cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, Unit> load_value, FSharpOption<LLVMTypeRef> return_value, FSharpFunc<Unit, Unit> save_call_result, int count_fixed_params, int count_extra_params, FSharpFunc<Unit, Unit> push_params)
		{
			this.typs = typs;
			this.il = il;
			this.op = op;
			this.load_value = load_value;
			this.return_value = return_value;
			this.save_call_result = save_call_result;
			this.count_fixed_params = count_fixed_params;
			this.count_extra_params = count_extra_params;
			this.push_params = push_params;
		}

		public override Unit Invoke(LLVMValueRef f)
		{
			FSharpOption<LLVMTypeRef> fSharpOption = return_value;
			Mono.Cecil.CallSite callSite;
			if (fSharpOption == null)
			{
				callSite = new Mono.Cecil.CallSite(typs.md.TypeSystem.Void);
			}
			else
			{
				FSharpOption<LLVMTypeRef> fSharpOption2 = fSharpOption;
				LLVMTypeRef value = fSharpOption2.Value;
				cil.FirstClassType vt = llvm_type_to_firstclass_type(value);
				TypeReference returnType = cil.firstclass_type_to_cecil_type(typs, vt);
				callSite = new Mono.Cecil.CallSite(returnType);
			}
			Mono.Cecil.CallSite callSite2 = callSite;
			if (count_fixed_params > 0)
			{
				int num = 0;
				int num2 = count_fixed_params - 1;
				if (num2 >= num)
				{
					do
					{
						cil.FirstClassType vt2 = llvm_type_to_firstclass_type(op.GetOperand((uint)num).TypeOf);
						TypeReference parameterType = cil.firstclass_type_to_cecil_type(typs, vt2);
						callSite2.Parameters.Add(new ParameterDefinition(parameterType));
						num++;
					}
					while (num != num2 + 1);
				}
			}
			if (count_extra_params > 0)
			{
				TypeReference parameterType2 = cil.firstclass_type_to_cecil_type(typs, cil.FirstClassType.Ptr);
				callSite2.Parameters.Add(new ParameterDefinition(parameterType2));
			}
			Mono.Cecil.CallSite item = callSite2;
			push_params.Invoke(null);
			load_value.Invoke(f);
			il.Append(cil.MyInstruction.NewCalli(item));
			return save_call_result.Invoke(null);
		}
	}

	[Serializable]
	internal sealed class push_params@2902-1 : FSharpFunc<Unit, Unit>
	{
		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal push_params@2902-1(LLVMValueRef op, FSharpFunc<LLVMValueRef, Unit> load_value)
		{
			this.op = op;
			this.load_value = load_value;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			int num = 0;
			int num2 = op.OperandCount - 2;
			if (num2 >= num)
			{
				do
				{
					LLVMValueRef operand = op.GetOperand((uint)num);
					load_value.Invoke(operand);
					num++;
				}
				while (num != num2 + 1);
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class do_regular_call@2908-1 : FSharpFunc<LLVMValueRef, Unit>
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<Unit, Unit> save_call_result;

		public FSharpFunc<Unit, Unit> push_params;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal do_regular_call@2908-1(GenSyms syms, cil.CilWriter il, LLVMValueRef op, FSharpFunc<Unit, Unit> save_call_result, FSharpFunc<Unit, Unit> push_params)
		{
			this.syms = syms;
			this.il = il;
			this.op = op;
			this.save_call_result = save_call_result;
			this.push_params = push_params;
		}

		public override Unit Invoke(LLVMValueRef f)
		{
			MethodReference methodReference = syms.d_methods@[f];
			MethodReference x = methodReference;
			MethodReference y = default(MethodReference);
			if (!LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(x, y))
			{
				if (f.Params.Length != methodReference.Parameters.Count)
				{
					FSharpFunc<LLVMValueRef, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, Unit, string, Unit, LLVMValueRef>("mismatch parameter count: %A"));
					LLVMValueRef func = op;
					fSharpFunc.Invoke(func);
				}
				push_params.Invoke(null);
				il.Append(cil.MyInstruction.NewCall(methodReference));
				return save_call_result.Invoke(null);
			}
			FSharpFunc<string, string> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("Call Missing method b: %s"));
			string name = f.Name;
			string text = fSharpFunc2.Invoke(name);
			FSharpFunc<string, Unit> fSharpFunc3 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("%s"));
			string func2 = text;
			fSharpFunc3.Invoke(func2);
			il.Append(cil.MyInstruction.NewLdstr(text));
			il.Append(cil.MyInstruction.NewNewobj(syms.ctor_exception@));
			il.Append(cil.MyInstruction.Throw);
			return null;
		}
	}

	[Serializable]
	internal sealed class do_indirect_call@2922-1 : FSharpFunc<LLVMValueRef, Unit>
	{
		public cil.GenTypes typs;

		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		public FSharpOption<LLVMTypeRef> return_value;

		public FSharpFunc<Unit, Unit> save_call_result;

		public FSharpFunc<Unit, Unit> push_params;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal do_indirect_call@2922-1(cil.GenTypes typs, cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, Unit> load_value, FSharpOption<LLVMTypeRef> return_value, FSharpFunc<Unit, Unit> save_call_result, FSharpFunc<Unit, Unit> push_params)
		{
			this.typs = typs;
			this.il = il;
			this.op = op;
			this.load_value = load_value;
			this.return_value = return_value;
			this.save_call_result = save_call_result;
			this.push_params = push_params;
		}

		public override Unit Invoke(LLVMValueRef f)
		{
			FSharpOption<LLVMTypeRef> fSharpOption = return_value;
			Mono.Cecil.CallSite callSite;
			if (fSharpOption == null)
			{
				callSite = new Mono.Cecil.CallSite(typs.md.TypeSystem.Void);
			}
			else
			{
				FSharpOption<LLVMTypeRef> fSharpOption2 = fSharpOption;
				LLVMTypeRef value = fSharpOption2.Value;
				cil.FirstClassType vt = llvm_type_to_firstclass_type(value);
				TypeReference returnType = cil.firstclass_type_to_cecil_type(typs, vt);
				callSite = new Mono.Cecil.CallSite(returnType);
			}
			Mono.Cecil.CallSite callSite2 = callSite;
			int num = 0;
			int num2 = op.OperandCount - 2;
			if (num2 >= num)
			{
				do
				{
					cil.FirstClassType vt2 = llvm_type_to_firstclass_type(op.GetOperand((uint)num).TypeOf);
					TypeReference parameterType = cil.firstclass_type_to_cecil_type(typs, vt2);
					callSite2.Parameters.Add(new ParameterDefinition(parameterType));
					num++;
				}
				while (num != num2 + 1);
			}
			Mono.Cecil.CallSite item = callSite2;
			push_params.Invoke(null);
			load_value.Invoke(f);
			il.Append(cil.MyInstruction.NewCalli(item));
			return save_call_result.Invoke(null);
		}
	}

	[Serializable]
	internal sealed class do_single_offset@2964 : OptimizedClosures.FSharpFunc<cil.FirstClassType, LLVMValueRef, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal do_single_offset@2964(cil.CilWriter il, FSharpFunc<LLVMValueRef, Unit> load_value)
		{
			this.il = il;
			this.load_value = load_value;
		}

		public override Unit Invoke(cil.FirstClassType elem_type, LLVMValueRef v)
		{
			FSharpOption<long> fSharpOption = get_maybe_integer_constant(v);
			if (fSharpOption != null)
			{
				FSharpOption<long> fSharpOption2 = fSharpOption;
				long value = fSharpOption2.Value;
				int num = (int)value;
				int num2 = cil.get_sizeof(elem_type) * num;
				if (num2 != 0)
				{
					il.Append(cil.MyInstruction.NewLdc_I4(num2));
					il.Append(cil.MyInstruction.Add);
				}
				il.Append(cil.MyInstruction.Conv_I);
				return null;
			}
			load_value.Invoke(v);
			int num3 = cil.get_sizeof(elem_type);
			if (num3 == 0)
			{
				string message = "TODO";
				throw Operators.Failure(message);
			}
			il.Append(cil.MyInstruction.NewLdc_I4(num3));
			il.Append(cil.MyInstruction.Mul);
			il.Append(cil.MyInstruction.Add);
			il.Append(cil.MyInstruction.Conv_I);
			return null;
		}
	}

	[Serializable]
	internal sealed class gen_instr@3001-1 : FSharpFunc<cil.FirstClassType, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<cil.FirstClassType, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_instr@3001-1(FSharpFunc<cil.FirstClassType, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(cil.FirstClassType arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class gen_instr@3001 : FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_instr@3001(FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<cil.FirstClassType, Unit> Invoke(cil.FirstClassType arg10)
		{
			FSharpFunc<cil.FirstClassType, Unit> clo = clo1.Invoke(arg10);
			return new gen_instr@3001-1(clo);
		}
	}

	[Serializable]
	internal sealed class offset_ptr@3024 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public int i;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal offset_ptr@3024(cil.CilWriter il, int i)
		{
			this.il = il;
			this.i = i;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			if (i != 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(i * 8));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3021-2 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> walk_indexes;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public LLVMValueRef[] indexes;

		public LLVMValueRef first;

		public cil.FirstClassType elem_type;

		public AddressOfValue addr_op0;

		public AddressOfValue addr_first;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3021-2(cil.CilWriter il, FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> walk_indexes, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, LLVMValueRef[] indexes, LLVMValueRef first, cil.FirstClassType elem_type, AddressOfValue addr_op0, AddressOfValue addr_first)
		{
			this.il = il;
			this.walk_indexes = walk_indexes;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.indexes = indexes;
			this.first = first;
			this.elem_type = elem_type;
			this.addr_op0 = addr_op0;
			this.addr_first = addr_first;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@3021-2T<a>(il, walk_indexes, load_vecprim_to_stack, indexes, first, elem_type, addr_op0, addr_first, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3021-2T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> walk_indexes;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public LLVMValueRef[] indexes;

		public LLVMValueRef first;

		public cil.FirstClassType elem_type;

		public AddressOfValue addr_op0;

		public AddressOfValue addr_first;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@3021-2 self8@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3021-2T(cil.CilWriter il, FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> walk_indexes, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, LLVMValueRef[] indexes, LLVMValueRef first, cil.FirstClassType elem_type, AddressOfValue addr_op0, AddressOfValue addr_first, get_nth_val@3021-2 self8@)
		{
			this.il = il;
			this.walk_indexes = walk_indexes;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.indexes = indexes;
			this.first = first;
			this.elem_type = elem_type;
			this.addr_op0 = addr_op0;
			this.addr_first = addr_first;
			this.self8@ = self8@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@3021-2 get_nth_val@3021-3 = self8@;
			FSharpFunc<Unit, Unit> fSharpFunc = new offset_ptr@3024(get_nth_val@3021-3.il, i);
			emit_address_of_value(get_nth_val@3021-3.il, get_nth_val@3021-3.addr_op0);
			fSharpFunc.Invoke(null);
			get_nth_val@3021-3.il.Append(cil.MyInstruction.Ldind_I);
			cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(get_nth_val@3021-3.first.TypeOf);
			if (firstClassType.Tag == 1)
			{
				cil.FirstClassType.VectorType vectorType = (cil.FirstClassType.VectorType)firstClassType;
				switch (vectorType.Item.elemtype.Tag)
				{
				case 2:
				{
					cil.VecPrimitiveType item2 = ((cil.VectorElementType.VecPrim)vectorType.Item.elemtype).Item;
					emit_address_of_value(get_nth_val@3021-3.il, get_nth_val@3021-3.addr_first);
					int num = cil.sizeof_vecprim(item2) * i;
					if (num != 0)
					{
						get_nth_val@3021-3.il.Append(cil.MyInstruction.NewLdc_I4(num));
						get_nth_val@3021-3.il.Append(cil.MyInstruction.Add);
					}
					get_nth_val@3021-3.load_vecprim_to_stack.Invoke(item2);
					int num2 = cil.get_sizeof(get_nth_val@3021-3.elem_type);
					if (num2 == 0)
					{
						string message2 = "TODO";
						throw Operators.Failure(message2);
					}
					get_nth_val@3021-3.il.Append(cil.MyInstruction.NewLdc_I4(num2));
					get_nth_val@3021-3.il.Append(cil.MyInstruction.Mul);
					get_nth_val@3021-3.il.Append(cil.MyInstruction.Add);
					get_nth_val@3021-3.il.Append(cil.MyInstruction.Conv_I);
					if (get_nth_val@3021-3.indexes.Length > 0)
					{
						for (int j = 0; j < get_nth_val@3021-3.indexes.Length; j++)
						{
							LLVMValueRef lLVMValueRef = get_nth_val@3021-3.indexes[j];
							cil.FirstClassType firstClassType2 = llvm_type_to_firstclass_type(lLVMValueRef.TypeOf);
							cil.FirstClassType firstClassType3;
							switch (firstClassType2.Tag)
							{
							case 0:
							{
								cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)firstClassType2;
								switch (primitiveType.Item.Tag)
								{
								case 3:
								case 4:
									continue;
								}
								firstClassType3 = firstClassType2;
								break;
							}
							case 1:
							{
								cil.VectorType item3 = ((cil.FirstClassType.VectorType)firstClassType2).Item;
								FSharpFunc<cil.VectorType, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.VectorType, Unit>, Unit, string, Unit, cil.VectorType>("vector in walk_indexes %A"));
								cil.VectorType func = item3;
								fSharpFunc2.Invoke(func);
								continue;
							}
							default:
								firstClassType3 = firstClassType2;
								break;
							}
							FSharpFunc<cil.FirstClassType, Unit> fSharpFunc3 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, Unit>, Unit, string, Unit, cil.FirstClassType>("unknown type for walk_indexes: %A"));
							cil.FirstClassType func2 = firstClassType3;
							fSharpFunc3.Invoke(func2);
						}
						cil.FirstClassType firstClassType4 = FSharpFunc<cil.FirstClassType, LLVMValueRef[]>.InvokeFast(get_nth_val@3021-3.walk_indexes, get_nth_val@3021-3.elem_type, get_nth_val@3021-3.indexes, 0);
						return null;
					}
					return null;
				}
				case 3:
				{
					cil.FunnyIntegerType item = ((cil.VectorElementType.VecFunny)vectorType.Item.elemtype).Item;
					string message = "TODO";
					throw Operators.Failure(message);
				}
				}
			}
			throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 3034, 30);
		}
	}

	[Serializable]
	internal sealed class offset_ptr@3088-1 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public int i;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal offset_ptr@3088-1(cil.CilWriter il, int i)
		{
			this.il = il;
			this.i = i;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			if (i != 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(i * 8));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3085-3 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> walk_indexes;

		public LLVMValueRef[] indexes;

		public LLVMValueRef first;

		public FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> do_single_offset;

		public cil.FirstClassType elem_type;

		public AddressOfValue addr_op0;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3085-3(cil.CilWriter il, FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> walk_indexes, LLVMValueRef[] indexes, LLVMValueRef first, FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> do_single_offset, cil.FirstClassType elem_type, AddressOfValue addr_op0)
		{
			this.il = il;
			this.walk_indexes = walk_indexes;
			this.indexes = indexes;
			this.first = first;
			this.do_single_offset = do_single_offset;
			this.elem_type = elem_type;
			this.addr_op0 = addr_op0;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@3085-3T<a>(il, walk_indexes, indexes, first, do_single_offset, elem_type, addr_op0, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3085-3T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> walk_indexes;

		public LLVMValueRef[] indexes;

		public LLVMValueRef first;

		public FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> do_single_offset;

		public cil.FirstClassType elem_type;

		public AddressOfValue addr_op0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@3085-3 self7@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3085-3T(cil.CilWriter il, FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> walk_indexes, LLVMValueRef[] indexes, LLVMValueRef first, FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> do_single_offset, cil.FirstClassType elem_type, AddressOfValue addr_op0, get_nth_val@3085-3 self7@)
		{
			this.il = il;
			this.walk_indexes = walk_indexes;
			this.indexes = indexes;
			this.first = first;
			this.do_single_offset = do_single_offset;
			this.elem_type = elem_type;
			this.addr_op0 = addr_op0;
			this.self7@ = self7@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@3085-3 get_nth_val@3085-4 = self7@;
			FSharpFunc<Unit, Unit> fSharpFunc = new offset_ptr@3088-1(get_nth_val@3085-4.il, i);
			emit_address_of_value(get_nth_val@3085-4.il, get_nth_val@3085-4.addr_op0);
			fSharpFunc.Invoke(null);
			get_nth_val@3085-4.il.Append(cil.MyInstruction.Ldind_I);
			FSharpFunc<cil.FirstClassType, LLVMValueRef>.InvokeFast(get_nth_val@3085-4.do_single_offset, get_nth_val@3085-4.elem_type, get_nth_val@3085-4.first);
			if (get_nth_val@3085-4.indexes.Length > 0)
			{
				for (int j = 0; j < get_nth_val@3085-4.indexes.Length; j++)
				{
					LLVMValueRef lLVMValueRef = get_nth_val@3085-4.indexes[j];
					cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(lLVMValueRef.TypeOf);
					cil.FirstClassType firstClassType2;
					switch (firstClassType.Tag)
					{
					case 0:
					{
						cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)firstClassType;
						switch (primitiveType.Item.Tag)
						{
						case 3:
						case 4:
							continue;
						}
						firstClassType2 = firstClassType;
						break;
					}
					case 1:
					{
						cil.VectorType item = ((cil.FirstClassType.VectorType)firstClassType).Item;
						FSharpFunc<cil.VectorType, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.VectorType, Unit>, Unit, string, Unit, cil.VectorType>("vector in walk_indexes %A"));
						cil.VectorType func = item;
						fSharpFunc2.Invoke(func);
						continue;
					}
					default:
						firstClassType2 = firstClassType;
						break;
					}
					FSharpFunc<cil.FirstClassType, Unit> fSharpFunc3 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, Unit>, Unit, string, Unit, cil.FirstClassType>("unknown type for walk_indexes: %A"));
					cil.FirstClassType func2 = firstClassType2;
					fSharpFunc3.Invoke(func2);
				}
				cil.FirstClassType firstClassType3 = FSharpFunc<cil.FirstClassType, LLVMValueRef[]>.InvokeFast(get_nth_val@3085-4.walk_indexes, get_nth_val@3085-4.elem_type, get_nth_val@3085-4.indexes, 0);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class offset_ptr@3130-2 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public int i;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal offset_ptr@3130-2(cil.CilWriter il, int i)
		{
			this.il = il;
			this.i = i;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			if (i != 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(i * 8));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3127-4 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		public FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> walk_indexes;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public LLVMValueRef[] indexes;

		public LLVMValueRef op0;

		public LLVMValueRef first;

		public cil.FirstClassType elem_type;

		public AddressOfValue addr_first;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3127-4(cil.CilWriter il, FSharpFunc<LLVMValueRef, Unit> load_value, FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> walk_indexes, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, LLVMValueRef[] indexes, LLVMValueRef op0, LLVMValueRef first, cil.FirstClassType elem_type, AddressOfValue addr_first)
		{
			this.il = il;
			this.load_value = load_value;
			this.walk_indexes = walk_indexes;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.indexes = indexes;
			this.op0 = op0;
			this.first = first;
			this.elem_type = elem_type;
			this.addr_first = addr_first;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@3127-4T<a>(il, load_value, walk_indexes, load_vecprim_to_stack, indexes, op0, first, elem_type, addr_first, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3127-4T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		public FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> walk_indexes;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public LLVMValueRef[] indexes;

		public LLVMValueRef op0;

		public LLVMValueRef first;

		public cil.FirstClassType elem_type;

		public AddressOfValue addr_first;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@3127-4 self9@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3127-4T(cil.CilWriter il, FSharpFunc<LLVMValueRef, Unit> load_value, FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> walk_indexes, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, LLVMValueRef[] indexes, LLVMValueRef op0, LLVMValueRef first, cil.FirstClassType elem_type, AddressOfValue addr_first, get_nth_val@3127-4 self9@)
		{
			this.il = il;
			this.load_value = load_value;
			this.walk_indexes = walk_indexes;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.indexes = indexes;
			this.op0 = op0;
			this.first = first;
			this.elem_type = elem_type;
			this.addr_first = addr_first;
			this.self9@ = self9@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@3127-4 get_nth_val@3127-5 = self9@;
			FSharpFunc<Unit, Unit> fSharpFunc = new offset_ptr@3130-2(get_nth_val@3127-5.il, i);
			get_nth_val@3127-5.load_value.Invoke(get_nth_val@3127-5.op0);
			cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(get_nth_val@3127-5.first.TypeOf);
			if (firstClassType.Tag == 1)
			{
				cil.FirstClassType.VectorType vectorType = (cil.FirstClassType.VectorType)firstClassType;
				switch (vectorType.Item.elemtype.Tag)
				{
				case 2:
				{
					cil.VecPrimitiveType item2 = ((cil.VectorElementType.VecPrim)vectorType.Item.elemtype).Item;
					emit_address_of_value(get_nth_val@3127-5.il, get_nth_val@3127-5.addr_first);
					int num = cil.sizeof_vecprim(item2) * i;
					if (num != 0)
					{
						get_nth_val@3127-5.il.Append(cil.MyInstruction.NewLdc_I4(num));
						get_nth_val@3127-5.il.Append(cil.MyInstruction.Add);
					}
					get_nth_val@3127-5.load_vecprim_to_stack.Invoke(item2);
					int num2 = cil.get_sizeof(get_nth_val@3127-5.elem_type);
					if (num2 == 0)
					{
						string message2 = "TODO";
						throw Operators.Failure(message2);
					}
					get_nth_val@3127-5.il.Append(cil.MyInstruction.NewLdc_I4(num2));
					get_nth_val@3127-5.il.Append(cil.MyInstruction.Mul);
					get_nth_val@3127-5.il.Append(cil.MyInstruction.Add);
					get_nth_val@3127-5.il.Append(cil.MyInstruction.Conv_I);
					if (get_nth_val@3127-5.indexes.Length > 0)
					{
						for (int j = 0; j < get_nth_val@3127-5.indexes.Length; j++)
						{
							LLVMValueRef lLVMValueRef = get_nth_val@3127-5.indexes[j];
							cil.FirstClassType firstClassType2 = llvm_type_to_firstclass_type(lLVMValueRef.TypeOf);
							cil.FirstClassType firstClassType3;
							switch (firstClassType2.Tag)
							{
							case 0:
							{
								cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)firstClassType2;
								switch (primitiveType.Item.Tag)
								{
								case 3:
								case 4:
									continue;
								}
								firstClassType3 = firstClassType2;
								break;
							}
							case 1:
							{
								cil.VectorType item3 = ((cil.FirstClassType.VectorType)firstClassType2).Item;
								FSharpFunc<cil.VectorType, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.VectorType, Unit>, Unit, string, Unit, cil.VectorType>("vector in walk_indexes %A"));
								cil.VectorType func = item3;
								fSharpFunc2.Invoke(func);
								continue;
							}
							default:
								firstClassType3 = firstClassType2;
								break;
							}
							FSharpFunc<cil.FirstClassType, Unit> fSharpFunc3 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, Unit>, Unit, string, Unit, cil.FirstClassType>("unknown type for walk_indexes: %A"));
							cil.FirstClassType func2 = firstClassType3;
							fSharpFunc3.Invoke(func2);
						}
						cil.FirstClassType firstClassType4 = FSharpFunc<cil.FirstClassType, LLVMValueRef[]>.InvokeFast(get_nth_val@3127-5.walk_indexes, get_nth_val@3127-5.elem_type, get_nth_val@3127-5.indexes, 0);
						return null;
					}
					return null;
				}
				case 3:
				{
					cil.FunnyIntegerType item = ((cil.VectorElementType.VecFunny)vectorType.Item.elemtype).Item;
					string message = "TODO";
					throw Operators.Failure(message);
				}
				}
			}
			throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 3138, 30);
		}
	}

	[Serializable]
	internal sealed class add_offset@3198-3 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@3198-3(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3205-5 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public FSharpFunc<cil.PrimitiveType, Unit> load_mem_primitive_to_stack;

		public cil.VecPrimitiveType elemtype;

		public int elem_size;

		public AddressOfValue v0;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public FSharpFunc<int, Unit> add_offset;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3205-5(cil.CilWriter il, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, FSharpFunc<cil.PrimitiveType, Unit> load_mem_primitive_to_stack, cil.VecPrimitiveType elemtype, int elem_size, AddressOfValue v0, AddressOfValue v1, AddressOfValue v2, FSharpFunc<int, Unit> add_offset)
		{
			this.il = il;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.load_mem_primitive_to_stack = load_mem_primitive_to_stack;
			this.elemtype = elemtype;
			this.elem_size = elem_size;
			this.v0 = v0;
			this.v1 = v1;
			this.v2 = v2;
			this.add_offset = add_offset;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@3205-5T<a>(il, load_vecprim_to_stack, load_mem_primitive_to_stack, elemtype, elem_size, v0, v1, v2, add_offset, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3205-5T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public FSharpFunc<cil.PrimitiveType, Unit> load_mem_primitive_to_stack;

		public cil.VecPrimitiveType elemtype;

		public int elem_size;

		public AddressOfValue v0;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public FSharpFunc<int, Unit> add_offset;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@3205-5 self9@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3205-5T(cil.CilWriter il, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, FSharpFunc<cil.PrimitiveType, Unit> load_mem_primitive_to_stack, cil.VecPrimitiveType elemtype, int elem_size, AddressOfValue v0, AddressOfValue v1, AddressOfValue v2, FSharpFunc<int, Unit> add_offset, get_nth_val@3205-5 self9@)
		{
			this.il = il;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.load_mem_primitive_to_stack = load_mem_primitive_to_stack;
			this.elemtype = elemtype;
			this.elem_size = elem_size;
			this.v0 = v0;
			this.v1 = v1;
			this.v2 = v2;
			this.add_offset = add_offset;
			this.self9@ = self9@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@3205-5 get_nth_val@3205-6 = self9@;
			int func = i * get_nth_val@3205-6.elem_size;
			emit_address_of_value(get_nth_val@3205-6.il, get_nth_val@3205-6.v0);
			get_nth_val@3205-6.add_offset.Invoke(i);
			get_nth_val@3205-6.load_mem_primitive_to_stack.Invoke(cil.PrimitiveType.I1);
			cil.Label item = get_nth_val@3205-6.il.NewLabel();
			cil.Label item2 = get_nth_val@3205-6.il.NewLabel();
			get_nth_val@3205-6.il.Append(cil.MyInstruction.NewBrfalse(item));
			emit_address_of_value(get_nth_val@3205-6.il, get_nth_val@3205-6.v1);
			get_nth_val@3205-6.add_offset.Invoke(func);
			get_nth_val@3205-6.load_vecprim_to_stack.Invoke(get_nth_val@3205-6.elemtype);
			get_nth_val@3205-6.il.Append(cil.MyInstruction.NewBr(item2));
			get_nth_val@3205-6.il.Append(cil.MyInstruction.NewLabel(item));
			emit_address_of_value(get_nth_val@3205-6.il, get_nth_val@3205-6.v2);
			get_nth_val@3205-6.add_offset.Invoke(func);
			get_nth_val@3205-6.load_vecprim_to_stack.Invoke(get_nth_val@3205-6.elemtype);
			get_nth_val@3205-6.il.Append(cil.MyInstruction.NewLabel(item2));
			return null;
		}
	}

	[Serializable]
	internal sealed class add_offset@3240-4 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@3240-4(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class load_as_u64@3308 : FSharpFunc<LLVMValueRef, Unit>
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public FSharpFunc<LLVMValueRef, Unit> load_value;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public cil.FirstClassType typ;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_as_u64@3308(GenSyms syms, cil.CilWriter il, FSharpFunc<LLVMValueRef, Unit> load_value, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, cil.FirstClassType typ)
		{
			this.syms = syms;
			this.il = il;
			this.load_value = load_value;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.typ = typ;
		}

		public override Unit Invoke(LLVMValueRef v)
		{
			cil.FirstClassType firstClassType = typ;
			switch (firstClassType.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)firstClassType;
				load_value.Invoke(v);
				il.Append(cil.MyInstruction.Conv_U8);
				return null;
			}
			case 4:
			{
				cil.FirstClassType.FunnyIntegerType funnyIntegerType = (cil.FirstClassType.FunnyIntegerType)firstClassType;
				cil.FunnyIntegerType item = funnyIntegerType.Item;
				il.Append(cil.MyInstruction.NewLdc_I4(item.bits));
				AddressOfValue addr = my_prep_address_of_value.Invoke(v);
				emit_address_of_value(il, addr);
				il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("part_to_u64")));
				release_address_of_value(il, addr);
				return null;
			}
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 3308, 22);
			}
		}
	}

	[Serializable]
	internal sealed class do_shuffle@3397 : OptimizedClosures.FSharpFunc<int, FSharpFunc<Unit, Unit>, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<ResultLocal, Unit> ldloca_result_local;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public int[] a_mask;

		public int v1_count;

		public ResultLocal resloc;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal do_shuffle@3397(cil.CilWriter il, FSharpFunc<ResultLocal, Unit> ldloca_result_local, AddressOfValue v1, AddressOfValue v2, int[] a_mask, int v1_count, ResultLocal resloc)
		{
			this.il = il;
			this.ldloca_result_local = ldloca_result_local;
			this.v1 = v1;
			this.v2 = v2;
			this.a_mask = a_mask;
			this.v1_count = v1_count;
			this.resloc = resloc;
		}

		public override Unit Invoke(int elem_size, FSharpFunc<Unit, Unit> f)
		{
			for (int i = 0; i < a_mask.Length; i++)
			{
				int num = a_mask[i];
				if (num < 0)
				{
					continue;
				}
				ldloca_result_local.Invoke(resloc);
				if (i > 0)
				{
					il.Append(cil.MyInstruction.NewLdc_I4(i * elem_size));
					il.Append(cil.MyInstruction.Add);
				}
				if (num < v1_count)
				{
					emit_address_of_value(il, v1);
					if (num > 0)
					{
						il.Append(cil.MyInstruction.NewLdc_I4(num * elem_size));
						il.Append(cil.MyInstruction.Add);
					}
				}
				else
				{
					emit_address_of_value(il, v2);
					int num2 = num - v1_count;
					if (num2 > 0)
					{
						il.Append(cil.MyInstruction.NewLdc_I4(num2 * elem_size));
						il.Append(cil.MyInstruction.Add);
					}
				}
				f.Invoke(null);
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class f@3426 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public cil.VecPrimitiveType elemtype;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@3426(cil.CilWriter il, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, cil.VecPrimitiveType elemtype)
		{
			this.il = il;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.elemtype = elemtype;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			load_vecprim_to_stack.Invoke(elemtype);
			store_vecprim_from_stack(il, elemtype);
			return null;
		}
	}

	[Serializable]
	internal sealed class f@3436-1 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public int elem_size;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@3436-1(cil.CilWriter il, int elem_size)
		{
			this.il = il;
			this.elem_size = elem_size;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			il.Append(cil.MyInstruction.NewLdc_I4(elem_size));
			il.Append(cil.MyInstruction.Cpblk);
			return null;
		}
	}

	[Serializable]
	internal sealed class f@3445-2 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@3445-2(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			il.Append(cil.MyInstruction.Ldind_I1);
			il.Append(cil.MyInstruction.Stind_I1);
			return null;
		}
	}

	[Serializable]
	internal sealed class f@3454-3 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@3454-3(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			il.Append(cil.MyInstruction.Ldind_I);
			il.Append(cil.MyInstruction.Stind_I);
			return null;
		}
	}

	[Serializable]
	internal sealed class add_offset@3595-5 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@3595-5(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3599-6 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public cil.VecPrimitiveType elemtype;

		public int elem_size;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public FSharpFunc<int, Unit> add_offset;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3599-6(cil.CilWriter il, LLVMValueRef op, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, cil.VecPrimitiveType elemtype, int elem_size, AddressOfValue v1, AddressOfValue v2, FSharpFunc<int, Unit> add_offset)
		{
			this.il = il;
			this.op = op;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.elemtype = elemtype;
			this.elem_size = elem_size;
			this.v1 = v1;
			this.v2 = v2;
			this.add_offset = add_offset;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@3599-6T<a>(il, op, load_vecprim_to_stack, elemtype, elem_size, v1, v2, add_offset, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3599-6T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public cil.VecPrimitiveType elemtype;

		public int elem_size;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public FSharpFunc<int, Unit> add_offset;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@3599-6 self8@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3599-6T(cil.CilWriter il, LLVMValueRef op, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, cil.VecPrimitiveType elemtype, int elem_size, AddressOfValue v1, AddressOfValue v2, FSharpFunc<int, Unit> add_offset, get_nth_val@3599-6 self8@)
		{
			this.il = il;
			this.op = op;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.elemtype = elemtype;
			this.elem_size = elem_size;
			this.v1 = v1;
			this.v2 = v2;
			this.add_offset = add_offset;
			this.self8@ = self8@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@3599-6 get_nth_val@3599-7 = self8@;
			int func = i * get_nth_val@3599-7.elem_size;
			emit_address_of_value(get_nth_val@3599-7.il, get_nth_val@3599-7.v1);
			get_nth_val@3599-7.add_offset.Invoke(func);
			get_nth_val@3599-7.load_vecprim_to_stack.Invoke(get_nth_val@3599-7.elemtype);
			emit_address_of_value(get_nth_val@3599-7.il, get_nth_val@3599-7.v2);
			get_nth_val@3599-7.add_offset.Invoke(func);
			get_nth_val@3599-7.load_vecprim_to_stack.Invoke(get_nth_val@3599-7.elemtype);
			emit_icmp(get_nth_val@3599-7.il, get_nth_val@3599-7.op.ICmpPredicate);
			return null;
		}
	}

	[Serializable]
	internal sealed class add_offset@3625-6 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@3625-6(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3629-7 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public int elem_size;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public FSharpFunc<int, Unit> add_offset;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3629-7(cil.CilWriter il, LLVMValueRef op, int elem_size, AddressOfValue v1, AddressOfValue v2, FSharpFunc<int, Unit> add_offset)
		{
			this.il = il;
			this.op = op;
			this.elem_size = elem_size;
			this.v1 = v1;
			this.v2 = v2;
			this.add_offset = add_offset;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@3629-7T<a>(il, op, elem_size, v1, v2, add_offset, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3629-7T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public int elem_size;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public FSharpFunc<int, Unit> add_offset;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@3629-7 self6@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3629-7T(cil.CilWriter il, LLVMValueRef op, int elem_size, AddressOfValue v1, AddressOfValue v2, FSharpFunc<int, Unit> add_offset, get_nth_val@3629-7 self6@)
		{
			this.il = il;
			this.op = op;
			this.elem_size = elem_size;
			this.v1 = v1;
			this.v2 = v2;
			this.add_offset = add_offset;
			this.self6@ = self6@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@3629-7 get_nth_val@3629-8 = self6@;
			int func = i * get_nth_val@3629-8.elem_size;
			emit_address_of_value(get_nth_val@3629-8.il, get_nth_val@3629-8.v1);
			get_nth_val@3629-8.add_offset.Invoke(func);
			get_nth_val@3629-8.il.Append(cil.MyInstruction.Ldind_I);
			emit_address_of_value(get_nth_val@3629-8.il, get_nth_val@3629-8.v2);
			get_nth_val@3629-8.add_offset.Invoke(func);
			get_nth_val@3629-8.il.Append(cil.MyInstruction.Ldind_I);
			emit_icmp(get_nth_val@3629-8.il, get_nth_val@3629-8.op.ICmpPredicate);
			return null;
		}
	}

	[Serializable]
	internal sealed class add_offset@3655-7 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@3655-7(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3659-8 : FSharpTypeFunc
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public LLVMValueRef op;

		public int elem_size;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public FSharpFunc<int, Unit> add_offset;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3659-8(GenSyms syms, cil.CilWriter il, LLVMValueRef op, int elem_size, AddressOfValue v1, AddressOfValue v2, FSharpFunc<int, Unit> add_offset)
		{
			this.syms = syms;
			this.il = il;
			this.op = op;
			this.elem_size = elem_size;
			this.v1 = v1;
			this.v2 = v2;
			this.add_offset = add_offset;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@3659-8T<a>(syms, il, op, elem_size, v1, v2, add_offset, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@3659-8T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public GenSyms syms;

		public cil.CilWriter il;

		public LLVMValueRef op;

		public int elem_size;

		public AddressOfValue v1;

		public AddressOfValue v2;

		public FSharpFunc<int, Unit> add_offset;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@3659-8 self7@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@3659-8T(GenSyms syms, cil.CilWriter il, LLVMValueRef op, int elem_size, AddressOfValue v1, AddressOfValue v2, FSharpFunc<int, Unit> add_offset, get_nth_val@3659-8 self7@)
		{
			this.syms = syms;
			this.il = il;
			this.op = op;
			this.elem_size = elem_size;
			this.v1 = v1;
			this.v2 = v2;
			this.add_offset = add_offset;
			this.self7@ = self7@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@3659-8 get_nth_val@3659-9 = self7@;
			get_nth_val@3659-9.il.Append(cil.MyInstruction.NewLdc_I4(get_nth_val@3659-9.elem_size));
			int func = i * get_nth_val@3659-9.elem_size;
			emit_address_of_value(get_nth_val@3659-9.il, get_nth_val@3659-9.v1);
			get_nth_val@3659-9.add_offset.Invoke(func);
			emit_address_of_value(get_nth_val@3659-9.il, get_nth_val@3659-9.v2);
			get_nth_val@3659-9.add_offset.Invoke(func);
			emit_strange_icmp(get_nth_val@3659-9.il, get_nth_val@3659-9.syms, get_nth_val@3659-9.op.ICmpPredicate);
			return null;
		}
	}

	[Serializable]
	internal sealed class gen_instr@3677-3 : FSharpFunc<LLVMValueRef, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueRef, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_instr@3677-3(FSharpFunc<LLVMValueRef, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(LLVMValueRef arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class gen_instr@3677-2 : FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_instr@3677-2(FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<LLVMValueRef, Unit> Invoke(cil.FirstClassType arg10)
		{
			FSharpFunc<LLVMValueRef, Unit> clo = clo1.Invoke(arg10);
			return new gen_instr@3677-3(clo);
		}
	}

	[Serializable]
	internal sealed class gen_instr@3951-5 : FSharpFunc<cil.FirstClassType, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<cil.FirstClassType, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_instr@3951-5(FSharpFunc<cil.FirstClassType, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(cil.FirstClassType arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class gen_instr@3951-4 : FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_instr@3951-4(FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<cil.FirstClassType, Unit> Invoke(cil.FirstClassType arg10)
		{
			FSharpFunc<cil.FirstClassType, Unit> clo = clo1.Invoke(arg10);
			return new gen_instr@3951-5(clo);
		}
	}

	[Serializable]
	internal sealed class f@3966-4 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public AddressOfValue v0;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@3966-4(cil.CilWriter il, AddressOfValue v0)
		{
			this.il = il;
			this.v0 = v0;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			emit_address_of_value(il, v0);
			return null;
		}
	}

	[Serializable]
	internal sealed class f@4035-5 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public AddressOfValue v0;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@4035-5(cil.CilWriter il, AddressOfValue v0)
		{
			this.il = il;
			this.v0 = v0;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			emit_address_of_value(il, v0);
			return null;
		}
	}

	[Serializable]
	internal sealed class f@4045-6 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public AddressOfValue v0;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@4045-6(cil.CilWriter il, AddressOfValue v0)
		{
			this.il = il;
			this.v0 = v0;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			emit_address_of_value(il, v0);
			return null;
		}
	}

	[Serializable]
	internal sealed class gen_instr@4090-7 : FSharpFunc<LLVMValueRef, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueRef, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_instr@4090-7(FSharpFunc<LLVMValueRef, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(LLVMValueRef arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class gen_instr@4090-6 : FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_instr@4090-6(FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<LLVMValueRef, Unit> Invoke(cil.FirstClassType arg10)
		{
			FSharpFunc<LLVMValueRef, Unit> clo = clo1.Invoke(arg10);
			return new gen_instr@4090-7(clo);
		}
	}

	[Serializable]
	internal sealed class conv_one@4164 : FSharpFunc<cil.PrimitiveType, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal conv_one@4164(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(cil.PrimitiveType pt)
		{
			switch (pt.Tag)
			{
			case 5:
				il.Append(cil.MyInstruction.Conv_R_Un);
				return null;
			case 6:
				il.Append(cil.MyInstruction.Conv_R_Un);
				il.Append(cil.MyInstruction.Conv_R8);
				return null;
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 4164, 22);
			}
		}
	}

	[Serializable]
	internal sealed class f@4198-7 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public AddressOfValue v0;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@4198-7(cil.CilWriter il, AddressOfValue v0)
		{
			this.il = il;
			this.v0 = v0;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			emit_address_of_value(il, v0);
			return null;
		}
	}

	[Serializable]
	internal sealed class gen_instr@4223-9 : FSharpFunc<cil.FirstClassType, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<cil.FirstClassType, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_instr@4223-9(FSharpFunc<cil.FirstClassType, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(cil.FirstClassType arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class gen_instr@4223-8 : FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_instr@4223-8(FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<cil.FirstClassType, Unit> Invoke(cil.FirstClassType arg10)
		{
			FSharpFunc<cil.FirstClassType, Unit> clo = clo1.Invoke(arg10);
			return new gen_instr@4223-9(clo);
		}
	}

	[Serializable]
	internal sealed class add_offset@4241-8 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@4241-8(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@4245-9 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public cil.VecPrimitiveType elemtype_dst;

		public int elem_size_src;

		public AddressOfValue v1;

		public FSharpFunc<int, Unit> add_offset;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@4245-9(cil.CilWriter il, cil.VecPrimitiveType elemtype_dst, int elem_size_src, AddressOfValue v1, FSharpFunc<int, Unit> add_offset)
		{
			this.il = il;
			this.elemtype_dst = elemtype_dst;
			this.elem_size_src = elem_size_src;
			this.v1 = v1;
			this.add_offset = add_offset;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@4245-9T<a>(il, elemtype_dst, elem_size_src, v1, add_offset, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@4245-9T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public cil.VecPrimitiveType elemtype_dst;

		public int elem_size_src;

		public AddressOfValue v1;

		public FSharpFunc<int, Unit> add_offset;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@4245-9 self5@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@4245-9T(cil.CilWriter il, cil.VecPrimitiveType elemtype_dst, int elem_size_src, AddressOfValue v1, FSharpFunc<int, Unit> add_offset, get_nth_val@4245-9 self5@)
		{
			this.il = il;
			this.elemtype_dst = elemtype_dst;
			this.elem_size_src = elem_size_src;
			this.v1 = v1;
			this.add_offset = add_offset;
			this.self5@ = self5@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@4245-9 get_nth_val@4245-10 = self5@;
			int func = i * get_nth_val@4245-10.elem_size_src;
			emit_address_of_value(get_nth_val@4245-10.il, get_nth_val@4245-10.v1);
			get_nth_val@4245-10.add_offset.Invoke(func);
			get_nth_val@4245-10.il.Append(cil.MyInstruction.Ldind_I1);
			if (get_nth_val@4245-10.elemtype_dst.Tag == 3)
			{
				get_nth_val@4245-10.il.Append(cil.MyInstruction.Conv_I8);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class sext_bit_vector@4234 : OptimizedClosures.FSharpFunc<cil.VecPrimitiveType, uint, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, cil.Variable> grab_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal sext_bit_vector@4234(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, cil.Variable> grab_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local)
		{
			this.il = il;
			this.op = op;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.get_result_local = get_result_local;
			this.grab_result_local = grab_result_local;
			this.finish_result_local = finish_result_local;
		}

		public override Unit Invoke(cil.VecPrimitiveType elemtype_dst, uint count)
		{
			int elem_size_src = 1;
			AddressOfValue addressOfValue = my_prep_address_of_value.Invoke(op.GetOperand(0u));
			ResultLocal func = get_result_local.Invoke(null);
			FSharpFunc<int, Unit> add_offset = new add_offset@4241-8(il);
			FSharpTypeFunc fSharpTypeFunc = new get_nth_val@4245-9(il, elemtype_dst, elem_size_src, addressOfValue, add_offset);
			do_vec_into_local(il, cil.vecprim_to_prim(elemtype_dst), count, grab_result_local.Invoke(func), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc.Specialize<cil.CilWriter>());
			release_address_of_value(il, addressOfValue);
			return finish_result_local.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class add_offset@4271-9 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@4271-9(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@4275-10 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public cil.VecPrimitiveType elemtype_src;

		public int elem_size_src;

		public AddressOfValue v1;

		public FSharpFunc<int, Unit> add_offset;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@4275-10(cil.CilWriter il, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, cil.VecPrimitiveType elemtype_src, int elem_size_src, AddressOfValue v1, FSharpFunc<int, Unit> add_offset)
		{
			this.il = il;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.elemtype_src = elemtype_src;
			this.elem_size_src = elem_size_src;
			this.v1 = v1;
			this.add_offset = add_offset;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@4275-10T<a>(il, load_vecprim_to_stack, elemtype_src, elem_size_src, v1, add_offset, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@4275-10T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		public cil.VecPrimitiveType elemtype_src;

		public int elem_size_src;

		public AddressOfValue v1;

		public FSharpFunc<int, Unit> add_offset;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@4275-10 self6@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@4275-10T(cil.CilWriter il, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack, cil.VecPrimitiveType elemtype_src, int elem_size_src, AddressOfValue v1, FSharpFunc<int, Unit> add_offset, get_nth_val@4275-10 self6@)
		{
			this.il = il;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
			this.elemtype_src = elemtype_src;
			this.elem_size_src = elem_size_src;
			this.v1 = v1;
			this.add_offset = add_offset;
			this.self6@ = self6@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@4275-10 get_nth_val@4275-11 = self6@;
			int func = i * get_nth_val@4275-11.elem_size_src;
			emit_address_of_value(get_nth_val@4275-11.il, get_nth_val@4275-11.v1);
			get_nth_val@4275-11.add_offset.Invoke(func);
			return get_nth_val@4275-11.load_vecprim_to_stack.Invoke(get_nth_val@4275-11.elemtype_src);
		}
	}

	[Serializable]
	internal sealed class sext_vector@4264 : OptimizedClosures.FSharpFunc<cil.VecPrimitiveType, cil.VecPrimitiveType, uint, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, cil.Variable> grab_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		public FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal sext_vector@4264(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, cil.Variable> grab_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local, FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack)
		{
			this.il = il;
			this.op = op;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.get_result_local = get_result_local;
			this.grab_result_local = grab_result_local;
			this.finish_result_local = finish_result_local;
			this.load_vecprim_to_stack = load_vecprim_to_stack;
		}

		public override Unit Invoke(cil.VecPrimitiveType elemtype_src, cil.VecPrimitiveType elemtype_dst, uint count)
		{
			int elem_size_src = cil.sizeof_vecprim(elemtype_src);
			AddressOfValue addressOfValue = my_prep_address_of_value.Invoke(op.GetOperand(0u));
			ResultLocal func = get_result_local.Invoke(null);
			FSharpFunc<int, Unit> add_offset = new add_offset@4271-9(il);
			FSharpTypeFunc fSharpTypeFunc = new get_nth_val@4275-10(il, load_vecprim_to_stack, elemtype_src, elem_size_src, addressOfValue, add_offset);
			do_vec_into_local(il, cil.vecprim_to_prim(elemtype_dst), count, grab_result_local.Invoke(func), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc.Specialize<cil.CilWriter>());
			release_address_of_value(il, addressOfValue);
			return finish_result_local.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class load_result@4296 : FSharpFunc<long, Unit>
	{
		public cil.CilWriter il;

		public cil.PrimitiveType ityp_to;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_result@4296(cil.CilWriter il, cil.PrimitiveType ityp_to)
		{
			this.il = il;
			this.ityp_to = ityp_to;
		}

		public override Unit Invoke(long n)
		{
			switch (ityp_to.Tag)
			{
			case 1:
			case 2:
			case 3:
				il.Append(cil.MyInstruction.NewLdc_I4((int)n));
				return null;
			case 4:
				il.Append(cil.MyInstruction.NewLdc_I8(n));
				return null;
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 4296, 26);
			}
		}
	}

	[Serializable]
	internal sealed class gen_instr@4339-11 : FSharpFunc<cil.FirstClassType, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<cil.FirstClassType, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_instr@4339-11(FSharpFunc<cil.FirstClassType, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(cil.FirstClassType arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class gen_instr@4339-10 : FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_instr@4339-10(FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<cil.FirstClassType, Unit> Invoke(cil.FirstClassType arg10)
		{
			FSharpFunc<cil.FirstClassType, Unit> clo = clo1.Invoke(arg10);
			return new gen_instr@4339-11(clo);
		}
	}

	[Serializable]
	internal sealed class add_offset@4356-10 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@4356-10(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@4360-11 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public cil.VecPrimitiveType elemtype_dst;

		public int elem_size_src;

		public AddressOfValue v1;

		public FSharpFunc<int, Unit> add_offset;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@4360-11(cil.CilWriter il, cil.VecPrimitiveType elemtype_dst, int elem_size_src, AddressOfValue v1, FSharpFunc<int, Unit> add_offset)
		{
			this.il = il;
			this.elemtype_dst = elemtype_dst;
			this.elem_size_src = elem_size_src;
			this.v1 = v1;
			this.add_offset = add_offset;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@4360-11T<a>(il, elemtype_dst, elem_size_src, v1, add_offset, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@4360-11T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public cil.VecPrimitiveType elemtype_dst;

		public int elem_size_src;

		public AddressOfValue v1;

		public FSharpFunc<int, Unit> add_offset;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@4360-11 self5@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@4360-11T(cil.CilWriter il, cil.VecPrimitiveType elemtype_dst, int elem_size_src, AddressOfValue v1, FSharpFunc<int, Unit> add_offset, get_nth_val@4360-11 self5@)
		{
			this.il = il;
			this.elemtype_dst = elemtype_dst;
			this.elem_size_src = elem_size_src;
			this.v1 = v1;
			this.add_offset = add_offset;
			this.self5@ = self5@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@4360-11 get_nth_val@4360-12 = self5@;
			int func = i * get_nth_val@4360-12.elem_size_src;
			emit_address_of_value(get_nth_val@4360-12.il, get_nth_val@4360-12.v1);
			get_nth_val@4360-12.add_offset.Invoke(func);
			get_nth_val@4360-12.il.Append(cil.MyInstruction.Ldind_U1);
			get_nth_val@4360-12.il.Append(cil.MyInstruction.NewLdc_I4(1));
			get_nth_val@4360-12.il.Append(cil.MyInstruction.And);
			if (get_nth_val@4360-12.elemtype_dst.Tag == 3)
			{
				get_nth_val@4360-12.il.Append(cil.MyInstruction.Conv_U8);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class zext_bit_vector@4349 : OptimizedClosures.FSharpFunc<cil.VecPrimitiveType, uint, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, cil.Variable> grab_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal zext_bit_vector@4349(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, cil.Variable> grab_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local)
		{
			this.il = il;
			this.op = op;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.get_result_local = get_result_local;
			this.grab_result_local = grab_result_local;
			this.finish_result_local = finish_result_local;
		}

		public override Unit Invoke(cil.VecPrimitiveType elemtype_dst, uint count)
		{
			int elem_size_src = 1;
			AddressOfValue addressOfValue = my_prep_address_of_value.Invoke(op.GetOperand(0u));
			ResultLocal func = get_result_local.Invoke(null);
			FSharpFunc<int, Unit> add_offset = new add_offset@4356-10(il);
			FSharpTypeFunc fSharpTypeFunc = new get_nth_val@4360-11(il, elemtype_dst, elem_size_src, addressOfValue, add_offset);
			do_vec_into_local(il, cil.vecprim_to_prim(elemtype_dst), count, grab_result_local.Invoke(func), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc.Specialize<cil.CilWriter>());
			release_address_of_value(il, addressOfValue);
			return finish_result_local.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class add_offset@4390-11 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@4390-11(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@4394-12 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public cil.VecPrimitiveType elemtype_src;

		public cil.VecPrimitiveType elemtype_dst;

		public int elem_size_src;

		public AddressOfValue v1;

		public FSharpFunc<int, Unit> add_offset;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@4394-12(cil.CilWriter il, cil.VecPrimitiveType elemtype_src, cil.VecPrimitiveType elemtype_dst, int elem_size_src, AddressOfValue v1, FSharpFunc<int, Unit> add_offset)
		{
			this.il = il;
			this.elemtype_src = elemtype_src;
			this.elemtype_dst = elemtype_dst;
			this.elem_size_src = elem_size_src;
			this.v1 = v1;
			this.add_offset = add_offset;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@4394-12T<a>(il, elemtype_src, elemtype_dst, elem_size_src, v1, add_offset, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@4394-12T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public cil.VecPrimitiveType elemtype_src;

		public cil.VecPrimitiveType elemtype_dst;

		public int elem_size_src;

		public AddressOfValue v1;

		public FSharpFunc<int, Unit> add_offset;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@4394-12 self6@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@4394-12T(cil.CilWriter il, cil.VecPrimitiveType elemtype_src, cil.VecPrimitiveType elemtype_dst, int elem_size_src, AddressOfValue v1, FSharpFunc<int, Unit> add_offset, get_nth_val@4394-12 self6@)
		{
			this.il = il;
			this.elemtype_src = elemtype_src;
			this.elemtype_dst = elemtype_dst;
			this.elem_size_src = elem_size_src;
			this.v1 = v1;
			this.add_offset = add_offset;
			this.self6@ = self6@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@4394-12 get_nth_val@4394-13 = self6@;
			int func = i * get_nth_val@4394-13.elem_size_src;
			emit_address_of_value(get_nth_val@4394-13.il, get_nth_val@4394-13.v1);
			get_nth_val@4394-13.add_offset.Invoke(func);
			switch (get_nth_val@4394-13.elemtype_src.Tag)
			{
			case 0:
				get_nth_val@4394-13.il.Append(cil.MyInstruction.Ldind_U1);
				break;
			case 1:
				get_nth_val@4394-13.il.Append(cil.MyInstruction.Ldind_U2);
				break;
			case 2:
				get_nth_val@4394-13.il.Append(cil.MyInstruction.Ldind_I4);
				break;
			case 3:
				get_nth_val@4394-13.il.Append(cil.MyInstruction.Ldind_I8);
				break;
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 4399, 26);
			}
			if (get_nth_val@4394-13.elemtype_dst.Tag == 3)
			{
				get_nth_val@4394-13.il.Append(cil.MyInstruction.Conv_U8);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class zext_vector@4383 : OptimizedClosures.FSharpFunc<cil.VecPrimitiveType, cil.VecPrimitiveType, uint, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, cil.Variable> grab_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal zext_vector@4383(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, cil.Variable> grab_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local)
		{
			this.il = il;
			this.op = op;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.get_result_local = get_result_local;
			this.grab_result_local = grab_result_local;
			this.finish_result_local = finish_result_local;
		}

		public override Unit Invoke(cil.VecPrimitiveType elemtype_src, cil.VecPrimitiveType elemtype_dst, uint count)
		{
			int elem_size_src = cil.sizeof_vecprim(elemtype_src);
			AddressOfValue addressOfValue = my_prep_address_of_value.Invoke(op.GetOperand(0u));
			ResultLocal func = get_result_local.Invoke(null);
			FSharpFunc<int, Unit> add_offset = new add_offset@4390-11(il);
			FSharpTypeFunc fSharpTypeFunc = new get_nth_val@4394-12(il, elemtype_src, elemtype_dst, elem_size_src, addressOfValue, add_offset);
			do_vec_into_local(il, cil.vecprim_to_prim(elemtype_dst), count, grab_result_local.Invoke(func), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc.Specialize<cil.CilWriter>());
			release_address_of_value(il, addressOfValue);
			return finish_result_local.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class add_offset@4439-12 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@4439-12(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@4443-13 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public cil.VecPrimitiveType elemtype_src;

		public int elem_size_src;

		public AddressOfValue v1;

		public FSharpFunc<int, Unit> add_offset;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@4443-13(cil.CilWriter il, cil.VecPrimitiveType elemtype_src, int elem_size_src, AddressOfValue v1, FSharpFunc<int, Unit> add_offset)
		{
			this.il = il;
			this.elemtype_src = elemtype_src;
			this.elem_size_src = elem_size_src;
			this.v1 = v1;
			this.add_offset = add_offset;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@4443-13T<a>(il, elemtype_src, elem_size_src, v1, add_offset, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@4443-13T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public cil.VecPrimitiveType elemtype_src;

		public int elem_size_src;

		public AddressOfValue v1;

		public FSharpFunc<int, Unit> add_offset;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@4443-13 self5@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@4443-13T(cil.CilWriter il, cil.VecPrimitiveType elemtype_src, int elem_size_src, AddressOfValue v1, FSharpFunc<int, Unit> add_offset, get_nth_val@4443-13 self5@)
		{
			this.il = il;
			this.elemtype_src = elemtype_src;
			this.elem_size_src = elem_size_src;
			this.v1 = v1;
			this.add_offset = add_offset;
			this.self5@ = self5@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@4443-13 get_nth_val@4443-14 = self5@;
			int func = i * get_nth_val@4443-14.elem_size_src;
			emit_address_of_value(get_nth_val@4443-14.il, get_nth_val@4443-14.v1);
			get_nth_val@4443-14.add_offset.Invoke(func);
			switch (get_nth_val@4443-14.elemtype_src.Tag)
			{
			case 0:
				get_nth_val@4443-14.il.Append(cil.MyInstruction.Ldind_U1);
				return null;
			case 1:
				get_nth_val@4443-14.il.Append(cil.MyInstruction.Ldind_U2);
				return null;
			case 2:
				get_nth_val@4443-14.il.Append(cil.MyInstruction.Ldind_U4);
				return null;
			case 3:
				get_nth_val@4443-14.il.Append(cil.MyInstruction.Ldind_I8);
				return null;
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 4448, 26);
			}
		}
	}

	[Serializable]
	internal sealed class zext_vector_2@4427 : OptimizedClosures.FSharpFunc<cil.VecPrimitiveType, int, uint, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, Unit> ldloca_result_local;

		public FSharpFunc<ResultLocal, Unit> init_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal zext_vector_2@4427(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, Unit> ldloca_result_local, FSharpFunc<ResultLocal, Unit> init_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local)
		{
			this.il = il;
			this.op = op;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.get_result_local = get_result_local;
			this.ldloca_result_local = ldloca_result_local;
			this.init_result_local = init_result_local;
			this.finish_result_local = finish_result_local;
		}

		public override Unit Invoke(cil.VecPrimitiveType elemtype_src, int elem_size_dst, uint count)
		{
			int elem_size_src = cil.sizeof_vecprim(elemtype_src);
			AddressOfValue addressOfValue = my_prep_address_of_value.Invoke(op.GetOperand(0u));
			ResultLocal func = get_result_local.Invoke(null);
			init_result_local.Invoke(func);
			FSharpFunc<int, Unit> add_offset = new add_offset@4439-12(il);
			FSharpTypeFunc fSharpTypeFunc = new get_nth_val@4443-13(il, elemtype_src, elem_size_src, addressOfValue, add_offset);
			int num = 0;
			int num2 = (int)(count - 1);
			if (num2 >= num)
			{
				do
				{
					ldloca_result_local.Invoke(func);
					if (num > 0)
					{
						il.Append(cil.MyInstruction.NewLdc_I4(num * elem_size_dst));
						il.Append(cil.MyInstruction.Add);
					}
					cil.CilWriter cilWriter = il;
					int arg = num;
					cil.CilWriter arg2 = cilWriter;
					FSharpFunc<cil.CilWriter, int>.InvokeFast((FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc.Specialize<cil.CilWriter>(), arg2, arg);
					switch (elemtype_src.Tag)
					{
					case 0:
						il.Append(cil.MyInstruction.Stind_I1);
						break;
					case 1:
						il.Append(cil.MyInstruction.Stind_I2);
						break;
					case 2:
						il.Append(cil.MyInstruction.Stind_I4);
						break;
					case 3:
						il.Append(cil.MyInstruction.Stind_I8);
						break;
					default:
						throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 4470, 26);
					}
					num++;
				}
				while (num != num2 + 1);
			}
			release_address_of_value(il, addressOfValue);
			return finish_result_local.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class f@4587-8 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public AddressOfValue addr;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@4587-8(cil.CilWriter il, AddressOfValue addr)
		{
			this.il = il;
			this.addr = addr;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			emit_address_of_value(il, addr);
			return null;
		}
	}

	[Serializable]
	internal sealed class f@4597-9 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public AddressOfValue addr;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@4597-9(cil.CilWriter il, AddressOfValue addr)
		{
			this.il = il;
			this.addr = addr;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			emit_address_of_value(il, addr);
			return null;
		}
	}

	[Serializable]
	internal sealed class f@4615-10 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public AddressOfValue addr;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@4615-10(cil.CilWriter il, AddressOfValue addr)
		{
			this.il = il;
			this.addr = addr;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			emit_address_of_value(il, addr);
			return null;
		}
	}

	[Serializable]
	internal sealed class add_offset@4639-13 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@4639-13(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class get_nth_val@4643-14 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public cil.VecPrimitiveType elemtype_src;

		public cil.VecPrimitiveType elemtype_dst;

		public int elem_size_src;

		public AddressOfValue v1;

		public FSharpFunc<int, Unit> add_offset;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@4643-14(cil.CilWriter il, cil.VecPrimitiveType elemtype_src, cil.VecPrimitiveType elemtype_dst, int elem_size_src, AddressOfValue v1, FSharpFunc<int, Unit> add_offset)
		{
			this.il = il;
			this.elemtype_src = elemtype_src;
			this.elemtype_dst = elemtype_dst;
			this.elem_size_src = elem_size_src;
			this.v1 = v1;
			this.add_offset = add_offset;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new get_nth_val@4643-14T<a>(il, elemtype_src, elemtype_dst, elem_size_src, v1, add_offset, this);
		}
	}

	[Serializable]
	internal sealed class get_nth_val@4643-14T<a> : OptimizedClosures.FSharpFunc<a, int, Unit>
	{
		public cil.CilWriter il;

		public cil.VecPrimitiveType elemtype_src;

		public cil.VecPrimitiveType elemtype_dst;

		public int elem_size_src;

		public AddressOfValue v1;

		public FSharpFunc<int, Unit> add_offset;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public get_nth_val@4643-14 self6@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_nth_val@4643-14T(cil.CilWriter il, cil.VecPrimitiveType elemtype_src, cil.VecPrimitiveType elemtype_dst, int elem_size_src, AddressOfValue v1, FSharpFunc<int, Unit> add_offset, get_nth_val@4643-14 self6@)
		{
			this.il = il;
			this.elemtype_src = elemtype_src;
			this.elemtype_dst = elemtype_dst;
			this.elem_size_src = elem_size_src;
			this.v1 = v1;
			this.add_offset = add_offset;
			this.self6@ = self6@;
		}

		public override Unit Invoke(a x, int i)
		{
			get_nth_val@4643-14 get_nth_val@4643-15 = self6@;
			int func = i * get_nth_val@4643-15.elem_size_src;
			emit_address_of_value(get_nth_val@4643-15.il, get_nth_val@4643-15.v1);
			get_nth_val@4643-15.add_offset.Invoke(func);
			switch (get_nth_val@4643-15.elemtype_src.Tag)
			{
			case 0:
				get_nth_val@4643-15.il.Append(cil.MyInstruction.Ldind_U1);
				break;
			case 1:
				get_nth_val@4643-15.il.Append(cil.MyInstruction.Ldind_U2);
				break;
			case 2:
				get_nth_val@4643-15.il.Append(cil.MyInstruction.Ldind_I4);
				break;
			case 3:
				get_nth_val@4643-15.il.Append(cil.MyInstruction.Ldind_I8);
				get_nth_val@4643-15.il.Append(cil.MyInstruction.Conv_U4);
				break;
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 4649, 26);
			}
			switch (get_nth_val@4643-15.elemtype_dst.Tag)
			{
			case 0:
				get_nth_val@4643-15.il.Append(cil.MyInstruction.NewLdc_I4(255));
				get_nth_val@4643-15.il.Append(cil.MyInstruction.And);
				return null;
			case 1:
				get_nth_val@4643-15.il.Append(cil.MyInstruction.NewLdc_I4(65535));
				get_nth_val@4643-15.il.Append(cil.MyInstruction.And);
				return null;
			case 2:
				get_nth_val@4643-15.il.Append(cil.MyInstruction.NewLdc_I4(-1));
				get_nth_val@4643-15.il.Append(cil.MyInstruction.And);
				return null;
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 4660, 26);
			}
		}
	}

	[Serializable]
	internal sealed class trunc_vector@4632 : OptimizedClosures.FSharpFunc<cil.VecPrimitiveType, cil.VecPrimitiveType, uint, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, cil.Variable> grab_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal trunc_vector@4632(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, cil.Variable> grab_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local)
		{
			this.il = il;
			this.op = op;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.get_result_local = get_result_local;
			this.grab_result_local = grab_result_local;
			this.finish_result_local = finish_result_local;
		}

		public override Unit Invoke(cil.VecPrimitiveType elemtype_src, cil.VecPrimitiveType elemtype_dst, uint count)
		{
			int elem_size_src = cil.sizeof_vecprim(elemtype_src);
			AddressOfValue addressOfValue = my_prep_address_of_value.Invoke(op.GetOperand(0u));
			ResultLocal func = get_result_local.Invoke(null);
			FSharpFunc<int, Unit> add_offset = new add_offset@4639-13(il);
			FSharpTypeFunc fSharpTypeFunc = new get_nth_val@4643-14(il, elemtype_src, elemtype_dst, elem_size_src, addressOfValue, add_offset);
			do_vec_into_local(il, cil.vecprim_to_prim(elemtype_dst), count, grab_result_local.Invoke(func), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc.Specialize<cil.CilWriter>());
			release_address_of_value(il, addressOfValue);
			return finish_result_local.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class add_offset@4685-14 : FSharpFunc<int, Unit>
	{
		public cil.CilWriter il;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal add_offset@4685-14(cil.CilWriter il)
		{
			this.il = il;
		}

		public override Unit Invoke(int off)
		{
			if (off > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(off));
				il.Append(cil.MyInstruction.Add);
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class trunc_vector_2@4676 : FSharpTypeFunc
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, Unit> ldloca_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal trunc_vector_2@4676(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, Unit> ldloca_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local)
		{
			this.il = il;
			this.op = op;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.get_result_local = get_result_local;
			this.ldloca_result_local = ldloca_result_local;
			this.finish_result_local = finish_result_local;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new trunc_vector_2@4676T<a>(il, op, my_prep_address_of_value, get_result_local, ldloca_result_local, finish_result_local, this);
		}
	}

	[Serializable]
	internal sealed class trunc_vector_2@4676T<a> : OptimizedClosures.FSharpFunc<cil.VectorElementType, a, uint, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef op;

		public FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value;

		public FSharpFunc<Unit, ResultLocal> get_result_local;

		public FSharpFunc<ResultLocal, Unit> ldloca_result_local;

		public FSharpFunc<ResultLocal, Unit> finish_result_local;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public trunc_vector_2@4676 self6@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal trunc_vector_2@4676T(cil.CilWriter il, LLVMValueRef op, FSharpFunc<LLVMValueRef, AddressOfValue> my_prep_address_of_value, FSharpFunc<Unit, ResultLocal> get_result_local, FSharpFunc<ResultLocal, Unit> ldloca_result_local, FSharpFunc<ResultLocal, Unit> finish_result_local, trunc_vector_2@4676 self6@)
		{
			this.il = il;
			this.op = op;
			this.my_prep_address_of_value = my_prep_address_of_value;
			this.get_result_local = get_result_local;
			this.ldloca_result_local = ldloca_result_local;
			this.finish_result_local = finish_result_local;
			this.self6@ = self6@;
		}

		public override Unit Invoke(cil.VectorElementType elemtype_src, a elemtype_dst, uint count)
		{
			trunc_vector_2@4676 trunc_vector_2@4677 = self6@;
			int num = cil.sizeof_vec_elem(elemtype_src);
			int num2 = cil.sizeof_vec_elem(elemtype_src);
			AddressOfValue addr = trunc_vector_2@4677.my_prep_address_of_value.Invoke(trunc_vector_2@4677.op.GetOperand(0u));
			ResultLocal func = trunc_vector_2@4677.get_result_local.Invoke(null);
			FSharpFunc<int, Unit> fSharpFunc = new add_offset@4685-14(trunc_vector_2@4677.il);
			int num3 = 0;
			int num4 = (int)(count - 1);
			if (num4 >= num3)
			{
				do
				{
					trunc_vector_2@4677.ldloca_result_local.Invoke(func);
					fSharpFunc.Invoke(num3 * num2);
					emit_address_of_value(trunc_vector_2@4677.il, addr);
					fSharpFunc.Invoke(num3 * num);
					trunc_vector_2@4677.il.Append(cil.MyInstruction.NewLdc_I4(num2));
					trunc_vector_2@4677.il.Append(cil.MyInstruction.Cpblk);
					num3++;
				}
				while (num3 != num4 + 1);
			}
			release_address_of_value(trunc_vector_2@4677.il, addr);
			return trunc_vector_2@4677.finish_result_local.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class f@4792-11 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public AddressOfValue addr;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@4792-11(cil.CilWriter il, AddressOfValue addr)
		{
			this.il = il;
			this.addr = addr;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			emit_address_of_value(il, addr);
			return null;
		}
	}

	[Serializable]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[DebuggerDisplay("{__DebugDisplay(),nq}")]
	[CompilationMapping(SourceConstructFlags.SumType)]
	public sealed class TraceEnterExit : IEquatable<TraceEnterExit>, IStructuralEquatable, IComparable<TraceEnterExit>, IComparable, IStructuralComparable
	{
		public static class Tags
		{
			public const int Yes = 0;

			public const int No = 1;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly TraceEnterExit _unique_Yes = new TraceEnterExit(0);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly TraceEnterExit _unique_No = new TraceEnterExit(1);

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[field: DebuggerNonUserCode]
		public int Tag
		{
			[DebuggerNonUserCode]
			get;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static TraceEnterExit Yes
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
			get
			{
				return _unique_Yes;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsYes
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 0;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static TraceEnterExit No
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
			get
			{
				return _unique_No;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsNo
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 1;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal TraceEnterExit(int _tag)
		{
			this._tag = _tag;
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<TraceEnterExit, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<TraceEnterExit, string>, Unit, string, string, TraceEnterExit>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(TraceEnterExit obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = _tag;
					int num2 = obj._tag;
					if (num == num2)
					{
						return 0;
					}
					return num - num2;
				}
				return 1;
			}
			if (obj != null)
			{
				return -1;
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj)
		{
			return CompareTo((TraceEnterExit)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			TraceEnterExit traceEnterExit = (TraceEnterExit)obj;
			if (this != null)
			{
				if ((TraceEnterExit)obj != null)
				{
					int num = _tag;
					int num2 = traceEnterExit._tag;
					if (num == num2)
					{
						return 0;
					}
					return num - num2;
				}
				return 1;
			}
			if ((TraceEnterExit)obj != null)
			{
				return -1;
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				return _tag;
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is TraceEnterExit traceEnterExit)
				{
					TraceEnterExit traceEnterExit2 = traceEnterExit;
					int num = _tag;
					int num2 = traceEnterExit2._tag;
					return num == num2;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(TraceEnterExit obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = _tag;
					int num2 = obj._tag;
					return num == num2;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is TraceEnterExit obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	internal sealed class used_outside_its_block@5097 : FSharpFunc<LLVMUseRef, bool>
	{
		public LLVMBasicBlockRef blk;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal used_outside_its_block@5097(LLVMBasicBlockRef blk)
		{
			this.blk = blk;
		}

		public override bool Invoke(LLVMUseRef x)
		{
			LLVMValueRef v = sgllvm.get_user(x);
			switch (sgllvm.get_value_kind(v))
			{
			case LLVMValueKind.LLVMInstructionValueKind:
			{
				LLVMBasicBlockRef instructionParent = v.InstructionParent;
				LLVMBasicBlockRef y = blk;
				return !LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(instructionParent, y);
			}
			default:
				return false;
			}
		}
	}

	[Serializable]
	internal sealed class used_by_phi@5106 : FSharpFunc<LLVMUseRef, bool>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal used_by_phi@5106()
		{
		}

		public override bool Invoke(LLVMUseRef x)
		{
			LLVMValueRef v = sgllvm.get_user(x);
			return sgllvm.get_value_kind(v) switch
			{
				LLVMValueKind.LLVMInstructionValueKind => v.InstructionOpcode switch
				{
					LLVMOpcode.LLVMPHI => true, 
					_ => false, 
				}, 
				_ => false, 
			};
		}
	}

	[Serializable]
	internal sealed class calc_depth@5117 : FSharpFunc<LLVMValueRef, int>
	{
		public Dictionary<LLVMValueRef, InstructionValue> vals;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal calc_depth@5117(Dictionary<LLVMValueRef, InstructionValue> vals)
		{
			this.vals = vals;
		}

		public override int Invoke(LLVMValueRef v)
		{
			List<int> list = new List<int>();
			int num = 0;
			int num2 = v.OperandCount - 1;
			if (num2 >= num)
			{
				do
				{
					LLVMValueRef operand = v.GetOperand((uint)num);
					switch (sgllvm.get_value_kind(operand))
					{
					case LLVMValueKind.LLVMInstructionValueKind:
					{
						InstructionValue value = null;
						Tuple<bool, InstructionValue> tuple = new Tuple<bool, InstructionValue>(vals.TryGetValue(operand, out value), value);
						if (tuple.Item1)
						{
							InstructionValue item = tuple.Item2;
							InstructionValue instructionValue = item;
							if (instructionValue is InstructionValue.Immed)
							{
								InstructionValue.Immed immed = (InstructionValue.Immed)instructionValue;
								int num3 = Invoke(operand);
								list.Add(1 + num3);
							}
						}
						break;
					}
					}
					num++;
				}
				while (num != num2 + 1);
			}
			if (list.Count > 0)
			{
				List<int> list2 = list;
				List<int> list3 = list2;
				IEnumerable<int> enumerable = list3;
				if (enumerable == null)
				{
					throw new ArgumentNullException("source");
				}
				IEnumerator<int> enumerator = enumerable.GetEnumerator();
				try
				{
					if (!enumerator.MoveNext())
					{
						throw new ArgumentException(LanguagePrimitives.ErrorStrings.InputSequenceEmptyString, "source");
					}
					int num4 = enumerator.Current;
					while (enumerator.MoveNext())
					{
						int current = enumerator.Current;
						if (current > num4)
						{
							num4 = current;
						}
					}
					return num4;
				}
				finally
				{
					if (enumerator is IDisposable disposable)
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
			return 0;
		}
	}

	[Serializable]
	internal sealed class f@5335-12 : FSharpFunc<LLVMValueRef, FSharpOption<LLVMValueRef>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@5335-12()
		{
		}

		public override FSharpOption<LLVMValueRef> Invoke(LLVMValueRef op)
		{
			return op.InstructionOpcode switch
			{
				LLVMOpcode.LLVMPHI => FSharpOption<LLVMValueRef>.Some(op), 
				_ => null, 
			};
		}
	}

	[Serializable]
	internal sealed class find_phis@5334 : FSharpFunc<LLVMBasicBlockRef, LLVMValueRef[]>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal find_phis@5334()
		{
		}

		public override LLVMValueRef[] Invoke(LLVMBasicBlockRef blk)
		{
			FSharpFunc<LLVMValueRef, FSharpOption<LLVMValueRef>> chooser = new f@5335-12();
			return ArrayModule.Choose(chooser, sgllvm.get_instructions(blk).ToArray());
		}
	}

	[Serializable]
	internal sealed class phi_val@5362 : FSharpFunc<Unit, Dictionary<LLVMValueRef, LLVMValueRef>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal phi_val@5362()
		{
		}

		public override Dictionary<LLVMValueRef, LLVMValueRef> Invoke(Unit unitVar0)
		{
			return new Dictionary<LLVMValueRef, LLVMValueRef>();
		}
	}

	[Serializable]
	internal sealed class my_load_instruction_value_to_stack@5401 : OptimizedClosures.FSharpFunc<cil.CilWriter, InstructionValue, FSharpFunc<cil.CilWriter, FSharpFunc<LLVMValueRef, Unit>>, Unit>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal my_load_instruction_value_to_stack@5401()
		{
		}

		public override Unit Invoke(cil.CilWriter il, InstructionValue iv, FSharpFunc<cil.CilWriter, FSharpFunc<LLVMValueRef, Unit>> f_gen)
		{
			if (!(iv is InstructionValue.Temp))
			{
				if (!(iv is InstructionValue.Immed))
				{
					InstructionValue.Local local = (InstructionValue.Local)iv;
					cil.Variable item = local.item;
					il.Append(cil.MyInstruction.NewLdloca(item));
					return null;
				}
				InstructionValue.Immed immed = (InstructionValue.Immed)iv;
				LLVMValueRef arg = immed.item;
				return FSharpFunc<cil.CilWriter, LLVMValueRef>.InvokeFast(f_gen, il, arg);
			}
			InstructionValue.Temp temp = (InstructionValue.Temp)iv;
			cil.Variable item2 = temp.item;
			il.Append(cil.MyInstruction.NewLdloc(item2));
			return null;
		}
	}

	[Serializable]
	internal sealed class f_get_instr_value@5427 : FSharpFunc<LLVMValueRef, InstructionValue>
	{
		public Dictionary<LLVMValueRef, InstructionValue> vals;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_get_instr_value@5427(Dictionary<LLVMValueRef, InstructionValue> vals)
		{
			this.vals = vals;
		}

		public override InstructionValue Invoke(LLVMValueRef v)
		{
			return vals[v];
		}
	}

	[Serializable]
	internal sealed class get_result_dest@5431 : FSharpFunc<LLVMValueRef, InstructionDest>
	{
		public Dictionary<LLVMValueRef, InstructionValue> vals;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_result_dest@5431(Dictionary<LLVMValueRef, InstructionValue> vals)
		{
			this.vals = vals;
		}

		public override InstructionDest Invoke(LLVMValueRef instr)
		{
			InstructionValue value;
			Tuple<bool, InstructionValue> tuple = new Tuple<bool, InstructionValue>(vals.TryGetValue(instr, out value), value);
			if (tuple.Item1)
			{
				InstructionValue item = tuple.Item2;
				if (!(item is InstructionValue.Immed))
				{
					if (!(item is InstructionValue.Local))
					{
						InstructionValue.Temp temp = (InstructionValue.Temp)tuple.Item2;
						cil.Variable item2 = temp.item;
						return InstructionDest.NewInTemp(item2);
					}
					InstructionValue.Local local = (InstructionValue.Local)tuple.Item2;
					return InstructionDest.Void;
				}
				InstructionValue.Immed immed = (InstructionValue.Immed)tuple.Item2;
				return InstructionDest.OnStack;
			}
			return InstructionDest.Void;
		}
	}

	[Serializable]
	internal sealed class f_load_instr_value_to_dest@5438 : OptimizedClosures.FSharpFunc<cil.CilWriter, InstructionValue, InstructionDest, Unit>
	{
		public cil.GenTypes typs;

		public GenSyms syms;

		public MethodRefInternal mi;

		public cil.Label lab_end;

		public Dictionary<LLVMBasicBlockRef, Block> labels;

		public FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_load_instr_value_to_dest@5438(cil.GenTypes typs, GenSyms syms, MethodRefInternal mi, cil.Label lab_end, Dictionary<LLVMBasicBlockRef, Block> labels, FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value)
		{
			this.typs = typs;
			this.syms = syms;
			this.mi = mi;
			this.lab_end = lab_end;
			this.labels = labels;
			this.f_get_instr_value = f_get_instr_value;
		}

		public override Unit Invoke(cil.CilWriter il, InstructionValue iv, InstructionDest dest)
		{
			if (!(iv is InstructionValue.Temp))
			{
				if (!(iv is InstructionValue.Immed))
				{
					InstructionValue.Local local = (InstructionValue.Local)iv;
					cil.Variable item = local.item;
					il.Append(cil.MyInstruction.NewLdloca(item));
					return null;
				}
				InstructionValue.Immed immed = (InstructionValue.Immed)iv;
				LLVMValueRef op = immed.item;
				gen_instr(typs, syms, il, lab_end, op, mi.args@, mi.extra@, f_get_instr_value, this, dest, labels);
				return null;
			}
			InstructionValue.Temp temp = (InstructionValue.Temp)iv;
			cil.Variable item2 = temp.item;
			il.Append(cil.MyInstruction.NewLdloc(item2));
			return null;
		}
	}

	[Serializable]
	internal sealed class fix_phis@5490 : FSharpFunc<LLVMBasicBlockRef, Unit>
	{
		public cil.GenTypes typs;

		public GenSyms syms;

		public MethodRefInternal mi;

		public cil.CilWriter il;

		public Dictionary<LLVMValueRef, InstructionValue> vals;

		public FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value;

		public FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest;

		public PhiBlockInfo blk_info;

		public Dictionary<LLVMValueRef, cil.Variable> phi_temps;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal fix_phis@5490(cil.GenTypes typs, GenSyms syms, MethodRefInternal mi, cil.CilWriter il, Dictionary<LLVMValueRef, InstructionValue> vals, FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, PhiBlockInfo blk_info, Dictionary<LLVMValueRef, cil.Variable> phi_temps)
		{
			this.typs = typs;
			this.syms = syms;
			this.mi = mi;
			this.il = il;
			this.vals = vals;
			this.f_get_instr_value = f_get_instr_value;
			this.f_load_instr_value_to_dest = f_load_instr_value_to_dest;
			this.blk_info = blk_info;
			this.phi_temps = phi_temps;
		}

		public override Unit Invoke(LLVMBasicBlockRef incoming_blk)
		{
			LLVMValueRef[] phis@ = blk_info.phis@;
			foreach (LLVMValueRef key in phis@)
			{
				LLVMValueRef v = blk_info.incoming_phi_val@[incoming_blk][key];
				outer_load_value(typs, syms, il, mi.args@, f_get_instr_value, f_load_instr_value_to_dest, v);
				cil.Variable value = null;
				Tuple<bool, cil.Variable> tuple = new Tuple<bool, cil.Variable>(phi_temps.TryGetValue(key, out value), value);
				if (tuple.Item1)
				{
					cil.Variable item = tuple.Item2;
					il.Append(cil.MyInstruction.NewStloc(item));
					continue;
				}
				InstructionValue instructionValue = vals[key];
				InstructionValue instructionValue2 = instructionValue;
				if (!(instructionValue2 is InstructionValue.Local))
				{
					if (!(instructionValue2 is InstructionValue.Immed))
					{
						InstructionValue.Temp temp = (InstructionValue.Temp)instructionValue;
						cil.Variable item2 = temp.item;
						il.Append(cil.MyInstruction.NewStloc(item2));
						continue;
					}
					InstructionValue.Immed immed = (InstructionValue.Immed)instructionValue;
					string message = "phi result can't be immed";
					throw Operators.Failure(message);
				}
				InstructionValue.Local local = (InstructionValue.Local)instructionValue;
				string message2 = "phi result won't be alloca";
				throw Operators.Failure(message2);
			}
			Dictionary<LLVMValueRef, cil.Variable> dictionary = phi_temps;
			Dictionary<LLVMValueRef, cil.Variable>.Enumerator enumerator = dictionary.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<LLVMValueRef, cil.Variable> current = enumerator.Current;
					Tuple<LLVMValueRef, cil.Variable> tuple2 = Operators.KeyValuePattern(current);
					cil.Variable item3 = tuple2.Item2;
					LLVMValueRef item4 = tuple2.Item1;
					il.Append(cil.MyInstruction.NewLdloc(item3));
					InstructionValue instructionValue3 = vals[item4];
					InstructionValue instructionValue4 = instructionValue3;
					if (!(instructionValue4 is InstructionValue.Local))
					{
						if (!(instructionValue4 is InstructionValue.Immed))
						{
							InstructionValue.Temp temp2 = (InstructionValue.Temp)instructionValue3;
							cil.Variable item5 = temp2.item;
							il.Append(cil.MyInstruction.NewStloc(item5));
							continue;
						}
						InstructionValue.Immed immed2 = (InstructionValue.Immed)instructionValue3;
						string message3 = "phi result can't be immed";
						throw Operators.Failure(message3);
					}
					InstructionValue.Local local2 = (InstructionValue.Local)instructionValue3;
					string message4 = "phi result won't be alloca";
					throw Operators.Failure(message4);
				}
				return null;
			}
			finally
			{
				((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
		}
	}

	[Serializable]
	internal sealed class fix_opcode@5574 : FSharpFunc<LLVMOpcode, string>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal fix_opcode@5574()
		{
		}

		public override string Invoke(LLVMOpcode x)
		{
			return x.ToString().Substring(4).ToLower();
		}
	}

	[Serializable]
	internal sealed class is_immed@5585 : FSharpFunc<LLVMValueRef, bool>
	{
		public Dictionary<LLVMValueRef, InstructionValue> vals;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal is_immed@5585(Dictionary<LLVMValueRef, InstructionValue> vals)
		{
			this.vals = vals;
		}

		public override bool Invoke(LLVMValueRef v)
		{
			InstructionValue value = null;
			Tuple<bool, InstructionValue> tuple = new Tuple<bool, InstructionValue>(vals.TryGetValue(v, out value), value);
			if (tuple.Item1 && tuple.Item2 is InstructionValue.Immed)
			{
				InstructionValue.Immed immed = (InstructionValue.Immed)tuple.Item2;
				return true;
			}
			return false;
		}
	}

	[Serializable]
	internal sealed class load_value@5603-1 : FSharpFunc<LLVMValueRef, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public cil.GenTypes typs;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public GenSyms syms;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public cil.CilWriter il;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public Dictionary<LLVMValueRef, ParameterDefinition> args;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal load_value@5603-1(cil.GenTypes typs, GenSyms syms, cil.CilWriter il, Dictionary<LLVMValueRef, ParameterDefinition> args, FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest)
		{
			this.typs = typs;
			this.syms = syms;
			this.il = il;
			this.args = args;
			this.f_get_instr_value = f_get_instr_value;
			this.f_load_instr_value_to_dest = f_load_instr_value_to_dest;
		}

		public override Unit Invoke(LLVMValueRef v)
		{
			outer_load_value(typs, syms, il, args, f_get_instr_value, f_load_instr_value_to_dest, v);
			return null;
		}
	}

	[Serializable]
	internal sealed class write_trace_args_instr_opcode_and_ir@5605 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public LLVMValueRef instr;

		public FSharpFunc<LLVMOpcode, string> fix_opcode;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal write_trace_args_instr_opcode_and_ir@5605(cil.CilWriter il, LLVMValueRef instr, FSharpFunc<LLVMOpcode, string> fix_opcode)
		{
			this.il = il;
			this.instr = instr;
			this.fix_opcode = fix_opcode;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			il.Append(cil.MyInstruction.NewLdstr(fix_opcode.Invoke(instr.InstructionOpcode)));
			il.Append(cil.MyInstruction.NewLdstr(one_line(instr.ToString())));
			return null;
		}
	}

	[Serializable]
	internal sealed class write_trace_generic_void@5610 : FSharpFunc<Unit, Unit>
	{
		public GenSyms syms;

		public MethodRefInternal mi;

		public cil.CilWriter il;

		public Dictionary<LLVMValueRef, InstructionValue> vals;

		public LLVMValueRef instr;

		public FSharpFunc<Unit, Unit> write_trace_args_instr_opcode_and_ir;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal write_trace_generic_void@5610(GenSyms syms, MethodRefInternal mi, cil.CilWriter il, Dictionary<LLVMValueRef, InstructionValue> vals, LLVMValueRef instr, FSharpFunc<Unit, Unit> write_trace_args_instr_opcode_and_ir)
		{
			this.syms = syms;
			this.mi = mi;
			this.il = il;
			this.vals = vals;
			this.instr = instr;
			this.write_trace_args_instr_opcode_and_ir = write_trace_args_instr_opcode_and_ir;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			write_trace_args_instr_opcode_and_ir.Invoke(null);
			int num = 0;
			int num2 = 4;
			int num3 = 0;
			int num4 = instr.OperandCount - 1;
			if (num4 >= num3)
			{
				do
				{
					if (num < num2)
					{
						LLVMValueRef operand = instr.GetOperand((uint)num3);
						switch (sgllvm.get_value_kind(operand))
						{
						case LLVMValueKind.LLVMArgumentValueKind:
						{
							ParameterDefinition item2 = mi.args@[operand];
							il.Append(cil.MyInstruction.NewLdarga(item2));
							cil.FirstClassType firstClassType2 = llvm_type_to_firstclass_type(operand.TypeOf);
							il.Append(cil.MyInstruction.NewLdc_I4(cil.get_sizeof(firstClassType2)));
							il.Append(cil.MyInstruction.NewLdc_I4(get_trace_type_code(firstClassType2)));
							num++;
							break;
						}
						case LLVMValueKind.LLVMInstructionValueKind:
						{
							InstructionValue instructionValue = vals[operand];
							InstructionValue instructionValue2 = instructionValue;
							if (!(instructionValue2 is InstructionValue.Temp))
							{
								if (!(instructionValue2 is InstructionValue.Local))
								{
									throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 5629, 46);
								}
								InstructionValue.Local local = (InstructionValue.Local)instructionValue;
								cil.Variable variable = local.item;
							}
							else
							{
								InstructionValue.Temp temp = (InstructionValue.Temp)instructionValue;
								cil.Variable item = temp.item;
								il.Append(cil.MyInstruction.NewLdloca(item));
								cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(operand.TypeOf);
								il.Append(cil.MyInstruction.NewLdc_I4(cil.get_sizeof(firstClassType)));
								il.Append(cil.MyInstruction.NewLdc_I4(get_trace_type_code(firstClassType)));
								num++;
							}
							break;
						}
						}
					}
					num3++;
				}
				while (num3 != num4 + 1);
			}
			cil.CilWriter cilWriter = il;
			FSharpFunc<string, MethodReference> f_get_rt@ = syms.f_get_rt@;
			FSharpFunc<int, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<int, string>, Unit, string, string, int>("trace_misc_void_%d"));
			int func = num;
			cilWriter.Append(cil.MyInstruction.NewCall(f_get_rt@.Invoke(fSharpFunc.Invoke(func))));
			return null;
		}
	}

	[Serializable]
	internal sealed class get_instruction_value_temp@5689 : OptimizedClosures.FSharpFunc<cil.CilWriter, Dictionary<LLVMValueRef, InstructionValue>, LLVMValueRef, cil.Variable>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_instruction_value_temp@5689()
		{
		}

		public override cil.Variable Invoke(cil.CilWriter il, Dictionary<LLVMValueRef, InstructionValue> vals, LLVMValueRef instr)
		{
			InstructionValue instructionValue = vals[instr];
			InstructionValue instructionValue2 = instructionValue;
			if (!(instructionValue2 is InstructionValue.Local))
			{
				if (!(instructionValue2 is InstructionValue.Immed))
				{
					InstructionValue.Temp temp = (InstructionValue.Temp)instructionValue;
					return temp.item;
				}
				InstructionValue.Immed immed = (InstructionValue.Immed)instructionValue;
				string message = "don't do this for an Immed, which has no Temp";
				throw Operators.Failure(message);
			}
			InstructionValue.Local local = (InstructionValue.Local)instructionValue;
			string message2 = "don't do this for a local";
			throw Operators.Failure(message2);
		}
	}

	[Serializable]
	internal sealed class write_trace_args_instr_result@5698 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public Dictionary<LLVMValueRef, InstructionValue> vals;

		public LLVMValueRef instr;

		public FSharpFunc<cil.CilWriter, FSharpFunc<Dictionary<LLVMValueRef, InstructionValue>, FSharpFunc<LLVMValueRef, cil.Variable>>> get_instruction_value_temp;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal write_trace_args_instr_result@5698(cil.CilWriter il, Dictionary<LLVMValueRef, InstructionValue> vals, LLVMValueRef instr, FSharpFunc<cil.CilWriter, FSharpFunc<Dictionary<LLVMValueRef, InstructionValue>, FSharpFunc<LLVMValueRef, cil.Variable>>> get_instruction_value_temp)
		{
			this.il = il;
			this.vals = vals;
			this.instr = instr;
			this.get_instruction_value_temp = get_instruction_value_temp;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			cil.Variable item = FSharpFunc<cil.CilWriter, Dictionary<LLVMValueRef, InstructionValue>>.InvokeFast(get_instruction_value_temp, il, vals, instr);
			il.Append(cil.MyInstruction.NewLdloca(item));
			cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(instr.TypeOf);
			il.Append(cil.MyInstruction.NewLdc_I4(cil.get_sizeof(firstClassType)));
			int item2 = get_trace_type_code(firstClassType);
			il.Append(cil.MyInstruction.NewLdc_I4(item2));
			return null;
		}
	}

	[Serializable]
	internal sealed class write_trace_generic_result@5706 : FSharpFunc<Unit, Unit>
	{
		public GenSyms syms;

		public MethodRefInternal mi;

		public cil.CilWriter il;

		public Dictionary<LLVMValueRef, InstructionValue> vals;

		public LLVMValueRef instr;

		public FSharpFunc<Unit, Unit> write_trace_args_instr_opcode_and_ir;

		public FSharpFunc<Unit, Unit> write_trace_args_instr_result;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal write_trace_generic_result@5706(GenSyms syms, MethodRefInternal mi, cil.CilWriter il, Dictionary<LLVMValueRef, InstructionValue> vals, LLVMValueRef instr, FSharpFunc<Unit, Unit> write_trace_args_instr_opcode_and_ir, FSharpFunc<Unit, Unit> write_trace_args_instr_result)
		{
			this.syms = syms;
			this.mi = mi;
			this.il = il;
			this.vals = vals;
			this.instr = instr;
			this.write_trace_args_instr_opcode_and_ir = write_trace_args_instr_opcode_and_ir;
			this.write_trace_args_instr_result = write_trace_args_instr_result;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			write_trace_args_instr_opcode_and_ir.Invoke(null);
			write_trace_args_instr_result.Invoke(null);
			int num = 0;
			int num2 = 4;
			int num3 = 0;
			int num4 = instr.OperandCount - 1;
			if (num4 >= num3)
			{
				do
				{
					if (num < num2)
					{
						LLVMValueRef operand = instr.GetOperand((uint)num3);
						switch (sgllvm.get_value_kind(operand))
						{
						case LLVMValueKind.LLVMArgumentValueKind:
						{
							ParameterDefinition item2 = mi.args@[operand];
							il.Append(cil.MyInstruction.NewLdarga(item2));
							cil.FirstClassType firstClassType2 = llvm_type_to_firstclass_type(operand.TypeOf);
							il.Append(cil.MyInstruction.NewLdc_I4(cil.get_sizeof(firstClassType2)));
							il.Append(cil.MyInstruction.NewLdc_I4(get_trace_type_code(firstClassType2)));
							num++;
							break;
						}
						case LLVMValueKind.LLVMInstructionValueKind:
						{
							InstructionValue instructionValue = vals[operand];
							InstructionValue instructionValue2 = instructionValue;
							if (!(instructionValue2 is InstructionValue.Temp))
							{
								if (!(instructionValue2 is InstructionValue.Local))
								{
									throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 5726, 46);
								}
								InstructionValue.Local local = (InstructionValue.Local)instructionValue;
								cil.Variable variable = local.item;
							}
							else
							{
								InstructionValue.Temp temp = (InstructionValue.Temp)instructionValue;
								cil.Variable item = temp.item;
								il.Append(cil.MyInstruction.NewLdloca(item));
								cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(operand.TypeOf);
								il.Append(cil.MyInstruction.NewLdc_I4(cil.get_sizeof(firstClassType)));
								il.Append(cil.MyInstruction.NewLdc_I4(get_trace_type_code(firstClassType)));
								num++;
							}
							break;
						}
						}
					}
					num3++;
				}
				while (num3 != num4 + 1);
			}
			cil.CilWriter cilWriter = il;
			FSharpFunc<string, MethodReference> f_get_rt@ = syms.f_get_rt@;
			FSharpFunc<int, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<int, string>, Unit, string, string, int>("trace_misc_result_%d"));
			int func = num;
			cilWriter.Append(cil.MyInstruction.NewCall(f_get_rt@.Invoke(fSharpFunc.Invoke(func))));
			return null;
		}
	}

	[Serializable]
	internal sealed class f@5835-13 : FSharpFunc<Instruction, FSharpOption<TypeDefinition>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@5835-13()
		{
		}

		public override FSharpOption<TypeDefinition> Invoke(Instruction i)
		{
			if (LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(i.Operand, null))
			{
				return null;
			}
			object operand = i.Operand;
			if (operand is MethodDefinition methodDefinition)
			{
				MethodDefinition methodDefinition2 = methodDefinition;
				if (methodDefinition2.DeclaringType.Name.EndsWith("_bridge"))
				{
					return FSharpOption<TypeDefinition>.Some(methodDefinition2.DeclaringType);
				}
				return null;
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class f@5851-14 : FSharpFunc<VariableDefinition, FSharpOption<TypeDefinition>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@5851-14()
		{
		}

		public override FSharpOption<TypeDefinition> Invoke(VariableDefinition i)
		{
			if (i.VariableType.Name.StartsWith("vararg_"))
			{
				return FSharpOption<TypeDefinition>.Some((TypeDefinition)(object)i.VariableType);
			}
			return null;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class CopyMap : IEquatable<CopyMap>, IStructuralEquatable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal cil.GenTypes typs@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<TypeReference, TypeReference> d_type@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<MethodReference, MethodReference> d_method@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<FieldReference, FieldReference> d_field@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public cil.GenTypes typs => typs@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public Dictionary<TypeReference, TypeReference> d_type => d_type@;

		[CompilationMapping(SourceConstructFlags.Field, 2)]
		public Dictionary<MethodReference, MethodReference> d_method => d_method@;

		[CompilationMapping(SourceConstructFlags.Field, 3)]
		public Dictionary<FieldReference, FieldReference> d_field => d_field@;

		public CopyMap(cil.GenTypes typs, Dictionary<TypeReference, TypeReference> d_type, Dictionary<MethodReference, MethodReference> d_method, Dictionary<FieldReference, FieldReference> d_field)
		{
			typs@ = typs;
			d_type@ = d_type;
			d_method@ = d_method;
			d_field@ = d_field;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<CopyMap, string>, Unit, string, string, CopyMap>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, d_field@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, d_method@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, d_type@) + ((num << 6) + (num >> 2)));
				return -1640531527 + (typs@.GetHashCode(comp) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is CopyMap copyMap)
				{
					CopyMap copyMap2 = copyMap;
					cil.GenTypes genTypes = typs@;
					cil.GenTypes obj2 = copyMap2.typs@;
					if (genTypes.Equals(obj2, comp))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, d_type@, copyMap2.d_type@))
						{
							if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, d_method@, copyMap2.d_method@))
							{
								return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, d_field@, copyMap2.d_field@);
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(CopyMap obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (typs@.Equals(obj.typs@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(d_type@, obj.d_type@))
						{
							if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(d_method@, obj.d_method@))
							{
								return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(d_field@, obj.d_field@);
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is CopyMap obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[SpecialName]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[CompilationMapping(SourceConstructFlags.Closure)]
	internal sealed class comparer@5882 : IEqualityComparer<TypeReference>
	{
		int IEqualityComparer<TypeReference>.GetHashCode(TypeReference o)
		{
			return o.GetHashCode();
		}

		bool IEqualityComparer<TypeReference>.Equals(TypeReference a, TypeReference b)
		{
			return object.ReferenceEquals(a, b);
		}
	}

	[Serializable]
	internal sealed class make_d_type@5881 : FSharpFunc<Unit, Dictionary<TypeReference, TypeReference>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal make_d_type@5881()
		{
		}

		public override Dictionary<TypeReference, TypeReference> Invoke(Unit unitVar0)
		{
			IEqualityComparer<TypeReference> comparer = new comparer@5882();
			return new Dictionary<TypeReference, TypeReference>(comparer);
		}
	}

	[Serializable]
	[SpecialName]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[CompilationMapping(SourceConstructFlags.Closure)]
	internal sealed class comparer@5894-1 : IEqualityComparer<MethodReference>
	{
		int IEqualityComparer<MethodReference>.GetHashCode(MethodReference o)
		{
			return o.GetHashCode();
		}

		bool IEqualityComparer<MethodReference>.Equals(MethodReference a, MethodReference b)
		{
			return object.ReferenceEquals(a, b);
		}
	}

	[Serializable]
	internal sealed class make_d_method@5893 : FSharpFunc<Unit, Dictionary<MethodReference, MethodReference>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal make_d_method@5893()
		{
		}

		public override Dictionary<MethodReference, MethodReference> Invoke(Unit unitVar0)
		{
			IEqualityComparer<MethodReference> comparer = new comparer@5894-1();
			return new Dictionary<MethodReference, MethodReference>(comparer);
		}
	}

	[Serializable]
	[SpecialName]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[CompilationMapping(SourceConstructFlags.Closure)]
	internal sealed class comparer@5906-2 : IEqualityComparer<FieldReference>
	{
		int IEqualityComparer<FieldReference>.GetHashCode(FieldReference o)
		{
			return o.GetHashCode();
		}

		bool IEqualityComparer<FieldReference>.Equals(FieldReference a, FieldReference b)
		{
			return object.ReferenceEquals(a, b);
		}
	}

	[Serializable]
	internal sealed class make_d_field@5905 : FSharpFunc<Unit, Dictionary<FieldReference, FieldReference>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal make_d_field@5905()
		{
		}

		public override Dictionary<FieldReference, FieldReference> Invoke(Unit unitVar0)
		{
			IEqualityComparer<FieldReference> comparer = new comparer@5906-2();
			return new Dictionary<FieldReference, FieldReference>(comparer);
		}
	}

	[Serializable]
	internal sealed class instr_new@5983-2 : FSharpFunc<string, Instruction>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, Instruction> clo3;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal instr_new@5983-2(FSharpFunc<string, Instruction> clo3)
		{
			this.clo3 = clo3;
		}

		public override Instruction Invoke(string arg30)
		{
			return clo3.Invoke(arg30);
		}
	}

	[Serializable]
	internal sealed class instr_new@5983-1 : FSharpFunc<object, FSharpFunc<string, Instruction>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<object, FSharpFunc<string, Instruction>> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal instr_new@5983-1(FSharpFunc<object, FSharpFunc<string, Instruction>> clo2)
		{
			this.clo2 = clo2;
		}

		public override FSharpFunc<string, Instruction> Invoke(object arg20)
		{
			FSharpFunc<string, Instruction> clo = clo2.Invoke(arg20);
			return new instr_new@5983-2(clo);
		}
	}

	[Serializable]
	internal sealed class instr_new@5983 : FSharpFunc<OpCode, FSharpFunc<object, FSharpFunc<string, Instruction>>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<OpCode, FSharpFunc<object, FSharpFunc<string, Instruction>>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal instr_new@5983(FSharpFunc<OpCode, FSharpFunc<object, FSharpFunc<string, Instruction>>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<object, FSharpFunc<string, Instruction>> Invoke(OpCode arg10)
		{
			FSharpFunc<object, FSharpFunc<string, Instruction>> clo = clo1.Invoke(arg10);
			return new instr_new@5983-1(clo);
		}
	}

	[Serializable]
	internal sealed class f_handles@6011 : FSharpFunc<FieldDefinition, FSharpOption<FieldDefinition>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_handles@6011()
		{
		}

		public override FSharpOption<FieldDefinition> Invoke(FieldDefinition x)
		{
			if (string.Equals(x.Name, "_handles"))
			{
				return FSharpOption<FieldDefinition>.Some(x);
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class create_methods@6148 : FSharpFunc<MethodReference, bool>
	{
		public LLVMValueRef f;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal create_methods@6148(LLVMValueRef f)
		{
			this.f = f;
		}

		public override bool Invoke(MethodReference x)
		{
			return string.Equals(x.Name, f.Name);
		}
	}

	[Serializable]
	internal sealed class f_get_instr_value@6338-1 : FSharpTypeFunc
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_get_instr_value@6338-1()
		{
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new f_get_instr_value@6338-1T<a>(this);
		}
	}

	[Serializable]
	internal sealed class f_get_instr_value@6338-1T<a> : FSharpTypeFunc
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public f_get_instr_value@6338-1 self0@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_get_instr_value@6338-1T(f_get_instr_value@6338-1 self0@)
		{
			this.self0@ = self0@;
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<b>()
		{
			return new f_get_instr_value@6338-1TT<a, b>(self0@, this);
		}
	}

	[Serializable]
	internal sealed class f_get_instr_value@6338-1TT<a, b> : FSharpFunc<a, b>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public f_get_instr_value@6338-1 self0@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public f_get_instr_value@6338-1T<a> self1@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_get_instr_value@6338-1TT(f_get_instr_value@6338-1 self0@, f_get_instr_value@6338-1T<a> self1@)
		{
			this.self0@ = self0@;
			this.self1@ = self1@;
		}

		public override b Invoke(a v)
		{
			f_get_instr_value@6338-1T<a> f_get_instr_value@6338-1T2 = self1@;
			f_get_instr_value@6338-1 f_get_instr_value@6338-2 = f_get_instr_value@6338-1T2.self0@;
			string message = "no";
			throw Operators.Failure(message);
		}
	}

	[Serializable]
	internal sealed class f_load_instr_value_to_dest@6341-1 : OptimizedClosures.FSharpFunc<cil.CilWriter, InstructionValue, InstructionDest, Unit>
	{
		public cil.GenTypes typs;

		public GenSyms syms;

		public cil.Label lab_end;

		public FSharpTypeFunc f_get_instr_value;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f_load_instr_value_to_dest@6341-1(cil.GenTypes typs, GenSyms syms, cil.Label lab_end, FSharpTypeFunc f_get_instr_value)
		{
			this.typs = typs;
			this.syms = syms;
			this.lab_end = lab_end;
			this.f_get_instr_value = f_get_instr_value;
		}

		public override Unit Invoke(cil.CilWriter il, InstructionValue iv, InstructionDest dest)
		{
			if (!(iv is InstructionValue.Temp))
			{
				if (!(iv is InstructionValue.Immed))
				{
					InstructionValue.Local local = (InstructionValue.Local)iv;
					cil.Variable item = local.item;
					il.Append(cil.MyInstruction.NewLdloca(item));
					return null;
				}
				InstructionValue.Immed immed = (InstructionValue.Immed)iv;
				LLVMValueRef op = immed.item;
				gen_instr(typs, syms, il, lab_end, op, null, null, (FSharpFunc<LLVMValueRef, InstructionValue>)((FSharpTypeFunc)f_get_instr_value.Specialize<LLVMValueRef>()).Specialize<InstructionValue>(), this, dest, null);
				return null;
			}
			InstructionValue.Temp temp = (InstructionValue.Temp)iv;
			cil.Variable item2 = temp.item;
			il.Append(cil.MyInstruction.NewLdloc(item2));
			return null;
		}
	}

	[Serializable]
	internal sealed class gen_cctor@6352 : FSharpFunc<Unit, Unit>
	{
		public cil.CilWriter il;

		public FieldReference field;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal gen_cctor@6352(cil.CilWriter il, FieldReference field)
		{
			this.il = il;
			this.field = field;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			il.Append(cil.MyInstruction.NewLdsfld(field));
			return null;
		}
	}

	[Serializable]
	internal sealed class m_main@6449 : FSharpFunc<MethodDefinition, bool>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal m_main@6449()
		{
		}

		public override bool Invoke(MethodDefinition m)
		{
			return string.Equals(m.Name, "main");
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class RtCopyType : IEquatable<RtCopyType>, IStructuralEquatable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal TypeDefinition tdef@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<MethodDefinition, MethodDefinition> d_method@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<FieldDefinition, FieldDefinition> d_field@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public TypeDefinition tdef => tdef@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public Dictionary<MethodDefinition, MethodDefinition> d_method => d_method@;

		[CompilationMapping(SourceConstructFlags.Field, 2)]
		public Dictionary<FieldDefinition, FieldDefinition> d_field => d_field@;

		public RtCopyType(TypeDefinition tdef, Dictionary<MethodDefinition, MethodDefinition> d_method, Dictionary<FieldDefinition, FieldDefinition> d_field)
		{
			tdef@ = tdef;
			d_method@ = d_method;
			d_field@ = d_field;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<RtCopyType, string>, Unit, string, string, RtCopyType>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, d_field@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, d_method@) + ((num << 6) + (num >> 2)));
				return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, tdef@) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is RtCopyType rtCopyType)
				{
					RtCopyType rtCopyType2 = rtCopyType;
					if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, tdef@, rtCopyType2.tdef@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, d_method@, rtCopyType2.d_method@))
						{
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, d_field@, rtCopyType2.d_field@);
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(RtCopyType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(tdef@, obj.tdef@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(d_method@, obj.d_method@))
						{
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(d_field@, obj.d_field@);
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is RtCopyType obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class RtCopyMap : IEquatable<RtCopyMap>, IStructuralEquatable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal cil.GenTypes typs@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<TypeDefinition, RtCopyType> d_type@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public cil.GenTypes typs => typs@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public Dictionary<TypeDefinition, RtCopyType> d_type => d_type@;

		public RtCopyMap(cil.GenTypes typs, Dictionary<TypeDefinition, RtCopyType> d_type)
		{
			typs@ = typs;
			d_type@ = d_type;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<RtCopyMap, string>, Unit, string, string, RtCopyMap>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, d_type@) + ((num << 6) + (num >> 2)));
				return -1640531527 + (typs@.GetHashCode(comp) + ((num << 6) + (num >> 2)));
			}
			return 0;
		}

		[CompilerGenerated]
		public sealed override int GetHashCode()
		{
			return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
		}

		[CompilerGenerated]
		public sealed bool Equals(object obj, IEqualityComparer comp)
		{
			if (this != null)
			{
				if (obj is RtCopyMap rtCopyMap)
				{
					RtCopyMap rtCopyMap2 = rtCopyMap;
					cil.GenTypes genTypes = typs@;
					cil.GenTypes obj2 = rtCopyMap2.typs@;
					if (genTypes.Equals(obj2, comp))
					{
						return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, d_type@, rtCopyMap2.d_type@);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(RtCopyMap obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (typs@.Equals(obj.typs@))
					{
						return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(d_type@, obj.d_type@);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is RtCopyMap obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[SpecialName]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[CompilationMapping(SourceConstructFlags.Closure)]
	internal sealed class comparer@6493-3 : IEqualityComparer<MethodDefinition>
	{
		int IEqualityComparer<MethodDefinition>.GetHashCode(MethodDefinition o)
		{
			return o.GetHashCode();
		}

		bool IEqualityComparer<MethodDefinition>.Equals(MethodDefinition a, MethodDefinition b)
		{
			return object.ReferenceEquals(a, b);
		}
	}

	[Serializable]
	[SpecialName]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[CompilationMapping(SourceConstructFlags.Closure)]
	internal sealed class comparer@6505-4 : IEqualityComparer<FieldDefinition>
	{
		int IEqualityComparer<FieldDefinition>.GetHashCode(FieldDefinition o)
		{
			return o.GetHashCode();
		}

		bool IEqualityComparer<FieldDefinition>.Equals(FieldDefinition a, FieldDefinition b)
		{
			return object.ReferenceEquals(a, b);
		}
	}

	[Serializable]
	[SpecialName]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[CompilationMapping(SourceConstructFlags.Closure)]
	internal sealed class comparer@6534-5 : IEqualityComparer<TypeDefinition>
	{
		int IEqualityComparer<TypeDefinition>.GetHashCode(TypeDefinition o)
		{
			return o.GetHashCode();
		}

		bool IEqualityComparer<TypeDefinition>.Equals(TypeDefinition a, TypeDefinition b)
		{
			return object.ReferenceEquals(a, b);
		}
	}

	[Serializable]
	internal sealed class inst@6641-2 : FSharpFunc<string, Instruction>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, Instruction> clo3;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal inst@6641-2(FSharpFunc<string, Instruction> clo3)
		{
			this.clo3 = clo3;
		}

		public override Instruction Invoke(string arg30)
		{
			return clo3.Invoke(arg30);
		}
	}

	[Serializable]
	internal sealed class inst@6641-1 : FSharpFunc<object, FSharpFunc<string, Instruction>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<object, FSharpFunc<string, Instruction>> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal inst@6641-1(FSharpFunc<object, FSharpFunc<string, Instruction>> clo2)
		{
			this.clo2 = clo2;
		}

		public override FSharpFunc<string, Instruction> Invoke(object arg20)
		{
			FSharpFunc<string, Instruction> clo = clo2.Invoke(arg20);
			return new inst@6641-2(clo);
		}
	}

	[Serializable]
	internal sealed class inst@6641 : FSharpFunc<OpCode, FSharpFunc<object, FSharpFunc<string, Instruction>>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<OpCode, FSharpFunc<object, FSharpFunc<string, Instruction>>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal inst@6641(FSharpFunc<OpCode, FSharpFunc<object, FSharpFunc<string, Instruction>>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<object, FSharpFunc<string, Instruction>> Invoke(OpCode arg10)
		{
			FSharpFunc<object, FSharpFunc<string, Instruction>> clo = clo1.Invoke(arg10);
			return new inst@6641-1(clo);
		}
	}

	[Serializable]
	internal sealed class f@6782-15 : FSharpFunc<string, MethodReference>
	{
		public Dictionary<string, MethodDefinition> d_ref_rt;

		public RtCopyMap map;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@6782-15(Dictionary<string, MethodDefinition> d_ref_rt, RtCopyMap map)
		{
			this.d_ref_rt = d_ref_rt;
			this.map = map;
		}

		public override MethodReference Invoke(string s)
		{
			MethodDefinition value;
			Tuple<bool, MethodDefinition> tuple = new Tuple<bool, MethodDefinition>(d_ref_rt.TryGetValue(s, out value), value);
			if (tuple.Item1)
			{
				MethodDefinition item = tuple.Item2;
				return rt_find_or_copy_method(map, item);
			}
			FSharpFunc<string, MethodReference> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, MethodReference>, Unit, string, MethodReference, string>("rt function not found: %s"));
			return fSharpFunc.Invoke(s);
		}
	}

	[Serializable]
	internal sealed class f@6795-16 : FSharpFunc<string, MethodReference>
	{
		public Dictionary<string, MethodReference> d_rt;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal f@6795-16(Dictionary<string, MethodReference> d_rt)
		{
			this.d_rt = d_rt;
		}

		public override MethodReference Invoke(string s)
		{
			MethodReference value;
			Tuple<bool, MethodReference> tuple = new Tuple<bool, MethodReference>(d_rt.TryGetValue(s, out value), value);
			if (tuple.Item1)
			{
				return tuple.Item2;
			}
			FSharpFunc<string, MethodReference> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, MethodReference>, Unit, string, MethodReference, string>("rt function not found: %s"));
			return fSharpFunc.Invoke(s);
		}
	}

	[Serializable]
	internal sealed class find_method@6829 : OptimizedClosures.FSharpFunc<ModuleDefinition, Assembly, string, string, Type[], MethodReference>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal find_method@6829()
		{
		}

		public override MethodReference Invoke(ModuleDefinition md, Assembly a, string type_name, string method_name, Type[] parms)
		{
			Type type = a.GetType(type_name);
			Type x = type;
			Type y = default(Type);
			if (!LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(x, y))
			{
				MethodInfo method = type.GetMethod(method_name, parms);
				MethodInfo x2 = method;
				MethodInfo y2 = default(MethodInfo);
				if (!LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(x2, y2))
				{
					return md.ImportReference(method);
				}
				return null;
			}
			return null;
		}
	}

	[CompilationMapping(SourceConstructFlags.Value)]
	public static LLVMModuleRef llvm_module
	{
		get
		{
			return <StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11;
		}
		set
		{
			<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11 = value;
		}
	}

	public static int TC_UNKNOWN
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return 0;
		}
	}

	public static int TC_I1
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return 1;
		}
	}

	public static int TC_I8
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return 2;
		}
	}

	public static int TC_I16
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return 3;
		}
	}

	public static int TC_I32
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return 4;
		}
	}

	public static int TC_I64
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return 5;
		}
	}

	public static int TC_PTR
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return 6;
		}
	}

	public static int TC_FUNNY
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return 7;
		}
	}

	public static int TC_S_I64_I1
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return 8;
		}
	}

	public static int TC_S_I64_I64
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return 9;
		}
	}

	public static int TC_V_I64
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return 10;
		}
	}

	public static int TC_V_I32
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		get
		{
			return 11;
		}
	}

	public static string one_line(string s)
	{
		return s.Replace("\t", " ").Replace("\r", " ").Replace("\n", " ")
			.Replace("    ", " ")
			.Replace("  ", " ")
			.Replace("  ", " ")
			.Replace("  ", " ")
			.Replace("  ", " ")
			.Trim();
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static V d_get_or_add<K, V>(Dictionary<K, V> d, K k, FSharpFunc<Unit, V> f)
	{
		V value;
		Tuple<bool, V> tuple = new Tuple<bool, V>(d.TryGetValue(k, out value), value);
		if (tuple.Item1)
		{
			return tuple.Item2;
		}
		V val = f.Invoke(null);
		d.Add(k, val);
		return val;
	}

	internal static string vecprim_to_prim_name(cil.VecPrimitiveType vp)
	{
		return vp.Tag switch
		{
			1 => "i16", 
			2 => "i32", 
			3 => "i64", 
			4 => "f32", 
			5 => "f64", 
			_ => "i8", 
		};
	}

	public static cil.VectorElementType to_vec_elem_type(cil.FirstClassType fc)
	{
		switch (fc.Tag)
		{
		case 5:
			return cil.VectorElementType.VecPtr;
		case 0:
		{
			cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)fc;
			cil.PrimitiveType item2 = primitiveType.Item;
			return item2.Tag switch
			{
				1 => cil.VectorElementType.NewVecPrim(cil.VecPrimitiveType.V_I8), 
				2 => cil.VectorElementType.NewVecPrim(cil.VecPrimitiveType.V_I16), 
				3 => cil.VectorElementType.NewVecPrim(cil.VecPrimitiveType.V_I32), 
				4 => cil.VectorElementType.NewVecPrim(cil.VecPrimitiveType.V_I64), 
				5 => cil.VectorElementType.NewVecPrim(cil.VecPrimitiveType.V_F32), 
				6 => cil.VectorElementType.NewVecPrim(cil.VecPrimitiveType.V_F64), 
				_ => cil.VectorElementType.VecBit, 
			};
		}
		case 4:
		{
			cil.FirstClassType.FunnyIntegerType funnyIntegerType = (cil.FirstClassType.FunnyIntegerType)fc;
			cil.FunnyIntegerType item = funnyIntegerType.Item;
			return cil.VectorElementType.NewVecFunny(item);
		}
		default:
		{
			FSharpFunc<cil.FirstClassType, cil.VectorElementType> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, cil.VectorElementType>, Unit, string, cil.VectorElementType, cil.FirstClassType>("invalid to_vec_elem_type on %A"));
			return fSharpFunc.Invoke(fc);
		}
		}
	}

	public static cil.PrimitiveType to_primitivetype(cil.FirstClassType fc)
	{
		if (fc.Tag == 0)
		{
			cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)fc;
			return primitiveType.Item;
		}
		FSharpFunc<cil.FirstClassType, cil.PrimitiveType> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, cil.PrimitiveType>, Unit, string, cil.PrimitiveType, cil.FirstClassType>("invalid to_primitivetype on %A"));
		return fSharpFunc.Invoke(fc);
	}

	internal static cil.FirstClassType llvm_type_to_firstclass_type(LLVMTypeRef t)
	{
		switch (t.Kind)
		{
		case LLVMTypeKind.LLVMPointerTypeKind:
			return cil.FirstClassType.Ptr;
		case LLVMTypeKind.LLVMIntegerTypeKind:
		{
			uint intWidth = t.IntWidth;
			switch (intWidth)
			{
			case 1u:
				return cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.I1);
			case 8u:
				return cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.I8);
			case 16u:
				return cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.I16);
			case 32u:
				return cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.I32);
			case 64u:
				return cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.I64);
			default:
			{
				uint bits = intWidth;
				return cil.FirstClassType.NewFunnyIntegerType(new cil.FunnyIntegerType((int)bits));
			}
			}
		}
		case LLVMTypeKind.LLVMFloatTypeKind:
			return cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.F32);
		case LLVMTypeKind.LLVMDoubleTypeKind:
			return cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.F64);
		case LLVMTypeKind.LLVMX86_FP80TypeKind:
			return cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.F64);
		case LLVMTypeKind.LLVMFP128TypeKind:
			return cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.F64);
		case LLVMTypeKind.LLVMVectorTypeKind:
		{
			cil.VectorElementType elemtype2 = to_vec_elem_type(llvm_type_to_firstclass_type(t.ElementType));
			uint vectorSize = t.VectorSize;
			cil.VectorType item3 = new cil.VectorType(vectorSize, elemtype2, sgllvm.get_size_in_bits(llvm_module, t));
			return cil.FirstClassType.NewVectorType(item3);
		}
		case LLVMTypeKind.LLVMArrayTypeKind:
		{
			cil.FirstClassType elemtype = llvm_type_to_firstclass_type(t.ElementType);
			uint arrayLength = t.ArrayLength;
			cil.ArrayType item2 = new cil.ArrayType(arrayLength, elemtype, sgllvm.get_size_in_bits(llvm_module, t));
			return cil.FirstClassType.NewArrayType(item2);
		}
		case LLVMTypeKind.LLVMStructTypeKind:
		{
			List<cil.StructItem> list = new List<cil.StructItem>();
			sgllvm.StructInfo structInfo = sgllvm.get_struct_element_types(llvm_module, t);
			sgllvm.StructElement[] items = structInfo.items;
			for (int i = 0; i < items.Length; i++)
			{
				sgllvm.StructElement structElement = items[i];
				cil.FirstClassType typ = llvm_type_to_firstclass_type(structElement.typ);
				cil.StructItem item = new cil.StructItem(typ, structElement.off);
				list.Add(item);
			}
			return cil.FirstClassType.NewStructType(new cil.StructType(t.StructName, ArrayModule.OfSeq(list), structInfo.size_in_bits));
		}
		default:
		{
			FSharpFunc<LLVMTypeKind, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<LLVMTypeKind, Unit>, TextWriter, Unit, Unit, LLVMTypeKind>("t.Kind: %A"));
			LLVMTypeKind kind = t.Kind;
			fSharpFunc.Invoke(kind);
			FSharpFunc<LLVMTypeRef, cil.FirstClassType> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMTypeRef, cil.FirstClassType>, Unit, string, cil.FirstClassType, LLVMTypeRef>("unknown type: %A"));
			LLVMTypeRef func = t;
			return fSharpFunc2.Invoke(func);
		}
		}
	}

	public static long make_mask_64(cil.FunnyIntegerType ft)
	{
		ulong num = 0uL;
		ulong num2 = ~num;
		ulong num3 = num2;
		int num4 = 64 - ft.bits;
		return (long)(num3 >> num4);
	}

	public static int make_mask_32(cil.FunnyIntegerType ft)
	{
		uint num = 0u;
		uint num2 = ~num;
		uint num3 = num2;
		int num4 = 32 - ft.bits;
		return (int)(num3 >> num4);
	}

	public static FSharpOption<long> get_maybe_integer_constant(LLVMValueRef v)
	{
		if (sgllvm.get_value_kind(v) == LLVMValueKind.LLVMConstantIntValueKind)
		{
			return FSharpOption<long>.Some(v.ConstIntSExt);
		}
		return null;
	}

	internal static long get_integer_constant(LLVMValueRef v)
	{
		LLVMValueKind lLVMValueKind = sgllvm.get_value_kind(v);
		switch (lLVMValueKind)
		{
		case LLVMValueKind.LLVMConstantIntValueKind:
			return v.ConstIntSExt;
		case LLVMValueKind.LLVMUndefValueValueKind:
			return 0L;
		default:
		{
			FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, FSharpFunc<string, long>>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, FSharpFunc<string, long>>>, Unit, string, long, Tuple<LLVMValueKind, LLVMValueRef, string>>("TODO int constant: vk: %A  v: %A  typ: %s"));
			return FSharpFunc<LLVMValueKind, LLVMValueRef>.InvokeFast(new get_integer_constant@222(clo), lLVMValueKind, v, v.GetType().FullName);
		}
		}
	}

	public static int get_trace_type_code(cil.FirstClassType fctyp)
	{
		switch (fctyp.Tag)
		{
		case 0:
		{
			cil.FirstClassType.PrimitiveType primitiveType3 = (cil.FirstClassType.PrimitiveType)fctyp;
			switch (primitiveType3.Item.Tag)
			{
			case 4:
				return TC_I64;
			case 3:
				return TC_I32;
			case 2:
				return TC_I16;
			case 1:
				return TC_I8;
			case 0:
				return TC_I8;
			}
			break;
		}
		case 1:
		{
			cil.FirstClassType.VectorType vectorType = (cil.FirstClassType.VectorType)fctyp;
			if (vectorType.Item.elemtype.Tag == 2)
			{
				cil.VectorElementType.VecPrim vecPrim = (cil.VectorElementType.VecPrim)vectorType.Item.elemtype;
				switch (vecPrim.Item.Tag)
				{
				case 3:
				{
					uint count2 = vectorType.Item.count;
					return TC_V_I64;
				}
				case 2:
				{
					uint count = vectorType.Item.count;
					return TC_V_I32;
				}
				}
			}
			break;
		}
		case 2:
		{
			cil.FirstClassType.StructType structType = (cil.FirstClassType.StructType)fctyp;
			cil.StructItem[] items = structType.Item.items;
			if (items == null || items.Length != 2 || structType.Item.items[0].typ.Tag != 0)
			{
				break;
			}
			cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)structType.Item.items[0].typ;
			if (primitiveType.Item.Tag == 4 && structType.Item.items[1].typ.Tag == 0)
			{
				cil.FirstClassType.PrimitiveType primitiveType2 = (cil.FirstClassType.PrimitiveType)structType.Item.items[1].typ;
				switch (primitiveType2.Item.Tag)
				{
				case 0:
					return TC_S_I64_I1;
				case 4:
					return TC_S_I64_I64;
				}
			}
			break;
		}
		case 5:
			return TC_PTR;
		case 4:
		{
			cil.FunnyIntegerType item = ((cil.FirstClassType.FunnyIntegerType)fctyp).Item;
			return TC_FUNNY;
		}
		}
		return TC_UNKNOWN;
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static void store_mem_primitive_from_stack(cil.CilWriter il, cil.PrimitiveType pt)
	{
		switch (pt.Tag)
		{
		case 1:
			il.Append(cil.MyInstruction.Stind_I1);
			return;
		case 2:
			il.Append(cil.MyInstruction.Stind_I2);
			return;
		case 3:
			il.Append(cil.MyInstruction.Stind_I4);
			return;
		case 4:
			il.Append(cil.MyInstruction.Stind_I8);
			return;
		case 5:
			il.Append(cil.MyInstruction.Stind_R4);
			return;
		case 6:
			il.Append(cil.MyInstruction.Stind_R8);
			return;
		}
		il.Append(cil.MyInstruction.NewLdc_I4(1));
		il.Append(cil.MyInstruction.And);
		il.Append(cil.MyInstruction.Stind_I1);
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static void store_vecprim_from_stack(cil.CilWriter il, cil.VecPrimitiveType pt)
	{
		switch (pt.Tag)
		{
		default:
			il.Append(cil.MyInstruction.Stind_I1);
			break;
		case 1:
			il.Append(cil.MyInstruction.Stind_I2);
			break;
		case 2:
			il.Append(cil.MyInstruction.Stind_I4);
			break;
		case 3:
			il.Append(cil.MyInstruction.Stind_I8);
			break;
		case 4:
			il.Append(cil.MyInstruction.Stind_R4);
			break;
		case 5:
			il.Append(cil.MyInstruction.Stind_R8);
			break;
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static void il_comment(cil.CilWriter il, string s)
	{
		il.Append(cil.MyInstruction.NewLdstr("comment: " + s));
		il.Append(cil.MyInstruction.Pop);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1 })]
	public static cil.FirstClassType walk_constant_indexes(cil.CilWriter il, cil.FirstClassType typ, int[] indexes, int cur)
	{
		cil.FirstClassType firstClassType3;
		while (true)
		{
			cil.FirstClassType firstClassType = typ;
			cil.FirstClassType firstClassType2;
			switch (firstClassType.Tag)
			{
			case 2:
			{
				cil.FirstClassType.StructType structType = (cil.FirstClassType.StructType)firstClassType;
				cil.StructType item2 = structType.Item;
				int num4 = indexes[cur];
				il.Append(cil.MyInstruction.NewLdc_I4((int)item2.items[num4].off));
				il.Append(cil.MyInstruction.Add);
				firstClassType2 = item2.items[num4].typ;
				break;
			}
			case 3:
			{
				cil.FirstClassType.ArrayType arrayType = (cil.FirstClassType.ArrayType)firstClassType;
				cil.ArrayType item3 = arrayType.Item;
				int num5 = indexes[cur];
				int num6 = cil.get_sizeof(item3.elemtype) * num5;
				if (num6 != 0)
				{
					il.Append(cil.MyInstruction.NewLdc_I4(num6));
					il.Append(cil.MyInstruction.Add);
				}
				firstClassType2 = item3.elemtype;
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType = (cil.FirstClassType.VectorType)firstClassType;
				cil.VectorType item = vectorType.Item;
				int num = indexes[cur];
				int num2 = cil.sizeof_vec_elem(item.elemtype);
				int num3 = num * num2;
				if (num3 != 0)
				{
					il.Append(cil.MyInstruction.NewLdc_I4(num3));
					il.Append(cil.MyInstruction.Add);
				}
				firstClassType2 = cil.vec_elem_type_to_first_class(item.elemtype);
				break;
			}
			default:
			{
				FSharpFunc<cil.FirstClassType, cil.FirstClassType> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, cil.FirstClassType>, Unit, string, cil.FirstClassType, cil.FirstClassType>("walk_constant_indexes for %A"));
				cil.FirstClassType func = typ;
				firstClassType2 = fSharpFunc.Invoke(func);
				break;
			}
			}
			firstClassType3 = firstClassType2;
			if (cur == indexes.Length - 1)
			{
				break;
			}
			cil.CilWriter cilWriter = il;
			int[] array = indexes;
			cur++;
			indexes = array;
			typ = firstClassType3;
			il = cilWriter;
		}
		return firstClassType3;
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1 })]
	internal static void do_vec(cil.CilWriter il, cil.PrimitiveType elem_typ, uint n, FSharpFunc<cil.CilWriter, Unit> load_base_ptr, FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>> load_nth_val)
	{
		int num = cil.sizeof_primitive(elem_typ);
		int num2 = 0;
		int num3 = (int)(n - 1);
		if (num3 < num2)
		{
			return;
		}
		do
		{
			load_base_ptr.Invoke(il);
			if (num2 > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(num2 * num));
				il.Append(cil.MyInstruction.Add);
			}
			FSharpFunc<cil.CilWriter, int>.InvokeFast(load_nth_val, il, num2);
			store_mem_primitive_from_stack(il, elem_typ);
			num2++;
		}
		while (num2 != num3 + 1);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1 })]
	internal static void do_vec_into_local(cil.CilWriter il, cil.PrimitiveType etype, uint n, cil.Variable result, FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>> load_nth_val)
	{
		do_vec(il, etype, n, new do_vec_into_local@340(result), load_nth_val);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1 })]
	internal static void do_ptrvec(cil.CilWriter il, uint n, FSharpFunc<cil.CilWriter, Unit> load_base_ptr, FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>> load_nth_val)
	{
		int num = cil.get_sizeof(cil.FirstClassType.Ptr);
		int num2 = 0;
		int num3 = (int)(n - 1);
		if (num3 < num2)
		{
			return;
		}
		do
		{
			load_base_ptr.Invoke(il);
			if (num2 > 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(num2 * num));
				il.Append(cil.MyInstruction.Add);
			}
			FSharpFunc<cil.CilWriter, int>.InvokeFast(load_nth_val, il, num2);
			il.Append(cil.MyInstruction.Stind_I);
			num2++;
		}
		while (num2 != num3 + 1);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1 })]
	internal static void do_ptrvec_into_local(cil.CilWriter il, uint n, cil.Variable result, FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>> load_nth_val)
	{
		do_ptrvec(il, n, new do_ptrvec_into_local@355(result), load_nth_val);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static void load_value_function(GenSyms syms, cil.CilWriter il, LLVMValueRef f)
	{
		MethodReference methodReference = syms.d_methods@[f];
		MethodReference x = methodReference;
		MethodReference y = default(MethodReference);
		if (!LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(x, y))
		{
			il.Append(cil.MyInstruction.NewLdftn(methodReference));
			return;
		}
		FSharpFunc<string, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("Ldftn Missing method: %s"));
		string name = f.Name;
		string text = fSharpFunc.Invoke(name);
		FSharpFunc<string, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("%s"));
		string func = text;
		fSharpFunc2.Invoke(func);
		il.Append(cil.MyInstruction.NewLdstr(text));
		il.Append(cil.MyInstruction.NewNewobj(syms.ctor_exception@));
		il.Append(cil.MyInstruction.Throw);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static void load_value_global(GenSyms syms, cil.CilWriter il, LLVMValueRef g)
	{
		FieldReference fieldReference = syms.d_globals@[g];
		FieldReference x = fieldReference;
		FieldReference y = default(FieldReference);
		if (!LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(x, y))
		{
			il.Append(cil.MyInstruction.NewLdsfld(fieldReference));
			return;
		}
		FSharpFunc<string, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("Ldsfld Missing global: %s"));
		string name = g.Name;
		string text = fSharpFunc.Invoke(name);
		FSharpFunc<string, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("%s"));
		string func = text;
		fSharpFunc2.Invoke(func);
		il.Append(cil.MyInstruction.NewLdstr(text));
		il.Append(cil.MyInstruction.NewNewobj(syms.ctor_exception@));
		il.Append(cil.MyInstruction.Throw);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static void load_value_constant_ptr(GenSyms syms, cil.CilWriter il, LLVMValueRef v)
	{
		while (true)
		{
			switch (sgllvm.get_value_kind(v))
			{
			case LLVMValueKind.LLVMFunctionValueKind:
				load_value_function(syms, il, v);
				return;
			case LLVMValueKind.LLVMGlobalVariableValueKind:
				load_value_global(syms, il, v);
				return;
			case LLVMValueKind.LLVMConstantPointerNullValueKind:
				il.Append(cil.MyInstruction.Ldnull);
				return;
			case LLVMValueKind.LLVMConstantExprValueKind:
				switch (v.ConstOpcode)
				{
				case LLVMOpcode.LLVMGetElementPtr:
					load_value_constant_gep(syms, il, v);
					return;
				case LLVMOpcode.LLVMBitCast:
					break;
				default:
				{
					FSharpFunc<LLVMValueRef, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, Unit, string, Unit, LLVMValueRef>("TODO const expr: %A"));
					LLVMValueRef func = v;
					fSharpFunc.Invoke(func);
					return;
				}
				}
				break;
			default:
			{
				FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>>, Unit, string, Unit, Tuple<string, LLVMValueRef>>("load_value_constant_ptr: %s %A"));
				FSharpFunc<string, LLVMValueRef>.InvokeFast(new load_value_constant_ptr@399(clo), v.GetType().FullName, v);
				return;
			}
			}
			LLVMValueRef operand = v.GetOperand(0u);
			GenSyms genSyms = syms;
			cil.CilWriter cilWriter = il;
			v = operand;
			il = cilWriter;
			syms = genSyms;
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static void load_value_constant_gep(GenSyms syms, cil.CilWriter il, LLVMValueRef c)
	{
		LLVMValueRef operand = c.GetOperand(0u);
		switch (operand.TypeOf.Kind)
		{
		case LLVMTypeKind.LLVMPointerTypeKind:
		{
			cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(operand.TypeOf.ElementType);
			load_value_constant_ptr(syms, il, operand);
			long num = get_integer_constant(c.GetOperand(1u));
			long num2 = num;
			int num3 = (int)num2;
			int num4 = cil.get_sizeof(firstClassType) * num3;
			if (num4 != 0)
			{
				il.Append(cil.MyInstruction.NewLdc_I4(num4));
				il.Append(cil.MyInstruction.Add);
			}
			int[] array = ArrayModule.OfSeq(SeqModule.Map(new indexes@424(), SeqModule.Skip(2, sgllvm.get_operands(c))));
			if (array.Length > 0)
			{
				cil.FirstClassType firstClassType2 = walk_constant_indexes(il, firstClassType, array, 0);
			}
			break;
		}
		default:
		{
			string message = "must be pointer";
			throw Operators.Failure(message);
		}
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static void emit_strange_icmp(cil.CilWriter il, GenSyms syms, LLVMIntPredicate pred)
	{
		switch (pred)
		{
		case LLVMIntPredicate.LLVMIntEQ:
			il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_eq")));
			break;
		case LLVMIntPredicate.LLVMIntUGT:
			il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_ugt")));
			break;
		case LLVMIntPredicate.LLVMIntSGT:
			il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_sgt")));
			break;
		case LLVMIntPredicate.LLVMIntULT:
			il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_ult")));
			break;
		case LLVMIntPredicate.LLVMIntSLT:
			il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_slt")));
			break;
		case LLVMIntPredicate.LLVMIntNE:
			il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_ne")));
			break;
		case LLVMIntPredicate.LLVMIntUGE:
			il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_uge")));
			break;
		case LLVMIntPredicate.LLVMIntSGE:
			il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_sge")));
			break;
		case LLVMIntPredicate.LLVMIntULE:
			il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_ule")));
			break;
		case LLVMIntPredicate.LLVMIntSLE:
			il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("strange_sle")));
			break;
		default:
		{
			FSharpFunc<LLVMIntPredicate, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMIntPredicate, Unit>, Unit, string, Unit, LLVMIntPredicate>("unknown strange int predicate: %A"));
			fSharpFunc.Invoke(pred);
			break;
		}
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static void emit_icmp(cil.CilWriter il, LLVMIntPredicate pred)
	{
		switch (pred)
		{
		case LLVMIntPredicate.LLVMIntEQ:
			il.Append(cil.MyInstruction.Ceq);
			break;
		case LLVMIntPredicate.LLVMIntUGT:
			il.Append(cil.MyInstruction.Cgt_Un);
			break;
		case LLVMIntPredicate.LLVMIntSGT:
			il.Append(cil.MyInstruction.Cgt);
			break;
		case LLVMIntPredicate.LLVMIntULT:
			il.Append(cil.MyInstruction.Clt_Un);
			break;
		case LLVMIntPredicate.LLVMIntSLT:
			il.Append(cil.MyInstruction.Clt);
			break;
		case LLVMIntPredicate.LLVMIntNE:
			il.Append(cil.MyInstruction.Ceq);
			il.Append(cil.MyInstruction.NewLdc_I4(0));
			il.Append(cil.MyInstruction.Ceq);
			break;
		case LLVMIntPredicate.LLVMIntSGE:
			il.Append(cil.MyInstruction.Clt);
			il.Append(cil.MyInstruction.NewLdc_I4(0));
			il.Append(cil.MyInstruction.Ceq);
			break;
		case LLVMIntPredicate.LLVMIntUGE:
			il.Append(cil.MyInstruction.Clt_Un);
			il.Append(cil.MyInstruction.NewLdc_I4(0));
			il.Append(cil.MyInstruction.Ceq);
			break;
		case LLVMIntPredicate.LLVMIntSLE:
			il.Append(cil.MyInstruction.Cgt);
			il.Append(cil.MyInstruction.NewLdc_I4(0));
			il.Append(cil.MyInstruction.Ceq);
			break;
		case LLVMIntPredicate.LLVMIntULE:
			il.Append(cil.MyInstruction.Cgt_Un);
			il.Append(cil.MyInstruction.NewLdc_I4(0));
			il.Append(cil.MyInstruction.Ceq);
			break;
		default:
		{
			FSharpFunc<LLVMIntPredicate, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMIntPredicate, Unit>, Unit, string, Unit, LLVMIntPredicate>("unknown int predicate: %A"));
			fSharpFunc.Invoke(pred);
			break;
		}
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1 })]
	internal static void cpblk_into(cil.CilWriter il, cil.Variable dest, FSharpFunc<Unit, Unit> f_load_src, int siz)
	{
		if (siz == 0)
		{
			string message = "zero size";
			throw Operators.Failure(message);
		}
		int num = cil.get_sizeof_var(dest);
		if (num > siz)
		{
		}
		if (num < siz)
		{
			FSharpFunc<int, FSharpFunc<int, Unit>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<int, FSharpFunc<int, Unit>>, Unit, string, Unit, Tuple<int, int>>("wrong size too much: siz_dest=%d  ask=%d"));
			FSharpFunc<int, int>.InvokeFast(new cpblk_into@490(clo), num, siz);
		}
		il.Append(cil.MyInstruction.NewLdloca(dest));
		f_load_src.Invoke(null);
		il.Append(cil.MyInstruction.NewLdc_I4(siz));
		il.Append(cil.MyInstruction.Cpblk);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1 })]
	internal static void cpblk_into_from_ldloc(cil.CilWriter il, cil.Variable dest, cil.Variable from, int siz)
	{
		cil.GeneralType typ = from.typ;
		if (typ.Tag == 3)
		{
			cil.GeneralType.FirstClassType firstClassType = (cil.GeneralType.FirstClassType)typ;
			if (firstClassType.Item.Tag == 5)
			{
				FSharpFunc<Unit, Unit> f_load_src = new f_load_src@505(il, from);
				cpblk_into(il, dest, f_load_src, siz);
				return;
			}
		}
		string message = "must be pointer";
		throw Operators.Failure(message);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1 })]
	internal static void cpblk_into_from_ldloca(cil.CilWriter il, cil.Variable dest, cil.Variable from, int siz)
	{
		FSharpFunc<Unit, Unit> f_load_src = new f_load_src@509-1(il, from);
		int num = cil.get_sizeof_var(from);
		if (siz > num)
		{
			FSharpFunc<int, FSharpFunc<int, Unit>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<int, FSharpFunc<int, Unit>>, Unit, string, Unit, Tuple<int, int>>("wrong size too much: siz_from=%d  ask=%d"));
			FSharpFunc<int, int>.InvokeFast(new cpblk_into_from_ldloca@516(clo), num, siz);
		}
		cpblk_into(il, dest, f_load_src, siz);
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	internal static void init_local(cil.CilWriter il, cil.Variable dest)
	{
		il.Append(cil.MyInstruction.NewLdloca(dest));
		il.Append(cil.MyInstruction.NewLdc_I4(0));
		int item = cil.get_sizeof_var(dest);
		il.Append(cil.MyInstruction.NewLdc_I4(item));
		il.Append(cil.MyInstruction.Initblk);
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	internal static void store_typ(cil.CilWriter il, cil.FirstClassType typ)
	{
		switch (typ.Tag)
		{
		case 1:
		{
			cil.FirstClassType.VectorType vectorType = (cil.FirstClassType.VectorType)typ;
			break;
		}
		case 2:
		{
			cil.FirstClassType.StructType structType = (cil.FirstClassType.StructType)typ;
			break;
		}
		case 3:
		{
			cil.FirstClassType.ArrayType arrayType = (cil.FirstClassType.ArrayType)typ;
			break;
		}
		case 4:
		{
			cil.FirstClassType.FunnyIntegerType funnyIntegerType = (cil.FirstClassType.FunnyIntegerType)typ;
			break;
		}
		default:
		{
			cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)typ;
			cil.PrimitiveType item = primitiveType.Item;
			store_mem_primitive_from_stack(il, item);
			return;
		}
		case 5:
			il.Append(cil.MyInstruction.Stind_I);
			return;
		}
		il.Append(cil.MyInstruction.NewLdc_I4(cil.get_sizeof(typ)));
		il.Append(cil.MyInstruction.Cpblk);
	}

	internal static bool is_llvm_overflow_intrinsic(string s)
	{
		if (s.StartsWith("llvm.uadd.with.overflow.") || s.StartsWith("llvm.usub.with.overflow.") || s.StartsWith("llvm.umul.with.overflow.") || s.StartsWith("llvm.sadd.with.overflow.") || s.StartsWith("llvm.ssub.with.overflow."))
		{
			return true;
		}
		return s.StartsWith("llvm.smul.with.overflow.");
	}

	internal static bool is_llvm_sat_intrinsic(string s)
	{
		if (s.StartsWith("llvm.uadd.sat.") || s.StartsWith("llvm.usub.sat.") || s.StartsWith("llvm.umul.sat.") || s.StartsWith("llvm.sadd.sat.") || s.StartsWith("llvm.ssub.sat."))
		{
			return true;
		}
		return s.StartsWith("llvm.smul.sat.");
	}

	internal static bool want_trace_instruction(TraceInstruction t)
	{
		return t.Tag != 1;
	}

	internal static int[] get_constant_vector_as_array(LLVMValueRef v)
	{
		int vectorSize = (int)v.TypeOf.VectorSize;
		int[] array = ArrayModule.ZeroCreate<int>(vectorSize);
		switch (sgllvm.get_value_kind(v))
		{
		case LLVMValueKind.LLVMConstantDataVectorValueKind:
		{
			int num6 = 0;
			int num7 = vectorSize - 1;
			if (num7 >= num6)
			{
				do
				{
					long constIntSExt2 = v.GetElementAsConstant((uint)num6).ConstIntSExt;
					long num8 = constIntSExt2;
					int num9 = (int)num8;
					array[num6] = num9;
					num6++;
				}
				while (num6 != num7 + 1);
			}
			break;
		}
		case LLVMValueKind.LLVMUndefValueValueKind:
		{
			int num10 = 0;
			int num11 = vectorSize - 1;
			if (num11 >= num10)
			{
				do
				{
					array[num10] = -1;
					num10++;
				}
				while (num10 != num11 + 1);
			}
			break;
		}
		case LLVMValueKind.LLVMConstantVectorValueKind:
		{
			int num = 0;
			int num2 = vectorSize - 1;
			if (num2 < num)
			{
				break;
			}
			do
			{
				LLVMValueRef operand = v.GetOperand((uint)num);
				LLVMValueKind lLVMValueKind = sgllvm.get_value_kind(operand);
				int num3;
				switch (lLVMValueKind)
				{
				case LLVMValueKind.LLVMConstantIntValueKind:
				{
					long constIntSExt = operand.ConstIntSExt;
					long num4 = constIntSExt;
					num3 = (int)num4;
					break;
				}
				case LLVMValueKind.LLVMUndefValueValueKind:
					num3 = -1;
					break;
				default:
				{
					LLVMValueKind lLVMValueKind2 = lLVMValueKind;
					FSharpFunc<LLVMValueKind, int> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueKind, int>, Unit, string, int, LLVMValueKind>("match case: %A"));
					LLVMValueKind func = lLVMValueKind2;
					num3 = fSharpFunc.Invoke(func);
					break;
				}
				}
				int num5 = num3;
				array[num] = num5;
				num++;
			}
			while (num != num2 + 1);
			break;
		}
		default:
			throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 570, 14);
		case LLVMValueKind.LLVMConstantAggregateZeroValueKind:
			break;
		}
		return array;
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	internal static void outer_load_arg(cil.CilWriter il, Dictionary<LLVMValueRef, ParameterDefinition> args, LLVMValueRef v)
	{
		LLVMValueKind lLVMValueKind = sgllvm.get_value_kind(v);
		switch (lLVMValueKind)
		{
		case LLVMValueKind.LLVMArgumentValueKind:
		{
			ParameterDefinition value;
			Tuple<bool, ParameterDefinition> tuple = new Tuple<bool, ParameterDefinition>(args.TryGetValue(v, out value), value);
			if (tuple.Item1)
			{
				ParameterDefinition item = tuple.Item2;
				il.Append(cil.MyInstruction.NewLdarg(item));
			}
			else
			{
				FSharpFunc<LLVMValueRef, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, Unit, string, Unit, LLVMValueRef>("load_value arg not found: %A"));
				fSharpFunc.Invoke(v);
			}
			break;
		}
		default:
		{
			LLVMValueKind arg = lLVMValueKind;
			FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>>, Unit, string, Unit, Tuple<LLVMValueKind, LLVMValueRef>>("TODO: %A -- %A"));
			FSharpFunc<LLVMValueKind, LLVMValueRef>.InvokeFast(new outer_load_arg@640(clo), arg, v);
			break;
		}
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1, 1 })]
	internal static void store_constant_at(GenSyms syms, cil.CilWriter il, cil.FirstClassType typ, FSharpFunc<Unit, Unit> f_get_addr, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, LLVMValueRef v)
	{
		while (true)
		{
			LLVMValueKind lLVMValueKind = sgllvm.get_value_kind(v);
			switch (lLVMValueKind)
			{
			case LLVMValueKind.LLVMUndefValueValueKind:
				return;
			case LLVMValueKind.LLVMFunctionValueKind:
				f_get_addr.Invoke(null);
				load_value_function(syms, il, v);
				il.Append(cil.MyInstruction.Stind_I);
				return;
			case LLVMValueKind.LLVMGlobalVariableValueKind:
				f_get_addr.Invoke(null);
				load_value_global(syms, il, v);
				il.Append(cil.MyInstruction.Stind_I);
				return;
			case LLVMValueKind.LLVMGlobalAliasValueKind:
				break;
			case LLVMValueKind.LLVMConstantIntValueKind:
			{
				cil.FirstClassType firstClassType6 = typ;
				switch (firstClassType6.Tag)
				{
				case 0:
				{
					cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)firstClassType6;
					switch (primitiveType.Item.Tag)
					{
					case 0:
						f_get_addr.Invoke(null);
						il.Append(cil.MyInstruction.NewLdc_I4((v.ConstIntSExt != 0L) ? 1 : 0));
						il.Append(cil.MyInstruction.Stind_I1);
						return;
					case 1:
						f_get_addr.Invoke(null);
						il.Append(cil.MyInstruction.NewLdc_I4((int)(v.ConstIntZExt & 0xFF)));
						il.Append(cil.MyInstruction.Stind_I1);
						return;
					case 2:
						f_get_addr.Invoke(null);
						il.Append(cil.MyInstruction.NewLdc_I4((int)(v.ConstIntZExt & 0xFFFF)));
						il.Append(cil.MyInstruction.Stind_I2);
						return;
					case 3:
						f_get_addr.Invoke(null);
						il.Append(cil.MyInstruction.NewLdc_I4((int)v.ConstIntSExt));
						il.Append(cil.MyInstruction.Stind_I4);
						return;
					case 4:
						f_get_addr.Invoke(null);
						il.Append(cil.MyInstruction.NewLdc_I8(v.ConstIntSExt));
						il.Append(cil.MyInstruction.Stind_I8);
						return;
					}
					break;
				}
				case 4:
				{
					cil.FunnyIntegerType item9 = ((cil.FirstClassType.FunnyIntegerType)firstClassType6).Item;
					store_funny_integer_constant_at(il, f_get_addr, v);
					return;
				}
				}
				FSharpFunc<LLVMValueRef, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, Unit, string, Unit, LLVMValueRef>("TODO store_value_ConstantInt : %A"));
				LLVMValueRef func = v;
				fSharpFunc.Invoke(func);
				return;
			}
			case LLVMValueKind.LLVMConstantFPValueKind:
			{
				double num10 = sgllvm.get_double(v);
				cil.FirstClassType firstClassType7 = typ;
				if (firstClassType7.Tag == 0)
				{
					cil.FirstClassType.PrimitiveType primitiveType2 = (cil.FirstClassType.PrimitiveType)firstClassType7;
					switch (primitiveType2.Item.Tag)
					{
					case 5:
						f_get_addr.Invoke(null);
						il.Append(cil.MyInstruction.NewLdc_R4((float)num10));
						il.Append(cil.MyInstruction.Stind_R4);
						return;
					case 6:
						f_get_addr.Invoke(null);
						il.Append(cil.MyInstruction.NewLdc_R8(num10));
						il.Append(cil.MyInstruction.Stind_R8);
						return;
					}
				}
				FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>> clo2 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>>, Unit, string, Unit, Tuple<string, LLVMValueRef>>("TODO store_value_ConstantFP at (%s) %A"));
				FSharpFunc<string, LLVMValueRef>.InvokeFast(new store_constant_at@698(clo2), v.GetType().FullName, v);
				return;
			}
			case LLVMValueKind.LLVMConstantStructValueKind:
			{
				cil.FirstClassType firstClassType3 = typ;
				if (firstClassType3.Tag == 2)
				{
					cil.FirstClassType.StructType structType = (cil.FirstClassType.StructType)firstClassType3;
					cil.StructType item5 = structType.Item;
					cil.StructType structType2 = item5;
					int num3 = 0;
					int num4 = v.OperandCount - 1;
					if (num4 >= num3)
					{
						do
						{
							FSharpFunc<Unit, Unit> f_get_addr3 = new sub_get_addr@708(il, f_get_addr, structType2, num3);
							LLVMValueRef operand2 = v.GetOperand((uint)num3);
							cil.FirstClassType typ2 = structType2.items[num3].typ;
							store_constant_at(syms, il, typ2, f_get_addr3, f_load_instr_value_to_dest, operand2);
							num3++;
						}
						while (num3 != num4 + 1);
					}
					return;
				}
				string message2 = "not a struct";
				throw Operators.Failure(message2);
			}
			case LLVMValueKind.LLVMConstantExprValueKind:
				f_get_addr.Invoke(null);
				outer_load_constant_value(syms, il, f_load_instr_value_to_dest, v);
				store_typ(il, typ);
				return;
			case LLVMValueKind.LLVMConstantPointerNullValueKind:
			{
				int item4 = cil.get_sizeof(typ);
				f_get_addr.Invoke(null);
				il.Append(cil.MyInstruction.NewLdc_I4(0));
				il.Append(cil.MyInstruction.NewLdc_I4(item4));
				il.Append(cil.MyInstruction.Initblk);
				return;
			}
			case LLVMValueKind.LLVMConstantAggregateZeroValueKind:
			{
				int num9 = cil.get_sizeof(typ);
				if (num9 != 0)
				{
					f_get_addr.Invoke(null);
					il.Append(cil.MyInstruction.NewLdc_I4(0));
					il.Append(cil.MyInstruction.NewLdc_I4(num9));
					il.Append(cil.MyInstruction.Initblk);
				}
				return;
			}
			case LLVMValueKind.LLVMConstantVectorValueKind:
			{
				cil.FirstClassType t = llvm_type_to_firstclass_type(v.TypeOf);
				cil.VectorElementType vectorElementType = to_vec_elem_type(llvm_type_to_firstclass_type(v.TypeOf.ElementType));
				uint vectorSize2 = v.TypeOf.VectorSize;
				cil.VectorElementType vectorElementType2 = vectorElementType;
				switch (vectorElementType2.Tag)
				{
				case 2:
				{
					cil.VectorElementType.VecPrim vecPrim = (cil.VectorElementType.VecPrim)vectorElementType2;
					cil.VecPrimitiveType item11 = vecPrim.Item;
					FSharpTypeFunc fSharpTypeFunc3 = new my_get_addr@744(f_get_addr);
					FSharpTypeFunc fSharpTypeFunc4 = new my_get_nth@745(syms, il, f_load_instr_value_to_dest, v);
					cil.PrimitiveType elem_typ2 = cil.vecprim_to_prim(item11);
					do_vec(il, elem_typ2, vectorSize2, (FSharpFunc<cil.CilWriter, Unit>)fSharpTypeFunc3.Specialize<cil.CilWriter>(), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc4.Specialize<cil.CilWriter>());
					break;
				}
				case 1:
				{
					FSharpTypeFunc fSharpTypeFunc5 = new my_get_addr@756-1(f_get_addr);
					FSharpTypeFunc fSharpTypeFunc6 = new my_get_nth@757-1(syms, il, f_load_instr_value_to_dest, v);
					do_ptrvec(il, vectorSize2, (FSharpFunc<cil.CilWriter, Unit>)fSharpTypeFunc5.Specialize<cil.CilWriter>(), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc6.Specialize<cil.CilWriter>());
					break;
				}
				case 3:
				{
					cil.VectorElementType.VecFunny vecFunny = (cil.VectorElementType.VecFunny)vectorElementType2;
					cil.FunnyIntegerType item10 = vecFunny.Item;
					int elem_siz = cil.sizeof_funnyinteger_rounded_up(item10);
					f_get_addr.Invoke(null);
					il.Append(cil.MyInstruction.NewLdc_I4(0));
					il.Append(cil.MyInstruction.NewLdc_I4(cil.get_sizeof(t)));
					il.Append(cil.MyInstruction.Initblk);
					FSharpFunc<int, Unit> dest_offset = new dest_offset@777(il, elem_siz);
					int num7 = 0;
					int num8 = (int)(v.TypeOf.VectorSize - 1);
					if (num8 >= num7)
					{
						do
						{
							LLVMValueRef operand3 = v.GetOperand((uint)num7);
							FSharpFunc<Unit, Unit> f_get_addr5 = new f_dest_addr@786(f_get_addr, dest_offset, num7);
							store_funny_integer_constant_at(il, f_get_addr5, operand3);
							num7++;
						}
						while (num7 != num8 + 1);
					}
					break;
				}
				default:
				{
					cil.VectorElementType vectorElementType3 = vectorElementType2;
					FSharpFunc<cil.VectorElementType, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.VectorElementType, Unit>, Unit, string, Unit, cil.VectorElementType>("%A"));
					cil.VectorElementType func2 = vectorElementType3;
					fSharpFunc2.Invoke(func2);
					break;
				}
				}
				return;
			}
			case LLVMValueKind.LLVMConstantDataVectorValueKind:
			{
				cil.PrimitiveType elem_typ = to_primitivetype(llvm_type_to_firstclass_type(v.TypeOf.ElementType));
				uint vectorSize = v.TypeOf.VectorSize;
				FSharpTypeFunc fSharpTypeFunc = new my_get_addr@800-2(f_get_addr);
				FSharpTypeFunc fSharpTypeFunc2 = new my_get_nth@801-2(syms, il, f_load_instr_value_to_dest, v);
				do_vec(il, elem_typ, vectorSize, (FSharpFunc<cil.CilWriter, Unit>)fSharpTypeFunc.Specialize<cil.CilWriter>(), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc2.Specialize<cil.CilWriter>());
				return;
			}
			case LLVMValueKind.LLVMConstantDataArrayValueKind:
			{
				cil.FirstClassType firstClassType4 = llvm_type_to_firstclass_type(v.TypeOf);
				cil.FirstClassType firstClassType5 = firstClassType4;
				if (firstClassType5.Tag == 3)
				{
					cil.FirstClassType.ArrayType arrayType2 = (cil.FirstClassType.ArrayType)firstClassType5;
					cil.ArrayType item6 = arrayType2.Item;
					Tuple<uint, cil.FirstClassType> tuple2 = new Tuple<uint, cil.FirstClassType>(item6.count, item6.elemtype);
					cil.FirstClassType item7 = tuple2.Item2;
					uint item8 = tuple2.Item1;
					int num5 = 0;
					int num6 = (int)(item8 - 1);
					if (num6 >= num5)
					{
						do
						{
							FSharpFunc<Unit, Unit> f_get_addr4 = new sub_get_addr@818-1(il, f_get_addr, item7, num5);
							LLVMValueRef elementAsConstant = v.GetElementAsConstant((uint)num5);
							store_constant_at(syms, il, item7, f_get_addr4, f_load_instr_value_to_dest, elementAsConstant);
							num5++;
						}
						while (num5 != num6 + 1);
					}
					return;
				}
				string message3 = "need array";
				throw Operators.Failure(message3);
			}
			case LLVMValueKind.LLVMConstantArrayValueKind:
			{
				cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(v.TypeOf);
				cil.FirstClassType firstClassType2 = firstClassType;
				if (firstClassType2.Tag == 3)
				{
					cil.FirstClassType.ArrayType arrayType = (cil.FirstClassType.ArrayType)firstClassType2;
					cil.ArrayType item = arrayType.Item;
					Tuple<uint, cil.FirstClassType> tuple = new Tuple<uint, cil.FirstClassType>(item.count, item.elemtype);
					cil.FirstClassType item2 = tuple.Item2;
					uint item3 = tuple.Item1;
					int num = 0;
					int num2 = (int)(item3 - 1);
					if (num2 >= num)
					{
						do
						{
							FSharpFunc<Unit, Unit> f_get_addr2 = new sub_get_addr@837-2(il, f_get_addr, item2, num);
							LLVMValueRef operand = v.GetOperand((uint)num);
							store_constant_at(syms, il, item2, f_get_addr2, f_load_instr_value_to_dest, operand);
							num++;
						}
						while (num != num2 + 1);
					}
					return;
				}
				string message = "need array";
				throw Operators.Failure(message);
			}
			default:
			{
				FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>>, Unit, string, Unit, Tuple<LLVMValueKind, LLVMValueRef>>("TODO store_constant_at (%A) %A"));
				FSharpFunc<LLVMValueKind, LLVMValueRef>.InvokeFast(new store_constant_at@847-2(clo), lLVMValueKind, v);
				return;
			}
			}
			LLVMValueRef operand4 = v.GetOperand(0u);
			GenSyms genSyms = syms;
			cil.CilWriter cilWriter = il;
			cil.FirstClassType firstClassType8 = typ;
			FSharpFunc<Unit, Unit> fSharpFunc3 = f_get_addr;
			FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> fSharpFunc4 = f_load_instr_value_to_dest;
			v = operand4;
			f_load_instr_value_to_dest = fSharpFunc4;
			f_get_addr = fSharpFunc3;
			typ = firstClassType8;
			il = cilWriter;
			syms = genSyms;
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	internal static void store_funny_integer_constant_at(cil.CilWriter il, FSharpFunc<Unit, Unit> f_get_addr, LLVMValueRef v)
	{
		cil.FunnyIntegerType item;
		Tuple<byte[], bool> tuple;
		byte[] array3;
		FSharpOption<int> fSharpOption2;
		int num;
		int item2;
		Tuple<int, int> tuple2;
		int item3;
		int item4;
		int item5;
		int num2;
		int num3;
		byte[] array4;
		int num4;
		int num5;
		Tuple<byte[], bool> tuple3;
		bool item6;
		byte[] item7;
		int item8;
		int item9;
		switch (sgllvm.get_value_kind(v))
		{
		case LLVMValueKind.LLVMConstantIntValueKind:
		{
			cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(v.TypeOf);
			cil.FirstClassType firstClassType2 = firstClassType;
			if (firstClassType2.Tag == 4)
			{
				cil.FirstClassType.FunnyIntegerType funnyIntegerType = (cil.FirstClassType.FunnyIntegerType)firstClassType2;
				item = funnyIntegerType.Item;
				FSharpFunc<string, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, string>, Unit, string, string, string>("%s"));
				string func = v.ToString();
				string text = fSharpFunc.Invoke(func);
				string[] array = text.Split(' ');
				if (array.Length != 2)
				{
					FSharpFunc<string, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, Unit>, Unit, string, Unit, string>("invalid funny integer: %s"));
					string func2 = text;
					fSharpFunc2.Invoke(func2);
				}
				FSharpFunc<int, string> fSharpFunc3 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<int, string>, Unit, string, string, int>("i%d"));
				int bits = item.bits;
				string b = fSharpFunc3.Invoke(bits);
				if (!string.Equals(array[0], b))
				{
					FSharpFunc<string, Unit> fSharpFunc4 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, Unit>, Unit, string, Unit, string>("invalid funny integer part 0: %s"));
					string func3 = text;
					fSharpFunc4.Invoke(func3);
				}
				string value = array[1];
				BigInteger bigInteger = BigInteger.Parse(value);
				byte[] array2 = bigInteger.ToByteArray();
				if (bigInteger.Sign < 0)
				{
					tuple = new Tuple<byte[], bool>(array2, item2: true);
				}
				else if (bigInteger.Sign == 0)
				{
					tuple = new Tuple<byte[], bool>(array2, item2: false);
				}
				else
				{
					if (array2[^1] == 0)
					{
						array3 = array2;
						FSharpOption<int> fSharpOption = FSharpOption<int>.Some(0);
						fSharpOption2 = FSharpOption<int>.Some(array2.Length - 2);
						num = array3.Length;
						if (fSharpOption != null)
						{
							FSharpOption<int> fSharpOption3 = fSharpOption;
							if (fSharpOption3.Value >= 0)
							{
								int value2 = fSharpOption3.Value;
								item2 = value2;
								goto IL_01b3;
							}
						}
						item2 = 0;
						goto IL_01b3;
					}
					tuple = new Tuple<byte[], bool>(array2, item2: false);
				}
				goto IL_0264;
			}
			throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 853, 18);
		}
		default:
			{
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 850, 14);
			}
			IL_01e1:
			tuple2 = new Tuple<int, int>(item2, item3);
			item4 = tuple2.Item1;
			item5 = tuple2.Item2;
			num2 = item5 - item4 + 1;
			num3 = ((num2 >= 0) ? num2 : 0);
			array4 = new byte[num3];
			num4 = 0;
			num5 = num3 - 1;
			if (num5 >= num4)
			{
				do
				{
					array4[num4] = array3[item4 + num4];
					num4++;
				}
				while (num4 != num5 + 1);
			}
			tuple = new Tuple<byte[], bool>(array4, item2: false);
			goto IL_0264;
			IL_0264:
			tuple3 = tuple;
			item6 = tuple3.Item2;
			item7 = tuple3.Item1;
			item8 = cil.sizeof_funnyinteger_rounded_up(item);
			item9 = (item6 ? 255 : 0);
			f_get_addr.Invoke(null);
			il.Append(cil.MyInstruction.NewLdc_I4(item9));
			il.Append(cil.MyInstruction.NewLdc_I4(item8));
			il.Append(cil.MyInstruction.Initblk);
			for (int i = 0; i < item7.Length; i++)
			{
				f_get_addr.Invoke(null);
				if (i > 0)
				{
					il.Append(cil.MyInstruction.NewLdc_I4(i));
					il.Append(cil.MyInstruction.Add);
				}
				il.Append(cil.MyInstruction.NewLdc_I4(item7[i]));
				il.Append(cil.MyInstruction.Stind_I1);
			}
			break;
			IL_01b3:
			if (fSharpOption2 != null)
			{
				FSharpOption<int> fSharpOption4 = fSharpOption2;
				if (fSharpOption4.Value < 0 + num)
				{
					int value3 = fSharpOption4.Value;
					item3 = value3;
					goto IL_01e1;
				}
			}
			item3 = 0 + num - 1;
			goto IL_01e1;
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1 })]
	internal static void outer_load_constant_value(GenSyms syms, cil.CilWriter il, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, LLVMValueRef v)
	{
		while (true)
		{
			LLVMValueKind lLVMValueKind = sgllvm.get_value_kind(v);
			switch (lLVMValueKind)
			{
			case LLVMValueKind.LLVMConstantArrayValueKind:
			case LLVMValueKind.LLVMConstantStructValueKind:
			case LLVMValueKind.LLVMConstantVectorValueKind:
			case LLVMValueKind.LLVMConstantAggregateZeroValueKind:
			case LLVMValueKind.LLVMConstantDataVectorValueKind:
			{
				cil.FirstClassType firstClassType4 = llvm_type_to_firstclass_type(v.TypeOf);
				cil.Variable tempVariable = il.GetTempVariable(firstClassType4);
				store_constant_at(syms, il, firstClassType4, new outer_load_constant_value@909(il, tempVariable), f_load_instr_value_to_dest, v);
				il.Append(cil.MyInstruction.NewLdloc(tempVariable));
				il.ReleaseTempVariable(tempVariable);
				return;
			}
			case LLVMValueKind.LLVMGlobalAliasValueKind:
				break;
			case LLVMValueKind.LLVMGlobalVariableValueKind:
				load_value_global(syms, il, v);
				return;
			case LLVMValueKind.LLVMConstantPointerNullValueKind:
				il.Append(cil.MyInstruction.NewLdc_I4(0));
				il.Append(cil.MyInstruction.Conv_I);
				return;
			case LLVMValueKind.LLVMConstantExprValueKind:
			{
				InstructionValue arg2 = InstructionValue.NewImmed(v);
				FSharpFunc<cil.CilWriter, InstructionValue>.InvokeFast(f_load_instr_value_to_dest, il, arg2, InstructionDest.OnStack);
				return;
			}
			case LLVMValueKind.LLVMConstantIntValueKind:
			{
				cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(v.TypeOf);
				cil.FirstClassType firstClassType2 = firstClassType;
				cil.FirstClassType firstClassType3;
				switch (firstClassType2.Tag)
				{
				case 0:
				{
					cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)firstClassType2;
					switch (primitiveType.Item.Tag)
					{
					case 0:
						il.Append(cil.MyInstruction.NewLdc_I4((v.ConstIntSExt != 0L) ? 1 : 0));
						return;
					case 1:
					case 2:
					case 3:
						il.Append(cil.MyInstruction.NewLdc_I4((int)v.ConstIntSExt));
						return;
					case 4:
						il.Append(cil.MyInstruction.NewLdc_I8(v.ConstIntSExt));
						return;
					}
					firstClassType3 = firstClassType2;
					break;
				}
				case 4:
				{
					cil.FunnyIntegerType item6 = ((cil.FirstClassType.FunnyIntegerType)firstClassType2).Item;
					cil.Variable variable = il.NewVariable(firstClassType);
					store_funny_integer_constant_at(il, new outer_load_constant_value@950-1(il, variable), v);
					il.Append(cil.MyInstruction.NewLdloc(variable));
					return;
				}
				default:
					firstClassType3 = firstClassType2;
					break;
				}
				FSharpFunc<cil.FirstClassType, Unit> fSharpFunc3 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, Unit>, Unit, string, Unit, cil.FirstClassType>("match case: %A"));
				cil.FirstClassType func3 = firstClassType3;
				fSharpFunc3.Invoke(func3);
				return;
			}
			case LLVMValueKind.LLVMConstantFPValueKind:
				switch (v.TypeOf.Kind)
				{
				case LLVMTypeKind.LLVMFloatTypeKind:
				{
					double num = sgllvm.get_double(v);
					il.Append(cil.MyInstruction.NewLdc_R4((float)num));
					break;
				}
				case LLVMTypeKind.LLVMDoubleTypeKind:
				{
					double item5 = sgllvm.get_double(v);
					il.Append(cil.MyInstruction.NewLdc_R8(item5));
					break;
				}
				case LLVMTypeKind.LLVMX86_FP80TypeKind:
				{
					double item4 = sgllvm.get_double(v);
					il.Append(cil.MyInstruction.NewLdc_R8(item4));
					break;
				}
				case LLVMTypeKind.LLVMFP128TypeKind:
				{
					double item3 = sgllvm.get_double(v);
					il.Append(cil.MyInstruction.NewLdc_R8(item3));
					break;
				}
				default:
				{
					FSharpFunc<LLVMValueRef, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, Unit, string, Unit, LLVMValueRef>("TODO ConstantFP %A"));
					LLVMValueRef func2 = v;
					fSharpFunc2.Invoke(func2);
					break;
				}
				}
				return;
			case LLVMValueKind.LLVMFunctionValueKind:
				load_value_function(syms, il, v);
				return;
			case LLVMValueKind.LLVMUndefValueValueKind:
				switch (v.TypeOf.Kind)
				{
				case LLVMTypeKind.LLVMStructTypeKind:
				case LLVMTypeKind.LLVMArrayTypeKind:
				case LLVMTypeKind.LLVMVectorTypeKind:
				{
					cil.FirstClassType tr2 = llvm_type_to_firstclass_type(v.TypeOf);
					cil.Variable item2 = il.NewVariable(tr2);
					il.Append(cil.MyInstruction.NewLdloc(item2));
					break;
				}
				case LLVMTypeKind.LLVMPointerTypeKind:
					il.Append(cil.MyInstruction.NewLdc_I4(0));
					il.Append(cil.MyInstruction.Conv_I);
					break;
				case LLVMTypeKind.LLVMFloatTypeKind:
					il.Append(cil.MyInstruction.NewLdc_R4(0f));
					break;
				case LLVMTypeKind.LLVMDoubleTypeKind:
					il.Append(cil.MyInstruction.NewLdc_R8(0.0));
					break;
				case LLVMTypeKind.LLVMX86_FP80TypeKind:
				{
					string message = "FP80";
					throw Operators.Failure(message);
				}
				case LLVMTypeKind.LLVMIntegerTypeKind:
					switch (v.TypeOf.IntWidth)
					{
					case 8u:
					case 16u:
					case 32u:
						il.Append(cil.MyInstruction.NewLdc_I4(0));
						break;
					case 64u:
						il.Append(cil.MyInstruction.NewLdc_I8(0L));
						break;
					default:
					{
						cil.FirstClassType tr = llvm_type_to_firstclass_type(v.TypeOf);
						cil.Variable item = il.NewVariable(tr);
						il.Append(cil.MyInstruction.NewLdloc(item));
						break;
					}
					}
					break;
				default:
				{
					FSharpFunc<LLVMValueRef, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, Unit, string, Unit, LLVMValueRef>("undef: %A"));
					LLVMValueRef func = v;
					fSharpFunc.Invoke(func);
					break;
				}
				}
				return;
			default:
			{
				LLVMValueKind arg = lLVMValueKind;
				FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>>, Unit, string, Unit, Tuple<LLVMValueKind, LLVMValueRef>>("TODO: %A -- %A"));
				FSharpFunc<LLVMValueKind, LLVMValueRef>.InvokeFast(new outer_load_constant_value@1019-2(clo), arg, v);
				return;
			}
			}
			GenSyms genSyms = syms;
			cil.CilWriter cilWriter = il;
			FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> fSharpFunc4 = f_load_instr_value_to_dest;
			v = v.GetOperand(0u);
			f_load_instr_value_to_dest = fSharpFunc4;
			il = cilWriter;
			syms = genSyms;
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1, 1, 1 })]
	internal static void outer_load_value<a>(a typs, GenSyms syms, cil.CilWriter il, Dictionary<LLVMValueRef, ParameterDefinition> args, FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, LLVMValueRef v)
	{
		LLVMValueKind lLVMValueKind = sgllvm.get_value_kind(v);
		switch (lLVMValueKind)
		{
		case LLVMValueKind.LLVMFunctionValueKind:
		case LLVMValueKind.LLVMGlobalAliasValueKind:
		case LLVMValueKind.LLVMGlobalVariableValueKind:
		case LLVMValueKind.LLVMConstantExprValueKind:
		case LLVMValueKind.LLVMConstantArrayValueKind:
		case LLVMValueKind.LLVMConstantStructValueKind:
		case LLVMValueKind.LLVMConstantVectorValueKind:
		case LLVMValueKind.LLVMUndefValueValueKind:
		case LLVMValueKind.LLVMConstantAggregateZeroValueKind:
		case LLVMValueKind.LLVMConstantDataVectorValueKind:
		case LLVMValueKind.LLVMConstantIntValueKind:
		case LLVMValueKind.LLVMConstantFPValueKind:
		case LLVMValueKind.LLVMConstantPointerNullValueKind:
			outer_load_constant_value(syms, il, f_load_instr_value_to_dest, v);
			break;
		case LLVMValueKind.LLVMArgumentValueKind:
			outer_load_arg(il, args, v);
			break;
		case LLVMValueKind.LLVMInstructionValueKind:
		{
			InstructionValue arg2 = f_get_instr_value.Invoke(v);
			FSharpFunc<cil.CilWriter, InstructionValue>.InvokeFast(f_load_instr_value_to_dest, il, arg2, InstructionDest.OnStack);
			break;
		}
		default:
		{
			LLVMValueKind arg = lLVMValueKind;
			FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueKind, FSharpFunc<LLVMValueRef, Unit>>, Unit, string, Unit, Tuple<LLVMValueKind, LLVMValueRef>>("TODO: %A -- %A"));
			FSharpFunc<LLVMValueKind, LLVMValueRef>.InvokeFast(new outer_load_value@1055(clo), arg, v);
			break;
		}
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	internal static void emit_address_of_value(cil.CilWriter il, AddressOfValue addr)
	{
		cil.Variable item2;
		if (!(addr is AddressOfValue.AlreadyVar))
		{
			if (!(addr is AddressOfValue.TempVar))
			{
				AddressOfValue.Arg arg = (AddressOfValue.Arg)addr;
				ParameterDefinition item = arg.item;
				il.Append(cil.MyInstruction.NewLdarga(item));
				return;
			}
			item2 = ((AddressOfValue.TempVar)addr).item;
		}
		else
		{
			item2 = ((AddressOfValue.AlreadyVar)addr).item;
		}
		il.Append(cil.MyInstruction.NewLdloca(item2));
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	internal static void release_address_of_value(cil.CilWriter il, AddressOfValue addr)
	{
		if (!(addr is AddressOfValue.AlreadyVar))
		{
			if (!(addr is AddressOfValue.TempVar))
			{
				AddressOfValue.Arg arg = (AddressOfValue.Arg)addr;
				ParameterDefinition parameterDefinition = arg.item;
			}
			else
			{
				AddressOfValue.TempVar tempVar = (AddressOfValue.TempVar)addr;
				cil.Variable v = tempVar.item;
				il.ReleaseTempVariable(v);
			}
		}
		else
		{
			AddressOfValue.AlreadyVar alreadyVar = (AddressOfValue.AlreadyVar)addr;
			cil.Variable variable = alreadyVar.item;
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1, 1 })]
	internal static AddressOfValue prep_address_of_value(GenSyms syms, cil.CilWriter il, Dictionary<LLVMValueRef, ParameterDefinition> args, FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, LLVMValueRef v)
	{
		cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(v.TypeOf);
		switch (sgllvm.get_value_kind(v))
		{
		case LLVMValueKind.LLVMArgumentValueKind:
		{
			ParameterDefinition parameterDefinition = args[v];
			ParameterDefinition item2 = parameterDefinition;
			return AddressOfValue.NewArg(item2);
		}
		case LLVMValueKind.LLVMConstantVectorValueKind:
		case LLVMValueKind.LLVMConstantDataVectorValueKind:
		case LLVMValueKind.LLVMConstantIntValueKind:
		{
			cil.Variable tempVariable4 = il.GetTempVariable(firstClassType);
			store_constant_at(syms, il, firstClassType, new prep_address_of_value@1091(il, tempVariable4), f_load_instr_value_to_dest, v);
			return AddressOfValue.NewTempVar(tempVariable4);
		}
		case LLVMValueKind.LLVMUndefValueValueKind:
		{
			cil.Variable tempVariable5 = il.GetTempVariable(firstClassType);
			cil.Variable item3 = tempVariable5;
			return AddressOfValue.NewTempVar(item3);
		}
		case LLVMValueKind.LLVMConstantAggregateZeroValueKind:
		{
			cil.Variable tempVariable3 = il.GetTempVariable(firstClassType);
			init_local(il, tempVariable3);
			return AddressOfValue.NewTempVar(tempVariable3);
		}
		case LLVMValueKind.LLVMInstructionValueKind:
		{
			InstructionValue instructionValue = f_get_instr_value.Invoke(v);
			InstructionValue instructionValue2 = instructionValue;
			InstructionValue instructionValue3 = instructionValue2;
			if (!(instructionValue3 is InstructionValue.Immed))
			{
				if (!(instructionValue3 is InstructionValue.Local))
				{
					InstructionValue.Temp temp = (InstructionValue.Temp)instructionValue2;
					cil.Variable item = temp.item;
					return AddressOfValue.NewAlreadyVar(item);
				}
				InstructionValue.Local local = (InstructionValue.Local)instructionValue2;
				cil.Variable variable = local.item;
				string message = "TODO do we need to deal with this case??";
				throw Operators.Failure(message);
			}
			InstructionValue.Immed immed = (InstructionValue.Immed)instructionValue2;
			cil.Variable tempVariable2 = il.GetTempVariable(firstClassType);
			InstructionDest arg3 = InstructionDest.NewInTemp(tempVariable2);
			FSharpFunc<cil.CilWriter, InstructionValue>.InvokeFast(f_load_instr_value_to_dest, il, instructionValue, arg3);
			return AddressOfValue.NewTempVar(tempVariable2);
		}
		case LLVMValueKind.LLVMConstantExprValueKind:
		{
			InstructionValue arg = InstructionValue.NewImmed(v);
			cil.Variable tempVariable = il.GetTempVariable(firstClassType);
			InstructionDest arg2 = InstructionDest.NewInTemp(tempVariable);
			FSharpFunc<cil.CilWriter, InstructionValue>.InvokeFast(f_load_instr_value_to_dest, il, arg, arg2);
			return AddressOfValue.NewTempVar(tempVariable);
		}
		default:
		{
			FSharpFunc<string, FSharpFunc<LLVMValueRef, AddressOfValue>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, FSharpFunc<LLVMValueRef, AddressOfValue>>, Unit, string, AddressOfValue, Tuple<string, LLVMValueRef>>("TODO prep_address_of_value: %s %A"));
			return FSharpFunc<string, LLVMValueRef>.InvokeFast(new prep_address_of_value@1118-1(clo), v.GetType().FullName, v);
		}
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1, 1, 1 })]
	internal static void load_aggorvec_into_specific_local(GenSyms syms, cil.CilWriter il, cil.Variable var_into, Dictionary<LLVMValueRef, ParameterDefinition> args, FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, LLVMValueRef v)
	{
		switch (sgllvm.get_value_kind(v))
		{
		case LLVMValueKind.LLVMUndefValueValueKind:
			break;
		case LLVMValueKind.LLVMConstantAggregateZeroValueKind:
			init_local(il, var_into);
			break;
		case LLVMValueKind.LLVMConstantStructValueKind:
		case LLVMValueKind.LLVMConstantVectorValueKind:
		case LLVMValueKind.LLVMConstantDataVectorValueKind:
		{
			cil.FirstClassType typ = llvm_type_to_firstclass_type(v.TypeOf);
			store_constant_at(syms, il, typ, new load_aggorvec_into_specific_local@1134(il, var_into), f_load_instr_value_to_dest, v);
			break;
		}
		case LLVMValueKind.LLVMInstructionValueKind:
		{
			InstructionValue arg = f_get_instr_value.Invoke(v);
			FSharpFunc<cil.CilWriter, InstructionValue>.InvokeFast(f_load_instr_value_to_dest, il, arg, InstructionDest.OnStack);
			il.Append(cil.MyInstruction.NewStloc(var_into));
			break;
		}
		case LLVMValueKind.LLVMArgumentValueKind:
			outer_load_arg(il, args, v);
			il.Append(cil.MyInstruction.NewStloc(var_into));
			break;
		default:
		{
			FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, FSharpFunc<LLVMValueRef, Unit>>, Unit, string, Unit, Tuple<string, LLVMValueRef>>("TODO load_aggorvec_into_specific_local: %s %A"));
			FSharpFunc<string, LLVMValueRef>.InvokeFast(new load_aggorvec_into_specific_local@1149-1(clo), v.GetType().FullName, v);
			break;
		}
		}
	}

	[CompilationArgumentCounts(new int[]
	{
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1
	})]
	public unsafe static void gen_instr(cil.GenTypes typs, GenSyms syms, cil.CilWriter il, cil.Label lab_end, LLVMValueRef op, Dictionary<LLVMValueRef, ParameterDefinition> args, ParameterDefinition extra_args, FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value, FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest, InstructionDest result_dest, Dictionary<LLVMBasicBlockRef, Block> labels)
	{
		MethodReference methodReference = typs.md.ImportReference(typeof(Exception).GetConstructor(new Type[1] { typeof(string) }));
		FSharpFunc<string, Unit> fSharpFunc = new gen_dump_str@1156(syms, il);
		FSharpFunc<string, Unit> fSharpFunc2 = new comment@1166(il);
		FSharpTypeFunc fSharpTypeFunc = new todo@1168(il, methodReference);
		FSharpFunc<LLVMValueRef, Unit> fSharpFunc3 = new load_value@1176(typs, syms, il, args, f_get_instr_value, f_load_instr_value_to_dest);
		FSharpFunc<LLVMValueRef, AddressOfValue> fSharpFunc4 = new my_prep_address_of_value@1179(syms, il, args, f_get_instr_value, f_load_instr_value_to_dest);
		FSharpFunc<Unit, ResultLocal> fSharpFunc5 = new get_result_local@1183(il, op, result_dest);
		FSharpFunc<ResultLocal, Unit> fSharpFunc6 = new ldloca_result_local@1191(il);
		FSharpFunc<ResultLocal, Unit> fSharpFunc7 = new init_result_local@1198(il);
		FSharpFunc<ResultLocal, cil.Variable> fSharpFunc8 = new grab_result_local@1206();
		FSharpFunc<ResultLocal, Unit> fSharpFunc9 = new finish_result_local@1213(il);
		FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef[], FSharpFunc<int, cil.FirstClassType>>> fSharpFunc10 = new walk_indexes@1221(il, fSharpFunc3);
		FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> func = new store@1284(il, fSharpFunc3, fSharpFunc4);
		FSharpFunc<cil.VecPrimitiveType, Unit> load_vecprim_to_stack = new load_vecprim_to_stack@1313(il);
		FSharpFunc<cil.PrimitiveType, Unit> fSharpFunc11 = new load_mem_primitive_to_stack@1329(il);
		FSharpFunc<Unit, Unit> fSharpFunc12 = new save_op_result_from_stack@1342(il, result_dest);
		FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> func2 = new load_mem_and_save_op_result@1355(il, fSharpFunc5, fSharpFunc8, fSharpFunc9, fSharpFunc11, fSharpFunc12);
		FSharpFunc<LLVMValueRef, FSharpFunc<LLVMValueRef, cil.Label>> func3 = new find_label@1383(labels);
		FSharpFunc<LLVMValueRef, LLVMOpcode> fSharpFunc13 = new get_opcode@1395();
		FSharpFunc<LLVMValueRef, cil.MyInstruction> fSharpFunc14 = new simple_binop_primitive_opcode@1402(fSharpFunc13);
		FSharpFunc<LLVMValueRef, MethodReference> get_funny_method_ref = new get_funny_method_ref@1424(syms, fSharpFunc13);
		FSharpFunc<cil.FunnyIntegerType, FSharpFunc<LLVMValueRef, Unit>> func4 = new simple_binop_funny@1445(syms, il, fSharpTypeFunc, fSharpFunc4, fSharpFunc5, fSharpFunc6, fSharpFunc8, fSharpFunc9, fSharpFunc13, fSharpFunc14);
		FSharpFunc<cil.FunnyIntegerType, FSharpFunc<LLVMValueRef, Unit>> func5 = new funny_shift@1511(syms, il, fSharpTypeFunc, fSharpFunc4, fSharpFunc5, fSharpFunc6, fSharpFunc8, fSharpFunc9, fSharpFunc13);
		FSharpFunc<LLVMValueRef, Unit> fSharpFunc15 = new simple_binop_vector@1578(syms, il, fSharpFunc3, fSharpFunc4, fSharpFunc5, fSharpFunc6, fSharpFunc8, fSharpFunc9, load_vecprim_to_stack, fSharpFunc11, fSharpFunc12, fSharpFunc13, fSharpFunc14, get_funny_method_ref);
		FSharpFunc<LLVMValueRef, Unit> fSharpFunc16 = new simple_binop_prim@1727(il, fSharpFunc3, fSharpFunc12, fSharpFunc14);
		FSharpFunc<LLVMTypeRef, Unit> fSharpFunc17 = new throw_if_not_prim@1790();
		cil.FirstClassType firstClassType45;
		cil.FirstClassType firstClassType30;
		FSharpFunc<cil.FirstClassType, Unit> fSharpFunc29;
		cil.FirstClassType func30;
		ResultLocal func51;
		AddressOfValue addressOfValue20;
		FSharpFunc<Unit, Unit> f_load_src8;
		int siz8;
		switch (fSharpFunc13.Invoke(op))
		{
		case LLVMOpcode.LLVMPHI:
			break;
		case LLVMOpcode.LLVMCall:
		{
			LLVMValueRef operand19 = op.GetOperand((uint)(op.OperandCount - 1));
			if (string.Equals(operand19.Name, "__sg_checkpoint"))
			{
				string item88 = one_line(op.ToString());
				il.Append(cil.MyInstruction.NewLdstr(item88));
				il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("__sg_checkpoint")));
			}
			else
			{
				if (operand19.Name.StartsWith("llvm.lifetime."))
				{
					break;
				}
				if (operand19.Name.StartsWith("llvm.expect."))
				{
					fSharpFunc3.Invoke(op.GetOperand(0u));
					fSharpFunc12.Invoke(null);
				}
				else
				{
					if (string.Equals(operand19.Name, "llvm.aarch64.hint") || string.Equals(operand19.Name, "llvm.dbg.declare") || string.Equals(operand19.Name, "llvm.assume") || string.Equals(operand19.Name, "llvm.sideeffect"))
					{
						break;
					}
					if (string.Equals(operand19.Name, "llvm.va_start"))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(extra_args, null))
						{
							ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<Unit, Unit, string, Unit, Unit>("va_start but no extra args"));
						}
						if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("riscv64"))
						{
							fSharpFunc3.Invoke(op.GetOperand(0u));
							il.Append(cil.MyInstruction.NewLdarg(extra_args));
							il.Append(cil.MyInstruction.NewLdc_I4(8));
							il.Append(cil.MyInstruction.Add);
							il.Append(cil.MyInstruction.Stind_I);
						}
						else if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("aarch64"))
						{
							cil.Variable item89 = il.NewVariable(cil.FirstClassType.Ptr);
							fSharpFunc3.Invoke(op.GetOperand(0u));
							il.Append(cil.MyInstruction.NewStloc(item89));
							il.Append(cil.MyInstruction.NewLdloc(item89));
							il.Append(cil.MyInstruction.NewLdc_I4(0));
							il.Append(cil.MyInstruction.NewLdc_I4(28 + 4));
							il.Append(cil.MyInstruction.Initblk);
							il.Append(cil.MyInstruction.NewLdloc(item89));
							il.Append(cil.MyInstruction.NewLdarg(extra_args));
							il.Append(cil.MyInstruction.NewLdc_I4(8));
							il.Append(cil.MyInstruction.Add);
							il.Append(cil.MyInstruction.Stind_I);
						}
						else if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("arm64"))
						{
							fSharpFunc3.Invoke(op.GetOperand(0u));
							il.Append(cil.MyInstruction.NewLdarg(extra_args));
							il.Append(cil.MyInstruction.NewLdc_I4(8));
							il.Append(cil.MyInstruction.Add);
							il.Append(cil.MyInstruction.Stind_I);
						}
						else if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("x86_64-pc-windows"))
						{
							fSharpFunc3.Invoke(op.GetOperand(0u));
							il.Append(cil.MyInstruction.NewLdarg(extra_args));
							il.Append(cil.MyInstruction.NewLdc_I4(8));
							il.Append(cil.MyInstruction.Add);
							il.Append(cil.MyInstruction.Stind_I);
						}
						else if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("x86_64"))
						{
							cil.Variable item90 = il.NewVariable(cil.FirstClassType.Ptr);
							fSharpFunc3.Invoke(op.GetOperand(0u));
							il.Append(cil.MyInstruction.NewStloc(item90));
							il.Append(cil.MyInstruction.NewLdloc(item90));
							il.Append(cil.MyInstruction.NewLdc_I4(48));
							il.Append(cil.MyInstruction.Stind_I4);
							il.Append(cil.MyInstruction.NewLdloc(item90));
							il.Append(cil.MyInstruction.NewLdc_I4(4));
							il.Append(cil.MyInstruction.Add);
							il.Append(cil.MyInstruction.NewLdc_I4(304));
							il.Append(cil.MyInstruction.Stind_I4);
							il.Append(cil.MyInstruction.NewLdloc(item90));
							il.Append(cil.MyInstruction.NewLdc_I4(8));
							il.Append(cil.MyInstruction.Add);
							il.Append(cil.MyInstruction.NewLdarg(extra_args));
							il.Append(cil.MyInstruction.NewLdc_I4(8));
							il.Append(cil.MyInstruction.Add);
							il.Append(cil.MyInstruction.Stind_I8);
							il.Append(cil.MyInstruction.NewLdloc(item90));
							il.Append(cil.MyInstruction.NewLdc_I4(16));
							il.Append(cil.MyInstruction.Add);
							il.Append(cil.MyInstruction.NewLdc_I8(0L));
							il.Append(cil.MyInstruction.Stind_I8);
						}
						else
						{
							FSharpFunc<string, Unit> fSharpFunc35 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, Unit>, Unit, string, Unit, string>("need va_start definition for %s"));
							string target = <StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target;
							fSharpFunc35.Invoke(target);
						}
						break;
					}
					if (string.Equals(operand19.Name, "llvm.va_copy"))
					{
						if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("x86_64-pc-windows"))
						{
							fSharpFunc3.Invoke(op.GetOperand(0u));
							fSharpFunc3.Invoke(op.GetOperand(1u));
							il.Append(cil.MyInstruction.NewLdc_I4(8));
							il.Append(cil.MyInstruction.Cpblk);
						}
						else if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("x86_64"))
						{
							fSharpFunc3.Invoke(op.GetOperand(0u));
							fSharpFunc3.Invoke(op.GetOperand(1u));
							il.Append(cil.MyInstruction.NewLdc_I4(24));
							il.Append(cil.MyInstruction.Cpblk);
						}
						else if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("aarch64"))
						{
							fSharpFunc3.Invoke(op.GetOperand(0u));
							fSharpFunc3.Invoke(op.GetOperand(1u));
							il.Append(cil.MyInstruction.NewLdc_I4(32));
							il.Append(cil.MyInstruction.Cpblk);
						}
						else if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("arm64"))
						{
							string func41 = "va_copy arm64 msvc";
							((FSharpFunc<string, Unit>)fSharpTypeFunc.Specialize<string>()).Invoke(func41);
						}
						else
						{
							FSharpFunc<string, Unit> fSharpFunc36 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, Unit>, Unit, string, Unit, string>("need va_copy definition for %s"));
							string target2 = <StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target;
							fSharpFunc36.Invoke(target2);
						}
						break;
					}
					if (string.Equals(operand19.Name, "llvm.va_end"))
					{
						if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("riscv64"))
						{
							fSharpFunc3.Invoke(op.GetOperand(0u));
							il.Append(cil.MyInstruction.Ldnull);
							il.Append(cil.MyInstruction.Stind_I);
						}
						else if (!<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("x86_64-pc-windows"))
						{
							if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("x86_64"))
							{
								fSharpFunc3.Invoke(op.GetOperand(0u));
								il.Append(cil.MyInstruction.NewLdc_I4(0));
								il.Append(cil.MyInstruction.NewLdc_I4(24));
								il.Append(cil.MyInstruction.Initblk);
							}
							else if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("aarch64"))
							{
								fSharpFunc3.Invoke(op.GetOperand(0u));
								il.Append(cil.MyInstruction.Ldnull);
								il.Append(cil.MyInstruction.Stind_I);
							}
							else if (!<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("arm64"))
							{
								FSharpFunc<string, Unit> fSharpFunc37 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, Unit>, Unit, string, Unit, string>("need va_end definition for %s"));
								string target3 = <StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target;
								fSharpFunc37.Invoke(target3);
							}
						}
						break;
					}
					if (string.Equals(operand19.Name, "llvm.trap"))
					{
						string item91 = "llvm.trap";
						il.Append(cil.MyInstruction.NewLdstr(item91));
						il.Append(cil.MyInstruction.NewNewobj(methodReference));
						il.Append(cil.MyInstruction.Throw);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.ctlz.i64"))
					{
						LLVMValueRef operand20 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand20.TypeOf);
						LLVMValueRef operand21 = op.GetOperand(1u);
						fSharpFunc3.Invoke(operand20);
						MethodReference item92 = typs.md.ImportReference(typeof(BitOperations).GetMethod("LeadingZeroCount", new Type[1] { typeof(ulong) }));
						il.Append(cil.MyInstruction.NewCall(item92));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.ctlz.i32"))
					{
						LLVMValueRef operand22 = op.GetOperand(0u);
						LLVMValueRef operand23 = op.GetOperand(1u);
						fSharpFunc17.Invoke(operand22.TypeOf);
						fSharpFunc3.Invoke(operand22);
						MethodReference item93 = typs.md.ImportReference(typeof(BitOperations).GetMethod("LeadingZeroCount", new Type[1] { typeof(uint) }));
						il.Append(cil.MyInstruction.NewCall(item93));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.ctlz.i16"))
					{
						LLVMValueRef operand24 = op.GetOperand(0u);
						LLVMValueRef operand25 = op.GetOperand(1u);
						fSharpFunc17.Invoke(operand24.TypeOf);
						fSharpFunc3.Invoke(operand24);
						il.Append(cil.MyInstruction.NewLdc_I4(65535));
						il.Append(cil.MyInstruction.And);
						MethodReference item94 = typs.md.ImportReference(typeof(BitOperations).GetMethod("LeadingZeroCount", new Type[1] { typeof(uint) }));
						il.Append(cil.MyInstruction.NewCall(item94));
						il.Append(cil.MyInstruction.NewLdc_I4(16));
						il.Append(cil.MyInstruction.Sub);
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.cttz.i64"))
					{
						LLVMValueRef operand26 = op.GetOperand(0u);
						LLVMValueRef operand27 = op.GetOperand(1u);
						fSharpFunc17.Invoke(operand26.TypeOf);
						fSharpFunc3.Invoke(operand26);
						MethodReference item95 = typs.md.ImportReference(typeof(BitOperations).GetMethod("TrailingZeroCount", new Type[1] { typeof(long) }));
						il.Append(cil.MyInstruction.NewCall(item95));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.cttz.i32"))
					{
						LLVMValueRef operand28 = op.GetOperand(0u);
						LLVMValueRef operand29 = op.GetOperand(1u);
						fSharpFunc17.Invoke(operand28.TypeOf);
						fSharpFunc3.Invoke(operand28);
						MethodReference item96 = typs.md.ImportReference(typeof(BitOperations).GetMethod("TrailingZeroCount", new Type[1] { typeof(int) }));
						il.Append(cil.MyInstruction.NewCall(item96));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.cttz.i16"))
					{
						LLVMValueRef operand30 = op.GetOperand(0u);
						LLVMValueRef operand31 = op.GetOperand(1u);
						fSharpFunc17.Invoke(operand30.TypeOf);
						fSharpFunc3.Invoke(operand30);
						MethodReference item97 = typs.md.ImportReference(typeof(BitOperations).GetMethod("TrailingZeroCount", new Type[1] { typeof(int) }));
						il.Append(cil.MyInstruction.NewCall(item97));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.ctpop.i64"))
					{
						LLVMValueRef operand32 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand32.TypeOf);
						LLVMValueRef operand33 = op.GetOperand(1u);
						fSharpFunc3.Invoke(operand32);
						MethodReference item98 = typs.md.ImportReference(typeof(BitOperations).GetMethod("PopCount", new Type[1] { typeof(ulong) }));
						il.Append(cil.MyInstruction.NewCall(item98));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.ctpop.i32") || string.Equals(operand19.Name, "llvm.ctpop.i16") || string.Equals(operand19.Name, "llvm.ctpop.i8"))
					{
						LLVMValueRef operand34 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand34.TypeOf);
						LLVMValueRef operand35 = op.GetOperand(1u);
						fSharpFunc3.Invoke(operand34);
						MethodReference item99 = typs.md.ImportReference(typeof(BitOperations).GetMethod("PopCount", new Type[1] { typeof(uint) }));
						il.Append(cil.MyInstruction.NewCall(item99));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.floor.f64"))
					{
						LLVMValueRef operand36 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand36.TypeOf);
						fSharpFunc3.Invoke(operand36);
						MethodReference item100 = typs.md.ImportReference(typeof(Math).GetMethod("Floor", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item100));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.ceil.f64"))
					{
						LLVMValueRef operand37 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand37.TypeOf);
						fSharpFunc3.Invoke(operand37);
						MethodReference item101 = typs.md.ImportReference(typeof(Math).GetMethod("Ceiling", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item101));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.floor.f32"))
					{
						LLVMValueRef operand38 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand38.TypeOf);
						fSharpFunc3.Invoke(operand38);
						il.Append(cil.MyInstruction.Conv_R8);
						MethodReference item102 = typs.md.ImportReference(typeof(Math).GetMethod("Floor", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item102));
						il.Append(cil.MyInstruction.Conv_R4);
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.ceil.f32"))
					{
						LLVMValueRef operand39 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand39.TypeOf);
						fSharpFunc3.Invoke(operand39);
						il.Append(cil.MyInstruction.Conv_R8);
						MethodReference item103 = typs.md.ImportReference(typeof(Math).GetMethod("Ceiling", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item103));
						il.Append(cil.MyInstruction.Conv_R4);
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.trunc.v2f32"))
					{
						LLVMValueRef operand40 = op.GetOperand(0u);
						fSharpFunc3.Invoke(operand40);
						int num13 = 2;
						string text10 = "f32";
						string text11 = "trunc";
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<int, string, string>>("v%P()%P()_%P()", new object[3] { num13, text10, text11 }, null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.fabs.v2f32"))
					{
						LLVMValueRef operand41 = op.GetOperand(0u);
						fSharpFunc3.Invoke(operand41);
						int num14 = 2;
						string text12 = "f32";
						string text13 = "fabs";
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<int, string, string>>("v%P()%P()_%P()", new object[3] { num14, text12, text13 }, null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.sqrt.v2f32"))
					{
						LLVMValueRef operand42 = op.GetOperand(0u);
						fSharpFunc3.Invoke(operand42);
						int num15 = 2;
						string text14 = "f32";
						string text15 = "sqrt";
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<int, string, string>>("v%P()%P()_%P()", new object[3] { num15, text14, text15 }, null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.trunc.f64"))
					{
						LLVMValueRef operand43 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand43.TypeOf);
						fSharpFunc3.Invoke(operand43);
						MethodReference item104 = typs.md.ImportReference(typeof(Math).GetMethod("Truncate", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item104));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.trunc.f32"))
					{
						LLVMValueRef operand44 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand44.TypeOf);
						fSharpFunc3.Invoke(operand44);
						MethodReference item105 = typs.md.ImportReference(typeof(Math).GetMethod("Truncate", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item105));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.log2.f64"))
					{
						LLVMValueRef operand45 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand45.TypeOf);
						fSharpFunc3.Invoke(operand45);
						MethodReference item106 = typs.md.ImportReference(typeof(Math).GetMethod("Log2", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item106));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.copysign.f64"))
					{
						fSharpFunc17.Invoke(op.GetOperand(0u).TypeOf);
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						MethodReference item107 = typs.md.ImportReference(typeof(Math).GetMethod("CopySign", new Type[2]
						{
							typeof(double),
							typeof(double)
						}));
						il.Append(cil.MyInstruction.NewCall(item107));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.powi.f64"))
					{
						fSharpFunc17.Invoke(op.GetOperand(0u).TypeOf);
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.Conv_R8);
						MethodReference item108 = typs.md.ImportReference(typeof(Math).GetMethod("Pow", new Type[2]
						{
							typeof(double),
							typeof(double)
						}));
						il.Append(cil.MyInstruction.NewCall(item108));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.log.f64"))
					{
						LLVMValueRef operand46 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand46.TypeOf);
						fSharpFunc3.Invoke(operand46);
						MethodReference item109 = typs.md.ImportReference(typeof(Math).GetMethod("Log", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item109));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.pow.f64"))
					{
						fSharpFunc17.Invoke(op.GetOperand(0u).TypeOf);
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						MethodReference item110 = typs.md.ImportReference(typeof(Math).GetMethod("Pow", new Type[2]
						{
							typeof(double),
							typeof(double)
						}));
						il.Append(cil.MyInstruction.NewCall(item110));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.pow.f32"))
					{
						fSharpFunc17.Invoke(op.GetOperand(0u).TypeOf);
						fSharpFunc3.Invoke(op.GetOperand(0u));
						il.Append(cil.MyInstruction.Conv_R8);
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.Conv_R8);
						MethodReference item111 = typs.md.ImportReference(typeof(Math).GetMethod("Pow", new Type[2]
						{
							typeof(double),
							typeof(double)
						}));
						il.Append(cil.MyInstruction.NewCall(item111));
						il.Append(cil.MyInstruction.Conv_R4);
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.round.f64"))
					{
						LLVMValueRef operand47 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand47.TypeOf);
						fSharpFunc3.Invoke(operand47);
						MethodReference item112 = typs.md.ImportReference(typeof(Math).GetMethod("Round", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item112));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.round.f32"))
					{
						LLVMValueRef operand48 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand48.TypeOf);
						fSharpFunc3.Invoke(operand48);
						il.Append(cil.MyInstruction.Conv_R8);
						MethodReference item113 = typs.md.ImportReference(typeof(Math).GetMethod("Round", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item113));
						il.Append(cil.MyInstruction.Conv_R4);
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.minnum.f64"))
					{
						fSharpFunc17.Invoke(op.GetOperand(0u).TypeOf);
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Unit>("minnum_f64", new object[0], null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.minnum.f32"))
					{
						fSharpFunc17.Invoke(op.GetOperand(0u).TypeOf);
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Unit>("minnum_f32", new object[0], null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.maxnum.f64"))
					{
						fSharpFunc17.Invoke(op.GetOperand(0u).TypeOf);
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Unit>("maxnum_f64", new object[0], null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.maxnum.f32"))
					{
						fSharpFunc17.Invoke(op.GetOperand(0u).TypeOf);
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Unit>("maxnum_f32", new object[0], null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.experimental.vector.reduce.add.v2i64"))
					{
						AddressOfValue addr10 = fSharpFunc4.Invoke(op.GetOperand(0u));
						il.Append(cil.MyInstruction.NewLdc_I8(0L));
						for (int i = 0; i < 1 + 1; i++)
						{
							emit_address_of_value(il, addr10);
							if (i > 0)
							{
								il.Append(cil.MyInstruction.NewLdc_I4(i * sizeof(long)));
								il.Append(cil.MyInstruction.Add);
							}
							il.Append(cil.MyInstruction.Ldind_I8);
							il.Append(cil.MyInstruction.Add);
						}
						fSharpFunc12.Invoke(null);
						release_address_of_value(il, addr10);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.experimental.vector.reduce.mul.v2i64"))
					{
						AddressOfValue addr11 = fSharpFunc4.Invoke(op.GetOperand(0u));
						il.Append(cil.MyInstruction.NewLdc_I8(1L));
						for (int j = 0; j < 1 + 1; j++)
						{
							emit_address_of_value(il, addr11);
							if (j > 0)
							{
								il.Append(cil.MyInstruction.NewLdc_I4(j * sizeof(long)));
								il.Append(cil.MyInstruction.Add);
							}
							il.Append(cil.MyInstruction.Ldind_I8);
							il.Append(cil.MyInstruction.Mul);
						}
						fSharpFunc12.Invoke(null);
						release_address_of_value(il, addr11);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.experimental.vector.reduce.add.v4i32"))
					{
						AddressOfValue addr12 = fSharpFunc4.Invoke(op.GetOperand(0u));
						il.Append(cil.MyInstruction.NewLdc_I4(0));
						for (int k = 0; k < 1 + 3; k++)
						{
							emit_address_of_value(il, addr12);
							if (k > 0)
							{
								il.Append(cil.MyInstruction.NewLdc_I4(k * sizeof(int)));
								il.Append(cil.MyInstruction.Add);
							}
							il.Append(cil.MyInstruction.Ldind_I4);
							il.Append(cil.MyInstruction.Add);
						}
						fSharpFunc12.Invoke(null);
						release_address_of_value(il, addr12);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.experimental.vector.reduce.add.v8i16"))
					{
						AddressOfValue addr13 = fSharpFunc4.Invoke(op.GetOperand(0u));
						il.Append(cil.MyInstruction.NewLdc_I4(0));
						for (int l = 0; l < 1 + 7; l++)
						{
							emit_address_of_value(il, addr13);
							if (l > 0)
							{
								il.Append(cil.MyInstruction.NewLdc_I4(l * sizeof(short)));
								il.Append(cil.MyInstruction.Add);
							}
							il.Append(cil.MyInstruction.Ldind_I2);
							il.Append(cil.MyInstruction.Add);
						}
						fSharpFunc12.Invoke(null);
						release_address_of_value(il, addr13);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.experimental.vector.reduce.mul.v4i32"))
					{
						AddressOfValue addr14 = fSharpFunc4.Invoke(op.GetOperand(0u));
						il.Append(cil.MyInstruction.NewLdc_I4(1));
						for (int m = 0; m < 1 + 3; m++)
						{
							emit_address_of_value(il, addr14);
							if (m > 0)
							{
								il.Append(cil.MyInstruction.NewLdc_I4(m * sizeof(int)));
								il.Append(cil.MyInstruction.Add);
							}
							il.Append(cil.MyInstruction.Ldind_I4);
							il.Append(cil.MyInstruction.Mul);
						}
						fSharpFunc12.Invoke(null);
						release_address_of_value(il, addr14);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.experimental.vector.reduce.smax.v4i32"))
					{
						MethodReference item114 = typs.md.ImportReference(typeof(Math).GetMethod("Max", new Type[2]
						{
							typeof(int),
							typeof(int)
						}));
						AddressOfValue addr15 = fSharpFunc4.Invoke(op.GetOperand(0u));
						il.Append(cil.MyInstruction.NewLdc_I4(int.MinValue));
						for (int n = 0; n < 1 + 3; n++)
						{
							emit_address_of_value(il, addr15);
							if (n > 0)
							{
								il.Append(cil.MyInstruction.NewLdc_I4(n * sizeof(int)));
								il.Append(cil.MyInstruction.Add);
							}
							il.Append(cil.MyInstruction.Ldind_I4);
							il.Append(cil.MyInstruction.NewCall(item114));
						}
						fSharpFunc12.Invoke(null);
						release_address_of_value(il, addr15);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.experimental.vector.reduce.umax.v4i32"))
					{
						MethodReference item115 = typs.md.ImportReference(typeof(Math).GetMethod("Max", new Type[2]
						{
							typeof(uint),
							typeof(uint)
						}));
						AddressOfValue addr16 = fSharpFunc4.Invoke(op.GetOperand(0u));
						il.Append(cil.MyInstruction.NewLdc_I4(0));
						for (int num16 = 0; num16 < 1 + 3; num16++)
						{
							emit_address_of_value(il, addr16);
							if (num16 > 0)
							{
								il.Append(cil.MyInstruction.NewLdc_I4(num16 * sizeof(uint)));
								il.Append(cil.MyInstruction.Add);
							}
							il.Append(cil.MyInstruction.Ldind_U4);
							il.Append(cil.MyInstruction.NewCall(item115));
						}
						fSharpFunc12.Invoke(null);
						release_address_of_value(il, addr16);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.minnum.v2f32"))
					{
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Unit>("v2f32_min", new object[0], null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.maxnum.v2f32"))
					{
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Unit>("v2f32_max", new object[0], null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.minnum.v2f64"))
					{
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Unit>("v2f64_min", new object[0], null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.maxnum.v2f64"))
					{
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Unit>("v2f64_max", new object[0], null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.minnum.v4f32"))
					{
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Unit>("v4f32_min", new object[0], null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.maxnum.v4f32"))
					{
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Unit>("v4f32_max", new object[0], null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.exp.f64"))
					{
						LLVMValueRef operand49 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand49.TypeOf);
						fSharpFunc3.Invoke(operand49);
						MethodReference item116 = typs.md.ImportReference(typeof(Math).GetMethod("Exp", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item116));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.fabs.f64"))
					{
						LLVMValueRef operand50 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand50.TypeOf);
						fSharpFunc3.Invoke(operand50);
						MethodReference item117 = typs.md.ImportReference(typeof(Math).GetMethod("Abs", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item117));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.fabs.f32"))
					{
						LLVMValueRef operand51 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand51.TypeOf);
						fSharpFunc3.Invoke(operand51);
						il.Append(cil.MyInstruction.Conv_R8);
						MethodReference item118 = typs.md.ImportReference(typeof(Math).GetMethod("Abs", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item118));
						il.Append(cil.MyInstruction.Conv_R4);
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.fabs.v2f64"))
					{
						LLVMValueRef operand52 = op.GetOperand(0u);
						fSharpFunc3.Invoke(operand52);
						int num17 = 2;
						string text16 = "f64";
						string text17 = "fabs";
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<int, string, string>>("v%P()%P()_%P()", new object[3] { num17, text16, text17 }, null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.sin.f64"))
					{
						LLVMValueRef operand53 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand53.TypeOf);
						fSharpFunc3.Invoke(operand53);
						MethodReference item119 = typs.md.ImportReference(typeof(Math).GetMethod("Sin", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item119));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.cos.f64"))
					{
						LLVMValueRef operand54 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand54.TypeOf);
						fSharpFunc3.Invoke(operand54);
						MethodReference item120 = typs.md.ImportReference(typeof(Math).GetMethod("Cos", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item120));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.cos.f32"))
					{
						LLVMValueRef operand55 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand55.TypeOf);
						fSharpFunc3.Invoke(operand55);
						il.Append(cil.MyInstruction.Conv_R8);
						MethodReference item121 = typs.md.ImportReference(typeof(Math).GetMethod("Cos", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.Conv_R4);
						il.Append(cil.MyInstruction.NewCall(item121));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.sqrt.v2f64"))
					{
						LLVMValueRef operand56 = op.GetOperand(0u);
						fSharpFunc3.Invoke(operand56);
						int num18 = 2;
						string text18 = "f64";
						string text19 = "sqrt";
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<int, string, string>>("v%P()%P()_%P()", new object[3] { num18, text18, text19 }, null)))));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.sqrt.f64"))
					{
						LLVMValueRef operand57 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand57.TypeOf);
						fSharpFunc3.Invoke(operand57);
						MethodReference item122 = typs.md.ImportReference(typeof(Math).GetMethod("Sqrt", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item122));
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.sqrt.f32"))
					{
						LLVMValueRef operand58 = op.GetOperand(0u);
						fSharpFunc17.Invoke(operand58.TypeOf);
						fSharpFunc3.Invoke(operand58);
						il.Append(cil.MyInstruction.Conv_R8);
						MethodReference item123 = typs.md.ImportReference(typeof(Math).GetMethod("Sqrt", new Type[1] { typeof(double) }));
						il.Append(cil.MyInstruction.NewCall(item123));
						il.Append(cil.MyInstruction.Conv_R4);
						fSharpFunc12.Invoke(null);
						break;
					}
					if (operand19.Name.StartsWith("llvm.memcpy."))
					{
						LLVMValueRef operand59 = op.GetOperand(0u);
						LLVMValueRef operand60 = op.GetOperand(1u);
						LLVMValueRef operand61 = op.GetOperand(2u);
						fSharpFunc3.Invoke(operand59);
						fSharpFunc3.Invoke(operand60);
						fSharpFunc3.Invoke(operand61);
						cil.FirstClassType firstClassType49 = llvm_type_to_firstclass_type(operand61.TypeOf);
						switch (firstClassType49.Tag)
						{
						case 0:
						{
							cil.FirstClassType.PrimitiveType primitiveType51 = (cil.FirstClassType.PrimitiveType)firstClassType49;
							if (primitiveType51.Item.Tag == 4)
							{
								il.Append(cil.MyInstruction.Conv_I4);
							}
							break;
						}
						case 4:
						{
							cil.FirstClassType.FunnyIntegerType funnyIntegerType21 = (cil.FirstClassType.FunnyIntegerType)firstClassType49;
							string message13 = "TODO";
							throw Operators.Failure(message13);
						}
						}
						il.Append(cil.MyInstruction.Cpblk);
						break;
					}
					if (operand19.Name.StartsWith("llvm.memmove."))
					{
						LLVMValueRef operand62 = op.GetOperand(0u);
						LLVMValueRef operand63 = op.GetOperand(1u);
						LLVMValueRef operand64 = op.GetOperand(2u);
						fSharpFunc3.Invoke(operand62);
						fSharpFunc3.Invoke(operand63);
						fSharpFunc3.Invoke(operand64);
						cil.FirstClassType firstClassType50 = llvm_type_to_firstclass_type(operand64.TypeOf);
						switch (firstClassType50.Tag)
						{
						case 0:
						{
							cil.FirstClassType.PrimitiveType primitiveType52 = (cil.FirstClassType.PrimitiveType)firstClassType50;
							if (primitiveType52.Item.Tag == 4)
							{
								il.Append(cil.MyInstruction.Conv_I4);
							}
							break;
						}
						case 4:
						{
							cil.FirstClassType.FunnyIntegerType funnyIntegerType22 = (cil.FirstClassType.FunnyIntegerType)firstClassType50;
							string message14 = "TODO";
							throw Operators.Failure(message14);
						}
						}
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("rt_memmove")));
						break;
					}
					if (is_llvm_overflow_intrinsic(operand19.Name))
					{
						ResultLocal resultLocal = fSharpFunc5.Invoke(null);
						cil.FirstClassType firstClassType51 = llvm_type_to_firstclass_type(op.TypeOf);
						if (firstClassType51.Tag == 2)
						{
							cil.FirstClassType.StructType structType3 = (cil.FirstClassType.StructType)firstClassType51;
							cil.StructType item124 = structType3.Item;
							cil.StructType styp = item124;
							FSharpFunc<MethodReference, Unit> fSharpFunc38 = new gen@2484(il, op, fSharpFunc3, fSharpFunc6, resultLocal, styp);
							FSharpFunc<MethodReference, Unit> fSharpFunc39 = new gen_strange@2506(il, op, fSharpFunc4, fSharpFunc6, resultLocal, styp);
							string name = operand19.Name;
							if (!string.Equals(name, "llvm.uadd.with.overflow.i128"))
							{
								if (!string.Equals(name, "llvm.usub.with.overflow.i128"))
								{
									if (!string.Equals(name, "llvm.umul.with.overflow.i128"))
									{
										if (!string.Equals(name, "llvm.sadd.with.overflow.i128"))
										{
											if (!string.Equals(name, "llvm.ssub.with.overflow.i128"))
											{
												if (!string.Equals(name, "llvm.smul.with.overflow.i128"))
												{
													if (!string.Equals(name, "llvm.uadd.with.overflow.i64"))
													{
														if (!string.Equals(name, "llvm.usub.with.overflow.i64"))
														{
															if (!string.Equals(name, "llvm.umul.with.overflow.i64"))
															{
																if (!string.Equals(name, "llvm.sadd.with.overflow.i64"))
																{
																	if (!string.Equals(name, "llvm.ssub.with.overflow.i64"))
																	{
																		if (!string.Equals(name, "llvm.smul.with.overflow.i64"))
																		{
																			if (!string.Equals(name, "llvm.uadd.with.overflow.i32"))
																			{
																				if (!string.Equals(name, "llvm.usub.with.overflow.i32"))
																				{
																					if (!string.Equals(name, "llvm.umul.with.overflow.i32"))
																					{
																						if (!string.Equals(name, "llvm.sadd.with.overflow.i32"))
																						{
																							if (!string.Equals(name, "llvm.ssub.with.overflow.i32"))
																							{
																								if (!string.Equals(name, "llvm.smul.with.overflow.i32"))
																								{
																									if (!string.Equals(name, "llvm.uadd.with.overflow.i16"))
																									{
																										if (!string.Equals(name, "llvm.usub.with.overflow.i16"))
																										{
																											if (!string.Equals(name, "llvm.umul.with.overflow.i16"))
																											{
																												if (!string.Equals(name, "llvm.sadd.with.overflow.i16"))
																												{
																													if (!string.Equals(name, "llvm.ssub.with.overflow.i16"))
																													{
																														if (!string.Equals(name, "llvm.smul.with.overflow.i16"))
																														{
																															if (!string.Equals(name, "llvm.uadd.with.overflow.i8"))
																															{
																																if (!string.Equals(name, "llvm.usub.with.overflow.i8"))
																																{
																																	if (!string.Equals(name, "llvm.umul.with.overflow.i8"))
																																	{
																																		if (!string.Equals(name, "llvm.sadd.with.overflow.i8"))
																																		{
																																			if (!string.Equals(name, "llvm.ssub.with.overflow.i8"))
																																			{
																																				if (string.Equals(name, "llvm.smul.with.overflow.i8"))
																																				{
																																					fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_smul_with_overflow_i8"));
																																				}
																																				else
																																				{
																																					FSharpFunc<string, Unit> fSharpFunc40 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, Unit>, Unit, string, Unit, string>("unknown: %s"));
																																					string name2 = operand19.Name;
																																					fSharpFunc40.Invoke(name2);
																																				}
																																			}
																																			else
																																			{
																																				fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_ssub_with_overflow_i8"));
																																			}
																																		}
																																		else
																																		{
																																			fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_sadd_with_overflow_i8"));
																																		}
																																	}
																																	else
																																	{
																																		fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_umul_with_overflow_i8"));
																																	}
																																}
																																else
																																{
																																	fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_usub_with_overflow_i8"));
																																}
																															}
																															else
																															{
																																fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_uadd_with_overflow_i8"));
																															}
																														}
																														else
																														{
																															fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_smul_with_overflow_i16"));
																														}
																													}
																													else
																													{
																														fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_ssub_with_overflow_i16"));
																													}
																												}
																												else
																												{
																													fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_sadd_with_overflow_i16"));
																												}
																											}
																											else
																											{
																												fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_umul_with_overflow_i16"));
																											}
																										}
																										else
																										{
																											fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_usub_with_overflow_i16"));
																										}
																									}
																									else
																									{
																										fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_uadd_with_overflow_i16"));
																									}
																								}
																								else
																								{
																									fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_smul_with_overflow_i32"));
																								}
																							}
																							else
																							{
																								fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_ssub_with_overflow_i32"));
																							}
																						}
																						else
																						{
																							fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_sadd_with_overflow_i32"));
																						}
																					}
																					else
																					{
																						fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_umul_with_overflow_i32"));
																					}
																				}
																				else
																				{
																					fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_usub_with_overflow_i32"));
																				}
																			}
																			else
																			{
																				fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_uadd_with_overflow_i32"));
																			}
																		}
																		else
																		{
																			fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_smul_with_overflow_i64"));
																		}
																	}
																	else
																	{
																		fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_ssub_with_overflow_i64"));
																	}
																}
																else
																{
																	fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_sadd_with_overflow_i64"));
																}
															}
															else
															{
																fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_umul_with_overflow_i64"));
															}
														}
														else
														{
															fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_usub_with_overflow_i64"));
														}
													}
													else
													{
														fSharpFunc38.Invoke(syms.f_get_rt@.Invoke("llvm_uadd_with_overflow_i64"));
													}
												}
												else
												{
													fSharpFunc39.Invoke(syms.f_get_rt@.Invoke("llvm_smul_with_overflow_i128"));
												}
											}
											else
											{
												fSharpFunc39.Invoke(syms.f_get_rt@.Invoke("llvm_ssub_with_overflow_i128"));
											}
										}
										else
										{
											fSharpFunc39.Invoke(syms.f_get_rt@.Invoke("llvm_sadd_with_overflow_i128"));
										}
									}
									else
									{
										fSharpFunc39.Invoke(syms.f_get_rt@.Invoke("llvm_umul_with_overflow_i128"));
									}
								}
								else
								{
									fSharpFunc39.Invoke(syms.f_get_rt@.Invoke("llvm_usub_with_overflow_i128"));
								}
							}
							else
							{
								fSharpFunc39.Invoke(syms.f_get_rt@.Invoke("llvm_uadd_with_overflow_i128"));
							}
							fSharpFunc9.Invoke(resultLocal);
							break;
						}
						string message15 = "not a struct";
						throw Operators.Failure(message15);
					}
					if (is_llvm_sat_intrinsic(operand19.Name))
					{
						FSharpFunc<MethodReference, Unit> fSharpFunc41 = new gen@2577-1(il, op, fSharpFunc3, fSharpFunc5, fSharpFunc6, fSharpFunc9);
						FSharpFunc<MethodReference, Unit> fSharpFunc42 = new gen_vec@2588(il, op, fSharpFunc3, fSharpFunc12);
						string name3 = operand19.Name;
						if (!string.Equals(name3, "llvm.uadd.sat.i64"))
						{
							if (!string.Equals(name3, "llvm.usub.sat.i64"))
							{
								if (!string.Equals(name3, "llvm.sadd.sat.i64"))
								{
									if (!string.Equals(name3, "llvm.ssub.sat.i64"))
									{
										if (!string.Equals(name3, "llvm.uadd.sat.i32"))
										{
											if (!string.Equals(name3, "llvm.usub.sat.i32"))
											{
												if (!string.Equals(name3, "llvm.sadd.sat.i32"))
												{
													if (!string.Equals(name3, "llvm.ssub.sat.i32"))
													{
														if (!string.Equals(name3, "llvm.uadd.sat.i16"))
														{
															if (!string.Equals(name3, "llvm.usub.sat.i16"))
															{
																if (!string.Equals(name3, "llvm.sadd.sat.i16"))
																{
																	if (!string.Equals(name3, "llvm.ssub.sat.i16"))
																	{
																		if (!string.Equals(name3, "llvm.uadd.sat.i8"))
																		{
																			if (!string.Equals(name3, "llvm.usub.sat.i8"))
																			{
																				if (!string.Equals(name3, "llvm.sadd.sat.i8"))
																				{
																					if (!string.Equals(name3, "llvm.ssub.sat.i8"))
																					{
																						if (string.Equals(name3, "llvm.usub.sat.v8i32"))
																						{
																							fSharpFunc42.Invoke(syms.f_get_rt@.Invoke("llvm_usub_sat_v8i32"));
																							break;
																						}
																						FSharpFunc<string, Unit> fSharpFunc43 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, Unit>, Unit, string, Unit, string>("unknown: %s"));
																						string name4 = operand19.Name;
																						fSharpFunc43.Invoke(name4);
																					}
																					else
																					{
																						fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_ssub_sat_i8"));
																					}
																				}
																				else
																				{
																					fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_sadd_sat_i8"));
																				}
																			}
																			else
																			{
																				fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_usub_sat_i8"));
																			}
																		}
																		else
																		{
																			fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_uadd_sat_i8"));
																		}
																	}
																	else
																	{
																		fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_ssub_sat_i16"));
																	}
																}
																else
																{
																	fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_sadd_sat_i16"));
																}
															}
															else
															{
																fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_usub_sat_i16"));
															}
														}
														else
														{
															fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_uadd_sat_i16"));
														}
													}
													else
													{
														fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_ssub_sat_i32"));
													}
												}
												else
												{
													fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_sadd_sat_i32"));
												}
											}
											else
											{
												fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_usub_sat_i32"));
											}
										}
										else
										{
											fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_uadd_sat_i32"));
										}
									}
									else
									{
										fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_ssub_sat_i64"));
									}
								}
								else
								{
									fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_sadd_sat_i64"));
								}
							}
							else
							{
								fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_usub_sat_i64"));
							}
						}
						else
						{
							fSharpFunc41.Invoke(syms.f_get_rt@.Invoke("llvm_uadd_sat_i64"));
						}
						break;
					}
					if (string.Equals(operand19.Name, "llvm.fshl.i32"))
					{
						LLVMValueRef operand65 = op.GetOperand(0u);
						LLVMValueRef operand66 = op.GetOperand(1u);
						LLVMValueRef operand67 = op.GetOperand(2u);
						fSharpFunc3.Invoke(operand65);
						fSharpFunc3.Invoke(operand67);
						il.Append(cil.MyInstruction.Shl);
						fSharpFunc3.Invoke(operand66);
						il.Append(cil.MyInstruction.NewLdc_I4(32));
						fSharpFunc3.Invoke(operand67);
						il.Append(cil.MyInstruction.Sub);
						il.Append(cil.MyInstruction.Shr_Un);
						il.Append(cil.MyInstruction.Or);
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.fshl.i64"))
					{
						LLVMValueRef operand68 = op.GetOperand(0u);
						LLVMValueRef operand69 = op.GetOperand(1u);
						LLVMValueRef operand70 = op.GetOperand(2u);
						fSharpFunc3.Invoke(operand68);
						fSharpFunc3.Invoke(operand70);
						il.Append(cil.MyInstruction.Shl);
						fSharpFunc3.Invoke(operand69);
						il.Append(cil.MyInstruction.NewLdc_I4(64));
						fSharpFunc3.Invoke(operand70);
						il.Append(cil.MyInstruction.Sub);
						il.Append(cil.MyInstruction.Shr_Un);
						il.Append(cil.MyInstruction.Or);
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.fshr.i32"))
					{
						LLVMValueRef operand71 = op.GetOperand(0u);
						LLVMValueRef operand72 = op.GetOperand(1u);
						LLVMValueRef operand73 = op.GetOperand(2u);
						fSharpFunc3.Invoke(operand71);
						fSharpFunc3.Invoke(operand73);
						il.Append(cil.MyInstruction.Shr_Un);
						fSharpFunc3.Invoke(operand72);
						il.Append(cil.MyInstruction.NewLdc_I4(32));
						fSharpFunc3.Invoke(operand73);
						il.Append(cil.MyInstruction.Sub);
						il.Append(cil.MyInstruction.Shl);
						il.Append(cil.MyInstruction.Or);
						fSharpFunc12.Invoke(null);
						break;
					}
					if (string.Equals(operand19.Name, "llvm.fshr.i64"))
					{
						LLVMValueRef operand74 = op.GetOperand(0u);
						LLVMValueRef operand75 = op.GetOperand(1u);
						LLVMValueRef operand76 = op.GetOperand(2u);
						fSharpFunc3.Invoke(operand74);
						fSharpFunc3.Invoke(operand76);
						il.Append(cil.MyInstruction.Shr_Un);
						fSharpFunc3.Invoke(operand75);
						il.Append(cil.MyInstruction.NewLdc_I4(64));
						fSharpFunc3.Invoke(operand76);
						il.Append(cil.MyInstruction.Sub);
						il.Append(cil.MyInstruction.Shl);
						il.Append(cil.MyInstruction.Or);
						fSharpFunc12.Invoke(null);
						break;
					}
					if (operand19.Name.StartsWith("llvm.bswap."))
					{
						FSharpFunc<int, FSharpFunc<FSharpFunc<Unit, Unit>, FSharpFunc<FSharpFunc<Unit, Unit>, Unit>>> func42 = new do_bswap@2702(il);
						if (string.Equals(operand19.Name, "llvm.bswap.i16"))
						{
							LLVMValueRef operand77 = op.GetOperand(0u);
							fSharpFunc3.Invoke(operand77);
							il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("bswap_i16")));
							fSharpFunc12.Invoke(null);
						}
						else if (string.Equals(operand19.Name, "llvm.bswap.i32"))
						{
							LLVMValueRef operand78 = op.GetOperand(0u);
							fSharpFunc3.Invoke(operand78);
							il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("bswap_i32")));
							fSharpFunc12.Invoke(null);
						}
						else if (string.Equals(operand19.Name, "llvm.bswap.i64"))
						{
							LLVMValueRef operand79 = op.GetOperand(0u);
							fSharpFunc3.Invoke(operand79);
							il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("bswap_i64")));
							fSharpFunc12.Invoke(null);
						}
						else if (string.Equals(operand19.Name, "llvm.bswap.v2i32"))
						{
							AddressOfValue addressOfValue11 = fSharpFunc4.Invoke(op.GetOperand(0u));
							ResultLocal resultLocal2 = fSharpFunc5.Invoke(null);
							FSharpFunc<Unit, Unit> arg10 = new f_load_dst@2740(fSharpFunc6, resultLocal2);
							FSharpFunc<Unit, Unit> arg11 = new f_load_src@2743-2(il, addressOfValue11);
							FSharpFunc<int, FSharpFunc<Unit, Unit>>.InvokeFast(func42, 4, arg11, arg10);
							FSharpFunc<Unit, Unit> arg12 = new f_load_dst@2750-1(il, fSharpFunc6, resultLocal2);
							FSharpFunc<Unit, Unit> arg13 = new f_load_src@2755-3(il, addressOfValue11);
							FSharpFunc<int, FSharpFunc<Unit, Unit>>.InvokeFast(func42, 4, arg13, arg12);
							release_address_of_value(il, addressOfValue11);
							fSharpFunc9.Invoke(resultLocal2);
						}
						else
						{
							FSharpFunc<string, Unit> fSharpFunc44 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<string, Unit>, Unit, string, Unit, string>("%s"));
							string name5 = operand19.Name;
							fSharpFunc44.Invoke(name5);
						}
						break;
					}
					if (operand19.Name.StartsWith("llvm.memset."))
					{
						LLVMValueRef operand80 = op.GetOperand(0u);
						LLVMValueRef operand81 = op.GetOperand(1u);
						LLVMValueRef operand82 = op.GetOperand(2u);
						fSharpFunc3.Invoke(operand80);
						fSharpFunc3.Invoke(operand81);
						fSharpFunc3.Invoke(operand82);
						cil.FirstClassType firstClassType52 = llvm_type_to_firstclass_type(operand82.TypeOf);
						if (firstClassType52.Tag == 0)
						{
							cil.FirstClassType.PrimitiveType primitiveType53 = (cil.FirstClassType.PrimitiveType)firstClassType52;
							if (primitiveType53.Item.Tag == 4)
							{
								il.Append(cil.MyInstruction.Conv_I4);
							}
						}
						il.Append(cil.MyInstruction.Initblk);
						break;
					}
					if (sgllvm.get_value_kind(operand19) == LLVMValueKind.LLVMInlineAsmValueKind)
					{
						FSharpFunc<LLVMValueRef, string> fSharpFunc45 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<LLVMValueRef, string>, Unit, string, string, LLVMValueRef>("%A"));
						LLVMValueRef func43 = operand19;
						string text20 = fSharpFunc45.Invoke(func43);
						if (<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("aarch64") && text20.Contains("brk 0xF003"))
						{
							MethodReference item125 = typs.md.ImportReference(typeof(Environment).GetMethod("FailFast", new Type[1] { typeof(string) }));
							il.Append(cil.MyInstruction.NewLdstr(text20));
							il.Append(cil.MyInstruction.NewCall(item125));
						}
						else if (!<StartupCode$llvm2cil>.$llvm_stuff.cecil.llvm_module@11.Target.StartsWith("aarch64") || !text20.Contains(" asm sideeffect \"\", "))
						{
							string func44 = text20;
							((FSharpFunc<string, Unit>)fSharpTypeFunc.Specialize<string>()).Invoke(func44);
						}
						break;
					}
					FSharpOption<LLVMTypeRef> return_value = op.TypeOf.Kind switch
					{
						LLVMTypeKind.LLVMVoidTypeKind => null, 
						_ => FSharpOption<LLVMTypeRef>.Some(op.TypeOf), 
					};
					FSharpFunc<Unit, Unit> save_call_result = new save_call_result@2803(fSharpFunc12, return_value);
					LLVMTypeRef lLVMTypeRef = sgllvm.get_called_function_type(op);
					int paramTypesCount = (int)lLVMTypeRef.ParamTypesCount;
					int num19 = op.OperandCount - 1;
					int count_extra_params = num19 - paramTypesCount;
					if (lLVMTypeRef.IsFunctionVarArg)
					{
						FSharpFunc<Unit, Unit> push_params = new push_params@2819(il, op, fSharpFunc3, paramTypesCount, count_extra_params);
						FSharpFunc<LLVMValueRef, Unit> fSharpFunc46 = new do_regular_call@2856(syms, il, save_call_result, push_params);
						FSharpFunc<LLVMValueRef, Unit> fSharpFunc47 = new do_indirect_call@2868(typs, il, op, fSharpFunc3, return_value, save_call_result, paramTypesCount, count_extra_params, push_params);
						switch (sgllvm.get_value_kind(operand19))
						{
						case LLVMValueKind.LLVMFunctionValueKind:
							fSharpFunc46.Invoke(operand19);
							break;
						default:
							fSharpFunc47.Invoke(operand19);
							break;
						}
					}
					else
					{
						FSharpFunc<Unit, Unit> push_params2 = new push_params@2902-1(op, fSharpFunc3);
						FSharpFunc<LLVMValueRef, Unit> fSharpFunc48 = new do_regular_call@2908-1(syms, il, op, save_call_result, push_params2);
						FSharpFunc<LLVMValueRef, Unit> fSharpFunc49 = new do_indirect_call@2922-1(typs, il, op, fSharpFunc3, return_value, save_call_result, push_params2);
						switch (sgllvm.get_value_kind(operand19))
						{
						case LLVMValueKind.LLVMFunctionValueKind:
							fSharpFunc48.Invoke(operand19);
							break;
						default:
							fSharpFunc49.Invoke(operand19);
							break;
						}
					}
				}
			}
			break;
		}
		case LLVMOpcode.LLVMGetElementPtr:
		{
			LLVMValueRef[] array = ArrayModule.OfSeq(SeqModule.Skip(2, sgllvm.get_operands(op)));
			LLVMValueRef operand2 = op.GetOperand(0u);
			LLVMValueRef operand3 = op.GetOperand(1u);
			FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> fSharpFunc18 = new do_single_offset@2964(il, fSharpFunc3);
			cil.FirstClassType firstClassType2 = llvm_type_to_firstclass_type(op.TypeOf);
			cil.FirstClassType firstClassType3;
			switch (firstClassType2.Tag)
			{
			case 1:
			{
				cil.FirstClassType.VectorType vectorType = (cil.FirstClassType.VectorType)firstClassType2;
				if (vectorType.Item.elemtype.Tag != 1)
				{
					firstClassType3 = firstClassType2;
					break;
				}
				uint count = vectorType.Item.count;
				ResultLocal func8 = fSharpFunc5.Invoke(null);
				Tuple<LLVMTypeKind, LLVMTypeKind> tuple2 = new Tuple<LLVMTypeKind, LLVMTypeKind>(operand2.TypeOf.Kind, operand3.TypeOf.Kind);
				Tuple<LLVMTypeKind, LLVMTypeKind> tuple3;
				cil.FirstClassType elem_type;
				AddressOfValue addressOfValue;
				FSharpTypeFunc fSharpTypeFunc2;
				cil.FirstClassType elem_type2;
				AddressOfValue addressOfValue2;
				FSharpTypeFunc fSharpTypeFunc3;
				cil.FirstClassType elem_type3;
				AddressOfValue addressOfValue3;
				AddressOfValue addressOfValue4;
				FSharpTypeFunc fSharpTypeFunc4;
				FSharpFunc<Tuple<LLVMTypeKind, LLVMTypeKind>, Unit> fSharpFunc19;
				Tuple<LLVMTypeKind, LLVMTypeKind> tuple4;
				LLVMTypeKind item3;
				LLVMTypeKind item4;
				switch (tuple2.Item1)
				{
				case LLVMTypeKind.LLVMPointerTypeKind:
					switch (tuple2.Item2)
					{
					case LLVMTypeKind.LLVMVectorTypeKind:
						goto IL_409b;
					}
					tuple3 = tuple2;
					goto IL_4115;
				case LLVMTypeKind.LLVMVectorTypeKind:
					switch (tuple2.Item2)
					{
					case LLVMTypeKind.LLVMVectorTypeKind:
						goto IL_3f33;
					case LLVMTypeKind.LLVMPointerTypeKind:
						goto IL_3fe5;
					}
					tuple3 = tuple2;
					goto IL_4115;
				default:
					{
						tuple3 = tuple2;
						goto IL_4115;
					}
					IL_3fe5:
					switch (operand2.TypeOf.ElementType.Kind)
					{
					case LLVMTypeKind.LLVMPointerTypeKind:
						break;
					default:
					{
						string message3 = "should not happen";
						throw Operators.Failure(message3);
					}
					}
					elem_type = llvm_type_to_firstclass_type(operand2.TypeOf.ElementType);
					addressOfValue = fSharpFunc4.Invoke(operand2);
					fSharpTypeFunc2 = new get_nth_val@3085-3(il, fSharpFunc10, array, operand3, fSharpFunc18, elem_type, addressOfValue);
					do_ptrvec_into_local(il, count, fSharpFunc8.Invoke(func8), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc2.Specialize<cil.CilWriter>());
					release_address_of_value(il, addressOfValue);
					break;
					IL_409b:
					elem_type2 = llvm_type_to_firstclass_type(operand2.TypeOf.ElementType);
					addressOfValue2 = fSharpFunc4.Invoke(operand3);
					fSharpTypeFunc3 = new get_nth_val@3127-4(il, fSharpFunc3, fSharpFunc10, load_vecprim_to_stack, array, operand2, operand3, elem_type2, addressOfValue2);
					do_ptrvec_into_local(il, count, fSharpFunc8.Invoke(func8), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc3.Specialize<cil.CilWriter>());
					release_address_of_value(il, addressOfValue2);
					break;
					IL_3f33:
					switch (operand2.TypeOf.ElementType.Kind)
					{
					case LLVMTypeKind.LLVMPointerTypeKind:
						break;
					default:
					{
						string message4 = "should not happen";
						throw Operators.Failure(message4);
					}
					}
					elem_type3 = llvm_type_to_firstclass_type(operand2.TypeOf.ElementType);
					addressOfValue3 = fSharpFunc4.Invoke(operand2);
					addressOfValue4 = fSharpFunc4.Invoke(operand3);
					fSharpTypeFunc4 = new get_nth_val@3021-2(il, fSharpFunc10, load_vecprim_to_stack, array, operand3, elem_type3, addressOfValue3, addressOfValue4);
					do_ptrvec_into_local(il, count, fSharpFunc8.Invoke(func8), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc4.Specialize<cil.CilWriter>());
					release_address_of_value(il, addressOfValue3);
					release_address_of_value(il, addressOfValue4);
					break;
					IL_4115:
					fSharpFunc19 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<Tuple<LLVMTypeKind, LLVMTypeKind>, Unit>, Unit, string, Unit, Tuple<LLVMTypeKind, LLVMTypeKind>>("should never happen: %A"));
					tuple4 = tuple3;
					item3 = tuple4.Item1;
					item4 = tuple4.Item2;
					fSharpFunc19.Invoke(new Tuple<LLVMTypeKind, LLVMTypeKind>(item3, item4));
					break;
				}
				fSharpFunc9.Invoke(func8);
				return;
			}
			case 5:
				switch (operand2.TypeOf.Kind)
				{
				case LLVMTypeKind.LLVMPointerTypeKind:
				{
					cil.FirstClassType arg2 = llvm_type_to_firstclass_type(operand2.TypeOf.ElementType);
					fSharpFunc3.Invoke(operand2);
					FSharpFunc<cil.FirstClassType, LLVMValueRef>.InvokeFast(fSharpFunc18, arg2, operand3);
					if (array.Length > 0)
					{
						cil.FirstClassType firstClassType4 = FSharpFunc<cil.FirstClassType, LLVMValueRef[]>.InvokeFast(fSharpFunc10, arg2, array, 0);
						cil.FirstClassType firstClassType5 = llvm_type_to_firstclass_type(op.TypeOf.ElementType);
						cil.FirstClassType firstClassType6 = firstClassType4;
						cil.FirstClassType obj = firstClassType5;
						if (!firstClassType6.Equals(obj, LanguagePrimitives.GenericEqualityComparer))
						{
							FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>> clo = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>>, TextWriter, Unit, Unit, Tuple<cil.FirstClassType, cil.FirstClassType>>("GEP type MISMATCH: %A vs %A"));
							FSharpFunc<cil.FirstClassType, cil.FirstClassType>.InvokeFast(new gen_instr@3001(clo), firstClassType4, firstClassType5);
						}
					}
					fSharpFunc12.Invoke(null);
					break;
				}
				default:
				{
					string message2 = "should not happen";
					throw Operators.Failure(message2);
				}
				}
				return;
			default:
				firstClassType3 = firstClassType2;
				break;
			}
			FSharpFunc<cil.FirstClassType, Unit> fSharpFunc20 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, Unit>, Unit, string, Unit, cil.FirstClassType>("GEP requires a pointer or a vec of pointers %A"));
			cil.FirstClassType func9 = firstClassType3;
			fSharpFunc20.Invoke(func9);
			break;
		}
		case LLVMOpcode.LLVMSelect:
		{
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple7 = new Tuple<cil.FirstClassType, cil.FirstClassType>(llvm_type_to_firstclass_type(op.GetOperand(0u).TypeOf), llvm_type_to_firstclass_type(op.GetOperand(1u).TypeOf));
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple8;
			switch (tuple7.Item1.Tag)
			{
			case 1:
			{
				cil.FirstClassType.VectorType vectorType6 = (cil.FirstClassType.VectorType)tuple7.Item1;
				if (vectorType6.Item.elemtype.Tag == 0)
				{
					if (tuple7.Item2.Tag == 1)
					{
						cil.FirstClassType.VectorType vectorType7 = (cil.FirstClassType.VectorType)tuple7.Item2;
						switch (vectorType7.Item.elemtype.Tag)
						{
						case 2:
						{
							cil.VectorElementType.VecPrim vecPrim5 = (cil.VectorElementType.VecPrim)vectorType7.Item.elemtype;
							cil.VecPrimitiveType item14 = vecPrim5.Item;
							uint count8 = vectorType6.Item.count;
							uint count9 = vectorType7.Item.count;
							int elem_size = cil.sizeof_vecprim(item14);
							AddressOfValue addressOfValue5 = fSharpFunc4.Invoke(op.GetOperand(0u));
							AddressOfValue addressOfValue6 = fSharpFunc4.Invoke(op.GetOperand(1u));
							AddressOfValue addressOfValue7 = fSharpFunc4.Invoke(op.GetOperand(2u));
							ResultLocal func15 = fSharpFunc5.Invoke(null);
							FSharpFunc<int, Unit> add_offset = new add_offset@3198-3(il);
							if (count8 != count9)
							{
								string message8 = "count mismatch";
								throw Operators.Failure(message8);
							}
							FSharpTypeFunc fSharpTypeFunc5 = new get_nth_val@3205-5(il, load_vecprim_to_stack, fSharpFunc11, item14, elem_size, addressOfValue5, addressOfValue6, addressOfValue7, add_offset);
							do_vec_into_local(il, cil.vecprim_to_prim(item14), count8, fSharpFunc8.Invoke(func15), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc5.Specialize<cil.CilWriter>());
							release_address_of_value(il, addressOfValue5);
							release_address_of_value(il, addressOfValue6);
							release_address_of_value(il, addressOfValue7);
							fSharpFunc9.Invoke(func15);
							return;
						}
						case 3:
						{
							cil.VectorElementType.VecFunny vecFunny = (cil.VectorElementType.VecFunny)vectorType7.Item.elemtype;
							cil.FunnyIntegerType item11 = vecFunny.Item;
							uint count6 = vectorType6.Item.count;
							uint count7 = vectorType7.Item.count;
							int num = cil.sizeof_funnyinteger_rounded_up(item11);
							AddressOfValue addr2 = fSharpFunc4.Invoke(op.GetOperand(0u));
							AddressOfValue addr3 = fSharpFunc4.Invoke(op.GetOperand(1u));
							AddressOfValue addr4 = fSharpFunc4.Invoke(op.GetOperand(2u));
							ResultLocal func13 = fSharpFunc5.Invoke(null);
							FSharpFunc<int, Unit> fSharpFunc23 = new add_offset@3240-4(il);
							if (count6 != count7)
							{
								string message7 = "count mismatch";
								throw Operators.Failure(message7);
							}
							int num2 = 0;
							int num3 = (int)(count6 - 1);
							if (num3 >= num2)
							{
								do
								{
									int func14 = num2 * num;
									emit_address_of_value(il, addr2);
									fSharpFunc23.Invoke(num2);
									fSharpFunc11.Invoke(cil.PrimitiveType.I1);
									cil.Label item12 = il.NewLabel();
									cil.Label item13 = il.NewLabel();
									il.Append(cil.MyInstruction.NewBrfalse(item12));
									fSharpFunc6.Invoke(func13);
									emit_address_of_value(il, addr3);
									fSharpFunc23.Invoke(func14);
									il.Append(cil.MyInstruction.NewBr(item13));
									il.Append(cil.MyInstruction.NewLabel(item12));
									fSharpFunc6.Invoke(func13);
									emit_address_of_value(il, addr4);
									fSharpFunc23.Invoke(func14);
									il.Append(cil.MyInstruction.NewLabel(item13));
									il.Append(cil.MyInstruction.NewLdc_I4(num));
									il.Append(cil.MyInstruction.Cpblk);
									num2++;
								}
								while (num2 != num3 + 1);
							}
							release_address_of_value(il, addr2);
							release_address_of_value(il, addr3);
							release_address_of_value(il, addr4);
							fSharpFunc9.Invoke(func13);
							return;
						}
						}
						tuple8 = tuple7;
					}
					else
					{
						tuple8 = tuple7;
					}
				}
				else
				{
					tuple8 = tuple7;
				}
				break;
			}
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType12 = (cil.FirstClassType.PrimitiveType)tuple7.Item1;
				if (primitiveType12.Item.Tag != 0)
				{
					tuple8 = tuple7;
					break;
				}
				fSharpFunc3.Invoke(op.GetOperand(0u));
				cil.Label item9 = il.NewLabel();
				cil.Label item10 = il.NewLabel();
				il.Append(cil.MyInstruction.NewBrfalse(item9));
				fSharpFunc3.Invoke(op.GetOperand(1u));
				il.Append(cil.MyInstruction.NewBr(item10));
				il.Append(cil.MyInstruction.NewLabel(item9));
				fSharpFunc3.Invoke(op.GetOperand(2u));
				il.Append(cil.MyInstruction.NewLabel(item10));
				fSharpFunc12.Invoke(null);
				return;
			}
			default:
				tuple8 = tuple7;
				break;
			}
			FSharpFunc<Tuple<cil.FirstClassType, cil.FirstClassType>, Unit> fSharpFunc24 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<Tuple<cil.FirstClassType, cil.FirstClassType>, Unit>, TextWriter, Unit, Unit, Tuple<cil.FirstClassType, cil.FirstClassType>>("POSSIBLE PROBLEM MATCH CASE: %A"));
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple9 = tuple8;
			cil.FirstClassType item15 = tuple9.Item1;
			cil.FirstClassType item16 = tuple9.Item2;
			fSharpFunc24.Invoke(new Tuple<cil.FirstClassType, cil.FirstClassType>(item15, item16));
			fSharpFunc3.Invoke(op.GetOperand(0u));
			cil.Label item17 = il.NewLabel();
			cil.Label item18 = il.NewLabel();
			il.Append(cil.MyInstruction.NewBrfalse(item17));
			fSharpFunc3.Invoke(op.GetOperand(1u));
			il.Append(cil.MyInstruction.NewBr(item18));
			il.Append(cil.MyInstruction.NewLabel(item17));
			fSharpFunc3.Invoke(op.GetOperand(2u));
			il.Append(cil.MyInstruction.NewLabel(item18));
			fSharpFunc12.Invoke(null);
			break;
		}
		case LLVMOpcode.LLVMSwitch:
		{
			cil.FirstClassType typ3 = llvm_type_to_firstclass_type(op.GetOperand(0u).TypeOf);
			FSharpFunc<LLVMValueRef, Unit> fSharpFunc51 = new load_as_u64@3308(syms, il, fSharpFunc3, fSharpFunc4, typ3);
			cil.Variable tempVariable = il.GetTempVariable(cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.I64));
			fSharpFunc51.Invoke(op.GetOperand(0u));
			il.Append(cil.MyInstruction.NewStloc(tempVariable));
			int num21 = (op.OperandCount - 2) / 2;
			int num22 = 0;
			int num23 = num21 - 1;
			if (num23 >= num22)
			{
				do
				{
					int num24 = num22 * 2 + 2;
					cil.Label item128 = FSharpFunc<LLVMValueRef, LLVMValueRef>.InvokeFast(func3, op, op.GetOperand((uint)(num24 + 1)));
					LLVMValueRef operand83 = op.GetOperand((uint)num24);
					fSharpFunc51.Invoke(operand83);
					il.Append(cil.MyInstruction.NewLdloc(tempVariable));
					il.Append(cil.MyInstruction.Ceq);
					il.Append(cil.MyInstruction.NewBrtrue(item128));
					num22++;
				}
				while (num22 != num23 + 1);
			}
			cil.Label item129 = FSharpFunc<LLVMValueRef, LLVMValueRef>.InvokeFast(func3, op, op.GetOperand(1u));
			il.Append(cil.MyInstruction.NewBr(item129));
			il.ReleaseTempVariable(tempVariable);
			break;
		}
		case LLVMOpcode.LLVMBr:
			if (op.IsConditional)
			{
				cil.Label item85 = FSharpFunc<LLVMValueRef, LLVMValueRef>.InvokeFast(func3, op, op.GetOperand(2u));
				cil.Label item86 = FSharpFunc<LLVMValueRef, LLVMValueRef>.InvokeFast(func3, op, op.GetOperand(1u));
				fSharpFunc3.Invoke(op.GetOperand(0u));
				il.Append(cil.MyInstruction.NewBrtrue(item85));
				il.Append(cil.MyInstruction.NewBr(item86));
			}
			else
			{
				cil.Label item87 = FSharpFunc<LLVMValueRef, LLVMValueRef>.InvokeFast(func3, op, op.GetOperand(0u));
				il.Append(cil.MyInstruction.NewBr(item87));
			}
			break;
		case LLVMOpcode.LLVMExtractValue:
		{
			LLVMValueRef operand = op.GetOperand(0u);
			cil.FirstClassType typ = llvm_type_to_firstclass_type(operand.TypeOf);
			AddressOfValue addr = fSharpFunc4.Invoke(operand);
			emit_address_of_value(il, addr);
			cil.FirstClassType arg = walk_constant_indexes(il, typ, sgllvm.get_indices(op), 0);
			FSharpFunc<cil.FirstClassType, LLVMValueRef>.InvokeFast(func2, arg, op);
			release_address_of_value(il, addr);
			break;
		}
		case LLVMOpcode.LLVMInsertValue:
		{
			cil.FirstClassType typ2 = llvm_type_to_firstclass_type(op.TypeOf);
			LLVMValueRef operand17 = op.GetOperand(0u);
			ResultLocal func40 = fSharpFunc5.Invoke(null);
			load_aggorvec_into_specific_local(syms, il, fSharpFunc8.Invoke(func40), args, f_get_instr_value, f_load_instr_value_to_dest, operand17);
			fSharpFunc6.Invoke(func40);
			cil.FirstClassType arg9 = walk_constant_indexes(il, typ2, sgllvm.get_indices(op), 0);
			LLVMValueRef operand18 = op.GetOperand(1u);
			FSharpFunc<cil.FirstClassType, LLVMValueRef>.InvokeFast(func, arg9, operand18);
			fSharpFunc9.Invoke(func40);
			break;
		}
		case LLVMOpcode.LLVMShuffleVector:
		{
			AddressOfValue addressOfValue12 = fSharpFunc4.Invoke(op.GetOperand(0u));
			AddressOfValue addressOfValue13 = fSharpFunc4.Invoke(op.GetOperand(1u));
			int[] a_mask = get_constant_vector_as_array(op.GetOperand(2u));
			int vectorSize = (int)op.GetOperand(0u).TypeOf.VectorSize;
			ResultLocal resultLocal3 = fSharpFunc5.Invoke(null);
			FSharpFunc<int, FSharpFunc<FSharpFunc<Unit, Unit>, Unit>> func45 = new do_shuffle@3397(il, fSharpFunc6, addressOfValue12, addressOfValue13, a_mask, vectorSize, resultLocal3);
			cil.FirstClassType firstClassType53 = llvm_type_to_firstclass_type(op.TypeOf);
			if (firstClassType53.Tag == 1)
			{
				cil.FirstClassType.VectorType vectorType45 = (cil.FirstClassType.VectorType)firstClassType53;
				switch (vectorType45.Item.elemtype.Tag)
				{
				default:
				{
					cil.VectorElementType.VecPrim vecPrim31 = (cil.VectorElementType.VecPrim)vectorType45.Item.elemtype;
					cil.VecPrimitiveType item127 = vecPrim31.Item;
					uint count40 = vectorType45.Item.count;
					int arg19 = cil.sizeof_vecprim(item127);
					FSharpFunc<Unit, Unit> arg20 = new f@3426(il, load_vecprim_to_stack, item127);
					FSharpFunc<int, FSharpFunc<Unit, Unit>>.InvokeFast(func45, arg19, arg20);
					break;
				}
				case 3:
				{
					cil.VectorElementType.VecFunny vecFunny3 = (cil.VectorElementType.VecFunny)vectorType45.Item.elemtype;
					cil.FunnyIntegerType item126 = vecFunny3.Item;
					uint count39 = vectorType45.Item.count;
					int num20 = cil.sizeof_funnyinteger_rounded_up(item126);
					FSharpFunc<Unit, Unit> arg18 = new f@3436-1(il, num20);
					FSharpFunc<int, FSharpFunc<Unit, Unit>>.InvokeFast(func45, num20, arg18);
					break;
				}
				case 0:
				{
					uint count38 = vectorType45.Item.count;
					int arg16 = 1;
					FSharpFunc<Unit, Unit> arg17 = new f@3445-2(il);
					FSharpFunc<int, FSharpFunc<Unit, Unit>>.InvokeFast(func45, arg16, arg17);
					break;
				}
				case 1:
				{
					uint count37 = vectorType45.Item.count;
					int arg14 = cil.get_sizeof(cil.FirstClassType.Ptr);
					FSharpFunc<Unit, Unit> arg15 = new f@3454-3(il);
					FSharpFunc<int, FSharpFunc<Unit, Unit>>.InvokeFast(func45, arg14, arg15);
					break;
				}
				}
			}
			else
			{
				cil.FirstClassType firstClassType54 = firstClassType53;
				FSharpFunc<cil.FirstClassType, Unit> fSharpFunc50 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, Unit>, Unit, string, Unit, cil.FirstClassType>("match case: %A"));
				cil.FirstClassType func46 = firstClassType54;
				fSharpFunc50.Invoke(func46);
			}
			release_address_of_value(il, addressOfValue12);
			release_address_of_value(il, addressOfValue13);
			fSharpFunc9.Invoke(resultLocal3);
			break;
		}
		case LLVMOpcode.LLVMExtractElement:
		{
			LLVMValueRef operand84 = op.GetOperand(0u);
			cil.FirstClassType typ4 = llvm_type_to_firstclass_type(operand84.TypeOf);
			AddressOfValue addr17 = fSharpFunc4.Invoke(operand84);
			emit_address_of_value(il, addr17);
			int[] array2 = new int[1];
			long num25 = get_integer_constant(op.GetOperand(1u));
			long num26 = num25;
			array2[0] = (int)num26;
			int[] indexes = array2;
			cil.FirstClassType arg21 = walk_constant_indexes(il, typ4, indexes, 0);
			FSharpFunc<cil.FirstClassType, LLVMValueRef>.InvokeFast(func2, arg21, op);
			release_address_of_value(il, addr17);
			break;
		}
		case LLVMOpcode.LLVMInsertElement:
		{
			cil.FirstClassType typ5 = llvm_type_to_firstclass_type(op.TypeOf);
			LLVMValueRef operand85 = op.GetOperand(0u);
			ResultLocal func47 = fSharpFunc5.Invoke(null);
			switch (sgllvm.get_value_kind(operand85))
			{
			case LLVMValueKind.LLVMUndefValueValueKind:
				fSharpFunc6.Invoke(func47);
				break;
			default:
				load_aggorvec_into_specific_local(syms, il, fSharpFunc8.Invoke(func47), args, f_get_instr_value, f_load_instr_value_to_dest, operand85);
				fSharpFunc6.Invoke(func47);
				break;
			}
			int[] array3 = new int[1];
			long num27 = get_integer_constant(op.GetOperand(2u));
			long num28 = num27;
			array3[0] = (int)num28;
			int[] indexes2 = array3;
			cil.FirstClassType arg22 = walk_constant_indexes(il, typ5, indexes2, 0);
			LLVMValueRef operand86 = op.GetOperand(1u);
			FSharpFunc<cil.FirstClassType, LLVMValueRef>.InvokeFast(func, arg22, operand86);
			fSharpFunc9.Invoke(func47);
			break;
		}
		case LLVMOpcode.LLVMICmp:
		{
			LLVMIntPredicate iCmpPredicate = op.ICmpPredicate;
			cil.FirstClassType firstClassType55 = llvm_type_to_firstclass_type(op.GetOperand(0u).TypeOf);
			cil.FirstClassType firstClassType56 = firstClassType55;
			switch (firstClassType56.Tag)
			{
			case 1:
			{
				cil.FirstClassType.VectorType vectorType46 = (cil.FirstClassType.VectorType)firstClassType56;
				switch (vectorType46.Item.elemtype.Tag)
				{
				case 2:
				{
					cil.VectorElementType.VecPrim vecPrim32 = (cil.VectorElementType.VecPrim)vectorType46.Item.elemtype;
					cil.VecPrimitiveType item132 = vecPrim32.Item;
					uint count43 = vectorType46.Item.count;
					int elem_size4 = cil.sizeof_vecprim(item132);
					AddressOfValue addressOfValue18 = fSharpFunc4.Invoke(op.GetOperand(0u));
					AddressOfValue addressOfValue19 = fSharpFunc4.Invoke(op.GetOperand(1u));
					ResultLocal func50 = fSharpFunc5.Invoke(null);
					FSharpFunc<int, Unit> add_offset4 = new add_offset@3595-5(il);
					FSharpTypeFunc fSharpTypeFunc9 = new get_nth_val@3599-6(il, op, load_vecprim_to_stack, item132, elem_size4, addressOfValue18, addressOfValue19, add_offset4);
					do_vec_into_local(il, cil.PrimitiveType.I1, count43, fSharpFunc8.Invoke(func50), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc9.Specialize<cil.CilWriter>());
					release_address_of_value(il, addressOfValue18);
					release_address_of_value(il, addressOfValue19);
					fSharpFunc9.Invoke(func50);
					return;
				}
				case 3:
				{
					cil.VectorElementType.VecFunny vecFunny4 = (cil.VectorElementType.VecFunny)vectorType46.Item.elemtype;
					cil.FunnyIntegerType item131 = vecFunny4.Item;
					uint count42 = vectorType46.Item.count;
					int elem_size3 = cil.sizeof_funnyinteger_rounded_up(item131);
					AddressOfValue addressOfValue16 = fSharpFunc4.Invoke(op.GetOperand(0u));
					AddressOfValue addressOfValue17 = fSharpFunc4.Invoke(op.GetOperand(1u));
					ResultLocal func49 = fSharpFunc5.Invoke(null);
					FSharpFunc<int, Unit> add_offset3 = new add_offset@3655-7(il);
					FSharpTypeFunc fSharpTypeFunc8 = new get_nth_val@3659-8(syms, il, op, elem_size3, addressOfValue16, addressOfValue17, add_offset3);
					do_vec_into_local(il, cil.PrimitiveType.I1, count42, fSharpFunc8.Invoke(func49), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc8.Specialize<cil.CilWriter>());
					release_address_of_value(il, addressOfValue16);
					release_address_of_value(il, addressOfValue17);
					fSharpFunc9.Invoke(func49);
					return;
				}
				case 1:
				{
					uint count41 = vectorType46.Item.count;
					int elem_size2 = cil.get_sizeof(cil.FirstClassType.Ptr);
					AddressOfValue addressOfValue14 = fSharpFunc4.Invoke(op.GetOperand(0u));
					AddressOfValue addressOfValue15 = fSharpFunc4.Invoke(op.GetOperand(1u));
					ResultLocal func48 = fSharpFunc5.Invoke(null);
					FSharpFunc<int, Unit> add_offset2 = new add_offset@3625-6(il);
					FSharpTypeFunc fSharpTypeFunc7 = new get_nth_val@3629-7(il, op, elem_size2, addressOfValue14, addressOfValue15, add_offset2);
					do_vec_into_local(il, cil.PrimitiveType.I1, count41, fSharpFunc8.Invoke(func48), (FSharpFunc<cil.CilWriter, FSharpFunc<int, Unit>>)fSharpTypeFunc7.Specialize<cil.CilWriter>());
					release_address_of_value(il, addressOfValue14);
					release_address_of_value(il, addressOfValue15);
					fSharpFunc9.Invoke(func48);
					return;
				}
				}
				cil.FirstClassType firstClassType57 = firstClassType56;
				break;
			}
			case 4:
			{
				cil.FunnyIntegerType item133 = ((cil.FirstClassType.FunnyIntegerType)firstClassType56).Item;
				if (item133.bits < 64)
				{
					MethodReference methodReference2;
					switch (iCmpPredicate)
					{
					case LLVMIntPredicate.LLVMIntUGT:
					case LLVMIntPredicate.LLVMIntUGE:
					case LLVMIntPredicate.LLVMIntULT:
					case LLVMIntPredicate.LLVMIntULE:
						methodReference2 = syms.f_get_rt@.Invoke("part_to_u64");
						break;
					default:
						methodReference2 = syms.f_get_rt@.Invoke("part_to_i64");
						break;
					}
					MethodReference item134 = methodReference2;
					AddressOfValue addr18 = fSharpFunc4.Invoke(op.GetOperand(0u));
					AddressOfValue addr19 = fSharpFunc4.Invoke(op.GetOperand(1u));
					il.Append(cil.MyInstruction.NewLdc_I4(item133.bits));
					emit_address_of_value(il, addr18);
					il.Append(cil.MyInstruction.NewCall(item134));
					il.Append(cil.MyInstruction.NewLdc_I4(item133.bits));
					emit_address_of_value(il, addr19);
					il.Append(cil.MyInstruction.NewCall(item134));
					emit_icmp(il, op.ICmpPredicate);
					fSharpFunc12.Invoke(null);
					release_address_of_value(il, addr18);
					release_address_of_value(il, addr19);
				}
				else
				{
					fSharpFunc3.Invoke(op.GetOperand(0u));
					cil.Variable item135 = il.NewVariable(firstClassType55);
					il.Append(cil.MyInstruction.NewStloc(item135));
					fSharpFunc3.Invoke(op.GetOperand(1u));
					cil.Variable item136 = il.NewVariable(firstClassType55);
					il.Append(cil.MyInstruction.NewStloc(item136));
					il.Append(cil.MyInstruction.NewLdc_I4(cil.sizeof_funnyinteger_rounded_up(item133)));
					il.Append(cil.MyInstruction.NewLdloca(item135));
					il.Append(cil.MyInstruction.NewLdloca(item136));
					emit_strange_icmp(il, syms, iCmpPredicate);
					fSharpFunc12.Invoke(null);
				}
				return;
			}
			case 5:
				fSharpFunc3.Invoke(op.GetOperand(0u));
				fSharpFunc3.Invoke(op.GetOperand(1u));
				emit_icmp(il, op.ICmpPredicate);
				fSharpFunc12.Invoke(null);
				return;
			case 0:
			{
				cil.PrimitiveType item130 = ((cil.FirstClassType.PrimitiveType)firstClassType56).Item;
				switch (iCmpPredicate)
				{
				case LLVMIntPredicate.LLVMIntUGT:
				case LLVMIntPredicate.LLVMIntUGE:
				case LLVMIntPredicate.LLVMIntULT:
				case LLVMIntPredicate.LLVMIntULE:
				{
					int num29 = item130.Tag switch
					{
						1 => 255, 
						2 => 65535, 
						_ => 0, 
					};
					fSharpFunc3.Invoke(op.GetOperand(0u));
					if (num29 != 0)
					{
						il.Append(cil.MyInstruction.NewLdc_I4(num29));
						il.Append(cil.MyInstruction.And);
					}
					fSharpFunc3.Invoke(op.GetOperand(1u));
					if (num29 != 0)
					{
						il.Append(cil.MyInstruction.NewLdc_I4(num29));
						il.Append(cil.MyInstruction.And);
					}
					emit_icmp(il, iCmpPredicate);
					break;
				}
				default:
					fSharpFunc3.Invoke(op.GetOperand(0u));
					fSharpFunc3.Invoke(op.GetOperand(1u));
					emit_icmp(il, op.ICmpPredicate);
					break;
				}
				fSharpFunc12.Invoke(null);
				return;
			}
			default:
			{
				cil.FirstClassType firstClassType57 = firstClassType56;
				break;
			}
			}
			FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> clo6 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>>, Unit, string, Unit, Tuple<cil.FirstClassType, LLVMValueRef>>("match case icmp %A : %A"));
			FSharpFunc<cil.FirstClassType, LLVMValueRef>.InvokeFast(new gen_instr@3677-2(clo6), firstClassType55, op);
			break;
		}
		case LLVMOpcode.LLVMFCmp:
		{
			object obj2;
			switch (op.FCmpPredicate)
			{
			case LLVMRealPredicate.LLVMRealOEQ:
				obj2 = "oeq";
				break;
			case LLVMRealPredicate.LLVMRealOGT:
				obj2 = "ogt";
				break;
			case LLVMRealPredicate.LLVMRealOLT:
				obj2 = "olt";
				break;
			case LLVMRealPredicate.LLVMRealONE:
				obj2 = "one";
				break;
			case LLVMRealPredicate.LLVMRealOGE:
				obj2 = "oge";
				break;
			case LLVMRealPredicate.LLVMRealOLE:
				obj2 = "ole";
				break;
			case LLVMRealPredicate.LLVMRealUEQ:
				obj2 = "ueq";
				break;
			case LLVMRealPredicate.LLVMRealUNE:
				obj2 = "une";
				break;
			case LLVMRealPredicate.LLVMRealUGT:
				obj2 = "ugt";
				break;
			case LLVMRealPredicate.LLVMRealULT:
				obj2 = "ult";
				break;
			case LLVMRealPredicate.LLVMRealUGE:
				obj2 = "uge";
				break;
			case LLVMRealPredicate.LLVMRealULE:
				obj2 = "ule";
				break;
			case LLVMRealPredicate.LLVMRealUNO:
				obj2 = "uno";
				break;
			case LLVMRealPredicate.LLVMRealORD:
				obj2 = "ord";
				break;
			default:
			{
				FSharpFunc<LLVMValueRef, string> fSharpFunc32 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, string>, Unit, string, string, LLVMValueRef>("unknown real predicate: %A"));
				LLVMValueRef func36 = op;
				obj2 = fSharpFunc32.Invoke(func36);
				break;
			}
			}
			string text7 = (string)obj2;
			cil.FirstClassType firstClassType44 = llvm_type_to_firstclass_type(op.GetOperand(0u).TypeOf);
			switch (firstClassType44.Tag)
			{
			case 1:
			{
				cil.FirstClassType.VectorType vectorType42 = (cil.FirstClassType.VectorType)firstClassType44;
				if (vectorType42.Item.elemtype.Tag == 2)
				{
					cil.VectorElementType.VecPrim vecPrim29 = (cil.VectorElementType.VecPrim)vectorType42.Item.elemtype;
					cil.VecPrimitiveType item83 = vecPrim29.Item;
					uint count35 = vectorType42.Item.count;
					ResultLocal func37 = fSharpFunc5.Invoke(null);
					fSharpFunc6.Invoke(func37);
					fSharpFunc3.Invoke(op.GetOperand(0u));
					fSharpFunc3.Invoke(op.GetOperand(1u));
					string text9 = vecprim_to_prim_name(item83);
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<uint, string, string>>("fcmp_v%P()%P()_%P()", new object[3] { count35, text9, text7 }, null)))));
					fSharpFunc9.Invoke(func37);
					return;
				}
				break;
			}
			case 0:
			{
				cil.PrimitiveType item82 = ((cil.FirstClassType.PrimitiveType)firstClassType44).Item;
				fSharpFunc3.Invoke(op.GetOperand(0u));
				fSharpFunc3.Invoke(op.GetOperand(1u));
				string text8 = item82.Tag switch
				{
					5 => "f32", 
					6 => "f64", 
					_ => throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 3703, 26), 
				};
				il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<string, string>>("fcmp_%P()_%P()", new object[2] { text8, text7 }, null)))));
				fSharpFunc12.Invoke(null);
				return;
			}
			}
			FSharpFunc<LLVMValueRef, Unit> fSharpFunc33 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, Unit, string, Unit, LLVMValueRef>("match case: %A"));
			LLVMValueRef func38 = op;
			fSharpFunc33.Invoke(func38);
			break;
		}
		case LLVMOpcode.LLVMFNeg:
			fSharpFunc3.Invoke(op.GetOperand(0u));
			il.Append(cil.MyInstruction.Neg);
			fSharpFunc12.Invoke(null);
			break;
		case LLVMOpcode.LLVMFAdd:
		case LLVMOpcode.LLVMFSub:
		case LLVMOpcode.LLVMFMul:
		case LLVMOpcode.LLVMFDiv:
		case LLVMOpcode.LLVMFRem:
		{
			cil.FirstClassType firstClassType37 = llvm_type_to_firstclass_type(op.TypeOf);
			cil.FirstClassType firstClassType38 = firstClassType37;
			switch (firstClassType38.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType48 = (cil.FirstClassType.PrimitiveType)firstClassType38;
				fSharpFunc16.Invoke(op);
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType35 = (cil.FirstClassType.VectorType)firstClassType38;
				fSharpFunc15.Invoke(op);
				break;
			}
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 3732, 18);
			}
			break;
		}
		case LLVMOpcode.LLVMAdd:
		case LLVMOpcode.LLVMSub:
		case LLVMOpcode.LLVMMul:
		case LLVMOpcode.LLVMSDiv:
		case LLVMOpcode.LLVMSRem:
		case LLVMOpcode.LLVMAnd:
		case LLVMOpcode.LLVMOr:
		case LLVMOpcode.LLVMXor:
		{
			cil.FirstClassType firstClassType34 = llvm_type_to_firstclass_type(op.TypeOf);
			cil.FirstClassType firstClassType35 = firstClassType34;
			switch (firstClassType35.Tag)
			{
			case 4:
			{
				cil.FirstClassType.FunnyIntegerType funnyIntegerType20 = (cil.FirstClassType.FunnyIntegerType)firstClassType35;
				cil.FunnyIntegerType item77 = funnyIntegerType20.Item;
				FSharpFunc<cil.FunnyIntegerType, LLVMValueRef>.InvokeFast(func4, item77, op);
				break;
			}
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType44 = (cil.FirstClassType.PrimitiveType)firstClassType35;
				fSharpFunc16.Invoke(op);
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType32 = (cil.FirstClassType.VectorType)firstClassType35;
				fSharpFunc15.Invoke(op);
				break;
			}
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 3746, 18);
			}
			break;
		}
		case LLVMOpcode.LLVMShl:
		{
			cil.FirstClassType firstClassType32 = llvm_type_to_firstclass_type(op.TypeOf);
			cil.FirstClassType firstClassType33 = firstClassType32;
			switch (firstClassType33.Tag)
			{
			case 1:
			{
				cil.FirstClassType.VectorType vectorType31 = (cil.FirstClassType.VectorType)firstClassType33;
				fSharpFunc15.Invoke(op);
				break;
			}
			case 4:
			{
				cil.FirstClassType.FunnyIntegerType funnyIntegerType19 = (cil.FirstClassType.FunnyIntegerType)firstClassType33;
				cil.FunnyIntegerType item76 = funnyIntegerType19.Item;
				FSharpFunc<cil.FunnyIntegerType, LLVMValueRef>.InvokeFast(func5, item76, op);
				break;
			}
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType43 = (cil.FirstClassType.PrimitiveType)firstClassType33;
				fSharpFunc16.Invoke(op);
				break;
			}
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 3755, 18);
			}
			break;
		}
		case LLVMOpcode.LLVMLShr:
		{
			cil.FirstClassType firstClassType26 = llvm_type_to_firstclass_type(op.TypeOf);
			cil.FirstClassType firstClassType27 = firstClassType26;
			switch (firstClassType27.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType40 = (cil.FirstClassType.PrimitiveType)firstClassType27;
				int num11;
				int num12;
				cil.MyInstruction instr2;
				switch (primitiveType40.Item.Tag)
				{
				case 1:
				case 2:
				{
					cil.FirstClassType firstClassType28 = firstClassType26;
					if (firstClassType28.Tag != 0)
					{
						goto IL_5bba;
					}
					cil.FirstClassType.PrimitiveType primitiveType41 = (cil.FirstClassType.PrimitiveType)firstClassType28;
					switch (primitiveType41.Item.Tag)
					{
					case 1:
						break;
					case 2:
						goto IL_5bb2;
					default:
						goto IL_5bba;
					}
					num11 = 255;
					goto IL_5bbc;
				}
				default:
					{
						fSharpFunc16.Invoke(op);
						break;
					}
					IL_5bba:
					num11 = 0;
					goto IL_5bbc;
					IL_5bb2:
					num11 = 65535;
					goto IL_5bbc;
					IL_5bbc:
					num12 = num11;
					fSharpFunc3.Invoke(op.GetOperand(0u));
					if (num12 != 0)
					{
						il.Append(cil.MyInstruction.NewLdc_I4(num12));
						il.Append(cil.MyInstruction.And);
					}
					fSharpFunc3.Invoke(op.GetOperand(1u));
					if (num12 != 0)
					{
						il.Append(cil.MyInstruction.NewLdc_I4(num12));
						il.Append(cil.MyInstruction.And);
					}
					instr2 = fSharpFunc14.Invoke(op);
					il.Append(instr2);
					fSharpFunc12.Invoke(null);
					break;
				}
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType30 = (cil.FirstClassType.VectorType)firstClassType27;
				fSharpFunc15.Invoke(op);
				break;
			}
			case 4:
			{
				cil.FunnyIntegerType item75 = ((cil.FirstClassType.FunnyIntegerType)firstClassType27).Item;
				FSharpFunc<cil.FunnyIntegerType, LLVMValueRef>.InvokeFast(func5, item75, op);
				break;
			}
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 3763, 18);
			}
			break;
		}
		case LLVMOpcode.LLVMAShr:
		{
			cil.FirstClassType firstClassType24 = llvm_type_to_firstclass_type(op.TypeOf);
			cil.FirstClassType firstClassType25 = firstClassType24;
			switch (firstClassType25.Tag)
			{
			case 4:
			{
				cil.FirstClassType.FunnyIntegerType funnyIntegerType18 = (cil.FirstClassType.FunnyIntegerType)firstClassType25;
				cil.FunnyIntegerType item67 = funnyIntegerType18.Item;
				FSharpFunc<cil.FunnyIntegerType, LLVMValueRef>.InvokeFast(func5, item67, op);
				break;
			}
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType34 = (cil.FirstClassType.PrimitiveType)firstClassType25;
				fSharpFunc16.Invoke(op);
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType25 = (cil.FirstClassType.VectorType)firstClassType25;
				fSharpFunc15.Invoke(op);
				break;
			}
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 3792, 18);
			}
			break;
		}
		case LLVMOpcode.LLVMURem:
		{
			cil.FirstClassType firstClassType20 = llvm_type_to_firstclass_type(op.TypeOf);
			cil.FirstClassType firstClassType21 = firstClassType20;
			switch (firstClassType21.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType31 = (cil.FirstClassType.PrimitiveType)firstClassType21;
				switch (primitiveType31.Item.Tag)
				{
				case 1:
					fSharpFunc3.Invoke(op.GetOperand(0u));
					il.Append(cil.MyInstruction.NewLdc_I4(255));
					il.Append(cil.MyInstruction.And);
					fSharpFunc3.Invoke(op.GetOperand(1u));
					il.Append(cil.MyInstruction.NewLdc_I4(255));
					il.Append(cil.MyInstruction.And);
					il.Append(cil.MyInstruction.Rem_Un);
					fSharpFunc12.Invoke(null);
					break;
				case 2:
					fSharpFunc3.Invoke(op.GetOperand(0u));
					il.Append(cil.MyInstruction.NewLdc_I4(65535));
					il.Append(cil.MyInstruction.And);
					fSharpFunc3.Invoke(op.GetOperand(1u));
					il.Append(cil.MyInstruction.NewLdc_I4(65535));
					il.Append(cil.MyInstruction.And);
					il.Append(cil.MyInstruction.Rem_Un);
					fSharpFunc12.Invoke(null);
					break;
				default:
					fSharpFunc16.Invoke(op);
					break;
				}
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType18 = (cil.FirstClassType.VectorType)firstClassType21;
				fSharpFunc15.Invoke(op);
				break;
			}
			case 4:
			{
				cil.FunnyIntegerType item55 = ((cil.FirstClassType.FunnyIntegerType)firstClassType21).Item;
				FSharpFunc<cil.FunnyIntegerType, LLVMValueRef>.InvokeFast(func4, item55, op);
				break;
			}
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 3800, 18);
			}
			break;
		}
		case LLVMOpcode.LLVMUDiv:
		{
			cil.FirstClassType firstClassType16 = llvm_type_to_firstclass_type(op.TypeOf);
			cil.FirstClassType firstClassType17 = firstClassType16;
			switch (firstClassType17.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType26 = (cil.FirstClassType.PrimitiveType)firstClassType17;
				cil.PrimitiveType item48 = primitiveType26.Item;
				int num7 = item48.Tag switch
				{
					1 => 255, 
					2 => 65535, 
					_ => 0, 
				};
				fSharpFunc3.Invoke(op.GetOperand(0u));
				if (num7 != 0)
				{
					il.Append(cil.MyInstruction.NewLdc_I4(num7));
					il.Append(cil.MyInstruction.And);
				}
				fSharpFunc3.Invoke(op.GetOperand(1u));
				if (num7 != 0)
				{
					il.Append(cil.MyInstruction.NewLdc_I4(num7));
					il.Append(cil.MyInstruction.And);
				}
				cil.MyInstruction instr = fSharpFunc14.Invoke(op);
				il.Append(instr);
				fSharpFunc12.Invoke(null);
				break;
			}
			case 4:
			{
				cil.FirstClassType.FunnyIntegerType funnyIntegerType16 = (cil.FirstClassType.FunnyIntegerType)firstClassType17;
				cil.FunnyIntegerType item47 = funnyIntegerType16.Item;
				FSharpFunc<cil.FunnyIntegerType, LLVMValueRef>.InvokeFast(func4, item47, op);
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType14 = (cil.FirstClassType.VectorType)firstClassType17;
				fSharpFunc15.Invoke(op);
				break;
			}
			default:
				throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 3831, 18);
			}
			break;
		}
		case LLVMOpcode.LLVMAlloca:
			switch (op.TypeOf.Kind)
			{
			case LLVMTypeKind.LLVMPointerTypeKind:
			{
				cil.FirstClassType t = llvm_type_to_firstclass_type(op.TypeOf.ElementType);
				LLVMValueRef operand9 = op.GetOperand(0u);
				FSharpOption<long> fSharpOption = get_maybe_integer_constant(operand9);
				if (fSharpOption != null)
				{
					FSharpOption<long> fSharpOption2 = fSharpOption;
					if (fSharpOption2.Value != 1)
					{
						long value = fSharpOption2.Value;
						int num5 = cil.get_sizeof(t);
						if (num5 == 0)
						{
							string message10 = "TODO";
							throw Operators.Failure(message10);
						}
						il.Append(cil.MyInstruction.NewLdc_I4(num5));
						if ((int)value != 1)
						{
							il.Append(cil.MyInstruction.NewLdc_I4((int)value));
							il.Append(cil.MyInstruction.Mul);
						}
						il.Append(cil.MyInstruction.Localloc);
						fSharpFunc12.Invoke(null);
					}
				}
				else
				{
					fSharpFunc3.Invoke(operand9);
					int num6 = cil.get_sizeof(t);
					switch (num6)
					{
					case 0:
					{
						string message11 = "TODO";
						throw Operators.Failure(message11);
					}
					default:
						il.Append(cil.MyInstruction.NewLdc_I4(num6));
						il.Append(cil.MyInstruction.Mul);
						break;
					case 1:
						break;
					}
					il.Append(cil.MyInstruction.Localloc);
					fSharpFunc12.Invoke(null);
				}
				break;
			}
			default:
			{
				string message9 = "alloca type not a ptr";
				throw Operators.Failure(message9);
			}
			}
			break;
		case LLVMOpcode.LLVMFreeze:
		{
			LLVMValueRef operand6 = op.GetOperand(0u);
			fSharpFunc3.Invoke(operand6);
			fSharpFunc12.Invoke(null);
			break;
		}
		case LLVMOpcode.LLVMStore:
		{
			LLVMValueRef operand7 = op.GetOperand(0u);
			cil.FirstClassType arg3 = llvm_type_to_firstclass_type(operand7.TypeOf);
			fSharpFunc3.Invoke(op.GetOperand(1u));
			FSharpFunc<cil.FirstClassType, LLVMValueRef>.InvokeFast(func, arg3, operand7);
			break;
		}
		case LLVMOpcode.LLVMFPExt:
		{
			LLVMValueRef operand5 = op.GetOperand(0u);
			cil.FirstClassType item7 = llvm_type_to_firstclass_type(operand5.TypeOf);
			cil.FirstClassType item8 = llvm_type_to_firstclass_type(op.TypeOf);
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple6 = new Tuple<cil.FirstClassType, cil.FirstClassType>(item7, item8);
			switch (tuple6.Item1.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType9 = (cil.FirstClassType.PrimitiveType)tuple6.Item1;
				switch (primitiveType9.Item.Tag)
				{
				case 5:
					if (tuple6.Item2.Tag == 0)
					{
						cil.FirstClassType.PrimitiveType primitiveType11 = (cil.FirstClassType.PrimitiveType)tuple6.Item2;
						switch (primitiveType11.Item.Tag)
						{
						case 6:
							fSharpFunc3.Invoke(operand5);
							il.Append(cil.MyInstruction.Conv_R8);
							fSharpFunc12.Invoke(null);
							return;
						case 5:
							fSharpFunc3.Invoke(operand5);
							fSharpFunc12.Invoke(null);
							return;
						}
					}
					break;
				case 6:
					if (tuple6.Item2.Tag == 0)
					{
						cil.FirstClassType.PrimitiveType primitiveType10 = (cil.FirstClassType.PrimitiveType)tuple6.Item2;
						if (primitiveType10.Item.Tag == 6)
						{
							fSharpFunc3.Invoke(operand5);
							fSharpFunc12.Invoke(null);
							return;
						}
					}
					break;
				}
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType4 = (cil.FirstClassType.VectorType)tuple6.Item1;
				if (vectorType4.Item.elemtype.Tag != 2)
				{
					break;
				}
				cil.VectorElementType.VecPrim vecPrim3 = (cil.VectorElementType.VecPrim)vectorType4.Item.elemtype;
				if (vecPrim3.Item.Tag != 4 || tuple6.Item2.Tag != 1)
				{
					break;
				}
				cil.FirstClassType.VectorType vectorType5 = (cil.FirstClassType.VectorType)tuple6.Item2;
				if (vectorType5.Item.elemtype.Tag == 2)
				{
					cil.VectorElementType.VecPrim vecPrim4 = (cil.VectorElementType.VecPrim)vectorType5.Item.elemtype;
					if (vecPrim4.Item.Tag == 5)
					{
						uint count4 = vectorType4.Item.count;
						uint count5 = vectorType5.Item.count;
						fSharpFunc3.Invoke(operand5);
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<uint, uint>>("v%P()f32_to_v%P()f64", new object[2] { count4, count5 }, null)))));
						fSharpFunc12.Invoke(null);
						return;
					}
				}
				break;
			}
			}
			FSharpFunc<LLVMValueRef, Unit> fSharpFunc22 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, Unit, string, Unit, LLVMValueRef>("fpext type not found %A"));
			LLVMValueRef func12 = op;
			fSharpFunc22.Invoke(func12);
			break;
		}
		case LLVMOpcode.LLVMFPTrunc:
		{
			LLVMValueRef operand4 = op.GetOperand(0u);
			cil.FirstClassType firstClassType7 = llvm_type_to_firstclass_type(operand4.TypeOf);
			cil.FirstClassType firstClassType8 = llvm_type_to_firstclass_type(op.TypeOf);
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple5 = new Tuple<cil.FirstClassType, cil.FirstClassType>(firstClassType7, firstClassType8);
			switch (tuple5.Item1.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType3 = (cil.FirstClassType.PrimitiveType)tuple5.Item1;
				if (primitiveType3.Item.Tag == 6 && tuple5.Item2.Tag == 0)
				{
					cil.FirstClassType.PrimitiveType primitiveType4 = (cil.FirstClassType.PrimitiveType)tuple5.Item2;
					switch (primitiveType4.Item.Tag)
					{
					case 6:
						fSharpFunc3.Invoke(operand4);
						fSharpFunc12.Invoke(null);
						return;
					case 5:
						fSharpFunc3.Invoke(operand4);
						il.Append(cil.MyInstruction.Conv_R4);
						fSharpFunc12.Invoke(null);
						return;
					}
				}
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType2 = (cil.FirstClassType.VectorType)tuple5.Item1;
				if (vectorType2.Item.elemtype.Tag != 2)
				{
					break;
				}
				cil.VectorElementType.VecPrim vecPrim = (cil.VectorElementType.VecPrim)vectorType2.Item.elemtype;
				if (vecPrim.Item.Tag != 5 || tuple5.Item2.Tag != 1)
				{
					break;
				}
				cil.FirstClassType.VectorType vectorType3 = (cil.FirstClassType.VectorType)tuple5.Item2;
				if (vectorType3.Item.elemtype.Tag == 2)
				{
					cil.VectorElementType.VecPrim vecPrim2 = (cil.VectorElementType.VecPrim)vectorType3.Item.elemtype;
					if (vecPrim2.Item.Tag == 4)
					{
						uint count2 = vectorType2.Item.count;
						uint count3 = vectorType3.Item.count;
						fSharpFunc3.Invoke(operand4);
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<uint, uint>>("v%P()f64_to_v%P()f32", new object[2] { count2, count3 }, null)))));
						fSharpFunc12.Invoke(null);
						return;
					}
				}
				break;
			}
			}
			FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>> clo2 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>>, Unit, string, Unit, Tuple<cil.FirstClassType, cil.FirstClassType>>("fptrunc type not found %A to %A"));
			FSharpFunc<cil.FirstClassType, cil.FirstClassType>.InvokeFast(new gen_instr@3951-4(clo2), firstClassType7, firstClassType8);
			break;
		}
		case LLVMOpcode.LLVMIntToPtr:
		{
			firstClassType45 = llvm_type_to_firstclass_type(op.TypeOf);
			cil.FirstClassType firstClassType46 = firstClassType45;
			if (firstClassType46.Tag == 1)
			{
				cil.FirstClassType.VectorType vectorType43 = (cil.FirstClassType.VectorType)firstClassType46;
				cil.VectorType item84 = vectorType43.Item;
				cil.FirstClassType firstClassType47 = llvm_type_to_firstclass_type(op.GetOperand(0u).TypeOf);
				cil.FirstClassType firstClassType48;
				if (firstClassType47.Tag == 1)
				{
					cil.FirstClassType.VectorType vectorType44 = (cil.FirstClassType.VectorType)firstClassType47;
					if (vectorType44.Item.elemtype.Tag == 2)
					{
						cil.VectorElementType.VecPrim vecPrim30 = (cil.VectorElementType.VecPrim)vectorType44.Item.elemtype;
						if (vecPrim30.Item.Tag == 3)
						{
							uint count36 = vectorType44.Item.count;
							goto IL_6980;
						}
						firstClassType48 = firstClassType47;
					}
					else
					{
						firstClassType48 = firstClassType47;
					}
				}
				else
				{
					firstClassType48 = firstClassType47;
				}
				FSharpFunc<cil.FirstClassType, Unit> fSharpFunc34 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, Unit>, Unit, string, Unit, cil.FirstClassType>("bad type for inttoptr: %A"));
				cil.FirstClassType func39 = firstClassType48;
				fSharpFunc34.Invoke(func39);
				goto IL_6980;
			}
			fSharpFunc3.Invoke(op.GetOperand(0u));
			il.Append(cil.MyInstruction.Conv_I);
			fSharpFunc12.Invoke(null);
			break;
		}
		case LLVMOpcode.LLVMPtrToInt:
		{
			cil.FirstClassType firstClassType40 = llvm_type_to_firstclass_type(op.GetOperand(0u).TypeOf);
			cil.FirstClassType firstClassType41 = llvm_type_to_firstclass_type(op.TypeOf);
			cil.FirstClassType firstClassType42 = firstClassType40;
			cil.FirstClassType firstClassType43;
			switch (firstClassType42.Tag)
			{
			case 1:
			{
				cil.FirstClassType.VectorType vectorType41 = (cil.FirstClassType.VectorType)firstClassType42;
				if (vectorType41.Item.elemtype.Tag != 1)
				{
					firstClassType43 = firstClassType42;
					break;
				}
				uint count34 = vectorType41.Item.count;
				fSharpFunc3.Invoke(op.GetOperand(0u));
				il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<uint, uint>>("v%P()ptr_to_v%P()i64", new object[2] { count34, count34 }, null)))));
				fSharpFunc12.Invoke(null);
				return;
			}
			case 5:
				fSharpFunc3.Invoke(op.GetOperand(0u));
				il.Append(cil.MyInstruction.Conv_I8);
				fSharpFunc12.Invoke(null);
				return;
			default:
				firstClassType43 = firstClassType42;
				break;
			}
			FSharpFunc<cil.FirstClassType, Unit> fSharpFunc31 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, Unit>, Unit, string, Unit, cil.FirstClassType>("match case %A"));
			cil.FirstClassType func35 = firstClassType43;
			fSharpFunc31.Invoke(func35);
			break;
		}
		case LLVMOpcode.LLVMBitCast:
		{
			cil.FirstClassType item79 = llvm_type_to_firstclass_type(op.GetOperand(0u).TypeOf);
			cil.FirstClassType firstClassType39 = llvm_type_to_firstclass_type(op.TypeOf);
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple18 = new Tuple<cil.FirstClassType, cil.FirstClassType>(item79, firstClassType39);
			ResultLocal func32;
			AddressOfValue addressOfValue10;
			FSharpFunc<Unit, Unit> f_load_src7;
			int siz7;
			switch (tuple18.Item1.Tag)
			{
			default:
				if (tuple18.Item2.Tag == 1)
				{
					cil.FirstClassType.VectorType vectorType40 = (cil.FirstClassType.VectorType)tuple18.Item2;
					goto IL_6d83;
				}
				break;
			case 5:
			{
				switch (tuple18.Item2.Tag)
				{
				case 1:
					break;
				case 5:
				{
					LLVMValueRef operand16 = op.GetOperand(0u);
					fSharpFunc3.Invoke(operand16);
					fSharpFunc12.Invoke(null);
					return;
				}
				default:
					goto end_IL_6bc5;
				}
				cil.FirstClassType.VectorType vectorType38 = (cil.FirstClassType.VectorType)tuple18.Item2;
				goto IL_6d83;
			}
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType49 = (cil.FirstClassType.PrimitiveType)tuple18.Item1;
				switch (tuple18.Item2.Tag)
				{
				case 0:
				{
					cil.FirstClassType.PrimitiveType primitiveType50 = (cil.FirstClassType.PrimitiveType)tuple18.Item2;
					cil.PrimitiveType item80 = primitiveType49.Item;
					cil.PrimitiveType item81 = primitiveType50.Item;
					ResultLocal func33 = fSharpFunc5.Invoke(null);
					fSharpFunc6.Invoke(func33);
					fSharpFunc3.Invoke(op.GetOperand(0u));
					store_mem_primitive_from_stack(il, item80);
					fSharpFunc9.Invoke(func33);
					return;
				}
				case 1:
					break;
				default:
					goto end_IL_6bc5;
				}
				cil.FirstClassType.VectorType vectorType39 = (cil.FirstClassType.VectorType)tuple18.Item2;
				goto IL_6d83;
			}
			case 1:
				{
					cil.FirstClassType.VectorType vectorType36 = (cil.FirstClassType.VectorType)tuple18.Item1;
					if (tuple18.Item2.Tag == 1)
					{
						cil.FirstClassType.VectorType vectorType37 = (cil.FirstClassType.VectorType)tuple18.Item2;
						goto IL_6d83;
					}
					ResultLocal func31 = fSharpFunc5.Invoke(null);
					AddressOfValue addressOfValue9 = fSharpFunc4.Invoke(op.GetOperand(0u));
					FSharpFunc<Unit, Unit> f_load_src6 = new f@4045-6(il, addressOfValue9);
					int siz6 = cil.get_sizeof(firstClassType39);
					cpblk_into(il, fSharpFunc8.Invoke(func31), f_load_src6, siz6);
					release_address_of_value(il, addressOfValue9);
					fSharpFunc9.Invoke(func31);
					return;
				}
				IL_6d83:
				func32 = fSharpFunc5.Invoke(null);
				addressOfValue10 = fSharpFunc4.Invoke(op.GetOperand(0u));
				f_load_src7 = new f@4035-5(il, addressOfValue10);
				siz7 = cil.get_sizeof(firstClassType39);
				cpblk_into(il, fSharpFunc8.Invoke(func32), f_load_src7, siz7);
				release_address_of_value(il, addressOfValue10);
				fSharpFunc9.Invoke(func32);
				return;
				end_IL_6bc5:
				break;
			}
			FSharpFunc<LLVMValueRef, Unit> fSharpFunc30 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, Unit, string, Unit, LLVMValueRef>("bitcast type not found %A"));
			LLVMValueRef func34 = op;
			fSharpFunc30.Invoke(func34);
			break;
		}
		case LLVMOpcode.LLVMFPToSI:
		{
			LLVMValueRef operand15 = op.GetOperand(0u);
			cil.FirstClassType item78 = llvm_type_to_firstclass_type(operand15.TypeOf);
			cil.FirstClassType firstClassType36 = llvm_type_to_firstclass_type(op.TypeOf);
			fSharpFunc3.Invoke(operand15);
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple17 = new Tuple<cil.FirstClassType, cil.FirstClassType>(item78, firstClassType36);
			FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>> clo5;
			switch (tuple17.Item1.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType46 = (cil.FirstClassType.PrimitiveType)tuple17.Item1;
				switch (primitiveType46.Item.Tag)
				{
				case 5:
					break;
				case 6:
					goto IL_6fad;
				default:
					goto IL_7173;
				}
				if (tuple17.Item2.Tag == 0)
				{
					cil.FirstClassType.PrimitiveType primitiveType47 = (cil.FirstClassType.PrimitiveType)tuple17.Item2;
					switch (primitiveType47.Item.Tag)
					{
					case 2:
						break;
					case 3:
						goto IL_70db;
					case 4:
						goto IL_70ed;
					default:
						goto IL_7173;
					}
					goto IL_70c9;
				}
				goto IL_7173;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType33 = (cil.FirstClassType.VectorType)tuple17.Item1;
				if (vectorType33.Item.elemtype.Tag == 2)
				{
					cil.VectorElementType.VecPrim vecPrim27 = (cil.VectorElementType.VecPrim)vectorType33.Item.elemtype;
					if (vecPrim27.Item.Tag == 4 && tuple17.Item2.Tag == 1)
					{
						cil.FirstClassType.VectorType vectorType34 = (cil.FirstClassType.VectorType)tuple17.Item2;
						if (vectorType34.Item.elemtype.Tag == 2)
						{
							cil.VectorElementType.VecPrim vecPrim28 = (cil.VectorElementType.VecPrim)vectorType34.Item.elemtype;
							if (vecPrim28.Item.Tag == 2)
							{
								uint count32 = vectorType34.Item.count;
								uint count33 = vectorType33.Item.count;
								il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<uint, uint>>("v%P()f32_to_v%P()i32", new object[2] { count33, count32 }, null)))));
								break;
							}
						}
					}
				}
				goto IL_7173;
			}
			default:
				goto IL_7173;
				IL_6fad:
				if (tuple17.Item2.Tag == 0)
				{
					cil.FirstClassType.PrimitiveType primitiveType45 = (cil.FirstClassType.PrimitiveType)tuple17.Item2;
					switch (primitiveType45.Item.Tag)
					{
					case 2:
						break;
					case 3:
						goto IL_70db;
					case 4:
						goto IL_70ed;
					default:
						goto IL_7173;
					}
					goto IL_70c9;
				}
				goto IL_7173;
				IL_7173:
				clo5 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, FSharpFunc<LLVMValueRef, Unit>>, Unit, string, Unit, Tuple<cil.FirstClassType, LLVMValueRef>>("fptosi type not found %A : %A"));
				FSharpFunc<cil.FirstClassType, LLVMValueRef>.InvokeFast(new gen_instr@4090-6(clo5), firstClassType36, op);
				break;
				IL_70db:
				il.Append(cil.MyInstruction.Conv_I4);
				break;
				IL_70c9:
				il.Append(cil.MyInstruction.Conv_I2);
				break;
				IL_70ed:
				il.Append(cil.MyInstruction.Conv_I8);
				break;
			}
			fSharpFunc12.Invoke(null);
			break;
		}
		case LLVMOpcode.LLVMFPToUI:
		{
			LLVMValueRef operand14 = op.GetOperand(0u);
			cil.FirstClassType firstClassType29 = llvm_type_to_firstclass_type(operand14.TypeOf);
			firstClassType30 = llvm_type_to_firstclass_type(op.TypeOf);
			fSharpFunc3.Invoke(operand14);
			cil.FirstClassType firstClassType31 = firstClassType30;
			if (firstClassType31.Tag != 0)
			{
				goto IL_7279;
			}
			cil.FirstClassType.PrimitiveType primitiveType42 = (cil.FirstClassType.PrimitiveType)firstClassType31;
			switch (primitiveType42.Item.Tag)
			{
			case 1:
				break;
			case 2:
				goto IL_724c;
			case 3:
				goto IL_725b;
			case 4:
				goto IL_726a;
			default:
				goto IL_7279;
			}
			il.Append(cil.MyInstruction.Conv_U1);
			goto IL_72a3;
		}
		case LLVMOpcode.LLVMSIToFP:
		{
			LLVMValueRef operand13 = op.GetOperand(0u);
			cil.FirstClassType item68 = llvm_type_to_firstclass_type(operand13.TypeOf);
			cil.FirstClassType item69 = llvm_type_to_firstclass_type(op.TypeOf);
			fSharpFunc3.Invoke(operand13);
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple14 = new Tuple<cil.FirstClassType, cil.FirstClassType>(item68, item69);
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple15;
			cil.FirstClassType.VectorType vectorType26;
			cil.VecPrimitiveType vp2;
			cil.VecPrimitiveType vp3;
			uint num10;
			uint count29;
			string text5;
			string text6;
			FSharpFunc<Tuple<cil.FirstClassType, cil.FirstClassType>, Unit> fSharpFunc28;
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple16;
			cil.FirstClassType item73;
			cil.FirstClassType item74;
			switch (tuple14.Item1.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType36 = (cil.FirstClassType.PrimitiveType)tuple14.Item1;
				switch (primitiveType36.Item.Tag)
				{
				case 1:
					goto IL_7376;
				case 2:
					goto IL_73ec;
				case 3:
					goto IL_7458;
				case 4:
					goto IL_74c4;
				}
				tuple15 = tuple14;
				goto IL_78a8;
			}
			case 1:
				vectorType26 = (cil.FirstClassType.VectorType)tuple14.Item1;
				switch (vectorType26.Item.count)
				{
				case 2u:
					goto IL_7607;
				}
				if (vectorType26.Item.elemtype.Tag == 2)
				{
					cil.VectorElementType.VecPrim vecPrim22 = (cil.VectorElementType.VecPrim)vectorType26.Item.elemtype;
					if (tuple14.Item2.Tag == 1)
					{
						cil.FirstClassType.VectorType vectorType27 = (cil.FirstClassType.VectorType)tuple14.Item2;
						if (vectorType27.Item.elemtype.Tag == 2)
						{
							cil.VectorElementType.VecPrim vecPrim23 = (cil.VectorElementType.VecPrim)vectorType27.Item.elemtype;
							vp2 = vecPrim22.Item;
							vp3 = vecPrim23.Item;
							num10 = vectorType26.Item.count;
							count29 = vectorType27.Item.count;
							goto IL_7816;
						}
						tuple15 = tuple14;
					}
					else
					{
						tuple15 = tuple14;
					}
				}
				else
				{
					tuple15 = tuple14;
				}
				goto IL_78a8;
			default:
				{
					tuple15 = tuple14;
					goto IL_78a8;
				}
				IL_7607:
				if (vectorType26.Item.elemtype.Tag == 2)
				{
					cil.VectorElementType.VecPrim vecPrim24 = (cil.VectorElementType.VecPrim)vectorType26.Item.elemtype;
					if (vecPrim24.Item.Tag == 1)
					{
						if (tuple14.Item2.Tag == 1)
						{
							cil.FirstClassType.VectorType vectorType28 = (cil.FirstClassType.VectorType)tuple14.Item2;
							if (vectorType28.Item.elemtype.Tag == 2)
							{
								cil.VectorElementType.VecPrim vecPrim25 = (cil.VectorElementType.VecPrim)vectorType28.Item.elemtype;
								cil.VecPrimitiveType item70 = vecPrim25.Item;
								uint count30 = vectorType28.Item.count;
								LLVMValueRef func29 = op;
								((FSharpFunc<LLVMValueRef, Unit>)fSharpTypeFunc.Specialize<LLVMValueRef>()).Invoke(func29);
								break;
							}
							tuple15 = tuple14;
						}
						else
						{
							tuple15 = tuple14;
						}
					}
					else if (tuple14.Item2.Tag == 1)
					{
						cil.FirstClassType.VectorType vectorType29 = (cil.FirstClassType.VectorType)tuple14.Item2;
						if (vectorType29.Item.elemtype.Tag == 2)
						{
							cil.VectorElementType.VecPrim vecPrim26 = (cil.VectorElementType.VecPrim)vectorType29.Item.elemtype;
							cil.VecPrimitiveType item71 = vecPrim24.Item;
							cil.VecPrimitiveType item72 = vecPrim26.Item;
							uint count31 = vectorType26.Item.count;
							count29 = vectorType29.Item.count;
							num10 = count31;
							vp3 = item72;
							vp2 = item71;
							goto IL_7816;
						}
						tuple15 = tuple14;
					}
					else
					{
						tuple15 = tuple14;
					}
				}
				else
				{
					tuple15 = tuple14;
				}
				goto IL_78a8;
				IL_7816:
				text5 = vecprim_to_prim_name(vp2);
				text6 = vecprim_to_prim_name(vp3);
				il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<uint, string, uint, string>>("v%P()%P()_to_v%P()%P()", new object[4] { num10, text5, count29, text6 }, null)))));
				break;
				IL_7458:
				if (tuple14.Item2.Tag == 0)
				{
					cil.FirstClassType.PrimitiveType primitiveType35 = (cil.FirstClassType.PrimitiveType)tuple14.Item2;
					switch (primitiveType35.Item.Tag)
					{
					case 5:
						goto IL_7774;
					case 6:
						goto IL_7786;
					}
					tuple15 = tuple14;
				}
				else
				{
					tuple15 = tuple14;
				}
				goto IL_78a8;
				IL_73ec:
				if (tuple14.Item2.Tag == 0)
				{
					cil.FirstClassType.PrimitiveType primitiveType37 = (cil.FirstClassType.PrimitiveType)tuple14.Item2;
					switch (primitiveType37.Item.Tag)
					{
					case 5:
						goto IL_7774;
					case 6:
						goto IL_7786;
					}
					tuple15 = tuple14;
				}
				else
				{
					tuple15 = tuple14;
				}
				goto IL_78a8;
				IL_78a8:
				fSharpFunc28 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<Tuple<cil.FirstClassType, cil.FirstClassType>, Unit>, Unit, string, Unit, Tuple<cil.FirstClassType, cil.FirstClassType>>("sitofp type not found %A"));
				tuple16 = tuple15;
				item73 = tuple16.Item1;
				item74 = tuple16.Item2;
				fSharpFunc28.Invoke(new Tuple<cil.FirstClassType, cil.FirstClassType>(item73, item74));
				break;
				IL_74c4:
				if (tuple14.Item2.Tag == 0)
				{
					cil.FirstClassType.PrimitiveType primitiveType38 = (cil.FirstClassType.PrimitiveType)tuple14.Item2;
					switch (primitiveType38.Item.Tag)
					{
					case 5:
						goto IL_7774;
					case 6:
						goto IL_7786;
					}
					tuple15 = tuple14;
				}
				else
				{
					tuple15 = tuple14;
				}
				goto IL_78a8;
				IL_7376:
				if (tuple14.Item2.Tag == 0)
				{
					cil.FirstClassType.PrimitiveType primitiveType39 = (cil.FirstClassType.PrimitiveType)tuple14.Item2;
					switch (primitiveType39.Item.Tag)
					{
					case 5:
						goto IL_7774;
					case 6:
						goto IL_7786;
					}
					tuple15 = tuple14;
				}
				else
				{
					tuple15 = tuple14;
				}
				goto IL_78a8;
				IL_7786:
				il.Append(cil.MyInstruction.Conv_R8);
				break;
				IL_7774:
				il.Append(cil.MyInstruction.Conv_R4);
				break;
			}
			fSharpFunc12.Invoke(null);
			break;
		}
		case LLVMOpcode.LLVMUIToFP:
		{
			LLVMValueRef operand12 = op.GetOperand(0u);
			cil.FirstClassType firstClassType22 = llvm_type_to_firstclass_type(operand12.TypeOf);
			cil.FirstClassType firstClassType23 = llvm_type_to_firstclass_type(op.TypeOf);
			FSharpFunc<cil.PrimitiveType, Unit> fSharpFunc27 = new conv_one@4164(il);
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple13 = new Tuple<cil.FirstClassType, cil.FirstClassType>(firstClassType22, firstClassType23);
			cil.FirstClassType.VectorType vectorType19;
			cil.VecPrimitiveType t2;
			cil.VecPrimitiveType vp;
			uint num8;
			uint count24;
			int num9;
			string text4;
			switch (tuple13.Item1.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType32 = (cil.FirstClassType.PrimitiveType)tuple13.Item1;
				switch (primitiveType32.Item.Tag)
				{
				case 1:
					goto IL_79d2;
				case 2:
					goto IL_79eb;
				}
				if (tuple13.Item2.Tag == 0)
				{
					cil.PrimitiveType item62 = ((cil.FirstClassType.PrimitiveType)tuple13.Item2).Item;
					fSharpFunc3.Invoke(operand12);
					fSharpFunc27.Invoke(item62);
					break;
				}
				goto default;
			}
			case 4:
			{
				cil.FirstClassType.FunnyIntegerType funnyIntegerType17 = (cil.FirstClassType.FunnyIntegerType)tuple13.Item1;
				if (tuple13.Item2.Tag == 0)
				{
					cil.FirstClassType.PrimitiveType primitiveType33 = (cil.FirstClassType.PrimitiveType)tuple13.Item2;
					cil.PrimitiveType item63 = primitiveType33.Item;
					cil.FunnyIntegerType item64 = funnyIntegerType17.Item;
					AddressOfValue addressOfValue8 = fSharpFunc4.Invoke(operand12);
					if (item64.bits < 64)
					{
						cil.Variable variable2 = il.NewVariable(cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.I64));
						init_local(il, variable2);
						FSharpFunc<Unit, Unit> f_load_src5 = new f@4198-7(il, addressOfValue8);
						int siz5 = cil.get_sizeof(firstClassType22);
						cpblk_into(il, variable2, f_load_src5, siz5);
						il.Append(cil.MyInstruction.NewLdloc(variable2));
					}
					else
					{
						emit_address_of_value(il, addressOfValue8);
						fSharpFunc11.Invoke(cil.PrimitiveType.I64);
					}
					fSharpFunc27.Invoke(item63);
					release_address_of_value(il, addressOfValue8);
					break;
				}
				goto default;
			}
			case 1:
				vectorType19 = (cil.FirstClassType.VectorType)tuple13.Item1;
				switch (vectorType19.Item.count)
				{
				case 2u:
					goto IL_7c45;
				}
				switch (vectorType19.Item.count)
				{
				case 4u:
					goto IL_7b10;
				}
				if (vectorType19.Item.elemtype.Tag == 2)
				{
					cil.VectorElementType.VecPrim vecPrim14 = (cil.VectorElementType.VecPrim)vectorType19.Item.elemtype;
					if (tuple13.Item2.Tag == 1)
					{
						cil.FirstClassType.VectorType vectorType20 = (cil.FirstClassType.VectorType)tuple13.Item2;
						if (vectorType20.Item.elemtype.Tag == 2)
						{
							cil.VectorElementType.VecPrim vecPrim15 = (cil.VectorElementType.VecPrim)vectorType20.Item.elemtype;
							t2 = vecPrim14.Item;
							vp = vecPrim15.Item;
							num8 = vectorType19.Item.count;
							count24 = vectorType20.Item.count;
							goto IL_8065;
						}
					}
				}
				goto default;
			default:
				{
					FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>> clo4 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>>, TextWriter, Unit, Unit, Tuple<cil.FirstClassType, cil.FirstClassType>>("uitofp type not found %A to %A"));
					FSharpFunc<cil.FirstClassType, cil.FirstClassType>.InvokeFast(new gen_instr@4223-8(clo4), firstClassType22, firstClassType23);
					LLVMValueRef func28 = op;
					((FSharpFunc<LLVMValueRef, Unit>)fSharpTypeFunc.Specialize<LLVMValueRef>()).Invoke(func28);
					break;
				}
				IL_7c45:
				if (vectorType19.Item.elemtype.Tag == 2)
				{
					cil.VectorElementType.VecPrim vecPrim16 = (cil.VectorElementType.VecPrim)vectorType19.Item.elemtype;
					if (vecPrim16.Item.Tag == 0)
					{
						if (tuple13.Item2.Tag == 1)
						{
							cil.FirstClassType.VectorType vectorType21 = (cil.FirstClassType.VectorType)tuple13.Item2;
							if (vectorType21.Item.elemtype.Tag == 2)
							{
								cil.VectorElementType.VecPrim vecPrim17 = (cil.VectorElementType.VecPrim)vectorType21.Item.elemtype;
								cil.VecPrimitiveType item56 = vecPrim17.Item;
								uint count25 = vectorType21.Item.count;
								LLVMValueRef func27 = op;
								((FSharpFunc<LLVMValueRef, Unit>)fSharpTypeFunc.Specialize<LLVMValueRef>()).Invoke(func27);
								break;
							}
						}
					}
					else if (tuple13.Item2.Tag == 1)
					{
						cil.FirstClassType.VectorType vectorType22 = (cil.FirstClassType.VectorType)tuple13.Item2;
						if (vectorType22.Item.elemtype.Tag == 2)
						{
							cil.VectorElementType.VecPrim vecPrim18 = (cil.VectorElementType.VecPrim)vectorType22.Item.elemtype;
							cil.VecPrimitiveType item57 = vecPrim16.Item;
							cil.VecPrimitiveType item58 = vecPrim18.Item;
							uint count26 = vectorType19.Item.count;
							count24 = vectorType22.Item.count;
							num8 = count26;
							vp = item58;
							t2 = item57;
							goto IL_8065;
						}
					}
				}
				goto default;
				IL_7b10:
				if (vectorType19.Item.elemtype.Tag == 2)
				{
					cil.VectorElementType.VecPrim vecPrim19 = (cil.VectorElementType.VecPrim)vectorType19.Item.elemtype;
					if (vecPrim19.Item.Tag == 0)
					{
						if (tuple13.Item2.Tag == 1)
						{
							cil.FirstClassType.VectorType vectorType23 = (cil.FirstClassType.VectorType)tuple13.Item2;
							if (vectorType23.Item.elemtype.Tag == 2)
							{
								cil.VectorElementType.VecPrim vecPrim20 = (cil.VectorElementType.VecPrim)vectorType23.Item.elemtype;
								cil.VecPrimitiveType item59 = vecPrim20.Item;
								uint count27 = vectorType23.Item.count;
								AddressOfValue addr9 = fSharpFunc4.Invoke(operand12);
								emit_address_of_value(il, addr9);
								string text3 = vecprim_to_prim_name(item59);
								il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<uint, string>>("v4u8_to_v%P()%P()", new object[2] { count27, text3 }, null)))));
								release_address_of_value(il, addr9);
								break;
							}
						}
					}
					else if (tuple13.Item2.Tag == 1)
					{
						cil.FirstClassType.VectorType vectorType24 = (cil.FirstClassType.VectorType)tuple13.Item2;
						if (vectorType24.Item.elemtype.Tag == 2)
						{
							cil.VectorElementType.VecPrim vecPrim21 = (cil.VectorElementType.VecPrim)vectorType24.Item.elemtype;
							cil.VecPrimitiveType item60 = vecPrim19.Item;
							cil.VecPrimitiveType item61 = vecPrim21.Item;
							uint count28 = vectorType19.Item.count;
							count24 = vectorType24.Item.count;
							num8 = count28;
							vp = item61;
							t2 = item60;
							goto IL_8065;
						}
					}
				}
				goto default;
				IL_8065:
				fSharpFunc3.Invoke(operand12);
				num9 = cil.sizeof_primitive(cil.vecprim_to_prim(t2)) * 8;
				text4 = vecprim_to_prim_name(vp);
				il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, Tuple<uint, int, uint, string>>("v%P()u%P()_to_v%P()%P()", new object[4] { num8, num9, count24, text4 }, null)))));
				break;
				IL_79d2:
				if (tuple13.Item2.Tag == 0)
				{
					cil.PrimitiveType item65 = ((cil.FirstClassType.PrimitiveType)tuple13.Item2).Item;
					fSharpFunc3.Invoke(operand12);
					il.Append(cil.MyInstruction.Conv_U4);
					il.Append(cil.MyInstruction.NewLdc_I4(255));
					il.Append(cil.MyInstruction.And);
					fSharpFunc27.Invoke(item65);
					break;
				}
				goto default;
				IL_79eb:
				if (tuple13.Item2.Tag == 0)
				{
					cil.PrimitiveType item66 = ((cil.FirstClassType.PrimitiveType)tuple13.Item2).Item;
					fSharpFunc3.Invoke(operand12);
					il.Append(cil.MyInstruction.Conv_U4);
					il.Append(cil.MyInstruction.NewLdc_I4(65535));
					il.Append(cil.MyInstruction.And);
					fSharpFunc27.Invoke(item66);
					break;
				}
				goto default;
			}
			fSharpFunc12.Invoke(null);
			break;
		}
		case LLVMOpcode.LLVMSExt:
		{
			LLVMValueRef operand11 = op.GetOperand(0u);
			cil.FirstClassType firstClassType18 = llvm_type_to_firstclass_type(operand11.TypeOf);
			cil.FirstClassType firstClassType19 = llvm_type_to_firstclass_type(op.TypeOf);
			FSharpFunc<cil.VecPrimitiveType, FSharpFunc<uint, Unit>> func25 = new sext_bit_vector@4234(il, op, fSharpFunc4, fSharpFunc5, fSharpFunc8, fSharpFunc9);
			FSharpFunc<cil.VecPrimitiveType, FSharpFunc<cil.VecPrimitiveType, FSharpFunc<uint, Unit>>> func26 = new sext_vector@4264(il, op, fSharpFunc4, fSharpFunc5, fSharpFunc8, fSharpFunc9, load_vecprim_to_stack);
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple12 = new Tuple<cil.FirstClassType, cil.FirstClassType>(firstClassType18, firstClassType19);
			switch (tuple12.Item1.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType27 = (cil.FirstClassType.PrimitiveType)tuple12.Item1;
				switch (primitiveType27.Item.Tag)
				{
				case 0:
					if (tuple12.Item2.Tag == 0)
					{
						cil.PrimitiveType item52 = ((cil.FirstClassType.PrimitiveType)tuple12.Item2).Item;
						fSharpFunc3.Invoke(operand11);
						il.Append(cil.MyInstruction.NewLdc_I4(1));
						il.Append(cil.MyInstruction.And);
						cil.Label item53 = il.NewLabel();
						cil.Label item54 = il.NewLabel();
						il.Append(cil.MyInstruction.NewBrfalse(item53));
						FSharpFunc<long, Unit> fSharpFunc26 = new load_result@4296(il, item52);
						fSharpFunc26.Invoke(-1L);
						il.Append(cil.MyInstruction.NewBr(item54));
						il.Append(cil.MyInstruction.NewLabel(item53));
						fSharpFunc26.Invoke(0L);
						il.Append(cil.MyInstruction.NewLabel(item54));
						fSharpFunc12.Invoke(null);
						return;
					}
					break;
				case 1:
					if (tuple12.Item2.Tag == 0)
					{
						cil.FirstClassType.PrimitiveType primitiveType29 = (cil.FirstClassType.PrimitiveType)tuple12.Item2;
						switch (primitiveType29.Item.Tag)
						{
						case 3:
							fSharpFunc3.Invoke(operand11);
							fSharpFunc12.Invoke(null);
							return;
						case 2:
							fSharpFunc3.Invoke(operand11);
							fSharpFunc12.Invoke(null);
							return;
						case 4:
							fSharpFunc3.Invoke(operand11);
							il.Append(cil.MyInstruction.Conv_I8);
							fSharpFunc12.Invoke(null);
							return;
						}
					}
					break;
				case 2:
					if (tuple12.Item2.Tag == 0)
					{
						cil.FirstClassType.PrimitiveType primitiveType30 = (cil.FirstClassType.PrimitiveType)tuple12.Item2;
						switch (primitiveType30.Item.Tag)
						{
						case 3:
							fSharpFunc3.Invoke(operand11);
							fSharpFunc12.Invoke(null);
							return;
						case 4:
							fSharpFunc3.Invoke(operand11);
							il.Append(cil.MyInstruction.Conv_I8);
							fSharpFunc12.Invoke(null);
							return;
						}
					}
					break;
				case 3:
					if (tuple12.Item2.Tag == 0)
					{
						cil.FirstClassType.PrimitiveType primitiveType28 = (cil.FirstClassType.PrimitiveType)tuple12.Item2;
						if (primitiveType28.Item.Tag == 4)
						{
							fSharpFunc3.Invoke(operand11);
							il.Append(cil.MyInstruction.Conv_I8);
							fSharpFunc12.Invoke(null);
							return;
						}
					}
					break;
				}
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType15 = (cil.FirstClassType.VectorType)tuple12.Item1;
				switch (vectorType15.Item.elemtype.Tag)
				{
				case 2:
				{
					cil.VectorElementType.VecPrim vecPrim12 = (cil.VectorElementType.VecPrim)vectorType15.Item.elemtype;
					if (tuple12.Item2.Tag == 1)
					{
						cil.FirstClassType.VectorType vectorType17 = (cil.FirstClassType.VectorType)tuple12.Item2;
						if (vectorType17.Item.elemtype.Tag == 2)
						{
							cil.VectorElementType.VecPrim vecPrim13 = (cil.VectorElementType.VecPrim)vectorType17.Item.elemtype;
							cil.VecPrimitiveType item50 = vecPrim12.Item;
							cil.VecPrimitiveType item51 = vecPrim13.Item;
							uint count22 = vectorType15.Item.count;
							uint count23 = vectorType17.Item.count;
							FSharpFunc<cil.VecPrimitiveType, cil.VecPrimitiveType>.InvokeFast(func26, item50, item51, count23);
							return;
						}
					}
					break;
				}
				case 0:
					if (tuple12.Item2.Tag == 1)
					{
						cil.FirstClassType.VectorType vectorType16 = (cil.FirstClassType.VectorType)tuple12.Item2;
						if (vectorType16.Item.elemtype.Tag == 2)
						{
							cil.VectorElementType.VecPrim vecPrim11 = (cil.VectorElementType.VecPrim)vectorType16.Item.elemtype;
							cil.VecPrimitiveType item49 = vecPrim11.Item;
							uint count20 = vectorType15.Item.count;
							uint count21 = vectorType16.Item.count;
							FSharpFunc<cil.VecPrimitiveType, uint>.InvokeFast(func25, item49, count20);
							return;
						}
					}
					break;
				}
				break;
			}
			}
			FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>> clo3 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, FSharpFunc<cil.FirstClassType, Unit>>, Unit, string, Unit, Tuple<cil.FirstClassType, cil.FirstClassType>>("sext not found %A -> %A"));
			FSharpFunc<cil.FirstClassType, cil.FirstClassType>.InvokeFast(new gen_instr@4339-10(clo3), firstClassType18, firstClassType19);
			break;
		}
		case LLVMOpcode.LLVMZExt:
		{
			LLVMValueRef operand10 = op.GetOperand(0u);
			cil.FirstClassType firstClassType14 = llvm_type_to_firstclass_type(operand10.TypeOf);
			cil.FirstClassType firstClassType15 = llvm_type_to_firstclass_type(op.TypeOf);
			FSharpFunc<cil.VecPrimitiveType, FSharpFunc<uint, Unit>> func19 = new zext_bit_vector@4349(il, op, fSharpFunc4, fSharpFunc5, fSharpFunc8, fSharpFunc9);
			FSharpFunc<cil.VecPrimitiveType, FSharpFunc<cil.VecPrimitiveType, FSharpFunc<uint, Unit>>> func20 = new zext_vector@4383(il, op, fSharpFunc4, fSharpFunc5, fSharpFunc8, fSharpFunc9);
			FSharpFunc<cil.VecPrimitiveType, FSharpFunc<int, FSharpFunc<uint, Unit>>> func21 = new zext_vector_2@4427(il, op, fSharpFunc4, fSharpFunc5, fSharpFunc6, fSharpFunc7, fSharpFunc9);
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple11 = new Tuple<cil.FirstClassType, cil.FirstClassType>(firstClassType14, firstClassType15);
			switch (tuple11.Item1.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType20 = (cil.FirstClassType.PrimitiveType)tuple11.Item1;
				cil.PrimitiveType pt;
				cil.FirstClassType.FunnyIntegerType funnyIntegerType12;
				cil.PrimitiveType item44;
				cil.FirstClassType.FunnyIntegerType funnyIntegerType13;
				cil.PrimitiveType item45;
				ResultLocal func23;
				cil.FirstClassType.FunnyIntegerType funnyIntegerType14;
				cil.PrimitiveType item46;
				cil.FunnyIntegerType item42;
				switch (primitiveType20.Item.Tag)
				{
				default:
				{
					if (tuple11.Item2.Tag != 4)
					{
						break;
					}
					cil.FirstClassType.FunnyIntegerType funnyIntegerType15 = (cil.FirstClassType.FunnyIntegerType)tuple11.Item2;
					pt = primitiveType20.Item;
					item42 = funnyIntegerType15.Item;
					goto IL_8f92;
				}
				case 0:
				{
					switch (tuple11.Item2.Tag)
					{
					case 0:
						break;
					case 4:
						goto IL_8827;
					default:
						goto end_IL_875b;
					}
					cil.FirstClassType.PrimitiveType primitiveType22 = (cil.FirstClassType.PrimitiveType)tuple11.Item2;
					switch (primitiveType22.Item.Tag)
					{
					case 1:
					case 2:
					case 3:
						fSharpFunc3.Invoke(operand10);
						il.Append(cil.MyInstruction.NewLdc_I4(1));
						il.Append(cil.MyInstruction.And);
						fSharpFunc12.Invoke(null);
						return;
					case 4:
						fSharpFunc3.Invoke(operand10);
						il.Append(cil.MyInstruction.NewLdc_I4(1));
						il.Append(cil.MyInstruction.And);
						il.Append(cil.MyInstruction.Conv_U8);
						fSharpFunc12.Invoke(null);
						return;
					}
					break;
				}
				case 1:
				{
					switch (tuple11.Item2.Tag)
					{
					case 0:
						break;
					case 4:
						goto IL_88dd;
					default:
						goto end_IL_875b;
					}
					cil.FirstClassType.PrimitiveType primitiveType25 = (cil.FirstClassType.PrimitiveType)tuple11.Item2;
					switch (primitiveType25.Item.Tag)
					{
					case 2:
						fSharpFunc3.Invoke(operand10);
						il.Append(cil.MyInstruction.Conv_U4);
						il.Append(cil.MyInstruction.NewLdc_I8(255L));
						il.Append(cil.MyInstruction.And);
						fSharpFunc12.Invoke(null);
						return;
					case 3:
						fSharpFunc3.Invoke(operand10);
						il.Append(cil.MyInstruction.Conv_U4);
						il.Append(cil.MyInstruction.NewLdc_I4(255));
						il.Append(cil.MyInstruction.And);
						fSharpFunc12.Invoke(null);
						return;
					case 4:
						fSharpFunc3.Invoke(operand10);
						il.Append(cil.MyInstruction.Conv_U8);
						il.Append(cil.MyInstruction.NewLdc_I8(255L));
						il.Append(cil.MyInstruction.And);
						fSharpFunc12.Invoke(null);
						return;
					}
					break;
				}
				case 2:
				{
					switch (tuple11.Item2.Tag)
					{
					case 0:
						break;
					case 4:
						goto IL_898e;
					default:
						goto end_IL_875b;
					}
					cil.FirstClassType.PrimitiveType primitiveType24 = (cil.FirstClassType.PrimitiveType)tuple11.Item2;
					switch (primitiveType24.Item.Tag)
					{
					case 3:
						fSharpFunc3.Invoke(operand10);
						il.Append(cil.MyInstruction.Conv_U4);
						il.Append(cil.MyInstruction.NewLdc_I4(65535));
						il.Append(cil.MyInstruction.And);
						fSharpFunc12.Invoke(null);
						return;
					case 4:
						fSharpFunc3.Invoke(operand10);
						il.Append(cil.MyInstruction.Conv_U8);
						il.Append(cil.MyInstruction.NewLdc_I8(65535L));
						il.Append(cil.MyInstruction.And);
						fSharpFunc12.Invoke(null);
						return;
					}
					break;
				}
				case 3:
				{
					switch (tuple11.Item2.Tag)
					{
					case 0:
					{
						cil.FirstClassType.PrimitiveType primitiveType23 = (cil.FirstClassType.PrimitiveType)tuple11.Item2;
						if (primitiveType23.Item.Tag == 4)
						{
							fSharpFunc3.Invoke(operand10);
							il.Append(cil.MyInstruction.Conv_U8);
							il.Append(cil.MyInstruction.NewLdc_I8(4294967295L));
							il.Append(cil.MyInstruction.And);
							fSharpFunc12.Invoke(null);
							return;
						}
						goto end_IL_875b;
					}
					case 4:
						break;
					default:
						goto end_IL_875b;
					}
					cil.FirstClassType.FunnyIntegerType funnyIntegerType11 = (cil.FirstClassType.FunnyIntegerType)tuple11.Item2;
					cil.PrimitiveType item43 = primitiveType20.Item;
					item42 = funnyIntegerType11.Item;
					pt = item43;
					goto IL_8f92;
				}
				case 4:
					{
						switch (tuple11.Item2.Tag)
						{
						case 0:
						{
							cil.FirstClassType.PrimitiveType primitiveType21 = (cil.FirstClassType.PrimitiveType)tuple11.Item2;
							if (primitiveType21.Item.Tag == 4)
							{
								fSharpFunc3.Invoke(operand10);
								fSharpFunc12.Invoke(null);
								return;
							}
							goto end_IL_875b;
						}
						case 4:
							break;
						default:
							goto end_IL_875b;
						}
						cil.FirstClassType.FunnyIntegerType funnyIntegerType10 = (cil.FirstClassType.FunnyIntegerType)tuple11.Item2;
						cil.PrimitiveType item41 = primitiveType20.Item;
						item42 = funnyIntegerType10.Item;
						pt = item41;
						goto IL_8f92;
					}
					IL_8827:
					funnyIntegerType12 = (cil.FirstClassType.FunnyIntegerType)tuple11.Item2;
					item44 = primitiveType20.Item;
					item42 = funnyIntegerType12.Item;
					pt = item44;
					goto IL_8f92;
					IL_898e:
					funnyIntegerType13 = (cil.FirstClassType.FunnyIntegerType)tuple11.Item2;
					item45 = primitiveType20.Item;
					item42 = funnyIntegerType13.Item;
					pt = item45;
					goto IL_8f92;
					IL_8f92:
					func23 = fSharpFunc5.Invoke(null);
					fSharpFunc7.Invoke(func23);
					fSharpFunc6.Invoke(func23);
					fSharpFunc3.Invoke(operand10);
					store_mem_primitive_from_stack(il, pt);
					fSharpFunc9.Invoke(func23);
					return;
					IL_88dd:
					funnyIntegerType14 = (cil.FirstClassType.FunnyIntegerType)tuple11.Item2;
					item46 = primitiveType20.Item;
					item42 = funnyIntegerType14.Item;
					pt = item46;
					goto IL_8f92;
					end_IL_875b:
					break;
				}
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType11 = (cil.FirstClassType.VectorType)tuple11.Item1;
				switch (vectorType11.Item.elemtype.Tag)
				{
				case 2:
				{
					cil.VectorElementType.VecPrim vecPrim9 = (cil.VectorElementType.VecPrim)vectorType11.Item.elemtype;
					if (tuple11.Item2.Tag == 1)
					{
						cil.FirstClassType.VectorType vectorType13 = (cil.FirstClassType.VectorType)tuple11.Item2;
						switch (vectorType13.Item.elemtype.Tag)
						{
						case 2:
						{
							cil.VectorElementType.VecPrim vecPrim10 = (cil.VectorElementType.VecPrim)vectorType13.Item.elemtype;
							cil.VecPrimitiveType item39 = vecPrim9.Item;
							cil.VecPrimitiveType item40 = vecPrim10.Item;
							uint count18 = vectorType11.Item.count;
							uint count19 = vectorType13.Item.count;
							FSharpFunc<cil.VecPrimitiveType, cil.VecPrimitiveType>.InvokeFast(func20, item39, item40, count18);
							return;
						}
						case 3:
						{
							cil.VectorElementType.VecFunny vecFunny2 = (cil.VectorElementType.VecFunny)vectorType13.Item.elemtype;
							cil.VecPrimitiveType item37 = vecPrim9.Item;
							cil.FunnyIntegerType item38 = vecFunny2.Item;
							uint count16 = vectorType11.Item.count;
							uint count17 = vectorType13.Item.count;
							int arg8 = cil.sizeof_funnyinteger_rounded_up(item38);
							FSharpFunc<cil.VecPrimitiveType, int>.InvokeFast(func21, item37, arg8, count16);
							return;
						}
						}
					}
					break;
				}
				case 0:
					if (tuple11.Item2.Tag == 1)
					{
						cil.FirstClassType.VectorType vectorType12 = (cil.FirstClassType.VectorType)tuple11.Item2;
						if (vectorType12.Item.elemtype.Tag == 2)
						{
							cil.VectorElementType.VecPrim vecPrim8 = (cil.VectorElementType.VecPrim)vectorType12.Item.elemtype;
							cil.VecPrimitiveType item36 = vecPrim8.Item;
							uint count14 = vectorType11.Item.count;
							uint count15 = vectorType12.Item.count;
							FSharpFunc<cil.VecPrimitiveType, uint>.InvokeFast(func19, item36, count14);
							return;
						}
					}
					break;
				}
				break;
			}
			case 4:
			{
				cil.FirstClassType.FunnyIntegerType funnyIntegerType8 = (cil.FirstClassType.FunnyIntegerType)tuple11.Item1;
				switch (tuple11.Item2.Tag)
				{
				case 0:
				{
					cil.FirstClassType.PrimitiveType primitiveType19 = (cil.FirstClassType.PrimitiveType)tuple11.Item2;
					AddressOfValue addr8 = fSharpFunc4.Invoke(operand10);
					if (!(result_dest is InstructionDest._OnStack))
					{
						if (result_dest is InstructionDest._Void)
						{
							string message12 = "should not happen";
							throw Operators.Failure(message12);
						}
						InstructionDest.InTemp inTemp = (InstructionDest.InTemp)result_dest;
						cil.Variable dest = inTemp.item;
						init_local(il, dest);
						FSharpFunc<Unit, Unit> f_load_src3 = new f@4587-8(il, addr8);
						int siz3 = cil.get_sizeof(firstClassType14);
						cpblk_into(il, dest, f_load_src3, siz3);
					}
					else
					{
						cil.Variable variable = il.NewVariable(firstClassType15);
						init_local(il, variable);
						FSharpFunc<Unit, Unit> f_load_src4 = new f@4597-9(il, addr8);
						int siz4 = cil.get_sizeof(firstClassType14);
						cpblk_into(il, variable, f_load_src4, siz4);
						il.Append(cil.MyInstruction.NewLdloc(variable));
					}
					release_address_of_value(il, addr8);
					return;
				}
				case 4:
				{
					cil.FirstClassType.FunnyIntegerType funnyIntegerType9 = (cil.FirstClassType.FunnyIntegerType)tuple11.Item2;
					ResultLocal func22 = fSharpFunc5.Invoke(null);
					fSharpFunc7.Invoke(func22);
					AddressOfValue addr7 = fSharpFunc4.Invoke(operand10);
					FSharpFunc<Unit, Unit> f_load_src2 = new f@4615-10(il, addr7);
					int siz2 = cil.get_sizeof(firstClassType14);
					cpblk_into(il, fSharpFunc8.Invoke(func22), f_load_src2, siz2);
					release_address_of_value(il, addr7);
					fSharpFunc9.Invoke(func22);
					return;
				}
				}
				break;
			}
			}
			LLVMValueRef func24 = op;
			((FSharpFunc<LLVMValueRef, Unit>)fSharpTypeFunc.Specialize<LLVMValueRef>()).Invoke(func24);
			break;
		}
		case LLVMOpcode.LLVMTrunc:
		{
			LLVMValueRef operand8 = op.GetOperand(0u);
			cil.FirstClassType firstClassType12 = llvm_type_to_firstclass_type(operand8.TypeOf);
			cil.FirstClassType firstClassType13 = llvm_type_to_firstclass_type(op.TypeOf);
			FSharpFunc<cil.VecPrimitiveType, FSharpFunc<cil.VecPrimitiveType, FSharpFunc<uint, Unit>>> func16 = new trunc_vector@4632(il, op, fSharpFunc4, fSharpFunc5, fSharpFunc8, fSharpFunc9);
			FSharpTypeFunc fSharpTypeFunc6 = new trunc_vector_2@4676(il, op, fSharpFunc4, fSharpFunc5, fSharpFunc6, fSharpFunc9);
			Tuple<cil.FirstClassType, cil.FirstClassType> tuple10 = new Tuple<cil.FirstClassType, cil.FirstClassType>(firstClassType12, firstClassType13);
			switch (tuple10.Item1.Tag)
			{
			case 0:
			{
				cil.FirstClassType.PrimitiveType primitiveType14 = (cil.FirstClassType.PrimitiveType)tuple10.Item1;
				cil.FunnyIntegerType ft;
				cil.FirstClassType.FunnyIntegerType funnyIntegerType3;
				cil.FunnyIntegerType item26;
				cil.FirstClassType.FunnyIntegerType funnyIntegerType5;
				cil.FunnyIntegerType item29;
				ResultLocal func18;
				long item30;
				cil.Variable item31;
				int item32;
				cil.FirstClassType.FunnyIntegerType funnyIntegerType6;
				cil.FunnyIntegerType item33;
				cil.PrimitiveType item27;
				switch (primitiveType14.Item.Tag)
				{
				default:
				{
					if (tuple10.Item2.Tag != 4)
					{
						break;
					}
					cil.FirstClassType.FunnyIntegerType funnyIntegerType7 = (cil.FirstClassType.FunnyIntegerType)tuple10.Item2;
					ft = funnyIntegerType7.Item;
					item27 = primitiveType14.Item;
					goto IL_976e;
				}
				case 1:
				{
					switch (tuple10.Item2.Tag)
					{
					case 0:
					{
						cil.FirstClassType.PrimitiveType primitiveType16 = (cil.FirstClassType.PrimitiveType)tuple10.Item2;
						if (primitiveType16.Item.Tag == 0)
						{
							fSharpFunc3.Invoke(operand8);
							il.Append(cil.MyInstruction.NewLdc_I4(1));
							il.Append(cil.MyInstruction.And);
							fSharpFunc12.Invoke(null);
							return;
						}
						goto end_IL_922b;
					}
					case 4:
						break;
					default:
						goto end_IL_922b;
					}
					cil.FirstClassType.FunnyIntegerType funnyIntegerType4 = (cil.FirstClassType.FunnyIntegerType)tuple10.Item2;
					cil.FunnyIntegerType item28 = funnyIntegerType4.Item;
					item27 = primitiveType14.Item;
					ft = item28;
					goto IL_976e;
				}
				case 4:
				{
					switch (tuple10.Item2.Tag)
					{
					case 0:
						break;
					case 4:
						goto IL_9388;
					default:
						goto end_IL_922b;
					}
					cil.FirstClassType.PrimitiveType primitiveType18 = (cil.FirstClassType.PrimitiveType)tuple10.Item2;
					switch (primitiveType18.Item.Tag)
					{
					case 1:
						break;
					case 2:
						goto IL_96bc;
					case 3:
						fSharpFunc3.Invoke(operand8);
						il.Append(cil.MyInstruction.Conv_I4);
						fSharpFunc12.Invoke(null);
						return;
					default:
						goto end_IL_922b;
					}
					goto IL_969a;
				}
				case 3:
				{
					switch (tuple10.Item2.Tag)
					{
					case 0:
						break;
					case 4:
						goto IL_942f;
					default:
						goto end_IL_922b;
					}
					cil.FirstClassType.PrimitiveType primitiveType17 = (cil.FirstClassType.PrimitiveType)tuple10.Item2;
					switch (primitiveType17.Item.Tag)
					{
					case 1:
						break;
					case 2:
						goto IL_96bc;
					default:
						goto end_IL_922b;
					}
					goto IL_969a;
				}
				case 2:
					{
						switch (tuple10.Item2.Tag)
						{
						case 0:
							break;
						case 4:
							goto IL_94bb;
						default:
							goto end_IL_922b;
						}
						cil.FirstClassType.PrimitiveType primitiveType15 = (cil.FirstClassType.PrimitiveType)tuple10.Item2;
						if (primitiveType15.Item.Tag != 1)
						{
							break;
						}
						goto IL_969a;
					}
					IL_94bb:
					funnyIntegerType3 = (cil.FirstClassType.FunnyIntegerType)tuple10.Item2;
					item26 = funnyIntegerType3.Item;
					item27 = primitiveType14.Item;
					ft = item26;
					goto IL_976e;
					IL_942f:
					funnyIntegerType5 = (cil.FirstClassType.FunnyIntegerType)tuple10.Item2;
					item29 = funnyIntegerType5.Item;
					item27 = primitiveType14.Item;
					ft = item29;
					goto IL_976e;
					IL_976e:
					func18 = fSharpFunc5.Invoke(null);
					fSharpFunc6.Invoke(func18);
					fSharpFunc3.Invoke(operand8);
					il.Append(cil.MyInstruction.Conv_I8);
					item30 = make_mask_64(ft);
					il.Append(cil.MyInstruction.NewLdc_I8(item30));
					il.Append(cil.MyInstruction.And);
					item31 = il.NewVariable(cil.FirstClassType.NewPrimitiveType(cil.PrimitiveType.I64));
					il.Append(cil.MyInstruction.NewStloc(item31));
					item32 = cil.get_sizeof(firstClassType13);
					il.Append(cil.MyInstruction.NewLdloca(item31));
					il.Append(cil.MyInstruction.NewLdc_I4(item32));
					il.Append(cil.MyInstruction.Cpblk);
					fSharpFunc9.Invoke(func18);
					return;
					IL_9388:
					funnyIntegerType6 = (cil.FirstClassType.FunnyIntegerType)tuple10.Item2;
					item33 = funnyIntegerType6.Item;
					item27 = primitiveType14.Item;
					ft = item33;
					goto IL_976e;
					IL_96bc:
					fSharpFunc3.Invoke(operand8);
					il.Append(cil.MyInstruction.Conv_I2);
					fSharpFunc12.Invoke(null);
					return;
					IL_969a:
					fSharpFunc3.Invoke(operand8);
					il.Append(cil.MyInstruction.Conv_I1);
					fSharpFunc12.Invoke(null);
					return;
					end_IL_922b:
					break;
				}
				break;
			}
			case 4:
			{
				cil.FirstClassType.FunnyIntegerType funnyIntegerType = (cil.FirstClassType.FunnyIntegerType)tuple10.Item1;
				switch (tuple10.Item2.Tag)
				{
				case 0:
				{
					cil.FirstClassType.PrimitiveType primitiveType13 = (cil.FirstClassType.PrimitiveType)tuple10.Item2;
					cil.FunnyIntegerType item24 = funnyIntegerType.Item;
					cil.PrimitiveType item25 = primitiveType13.Item;
					AddressOfValue addr6 = fSharpFunc4.Invoke(operand8);
					emit_address_of_value(il, addr6);
					fSharpFunc11.Invoke(item25);
					fSharpFunc12.Invoke(null);
					release_address_of_value(il, addr6);
					return;
				}
				case 4:
				{
					cil.FirstClassType.FunnyIntegerType funnyIntegerType2 = (cil.FirstClassType.FunnyIntegerType)tuple10.Item2;
					cil.FunnyIntegerType item22 = funnyIntegerType.Item;
					cil.FunnyIntegerType item23 = funnyIntegerType2.Item;
					ResultLocal func17 = fSharpFunc5.Invoke(null);
					AddressOfValue addr5 = fSharpFunc4.Invoke(operand8);
					FSharpFunc<Unit, Unit> f_load_src = new f@4792-11(il, addr5);
					int siz = cil.get_sizeof(firstClassType13);
					cpblk_into(il, fSharpFunc8.Invoke(func17), f_load_src, siz);
					release_address_of_value(il, addr5);
					fSharpFunc9.Invoke(func17);
					return;
				}
				}
				break;
			}
			case 1:
			{
				cil.FirstClassType.VectorType vectorType8 = (cil.FirstClassType.VectorType)tuple10.Item1;
				cil.VectorElementType vectorElementType;
				cil.VectorElementType vectorElementType2;
				uint num4;
				if (vectorType8.Item.elemtype.Tag == 2)
				{
					cil.VectorElementType.VecPrim vecPrim6 = (cil.VectorElementType.VecPrim)vectorType8.Item.elemtype;
					if (tuple10.Item2.Tag != 1)
					{
						break;
					}
					cil.FirstClassType.VectorType vectorType9 = (cil.FirstClassType.VectorType)tuple10.Item2;
					if (vectorType9.Item.elemtype.Tag == 2)
					{
						cil.VectorElementType.VecPrim vecPrim7 = (cil.VectorElementType.VecPrim)vectorType9.Item.elemtype;
						cil.VecPrimitiveType item20 = vecPrim6.Item;
						cil.VecPrimitiveType item21 = vecPrim7.Item;
						uint count10 = vectorType8.Item.count;
						uint count11 = vectorType9.Item.count;
						FSharpFunc<cil.VecPrimitiveType, cil.VecPrimitiveType>.InvokeFast(func16, item20, item21, count10);
						return;
					}
					vectorElementType = vectorType8.Item.elemtype;
					vectorElementType2 = vectorType9.Item.elemtype;
					num4 = vectorType8.Item.count;
					uint count12 = vectorType9.Item.count;
				}
				else
				{
					if (tuple10.Item2.Tag != 1)
					{
						break;
					}
					cil.FirstClassType.VectorType vectorType10 = (cil.FirstClassType.VectorType)tuple10.Item2;
					cil.VectorElementType elemtype = vectorType8.Item.elemtype;
					cil.VectorElementType elemtype2 = vectorType10.Item.elemtype;
					uint count13 = vectorType8.Item.count;
					uint count12 = vectorType10.Item.count;
					num4 = count13;
					vectorElementType2 = elemtype2;
					vectorElementType = elemtype;
				}
				cil.VectorElementType vectorElementType3 = vectorElementType;
				cil.VectorElementType vectorElementType4 = vectorElementType2;
				uint arg5 = num4;
				cil.VectorElementType arg6 = vectorElementType4;
				cil.VectorElementType arg7 = vectorElementType3;
				FSharpFunc<cil.VectorElementType, cil.VectorElementType>.InvokeFast((FSharpFunc<cil.VectorElementType, FSharpFunc<cil.VectorElementType, FSharpFunc<uint, Unit>>>)fSharpTypeFunc6.Specialize<cil.VectorElementType>(), arg7, arg6, arg5);
				return;
			}
			}
			FSharpFunc<Tuple<cil.FirstClassType, cil.FirstClassType>, Unit> fSharpFunc25 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<Tuple<cil.FirstClassType, cil.FirstClassType>, Unit>, Unit, string, Unit, Tuple<cil.FirstClassType, cil.FirstClassType>>("no match case %A"));
			cil.FirstClassType item34 = firstClassType12;
			cil.FirstClassType item35 = firstClassType13;
			fSharpFunc25.Invoke(new Tuple<cil.FirstClassType, cil.FirstClassType>(item34, item35));
			break;
		}
		case LLVMOpcode.LLVMLoad:
		{
			cil.FirstClassType arg4 = llvm_type_to_firstclass_type(op.TypeOf);
			fSharpFunc3.Invoke(op.GetOperand(0u));
			FSharpFunc<cil.FirstClassType, LLVMValueRef>.InvokeFast(func2, arg4, op);
			break;
		}
		case LLVMOpcode.LLVMRet:
			if (op.OperandCount == 1)
			{
				fSharpFunc3.Invoke(op.GetOperand(0u));
			}
			il.Append(cil.MyInstruction.NewBr(lab_end));
			break;
		case LLVMOpcode.LLVMUnreachable:
		{
			string item19 = "unreachable";
			il.Append(cil.MyInstruction.NewLdstr(item19));
			il.Append(cil.MyInstruction.NewNewobj(methodReference));
			il.Append(cil.MyInstruction.Throw);
			break;
		}
		case LLVMOpcode.LLVMAtomicCmpXchg:
		{
			cil.FirstClassType firstClassType9 = llvm_type_to_firstclass_type(op.GetOperand(1u).TypeOf);
			if (firstClassType9.Tag == 0)
			{
				cil.FirstClassType.PrimitiveType primitiveType5 = (cil.FirstClassType.PrimitiveType)firstClassType9;
				cil.PrimitiveType item5 = primitiveType5.Item;
				cil.PrimitiveType primitiveType6 = item5;
				cil.PrimitiveType primitiveType7 = primitiveType6;
				switch (primitiveType7.Tag)
				{
				case 1:
				case 3:
				case 4:
				{
					cil.FirstClassType firstClassType10 = llvm_type_to_firstclass_type(op.TypeOf);
					cil.FirstClassType firstClassType11 = firstClassType10;
					if (firstClassType11.Tag == 2)
					{
						cil.FirstClassType.StructType structType = (cil.FirstClassType.StructType)firstClassType11;
						cil.StructType item6 = structType.Item;
						cil.StructType structType2 = item6;
						ResultLocal func11 = fSharpFunc5.Invoke(null);
						fSharpFunc6.Invoke(func11);
						if ((uint)structType2.items[0].off != 0)
						{
							il.Append(cil.MyInstruction.NewLdc_I4((int)structType2.items[0].off));
							il.Append(cil.MyInstruction.Add);
						}
						fSharpFunc6.Invoke(func11);
						il.Append(cil.MyInstruction.NewLdc_I4((int)structType2.items[1].off));
						il.Append(cil.MyInstruction.Add);
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(2u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						string text2 = primitiveType6.Tag switch
						{
							1 => "i8", 
							2 => "i16", 
							3 => "i32", 
							4 => "i64", 
							_ => throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 4864, 26), 
						};
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, string>("cmpxchg_%P()", new object[1] { text2 }, null)))));
						fSharpFunc9.Invoke(func11);
						break;
					}
					string message5 = "not a struct";
					throw Operators.Failure(message5);
				}
				default:
				{
					cil.PrimitiveType primitiveType8 = primitiveType7;
					FSharpFunc<cil.PrimitiveType, Unit> fSharpFunc21 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.PrimitiveType, Unit>, Unit, string, Unit, cil.PrimitiveType>("match case fail: %A"));
					cil.PrimitiveType func10 = primitiveType8;
					fSharpFunc21.Invoke(func10);
					break;
				}
				}
				break;
			}
			string message6 = "not prim";
			throw Operators.Failure(message6);
		}
		case LLVMOpcode.LLVMAtomicRMW:
		{
			LLVMAtomicRMWBinOp atomicRMWBinOp = op.AtomicRMWBinOp;
			cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(op.TypeOf);
			if (firstClassType.Tag == 0)
			{
				cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)firstClassType;
				cil.PrimitiveType item2 = primitiveType.Item;
				cil.PrimitiveType primitiveType2 = item2;
				string text = primitiveType2.Tag switch
				{
					1 => "i8", 
					2 => "i16", 
					3 => "i32", 
					4 => "i64", 
					_ => throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 4883, 22), 
				};
				Tuple<LLVMAtomicRMWBinOp, cil.PrimitiveType> tuple = new Tuple<LLVMAtomicRMWBinOp, cil.PrimitiveType>(atomicRMWBinOp, primitiveType2);
				switch (tuple.Item1)
				{
				default:
					switch (tuple.Item1)
					{
					case LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpOr:
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, string>("atomicrmw_or_%P()", new object[1] { text }, null)))));
						fSharpFunc12.Invoke(null);
						break;
					case LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpXor:
						fSharpFunc3.Invoke(op.GetOperand(0u));
						fSharpFunc3.Invoke(op.GetOperand(1u));
						il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, string>("atomicrmw_xor_%P()", new object[1] { text }, null)))));
						fSharpFunc12.Invoke(null);
						break;
					default:
					{
						LLVMValueRef func7 = op;
						((FSharpFunc<LLVMValueRef, Unit>)fSharpTypeFunc.Specialize<LLVMValueRef>()).Invoke(func7);
						break;
					}
					}
					break;
				case LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpXchg:
					fSharpFunc3.Invoke(op.GetOperand(0u));
					fSharpFunc3.Invoke(op.GetOperand(1u));
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, string>("atomicrmw_xchg_%P()", new object[1] { text }, null)))));
					fSharpFunc12.Invoke(null);
					break;
				case LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpAnd:
					fSharpFunc3.Invoke(op.GetOperand(0u));
					fSharpFunc3.Invoke(op.GetOperand(1u));
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, string>("atomicrmw_and_%P()", new object[1] { text }, null)))));
					fSharpFunc12.Invoke(null);
					break;
				case LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpAdd:
					fSharpFunc3.Invoke(op.GetOperand(0u));
					fSharpFunc3.Invoke(op.GetOperand(1u));
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, string>("atomicrmw_add_%P()", new object[1] { text }, null)))));
					fSharpFunc12.Invoke(null);
					break;
				case LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpSub:
					fSharpFunc3.Invoke(op.GetOperand(0u));
					fSharpFunc3.Invoke(op.GetOperand(1u));
					il.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke(ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<string, Unit, string, string, string>("atomicrmw_sub_%P()", new object[1] { text }, null)))));
					fSharpFunc12.Invoke(null);
					break;
				}
				break;
			}
			string message = "not prim";
			throw Operators.Failure(message);
		}
		case LLVMOpcode.LLVMFence:
		{
			MethodReference item = typs.md.ImportReference(typeof(Interlocked).GetMethod("MemoryBarrier", new Type[0]));
			il.Append(cil.MyInstruction.NewCall(item));
			break;
		}
		default:
			{
				LLVMValueRef func6 = op;
				((FSharpFunc<LLVMValueRef, Unit>)fSharpTypeFunc.Specialize<LLVMValueRef>()).Invoke(func6);
				break;
			}
			IL_726a:
			il.Append(cil.MyInstruction.Conv_U8);
			goto IL_72a3;
			IL_725b:
			il.Append(cil.MyInstruction.Conv_U4);
			goto IL_72a3;
			IL_724c:
			il.Append(cil.MyInstruction.Conv_U2);
			goto IL_72a3;
			IL_7279:
			fSharpFunc29 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<cil.FirstClassType, Unit>, Unit, string, Unit, cil.FirstClassType>("fptoui type not found %A"));
			func30 = firstClassType30;
			fSharpFunc29.Invoke(func30);
			goto IL_72a3;
			IL_72a3:
			fSharpFunc12.Invoke(null);
			break;
			IL_6980:
			func51 = fSharpFunc5.Invoke(null);
			addressOfValue20 = fSharpFunc4.Invoke(op.GetOperand(0u));
			f_load_src8 = new f@3966-4(il, addressOfValue20);
			siz8 = cil.get_sizeof(firstClassType45);
			cpblk_into(il, fSharpFunc8.Invoke(func51), f_load_src8, siz8);
			release_address_of_value(il, addressOfValue20);
			fSharpFunc9.Invoke(func51);
			break;
		}
	}

	public static cil.FirstClassType global_type(LLVMValueRef gi)
	{
		switch (gi.TypeOf.Kind)
		{
		case LLVMTypeKind.LLVMPointerTypeKind:
			return llvm_type_to_firstclass_type(gi.TypeOf.ElementType);
		default:
		{
			string message = "global type not a ptr";
			throw Operators.Failure(message);
		}
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static FieldDefinition create_global(cil.GenTypes typs, LLVMValueRef gi)
	{
		Mono.Cecil.FieldAttributes fieldAttributes = Mono.Cecil.FieldAttributes.Public;
		return new FieldDefinition(gi.Name, fieldAttributes | Mono.Cecil.FieldAttributes.Static, typs.md.TypeSystem.IntPtr);
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static MethodDefinition create_method(cil.GenTypes typs, LLVMValueRef fi)
	{
		LLVMTypeRef t = sgllvm.get_return_type(fi.TypeOf.ElementType);
		TypeReference typeReference;
		switch (t.Kind)
		{
		case LLVMTypeKind.LLVMVoidTypeKind:
			typeReference = typs.md.TypeSystem.Void;
			break;
		default:
		{
			cil.FirstClassType vt = llvm_type_to_firstclass_type(t);
			typeReference = cil.firstclass_type_to_cecil_type(typs, vt);
			break;
		}
		}
		TypeReference returnType = typeReference;
		Mono.Cecil.MethodAttributes methodAttributes = Mono.Cecil.MethodAttributes.Public;
		return new MethodDefinition(fi.Name, methodAttributes | Mono.Cecil.MethodAttributes.Static, returnType);
	}

	public static bool want_trace_enter_exit(TraceEnterExit t)
	{
		return t.Tag != 1;
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1 })]
	public static void gen_function_code(cil.GenTypes typs, GenSyms syms, TraceEnterExit trace_enter_exit, MethodRefInternal mi)
	{
		mi.method@.Body.InitLocals = false;
		cil.CilWriter cilWriter = new cil.CilWriter();
		if (want_trace_enter_exit(trace_enter_exit))
		{
			cilWriter.Append(cil.MyInstruction.NewLdstr(mi.method@.Name));
			cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("trace_enter")));
			int num = 0;
			int num2 = mi.func@.Params.Length - 1;
			if (num2 >= num)
			{
				do
				{
					LLVMValueRef key = mi.func@.Params[num];
					cil.FirstClassType firstClassType = llvm_type_to_firstclass_type(key.TypeOf);
					ParameterDefinition item = mi.args@[key];
					cilWriter.Append(cil.MyInstruction.NewLdc_I4(num));
					int item2 = get_trace_type_code(firstClassType);
					cilWriter.Append(cil.MyInstruction.NewLdc_I4(item2));
					cilWriter.Append(cil.MyInstruction.NewLdarga(item));
					int item3 = cil.get_sizeof(firstClassType);
					cilWriter.Append(cil.MyInstruction.NewLdc_I4(item3));
					cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("trace_parma")));
					num++;
				}
				while (num != num2 + 1);
			}
		}
		Dictionary<LLVMValueRef, InstructionValue> dictionary = new Dictionary<LLVMValueRef, InstructionValue>();
		LLVMBasicBlockRef[] basicBlocks = mi.func@.BasicBlocks;
		foreach (LLVMBasicBlockRef lLVMBasicBlockRef in basicBlocks)
		{
			HashSet<cil.Variable> hashSet = new HashSet<cil.Variable>();
			List<LLVMValueRef> list = sgllvm.get_instructions(lLVMBasicBlockRef);
			List<LLVMValueRef>.Enumerator enumerator = list.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					LLVMValueRef current = enumerator.Current;
					switch (current.InstructionOpcode)
					{
					case LLVMOpcode.LLVMAlloca:
						switch (current.TypeOf.Kind)
						{
						case LLVMTypeKind.LLVMPointerTypeKind:
						{
							cil.FirstClassType tr = llvm_type_to_firstclass_type(current.TypeOf.ElementType);
							LLVMValueRef operand = current.GetOperand(0u);
							FSharpOption<long> fSharpOption = get_maybe_integer_constant(operand);
							if (fSharpOption != null)
							{
								FSharpOption<long> fSharpOption2 = fSharpOption;
								if (fSharpOption2.Value == 1)
								{
									cil.Variable item4 = cilWriter.NewVariable(tr);
									dictionary.Add(current, InstructionValue.NewLocal(item4));
								}
								else
								{
									long value = fSharpOption2.Value;
								}
							}
							else
							{
								cil.FirstClassType tr2 = llvm_type_to_firstclass_type(current.TypeOf);
								cil.Variable item5 = cilWriter.NewVariable(tr2);
								dictionary.Add(current, InstructionValue.NewTemp(item5));
							}
							break;
						}
						default:
						{
							string message = "alloca type not a ptr";
							throw Operators.Failure(message);
						}
						}
						continue;
					}
					switch (current.TypeOf.Kind)
					{
					case LLVMTypeKind.LLVMVoidTypeKind:
						continue;
					}
					cil.FirstClassType firstClassType2 = llvm_type_to_firstclass_type(current.TypeOf);
					List<LLVMUseRef> list2 = sgllvm.get_uses(current);
					bool flag = SeqModule.Exists(new used_outside_its_block@5097(lLVMBasicBlockRef), list2);
					bool flag2 = SeqModule.Exists(new used_by_phi@5106(), list2);
					FSharpFunc<LLVMValueRef, int> fSharpFunc = new calc_depth@5117(dictionary);
					cil.FirstClassType firstClassType3 = firstClassType2;
					switch (firstClassType3.Tag)
					{
					case 0:
					{
						cil.FirstClassType.PrimitiveType primitiveType = (cil.FirstClassType.PrimitiveType)firstClassType3;
						break;
					}
					case 1:
					{
						cil.FirstClassType.VectorType vectorType = (cil.FirstClassType.VectorType)firstClassType3;
						break;
					}
					case 4:
					{
						cil.FirstClassType.FunnyIntegerType funnyIntegerType = (cil.FirstClassType.FunnyIntegerType)firstClassType3;
						break;
					}
					case 2:
					{
						cil.FirstClassType.StructType structType = (cil.FirstClassType.StructType)firstClassType3;
						break;
					}
					case 3:
					{
						cil.FirstClassType.ArrayType arrayType = (cil.FirstClassType.ArrayType)firstClassType3;
						break;
					}
					}
					switch (current.InstructionOpcode)
					{
					case LLVMOpcode.LLVMAdd:
					case LLVMOpcode.LLVMSub:
					case LLVMOpcode.LLVMMul:
					case LLVMOpcode.LLVMUDiv:
					case LLVMOpcode.LLVMSDiv:
					case LLVMOpcode.LLVMURem:
					case LLVMOpcode.LLVMSRem:
					case LLVMOpcode.LLVMShl:
					case LLVMOpcode.LLVMLShr:
					case LLVMOpcode.LLVMAShr:
					case LLVMOpcode.LLVMAnd:
					case LLVMOpcode.LLVMOr:
					case LLVMOpcode.LLVMXor:
					case LLVMOpcode.LLVMGetElementPtr:
					case LLVMOpcode.LLVMTrunc:
					case LLVMOpcode.LLVMZExt:
					case LLVMOpcode.LLVMSExt:
					case LLVMOpcode.LLVMPtrToInt:
					case LLVMOpcode.LLVMIntToPtr:
					case LLVMOpcode.LLVMICmp:
					case LLVMOpcode.LLVMFCmp:
						if (list2.Count != 1 || flag || flag2)
						{
							break;
						}
						if (fSharpFunc.Invoke(current) >= 16)
						{
							FSharpFunc<LLVMValueRef, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<LLVMValueRef, Unit>, TextWriter, Unit, Unit, LLVMValueRef>("WOULD BE IMMED, but chain too long: %A"));
							LLVMValueRef func = current;
							fSharpFunc2.Invoke(func);
							if (0 == 0)
							{
								break;
							}
						}
						dictionary.Add(current, InstructionValue.NewImmed(current));
						continue;
					}
					cil.Variable item6 = cilWriter.NewVariable(firstClassType2);
					dictionary.Add(current, InstructionValue.NewTemp(item6));
				}
				Unit unit = null;
			}
			finally
			{
				((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
		}
		FSharpFunc<LLVMBasicBlockRef, LLVMValueRef[]> fSharpFunc3 = new find_phis@5334();
		cil.Label label = cilWriter.NewLabel();
		Dictionary<LLVMBasicBlockRef, Block> dictionary2 = new Dictionary<LLVMBasicBlockRef, Block>();
		Dictionary<LLVMBasicBlockRef, PhiBlockInfo> dictionary3 = new Dictionary<LLVMBasicBlockRef, PhiBlockInfo>();
		LLVMBasicBlockRef[] basicBlocks2 = mi.func@.BasicBlocks;
		LLVMValueRef lLVMValueRef = default(LLVMValueRef);
		LLVMValueRef lLVMValueRef2 = default(LLVMValueRef);
		foreach (LLVMBasicBlockRef lLVMBasicBlockRef2 in basicBlocks2)
		{
			LLVMValueRef[] array = fSharpFunc3.Invoke(lLVMBasicBlockRef2);
			if (array.Length > 0)
			{
				Dictionary<LLVMBasicBlockRef, Dictionary<LLVMValueRef, LLVMValueRef>> dictionary4 = new Dictionary<LLVMBasicBlockRef, Dictionary<LLVMValueRef, LLVMValueRef>>();
				Dictionary<LLVMValueRef, Dictionary<LLVMBasicBlockRef, LLVMValueRef>> dictionary5 = new Dictionary<LLVMValueRef, Dictionary<LLVMBasicBlockRef, LLVMValueRef>>();
				LLVMValueRef[] array2 = array;
				for (int k = 0; k < array2.Length; k++)
				{
					LLVMValueRef key2 = array2[k];
					Dictionary<LLVMBasicBlockRef, LLVMValueRef> dictionary6 = new Dictionary<LLVMBasicBlockRef, LLVMValueRef>();
					dictionary5.Add(key2, dictionary6);
					int num3 = 0;
					int num4 = (int)(key2.IncomingCount - 1);
					if (num4 < num3)
					{
						continue;
					}
					do
					{
						LLVMBasicBlockRef incomingBlock = key2.GetIncomingBlock((uint)num3);
						LLVMValueRef incomingValue = key2.GetIncomingValue((uint)num3);
						LLVMValueRef value2 = lLVMValueRef;
						Tuple<bool, LLVMValueRef> tuple = new Tuple<bool, LLVMValueRef>(dictionary6.TryGetValue(incomingBlock, out value2), value2);
						if (tuple.Item1)
						{
							LLVMValueRef item7 = tuple.Item2;
							LLVMValueRef x = item7;
							LLVMValueRef y = incomingValue;
							if (!LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(x, y))
							{
								string message2 = "phi mismatch";
								throw Operators.Failure(message2);
							}
						}
						else
						{
							dictionary6.Add(incomingBlock, incomingValue);
						}
						Dictionary<LLVMValueRef, LLVMValueRef> dictionary7 = d_get_or_add(dictionary4, incomingBlock, new phi_val@5362());
						LLVMValueRef value3 = lLVMValueRef2;
						Tuple<bool, LLVMValueRef> tuple2 = new Tuple<bool, LLVMValueRef>(dictionary7.TryGetValue(key2, out value3), value3);
						if (tuple2.Item1)
						{
							LLVMValueRef item8 = tuple2.Item2;
							LLVMValueRef x2 = item8;
							LLVMValueRef y2 = incomingValue;
							if (!LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(x2, y2))
							{
								string message3 = "phi mismatch";
								throw Operators.Failure(message3);
							}
						}
						else
						{
							dictionary7.Add(key2, incomingValue);
						}
						num3++;
					}
					while (num3 != num4 + 1);
				}
				dictionary3.Add(lLVMBasicBlockRef2, new PhiBlockInfo(array, dictionary4, dictionary5));
				Dictionary<LLVMBasicBlockRef, cil.Label> dictionary8 = new Dictionary<LLVMBasicBlockRef, cil.Label>();
				Dictionary<LLVMBasicBlockRef, Dictionary<LLVMValueRef, LLVMValueRef>>.KeyCollection keys = dictionary4.Keys;
				Dictionary<LLVMBasicBlockRef, Dictionary<LLVMValueRef, LLVMValueRef>>.KeyCollection.Enumerator enumerator2 = keys.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						LLVMBasicBlockRef current2 = enumerator2.Current;
						cil.Label value4 = cilWriter.NewLabel();
						dictionary8.Add(current2, value4);
					}
					Unit unit2 = null;
				}
				finally
				{
					((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
					_ = null;
				}
				dictionary2.Add(lLVMBasicBlockRef2, Block.NewPhi(dictionary8));
			}
			else
			{
				cil.Label item9 = cilWriter.NewLabel();
				dictionary2.Add(lLVMBasicBlockRef2, Block.NewRegular(item9));
			}
		}
		string name = mi.func@.Name;
		bool flag3 = false;
		FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<FSharpFunc<cil.CilWriter, FSharpFunc<LLVMValueRef, Unit>>, Unit>>> fSharpFunc4 = new my_load_instruction_value_to_stack@5401();
		FSharpFunc<LLVMValueRef, InstructionValue> fSharpFunc5 = new f_get_instr_value@5427(dictionary);
		FSharpFunc<LLVMValueRef, InstructionDest> fSharpFunc6 = new get_result_dest@5431(dictionary);
		FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> fSharpFunc7 = new f_load_instr_value_to_dest@5438(typs, syms, mi, label, dictionary2, fSharpFunc5);
		LLVMBasicBlockRef[] basicBlocks3 = mi.func@.BasicBlocks;
		foreach (LLVMBasicBlockRef lLVMBasicBlockRef3 in basicBlocks3)
		{
			Block block = dictionary2[lLVMBasicBlockRef3];
			if (!(block is Block.Phi))
			{
				Block.Regular regular = (Block.Regular)block;
				cil.Label item10 = regular.item;
				cilWriter.Append(cil.MyInstruction.NewLabel(item10));
			}
			else
			{
				Block.Phi phi = (Block.Phi)block;
				Dictionary<LLVMBasicBlockRef, cil.Label> dictionary9 = phi.item;
				PhiBlockInfo phiBlockInfo = dictionary3[lLVMBasicBlockRef3];
				HashSet<LLVMValueRef> hashSet2 = new HashSet<LLVMValueRef>();
				Dictionary<LLVMBasicBlockRef, cil.Label> dictionary10 = dictionary9;
				Dictionary<LLVMBasicBlockRef, cil.Label>.Enumerator enumerator3 = dictionary10.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						LLVMBasicBlockRef key3 = enumerator3.Current.Key;
						Dictionary<LLVMValueRef, LLVMValueRef> dictionary11 = phiBlockInfo.incoming_phi_val@[key3];
						Dictionary<LLVMValueRef, LLVMValueRef>.Enumerator enumerator4 = dictionary11.GetEnumerator();
						try
						{
							while (enumerator4.MoveNext())
							{
								KeyValuePair<LLVMValueRef, LLVMValueRef> current3 = enumerator4.Current;
								Tuple<LLVMValueRef, LLVMValueRef> tuple3 = Operators.KeyValuePattern(current3);
								LLVMValueRef item11 = tuple3.Item2;
								if (sgllvm.get_value_kind(item11) == LLVMValueKind.LLVMInstructionValueKind && item11.InstructionOpcode == LLVMOpcode.LLVMPHI && LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(item11.InstructionParent, lLVMBasicBlockRef3))
								{
									bool flag4 = hashSet2.Add(item11);
									bool flag5 = flag4;
								}
							}
							Unit unit3 = null;
						}
						finally
						{
							((IDisposable)enumerator4/*cast due to .constrained prefix*/).Dispose();
							_ = null;
						}
					}
					Unit unit4 = null;
				}
				finally
				{
					((IDisposable)enumerator3/*cast due to .constrained prefix*/).Dispose();
					_ = null;
				}
				cil.Label item12 = cilWriter.NewLabel();
				Dictionary<LLVMValueRef, cil.Variable> dictionary12 = new Dictionary<LLVMValueRef, cil.Variable>();
				LLVMValueRef[] phis@ = phiBlockInfo.phis@;
				for (int m = 0; m < phis@.Length; m++)
				{
					LLVMValueRef lLVMValueRef3 = phis@[m];
					if (hashSet2.Contains(lLVMValueRef3))
					{
						cil.FirstClassType tr3 = llvm_type_to_firstclass_type(lLVMValueRef3.TypeOf);
						cil.Variable value5 = cilWriter.NewVariable(tr3);
						dictionary12.Add(lLVMValueRef3, value5);
					}
				}
				FSharpFunc<LLVMBasicBlockRef, Unit> fSharpFunc8 = new fix_phis@5490(typs, syms, mi, cilWriter, dictionary, fSharpFunc5, fSharpFunc7, phiBlockInfo, dictionary12);
				Dictionary<LLVMBasicBlockRef, cil.Label> dictionary13 = dictionary9;
				Dictionary<LLVMBasicBlockRef, cil.Label>.Enumerator enumerator5 = dictionary13.GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						KeyValuePair<LLVMBasicBlockRef, cil.Label> current4 = enumerator5.Current;
						LLVMBasicBlockRef key4 = current4.Key;
						cil.Label value6 = current4.Value;
						cilWriter.Append(cil.MyInstruction.NewLabel(value6));
						fSharpFunc8.Invoke(key4);
						cilWriter.Append(cil.MyInstruction.NewBr(item12));
					}
					Unit unit5 = null;
				}
				finally
				{
					((IDisposable)enumerator5/*cast due to .constrained prefix*/).Dispose();
					_ = null;
				}
				cilWriter.Append(cil.MyInstruction.NewLabel(item12));
			}
			List<LLVMValueRef> list3 = sgllvm.get_instructions(lLVMBasicBlockRef3);
			List<LLVMValueRef>.Enumerator enumerator6 = list3.GetEnumerator();
			try
			{
				while (enumerator6.MoveNext())
				{
					LLVMValueRef current5 = enumerator6.Current;
					TraceInstruction traceInstruction = ((!flag3) ? TraceInstruction.No : (current5.InstructionOpcode switch
					{
						LLVMOpcode.LLVMCall => (!current5.GetOperand((uint)(current5.OperandCount - 1)).Name.StartsWith("llvm.lifetime.")) ? TraceInstruction.Yes : TraceInstruction.No, 
						_ => TraceInstruction.Yes, 
					}));
					bool flag6 = flag3 && current5.TypeOf.Kind switch
					{
						LLVMTypeKind.LLVMVoidTypeKind => current5.InstructionOpcode switch
						{
							LLVMOpcode.LLVMBr => true, 
							_ => false, 
						}, 
						_ => false, 
					};
					FSharpFunc<LLVMOpcode, string> fSharpFunc9 = new fix_opcode@5574();
					if (flag6)
					{
						LLVMOpcode instructionOpcode = current5.InstructionOpcode;
						cilWriter.Append(cil.MyInstruction.NewLdstr(fSharpFunc9.Invoke(instructionOpcode)));
						string item13 = one_line(current5.ToString());
						cilWriter.Append(cil.MyInstruction.NewLdstr(item13));
						cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("trace_misc_void_0")));
					}
					FSharpFunc<LLVMValueRef, bool> fSharpFunc10 = new is_immed@5585(dictionary);
					if (!fSharpFunc10.Invoke(current5))
					{
						InstructionDest result_dest = fSharpFunc6.Invoke(current5);
						gen_instr(typs, syms, cilWriter, label, current5, mi.args@, mi.extra@, fSharpFunc5, fSharpFunc7, result_dest, dictionary2);
					}
					if (!flag3 || flag6)
					{
						continue;
					}
					cil.CilWriter il = cilWriter;
					Dictionary<LLVMValueRef, ParameterDefinition> args@ = mi.args@;
					FSharpFunc<LLVMValueRef, InstructionValue> f_get_instr_value = fSharpFunc5;
					FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest = fSharpFunc7;
					FSharpFunc<LLVMValueRef, Unit> fSharpFunc11 = new load_value@5603-1(typs, syms, il, args@, f_get_instr_value, f_load_instr_value_to_dest);
					FSharpFunc<Unit, Unit> write_trace_args_instr_opcode_and_ir = new write_trace_args_instr_opcode_and_ir@5605(cilWriter, current5, fSharpFunc9);
					switch (current5.TypeOf.Kind)
					{
					case LLVMTypeKind.LLVMVoidTypeKind:
					{
						FSharpFunc<Unit, Unit> fSharpFunc12 = new write_trace_generic_void@5610(syms, mi, cilWriter, dictionary, current5, write_trace_args_instr_opcode_and_ir);
						switch (current5.InstructionOpcode)
						{
						case LLVMOpcode.LLVMStore:
							fSharpFunc12.Invoke(null);
							break;
						case LLVMOpcode.LLVMCall:
						{
							LLVMValueRef operand2 = current5.GetOperand((uint)(current5.OperandCount - 1));
							if (operand2.Name.StartsWith("llvm.memcpy."))
							{
								LLVMValueRef operand3 = current5.GetOperand(0u);
								LLVMValueRef operand4 = current5.GetOperand(1u);
								LLVMValueRef operand5 = current5.GetOperand(2u);
								cilWriter.Append(cil.MyInstruction.NewLdstr(one_line(current5.ToString())));
								fSharpFunc11.Invoke(operand3);
								fSharpFunc11.Invoke(operand4);
								fSharpFunc11.Invoke(operand5);
								cilWriter.Append(cil.MyInstruction.Conv_I4);
								cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("trace_memcpy")));
							}
							else if (operand2.Name.StartsWith("llvm.memmove."))
							{
								LLVMValueRef operand6 = current5.GetOperand(0u);
								LLVMValueRef operand7 = current5.GetOperand(1u);
								LLVMValueRef operand8 = current5.GetOperand(2u);
								cilWriter.Append(cil.MyInstruction.NewLdstr(one_line(current5.ToString())));
								fSharpFunc11.Invoke(operand6);
								fSharpFunc11.Invoke(operand7);
								fSharpFunc11.Invoke(operand8);
								cilWriter.Append(cil.MyInstruction.Conv_I4);
								cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("trace_memmove")));
							}
							else if (operand2.Name.StartsWith("llvm.memset."))
							{
								LLVMValueRef operand9 = current5.GetOperand(0u);
								LLVMValueRef operand10 = current5.GetOperand(1u);
								LLVMValueRef operand11 = current5.GetOperand(2u);
								cilWriter.Append(cil.MyInstruction.NewLdstr(one_line(current5.ToString())));
								fSharpFunc11.Invoke(operand9);
								fSharpFunc11.Invoke(operand10);
								fSharpFunc11.Invoke(operand11);
								cilWriter.Append(cil.MyInstruction.Conv_I4);
								cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("trace_memset")));
							}
							else
							{
								fSharpFunc12.Invoke(null);
							}
							break;
						}
						default:
							fSharpFunc12.Invoke(null);
							break;
						}
						continue;
					}
					}
					FSharpFunc<cil.CilWriter, FSharpFunc<Dictionary<LLVMValueRef, InstructionValue>, FSharpFunc<LLVMValueRef, cil.Variable>>> fSharpFunc13 = new get_instruction_value_temp@5689();
					FSharpFunc<Unit, Unit> write_trace_args_instr_result = new write_trace_args_instr_result@5698(cilWriter, dictionary, current5, fSharpFunc13);
					FSharpFunc<Unit, Unit> fSharpFunc14 = new write_trace_generic_result@5706(syms, mi, cilWriter, dictionary, current5, write_trace_args_instr_opcode_and_ir, write_trace_args_instr_result);
					switch (current5.InstructionOpcode)
					{
					case LLVMOpcode.LLVMCall:
					{
						LLVMValueRef operand12 = current5.GetOperand((uint)(current5.OperandCount - 1));
						if (string.Equals(operand12.Name, "HeapAlloc") && operand12.ParamsCount == 3)
						{
							cilWriter.Append(cil.MyInstruction.NewLdstr(one_line(current5.ToString())));
							cilWriter.Append(cil.MyInstruction.NewLdstr(operand12.Name));
							fSharpFunc11.Invoke(current5.GetOperand(2u));
							cil.Variable item14 = FSharpFunc<cil.CilWriter, Dictionary<LLVMValueRef, InstructionValue>>.InvokeFast(fSharpFunc13, cilWriter, dictionary, current5);
							cilWriter.Append(cil.MyInstruction.NewLdloc(item14));
							cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("trace_heapa")));
						}
						else
						{
							fSharpFunc14.Invoke(null);
						}
						continue;
					}
					}
					InstructionValue instructionValue = dictionary[current5];
					InstructionValue instructionValue2 = instructionValue;
					if (!(instructionValue2 is InstructionValue.Temp))
					{
						if (!(instructionValue2 is InstructionValue.Local))
						{
							throw new MatchFailureException("C:\\Users\\eric\\dev\\glue_dotnet_rust\\llvm2cil\\Cecil.fs", 5754, 34);
						}
						InstructionValue.Local local = (InstructionValue.Local)instructionValue;
						cil.Variable item15 = local.item;
						cilWriter.Append(cil.MyInstruction.NewLdstr(one_line(current5.ToString())));
						switch (current5.TypeOf.Kind)
						{
						case LLVMTypeKind.LLVMPointerTypeKind:
						{
							cil.FirstClassType t = llvm_type_to_firstclass_type(current5.TypeOf.ElementType);
							cilWriter.Append(cil.MyInstruction.NewLdloca(item15));
							int item16 = cil.get_sizeof(t);
							cilWriter.Append(cil.MyInstruction.NewLdc_I4(item16));
							cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("trace_alloca")));
							break;
						}
						default:
						{
							string message4 = "alloca type not a ptr";
							throw Operators.Failure(message4);
						}
						}
					}
					else
					{
						InstructionValue.Temp temp = (InstructionValue.Temp)instructionValue;
						cil.Variable variable = temp.item;
						fSharpFunc14.Invoke(null);
					}
				}
				Unit unit6 = null;
			}
			finally
			{
				((IDisposable)enumerator6/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
		}
		cilWriter.Append(cil.MyInstruction.NewLabel(label));
		if (want_trace_enter_exit(trace_enter_exit))
		{
			cilWriter.Append(cil.MyInstruction.NewLdstr(mi.method@.Name));
			cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("trace_exit")));
		}
		cilWriter.Append(cil.MyInstruction.Ret);
		cilWriter.Finish(mi.method@, typs);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static FieldReference import_global(ModuleDefinition md, LLVMValueRef f, TypeDefinition[] references)
	{
		FSharpOption<FieldReference> fSharpOption = import.refs_import_field(md, references, f.Name);
		if (fSharpOption != null)
		{
			FSharpOption<FieldReference> fSharpOption2 = fSharpOption;
			return fSharpOption2.Value;
		}
		FSharpFunc<string, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("import global not found: %s"));
		string name = f.Name;
		fSharpFunc.Invoke(name);
		return null;
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static MethodReference import_method(ModuleDefinition md, string name, TypeDefinition[] references)
	{
		FSharpOption<MethodReference> fSharpOption = import.refs_import_function_simple(md, references, name);
		if (fSharpOption != null)
		{
			FSharpOption<MethodReference> fSharpOption2 = fSharpOption;
			return fSharpOption2.Value;
		}
		FSharpFunc<string, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("import method not found: %s"));
		fSharpFunc.Invoke(name);
		return null;
	}

	public static FSharpOption<TypeDefinition> find_mention_of_bridge_type(MethodDefinition m)
	{
		FSharpFunc<Instruction, FSharpOption<TypeDefinition>> chooser = new f@5835-13();
		return SeqModule.TryPick(chooser, m.Body.Instructions);
	}

	public static FSharpOption<TypeDefinition> find_mention_of_vararg_type(MethodDefinition m)
	{
		FSharpFunc<VariableDefinition, FSharpOption<TypeDefinition>> chooser = new f@5851-14();
		return SeqModule.TryPick(chooser, m.Body.Variables);
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static TypeReference copy_type_reference(CopyMap map, TypeReference tref)
	{
		TypeReference value;
		Tuple<bool, TypeReference> tuple = new Tuple<bool, TypeReference>(map.d_type@.TryGetValue(tref, out value), value);
		if (tuple.Item1)
		{
			return tuple.Item2;
		}
		return map.typs@.md.ImportReference(tref);
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static MethodReference copy_method_reference(CopyMap map, MethodReference tref)
	{
		MethodReference value;
		Tuple<bool, MethodReference> tuple = new Tuple<bool, MethodReference>(map.d_method@.TryGetValue(tref, out value), value);
		if (tuple.Item1)
		{
			return tuple.Item2;
		}
		return map.typs@.md.ImportReference(tref);
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static FieldReference copy_field_reference(CopyMap map, FieldReference tref)
	{
		FieldReference value;
		Tuple<bool, FieldReference> tuple = new Tuple<bool, FieldReference>(map.d_field@.TryGetValue(tref, out value), value);
		if (tuple.Item1)
		{
			return tuple.Item2;
		}
		return map.typs@.md.ImportReference(tref);
	}

	public static CopyMap make_map(cil.GenTypes typs)
	{
		FSharpFunc<Unit, Dictionary<TypeReference, TypeReference>> fSharpFunc = new make_d_type@5881();
		FSharpFunc<Unit, Dictionary<MethodReference, MethodReference>> fSharpFunc2 = new make_d_method@5893();
		FSharpFunc<Unit, Dictionary<FieldReference, FieldReference>> fSharpFunc3 = new make_d_field@5905();
		return new CopyMap(typs, fSharpFunc.Invoke(null), fSharpFunc2.Invoke(null), fSharpFunc3.Invoke(null));
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static MethodDefinition copy_method(cil.GenTypes typs, MethodDefinition m_from, CopyMap map)
	{
		TypeReference returnType = copy_type_reference(map, m_from.ReturnType);
		MethodDefinition methodDefinition = new MethodDefinition(m_from.Name, m_from.Attributes, returnType);
		List<ParameterDefinition> list = new List<ParameterDefinition>();
		Collection<ParameterDefinition> parameters = m_from.Parameters;
		Collection<ParameterDefinition>.Enumerator enumerator = parameters.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ParameterDefinition current = enumerator.Current;
				TypeReference parameterType = copy_type_reference(map, current.ParameterType);
				ParameterDefinition item = new ParameterDefinition(current.Name, Mono.Cecil.ParameterAttributes.None, parameterType);
				list.Add(item);
				methodDefinition.Parameters.Add(item);
			}
			Unit unit = null;
		}
		finally
		{
			((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		ILProcessor iLProcessor = methodDefinition.Body.GetILProcessor();
		List<VariableDefinition> list2 = new List<VariableDefinition>();
		Collection<VariableDefinition> variables = m_from.Body.Variables;
		Collection<VariableDefinition>.Enumerator enumerator2 = variables.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				VariableDefinition current2 = enumerator2.Current;
				TypeReference variableType = copy_type_reference(map, current2.VariableType);
				VariableDefinition item2 = new VariableDefinition(variableType);
				list2.Add(item2);
				methodDefinition.Body.Variables.Add(item2);
			}
			Unit unit2 = null;
		}
		finally
		{
			((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		Dictionary<Instruction, Instruction> dictionary = new Dictionary<Instruction, Instruction>();
		Collection<Instruction> instructions = m_from.Body.Instructions;
		Collection<Instruction>.Enumerator enumerator3 = instructions.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				Instruction current3 = enumerator3.Current;
				Instruction instruction;
				if (LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(current3.Operand, null))
				{
					instruction = current3;
				}
				else
				{
					object operand = current3.Operand;
					if (!(operand is MethodDefinition methodDefinition2))
					{
						if (!(operand is MethodReference methodReference))
						{
							if (!(operand is FieldDefinition fieldDefinition))
							{
								if (!(operand is FieldReference fieldReference))
								{
									if (!(operand is ParameterReference parameterReference))
									{
										if (!(operand is VariableDefinition variableDefinition))
										{
											if (!(operand is TypeReference typeReference))
											{
												if (!(operand is Mono.Cecil.CallSite callSite))
												{
													if (!(operand is string text))
													{
														if (!LanguagePrimitives.IntrinsicFunctions.TypeTestGeneric<sbyte>(operand))
														{
															if (!LanguagePrimitives.IntrinsicFunctions.TypeTestGeneric<int>(operand))
															{
																if (!LanguagePrimitives.IntrinsicFunctions.TypeTestGeneric<long>(operand))
																{
																	if (operand is Instruction instruction2)
																	{
																		Instruction target = instruction2;
																		instruction = iLProcessor.Create(current3.OpCode, target);
																	}
																	else
																	{
																		FSharpFunc<OpCode, FSharpFunc<object, FSharpFunc<string, Instruction>>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<OpCode, FSharpFunc<object, FSharpFunc<string, Instruction>>>, Unit, string, Instruction, Tuple<OpCode, object, string>>("OpCode is %A, Operand is %A, %s"));
																		instruction = FSharpFunc<OpCode, object>.InvokeFast(new instr_new@5983(clo), current3.OpCode, current3.Operand, current3.Operand.GetType().ToString());
																	}
																}
																else
																{
																	long value = (long)operand;
																	instruction = iLProcessor.Create(current3.OpCode, value);
																}
															}
															else
															{
																int value2 = (int)operand;
																instruction = iLProcessor.Create(current3.OpCode, value2);
															}
														}
														else
														{
															sbyte value3 = (sbyte)operand;
															instruction = iLProcessor.Create(current3.OpCode, value3);
														}
													}
													else
													{
														string value4 = text;
														instruction = iLProcessor.Create(current3.OpCode, value4);
													}
												}
												else
												{
													Mono.Cecil.CallSite site = callSite;
													instruction = iLProcessor.Create(current3.OpCode, site);
												}
											}
											else
											{
												TypeReference tref = typeReference;
												instruction = iLProcessor.Create(current3.OpCode, copy_type_reference(map, tref));
											}
										}
										else
										{
											VariableDefinition variableDefinition2 = variableDefinition;
											instruction = iLProcessor.Create(current3.OpCode, list2[variableDefinition2.Index]);
										}
									}
									else
									{
										ParameterReference parameterReference2 = parameterReference;
										ParameterDefinition parameter = list[parameterReference2.Index];
										instruction = iLProcessor.Create(current3.OpCode, parameter);
									}
								}
								else
								{
									FieldReference tref2 = fieldReference;
									FieldReference field = copy_field_reference(map, tref2);
									instruction = iLProcessor.Create(current3.OpCode, field);
								}
							}
							else
							{
								FieldDefinition tref3 = fieldDefinition;
								FieldReference field2 = copy_field_reference(map, tref3);
								instruction = iLProcessor.Create(current3.OpCode, field2);
							}
						}
						else
						{
							MethodReference tref4 = methodReference;
							MethodReference method = copy_method_reference(map, tref4);
							instruction = iLProcessor.Create(current3.OpCode, method);
						}
					}
					else
					{
						MethodDefinition tref5 = methodDefinition2;
						MethodReference method2 = copy_method_reference(map, tref5);
						instruction = iLProcessor.Create(current3.OpCode, method2);
					}
				}
				Instruction instruction3 = instruction;
				iLProcessor.Append(instruction3);
				dictionary.Add(current3, instruction3);
			}
			Unit unit3 = null;
		}
		finally
		{
			((IDisposable)enumerator3/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		Collection<ExceptionHandler> exceptionHandlers = m_from.Body.ExceptionHandlers;
		Collection<ExceptionHandler>.Enumerator enumerator4 = exceptionHandlers.GetEnumerator();
		try
		{
			while (enumerator4.MoveNext())
			{
				ExceptionHandler current4 = enumerator4.Current;
				ExceptionHandler exceptionHandler = new ExceptionHandler(ExceptionHandlerType.Catch);
				exceptionHandler.TryStart = dictionary[current4.TryStart];
				exceptionHandler.TryEnd = dictionary[current4.TryEnd];
				exceptionHandler.HandlerStart = dictionary[current4.HandlerStart];
				exceptionHandler.HandlerEnd = dictionary[current4.HandlerEnd];
				exceptionHandler.CatchType = typs.md.ImportReference(current4.CatchType);
				methodDefinition.Body.ExceptionHandlers.Add(exceptionHandler);
			}
			Unit unit4 = null;
			return methodDefinition;
		}
		finally
		{
			((IDisposable)enumerator4/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static void map_add_type(CopyMap map, TypeReference t_from, TypeReference t_to)
	{
		map.d_type@.Add(t_from, t_to);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static void map_add_method(CopyMap map, MethodReference t_from, MethodReference t_to)
	{
		map.d_method@.Add(t_from, t_to);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static void map_add_field(CopyMap map, FieldReference t_from, FieldReference t_to)
	{
		map.d_field@.Add(t_from, t_to);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static void copy_type(CopyMap map, cil.GenTypes typs, TypeDefinition t_from)
	{
		if (t_from.Name.EndsWith("_bridge"))
		{
			Collection<FieldDefinition> fields = t_from.Fields;
			FSharpFunc<FieldDefinition, FSharpOption<FieldDefinition>> chooser = new f_handles@6011();
			Collection<FieldDefinition> source = fields;
			FieldDefinition fieldDefinition = SeqModule.Pick(chooser, source);
			copy_type(map, typs, (TypeDefinition)(object)fieldDefinition.FieldType);
		}
		TypeDefinition typeDefinition = new TypeDefinition(t_from.Namespace, t_from.Name, t_from.Attributes, typs.md.ImportReference(t_from.BaseType));
		map_add_type(map, t_from, typeDefinition);
		Collection<GenericParameter> genericParameters = t_from.GenericParameters;
		Collection<GenericParameter>.Enumerator enumerator = genericParameters.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				GenericParameter current = enumerator.Current;
				GenericParameter genericParameter = new GenericParameter(typeDefinition);
				map_add_type(map, current, genericParameter);
				typeDefinition.GenericParameters.Add(genericParameter);
			}
			Unit unit = null;
		}
		finally
		{
			((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		Collection<TypeDefinition> nestedTypes = t_from.NestedTypes;
		Collection<TypeDefinition>.Enumerator enumerator2 = nestedTypes.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				TypeDefinition current2 = enumerator2.Current;
				FSharpFunc<TypeDefinition, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<TypeDefinition, Unit>, TextWriter, Unit, Unit, TypeDefinition>("TODO copy sub type: %A"));
				TypeDefinition func = current2;
				fSharpFunc.Invoke(func);
			}
			Unit unit2 = null;
		}
		finally
		{
			((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		Collection<FieldDefinition> fields2 = t_from.Fields;
		Collection<FieldDefinition>.Enumerator enumerator3 = fields2.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				FieldDefinition current3 = enumerator3.Current;
				FieldDefinition fieldDefinition2 = new FieldDefinition(current3.Name, current3.Attributes, copy_type_reference(map, current3.FieldType));
				map_add_field(map, current3, fieldDefinition2);
				typeDefinition.Fields.Add(fieldDefinition2);
			}
			Unit unit3 = null;
		}
		finally
		{
			((IDisposable)enumerator3/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		Collection<MethodDefinition> methods = t_from.Methods;
		Collection<MethodDefinition>.Enumerator enumerator4 = methods.GetEnumerator();
		try
		{
			while (enumerator4.MoveNext())
			{
				MethodDefinition current4 = enumerator4.Current;
				MethodDefinition methodDefinition = copy_method(typs, current4, map);
				map_add_method(map, current4, methodDefinition);
				typeDefinition.Methods.Add(methodDefinition);
			}
			Unit unit4 = null;
		}
		finally
		{
			((IDisposable)enumerator4/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		typs.md.Types.Add(typeDefinition);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1, 1, 1 })]
	public static MethodRefInternal[] create_methods(cil.GenTypes typs, GenSyms syms, TypeDefinition container, LLVMModuleRef m, TypeDefinition[] refs, Dictionary<string, MethodDefinition> cpref, CopyMap map)
	{
		List<MethodRefInternal> list = new List<MethodRefInternal>();
		List<LLVMValueRef> list2 = sgllvm.get_functions(m);
		List<LLVMValueRef>.Enumerator enumerator = list2.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				LLVMValueRef current = enumerator.Current;
				if (current.Name.StartsWith("llvm.lifetime.") || string.Equals(current.Name, "llvm.dbg.declare") || current.Name.StartsWith("llvm.expect.") || current.Name.StartsWith("llvm.memcpy.") || current.Name.StartsWith("llvm.memmove.") || current.Name.StartsWith("llvm.memset.") || string.Equals(current.Name, "llvm.aarch64.hint") || current.Name.StartsWith("llvm.ceil.") || current.Name.StartsWith("llvm.floor.") || current.Name.StartsWith("llvm.copysign.") || current.Name.StartsWith("llvm.trunc.") || current.Name.StartsWith("llvm.log2.") || current.Name.StartsWith("llvm.powi.") || current.Name.StartsWith("llvm.log.") || current.Name.StartsWith("llvm.minnum.") || current.Name.StartsWith("llvm.maxnum.") || current.Name.StartsWith("llvm.sqrt.") || current.Name.StartsWith("llvm.pow.") || current.Name.StartsWith("llvm.round.") || current.Name.StartsWith("llvm.exp.") || current.Name.StartsWith("llvm.fabs.") || current.Name.StartsWith("llvm.sin.") || current.Name.StartsWith("llvm.cos.") || is_llvm_overflow_intrinsic(current.Name) || is_llvm_sat_intrinsic(current.Name) || current.Name.StartsWith("llvm.experimental.vector.reduce.") || current.Name.StartsWith("llvm.fshl.") || current.Name.StartsWith("llvm.fshr.") || current.Name.StartsWith("llvm.bswap.") || current.Name.StartsWith("llvm.ctlz.") || current.Name.StartsWith("llvm.cttz.") || current.Name.StartsWith("llvm.ctpop.") || string.Equals(current.Name, "llvm.trap") || string.Equals(current.Name, "llvm.sideeffect") || string.Equals(current.Name, "llvm.assume") || string.Equals(current.Name, "llvm.va_start") || string.Equals(current.Name, "llvm.va_copy") || string.Equals(current.Name, "llvm.va_end") || string.Equals(current.Name, "__sg_checkpoint"))
				{
					continue;
				}
				if (current.IsDeclaration)
				{
					MethodDefinition value = null;
					Tuple<bool, MethodDefinition> tuple = new Tuple<bool, MethodDefinition>(cpref.TryGetValue(current.Name, out value), value);
					if (tuple.Item1)
					{
						MethodDefinition item = tuple.Item2;
						FSharpOption<TypeDefinition> fSharpOption = find_mention_of_bridge_type(item);
						if (fSharpOption != null)
						{
							FSharpOption<TypeDefinition> fSharpOption2 = fSharpOption;
							TypeDefinition value2 = fSharpOption2.Value;
							copy_type(map, typs, value2);
						}
						FSharpOption<TypeDefinition> fSharpOption3 = find_mention_of_vararg_type(item);
						if (fSharpOption3 != null)
						{
							FSharpOption<TypeDefinition> fSharpOption4 = fSharpOption3;
							TypeDefinition value3 = fSharpOption4.Value;
							copy_type(map, typs, value3);
						}
						MethodDefinition methodDefinition = copy_method(typs, item, map);
						syms.d_methods@.Add(current, methodDefinition);
						container.Methods.Add(methodDefinition);
					}
					else
					{
						FSharpOption<MethodReference> fSharpOption5 = SeqModule.TryFind(new create_methods@6148(current), map.d_method@.Keys);
						if (fSharpOption5 != null)
						{
							FSharpOption<MethodReference> fSharpOption6 = fSharpOption5;
							MethodReference value4 = fSharpOption6.Value;
							MethodReference value5 = map.d_method@[value4];
							syms.d_methods@.Add(current, value5);
						}
						else
						{
							MethodReference value6 = import_method(typs.md, current.Name, refs);
							syms.d_methods@.Add(current, value6);
						}
					}
				}
				else
				{
					MethodDefinition methodDefinition2 = create_method(typs, current);
					Dictionary<LLVMValueRef, ParameterDefinition> dictionary = new Dictionary<LLVMValueRef, ParameterDefinition>();
					LLVMValueRef[] array = current.Params;
					for (int i = 0; i < array.Length; i++)
					{
						LLVMValueRef key = array[i];
						cil.FirstClassType vt = llvm_type_to_firstclass_type(key.TypeOf);
						TypeReference parameterType = cil.firstclass_type_to_cecil_type(typs, vt);
						ParameterDefinition parameterDefinition = new ParameterDefinition(key.Name, Mono.Cecil.ParameterAttributes.None, parameterType);
						methodDefinition2.Parameters.Add(parameterDefinition);
						dictionary.Add(key, parameterDefinition);
					}
					object obj;
					if (current.TypeOf.IsFunctionVarArg || current.TypeOf.ToString().Contains("..."))
					{
						TypeReference parameterType2 = cil.firstclass_type_to_cecil_type(typs, cil.FirstClassType.Ptr);
						ParameterDefinition parameterDefinition2 = new ParameterDefinition("extra_args", Mono.Cecil.ParameterAttributes.None, parameterType2);
						methodDefinition2.Parameters.Add(parameterDefinition2);
						obj = parameterDefinition2;
					}
					else
					{
						obj = null;
					}
					ParameterDefinition extra = (ParameterDefinition)obj;
					syms.d_methods@.Add(current, methodDefinition2);
					container.Methods.Add(methodDefinition2);
					MethodRefInternal item2 = new MethodRefInternal(current, methodDefinition2, dictionary, extra);
					list.Add(item2);
				}
			}
			Unit unit = null;
		}
		finally
		{
			((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		return ArrayModule.OfSeq(list);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1 })]
	public static void create_globals(cil.GenTypes typs, GenSyms syms, TypeDefinition container, LLVMModuleRef m, TypeDefinition[] refs)
	{
		List<LLVMValueRef> list = sgllvm.get_globals(m);
		List<LLVMValueRef> list2 = list;
		List<LLVMValueRef>.Enumerator enumerator = list2.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				LLVMValueRef current = enumerator.Current;
				object obj;
				if (current.IsDeclaration)
				{
					FieldReference fieldReference = import_global(typs.md, current, refs);
					obj = fieldReference;
				}
				else
				{
					FieldDefinition fieldDefinition = create_global(typs, current);
					container.Fields.Add(fieldDefinition);
					obj = fieldDefinition;
				}
				FieldReference value = (FieldReference)obj;
				syms.d_globals@.Add(current, value);
			}
			Unit unit = null;
		}
		finally
		{
			((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1 })]
	public static MethodDefinition gen_cctor(cil.GenTypes typs, GenSyms syms, FSharpOption<MethodDefinition> data_setup, TraceEnterExit trace_enter_exit, TraceInstruction trace_instr)
	{
		MethodDefinition methodDefinition = new MethodDefinition(".cctor", Mono.Cecil.MethodAttributes.Private | Mono.Cecil.MethodAttributes.Static | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.RTSpecialName, typs.md.TypeSystem.Void);
		cil.CilWriter cilWriter = new cil.CilWriter();
		if (want_trace_enter_exit(trace_enter_exit))
		{
			cilWriter.Append(cil.MyInstruction.NewLdstr("cctor"));
			cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("trace_enter")));
		}
		if (data_setup != null)
		{
			FSharpOption<MethodDefinition> fSharpOption = data_setup;
			MethodDefinition value = fSharpOption.Value;
			cilWriter.Append(cil.MyInstruction.NewCall(value));
		}
		MethodReference item = typs.md.ImportReference(typeof(Marshal).GetMethod("AllocHGlobal", new Type[1] { typeof(int) }));
		Dictionary<LLVMValueRef, FieldReference> d_globals@ = syms.d_globals@;
		Dictionary<LLVMValueRef, FieldReference>.Enumerator enumerator = d_globals@.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<LLVMValueRef, FieldReference> current = enumerator.Current;
				LLVMValueRef key = current.Key;
				FieldReference value2 = current.Value;
				cil.FirstClassType t = global_type(key);
				if (!key.IsDeclaration)
				{
					int item2 = cil.get_sizeof(t);
					cilWriter.Append(cil.MyInstruction.NewLdc_I4(item2));
					cilWriter.Append(cil.MyInstruction.NewCall(item));
					cilWriter.Append(cil.MyInstruction.NewStsfld(value2));
					if (want_trace_instruction(trace_instr))
					{
						cilWriter.Append(cil.MyInstruction.NewLdsfld(value2));
						cilWriter.Append(cil.MyInstruction.NewLdc_I4(item2));
						cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("trace_globala")));
					}
				}
			}
			Unit unit = null;
		}
		finally
		{
			((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		cil.Label label = cilWriter.NewLabel();
		Dictionary<LLVMValueRef, FieldReference> d_globals@2 = syms.d_globals@;
		Dictionary<LLVMValueRef, FieldReference>.Enumerator enumerator2 = d_globals@2.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<LLVMValueRef, FieldReference> current2 = enumerator2.Current;
				LLVMValueRef key2 = current2.Key;
				FieldReference value3 = current2.Value;
				cil.FirstClassType typ = global_type(key2);
				if (!key2.IsDeclaration)
				{
					FSharpTypeFunc f_get_instr_value = new f_get_instr_value@6338-1();
					Dictionary<LLVMBasicBlockRef, Block> dictionary = new Dictionary<LLVMBasicBlockRef, Block>();
					FSharpFunc<cil.CilWriter, FSharpFunc<InstructionValue, FSharpFunc<InstructionDest, Unit>>> f_load_instr_value_to_dest = new f_load_instr_value_to_dest@6341-1(typs, syms, label, f_get_instr_value);
					store_constant_at(syms, cilWriter, typ, new gen_cctor@6352(cilWriter, value3), f_load_instr_value_to_dest, key2.Initializer);
				}
			}
			Unit unit2 = null;
		}
		finally
		{
			((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		if (want_trace_enter_exit(trace_enter_exit))
		{
			cilWriter.Append(cil.MyInstruction.NewLdstr("cctor"));
			cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("trace_exit")));
		}
		cilWriter.Append(cil.MyInstruction.NewLabel(label));
		cilWriter.Append(cil.MyInstruction.Ret);
		cilWriter.Finish(methodDefinition, typs);
		return methodDefinition;
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static void gen_main(cil.GenTypes typs, GenSyms syms, TypeDefinition container)
	{
		MethodDefinition methodDefinition = new MethodDefinition("Main", Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.Static, typs.md.TypeSystem.Int32);
		ParameterDefinition item = new ParameterDefinition(new ArrayType(typs.md.TypeSystem.String));
		methodDefinition.Parameters.Add(item);
		cil.CilWriter cilWriter = new cil.CilWriter();
		MethodDefinition methodDefinition2 = SeqModule.Find(new m_main@6449(), container.Methods);
		if (LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(methodDefinition2.ReturnType, typs.md.TypeSystem.Void) && methodDefinition2.Parameters.Count == 0)
		{
			cilWriter.Append(cil.MyInstruction.NewLdftn(methodDefinition2));
			cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("my_main_rs")));
		}
		else if (LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(methodDefinition2.ReturnType, typs.md.TypeSystem.IntPtr) && methodDefinition2.Parameters.Count == 0)
		{
			cilWriter.Append(cil.MyInstruction.NewLdftn(methodDefinition2));
			cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("my_main_rs_result")));
		}
		else if (LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(methodDefinition2.ReturnType, typs.md.TypeSystem.Void) && methodDefinition2.Parameters.Count == 1)
		{
			cilWriter.Append(cil.MyInstruction.NewLdftn(methodDefinition2));
			cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("my_main_rs")));
		}
		else if (LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(methodDefinition2.ReturnType, typs.md.TypeSystem.Int32) && methodDefinition2.Parameters.Count == 0)
		{
			cilWriter.Append(cil.MyInstruction.NewLdftn(methodDefinition2));
			cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("my_main_c_noargs")));
		}
		else if (LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(methodDefinition2.ReturnType, typs.md.TypeSystem.Int32) && methodDefinition2.Parameters.Count == 2 && LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(methodDefinition2.Parameters[0].ParameterType, typs.md.TypeSystem.Int32) && (methodDefinition2.Parameters[1].ParameterType.IsPointer || LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(methodDefinition2.Parameters[1].ParameterType, typs.md.TypeSystem.IntPtr)))
		{
			cilWriter.Append(cil.MyInstruction.NewLdftn(methodDefinition2));
			cilWriter.Append(cil.MyInstruction.NewLdarg(item));
			cilWriter.Append(cil.MyInstruction.NewCall(syms.f_get_rt@.Invoke("my_main_c")));
		}
		else
		{
			FSharpFunc<MethodDefinition, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<MethodDefinition, Unit>, Unit, string, Unit, MethodDefinition>("unknown main signature: %A"));
			MethodDefinition func = methodDefinition2;
			fSharpFunc.Invoke(func);
		}
		cilWriter.Append(cil.MyInstruction.Ret);
		cilWriter.Finish(methodDefinition, typs);
		container.Methods.Add(methodDefinition);
		typs.md.EntryPoint = methodDefinition;
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static RtCopyType rt_add_type(RtCopyMap map, TypeDefinition t)
	{
		IEqualityComparer<MethodDefinition> comparer = new comparer@6493-3();
		Dictionary<MethodDefinition, MethodDefinition> dictionary = new Dictionary<MethodDefinition, MethodDefinition>(comparer);
		Dictionary<MethodDefinition, MethodDefinition> d_method = dictionary;
		IEqualityComparer<FieldDefinition> comparer2 = new comparer@6505-4();
		Dictionary<FieldDefinition, FieldDefinition> dictionary2 = new Dictionary<FieldDefinition, FieldDefinition>(comparer2);
		Dictionary<FieldDefinition, FieldDefinition> d_field = dictionary2;
		TypeDefinition tdef = new TypeDefinition(t.Namespace, t.Name, t.Attributes, map.typs@.md.ImportReference(t.BaseType));
		RtCopyType rtCopyType = new RtCopyType(tdef, d_method, d_field);
		map.d_type@.Add(t, rtCopyType);
		return rtCopyType;
	}

	public static RtCopyMap rt_make_map(cil.GenTypes typs)
	{
		IEqualityComparer<TypeDefinition> comparer = new comparer@6534-5();
		Dictionary<TypeDefinition, RtCopyType> dictionary = new Dictionary<TypeDefinition, RtCopyType>(comparer);
		Dictionary<TypeDefinition, RtCopyType> d_type = dictionary;
		return new RtCopyMap(typs, d_type);
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static RtCopyType rt_find_or_copy_type(RtCopyMap map, TypeDefinition t)
	{
		RtCopyType value;
		Tuple<bool, RtCopyType> tuple = new Tuple<bool, RtCopyType>(map.d_type@.TryGetValue(t, out value), value);
		if (tuple.Item1)
		{
			return tuple.Item2;
		}
		RtCopyType rtCopyType = rt_add_type(map, t);
		map.typs@.md.Types.Add(rtCopyType.tdef@);
		return rtCopyType;
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static FieldDefinition rt_copy_field(RtCopyMap map, FieldDefinition f_from)
	{
		TypeReference fieldType = map.typs@.md.ImportReference(f_from.FieldType);
		FieldDefinition fieldDefinition = new FieldDefinition(f_from.Name, f_from.Attributes, fieldType);
		RtCopyType rtCopyType = map.d_type@[f_from.DeclaringType];
		rtCopyType.tdef@.Fields.Add(fieldDefinition);
		return fieldDefinition;
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static MethodDefinition rt_copy_method(RtCopyMap map, MethodDefinition m_from)
	{
		TypeReference returnType = map.typs@.md.ImportReference(m_from.ReturnType);
		MethodDefinition methodDefinition = new MethodDefinition(m_from.Name, m_from.Attributes, returnType);
		List<ParameterDefinition> list = new List<ParameterDefinition>();
		Collection<ParameterDefinition> parameters = m_from.Parameters;
		Collection<ParameterDefinition>.Enumerator enumerator = parameters.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ParameterDefinition current = enumerator.Current;
				TypeReference parameterType = map.typs@.md.ImportReference(current.ParameterType);
				ParameterDefinition item = new ParameterDefinition(current.Name, Mono.Cecil.ParameterAttributes.None, parameterType);
				list.Add(item);
				methodDefinition.Parameters.Add(item);
			}
			Unit unit = null;
		}
		finally
		{
			((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		ILProcessor iLProcessor = methodDefinition.Body.GetILProcessor();
		List<VariableDefinition> list2 = new List<VariableDefinition>();
		Collection<VariableDefinition> variables = m_from.Body.Variables;
		Collection<VariableDefinition>.Enumerator enumerator2 = variables.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				VariableDefinition current2 = enumerator2.Current;
				TypeReference variableType = map.typs@.md.ImportReference(current2.VariableType);
				VariableDefinition item2 = new VariableDefinition(variableType);
				list2.Add(item2);
				methodDefinition.Body.Variables.Add(item2);
			}
			Unit unit2 = null;
		}
		finally
		{
			((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		Collection<Instruction> instructions = m_from.Body.Instructions;
		Collection<Instruction>.Enumerator enumerator3 = instructions.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				Instruction current3 = enumerator3.Current;
				if (LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(current3.Operand, null))
				{
					iLProcessor.Append(current3);
					continue;
				}
				object operand = current3.Operand;
				Instruction instruction2;
				if (!(operand is MethodDefinition methodDefinition2))
				{
					if (!(operand is MethodReference methodReference))
					{
						if (!(operand is FieldDefinition fieldDefinition))
						{
							if (!(operand is FieldReference fieldReference))
							{
								if (!(operand is ParameterReference parameterReference))
								{
									if (!(operand is VariableDefinition variableDefinition))
									{
										if (!(operand is TypeReference typeReference))
										{
											if (!(operand is Mono.Cecil.CallSite callSite))
											{
												if (!(operand is string text))
												{
													if (!LanguagePrimitives.IntrinsicFunctions.TypeTestGeneric<sbyte>(operand))
													{
														if (!LanguagePrimitives.IntrinsicFunctions.TypeTestGeneric<int>(operand))
														{
															if (!LanguagePrimitives.IntrinsicFunctions.TypeTestGeneric<long>(operand))
															{
																if (!(operand is Instruction instruction))
																{
																	if (operand is Instruction[] array)
																	{
																		Instruction[] targets = array;
																		instruction2 = iLProcessor.Create(current3.OpCode, targets);
																	}
																	else
																	{
																		FSharpFunc<OpCode, FSharpFunc<object, FSharpFunc<string, Instruction>>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<OpCode, FSharpFunc<object, FSharpFunc<string, Instruction>>>, Unit, string, Instruction, Tuple<OpCode, object, string>>("OpCode is %A, Operand is %A, %s"));
																		instruction2 = FSharpFunc<OpCode, object>.InvokeFast(new inst@6641(clo), current3.OpCode, current3.Operand, current3.Operand.GetType().ToString());
																	}
																}
																else
																{
																	Instruction target = instruction;
																	instruction2 = iLProcessor.Create(current3.OpCode, target);
																}
															}
															else
															{
																long value = (long)operand;
																instruction2 = iLProcessor.Create(current3.OpCode, value);
															}
														}
														else
														{
															int value2 = (int)operand;
															instruction2 = iLProcessor.Create(current3.OpCode, value2);
														}
													}
													else
													{
														sbyte value3 = (sbyte)operand;
														instruction2 = iLProcessor.Create(current3.OpCode, value3);
													}
												}
												else
												{
													string value4 = text;
													instruction2 = iLProcessor.Create(current3.OpCode, value4);
												}
											}
											else
											{
												Mono.Cecil.CallSite site = callSite;
												instruction2 = iLProcessor.Create(current3.OpCode, site);
											}
										}
										else
										{
											TypeReference type = typeReference;
											TypeReference type2 = map.typs@.md.ImportReference(type);
											instruction2 = iLProcessor.Create(current3.OpCode, type2);
										}
									}
									else
									{
										VariableDefinition variableDefinition2 = variableDefinition;
										instruction2 = iLProcessor.Create(current3.OpCode, list2[variableDefinition2.Index]);
									}
								}
								else
								{
									ParameterReference parameterReference2 = parameterReference;
									ParameterDefinition parameter = list[parameterReference2.Index];
									instruction2 = iLProcessor.Create(current3.OpCode, parameter);
								}
							}
							else
							{
								FieldReference field = fieldReference;
								FieldReference field2 = map.typs@.md.ImportReference(field);
								instruction2 = iLProcessor.Create(current3.OpCode, field2);
							}
						}
						else
						{
							FieldDefinition f = fieldDefinition;
							FieldReference field3 = rt_find_or_copy_field(map, f);
							instruction2 = iLProcessor.Create(current3.OpCode, field3);
						}
					}
					else
					{
						MethodReference method = methodReference;
						MethodReference method2 = map.typs@.md.ImportReference(method);
						instruction2 = iLProcessor.Create(current3.OpCode, method2);
					}
				}
				else
				{
					MethodDefinition m = methodDefinition2;
					MethodReference method3 = rt_find_or_copy_method(map, m);
					instruction2 = iLProcessor.Create(current3.OpCode, method3);
				}
				Instruction instruction3 = instruction2;
				iLProcessor.Append(instruction3);
			}
			Unit unit3 = null;
		}
		finally
		{
			((IDisposable)enumerator3/*cast due to .constrained prefix*/).Dispose();
			_ = null;
		}
		RtCopyType rtCopyType = map.d_type@[m_from.DeclaringType];
		rtCopyType.tdef@.Methods.Add(methodDefinition);
		return methodDefinition;
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static MethodDefinition rt_find_or_copy_method(RtCopyMap map, MethodDefinition m)
	{
		RtCopyType rtCopyType = rt_find_or_copy_type(map, m.DeclaringType);
		MethodDefinition value = null;
		Tuple<bool, MethodDefinition> tuple = new Tuple<bool, MethodDefinition>(rtCopyType.d_method@.TryGetValue(m, out value), value);
		if (tuple.Item1)
		{
			return tuple.Item2;
		}
		MethodDefinition methodDefinition = rt_copy_method(map, m);
		rtCopyType.d_method@.Add(m, methodDefinition);
		return methodDefinition;
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static FieldDefinition rt_find_or_copy_field(RtCopyMap map, FieldDefinition f)
	{
		RtCopyType rtCopyType = rt_find_or_copy_type(map, f.DeclaringType);
		FieldDefinition value = null;
		Tuple<bool, FieldDefinition> tuple = new Tuple<bool, FieldDefinition>(rtCopyType.d_field@.TryGetValue(f, out value), value);
		if (tuple.Item1)
		{
			return tuple.Item2;
		}
		FieldDefinition fieldDefinition = rt_copy_field(map, f);
		rtCopyType.d_field@.Add(f, fieldDefinition);
		return fieldDefinition;
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1, 1 })]
	public static void gen_assembly(CompilerSettings settings, LLVMModuleRef m, string assembly_name, string classname, Version ver, Stream dest)
	{
		llvm_module = m;
		AssemblyDefinition assemblyDefinition = AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition(assembly_name, ver), assembly_name, ModuleKind.Dll);
		ModuleDefinition mainModule = assemblyDefinition.MainModule;
		int num = classname.LastIndexOf(".");
		Tuple<string, string> tuple = ((num >= 0) ? new Tuple<string, string>(classname.Substring(0, num), classname.Substring(num + 1)) : new Tuple<string, string>("", classname));
		string item = tuple.Item1;
		string item2 = tuple.Item2;
		TypeDefinition typeDefinition = new TypeDefinition(item, item2, Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Abstract | Mono.Cecil.TypeAttributes.Sealed, mainModule.TypeSystem.Object);
		mainModule.Types.Add(typeDefinition);
		Dictionary<cil.StructType, TypeReference> d_structs = new Dictionary<cil.StructType, TypeReference>();
		Dictionary<cil.ArrayType, TypeReference> d_arrays = new Dictionary<cil.ArrayType, TypeReference>();
		Dictionary<uint, TypeReference> d_funnyints = new Dictionary<uint, TypeReference>();
		Dictionary<cil.VectorType, TypeReference> d_vectors = new Dictionary<cil.VectorType, TypeReference>();
		Dictionary<int, TypeReference> d_vararg = new Dictionary<int, TypeReference>();
		cil.GenTypes genTypes = new cil.GenTypes(item, mainModule, d_structs, d_arrays, d_funnyints, d_vectors, d_vararg);
		Dictionary<LLVMValueRef, MethodReference> d_methods = new Dictionary<LLVMValueRef, MethodReference>();
		Dictionary<LLVMValueRef, FieldReference> d_globals = new Dictionary<LLVMValueRef, FieldReference>();
		MethodReference ctor_exception = genTypes.md.ImportReference(typeof(Exception).GetConstructor(new Type[1] { typeof(string) }));
		RtSetting ref_rt@ = settings.ref_rt@;
		FSharpFunc<string, MethodReference> fSharpFunc2;
		if (!(ref_rt@ is RtSetting.Reference))
		{
			RtSetting.Copy copy = (RtSetting.Copy)ref_rt@;
			TypeDefinition typeDefinition2 = copy.item;
			Dictionary<string, MethodDefinition> dictionary = new Dictionary<string, MethodDefinition>();
			Collection<MethodDefinition> methods = typeDefinition2.Methods;
			Collection<MethodDefinition>.Enumerator enumerator = methods.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MethodDefinition current = enumerator.Current;
					dictionary.Add(current.Name, current);
				}
				Unit unit = null;
			}
			finally
			{
				((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
			RtCopyMap map = rt_make_map(genTypes);
			FSharpFunc<string, MethodReference> fSharpFunc = new f@6782-15(dictionary, map);
			fSharpFunc2 = fSharpFunc;
		}
		else
		{
			RtSetting.Reference reference = (RtSetting.Reference)ref_rt@;
			TypeDefinition typeDefinition3 = reference.item;
			Dictionary<string, MethodReference> dictionary2 = new Dictionary<string, MethodReference>();
			Collection<MethodDefinition> methods2 = typeDefinition3.Methods;
			Collection<MethodDefinition>.Enumerator enumerator2 = methods2.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					MethodDefinition current2 = enumerator2.Current;
					MethodReference methodReference = genTypes.md.ImportReference(current2);
					dictionary2.Add(methodReference.Name, methodReference);
				}
				Unit unit2 = null;
			}
			finally
			{
				((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
			FSharpFunc<string, MethodReference> fSharpFunc3 = new f@6795-16(dictionary2);
			fSharpFunc2 = fSharpFunc3;
		}
		FSharpFunc<string, MethodReference> f_get_rt = fSharpFunc2;
		GenSyms syms = new GenSyms(ctor_exception, d_globals, d_methods, f_get_rt);
		create_globals(genTypes, syms, typeDefinition, m, settings.references@);
		CopyMap map2 = make_map(genTypes);
		Dictionary<string, MethodDefinition> dictionary3 = new Dictionary<string, MethodDefinition>();
		FSharpOption<TypeDefinition> cpref@ = settings.cpref@;
		if (cpref@ != null)
		{
			FSharpOption<TypeDefinition> fSharpOption = cpref@;
			TypeDefinition value = fSharpOption.Value;
			Collection<MethodDefinition> methods3 = value.Methods;
			Collection<MethodDefinition>.Enumerator enumerator3 = methods3.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					MethodDefinition current3 = enumerator3.Current;
					MethodDefinition value2 = null;
					Tuple<bool, MethodDefinition> tuple2 = new Tuple<bool, MethodDefinition>(dictionary3.TryGetValue(current3.Name, out value2), value2);
					if (!tuple2.Item1)
					{
						dictionary3.Add(current3.Name, current3);
					}
				}
				Unit unit3 = null;
			}
			finally
			{
				((IDisposable)enumerator3/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
		}
		MethodRefInternal[] array = create_methods(genTypes, syms, typeDefinition, m, settings.references@, dictionary3, map2);
		FSharpFunc<ModuleDefinition, FSharpFunc<Assembly, FSharpFunc<string, FSharpFunc<string, FSharpFunc<Type[], MethodReference>>>>> fSharpFunc4 = new find_method@6829();
		MethodRefInternal[] array2 = array;
		foreach (MethodRefInternal mi in array2)
		{
			gen_function_code(genTypes, syms, TraceEnterExit.No, mi);
		}
		MethodDefinition item3 = gen_cctor(genTypes, syms, null, TraceEnterExit.No, TraceInstruction.No);
		typeDefinition.Methods.Add(item3);
		if (settings.output_type@.Tag != 0)
		{
			gen_main(genTypes, syms, typeDefinition);
		}
		assemblyDefinition.Write(dest);
	}
}
