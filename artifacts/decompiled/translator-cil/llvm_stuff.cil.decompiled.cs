using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace llvm_stuff;

[CompilationMapping(SourceConstructFlags.Module)]
public static class cil
{
	[Serializable]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[DebuggerDisplay("{__DebugDisplay(),nq}")]
	[CompilationMapping(SourceConstructFlags.SumType)]
	public sealed class StackItemType : IEquatable<StackItemType>, IStructuralEquatable, IComparable<StackItemType>, IComparable, IStructuralComparable
	{
		public static class Tags
		{
			public const int I = 0;

			public const int O = 1;

			public const int I4 = 2;

			public const int I8 = 3;

			public const int R4 = 4;

			public const int R8 = 5;

			public const int S = 6;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly StackItemType _unique_I = new StackItemType(0);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly StackItemType _unique_O = new StackItemType(1);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly StackItemType _unique_I4 = new StackItemType(2);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly StackItemType _unique_I8 = new StackItemType(3);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly StackItemType _unique_R4 = new StackItemType(4);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly StackItemType _unique_R8 = new StackItemType(5);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly StackItemType _unique_S = new StackItemType(6);

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
		public static StackItemType I
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
			get
			{
				return _unique_I;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsI
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
		public static StackItemType O
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
			get
			{
				return _unique_O;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsO
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
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static StackItemType I4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 2)]
			get
			{
				return _unique_I4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsI4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static StackItemType I8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 3)]
			get
			{
				return _unique_I8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsI8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 3;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static StackItemType R4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 4)]
			get
			{
				return _unique_R4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsR4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static StackItemType R8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 5)]
			get
			{
				return _unique_R8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsR8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 5;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static StackItemType S
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 6)]
			get
			{
				return _unique_S;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsS
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 6;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal StackItemType(int _tag)
		{
			this._tag = _tag;
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<StackItemType, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<StackItemType, string>, Unit, string, string, StackItemType>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(StackItemType obj)
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
			return CompareTo((StackItemType)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			StackItemType stackItemType = (StackItemType)obj;
			if (this != null)
			{
				if ((StackItemType)obj != null)
				{
					int num = _tag;
					int num2 = stackItemType._tag;
					if (num == num2)
					{
						return 0;
					}
					return num - num2;
				}
				return 1;
			}
			if ((StackItemType)obj != null)
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
				if (obj is StackItemType stackItemType)
				{
					StackItemType stackItemType2 = stackItemType;
					int num = _tag;
					int num2 = stackItemType2._tag;
					return num == num2;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(StackItemType obj)
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
			if (obj is StackItemType obj2)
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
	public abstract class GeneralType : IEquatable<GeneralType>, IStructuralEquatable
	{
		public static class Tags
		{
			public const int VarArgType = 0;

			public const int SystemType = 1;

			public const int TypeReference = 2;

			public const int FirstClassType = 3;
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(VarArgType@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class VarArgType : GeneralType
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly int item;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public int Item
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
			internal VarArgType(int item)
				: base(0)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(SystemType@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class SystemType : GeneralType
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Type item;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Type Item
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
			internal SystemType(Type item)
				: base(1)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(TypeReference@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class TypeReference : GeneralType
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Mono.Cecil.TypeReference item;

			[CompilationMapping(SourceConstructFlags.Field, 2, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Mono.Cecil.TypeReference Item
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
			internal TypeReference(Mono.Cecil.TypeReference item)
				: base(2)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(FirstClassType@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class FirstClassType : GeneralType
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.FirstClassType item;

			[CompilationMapping(SourceConstructFlags.Field, 3, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.FirstClassType Item
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
			internal FirstClassType(cil.FirstClassType item)
				: base(3)
			{
				this.item = item;
			}
		}

		[SpecialName]
		internal class VarArgType@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal VarArgType _obj;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public int Item
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
			public VarArgType@DebugTypeProxy(VarArgType obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class SystemType@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal SystemType _obj;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Type Item
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
			public SystemType@DebugTypeProxy(SystemType obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class TypeReference@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal TypeReference _obj;

			[CompilationMapping(SourceConstructFlags.Field, 2, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Mono.Cecil.TypeReference Item
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
			public TypeReference@DebugTypeProxy(TypeReference obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class FirstClassType@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal FirstClassType _obj;

			[CompilationMapping(SourceConstructFlags.Field, 3, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.FirstClassType Item
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
			public FirstClassType@DebugTypeProxy(FirstClassType obj)
			{
				_obj = obj;
			}
		}

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
		public bool IsVarArgType
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
		public bool IsSystemType
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
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsTypeReference
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsFirstClassType
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 3;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal GeneralType(int _tag)
		{
			this._tag = _tag;
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
		public static GeneralType NewVarArgType(int item)
		{
			return new VarArgType(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
		public static GeneralType NewSystemType(Type item)
		{
			return new SystemType(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 2)]
		public static GeneralType NewTypeReference(Mono.Cecil.TypeReference item)
		{
			return new TypeReference(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 3)]
		public static GeneralType NewFirstClassType(cil.FirstClassType item)
		{
			return new FirstClassType(item);
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<GeneralType, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<GeneralType, string>, Unit, string, string, GeneralType>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public virtual sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				switch (Tag)
				{
				default:
				{
					VarArgType varArgType = (VarArgType)this;
					num = 0;
					return -1640531527 + (varArgType.item + ((num << 6) + (num >> 2)));
				}
				case 1:
				{
					SystemType systemType = (SystemType)this;
					num = 1;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, systemType.item) + ((num << 6) + (num >> 2)));
				}
				case 2:
				{
					TypeReference typeReference = (TypeReference)this;
					num = 2;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, typeReference.item) + ((num << 6) + (num >> 2)));
				}
				case 3:
				{
					FirstClassType firstClassType = (FirstClassType)this;
					num = 3;
					return -1640531527 + (firstClassType.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				}
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
				if (obj is GeneralType generalType)
				{
					GeneralType generalType2 = generalType;
					int num = _tag;
					int num2 = generalType2._tag;
					if (num == num2)
					{
						switch (Tag)
						{
						default:
						{
							VarArgType varArgType = (VarArgType)this;
							VarArgType varArgType2 = (VarArgType)generalType2;
							return varArgType.item == varArgType2.item;
						}
						case 1:
						{
							SystemType systemType = (SystemType)this;
							SystemType systemType2 = (SystemType)generalType2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, systemType.item, systemType2.item);
						}
						case 2:
						{
							TypeReference typeReference = (TypeReference)this;
							TypeReference typeReference2 = (TypeReference)generalType2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, typeReference.item, typeReference2.item);
						}
						case 3:
						{
							FirstClassType firstClassType = (FirstClassType)this;
							FirstClassType firstClassType2 = (FirstClassType)generalType2;
							cil.FirstClassType firstClassType3 = firstClassType.item;
							cil.FirstClassType obj2 = firstClassType2.item;
							return firstClassType3.Equals(obj2, comp);
						}
						}
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(GeneralType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = _tag;
					int num2 = obj._tag;
					if (num == num2)
					{
						switch (Tag)
						{
						default:
						{
							VarArgType varArgType = (VarArgType)this;
							VarArgType varArgType2 = (VarArgType)obj;
							return varArgType.item == varArgType2.item;
						}
						case 1:
						{
							SystemType systemType = (SystemType)this;
							SystemType systemType2 = (SystemType)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(systemType.item, systemType2.item);
						}
						case 2:
						{
							TypeReference typeReference = (TypeReference)this;
							TypeReference typeReference2 = (TypeReference)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(typeReference.item, typeReference2.item);
						}
						case 3:
						{
							FirstClassType firstClassType = (FirstClassType)this;
							FirstClassType firstClassType2 = (FirstClassType)obj;
							return firstClassType.item.Equals(firstClassType2.item);
						}
						}
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
			if (obj is GeneralType obj2)
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
	public class FirstClassType : IEquatable<FirstClassType>, IStructuralEquatable, IComparable<FirstClassType>, IComparable, IStructuralComparable
	{
		public static class Tags
		{
			public const int PrimitiveType = 0;

			public const int VectorType = 1;

			public const int StructType = 2;

			public const int ArrayType = 3;

			public const int FunnyIntegerType = 4;

			public const int Ptr = 5;
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(PrimitiveType@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class PrimitiveType : FirstClassType
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.PrimitiveType item;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.PrimitiveType Item
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
			internal PrimitiveType(cil.PrimitiveType item)
				: base(0)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(VectorType@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class VectorType : FirstClassType
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.VectorType item;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.VectorType Item
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
			internal VectorType(cil.VectorType item)
				: base(1)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(StructType@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class StructType : FirstClassType
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.StructType item;

			[CompilationMapping(SourceConstructFlags.Field, 2, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.StructType Item
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
			internal StructType(cil.StructType item)
				: base(2)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(ArrayType@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class ArrayType : FirstClassType
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.ArrayType item;

			[CompilationMapping(SourceConstructFlags.Field, 3, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.ArrayType Item
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
			internal ArrayType(cil.ArrayType item)
				: base(3)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(FunnyIntegerType@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class FunnyIntegerType : FirstClassType
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.FunnyIntegerType item;

			[CompilationMapping(SourceConstructFlags.Field, 4, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.FunnyIntegerType Item
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
			internal FunnyIntegerType(cil.FunnyIntegerType item)
				: base(4)
			{
				this.item = item;
			}
		}

		[SpecialName]
		internal class PrimitiveType@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal PrimitiveType _obj;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.PrimitiveType Item
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
			public PrimitiveType@DebugTypeProxy(PrimitiveType obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class VectorType@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal VectorType _obj;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.VectorType Item
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
			public VectorType@DebugTypeProxy(VectorType obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class StructType@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal StructType _obj;

			[CompilationMapping(SourceConstructFlags.Field, 2, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.StructType Item
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
			public StructType@DebugTypeProxy(StructType obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class ArrayType@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal ArrayType _obj;

			[CompilationMapping(SourceConstructFlags.Field, 3, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.ArrayType Item
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
			public ArrayType@DebugTypeProxy(ArrayType obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class FunnyIntegerType@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal FunnyIntegerType _obj;

			[CompilationMapping(SourceConstructFlags.Field, 4, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.FunnyIntegerType Item
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
			public FunnyIntegerType@DebugTypeProxy(FunnyIntegerType obj)
			{
				_obj = obj;
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly FirstClassType _unique_Ptr = new FirstClassType(5);

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
		public bool IsPrimitiveType
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
		public bool IsVectorType
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
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStructType
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsArrayType
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 3;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsFunnyIntegerType
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static FirstClassType Ptr
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 5)]
			get
			{
				return _unique_Ptr;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsPtr
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 5;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal FirstClassType(int _tag)
		{
			this._tag = _tag;
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
		public static FirstClassType NewPrimitiveType(cil.PrimitiveType item)
		{
			return new PrimitiveType(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
		public static FirstClassType NewVectorType(cil.VectorType item)
		{
			return new VectorType(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 2)]
		public static FirstClassType NewStructType(cil.StructType item)
		{
			return new StructType(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 3)]
		public static FirstClassType NewArrayType(cil.ArrayType item)
		{
			return new ArrayType(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 4)]
		public static FirstClassType NewFunnyIntegerType(cil.FunnyIntegerType item)
		{
			return new FunnyIntegerType(item);
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<FirstClassType, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<FirstClassType, string>, Unit, string, string, FirstClassType>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public virtual sealed int CompareTo(FirstClassType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = _tag;
					int num2 = obj._tag;
					if (num == num2)
					{
						switch (Tag)
						{
						case 0:
						{
							PrimitiveType primitiveType = (PrimitiveType)this;
							PrimitiveType primitiveType2 = (PrimitiveType)obj;
							IComparer genericComparer5 = LanguagePrimitives.GenericComparer;
							cil.PrimitiveType primitiveType3 = primitiveType.item;
							cil.PrimitiveType obj6 = primitiveType2.item;
							return primitiveType3.CompareTo(obj6, genericComparer5);
						}
						case 1:
						{
							VectorType vectorType = (VectorType)this;
							VectorType vectorType2 = (VectorType)obj;
							IComparer genericComparer4 = LanguagePrimitives.GenericComparer;
							cil.VectorType vectorType3 = vectorType.item;
							cil.VectorType obj5 = vectorType2.item;
							return vectorType3.CompareTo(obj5, genericComparer4);
						}
						case 2:
						{
							StructType structType = (StructType)this;
							StructType structType2 = (StructType)obj;
							IComparer genericComparer3 = LanguagePrimitives.GenericComparer;
							cil.StructType structType3 = structType.item;
							cil.StructType obj4 = structType2.item;
							return structType3.CompareTo(obj4, genericComparer3);
						}
						case 3:
						{
							ArrayType arrayType = (ArrayType)this;
							ArrayType arrayType2 = (ArrayType)obj;
							IComparer genericComparer2 = LanguagePrimitives.GenericComparer;
							cil.ArrayType arrayType3 = arrayType.item;
							cil.ArrayType obj3 = arrayType2.item;
							return arrayType3.CompareTo(obj3, genericComparer2);
						}
						case 4:
						{
							FunnyIntegerType funnyIntegerType = (FunnyIntegerType)this;
							FunnyIntegerType funnyIntegerType2 = (FunnyIntegerType)obj;
							IComparer genericComparer = LanguagePrimitives.GenericComparer;
							cil.FunnyIntegerType funnyIntegerType3 = funnyIntegerType.item;
							cil.FunnyIntegerType obj2 = funnyIntegerType2.item;
							return funnyIntegerType3.CompareTo(obj2, genericComparer);
						}
						default:
							return 0;
						}
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
		public virtual sealed int CompareTo(object obj)
		{
			return CompareTo((FirstClassType)obj);
		}

		[CompilerGenerated]
		public virtual sealed int CompareTo(object obj, IComparer comp)
		{
			FirstClassType firstClassType = (FirstClassType)obj;
			if (this != null)
			{
				if ((FirstClassType)obj != null)
				{
					int num = _tag;
					int num2 = firstClassType._tag;
					if (num == num2)
					{
						switch (Tag)
						{
						case 0:
						{
							PrimitiveType primitiveType = (PrimitiveType)this;
							PrimitiveType primitiveType2 = (PrimitiveType)firstClassType;
							cil.PrimitiveType primitiveType3 = primitiveType.item;
							cil.PrimitiveType obj6 = primitiveType2.item;
							return primitiveType3.CompareTo(obj6, comp);
						}
						case 1:
						{
							VectorType vectorType = (VectorType)this;
							VectorType vectorType2 = (VectorType)firstClassType;
							cil.VectorType vectorType3 = vectorType.item;
							cil.VectorType obj5 = vectorType2.item;
							return vectorType3.CompareTo(obj5, comp);
						}
						case 2:
						{
							StructType structType = (StructType)this;
							StructType structType2 = (StructType)firstClassType;
							cil.StructType structType3 = structType.item;
							cil.StructType obj4 = structType2.item;
							return structType3.CompareTo(obj4, comp);
						}
						case 3:
						{
							ArrayType arrayType = (ArrayType)this;
							ArrayType arrayType2 = (ArrayType)firstClassType;
							cil.ArrayType arrayType3 = arrayType.item;
							cil.ArrayType obj3 = arrayType2.item;
							return arrayType3.CompareTo(obj3, comp);
						}
						case 4:
						{
							FunnyIntegerType funnyIntegerType = (FunnyIntegerType)this;
							FunnyIntegerType funnyIntegerType2 = (FunnyIntegerType)firstClassType;
							cil.FunnyIntegerType funnyIntegerType3 = funnyIntegerType.item;
							cil.FunnyIntegerType obj2 = funnyIntegerType2.item;
							return funnyIntegerType3.CompareTo(obj2, comp);
						}
						default:
							return 0;
						}
					}
					return num - num2;
				}
				return 1;
			}
			if ((FirstClassType)obj != null)
			{
				return -1;
			}
			return 0;
		}

		[CompilerGenerated]
		public virtual sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				switch (Tag)
				{
				case 0:
				{
					PrimitiveType primitiveType = (PrimitiveType)this;
					num = 0;
					return -1640531527 + (primitiveType.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 1:
				{
					VectorType vectorType = (VectorType)this;
					num = 1;
					return -1640531527 + (vectorType.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 2:
				{
					StructType structType = (StructType)this;
					num = 2;
					return -1640531527 + (structType.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 3:
				{
					ArrayType arrayType = (ArrayType)this;
					num = 3;
					return -1640531527 + (arrayType.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 4:
				{
					FunnyIntegerType funnyIntegerType = (FunnyIntegerType)this;
					num = 4;
					return -1640531527 + (funnyIntegerType.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				default:
					return _tag;
				}
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
				if (obj is FirstClassType firstClassType)
				{
					FirstClassType firstClassType2 = firstClassType;
					int num = _tag;
					int num2 = firstClassType2._tag;
					if (num == num2)
					{
						switch (Tag)
						{
						case 0:
						{
							PrimitiveType primitiveType = (PrimitiveType)this;
							PrimitiveType primitiveType2 = (PrimitiveType)firstClassType2;
							cil.PrimitiveType primitiveType3 = primitiveType.item;
							cil.PrimitiveType obj6 = primitiveType2.item;
							return primitiveType3.Equals(obj6, comp);
						}
						case 1:
						{
							VectorType vectorType = (VectorType)this;
							VectorType vectorType2 = (VectorType)firstClassType2;
							cil.VectorType vectorType3 = vectorType.item;
							cil.VectorType obj5 = vectorType2.item;
							return vectorType3.Equals(obj5, comp);
						}
						case 2:
						{
							StructType structType = (StructType)this;
							StructType structType2 = (StructType)firstClassType2;
							cil.StructType structType3 = structType.item;
							cil.StructType obj4 = structType2.item;
							return structType3.Equals(obj4, comp);
						}
						case 3:
						{
							ArrayType arrayType = (ArrayType)this;
							ArrayType arrayType2 = (ArrayType)firstClassType2;
							cil.ArrayType arrayType3 = arrayType.item;
							cil.ArrayType obj3 = arrayType2.item;
							return arrayType3.Equals(obj3, comp);
						}
						case 4:
						{
							FunnyIntegerType funnyIntegerType = (FunnyIntegerType)this;
							FunnyIntegerType funnyIntegerType2 = (FunnyIntegerType)firstClassType2;
							cil.FunnyIntegerType funnyIntegerType3 = funnyIntegerType.item;
							cil.FunnyIntegerType obj2 = funnyIntegerType2.item;
							return funnyIntegerType3.Equals(obj2, comp);
						}
						default:
							return true;
						}
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(FirstClassType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = _tag;
					int num2 = obj._tag;
					if (num == num2)
					{
						switch (Tag)
						{
						case 0:
						{
							PrimitiveType primitiveType = (PrimitiveType)this;
							PrimitiveType primitiveType2 = (PrimitiveType)obj;
							return primitiveType.item.Equals(primitiveType2.item);
						}
						case 1:
						{
							VectorType vectorType = (VectorType)this;
							VectorType vectorType2 = (VectorType)obj;
							return vectorType.item.Equals(vectorType2.item);
						}
						case 2:
						{
							StructType structType = (StructType)this;
							StructType structType2 = (StructType)obj;
							return structType.item.Equals(structType2.item);
						}
						case 3:
						{
							ArrayType arrayType = (ArrayType)this;
							ArrayType arrayType2 = (ArrayType)obj;
							return arrayType.item.Equals(arrayType2.item);
						}
						case 4:
						{
							FunnyIntegerType funnyIntegerType = (FunnyIntegerType)this;
							FunnyIntegerType funnyIntegerType2 = (FunnyIntegerType)obj;
							return funnyIntegerType.item.Equals(funnyIntegerType2.item);
						}
						default:
							return true;
						}
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
			if (obj is FirstClassType obj2)
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
	public sealed class VecPrimitiveType : IEquatable<VecPrimitiveType>, IStructuralEquatable, IComparable<VecPrimitiveType>, IComparable, IStructuralComparable
	{
		public static class Tags
		{
			public const int V_I8 = 0;

			public const int V_I16 = 1;

			public const int V_I32 = 2;

			public const int V_I64 = 3;

			public const int V_F32 = 4;

			public const int V_F64 = 5;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly VecPrimitiveType _unique_V_I8 = new VecPrimitiveType(0);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly VecPrimitiveType _unique_V_I16 = new VecPrimitiveType(1);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly VecPrimitiveType _unique_V_I32 = new VecPrimitiveType(2);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly VecPrimitiveType _unique_V_I64 = new VecPrimitiveType(3);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly VecPrimitiveType _unique_V_F32 = new VecPrimitiveType(4);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly VecPrimitiveType _unique_V_F64 = new VecPrimitiveType(5);

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
		public static VecPrimitiveType V_I8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
			get
			{
				return _unique_V_I8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsV_I8
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
		public static VecPrimitiveType V_I16
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
			get
			{
				return _unique_V_I16;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsV_I16
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
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static VecPrimitiveType V_I32
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 2)]
			get
			{
				return _unique_V_I32;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsV_I32
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static VecPrimitiveType V_I64
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 3)]
			get
			{
				return _unique_V_I64;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsV_I64
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 3;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static VecPrimitiveType V_F32
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 4)]
			get
			{
				return _unique_V_F32;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsV_F32
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static VecPrimitiveType V_F64
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 5)]
			get
			{
				return _unique_V_F64;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsV_F64
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 5;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal VecPrimitiveType(int _tag)
		{
			this._tag = _tag;
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<VecPrimitiveType, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<VecPrimitiveType, string>, Unit, string, string, VecPrimitiveType>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(VecPrimitiveType obj)
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
			return CompareTo((VecPrimitiveType)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			VecPrimitiveType vecPrimitiveType = (VecPrimitiveType)obj;
			if (this != null)
			{
				if ((VecPrimitiveType)obj != null)
				{
					int num = _tag;
					int num2 = vecPrimitiveType._tag;
					if (num == num2)
					{
						return 0;
					}
					return num - num2;
				}
				return 1;
			}
			if ((VecPrimitiveType)obj != null)
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
				if (obj is VecPrimitiveType vecPrimitiveType)
				{
					VecPrimitiveType vecPrimitiveType2 = vecPrimitiveType;
					int num = _tag;
					int num2 = vecPrimitiveType2._tag;
					return num == num2;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(VecPrimitiveType obj)
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
			if (obj is VecPrimitiveType obj2)
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
	public class VectorElementType : IEquatable<VectorElementType>, IStructuralEquatable, IComparable<VectorElementType>, IComparable, IStructuralComparable
	{
		public static class Tags
		{
			public const int VecBit = 0;

			public const int VecPtr = 1;

			public const int VecPrim = 2;

			public const int VecFunny = 3;
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(VecPrim@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class VecPrim : VectorElementType
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly VecPrimitiveType item;

			[CompilationMapping(SourceConstructFlags.Field, 2, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public VecPrimitiveType Item
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
			internal VecPrim(VecPrimitiveType item)
				: base(2)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(VecFunny@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class VecFunny : VectorElementType
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly FunnyIntegerType item;

			[CompilationMapping(SourceConstructFlags.Field, 3, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FunnyIntegerType Item
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
			internal VecFunny(FunnyIntegerType item)
				: base(3)
			{
				this.item = item;
			}
		}

		[SpecialName]
		internal class VecPrim@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal VecPrim _obj;

			[CompilationMapping(SourceConstructFlags.Field, 2, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public VecPrimitiveType Item
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
			public VecPrim@DebugTypeProxy(VecPrim obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class VecFunny@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal VecFunny _obj;

			[CompilationMapping(SourceConstructFlags.Field, 3, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FunnyIntegerType Item
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
			public VecFunny@DebugTypeProxy(VecFunny obj)
			{
				_obj = obj;
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly VectorElementType _unique_VecBit = new VectorElementType(0);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly VectorElementType _unique_VecPtr = new VectorElementType(1);

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
		public static VectorElementType VecBit
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
			get
			{
				return _unique_VecBit;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsVecBit
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
		public static VectorElementType VecPtr
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
			get
			{
				return _unique_VecPtr;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsVecPtr
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
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsVecPrim
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsVecFunny
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 3;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal VectorElementType(int _tag)
		{
			this._tag = _tag;
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 2)]
		public static VectorElementType NewVecPrim(VecPrimitiveType item)
		{
			return new VecPrim(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 3)]
		public static VectorElementType NewVecFunny(FunnyIntegerType item)
		{
			return new VecFunny(item);
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<VectorElementType, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<VectorElementType, string>, Unit, string, string, VectorElementType>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public virtual sealed int CompareTo(VectorElementType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = _tag;
					int num2 = obj._tag;
					if (num == num2)
					{
						switch (Tag)
						{
						case 2:
						{
							VecPrim vecPrim = (VecPrim)this;
							VecPrim vecPrim2 = (VecPrim)obj;
							IComparer genericComparer2 = LanguagePrimitives.GenericComparer;
							VecPrimitiveType vecPrimitiveType = vecPrim.item;
							VecPrimitiveType obj3 = vecPrim2.item;
							return vecPrimitiveType.CompareTo(obj3, genericComparer2);
						}
						case 3:
						{
							VecFunny vecFunny = (VecFunny)this;
							VecFunny vecFunny2 = (VecFunny)obj;
							IComparer genericComparer = LanguagePrimitives.GenericComparer;
							FunnyIntegerType funnyIntegerType = vecFunny.item;
							FunnyIntegerType obj2 = vecFunny2.item;
							return funnyIntegerType.CompareTo(obj2, genericComparer);
						}
						default:
							return 0;
						}
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
		public virtual sealed int CompareTo(object obj)
		{
			return CompareTo((VectorElementType)obj);
		}

		[CompilerGenerated]
		public virtual sealed int CompareTo(object obj, IComparer comp)
		{
			VectorElementType vectorElementType = (VectorElementType)obj;
			if (this != null)
			{
				if ((VectorElementType)obj != null)
				{
					int num = _tag;
					int num2 = vectorElementType._tag;
					if (num == num2)
					{
						switch (Tag)
						{
						case 2:
						{
							VecPrim vecPrim = (VecPrim)this;
							VecPrim vecPrim2 = (VecPrim)vectorElementType;
							VecPrimitiveType vecPrimitiveType = vecPrim.item;
							VecPrimitiveType obj3 = vecPrim2.item;
							return vecPrimitiveType.CompareTo(obj3, comp);
						}
						case 3:
						{
							VecFunny vecFunny = (VecFunny)this;
							VecFunny vecFunny2 = (VecFunny)vectorElementType;
							FunnyIntegerType funnyIntegerType = vecFunny.item;
							FunnyIntegerType obj2 = vecFunny2.item;
							return funnyIntegerType.CompareTo(obj2, comp);
						}
						default:
							return 0;
						}
					}
					return num - num2;
				}
				return 1;
			}
			if ((VectorElementType)obj != null)
			{
				return -1;
			}
			return 0;
		}

		[CompilerGenerated]
		public virtual sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				switch (Tag)
				{
				case 2:
				{
					VecPrim vecPrim = (VecPrim)this;
					num = 2;
					return -1640531527 + (vecPrim.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 3:
				{
					VecFunny vecFunny = (VecFunny)this;
					num = 3;
					return -1640531527 + (vecFunny.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				default:
					return _tag;
				}
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
				if (obj is VectorElementType vectorElementType)
				{
					VectorElementType vectorElementType2 = vectorElementType;
					int num = _tag;
					int num2 = vectorElementType2._tag;
					if (num == num2)
					{
						switch (Tag)
						{
						case 2:
						{
							VecPrim vecPrim = (VecPrim)this;
							VecPrim vecPrim2 = (VecPrim)vectorElementType2;
							VecPrimitiveType vecPrimitiveType = vecPrim.item;
							VecPrimitiveType obj3 = vecPrim2.item;
							return vecPrimitiveType.Equals(obj3, comp);
						}
						case 3:
						{
							VecFunny vecFunny = (VecFunny)this;
							VecFunny vecFunny2 = (VecFunny)vectorElementType2;
							FunnyIntegerType funnyIntegerType = vecFunny.item;
							FunnyIntegerType obj2 = vecFunny2.item;
							return funnyIntegerType.Equals(obj2, comp);
						}
						default:
							return true;
						}
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(VectorElementType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = _tag;
					int num2 = obj._tag;
					if (num == num2)
					{
						switch (Tag)
						{
						case 2:
						{
							VecPrim vecPrim = (VecPrim)this;
							VecPrim vecPrim2 = (VecPrim)obj;
							return vecPrim.item.Equals(vecPrim2.item);
						}
						case 3:
						{
							VecFunny vecFunny = (VecFunny)this;
							VecFunny vecFunny2 = (VecFunny)obj;
							return vecFunny.item.Equals(vecFunny2.item);
						}
						default:
							return true;
						}
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
			if (obj is VectorElementType obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class VectorType : IEquatable<VectorType>, IStructuralEquatable, IComparable<VectorType>, IComparable, IStructuralComparable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal uint count@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal VectorElementType elemtype@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal ulong llvm_size_in_bits@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public uint count => count@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public VectorElementType elemtype => elemtype@;

		[CompilationMapping(SourceConstructFlags.Field, 2)]
		public ulong llvm_size_in_bits => llvm_size_in_bits@;

		public VectorType(uint count, VectorElementType elemtype, ulong llvm_size_in_bits)
		{
			count@ = count;
			elemtype@ = elemtype;
			llvm_size_in_bits@ = llvm_size_in_bits;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<VectorType, string>, Unit, string, string, VectorType>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(VectorType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					IComparer genericComparer = LanguagePrimitives.GenericComparer;
					uint num = count@;
					uint num2 = obj.count@;
					int num3 = ((num >= num2) ? ((num > num2) ? 1 : 0) : (-1));
					if (num3 < 0)
					{
						return num3;
					}
					if (num3 > 0)
					{
						return num3;
					}
					IComparer genericComparer2 = LanguagePrimitives.GenericComparer;
					VectorElementType vectorElementType = elemtype@;
					VectorElementType obj2 = obj.elemtype@;
					int num4 = vectorElementType.CompareTo(obj2, genericComparer2);
					if (num4 < 0)
					{
						return num4;
					}
					if (num4 > 0)
					{
						return num4;
					}
					IComparer genericComparer3 = LanguagePrimitives.GenericComparer;
					ulong num5 = llvm_size_in_bits@;
					ulong num6 = obj.llvm_size_in_bits@;
					if (num5 < num6)
					{
						return -1;
					}
					return (num5 > num6) ? 1 : 0;
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
			return CompareTo((VectorType)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			VectorType vectorType = (VectorType)obj;
			VectorType vectorType2 = vectorType;
			if (this != null)
			{
				if ((VectorType)obj != null)
				{
					uint num = count@;
					uint num2 = vectorType2.count@;
					int num3 = ((num >= num2) ? ((num > num2) ? 1 : 0) : (-1));
					if (num3 < 0)
					{
						return num3;
					}
					if (num3 > 0)
					{
						return num3;
					}
					VectorElementType vectorElementType = elemtype@;
					VectorElementType obj2 = vectorType2.elemtype@;
					int num4 = vectorElementType.CompareTo(obj2, comp);
					if (num4 < 0)
					{
						return num4;
					}
					if (num4 > 0)
					{
						return num4;
					}
					ulong num5 = llvm_size_in_bits@;
					ulong num6 = vectorType2.llvm_size_in_bits@;
					if (num5 < num6)
					{
						return -1;
					}
					return (num5 > num6) ? 1 : 0;
				}
				return 1;
			}
			if ((VectorType)obj != null)
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
				ulong num2 = llvm_size_in_bits@;
				ulong num3 = num2;
				num = -1640531527 + (((int)num3 ^ (int)(num3 >> 32)) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (elemtype@.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				return -1640531527 + ((int)count@ + ((num << 6) + (num >> 2)));
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
				if (obj is VectorType vectorType)
				{
					VectorType vectorType2 = vectorType;
					if (count@ == vectorType2.count@)
					{
						VectorElementType vectorElementType = elemtype@;
						VectorElementType obj2 = vectorType2.elemtype@;
						if (vectorElementType.Equals(obj2, comp))
						{
							return llvm_size_in_bits@ == vectorType2.llvm_size_in_bits@;
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
		public sealed bool Equals(VectorType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (count@ == obj.count@)
					{
						if (elemtype@.Equals(obj.elemtype@))
						{
							return llvm_size_in_bits@ == obj.llvm_size_in_bits@;
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
			if (obj is VectorType obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class StructItem : IEquatable<StructItem>, IStructuralEquatable, IComparable<StructItem>, IComparable, IStructuralComparable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal FirstClassType typ@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal ulong off@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public FirstClassType typ => typ@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public ulong off => off@;

		public StructItem(FirstClassType typ, ulong off)
		{
			typ@ = typ;
			off@ = off;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<StructItem, string>, Unit, string, string, StructItem>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(StructItem obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					IComparer genericComparer = LanguagePrimitives.GenericComparer;
					FirstClassType firstClassType = typ@;
					FirstClassType obj2 = obj.typ@;
					int num = firstClassType.CompareTo(obj2, genericComparer);
					if (num < 0)
					{
						return num;
					}
					if (num > 0)
					{
						return num;
					}
					IComparer genericComparer2 = LanguagePrimitives.GenericComparer;
					ulong num2 = off@;
					ulong num3 = obj.off@;
					if (num2 < num3)
					{
						return -1;
					}
					return (num2 > num3) ? 1 : 0;
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
			return CompareTo((StructItem)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			StructItem structItem = (StructItem)obj;
			StructItem structItem2 = structItem;
			if (this != null)
			{
				if ((StructItem)obj != null)
				{
					FirstClassType firstClassType = typ@;
					FirstClassType obj2 = structItem2.typ@;
					int num = firstClassType.CompareTo(obj2, comp);
					if (num < 0)
					{
						return num;
					}
					if (num > 0)
					{
						return num;
					}
					ulong num2 = off@;
					ulong num3 = structItem2.off@;
					if (num2 < num3)
					{
						return -1;
					}
					return (num2 > num3) ? 1 : 0;
				}
				return 1;
			}
			if ((StructItem)obj != null)
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
				ulong num2 = off@;
				ulong num3 = num2;
				num = -1640531527 + (((int)num3 ^ (int)(num3 >> 32)) + ((num << 6) + (num >> 2)));
				return -1640531527 + (typ@.GetHashCode(comp) + ((num << 6) + (num >> 2)));
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
				if (obj is StructItem structItem)
				{
					StructItem structItem2 = structItem;
					FirstClassType firstClassType = typ@;
					FirstClassType obj2 = structItem2.typ@;
					if (firstClassType.Equals(obj2, comp))
					{
						return off@ == structItem2.off@;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(StructItem obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (typ@.Equals(obj.typ@))
					{
						return off@ == obj.off@;
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
			if (obj is StructItem obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class StructType : IEquatable<StructType>, IStructuralEquatable, IComparable<StructType>, IComparable, IStructuralComparable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal string name@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal StructItem[] items@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal ulong llvm_size_in_bits@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public string name => name@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public StructItem[] items => items@;

		[CompilationMapping(SourceConstructFlags.Field, 2)]
		public ulong llvm_size_in_bits => llvm_size_in_bits@;

		public StructType(string name, StructItem[] items, ulong llvm_size_in_bits)
		{
			name@ = name;
			items@ = items;
			llvm_size_in_bits@ = llvm_size_in_bits;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<StructType, string>, Unit, string, string, StructType>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(StructType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					IComparer genericComparer = LanguagePrimitives.GenericComparer;
					int num = string.CompareOrdinal(name@, obj.name@);
					if (num < 0)
					{
						return num;
					}
					if (num > 0)
					{
						return num;
					}
					int num2 = LanguagePrimitives.HashCompare.GenericComparisonWithComparerIntrinsic(LanguagePrimitives.GenericComparer, items@, obj.items@);
					if (num2 < 0)
					{
						return num2;
					}
					if (num2 > 0)
					{
						return num2;
					}
					IComparer genericComparer2 = LanguagePrimitives.GenericComparer;
					ulong num3 = llvm_size_in_bits@;
					ulong num4 = obj.llvm_size_in_bits@;
					if (num3 < num4)
					{
						return -1;
					}
					return (num3 > num4) ? 1 : 0;
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
			return CompareTo((StructType)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			StructType structType = (StructType)obj;
			StructType structType2 = structType;
			if (this != null)
			{
				if ((StructType)obj != null)
				{
					int num = string.CompareOrdinal(name@, structType2.name@);
					if (num < 0)
					{
						return num;
					}
					if (num > 0)
					{
						return num;
					}
					int num2 = LanguagePrimitives.HashCompare.GenericComparisonWithComparerIntrinsic(comp, items@, structType2.items@);
					if (num2 < 0)
					{
						return num2;
					}
					if (num2 > 0)
					{
						return num2;
					}
					ulong num3 = llvm_size_in_bits@;
					ulong num4 = structType2.llvm_size_in_bits@;
					if (num3 < num4)
					{
						return -1;
					}
					return (num3 > num4) ? 1 : 0;
				}
				return 1;
			}
			if ((StructType)obj != null)
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
				ulong num2 = llvm_size_in_bits@;
				ulong num3 = num2;
				num = -1640531527 + (((int)num3 ^ (int)(num3 >> 32)) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, items@) + ((num << 6) + (num >> 2)));
				return -1640531527 + ((name@?.GetHashCode() ?? 0) + ((num << 6) + (num >> 2)));
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
				if (obj is StructType structType)
				{
					StructType structType2 = structType;
					if (string.Equals(name@, structType2.name@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, items@, structType2.items@))
						{
							return llvm_size_in_bits@ == structType2.llvm_size_in_bits@;
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
		public sealed bool Equals(StructType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (string.Equals(name@, obj.name@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(items@, obj.items@))
						{
							return llvm_size_in_bits@ == obj.llvm_size_in_bits@;
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
			if (obj is StructType obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class FunnyIntegerType : IEquatable<FunnyIntegerType>, IStructuralEquatable, IComparable<FunnyIntegerType>, IComparable, IStructuralComparable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal int bits@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public int bits => bits@;

		public FunnyIntegerType(int bits)
		{
			bits@ = bits;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<FunnyIntegerType, string>, Unit, string, string, FunnyIntegerType>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(FunnyIntegerType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					IComparer genericComparer = LanguagePrimitives.GenericComparer;
					int num = bits@;
					int num2 = obj.bits@;
					if (num < num2)
					{
						return -1;
					}
					return (num > num2) ? 1 : 0;
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
			return CompareTo((FunnyIntegerType)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			FunnyIntegerType funnyIntegerType = (FunnyIntegerType)obj;
			FunnyIntegerType funnyIntegerType2 = funnyIntegerType;
			if (this != null)
			{
				if ((FunnyIntegerType)obj != null)
				{
					int num = bits@;
					int num2 = funnyIntegerType2.bits@;
					if (num < num2)
					{
						return -1;
					}
					return (num > num2) ? 1 : 0;
				}
				return 1;
			}
			if ((FunnyIntegerType)obj != null)
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
				return -1640531527 + (bits@ + ((num << 6) + (num >> 2)));
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
				if (obj is FunnyIntegerType funnyIntegerType)
				{
					FunnyIntegerType funnyIntegerType2 = funnyIntegerType;
					return bits@ == funnyIntegerType2.bits@;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(FunnyIntegerType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					return bits@ == obj.bits@;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is FunnyIntegerType obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class ArrayType : IEquatable<ArrayType>, IStructuralEquatable, IComparable<ArrayType>, IComparable, IStructuralComparable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal uint count@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal FirstClassType elemtype@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal ulong llvm_size_in_bits@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public uint count => count@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public FirstClassType elemtype => elemtype@;

		[CompilationMapping(SourceConstructFlags.Field, 2)]
		public ulong llvm_size_in_bits => llvm_size_in_bits@;

		public ArrayType(uint count, FirstClassType elemtype, ulong llvm_size_in_bits)
		{
			count@ = count;
			elemtype@ = elemtype;
			llvm_size_in_bits@ = llvm_size_in_bits;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<ArrayType, string>, Unit, string, string, ArrayType>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(ArrayType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					IComparer genericComparer = LanguagePrimitives.GenericComparer;
					uint num = count@;
					uint num2 = obj.count@;
					int num3 = ((num >= num2) ? ((num > num2) ? 1 : 0) : (-1));
					if (num3 < 0)
					{
						return num3;
					}
					if (num3 > 0)
					{
						return num3;
					}
					IComparer genericComparer2 = LanguagePrimitives.GenericComparer;
					FirstClassType firstClassType = elemtype@;
					FirstClassType obj2 = obj.elemtype@;
					int num4 = firstClassType.CompareTo(obj2, genericComparer2);
					if (num4 < 0)
					{
						return num4;
					}
					if (num4 > 0)
					{
						return num4;
					}
					IComparer genericComparer3 = LanguagePrimitives.GenericComparer;
					ulong num5 = llvm_size_in_bits@;
					ulong num6 = obj.llvm_size_in_bits@;
					if (num5 < num6)
					{
						return -1;
					}
					return (num5 > num6) ? 1 : 0;
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
			return CompareTo((ArrayType)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			ArrayType arrayType = (ArrayType)obj;
			ArrayType arrayType2 = arrayType;
			if (this != null)
			{
				if ((ArrayType)obj != null)
				{
					uint num = count@;
					uint num2 = arrayType2.count@;
					int num3 = ((num >= num2) ? ((num > num2) ? 1 : 0) : (-1));
					if (num3 < 0)
					{
						return num3;
					}
					if (num3 > 0)
					{
						return num3;
					}
					FirstClassType firstClassType = elemtype@;
					FirstClassType obj2 = arrayType2.elemtype@;
					int num4 = firstClassType.CompareTo(obj2, comp);
					if (num4 < 0)
					{
						return num4;
					}
					if (num4 > 0)
					{
						return num4;
					}
					ulong num5 = llvm_size_in_bits@;
					ulong num6 = arrayType2.llvm_size_in_bits@;
					if (num5 < num6)
					{
						return -1;
					}
					return (num5 > num6) ? 1 : 0;
				}
				return 1;
			}
			if ((ArrayType)obj != null)
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
				ulong num2 = llvm_size_in_bits@;
				ulong num3 = num2;
				num = -1640531527 + (((int)num3 ^ (int)(num3 >> 32)) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (elemtype@.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				return -1640531527 + ((int)count@ + ((num << 6) + (num >> 2)));
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
				if (obj is ArrayType arrayType)
				{
					ArrayType arrayType2 = arrayType;
					if (count@ == arrayType2.count@)
					{
						FirstClassType firstClassType = elemtype@;
						FirstClassType obj2 = arrayType2.elemtype@;
						if (firstClassType.Equals(obj2, comp))
						{
							return llvm_size_in_bits@ == arrayType2.llvm_size_in_bits@;
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
		public sealed bool Equals(ArrayType obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (count@ == obj.count@)
					{
						if (elemtype@.Equals(obj.elemtype@))
						{
							return llvm_size_in_bits@ == obj.llvm_size_in_bits@;
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
			if (obj is ArrayType obj2)
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
	public sealed class PrimitiveType : IEquatable<PrimitiveType>, IStructuralEquatable, IComparable<PrimitiveType>, IComparable, IStructuralComparable
	{
		public static class Tags
		{
			public const int I1 = 0;

			public const int I8 = 1;

			public const int I16 = 2;

			public const int I32 = 3;

			public const int I64 = 4;

			public const int F32 = 5;

			public const int F64 = 6;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly PrimitiveType _unique_I1 = new PrimitiveType(0);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly PrimitiveType _unique_I8 = new PrimitiveType(1);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly PrimitiveType _unique_I16 = new PrimitiveType(2);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly PrimitiveType _unique_I32 = new PrimitiveType(3);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly PrimitiveType _unique_I64 = new PrimitiveType(4);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly PrimitiveType _unique_F32 = new PrimitiveType(5);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly PrimitiveType _unique_F64 = new PrimitiveType(6);

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
		public static PrimitiveType I1
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
			get
			{
				return _unique_I1;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsI1
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
		public static PrimitiveType I8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
			get
			{
				return _unique_I8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsI8
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
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static PrimitiveType I16
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 2)]
			get
			{
				return _unique_I16;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsI16
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static PrimitiveType I32
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 3)]
			get
			{
				return _unique_I32;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsI32
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 3;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static PrimitiveType I64
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 4)]
			get
			{
				return _unique_I64;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsI64
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static PrimitiveType F32
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 5)]
			get
			{
				return _unique_F32;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsF32
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 5;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static PrimitiveType F64
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 6)]
			get
			{
				return _unique_F64;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsF64
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 6;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal PrimitiveType(int _tag)
		{
			this._tag = _tag;
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<PrimitiveType, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<PrimitiveType, string>, Unit, string, string, PrimitiveType>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(PrimitiveType obj)
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
			return CompareTo((PrimitiveType)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			PrimitiveType primitiveType = (PrimitiveType)obj;
			if (this != null)
			{
				if ((PrimitiveType)obj != null)
				{
					int num = _tag;
					int num2 = primitiveType._tag;
					if (num == num2)
					{
						return 0;
					}
					return num - num2;
				}
				return 1;
			}
			if ((PrimitiveType)obj != null)
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
				if (obj is PrimitiveType primitiveType)
				{
					PrimitiveType primitiveType2 = primitiveType;
					int num = _tag;
					int num2 = primitiveType2._tag;
					return num == num2;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(PrimitiveType obj)
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
			if (obj is PrimitiveType obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class GenTypes : IEquatable<GenTypes>, IStructuralEquatable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal string ns@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal ModuleDefinition md@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<StructType, TypeReference> d_structs@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<ArrayType, TypeReference> d_arrays@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<uint, TypeReference> d_funnyints@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<VectorType, TypeReference> d_vectors@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<int, TypeReference> d_vararg@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public string ns => ns@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public ModuleDefinition md => md@;

		[CompilationMapping(SourceConstructFlags.Field, 2)]
		public Dictionary<StructType, TypeReference> d_structs => d_structs@;

		[CompilationMapping(SourceConstructFlags.Field, 3)]
		public Dictionary<ArrayType, TypeReference> d_arrays => d_arrays@;

		[CompilationMapping(SourceConstructFlags.Field, 4)]
		public Dictionary<uint, TypeReference> d_funnyints => d_funnyints@;

		[CompilationMapping(SourceConstructFlags.Field, 5)]
		public Dictionary<VectorType, TypeReference> d_vectors => d_vectors@;

		[CompilationMapping(SourceConstructFlags.Field, 6)]
		public Dictionary<int, TypeReference> d_vararg => d_vararg@;

		public GenTypes(string ns, ModuleDefinition md, Dictionary<StructType, TypeReference> d_structs, Dictionary<ArrayType, TypeReference> d_arrays, Dictionary<uint, TypeReference> d_funnyints, Dictionary<VectorType, TypeReference> d_vectors, Dictionary<int, TypeReference> d_vararg)
		{
			ns@ = ns;
			md@ = md;
			d_structs@ = d_structs;
			d_arrays@ = d_arrays;
			d_funnyints@ = d_funnyints;
			d_vectors@ = d_vectors;
			d_vararg@ = d_vararg;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<GenTypes, string>, Unit, string, string, GenTypes>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, d_vararg@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, d_vectors@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, d_funnyints@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, d_arrays@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, d_structs@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, md@) + ((num << 6) + (num >> 2)));
				return -1640531527 + ((ns@?.GetHashCode() ?? 0) + ((num << 6) + (num >> 2)));
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
				if (obj is GenTypes genTypes)
				{
					GenTypes genTypes2 = genTypes;
					if (string.Equals(ns@, genTypes2.ns@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, md@, genTypes2.md@))
						{
							if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, d_structs@, genTypes2.d_structs@))
							{
								if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, d_arrays@, genTypes2.d_arrays@))
								{
									if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, d_funnyints@, genTypes2.d_funnyints@))
									{
										if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, d_vectors@, genTypes2.d_vectors@))
										{
											return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, d_vararg@, genTypes2.d_vararg@);
										}
										return false;
									}
									return false;
								}
								return false;
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
		public sealed bool Equals(GenTypes obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (string.Equals(ns@, obj.ns@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(md@, obj.md@))
						{
							if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(d_structs@, obj.d_structs@))
							{
								if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(d_arrays@, obj.d_arrays@))
								{
									if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(d_funnyints@, obj.d_funnyints@))
									{
										if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(d_vectors@, obj.d_vectors@))
										{
											return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(d_vararg@, obj.d_vararg@);
										}
										return false;
									}
									return false;
								}
								return false;
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
			if (obj is GenTypes obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	internal sealed class fctype_to_abbrev@124-1 : FSharpFunc<uint, string>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<uint, string> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal fctype_to_abbrev@124-1(FSharpFunc<uint, string> clo2)
		{
			this.clo2 = clo2;
		}

		public override string Invoke(uint arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class fctype_to_abbrev@124 : FSharpFunc<string, FSharpFunc<uint, string>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, FSharpFunc<uint, string>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal fctype_to_abbrev@124(FSharpFunc<string, FSharpFunc<uint, string>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<uint, string> Invoke(string arg10)
		{
			FSharpFunc<uint, string> clo = clo1.Invoke(arg10);
			return new fctype_to_abbrev@124-1(clo);
		}
	}

	[Serializable]
	internal sealed class fctype_to_abbrev@126-3 : FSharpFunc<uint, string>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<uint, string> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal fctype_to_abbrev@126-3(FSharpFunc<uint, string> clo2)
		{
			this.clo2 = clo2;
		}

		public override string Invoke(uint arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class fctype_to_abbrev@126-2 : FSharpFunc<string, FSharpFunc<uint, string>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, FSharpFunc<uint, string>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal fctype_to_abbrev@126-2(FSharpFunc<string, FSharpFunc<uint, string>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<uint, string> Invoke(string arg10)
		{
			FSharpFunc<uint, string> clo = clo1.Invoke(arg10);
			return new fctype_to_abbrev@126-3(clo);
		}
	}

	[Serializable]
	internal sealed class a@131 : FSharpFunc<StructItem, string>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal a@131()
		{
		}

		public override string Invoke(StructItem it)
		{
			return fctype_to_abbrev(it.typ@);
		}
	}

	[Serializable]
	internal sealed class struct_type_to_abbrev@133-1 : FSharpFunc<string, string>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, string> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal struct_type_to_abbrev@133-1(FSharpFunc<string, string> clo2)
		{
			this.clo2 = clo2;
		}

		public override string Invoke(string arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class struct_type_to_abbrev@133 : FSharpFunc<string, FSharpFunc<string, string>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, FSharpFunc<string, string>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal struct_type_to_abbrev@133(FSharpFunc<string, FSharpFunc<string, string>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<string, string> Invoke(string arg10)
		{
			FSharpFunc<string, string> clo = clo1.Invoke(arg10);
			return new struct_type_to_abbrev@133-1(clo);
		}
	}

	[Serializable]
	internal sealed class ty@336-1 : FSharpFunc<uint, string>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<uint, string> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal ty@336-1(FSharpFunc<uint, string> clo2)
		{
			this.clo2 = clo2;
		}

		public override string Invoke(uint arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class ty@336 : FSharpFunc<VectorElementType, FSharpFunc<uint, string>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<VectorElementType, FSharpFunc<uint, string>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal ty@336(FSharpFunc<VectorElementType, FSharpFunc<uint, string>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<uint, string> Invoke(VectorElementType arg10)
		{
			FSharpFunc<uint, string> clo = clo1.Invoke(arg10);
			return new ty@336-1(clo);
		}
	}

	[Serializable]
	internal sealed class firstclass_type_to_cecil_type@344-2 : FSharpFunc<VectorType, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<VectorType, Unit> clo3;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal firstclass_type_to_cecil_type@344-2(FSharpFunc<VectorType, Unit> clo3)
		{
			this.clo3 = clo3;
		}

		public override Unit Invoke(VectorType arg30)
		{
			return clo3.Invoke(arg30);
		}
	}

	[Serializable]
	internal sealed class firstclass_type_to_cecil_type@344-1 : FSharpFunc<int, FSharpFunc<VectorType, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, FSharpFunc<VectorType, Unit>> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal firstclass_type_to_cecil_type@344-1(FSharpFunc<int, FSharpFunc<VectorType, Unit>> clo2)
		{
			this.clo2 = clo2;
		}

		public override FSharpFunc<VectorType, Unit> Invoke(int arg20)
		{
			FSharpFunc<VectorType, Unit> clo = clo2.Invoke(arg20);
			return new firstclass_type_to_cecil_type@344-2(clo);
		}
	}

	[Serializable]
	internal sealed class firstclass_type_to_cecil_type@344 : FSharpFunc<int, FSharpFunc<int, FSharpFunc<VectorType, Unit>>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, FSharpFunc<int, FSharpFunc<VectorType, Unit>>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal firstclass_type_to_cecil_type@344(FSharpFunc<int, FSharpFunc<int, FSharpFunc<VectorType, Unit>>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<int, FSharpFunc<VectorType, Unit>> Invoke(int arg10)
		{
			FSharpFunc<int, FSharpFunc<VectorType, Unit>> clo = clo1.Invoke(arg10);
			return new firstclass_type_to_cecil_type@344-1(clo);
		}
	}

	[Serializable]
	internal sealed class ty@377-3 : FSharpFunc<uint, string>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<uint, string> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal ty@377-3(FSharpFunc<uint, string> clo2)
		{
			this.clo2 = clo2;
		}

		public override string Invoke(uint arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class ty@377-2 : FSharpFunc<string, FSharpFunc<uint, string>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<string, FSharpFunc<uint, string>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal ty@377-2(FSharpFunc<string, FSharpFunc<uint, string>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<uint, string> Invoke(string arg10)
		{
			FSharpFunc<uint, string> clo = clo1.Invoke(arg10);
			return new ty@377-3(clo);
		}
	}

	[Serializable]
	internal sealed class firstclass_type_to_cecil_type@385-5 : FSharpFunc<ArrayType, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<ArrayType, Unit> clo3;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal firstclass_type_to_cecil_type@385-5(FSharpFunc<ArrayType, Unit> clo3)
		{
			this.clo3 = clo3;
		}

		public override Unit Invoke(ArrayType arg30)
		{
			return clo3.Invoke(arg30);
		}
	}

	[Serializable]
	internal sealed class firstclass_type_to_cecil_type@385-4 : FSharpFunc<int, FSharpFunc<ArrayType, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, FSharpFunc<ArrayType, Unit>> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal firstclass_type_to_cecil_type@385-4(FSharpFunc<int, FSharpFunc<ArrayType, Unit>> clo2)
		{
			this.clo2 = clo2;
		}

		public override FSharpFunc<ArrayType, Unit> Invoke(int arg20)
		{
			FSharpFunc<ArrayType, Unit> clo = clo2.Invoke(arg20);
			return new firstclass_type_to_cecil_type@385-5(clo);
		}
	}

	[Serializable]
	internal sealed class firstclass_type_to_cecil_type@385-3 : FSharpFunc<int, FSharpFunc<int, FSharpFunc<ArrayType, Unit>>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, FSharpFunc<int, FSharpFunc<ArrayType, Unit>>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal firstclass_type_to_cecil_type@385-3(FSharpFunc<int, FSharpFunc<int, FSharpFunc<ArrayType, Unit>>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<int, FSharpFunc<ArrayType, Unit>> Invoke(int arg10)
		{
			FSharpFunc<int, FSharpFunc<ArrayType, Unit>> clo = clo1.Invoke(arg10);
			return new firstclass_type_to_cecil_type@385-4(clo);
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class Label : IEquatable<Label>, IStructuralEquatable, IComparable<Label>, IComparable, IStructuralComparable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal int id@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public int id => id@;

		public Label(int id)
		{
			id@ = id;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<Label, string>, Unit, string, string, Label>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(Label obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					IComparer genericComparer = LanguagePrimitives.GenericComparer;
					int num = id@;
					int num2 = obj.id@;
					if (num < num2)
					{
						return -1;
					}
					return (num > num2) ? 1 : 0;
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
			return CompareTo((Label)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			Label label = (Label)obj;
			Label label2 = label;
			if (this != null)
			{
				if ((Label)obj != null)
				{
					int num = id@;
					int num2 = label2.id@;
					if (num < num2)
					{
						return -1;
					}
					return (num > num2) ? 1 : 0;
				}
				return 1;
			}
			if ((Label)obj != null)
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
				return -1640531527 + (id@ + ((num << 6) + (num >> 2)));
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
				if (obj is Label label)
				{
					Label label2 = label;
					return id@ == label2.id@;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(Label obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					return id@ == obj.id@;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed override bool Equals(object obj)
		{
			if (obj is Label obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class TryCatch : IEquatable<TryCatch>, IStructuralEquatable, IComparable<TryCatch>, IComparable, IStructuralComparable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal int tcid@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Label lab_ret@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public int tcid => tcid@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public Label lab_ret => lab_ret@;

		public TryCatch(int tcid, Label lab_ret)
		{
			tcid@ = tcid;
			lab_ret@ = lab_ret;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<TryCatch, string>, Unit, string, string, TryCatch>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(TryCatch obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					IComparer genericComparer = LanguagePrimitives.GenericComparer;
					int num = tcid@;
					int num2 = obj.tcid@;
					int num3 = ((num >= num2) ? ((num > num2) ? 1 : 0) : (-1));
					if (num3 < 0)
					{
						return num3;
					}
					if (num3 > 0)
					{
						return num3;
					}
					IComparer genericComparer2 = LanguagePrimitives.GenericComparer;
					Label label = lab_ret@;
					Label obj2 = obj.lab_ret@;
					return label.CompareTo(obj2, genericComparer2);
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
			return CompareTo((TryCatch)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			TryCatch tryCatch = (TryCatch)obj;
			TryCatch tryCatch2 = tryCatch;
			if (this != null)
			{
				if ((TryCatch)obj != null)
				{
					int num = tcid@;
					int num2 = tryCatch2.tcid@;
					int num3 = ((num >= num2) ? ((num > num2) ? 1 : 0) : (-1));
					if (num3 < 0)
					{
						return num3;
					}
					if (num3 > 0)
					{
						return num3;
					}
					Label label = lab_ret@;
					Label obj2 = tryCatch2.lab_ret@;
					return label.CompareTo(obj2, comp);
				}
				return 1;
			}
			if ((TryCatch)obj != null)
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
				num = -1640531527 + (lab_ret@.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				return -1640531527 + (tcid@ + ((num << 6) + (num >> 2)));
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
				if (obj is TryCatch tryCatch)
				{
					TryCatch tryCatch2 = tryCatch;
					if (tcid@ == tryCatch2.tcid@)
					{
						Label label = lab_ret@;
						Label obj2 = tryCatch2.lab_ret@;
						return label.Equals(obj2, comp);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(TryCatch obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (tcid@ == obj.tcid@)
					{
						return lab_ret@.Equals(obj.lab_ret@);
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
			if (obj is TryCatch obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class CecilTryCatch : IEquatable<CecilTryCatch>, IStructuralEquatable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Instruction begin_try@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Instruction begin_catch@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Instruction end_catch@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public Instruction begin_try => begin_try@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public Instruction begin_catch => begin_catch@;

		[CompilationMapping(SourceConstructFlags.Field, 2)]
		public Instruction end_catch => end_catch@;

		public CecilTryCatch(Instruction begin_try, Instruction begin_catch, Instruction end_catch)
		{
			begin_try@ = begin_try;
			begin_catch@ = begin_catch;
			end_catch@ = end_catch;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<CecilTryCatch, string>, Unit, string, string, CecilTryCatch>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, end_catch@) + ((num << 6) + (num >> 2)));
				num = -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, begin_catch@) + ((num << 6) + (num >> 2)));
				return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, begin_try@) + ((num << 6) + (num >> 2)));
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
				if (obj is CecilTryCatch cecilTryCatch)
				{
					CecilTryCatch cecilTryCatch2 = cecilTryCatch;
					if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, begin_try@, cecilTryCatch2.begin_try@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, begin_catch@, cecilTryCatch2.begin_catch@))
						{
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, end_catch@, cecilTryCatch2.end_catch@);
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
		public sealed bool Equals(CecilTryCatch obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(begin_try@, obj.begin_try@))
					{
						if (LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(begin_catch@, obj.begin_catch@))
						{
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(end_catch@, obj.end_catch@);
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
			if (obj is CecilTryCatch obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class Variable : IEquatable<Variable>, IStructuralEquatable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal int id@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal GeneralType typ@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public int id => id@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public GeneralType typ => typ@;

		public Variable(int id, GeneralType typ)
		{
			id@ = id;
			typ@ = typ;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<Variable, string>, Unit, string, string, Variable>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				num = -1640531527 + (typ@.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				return -1640531527 + (id@ + ((num << 6) + (num >> 2)));
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
				if (obj is Variable variable)
				{
					Variable variable2 = variable;
					if (id@ == variable2.id@)
					{
						GeneralType generalType = typ@;
						GeneralType obj2 = variable2.typ@;
						return generalType.Equals(obj2, comp);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(Variable obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (id@ == obj.id@)
					{
						return typ@.Equals(obj.typ@);
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
			if (obj is Variable obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.RecordType)]
	public sealed class FieldNumber : IEquatable<FieldNumber>, IStructuralEquatable, IComparable<FieldNumber>, IComparable, IStructuralComparable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal StructType typ@;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal int i@;

		[CompilationMapping(SourceConstructFlags.Field, 0)]
		public StructType typ => typ@;

		[CompilationMapping(SourceConstructFlags.Field, 1)]
		public int i => i@;

		public FieldNumber(StructType typ, int i)
		{
			typ@ = typ;
			i@ = i;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<FieldNumber, string>, Unit, string, string, FieldNumber>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public sealed int CompareTo(FieldNumber obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					IComparer genericComparer = LanguagePrimitives.GenericComparer;
					StructType structType = typ@;
					StructType obj2 = obj.typ@;
					int num = structType.CompareTo(obj2, genericComparer);
					if (num < 0)
					{
						return num;
					}
					if (num > 0)
					{
						return num;
					}
					IComparer genericComparer2 = LanguagePrimitives.GenericComparer;
					int num2 = i@;
					int num3 = obj.i@;
					if (num2 < num3)
					{
						return -1;
					}
					return (num2 > num3) ? 1 : 0;
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
			return CompareTo((FieldNumber)obj);
		}

		[CompilerGenerated]
		public sealed int CompareTo(object obj, IComparer comp)
		{
			FieldNumber fieldNumber = (FieldNumber)obj;
			FieldNumber fieldNumber2 = fieldNumber;
			if (this != null)
			{
				if ((FieldNumber)obj != null)
				{
					StructType structType = typ@;
					StructType obj2 = fieldNumber2.typ@;
					int num = structType.CompareTo(obj2, comp);
					if (num < 0)
					{
						return num;
					}
					if (num > 0)
					{
						return num;
					}
					int num2 = i@;
					int num3 = fieldNumber2.i@;
					if (num2 < num3)
					{
						return -1;
					}
					return (num2 > num3) ? 1 : 0;
				}
				return 1;
			}
			if ((FieldNumber)obj != null)
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
				num = -1640531527 + (i@ + ((num << 6) + (num >> 2)));
				return -1640531527 + (typ@.GetHashCode(comp) + ((num << 6) + (num >> 2)));
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
				if (obj is FieldNumber fieldNumber)
				{
					FieldNumber fieldNumber2 = fieldNumber;
					StructType structType = typ@;
					StructType obj2 = fieldNumber2.typ@;
					if (structType.Equals(obj2, comp))
					{
						return i@ == fieldNumber2.i@;
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public sealed bool Equals(FieldNumber obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					if (typ@.Equals(obj.typ@))
					{
						return i@ == obj.i@;
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
			if (obj is FieldNumber obj2)
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
	public abstract class FieldSpec : IEquatable<FieldSpec>, IStructuralEquatable
	{
		public static class Tags
		{
			public const int FieldNumber = 0;

			public const int FieldReference = 1;
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(FieldNumber@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class FieldNumber : FieldSpec
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.FieldNumber item;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.FieldNumber Item
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
			internal FieldNumber(cil.FieldNumber item)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(FieldReference@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class FieldReference : FieldSpec
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Mono.Cecil.FieldReference item;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Mono.Cecil.FieldReference Item
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
			internal FieldReference(Mono.Cecil.FieldReference item)
			{
				this.item = item;
			}
		}

		[SpecialName]
		internal class FieldNumber@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal FieldNumber _obj;

			[CompilationMapping(SourceConstructFlags.Field, 0, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public cil.FieldNumber Item
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
			public FieldNumber@DebugTypeProxy(FieldNumber obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class FieldReference@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal FieldReference _obj;

			[CompilationMapping(SourceConstructFlags.Field, 1, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Mono.Cecil.FieldReference Item
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
			public FieldReference@DebugTypeProxy(FieldReference obj)
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
				return (this is FieldReference) ? 1 : 0;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsFieldNumber
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is FieldNumber;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsFieldReference
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return this is FieldReference;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal FieldSpec()
		{
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
		public static FieldSpec NewFieldNumber(cil.FieldNumber item)
		{
			return new FieldNumber(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
		public static FieldSpec NewFieldReference(Mono.Cecil.FieldReference item)
		{
			return new FieldReference(item);
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<FieldSpec, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<FieldSpec, string>, Unit, string, string, FieldSpec>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public virtual sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				if (this is FieldNumber)
				{
					FieldNumber fieldNumber = (FieldNumber)this;
					num = 0;
					return -1640531527 + (fieldNumber.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				FieldReference fieldReference = (FieldReference)this;
				num = 1;
				return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, fieldReference.item) + ((num << 6) + (num >> 2)));
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
				if (obj is FieldSpec fieldSpec)
				{
					FieldSpec fieldSpec2 = fieldSpec;
					int num = ((this is FieldReference) ? 1 : 0);
					FieldSpec fieldSpec3 = fieldSpec2;
					int num2 = ((fieldSpec3 is FieldReference) ? 1 : 0);
					if (num == num2)
					{
						if (this is FieldNumber)
						{
							FieldNumber fieldNumber = (FieldNumber)this;
							FieldNumber fieldNumber2 = (FieldNumber)fieldSpec2;
							cil.FieldNumber fieldNumber3 = fieldNumber.item;
							cil.FieldNumber obj2 = fieldNumber2.item;
							return fieldNumber3.Equals(obj2, comp);
						}
						FieldReference fieldReference = (FieldReference)this;
						FieldReference fieldReference2 = (FieldReference)fieldSpec2;
						return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, fieldReference.item, fieldReference2.item);
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(FieldSpec obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = ((this is FieldReference) ? 1 : 0);
					int num2 = ((obj is FieldReference) ? 1 : 0);
					if (num == num2)
					{
						if (this is FieldNumber)
						{
							FieldNumber fieldNumber = (FieldNumber)this;
							FieldNumber fieldNumber2 = (FieldNumber)obj;
							return fieldNumber.item.Equals(fieldNumber2.item);
						}
						FieldReference fieldReference = (FieldReference)this;
						FieldReference fieldReference2 = (FieldReference)obj;
						return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(fieldReference.item, fieldReference2.item);
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
			if (obj is FieldSpec obj2)
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
	public class MyInstruction : IEquatable<MyInstruction>, IStructuralEquatable
	{
		public static class Tags
		{
			public const int Add = 0;

			public const int And = 1;

			public const int Br = 2;

			public const int Brfalse = 3;

			public const int Brtrue = 4;

			public const int BoxFC = 5;

			public const int Box = 6;

			public const int Unbox = 7;

			public const int Unbox_Any = 8;

			public const int Call = 9;

			public const int Calli = 10;

			public const int Callvirt = 11;

			public const int Castclass = 12;

			public const int Ceq = 13;

			public const int Cgt = 14;

			public const int Cgt_Un = 15;

			public const int Clt = 16;

			public const int Clt_Un = 17;

			public const int Conv_I = 18;

			public const int Conv_I1 = 19;

			public const int Conv_I2 = 20;

			public const int Conv_I4 = 21;

			public const int Conv_I8 = 22;

			public const int Conv_U1 = 23;

			public const int Conv_U2 = 24;

			public const int Conv_U4 = 25;

			public const int Conv_U8 = 26;

			public const int Conv_R4 = 27;

			public const int Conv_R8 = 28;

			public const int Conv_R_Un = 29;

			public const int Cpblk = 30;

			public const int Div = 31;

			public const int Div_Un = 32;

			public const int Dup = 33;

			public const int Initblk = 34;

			public const int Label = 35;

			public const int Ldarg_0 = 36;

			public const int Ldarg = 37;

			public const int Ldarga = 38;

			public const int Ldc_I4 = 39;

			public const int Ldc_I8 = 40;

			public const int Ldc_R4 = 41;

			public const int Ldc_R8 = 42;

			public const int Ldfld = 43;

			public const int Ldflda = 44;

			public const int Ldftn = 45;

			public const int Ldind_I = 46;

			public const int Ldind_I1 = 47;

			public const int Ldind_I2 = 48;

			public const int Ldind_I4 = 49;

			public const int Ldind_I8 = 50;

			public const int Ldind_R4 = 51;

			public const int Ldind_R8 = 52;

			public const int Ldind_U1 = 53;

			public const int Ldind_U2 = 54;

			public const int Ldind_U4 = 55;

			public const int Ldloc = 56;

			public const int Ldloca = 57;

			public const int Ldnull = 58;

			public const int Ldelem_Ref = 59;

			public const int Ldelem_I1 = 60;

			public const int Ldelem_I2 = 61;

			public const int Ldelem_I4 = 62;

			public const int Ldelem_I8 = 63;

			public const int Ldelem_U1 = 64;

			public const int Ldelem_U2 = 65;

			public const int Ldelem_U4 = 66;

			public const int Ldelem_I = 67;

			public const int Ldelem_R4 = 68;

			public const int Ldelem_R8 = 69;

			public const int Ldsfld = 70;

			public const int Ldsflda = 71;

			public const int Ldstr = 72;

			public const int Ldtoken = 73;

			public const int LdtokenM = 74;

			public const int Localloc = 75;

			public const int Mul = 76;

			public const int Neg = 77;

			public const int Newarr = 78;

			public const int Newobj = 79;

			public const int Nop = 80;

			public const int Or = 81;

			public const int Pop = 82;

			public const int Rem = 83;

			public const int Rem_Un = 84;

			public const int Shl = 85;

			public const int Shr = 86;

			public const int Shr_Un = 87;

			public const int Sizeof = 88;

			public const int Stelem_Ref = 89;

			public const int Stelem_I = 90;

			public const int Stelem_I1 = 91;

			public const int Stelem_I2 = 92;

			public const int Stelem_I4 = 93;

			public const int Stelem_I8 = 94;

			public const int Stelem_R4 = 95;

			public const int Stelem_R8 = 96;

			public const int Stfld = 97;

			public const int Stind_I = 98;

			public const int Stind_I1 = 99;

			public const int Stind_I2 = 100;

			public const int Stind_I4 = 101;

			public const int Stind_I8 = 102;

			public const int Stind_R4 = 103;

			public const int Stind_R8 = 104;

			public const int Stloc = 105;

			public const int Stsfld = 106;

			public const int Sub = 107;

			public const int Ret = 108;

			public const int Throw = 109;

			public const int Xor = 110;

			public const int Add_Ovf_Un = 111;

			public const int Sub_Ovf_Un = 112;

			public const int Mul_Ovf_Un = 113;

			public const int Add_Ovf = 114;

			public const int Sub_Ovf = 115;

			public const int Mul_Ovf = 116;

			public const int Leave = 117;

			public const int BeginTry = 118;

			public const int BeginCatch = 119;

			public const int EndCatch = 120;
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Br@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Br : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Label item;

			[CompilationMapping(SourceConstructFlags.Field, 2, 0)]
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
			internal Br(cil.Label item)
				: base(2)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Brfalse@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Brfalse : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Label item;

			[CompilationMapping(SourceConstructFlags.Field, 3, 0)]
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
			internal Brfalse(cil.Label item)
				: base(3)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Brtrue@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Brtrue : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Label item;

			[CompilationMapping(SourceConstructFlags.Field, 4, 0)]
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
			internal Brtrue(cil.Label item)
				: base(4)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(BoxFC@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class BoxFC : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly FirstClassType item;

			[CompilationMapping(SourceConstructFlags.Field, 5, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FirstClassType Item
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
			internal BoxFC(FirstClassType item)
				: base(5)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Box@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Box : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Type item;

			[CompilationMapping(SourceConstructFlags.Field, 6, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Type Item
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
			internal Box(Type item)
				: base(6)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Unbox@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Unbox : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Type item;

			[CompilationMapping(SourceConstructFlags.Field, 7, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Type Item
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
			internal Unbox(Type item)
				: base(7)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Unbox_Any@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Unbox_Any : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Type item;

			[CompilationMapping(SourceConstructFlags.Field, 8, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Type Item
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
			internal Unbox_Any(Type item)
				: base(8)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Call@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Call : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly MethodReference item;

			[CompilationMapping(SourceConstructFlags.Field, 9, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public MethodReference Item
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
			internal Call(MethodReference item)
				: base(9)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Calli@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Calli : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Mono.Cecil.CallSite item;

			[CompilationMapping(SourceConstructFlags.Field, 10, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Mono.Cecil.CallSite Item
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
			internal Calli(Mono.Cecil.CallSite item)
				: base(10)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Callvirt@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Callvirt : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly MethodReference item;

			[CompilationMapping(SourceConstructFlags.Field, 11, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public MethodReference Item
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
			internal Callvirt(MethodReference item)
				: base(11)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Castclass@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Castclass : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Type item;

			[CompilationMapping(SourceConstructFlags.Field, 12, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Type Item
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
			internal Castclass(Type item)
				: base(12)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Label@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Label : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Label item;

			[CompilationMapping(SourceConstructFlags.Field, 35, 0)]
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
			internal Label(cil.Label item)
				: base(35)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldarg@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldarg : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly ParameterDefinition item;

			[CompilationMapping(SourceConstructFlags.Field, 37, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public ParameterDefinition Item
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
			internal Ldarg(ParameterDefinition item)
				: base(37)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldarga@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldarga : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly ParameterDefinition item;

			[CompilationMapping(SourceConstructFlags.Field, 38, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public ParameterDefinition Item
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
			internal Ldarga(ParameterDefinition item)
				: base(38)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldc_I4@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldc_I4 : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly int item;

			[CompilationMapping(SourceConstructFlags.Field, 39, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public int Item
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
			internal Ldc_I4(int item)
				: base(39)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldc_I8@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldc_I8 : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly long item;

			[CompilationMapping(SourceConstructFlags.Field, 40, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public long Item
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
			internal Ldc_I8(long item)
				: base(40)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldc_R4@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldc_R4 : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly float item;

			[CompilationMapping(SourceConstructFlags.Field, 41, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public float Item
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
			internal Ldc_R4(float item)
				: base(41)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldc_R8@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldc_R8 : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly double item;

			[CompilationMapping(SourceConstructFlags.Field, 42, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public double Item
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
			internal Ldc_R8(double item)
				: base(42)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldfld@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldfld : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly FieldSpec item;

			[CompilationMapping(SourceConstructFlags.Field, 43, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FieldSpec Item
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
			internal Ldfld(FieldSpec item)
				: base(43)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldflda@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldflda : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly FieldSpec item;

			[CompilationMapping(SourceConstructFlags.Field, 44, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FieldSpec Item
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
			internal Ldflda(FieldSpec item)
				: base(44)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldftn@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldftn : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly MethodReference item;

			[CompilationMapping(SourceConstructFlags.Field, 45, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public MethodReference Item
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
			internal Ldftn(MethodReference item)
				: base(45)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldloc@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldloc : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Variable item;

			[CompilationMapping(SourceConstructFlags.Field, 56, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Variable Item
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
			internal Ldloc(Variable item)
				: base(56)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldloca@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldloca : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Variable item;

			[CompilationMapping(SourceConstructFlags.Field, 57, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Variable Item
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
			internal Ldloca(Variable item)
				: base(57)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldsfld@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldsfld : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly FieldReference item;

			[CompilationMapping(SourceConstructFlags.Field, 70, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FieldReference Item
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
			internal Ldsfld(FieldReference item)
				: base(70)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldsflda@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldsflda : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly FieldReference item;

			[CompilationMapping(SourceConstructFlags.Field, 71, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FieldReference Item
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
			internal Ldsflda(FieldReference item)
				: base(71)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldstr@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldstr : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly string item;

			[CompilationMapping(SourceConstructFlags.Field, 72, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public string Item
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
			internal Ldstr(string item)
				: base(72)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Ldtoken@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Ldtoken : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly TypeReference item;

			[CompilationMapping(SourceConstructFlags.Field, 73, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TypeReference Item
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
			internal Ldtoken(TypeReference item)
				: base(73)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(LdtokenM@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class LdtokenM : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly MethodReference item;

			[CompilationMapping(SourceConstructFlags.Field, 74, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public MethodReference Item
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
			internal LdtokenM(MethodReference item)
				: base(74)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Newarr@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Newarr : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Type item;

			[CompilationMapping(SourceConstructFlags.Field, 78, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Type Item
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
			internal Newarr(Type item)
				: base(78)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Newobj@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Newobj : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly MethodReference item;

			[CompilationMapping(SourceConstructFlags.Field, 79, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public MethodReference Item
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
			internal Newobj(MethodReference item)
				: base(79)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Sizeof@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Sizeof : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly StructType item;

			[CompilationMapping(SourceConstructFlags.Field, 88, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public StructType Item
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
			internal Sizeof(StructType item)
				: base(88)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Stfld@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Stfld : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly FieldSpec item;

			[CompilationMapping(SourceConstructFlags.Field, 97, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FieldSpec Item
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
			internal Stfld(FieldSpec item)
				: base(97)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Stloc@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Stloc : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly Variable item;

			[CompilationMapping(SourceConstructFlags.Field, 105, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Variable Item
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
			internal Stloc(Variable item)
				: base(105)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Stsfld@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Stsfld : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly FieldReference item;

			[CompilationMapping(SourceConstructFlags.Field, 106, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FieldReference Item
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
			internal Stsfld(FieldReference item)
				: base(106)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(Leave@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class Leave : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly cil.Label item;

			[CompilationMapping(SourceConstructFlags.Field, 117, 0)]
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
			internal Leave(cil.Label item)
				: base(117)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(BeginTry@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class BeginTry : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly TryCatch item;

			[CompilationMapping(SourceConstructFlags.Field, 118, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TryCatch Item
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
			internal BeginTry(TryCatch item)
				: base(118)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(BeginCatch@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class BeginCatch : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly TryCatch item;

			[CompilationMapping(SourceConstructFlags.Field, 119, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TryCatch Item
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
			internal BeginCatch(TryCatch item)
				: base(119)
			{
				this.item = item;
			}
		}

		[Serializable]
		[SpecialName]
		[DebuggerTypeProxy(typeof(EndCatch@DebugTypeProxy))]
		[DebuggerDisplay("{__DebugDisplay(),nq}")]
		public class EndCatch : MyInstruction
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal readonly TryCatch item;

			[CompilationMapping(SourceConstructFlags.Field, 120, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TryCatch Item
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
			internal EndCatch(TryCatch item)
				: base(120)
			{
				this.item = item;
			}
		}

		[SpecialName]
		internal class Br@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Br _obj;

			[CompilationMapping(SourceConstructFlags.Field, 2, 0)]
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
			public Br@DebugTypeProxy(Br obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Brfalse@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Brfalse _obj;

			[CompilationMapping(SourceConstructFlags.Field, 3, 0)]
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
			public Brfalse@DebugTypeProxy(Brfalse obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Brtrue@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Brtrue _obj;

			[CompilationMapping(SourceConstructFlags.Field, 4, 0)]
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
			public Brtrue@DebugTypeProxy(Brtrue obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class BoxFC@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal BoxFC _obj;

			[CompilationMapping(SourceConstructFlags.Field, 5, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FirstClassType Item
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
			public BoxFC@DebugTypeProxy(BoxFC obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Box@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Box _obj;

			[CompilationMapping(SourceConstructFlags.Field, 6, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Type Item
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
			public Box@DebugTypeProxy(Box obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Unbox@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Unbox _obj;

			[CompilationMapping(SourceConstructFlags.Field, 7, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Type Item
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
			public Unbox@DebugTypeProxy(Unbox obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Unbox_Any@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Unbox_Any _obj;

			[CompilationMapping(SourceConstructFlags.Field, 8, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Type Item
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
			public Unbox_Any@DebugTypeProxy(Unbox_Any obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Call@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Call _obj;

			[CompilationMapping(SourceConstructFlags.Field, 9, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public MethodReference Item
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
			public Call@DebugTypeProxy(Call obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Calli@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Calli _obj;

			[CompilationMapping(SourceConstructFlags.Field, 10, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Mono.Cecil.CallSite Item
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
			public Calli@DebugTypeProxy(Calli obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Callvirt@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Callvirt _obj;

			[CompilationMapping(SourceConstructFlags.Field, 11, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public MethodReference Item
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
			public Callvirt@DebugTypeProxy(Callvirt obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Castclass@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Castclass _obj;

			[CompilationMapping(SourceConstructFlags.Field, 12, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Type Item
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
			public Castclass@DebugTypeProxy(Castclass obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Label@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Label _obj;

			[CompilationMapping(SourceConstructFlags.Field, 35, 0)]
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
			public Label@DebugTypeProxy(Label obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldarg@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldarg _obj;

			[CompilationMapping(SourceConstructFlags.Field, 37, 0)]
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
			public Ldarg@DebugTypeProxy(Ldarg obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldarga@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldarga _obj;

			[CompilationMapping(SourceConstructFlags.Field, 38, 0)]
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
			public Ldarga@DebugTypeProxy(Ldarga obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldc_I4@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldc_I4 _obj;

			[CompilationMapping(SourceConstructFlags.Field, 39, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public int Item
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
			public Ldc_I4@DebugTypeProxy(Ldc_I4 obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldc_I8@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldc_I8 _obj;

			[CompilationMapping(SourceConstructFlags.Field, 40, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public long Item
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
			public Ldc_I8@DebugTypeProxy(Ldc_I8 obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldc_R4@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldc_R4 _obj;

			[CompilationMapping(SourceConstructFlags.Field, 41, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public float Item
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
			public Ldc_R4@DebugTypeProxy(Ldc_R4 obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldc_R8@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldc_R8 _obj;

			[CompilationMapping(SourceConstructFlags.Field, 42, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public double Item
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
			public Ldc_R8@DebugTypeProxy(Ldc_R8 obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldfld@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldfld _obj;

			[CompilationMapping(SourceConstructFlags.Field, 43, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FieldSpec Item
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
			public Ldfld@DebugTypeProxy(Ldfld obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldflda@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldflda _obj;

			[CompilationMapping(SourceConstructFlags.Field, 44, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FieldSpec Item
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
			public Ldflda@DebugTypeProxy(Ldflda obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldftn@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldftn _obj;

			[CompilationMapping(SourceConstructFlags.Field, 45, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public MethodReference Item
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
			public Ldftn@DebugTypeProxy(Ldftn obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldloc@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldloc _obj;

			[CompilationMapping(SourceConstructFlags.Field, 56, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Variable Item
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
			public Ldloc@DebugTypeProxy(Ldloc obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldloca@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldloca _obj;

			[CompilationMapping(SourceConstructFlags.Field, 57, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Variable Item
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
			public Ldloca@DebugTypeProxy(Ldloca obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldsfld@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldsfld _obj;

			[CompilationMapping(SourceConstructFlags.Field, 70, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FieldReference Item
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
			public Ldsfld@DebugTypeProxy(Ldsfld obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldsflda@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldsflda _obj;

			[CompilationMapping(SourceConstructFlags.Field, 71, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FieldReference Item
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
			public Ldsflda@DebugTypeProxy(Ldsflda obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldstr@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldstr _obj;

			[CompilationMapping(SourceConstructFlags.Field, 72, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public string Item
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
			public Ldstr@DebugTypeProxy(Ldstr obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Ldtoken@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Ldtoken _obj;

			[CompilationMapping(SourceConstructFlags.Field, 73, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TypeReference Item
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
			public Ldtoken@DebugTypeProxy(Ldtoken obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class LdtokenM@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal LdtokenM _obj;

			[CompilationMapping(SourceConstructFlags.Field, 74, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public MethodReference Item
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
			public LdtokenM@DebugTypeProxy(LdtokenM obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Newarr@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Newarr _obj;

			[CompilationMapping(SourceConstructFlags.Field, 78, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Type Item
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
			public Newarr@DebugTypeProxy(Newarr obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Newobj@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Newobj _obj;

			[CompilationMapping(SourceConstructFlags.Field, 79, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public MethodReference Item
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
			public Newobj@DebugTypeProxy(Newobj obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Sizeof@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Sizeof _obj;

			[CompilationMapping(SourceConstructFlags.Field, 88, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public StructType Item
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
			public Sizeof@DebugTypeProxy(Sizeof obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Stfld@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Stfld _obj;

			[CompilationMapping(SourceConstructFlags.Field, 97, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FieldSpec Item
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
			public Stfld@DebugTypeProxy(Stfld obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Stloc@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Stloc _obj;

			[CompilationMapping(SourceConstructFlags.Field, 105, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public Variable Item
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
			public Stloc@DebugTypeProxy(Stloc obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Stsfld@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Stsfld _obj;

			[CompilationMapping(SourceConstructFlags.Field, 106, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public FieldReference Item
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
			public Stsfld@DebugTypeProxy(Stsfld obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class Leave@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal Leave _obj;

			[CompilationMapping(SourceConstructFlags.Field, 117, 0)]
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
			public Leave@DebugTypeProxy(Leave obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class BeginTry@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal BeginTry _obj;

			[CompilationMapping(SourceConstructFlags.Field, 118, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TryCatch Item
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
			public BeginTry@DebugTypeProxy(BeginTry obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class BeginCatch@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal BeginCatch _obj;

			[CompilationMapping(SourceConstructFlags.Field, 119, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TryCatch Item
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
			public BeginCatch@DebugTypeProxy(BeginCatch obj)
			{
				_obj = obj;
			}
		}

		[SpecialName]
		internal class EndCatch@DebugTypeProxy
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			internal EndCatch _obj;

			[CompilationMapping(SourceConstructFlags.Field, 120, 0)]
			[CompilerGenerated]
			[DebuggerNonUserCode]
			public TryCatch Item
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
			public EndCatch@DebugTypeProxy(EndCatch obj)
			{
				_obj = obj;
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Add = new MyInstruction(0);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_And = new MyInstruction(1);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ceq = new MyInstruction(13);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Cgt = new MyInstruction(14);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Cgt_Un = new MyInstruction(15);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Clt = new MyInstruction(16);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Clt_Un = new MyInstruction(17);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Conv_I = new MyInstruction(18);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Conv_I1 = new MyInstruction(19);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Conv_I2 = new MyInstruction(20);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Conv_I4 = new MyInstruction(21);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Conv_I8 = new MyInstruction(22);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Conv_U1 = new MyInstruction(23);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Conv_U2 = new MyInstruction(24);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Conv_U4 = new MyInstruction(25);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Conv_U8 = new MyInstruction(26);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Conv_R4 = new MyInstruction(27);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Conv_R8 = new MyInstruction(28);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Conv_R_Un = new MyInstruction(29);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Cpblk = new MyInstruction(30);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Div = new MyInstruction(31);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Div_Un = new MyInstruction(32);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Dup = new MyInstruction(33);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Initblk = new MyInstruction(34);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldarg_0 = new MyInstruction(36);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldind_I = new MyInstruction(46);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldind_I1 = new MyInstruction(47);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldind_I2 = new MyInstruction(48);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldind_I4 = new MyInstruction(49);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldind_I8 = new MyInstruction(50);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldind_R4 = new MyInstruction(51);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldind_R8 = new MyInstruction(52);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldind_U1 = new MyInstruction(53);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldind_U2 = new MyInstruction(54);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldind_U4 = new MyInstruction(55);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldnull = new MyInstruction(58);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldelem_Ref = new MyInstruction(59);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldelem_I1 = new MyInstruction(60);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldelem_I2 = new MyInstruction(61);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldelem_I4 = new MyInstruction(62);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldelem_I8 = new MyInstruction(63);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldelem_U1 = new MyInstruction(64);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldelem_U2 = new MyInstruction(65);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldelem_U4 = new MyInstruction(66);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldelem_I = new MyInstruction(67);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldelem_R4 = new MyInstruction(68);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ldelem_R8 = new MyInstruction(69);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Localloc = new MyInstruction(75);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Mul = new MyInstruction(76);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Neg = new MyInstruction(77);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Nop = new MyInstruction(80);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Or = new MyInstruction(81);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Pop = new MyInstruction(82);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Rem = new MyInstruction(83);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Rem_Un = new MyInstruction(84);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Shl = new MyInstruction(85);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Shr = new MyInstruction(86);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Shr_Un = new MyInstruction(87);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stelem_Ref = new MyInstruction(89);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stelem_I = new MyInstruction(90);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stelem_I1 = new MyInstruction(91);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stelem_I2 = new MyInstruction(92);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stelem_I4 = new MyInstruction(93);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stelem_I8 = new MyInstruction(94);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stelem_R4 = new MyInstruction(95);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stelem_R8 = new MyInstruction(96);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stind_I = new MyInstruction(98);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stind_I1 = new MyInstruction(99);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stind_I2 = new MyInstruction(100);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stind_I4 = new MyInstruction(101);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stind_I8 = new MyInstruction(102);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stind_R4 = new MyInstruction(103);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Stind_R8 = new MyInstruction(104);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Sub = new MyInstruction(107);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Ret = new MyInstruction(108);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Throw = new MyInstruction(109);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Xor = new MyInstruction(110);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Add_Ovf_Un = new MyInstruction(111);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Sub_Ovf_Un = new MyInstruction(112);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Mul_Ovf_Un = new MyInstruction(113);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Add_Ovf = new MyInstruction(114);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Sub_Ovf = new MyInstruction(115);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal static readonly MyInstruction _unique_Mul_Ovf = new MyInstruction(116);

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
		public static MyInstruction Add
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 0)]
			get
			{
				return _unique_Add;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsAdd
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
		public static MyInstruction And
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 1)]
			get
			{
				return _unique_And;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsAnd
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
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsBr
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsBrfalse
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 3;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsBrtrue
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsBoxFC
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 5;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsBox
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 6;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsUnbox
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 7;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsUnbox_Any
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsCall
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 9;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsCalli
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 10;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsCallvirt
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 11;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsCastclass
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 12;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ceq
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 13)]
			get
			{
				return _unique_Ceq;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsCeq
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 13;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Cgt
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 14)]
			get
			{
				return _unique_Cgt;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsCgt
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 14;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Cgt_Un
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 15)]
			get
			{
				return _unique_Cgt_Un;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsCgt_Un
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 15;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Clt
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 16)]
			get
			{
				return _unique_Clt;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsClt
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 16;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Clt_Un
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 17)]
			get
			{
				return _unique_Clt_Un;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsClt_Un
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 17;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Conv_I
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 18)]
			get
			{
				return _unique_Conv_I;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsConv_I
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 18;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Conv_I1
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 19)]
			get
			{
				return _unique_Conv_I1;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsConv_I1
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 19;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Conv_I2
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 20)]
			get
			{
				return _unique_Conv_I2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsConv_I2
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 20;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Conv_I4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 21)]
			get
			{
				return _unique_Conv_I4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsConv_I4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 21;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Conv_I8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 22)]
			get
			{
				return _unique_Conv_I8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsConv_I8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 22;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Conv_U1
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 23)]
			get
			{
				return _unique_Conv_U1;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsConv_U1
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 23;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Conv_U2
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 24)]
			get
			{
				return _unique_Conv_U2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsConv_U2
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 24;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Conv_U4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 25)]
			get
			{
				return _unique_Conv_U4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsConv_U4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 25;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Conv_U8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 26)]
			get
			{
				return _unique_Conv_U8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsConv_U8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 26;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Conv_R4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 27)]
			get
			{
				return _unique_Conv_R4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsConv_R4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 27;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Conv_R8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 28)]
			get
			{
				return _unique_Conv_R8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsConv_R8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 28;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Conv_R_Un
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 29)]
			get
			{
				return _unique_Conv_R_Un;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsConv_R_Un
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 29;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Cpblk
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 30)]
			get
			{
				return _unique_Cpblk;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsCpblk
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 30;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Div
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 31)]
			get
			{
				return _unique_Div;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsDiv
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 31;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Div_Un
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 32)]
			get
			{
				return _unique_Div_Un;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsDiv_Un
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 32;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Dup
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 33)]
			get
			{
				return _unique_Dup;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsDup
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 33;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Initblk
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 34)]
			get
			{
				return _unique_Initblk;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsInitblk
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 34;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLabel
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 35;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldarg_0
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 36)]
			get
			{
				return _unique_Ldarg_0;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdarg_0
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 36;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdarg
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 37;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdarga
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 38;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdc_I4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 39;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdc_I8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 40;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdc_R4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 41;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdc_R8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 42;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdfld
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 43;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdflda
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 44;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdftn
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 45;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldind_I
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 46)]
			get
			{
				return _unique_Ldind_I;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdind_I
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 46;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldind_I1
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 47)]
			get
			{
				return _unique_Ldind_I1;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdind_I1
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 47;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldind_I2
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 48)]
			get
			{
				return _unique_Ldind_I2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdind_I2
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 48;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldind_I4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 49)]
			get
			{
				return _unique_Ldind_I4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdind_I4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 49;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldind_I8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 50)]
			get
			{
				return _unique_Ldind_I8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdind_I8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 50;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldind_R4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 51)]
			get
			{
				return _unique_Ldind_R4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdind_R4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 51;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldind_R8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 52)]
			get
			{
				return _unique_Ldind_R8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdind_R8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 52;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldind_U1
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 53)]
			get
			{
				return _unique_Ldind_U1;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdind_U1
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 53;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldind_U2
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 54)]
			get
			{
				return _unique_Ldind_U2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdind_U2
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 54;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldind_U4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 55)]
			get
			{
				return _unique_Ldind_U4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdind_U4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 55;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdloc
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 56;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdloca
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 57;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldnull
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 58)]
			get
			{
				return _unique_Ldnull;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdnull
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 58;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldelem_Ref
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 59)]
			get
			{
				return _unique_Ldelem_Ref;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdelem_Ref
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 59;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldelem_I1
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 60)]
			get
			{
				return _unique_Ldelem_I1;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdelem_I1
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 60;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldelem_I2
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 61)]
			get
			{
				return _unique_Ldelem_I2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdelem_I2
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 61;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldelem_I4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 62)]
			get
			{
				return _unique_Ldelem_I4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdelem_I4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 62;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldelem_I8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 63)]
			get
			{
				return _unique_Ldelem_I8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdelem_I8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 63;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldelem_U1
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 64)]
			get
			{
				return _unique_Ldelem_U1;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdelem_U1
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 64;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldelem_U2
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 65)]
			get
			{
				return _unique_Ldelem_U2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdelem_U2
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 65;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldelem_U4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 66)]
			get
			{
				return _unique_Ldelem_U4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdelem_U4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 66;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldelem_I
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 67)]
			get
			{
				return _unique_Ldelem_I;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdelem_I
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 67;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldelem_R4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 68)]
			get
			{
				return _unique_Ldelem_R4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdelem_R4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 68;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ldelem_R8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 69)]
			get
			{
				return _unique_Ldelem_R8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdelem_R8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 69;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdsfld
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 70;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdsflda
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 71;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdstr
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 72;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdtoken
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 73;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLdtokenM
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 74;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Localloc
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 75)]
			get
			{
				return _unique_Localloc;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLocalloc
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 75;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Mul
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 76)]
			get
			{
				return _unique_Mul;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsMul
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 76;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Neg
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 77)]
			get
			{
				return _unique_Neg;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsNeg
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 77;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsNewarr
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 78;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsNewobj
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 79;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Nop
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 80)]
			get
			{
				return _unique_Nop;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsNop
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 80;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Or
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 81)]
			get
			{
				return _unique_Or;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsOr
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 81;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Pop
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 82)]
			get
			{
				return _unique_Pop;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsPop
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 82;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Rem
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 83)]
			get
			{
				return _unique_Rem;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsRem
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 83;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Rem_Un
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 84)]
			get
			{
				return _unique_Rem_Un;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsRem_Un
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 84;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Shl
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 85)]
			get
			{
				return _unique_Shl;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsShl
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 85;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Shr
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 86)]
			get
			{
				return _unique_Shr;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsShr
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 86;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Shr_Un
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 87)]
			get
			{
				return _unique_Shr_Un;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsShr_Un
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 87;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsSizeof
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 88;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stelem_Ref
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 89)]
			get
			{
				return _unique_Stelem_Ref;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStelem_Ref
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 89;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stelem_I
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 90)]
			get
			{
				return _unique_Stelem_I;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStelem_I
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 90;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stelem_I1
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 91)]
			get
			{
				return _unique_Stelem_I1;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStelem_I1
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 91;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stelem_I2
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 92)]
			get
			{
				return _unique_Stelem_I2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStelem_I2
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 92;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stelem_I4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 93)]
			get
			{
				return _unique_Stelem_I4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStelem_I4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 93;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stelem_I8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 94)]
			get
			{
				return _unique_Stelem_I8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStelem_I8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 94;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stelem_R4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 95)]
			get
			{
				return _unique_Stelem_R4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStelem_R4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 95;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stelem_R8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 96)]
			get
			{
				return _unique_Stelem_R8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStelem_R8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 96;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStfld
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 97;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stind_I
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 98)]
			get
			{
				return _unique_Stind_I;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStind_I
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 98;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stind_I1
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 99)]
			get
			{
				return _unique_Stind_I1;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStind_I1
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 99;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stind_I2
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 100)]
			get
			{
				return _unique_Stind_I2;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStind_I2
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 100;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stind_I4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 101)]
			get
			{
				return _unique_Stind_I4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStind_I4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 101;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stind_I8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 102)]
			get
			{
				return _unique_Stind_I8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStind_I8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 102;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stind_R4
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 103)]
			get
			{
				return _unique_Stind_R4;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStind_R4
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 103;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Stind_R8
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 104)]
			get
			{
				return _unique_Stind_R8;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStind_R8
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 104;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStloc
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 105;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsStsfld
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 106;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Sub
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 107)]
			get
			{
				return _unique_Sub;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsSub
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 107;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Ret
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 108)]
			get
			{
				return _unique_Ret;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsRet
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 108;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Throw
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 109)]
			get
			{
				return _unique_Throw;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsThrow
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 109;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Xor
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 110)]
			get
			{
				return _unique_Xor;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsXor
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 110;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Add_Ovf_Un
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 111)]
			get
			{
				return _unique_Add_Ovf_Un;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsAdd_Ovf_Un
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 111;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Sub_Ovf_Un
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 112)]
			get
			{
				return _unique_Sub_Ovf_Un;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsSub_Ovf_Un
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 112;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Mul_Ovf_Un
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 113)]
			get
			{
				return _unique_Mul_Ovf_Un;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsMul_Ovf_Un
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 113;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Add_Ovf
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 114)]
			get
			{
				return _unique_Add_Ovf;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsAdd_Ovf
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 114;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Sub_Ovf
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 115)]
			get
			{
				return _unique_Sub_Ovf;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsSub_Ovf
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 115;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static MyInstruction Mul_Ovf
		{
			[CompilationMapping(SourceConstructFlags.UnionCase, 116)]
			get
			{
				return _unique_Mul_Ovf;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsMul_Ovf
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 116;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsLeave
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 117;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsBeginTry
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 118;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsBeginCatch
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 119;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsEndCatch
		{
			[CompilerGenerated]
			[DebuggerNonUserCode]
			get
			{
				return Tag == 120;
			}
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal MyInstruction(int _tag)
		{
			this._tag = _tag;
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 2)]
		public static MyInstruction NewBr(cil.Label item)
		{
			return new Br(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 3)]
		public static MyInstruction NewBrfalse(cil.Label item)
		{
			return new Brfalse(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 4)]
		public static MyInstruction NewBrtrue(cil.Label item)
		{
			return new Brtrue(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 5)]
		public static MyInstruction NewBoxFC(FirstClassType item)
		{
			return new BoxFC(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 6)]
		public static MyInstruction NewBox(Type item)
		{
			return new Box(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 7)]
		public static MyInstruction NewUnbox(Type item)
		{
			return new Unbox(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 8)]
		public static MyInstruction NewUnbox_Any(Type item)
		{
			return new Unbox_Any(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 9)]
		public static MyInstruction NewCall(MethodReference item)
		{
			return new Call(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 10)]
		public static MyInstruction NewCalli(Mono.Cecil.CallSite item)
		{
			return new Calli(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 11)]
		public static MyInstruction NewCallvirt(MethodReference item)
		{
			return new Callvirt(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 12)]
		public static MyInstruction NewCastclass(Type item)
		{
			return new Castclass(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 35)]
		public static MyInstruction NewLabel(cil.Label item)
		{
			return new Label(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 37)]
		public static MyInstruction NewLdarg(ParameterDefinition item)
		{
			return new Ldarg(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 38)]
		public static MyInstruction NewLdarga(ParameterDefinition item)
		{
			return new Ldarga(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 39)]
		public static MyInstruction NewLdc_I4(int item)
		{
			return new Ldc_I4(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 40)]
		public static MyInstruction NewLdc_I8(long item)
		{
			return new Ldc_I8(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 41)]
		public static MyInstruction NewLdc_R4(float item)
		{
			return new Ldc_R4(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 42)]
		public static MyInstruction NewLdc_R8(double item)
		{
			return new Ldc_R8(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 43)]
		public static MyInstruction NewLdfld(FieldSpec item)
		{
			return new Ldfld(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 44)]
		public static MyInstruction NewLdflda(FieldSpec item)
		{
			return new Ldflda(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 45)]
		public static MyInstruction NewLdftn(MethodReference item)
		{
			return new Ldftn(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 56)]
		public static MyInstruction NewLdloc(Variable item)
		{
			return new Ldloc(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 57)]
		public static MyInstruction NewLdloca(Variable item)
		{
			return new Ldloca(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 70)]
		public static MyInstruction NewLdsfld(FieldReference item)
		{
			return new Ldsfld(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 71)]
		public static MyInstruction NewLdsflda(FieldReference item)
		{
			return new Ldsflda(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 72)]
		public static MyInstruction NewLdstr(string item)
		{
			return new Ldstr(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 73)]
		public static MyInstruction NewLdtoken(TypeReference item)
		{
			return new Ldtoken(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 74)]
		public static MyInstruction NewLdtokenM(MethodReference item)
		{
			return new LdtokenM(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 78)]
		public static MyInstruction NewNewarr(Type item)
		{
			return new Newarr(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 79)]
		public static MyInstruction NewNewobj(MethodReference item)
		{
			return new Newobj(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 88)]
		public static MyInstruction NewSizeof(StructType item)
		{
			return new Sizeof(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 97)]
		public static MyInstruction NewStfld(FieldSpec item)
		{
			return new Stfld(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 105)]
		public static MyInstruction NewStloc(Variable item)
		{
			return new Stloc(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 106)]
		public static MyInstruction NewStsfld(FieldReference item)
		{
			return new Stsfld(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 117)]
		public static MyInstruction NewLeave(cil.Label item)
		{
			return new Leave(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 118)]
		public static MyInstruction NewBeginTry(TryCatch item)
		{
			return new BeginTry(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 119)]
		public static MyInstruction NewBeginCatch(TryCatch item)
		{
			return new BeginCatch(item);
		}

		[CompilationMapping(SourceConstructFlags.UnionCase, 120)]
		public static MyInstruction NewEndCatch(TryCatch item)
		{
			return new EndCatch(item);
		}

		[SpecialName]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal object __DebugDisplay()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<MyInstruction, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
		}

		[CompilerGenerated]
		public override string ToString()
		{
			return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<MyInstruction, string>, Unit, string, string, MyInstruction>("%+A")).Invoke(this);
		}

		[CompilerGenerated]
		public virtual sealed int GetHashCode(IEqualityComparer comp)
		{
			if (this != null)
			{
				int num = 0;
				switch (Tag)
				{
				case 2:
				{
					Br br = (Br)this;
					num = 2;
					return -1640531527 + (br.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 3:
				{
					Brfalse brfalse = (Brfalse)this;
					num = 3;
					return -1640531527 + (brfalse.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 4:
				{
					Brtrue brtrue = (Brtrue)this;
					num = 4;
					return -1640531527 + (brtrue.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 5:
				{
					BoxFC boxFC = (BoxFC)this;
					num = 5;
					return -1640531527 + (boxFC.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 6:
				{
					Box box = (Box)this;
					num = 6;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, box.item) + ((num << 6) + (num >> 2)));
				}
				case 7:
				{
					Unbox unbox = (Unbox)this;
					num = 7;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, unbox.item) + ((num << 6) + (num >> 2)));
				}
				case 8:
				{
					Unbox_Any unbox_Any = (Unbox_Any)this;
					num = 8;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, unbox_Any.item) + ((num << 6) + (num >> 2)));
				}
				case 9:
				{
					Call call = (Call)this;
					num = 9;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, call.item) + ((num << 6) + (num >> 2)));
				}
				case 10:
				{
					Calli calli = (Calli)this;
					num = 10;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, calli.item) + ((num << 6) + (num >> 2)));
				}
				case 11:
				{
					Callvirt callvirt = (Callvirt)this;
					num = 11;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, callvirt.item) + ((num << 6) + (num >> 2)));
				}
				case 12:
				{
					Castclass castclass = (Castclass)this;
					num = 12;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, castclass.item) + ((num << 6) + (num >> 2)));
				}
				case 35:
				{
					Label label = (Label)this;
					num = 35;
					return -1640531527 + (label.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 37:
				{
					Ldarg ldarg = (Ldarg)this;
					num = 37;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, ldarg.item) + ((num << 6) + (num >> 2)));
				}
				case 38:
				{
					Ldarga ldarga = (Ldarga)this;
					num = 38;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, ldarga.item) + ((num << 6) + (num >> 2)));
				}
				case 39:
				{
					Ldc_I4 ldc_I2 = (Ldc_I4)this;
					num = 39;
					return -1640531527 + (ldc_I2.item + ((num << 6) + (num >> 2)));
				}
				case 40:
				{
					Ldc_I8 ldc_I = (Ldc_I8)this;
					num = 40;
					long num2 = ldc_I.item;
					long num3 = num2;
					return -1640531527 + (((int)num3 ^ (int)(num3 >> 32)) + ((num << 6) + (num >> 2)));
				}
				case 41:
				{
					Ldc_R4 ldc_R2 = (Ldc_R4)this;
					num = 41;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, ldc_R2.item) + ((num << 6) + (num >> 2)));
				}
				case 42:
				{
					Ldc_R8 ldc_R = (Ldc_R8)this;
					num = 42;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, ldc_R.item) + ((num << 6) + (num >> 2)));
				}
				case 43:
				{
					Ldfld ldfld = (Ldfld)this;
					num = 43;
					return -1640531527 + (ldfld.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 44:
				{
					Ldflda ldflda = (Ldflda)this;
					num = 44;
					return -1640531527 + (ldflda.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 45:
				{
					Ldftn ldftn = (Ldftn)this;
					num = 45;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, ldftn.item) + ((num << 6) + (num >> 2)));
				}
				case 56:
				{
					Ldloc ldloc = (Ldloc)this;
					num = 56;
					return -1640531527 + (ldloc.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 57:
				{
					Ldloca ldloca = (Ldloca)this;
					num = 57;
					return -1640531527 + (ldloca.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 70:
				{
					Ldsfld ldsfld = (Ldsfld)this;
					num = 70;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, ldsfld.item) + ((num << 6) + (num >> 2)));
				}
				case 71:
				{
					Ldsflda ldsflda = (Ldsflda)this;
					num = 71;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, ldsflda.item) + ((num << 6) + (num >> 2)));
				}
				case 72:
				{
					Ldstr ldstr = (Ldstr)this;
					num = 72;
					return -1640531527 + ((ldstr.item?.GetHashCode() ?? 0) + ((num << 6) + (num >> 2)));
				}
				case 73:
				{
					Ldtoken ldtoken = (Ldtoken)this;
					num = 73;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, ldtoken.item) + ((num << 6) + (num >> 2)));
				}
				case 74:
				{
					LdtokenM ldtokenM = (LdtokenM)this;
					num = 74;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, ldtokenM.item) + ((num << 6) + (num >> 2)));
				}
				case 78:
				{
					Newarr newarr = (Newarr)this;
					num = 78;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, newarr.item) + ((num << 6) + (num >> 2)));
				}
				case 79:
				{
					Newobj newobj = (Newobj)this;
					num = 79;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, newobj.item) + ((num << 6) + (num >> 2)));
				}
				case 88:
				{
					Sizeof obj = (Sizeof)this;
					num = 88;
					return -1640531527 + (obj.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 97:
				{
					Stfld stfld = (Stfld)this;
					num = 97;
					return -1640531527 + (stfld.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 105:
				{
					Stloc stloc = (Stloc)this;
					num = 105;
					return -1640531527 + (stloc.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 106:
				{
					Stsfld stsfld = (Stsfld)this;
					num = 106;
					return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, stsfld.item) + ((num << 6) + (num >> 2)));
				}
				case 117:
				{
					Leave leave = (Leave)this;
					num = 117;
					return -1640531527 + (leave.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 118:
				{
					BeginTry beginTry = (BeginTry)this;
					num = 118;
					return -1640531527 + (beginTry.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 119:
				{
					BeginCatch beginCatch = (BeginCatch)this;
					num = 119;
					return -1640531527 + (beginCatch.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				case 120:
				{
					EndCatch endCatch = (EndCatch)this;
					num = 120;
					return -1640531527 + (endCatch.item.GetHashCode(comp) + ((num << 6) + (num >> 2)));
				}
				default:
					return _tag;
				}
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
				if (obj is MyInstruction myInstruction)
				{
					MyInstruction myInstruction2 = myInstruction;
					int num = _tag;
					int num2 = myInstruction2._tag;
					if (num == num2)
					{
						switch (Tag)
						{
						case 2:
						{
							Br br = (Br)this;
							Br br2 = (Br)myInstruction2;
							cil.Label label7 = br.item;
							cil.Label obj19 = br2.item;
							return label7.Equals(obj19, comp);
						}
						case 3:
						{
							Brfalse brfalse = (Brfalse)this;
							Brfalse brfalse2 = (Brfalse)myInstruction2;
							cil.Label label6 = brfalse.item;
							cil.Label obj18 = brfalse2.item;
							return label6.Equals(obj18, comp);
						}
						case 4:
						{
							Brtrue brtrue = (Brtrue)this;
							Brtrue brtrue2 = (Brtrue)myInstruction2;
							cil.Label label5 = brtrue.item;
							cil.Label obj17 = brtrue2.item;
							return label5.Equals(obj17, comp);
						}
						case 5:
						{
							BoxFC boxFC = (BoxFC)this;
							BoxFC boxFC2 = (BoxFC)myInstruction2;
							FirstClassType firstClassType = boxFC.item;
							FirstClassType obj16 = boxFC2.item;
							return firstClassType.Equals(obj16, comp);
						}
						case 6:
						{
							Box box = (Box)this;
							Box box2 = (Box)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, box.item, box2.item);
						}
						case 7:
						{
							Unbox unbox = (Unbox)this;
							Unbox unbox2 = (Unbox)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, unbox.item, unbox2.item);
						}
						case 8:
						{
							Unbox_Any unbox_Any = (Unbox_Any)this;
							Unbox_Any unbox_Any2 = (Unbox_Any)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, unbox_Any.item, unbox_Any2.item);
						}
						case 9:
						{
							Call call = (Call)this;
							Call call2 = (Call)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, call.item, call2.item);
						}
						case 10:
						{
							Calli calli = (Calli)this;
							Calli calli2 = (Calli)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, calli.item, calli2.item);
						}
						case 11:
						{
							Callvirt callvirt = (Callvirt)this;
							Callvirt callvirt2 = (Callvirt)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, callvirt.item, callvirt2.item);
						}
						case 12:
						{
							Castclass castclass = (Castclass)this;
							Castclass castclass2 = (Castclass)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, castclass.item, castclass2.item);
						}
						case 35:
						{
							Label label2 = (Label)this;
							Label label3 = (Label)myInstruction2;
							cil.Label label4 = label2.item;
							cil.Label obj15 = label3.item;
							return label4.Equals(obj15, comp);
						}
						case 37:
						{
							Ldarg ldarg = (Ldarg)this;
							Ldarg ldarg2 = (Ldarg)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, ldarg.item, ldarg2.item);
						}
						case 38:
						{
							Ldarga ldarga = (Ldarga)this;
							Ldarga ldarga2 = (Ldarga)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, ldarga.item, ldarga2.item);
						}
						case 39:
						{
							Ldc_I4 ldc_I3 = (Ldc_I4)this;
							Ldc_I4 ldc_I4 = (Ldc_I4)myInstruction2;
							return ldc_I3.item == ldc_I4.item;
						}
						case 40:
						{
							Ldc_I8 ldc_I = (Ldc_I8)this;
							Ldc_I8 ldc_I2 = (Ldc_I8)myInstruction2;
							return ldc_I.item == ldc_I2.item;
						}
						case 41:
						{
							Ldc_R4 ldc_R3 = (Ldc_R4)this;
							Ldc_R4 ldc_R4 = (Ldc_R4)myInstruction2;
							return ldc_R3.item == ldc_R4.item;
						}
						case 42:
						{
							Ldc_R8 ldc_R = (Ldc_R8)this;
							Ldc_R8 ldc_R2 = (Ldc_R8)myInstruction2;
							return ldc_R.item == ldc_R2.item;
						}
						case 43:
						{
							Ldfld ldfld = (Ldfld)this;
							Ldfld ldfld2 = (Ldfld)myInstruction2;
							FieldSpec fieldSpec3 = ldfld.item;
							FieldSpec obj14 = ldfld2.item;
							return fieldSpec3.Equals(obj14, comp);
						}
						case 44:
						{
							Ldflda ldflda = (Ldflda)this;
							Ldflda ldflda2 = (Ldflda)myInstruction2;
							FieldSpec fieldSpec2 = ldflda.item;
							FieldSpec obj13 = ldflda2.item;
							return fieldSpec2.Equals(obj13, comp);
						}
						case 45:
						{
							Ldftn ldftn = (Ldftn)this;
							Ldftn ldftn2 = (Ldftn)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, ldftn.item, ldftn2.item);
						}
						case 56:
						{
							Ldloc ldloc = (Ldloc)this;
							Ldloc ldloc2 = (Ldloc)myInstruction2;
							Variable variable3 = ldloc.item;
							Variable obj12 = ldloc2.item;
							return variable3.Equals(obj12, comp);
						}
						case 57:
						{
							Ldloca ldloca = (Ldloca)this;
							Ldloca ldloca2 = (Ldloca)myInstruction2;
							Variable variable2 = ldloca.item;
							Variable obj11 = ldloca2.item;
							return variable2.Equals(obj11, comp);
						}
						case 70:
						{
							Ldsfld ldsfld = (Ldsfld)this;
							Ldsfld ldsfld2 = (Ldsfld)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, ldsfld.item, ldsfld2.item);
						}
						case 71:
						{
							Ldsflda ldsflda = (Ldsflda)this;
							Ldsflda ldsflda2 = (Ldsflda)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, ldsflda.item, ldsflda2.item);
						}
						case 72:
						{
							Ldstr ldstr = (Ldstr)this;
							Ldstr ldstr2 = (Ldstr)myInstruction2;
							return string.Equals(ldstr.item, ldstr2.item);
						}
						case 73:
						{
							Ldtoken ldtoken = (Ldtoken)this;
							Ldtoken ldtoken2 = (Ldtoken)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, ldtoken.item, ldtoken2.item);
						}
						case 74:
						{
							LdtokenM ldtokenM = (LdtokenM)this;
							LdtokenM ldtokenM2 = (LdtokenM)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, ldtokenM.item, ldtokenM2.item);
						}
						case 78:
						{
							Newarr newarr = (Newarr)this;
							Newarr newarr2 = (Newarr)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, newarr.item, newarr2.item);
						}
						case 79:
						{
							Newobj newobj = (Newobj)this;
							Newobj newobj2 = (Newobj)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, newobj.item, newobj2.item);
						}
						case 88:
						{
							Sizeof obj8 = (Sizeof)this;
							Sizeof obj9 = (Sizeof)myInstruction2;
							StructType structType = obj8.item;
							StructType obj10 = obj9.item;
							return structType.Equals(obj10, comp);
						}
						case 97:
						{
							Stfld stfld = (Stfld)this;
							Stfld stfld2 = (Stfld)myInstruction2;
							FieldSpec fieldSpec = stfld.item;
							FieldSpec obj7 = stfld2.item;
							return fieldSpec.Equals(obj7, comp);
						}
						case 105:
						{
							Stloc stloc = (Stloc)this;
							Stloc stloc2 = (Stloc)myInstruction2;
							Variable variable = stloc.item;
							Variable obj6 = stloc2.item;
							return variable.Equals(obj6, comp);
						}
						case 106:
						{
							Stsfld stsfld = (Stsfld)this;
							Stsfld stsfld2 = (Stsfld)myInstruction2;
							return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic(comp, stsfld.item, stsfld2.item);
						}
						case 117:
						{
							Leave leave = (Leave)this;
							Leave leave2 = (Leave)myInstruction2;
							cil.Label label = leave.item;
							cil.Label obj5 = leave2.item;
							return label.Equals(obj5, comp);
						}
						case 118:
						{
							BeginTry beginTry = (BeginTry)this;
							BeginTry beginTry2 = (BeginTry)myInstruction2;
							TryCatch tryCatch3 = beginTry.item;
							TryCatch obj4 = beginTry2.item;
							return tryCatch3.Equals(obj4, comp);
						}
						case 119:
						{
							BeginCatch beginCatch = (BeginCatch)this;
							BeginCatch beginCatch2 = (BeginCatch)myInstruction2;
							TryCatch tryCatch2 = beginCatch.item;
							TryCatch obj3 = beginCatch2.item;
							return tryCatch2.Equals(obj3, comp);
						}
						case 120:
						{
							EndCatch endCatch = (EndCatch)this;
							EndCatch endCatch2 = (EndCatch)myInstruction2;
							TryCatch tryCatch = endCatch.item;
							TryCatch obj2 = endCatch2.item;
							return tryCatch.Equals(obj2, comp);
						}
						default:
							return true;
						}
					}
					return false;
				}
				return false;
			}
			return obj == null;
		}

		[CompilerGenerated]
		public virtual sealed bool Equals(MyInstruction obj)
		{
			if (this != null)
			{
				if (obj != null)
				{
					int num = _tag;
					int num2 = obj._tag;
					if (num == num2)
					{
						switch (Tag)
						{
						case 2:
						{
							Br br = (Br)this;
							Br br2 = (Br)obj;
							return br.item.Equals(br2.item);
						}
						case 3:
						{
							Brfalse brfalse = (Brfalse)this;
							Brfalse brfalse2 = (Brfalse)obj;
							return brfalse.item.Equals(brfalse2.item);
						}
						case 4:
						{
							Brtrue brtrue = (Brtrue)this;
							Brtrue brtrue2 = (Brtrue)obj;
							return brtrue.item.Equals(brtrue2.item);
						}
						case 5:
						{
							BoxFC boxFC = (BoxFC)this;
							BoxFC boxFC2 = (BoxFC)obj;
							return boxFC.item.Equals(boxFC2.item);
						}
						case 6:
						{
							Box box = (Box)this;
							Box box2 = (Box)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(box.item, box2.item);
						}
						case 7:
						{
							Unbox unbox = (Unbox)this;
							Unbox unbox2 = (Unbox)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(unbox.item, unbox2.item);
						}
						case 8:
						{
							Unbox_Any unbox_Any = (Unbox_Any)this;
							Unbox_Any unbox_Any2 = (Unbox_Any)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(unbox_Any.item, unbox_Any2.item);
						}
						case 9:
						{
							Call call = (Call)this;
							Call call2 = (Call)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(call.item, call2.item);
						}
						case 10:
						{
							Calli calli = (Calli)this;
							Calli calli2 = (Calli)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(calli.item, calli2.item);
						}
						case 11:
						{
							Callvirt callvirt = (Callvirt)this;
							Callvirt callvirt2 = (Callvirt)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(callvirt.item, callvirt2.item);
						}
						case 12:
						{
							Castclass castclass = (Castclass)this;
							Castclass castclass2 = (Castclass)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(castclass.item, castclass2.item);
						}
						case 35:
						{
							Label label = (Label)this;
							Label label2 = (Label)obj;
							return label.item.Equals(label2.item);
						}
						case 37:
						{
							Ldarg ldarg = (Ldarg)this;
							Ldarg ldarg2 = (Ldarg)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(ldarg.item, ldarg2.item);
						}
						case 38:
						{
							Ldarga ldarga = (Ldarga)this;
							Ldarga ldarga2 = (Ldarga)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(ldarga.item, ldarga2.item);
						}
						case 39:
						{
							Ldc_I4 ldc_I3 = (Ldc_I4)this;
							Ldc_I4 ldc_I4 = (Ldc_I4)obj;
							return ldc_I3.item == ldc_I4.item;
						}
						case 40:
						{
							Ldc_I8 ldc_I = (Ldc_I8)this;
							Ldc_I8 ldc_I2 = (Ldc_I8)obj;
							return ldc_I.item == ldc_I2.item;
						}
						case 41:
						{
							Ldc_R4 ldc_R3 = (Ldc_R4)this;
							Ldc_R4 ldc_R4 = (Ldc_R4)obj;
							float num5 = ldc_R3.item;
							float num6 = ldc_R4.item;
							if (num5 == num6)
							{
								return true;
							}
							if (num5 != num5)
							{
								return num6 != num6;
							}
							return false;
						}
						case 42:
						{
							Ldc_R8 ldc_R = (Ldc_R8)this;
							Ldc_R8 ldc_R2 = (Ldc_R8)obj;
							double num3 = ldc_R.item;
							double num4 = ldc_R2.item;
							if (num3 == num4)
							{
								return true;
							}
							if (num3 != num3)
							{
								return num4 != num4;
							}
							return false;
						}
						case 43:
						{
							Ldfld ldfld = (Ldfld)this;
							Ldfld ldfld2 = (Ldfld)obj;
							return ldfld.item.Equals(ldfld2.item);
						}
						case 44:
						{
							Ldflda ldflda = (Ldflda)this;
							Ldflda ldflda2 = (Ldflda)obj;
							return ldflda.item.Equals(ldflda2.item);
						}
						case 45:
						{
							Ldftn ldftn = (Ldftn)this;
							Ldftn ldftn2 = (Ldftn)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(ldftn.item, ldftn2.item);
						}
						case 56:
						{
							Ldloc ldloc = (Ldloc)this;
							Ldloc ldloc2 = (Ldloc)obj;
							return ldloc.item.Equals(ldloc2.item);
						}
						case 57:
						{
							Ldloca ldloca = (Ldloca)this;
							Ldloca ldloca2 = (Ldloca)obj;
							return ldloca.item.Equals(ldloca2.item);
						}
						case 70:
						{
							Ldsfld ldsfld = (Ldsfld)this;
							Ldsfld ldsfld2 = (Ldsfld)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(ldsfld.item, ldsfld2.item);
						}
						case 71:
						{
							Ldsflda ldsflda = (Ldsflda)this;
							Ldsflda ldsflda2 = (Ldsflda)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(ldsflda.item, ldsflda2.item);
						}
						case 72:
						{
							Ldstr ldstr = (Ldstr)this;
							Ldstr ldstr2 = (Ldstr)obj;
							return string.Equals(ldstr.item, ldstr2.item);
						}
						case 73:
						{
							Ldtoken ldtoken = (Ldtoken)this;
							Ldtoken ldtoken2 = (Ldtoken)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(ldtoken.item, ldtoken2.item);
						}
						case 74:
						{
							LdtokenM ldtokenM = (LdtokenM)this;
							LdtokenM ldtokenM2 = (LdtokenM)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(ldtokenM.item, ldtokenM2.item);
						}
						case 78:
						{
							Newarr newarr = (Newarr)this;
							Newarr newarr2 = (Newarr)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(newarr.item, newarr2.item);
						}
						case 79:
						{
							Newobj newobj = (Newobj)this;
							Newobj newobj2 = (Newobj)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(newobj.item, newobj2.item);
						}
						case 88:
						{
							Sizeof obj2 = (Sizeof)this;
							Sizeof obj3 = (Sizeof)obj;
							return obj2.item.Equals(obj3.item);
						}
						case 97:
						{
							Stfld stfld = (Stfld)this;
							Stfld stfld2 = (Stfld)obj;
							return stfld.item.Equals(stfld2.item);
						}
						case 105:
						{
							Stloc stloc = (Stloc)this;
							Stloc stloc2 = (Stloc)obj;
							return stloc.item.Equals(stloc2.item);
						}
						case 106:
						{
							Stsfld stsfld = (Stsfld)this;
							Stsfld stsfld2 = (Stsfld)obj;
							return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic(stsfld.item, stsfld2.item);
						}
						case 117:
						{
							Leave leave = (Leave)this;
							Leave leave2 = (Leave)obj;
							return leave.item.Equals(leave2.item);
						}
						case 118:
						{
							BeginTry beginTry = (BeginTry)this;
							BeginTry beginTry2 = (BeginTry)obj;
							return beginTry.item.Equals(beginTry2.item);
						}
						case 119:
						{
							BeginCatch beginCatch = (BeginCatch)this;
							BeginCatch beginCatch2 = (BeginCatch)obj;
							return beginCatch.item.Equals(beginCatch2.item);
						}
						case 120:
						{
							EndCatch endCatch = (EndCatch)this;
							EndCatch endCatch2 = (EndCatch)obj;
							return endCatch.item.Equals(endCatch2.item);
						}
						default:
							return true;
						}
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
			if (obj is MyInstruction obj2)
			{
				return Equals(obj2);
			}
			return false;
		}
	}

	[Serializable]
	[CompilationMapping(SourceConstructFlags.ObjectType)]
	public class CilWriter
	{
		internal List<MyInstruction> a;

		internal List<int> blocks;

		internal Dictionary<int, Stack<StackItemType>> stacks;

		internal Dictionary<Label, List<int>> branch_from_blocks;

		internal Dictionary<Label, int> label_to_block;

		internal List<Label> labels;

		internal List<TryCatch> trycatch;

		internal List<Variable> variables;

		internal Dictionary<FirstClassType, List<Variable>> temps_avail;

		internal Dictionary<FirstClassType, List<Variable>> temps_all;

		public CilWriter()
		{
			a = new List<MyInstruction>();
			blocks = new List<int>();
			stacks = new Dictionary<int, Stack<StackItemType>>();
			branch_from_blocks = new Dictionary<Label, List<int>>();
			label_to_block = new Dictionary<Label, int>();
			labels = new List<Label>();
			trycatch = new List<TryCatch>();
			variables = new List<Variable>();
			temps_avail = new Dictionary<FirstClassType, List<Variable>>();
			temps_all = new Dictionary<FirstClassType, List<Variable>>();
		}

		public void Append(MyInstruction instr)
		{
			int count = a.Count;
			a.Add(instr);
			if (count == 0)
			{
				blocks.Add(0);
				Stack<StackItemType> value = new Stack<StackItemType>();
				stacks.Add(0, value);
			}
			else
			{
				MyInstruction myInstruction = a[count - 1];
				switch (myInstruction.Tag)
				{
				case 4:
				{
					Label label = ((MyInstruction.Brtrue)myInstruction).item;
					goto case 108;
				}
				case 3:
				{
					Label label = ((MyInstruction.Brfalse)myInstruction).item;
					goto case 108;
				}
				case 117:
				{
					Label label = ((MyInstruction.Leave)myInstruction).item;
					goto case 108;
				}
				case 2:
				{
					Label label = ((MyInstruction.Br)myInstruction).item;
					goto case 108;
				}
				default:
					if (false)
					{
						goto case 108;
					}
					break;
				case 108:
				case 109:
				{
					blocks.Add(count);
					int num = blocks.Count - 1;
					List<int> preds = get_predecessors(num);
					FSharpOption<Stack<StackItemType>> fSharpOption = verify_stack(preds);
					if (fSharpOption != null)
					{
						FSharpOption<Stack<StackItemType>> fSharpOption2 = fSharpOption;
						Stack<StackItemType> value2 = fSharpOption2.Value;
						Stack<StackItemType> s = value2;
						Stack<StackItemType> value3 = copy_stack(s);
						stacks.Add(num, value3);
					}
					break;
				}
				}
			}
			int num2 = blocks.Count - 1;
			if (instr.Tag == 35)
			{
				MyInstruction.Label label2 = (MyInstruction.Label)instr;
				Label key = label2.item;
				label_to_block.Add(key, num2);
			}
			Stack<StackItemType> value4 = null;
			Tuple<bool, Stack<StackItemType>> tuple = new Tuple<bool, Stack<StackItemType>>(stacks.TryGetValue(num2, out value4), value4);
			if (tuple.Item1)
			{
				Stack<StackItemType> item = tuple.Item2;
				fix_stack(item, instr);
			}
			Label x;
			switch (instr.Tag)
			{
			default:
				return;
			case 4:
				x = ((MyInstruction.Brtrue)instr).item;
				break;
			case 3:
				x = ((MyInstruction.Brfalse)instr).item;
				break;
			case 117:
				x = ((MyInstruction.Leave)instr).item;
				break;
			case 2:
				x = ((MyInstruction.Br)instr).item;
				break;
			}
			add_branch_from_to(num2, x);
		}

		public TryCatch NewTryCatch()
		{
			TryCatch tryCatch = new TryCatch(trycatch.Count, NewLabel());
			trycatch.Add(tryCatch);
			return tryCatch;
		}

		public Label NewLabel()
		{
			Label label = new Label(labels.Count);
			labels.Add(label);
			return label;
		}

		public Variable NewVariable_VarArg(int n)
		{
			Variable variable = new Variable(variables.Count, GeneralType.NewVarArgType(n));
			variables.Add(variable);
			return variable;
		}

		public Variable NewVariable(FirstClassType tr)
		{
			Variable variable = new Variable(variables.Count, GeneralType.NewFirstClassType(tr));
			variables.Add(variable);
			return variable;
		}

		public Variable GetTempVariable(FirstClassType tr)
		{
			List<Variable> list = d_get_or_add(temps_avail, tr, new a@1735-1());
			if (list.Count > 0)
			{
				Variable result = list[0];
				list.RemoveAt(0);
				return result;
			}
			Variable variable = NewVariable(tr);
			List<Variable> list2 = d_get_or_add(temps_all, tr, new a@1742-2());
			list2.Add(variable);
			return variable;
		}

		public void ReleaseTempVariable(Variable v)
		{
			GeneralType typ@ = v.typ@;
			if (typ@.Tag == 3)
			{
				GeneralType.FirstClassType firstClassType = (GeneralType.FirstClassType)typ@;
				FirstClassType firstClassType2 = firstClassType.item;
				FirstClassType k = firstClassType2;
				List<Variable> list = d_get_or_add(temps_avail, k, new a@1752-3());
				list.Add(v);
				return;
			}
			string message = "no";
			throw Operators.Failure(message);
		}

		public Variable NewVariable(Type tr)
		{
			Variable variable = new Variable(variables.Count, GeneralType.NewSystemType(tr));
			variables.Add(variable);
			return variable;
		}

		public Variable NewVariable(TypeReference tr)
		{
			Variable variable = new Variable(variables.Count, GeneralType.NewTypeReference(tr));
			variables.Add(variable);
			return variable;
		}

		public void Dump()
		{
			List<MyInstruction> list = a;
			List<MyInstruction>.Enumerator enumerator = list.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyInstruction current = enumerator.Current;
					FSharpFunc<MyInstruction, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<MyInstruction, Unit>, TextWriter, Unit, Unit, MyInstruction>("%A"));
					MyInstruction func = current;
					fSharpFunc.Invoke(func);
				}
				Unit unit = null;
			}
			finally
			{
				((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
		}

		public void Finish(MethodDefinition method, GenTypes typs)
		{
			int num = 0;
			int num2 = blocks.Count - 1;
			if (num2 >= num)
			{
				do
				{
					Stack<StackItemType> value = null;
					Tuple<bool, Stack<StackItemType>> tuple = new Tuple<bool, Stack<StackItemType>>(stacks.TryGetValue(num, out value), value);
					if (tuple.Item1)
					{
					}
					num++;
				}
				while (num != num2 + 1);
			}
			rm_conv_i_after_ldloca();
			rm_conv_i4_after_ldc_i8();
			rm_conv_i8_after_ldc_i4();
			rm_branch_to_next_instruction();
			rm_unused_labels();
			int count = variables.Count;
			rm_unused_variables(make_varmap());
			rm_trivial_variables(make_varmap());
			rm_undef_variables(make_varmap());
			int count2 = variables.Count;
			List<MyInstruction> list = a;
			FSharpFunc<MyInstruction, bool> predicate = new a@1805-4();
			List<MyInstruction> source = list;
			MyInstruction[] array = ArrayModule.OfSeq(SeqModule.Filter(predicate, source));
			ILProcessor iLProcessor = method.Body.GetILProcessor();
			Dictionary<Label, Instruction> dictionary = new Dictionary<Label, Instruction>();
			List<Label> list2 = labels;
			List<Label>.Enumerator enumerator = list2.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Label current = enumerator.Current;
					dictionary.Add(current, iLProcessor.Create(OpCodes.Nop));
				}
				Unit unit = null;
			}
			finally
			{
				((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
			Dictionary<Variable, VariableDefinition> dictionary2 = new Dictionary<Variable, VariableDefinition>();
			Dictionary<FirstClassType, List<Variable>>.KeyCollection keys = temps_all.Keys;
			Dictionary<FirstClassType, List<Variable>>.KeyCollection.Enumerator enumerator2 = keys.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					FirstClassType current2 = enumerator2.Current;
					List<Variable> list3 = temps_all[current2];
					List<Variable> list4 = temps_avail[current2];
					if (list3.Count != list4.Count)
					{
						FSharpFunc<FirstClassType, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<FirstClassType, Unit>, Unit, string, Unit, FirstClassType>("NOT ALL TEMPS GOT RELEASED: %A"));
						FirstClassType func = current2;
						fSharpFunc.Invoke(func);
					}
				}
				Unit unit2 = null;
			}
			finally
			{
				((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
			Dictionary<TryCatch, CecilTryCatch> dictionary3 = new Dictionary<TryCatch, CecilTryCatch>();
			List<TryCatch> list5 = trycatch;
			List<TryCatch>.Enumerator enumerator3 = list5.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					TryCatch current3 = enumerator3.Current;
					CecilTryCatch value2 = new CecilTryCatch(iLProcessor.Create(OpCodes.Nop), iLProcessor.Create(OpCodes.Nop), iLProcessor.Create(OpCodes.Nop));
					dictionary3.Add(current3, value2);
				}
				Unit unit3 = null;
			}
			finally
			{
				((IDisposable)enumerator3/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
			List<Variable> list6 = variables;
			List<Variable>.Enumerator enumerator4 = list6.GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					Variable current4 = enumerator4.Current;
					TypeReference variableType = general_type_to_cecil_type(typs, current4.typ@);
					VariableDefinition variableDefinition = new VariableDefinition(variableType);
					method.Body.Variables.Add(variableDefinition);
					dictionary2.Add(current4, variableDefinition);
				}
				Unit unit4 = null;
			}
			finally
			{
				((IDisposable)enumerator4/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
			MyInstruction[] array2 = array;
			foreach (MyInstruction instr in array2)
			{
				Instruction instruction = make_cil_instruction(iLProcessor, dictionary2, dictionary, dictionary3, typs, instr);
				iLProcessor.Append(instruction);
			}
			Dictionary<TryCatch, CecilTryCatch> dictionary4 = dictionary3;
			Dictionary<TryCatch, CecilTryCatch>.Enumerator enumerator5 = dictionary4.GetEnumerator();
			try
			{
				while (enumerator5.MoveNext())
				{
					KeyValuePair<TryCatch, CecilTryCatch> current5 = enumerator5.Current;
					Tuple<TryCatch, CecilTryCatch> tuple2 = Operators.KeyValuePattern(current5);
					TryCatch item = tuple2.Item1;
					CecilTryCatch item2 = tuple2.Item2;
					ExceptionHandler exceptionHandler = new ExceptionHandler(ExceptionHandlerType.Catch);
					exceptionHandler.TryStart = item2.begin_try@;
					exceptionHandler.TryEnd = item2.begin_catch@;
					exceptionHandler.HandlerStart = item2.begin_catch@;
					exceptionHandler.HandlerEnd = item2.end_catch@;
					exceptionHandler.CatchType = typs.md@.ImportReference(typeof(Exception));
					method.Body.ExceptionHandlers.Add(exceptionHandler);
				}
				Unit unit5 = null;
			}
			finally
			{
				((IDisposable)enumerator5/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
		}

		[CompilerGenerated]
		internal int find_first_label()
		{
			List<int> list = new List<int>();
			int num = 0;
			int num2 = a.Count - 1;
			if (num2 >= num)
			{
				do
				{
					MyInstruction myInstruction = a[num];
					MyInstruction myInstruction2 = myInstruction;
					if (myInstruction2.Tag == 35)
					{
						MyInstruction.Label label = (MyInstruction.Label)myInstruction2;
						Label label2 = label.item;
						list.Add(num);
					}
					num++;
				}
				while (num != num2 + 1);
			}
			if (list.Count > 0)
			{
				return list[0];
			}
			return int.MaxValue;
		}

		[CompilerGenerated]
		internal int find_label(Label lab)
		{
			List<int> list = new List<int>();
			int num = 0;
			int num2 = a.Count - 1;
			if (num2 >= num)
			{
				do
				{
					MyInstruction myInstruction = a[num];
					MyInstruction myInstruction2 = myInstruction;
					if (myInstruction2.Tag == 35)
					{
						MyInstruction.Label label = (MyInstruction.Label)myInstruction2;
						Label label2 = label.item;
						Label label3 = label2;
						if (label3.Equals(lab, LanguagePrimitives.GenericEqualityComparer))
						{
							list.Add(num);
						}
					}
					num++;
				}
				while (num != num2 + 1);
			}
			if (list.Count == 1)
			{
				return list[0];
			}
			string message = "find_label";
			throw Operators.Failure(message);
		}

		[CompilerGenerated]
		internal void rm_conv_i_after_ldloca()
		{
			int num = 0;
			int num2 = a.Count - 1;
			if (num2 < num)
			{
				return;
			}
			do
			{
				MyInstruction myInstruction = a[num];
				if (myInstruction.Tag == 57)
				{
					MyInstruction.Ldloca ldloca = (MyInstruction.Ldloca)myInstruction;
					if (a[num + 1].Tag == 18)
					{
						a[num + 1] = MyInstruction.Nop;
					}
				}
				num++;
			}
			while (num != num2 + 1);
		}

		[CompilerGenerated]
		internal void rm_conv_i4_after_ldc_i8()
		{
			int num = 0;
			int num2 = a.Count - 1;
			if (num2 < num)
			{
				return;
			}
			do
			{
				MyInstruction myInstruction = a[num];
				if (myInstruction.Tag == 40)
				{
					MyInstruction.Ldc_I8 ldc_I = (MyInstruction.Ldc_I8)myInstruction;
					long num3 = ldc_I.item;
					if (a[num + 1].Tag == 21 && num3 >= int.MinValue && num3 <= int.MaxValue)
					{
						a[num] = MyInstruction.NewLdc_I4((int)num3);
						a[num + 1] = MyInstruction.Nop;
					}
				}
				num++;
			}
			while (num != num2 + 1);
		}

		[CompilerGenerated]
		internal void rm_conv_i8_after_ldc_i4()
		{
			int num = 0;
			int num2 = a.Count - 1;
			if (num2 < num)
			{
				return;
			}
			do
			{
				MyInstruction myInstruction = a[num];
				if (myInstruction.Tag == 39)
				{
					MyInstruction.Ldc_I4 ldc_I = (MyInstruction.Ldc_I4)myInstruction;
					int num3 = ldc_I.item;
					if (a[num + 1].Tag == 22)
					{
						a[num] = MyInstruction.NewLdc_I8(num3);
						a[num + 1] = MyInstruction.Nop;
					}
				}
				num++;
			}
			while (num != num2 + 1);
		}

		[CompilerGenerated]
		internal void rm_branch_to_next_instruction()
		{
			int num = 0;
			int num2 = a.Count - 1;
			if (num2 < num)
			{
				return;
			}
			do
			{
				MyInstruction myInstruction = a[num];
				MyInstruction myInstruction2 = myInstruction;
				if (myInstruction2.Tag == 2)
				{
					MyInstruction.Br br = (MyInstruction.Br)myInstruction2;
					Label lab = br.item;
					int num3 = find_label(lab);
					if (num + 1 == num3)
					{
						a[num] = MyInstruction.Nop;
					}
				}
				num++;
			}
			while (num != num2 + 1);
		}

		[CompilerGenerated]
		internal void rm_unused_labels()
		{
			FSharpFunc<Unit, Dictionary<Label, List<int>>> fSharpFunc = new make_labelmap@988(this);
			List<Label> list = new List<Label>();
			Dictionary<Label, List<int>> dictionary = fSharpFunc.Invoke(null);
			List<Label> list2 = labels;
			List<Label>.Enumerator enumerator = list2.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Label current = enumerator.Current;
					List<int> value = null;
					Tuple<bool, List<int>> tuple = new Tuple<bool, List<int>>(dictionary.TryGetValue(current, out value), value);
					if (!tuple.Item1)
					{
						int index = find_label(current);
						a[index] = MyInstruction.Nop;
						list.Add(current);
					}
				}
				Unit unit = null;
			}
			finally
			{
				((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
			List<Label> list3 = list;
			List<Label>.Enumerator enumerator2 = list3.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Label current2 = enumerator2.Current;
					bool flag = labels.Remove(current2);
					bool flag2 = flag;
				}
				Unit unit2 = null;
			}
			finally
			{
				((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
		}

		[CompilerGenerated]
		internal Dictionary<Variable, List<int>> make_varmap()
		{
			Dictionary<Variable, List<int>> dictionary = new Dictionary<Variable, List<int>>();
			int num = 0;
			int num2 = a.Count - 1;
			if (num2 >= num)
			{
				do
				{
					MyInstruction myInstruction = a[num];
					MyInstruction myInstruction2 = myInstruction;
					Variable key;
					switch (myInstruction2.Tag)
					{
					case 57:
						key = ((MyInstruction.Ldloca)myInstruction2).item;
						break;
					case 105:
						key = ((MyInstruction.Stloc)myInstruction2).item;
						break;
					case 56:
						key = ((MyInstruction.Ldloc)myInstruction2).item;
						break;
					default:
						goto IL_02a6;
					}
					List<int> value = null;
					Tuple<bool, List<int>> tuple = new Tuple<bool, List<int>>(dictionary.TryGetValue(key, out value), value);
					List<int> list;
					if (tuple.Item1)
					{
						List<int> item = tuple.Item2;
						list = item;
					}
					else
					{
						List<int> list2 = new List<int>();
						dictionary.Add(key, list2);
						list = list2;
					}
					List<int> list3 = list;
					list3.Add(num);
					goto IL_02a6;
					IL_02a6:
					num++;
				}
				while (num != num2 + 1);
			}
			return dictionary;
		}

		[CompilerGenerated]
		internal void rm_unused_variables(Dictionary<Variable, List<int>> varmap)
		{
			List<Variable> list = new List<Variable>();
			List<Variable> list2 = variables;
			List<Variable>.Enumerator enumerator = list2.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Variable current = enumerator.Current;
					List<int> value = null;
					Tuple<bool, List<int>> tuple = new Tuple<bool, List<int>>(varmap.TryGetValue(current, out value), value);
					if (tuple.Item1)
					{
						List<int> item = tuple.Item2;
					}
					else
					{
						list.Add(current);
					}
				}
				Unit unit = null;
			}
			finally
			{
				((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
			List<Variable> list3 = list;
			List<Variable>.Enumerator enumerator2 = list3.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Variable current2 = enumerator2.Current;
					bool flag = variables.Remove(current2);
					bool flag2 = flag;
				}
				Unit unit2 = null;
			}
			finally
			{
				((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
		}

		[CompilerGenerated]
		internal void rm_trivial_variables(Dictionary<Variable, List<int>> varmap)
		{
			List<Variable> list = new List<Variable>();
			List<Variable> list2 = variables;
			List<Variable>.Enumerator enumerator = list2.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Variable current = enumerator.Current;
					List<int> list3 = varmap[current];
					if (list3.Count == 2 && list3[1] == list3[0] + 1)
					{
						MyInstruction myInstruction = a[list3[0]];
						int num;
						if (myInstruction.Tag == 105)
						{
							MyInstruction.Stloc stloc = (MyInstruction.Stloc)myInstruction;
							num = 1;
						}
						else
						{
							num = 0;
						}
						bool flag = (byte)num != 0;
						MyInstruction myInstruction2 = a[list3[1]];
						int num2;
						if (myInstruction2.Tag == 56)
						{
							MyInstruction.Ldloc ldloc = (MyInstruction.Ldloc)myInstruction2;
							num2 = 1;
						}
						else
						{
							num2 = 0;
						}
						bool flag2 = (byte)num2 != 0;
						if (flag && flag2)
						{
							a[list3[0]] = MyInstruction.Nop;
							a[list3[1]] = MyInstruction.Nop;
							list.Add(current);
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
			List<Variable> list4 = list;
			List<Variable>.Enumerator enumerator2 = list4.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Variable current2 = enumerator2.Current;
					bool flag3 = variables.Remove(current2);
					bool flag4 = flag3;
				}
				Unit unit2 = null;
			}
			finally
			{
				((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
		}

		[CompilerGenerated]
		internal void rm_undef_variables(Dictionary<Variable, List<int>> varmap)
		{
			List<Variable> list = new List<Variable>();
			List<Variable> list2 = variables;
			List<Variable>.Enumerator enumerator = list2.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Variable current = enumerator.Current;
					List<int> list3 = varmap[current];
					if (list3.Count != 1)
					{
						continue;
					}
					int num = list3[0];
					MyInstruction myInstruction = a[num];
					if (myInstruction.Tag != 56)
					{
						continue;
					}
					MyInstruction.Ldloc ldloc = (MyInstruction.Ldloc)myInstruction;
					if (true)
					{
						MyInstruction myInstruction2 = a[num + 1];
						if (myInstruction2.Tag == 105)
						{
							MyInstruction.Stloc stloc = (MyInstruction.Stloc)myInstruction2;
							Variable variable = stloc.item;
							a[num] = MyInstruction.Nop;
							a[num + 1] = MyInstruction.Nop;
							list.Add(current);
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
			List<Variable> list4 = list;
			List<Variable>.Enumerator enumerator2 = list4.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Variable current2 = enumerator2.Current;
					bool flag = variables.Remove(current2);
					bool flag2 = flag;
				}
				Unit unit2 = null;
			}
			finally
			{
				((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
				_ = null;
			}
		}

		[CompilationArgumentCounts(new int[] { 1, 1 })]
		[CompilerGenerated]
		internal void fix_stack(Stack<StackItemType> stk, MyInstruction instr)
		{
			FSharpFunc<Unit, StackItemType> fSharpFunc = new pop_int@1136(stk);
			FSharpFunc<Unit, StackItemType> fSharpFunc2 = new pop_num@1144(this, stk);
			FSharpFunc<Unit, Unit> fSharpFunc3 = new pop_I4@1160(stk);
			FSharpFunc<Unit, Unit> fSharpFunc4 = new pop_O@1165(stk);
			FSharpFunc<Unit, Unit> fSharpFunc5 = new pop_I@1170(stk);
			FSharpFunc<Unit, Unit> fSharpFunc6 = new pop@1175(stk);
			FSharpFunc<Unit, StackItemType> fSharpFunc7 = new pop_any@1178(stk);
			FSharpFunc<StackItemType, Unit> fSharpFunc8 = new push@1181(stk);
			StackItemType stackItemType5;
			StackItemType stackItemType6;
			switch (instr.Tag)
			{
			case 88:
			{
				MyInstruction.Sizeof obj = (MyInstruction.Sizeof)instr;
				fSharpFunc8.Invoke(StackItemType.I4);
				break;
			}
			case 119:
			{
				TryCatch tryCatch = ((MyInstruction.BeginCatch)instr).item;
				break;
			}
			case 120:
			{
				TryCatch tryCatch = ((MyInstruction.EndCatch)instr).item;
				break;
			}
			case 4:
			{
				Label label = ((MyInstruction.Brtrue)instr).item;
				goto IL_0728;
			}
			case 43:
			{
				MyInstruction.Ldfld ldfld = (MyInstruction.Ldfld)instr;
				if (!(ldfld.item is FieldSpec.FieldReference))
				{
					FieldSpec.FieldNumber fieldNumber = (FieldSpec.FieldNumber)ldfld.item;
					StructType typ@ = fieldNumber.item.typ@;
					int i@ = fieldNumber.item.i@;
					fSharpFunc6.Invoke(null);
					StackItemType func7 = firstclass_type_to_stacktype(typ@.items@[i@].typ@);
					fSharpFunc8.Invoke(func7);
				}
				else
				{
					FieldReference fieldReference = ((FieldSpec.FieldReference)ldfld.item).item;
					fSharpFunc6.Invoke(null);
					StackItemType func8 = cecil_type_to_stacktype(fieldReference.FieldType);
					fSharpFunc8.Invoke(func8);
				}
				break;
			}
			case 80:
				break;
			default:
				fSharpFunc6.Invoke(null);
				fSharpFunc6.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.I4);
				break;
			case 1:
			case 81:
			case 110:
			{
				StackItemType func4 = fSharpFunc.Invoke(null);
				StackItemType stackItemType2 = fSharpFunc.Invoke(null);
				fSharpFunc8.Invoke(func4);
				break;
			}
			case 85:
			case 86:
			case 87:
			{
				StackItemType stackItemType3 = fSharpFunc.Invoke(null);
				StackItemType stackItemType4 = stackItemType3;
				StackItemType func5 = fSharpFunc.Invoke(null);
				fSharpFunc8.Invoke(func5);
				break;
			}
			case 0:
			case 31:
			case 32:
			case 76:
			case 83:
			case 84:
			case 107:
			case 111:
			case 112:
			case 113:
			case 114:
			case 115:
			case 116:
			{
				StackItemType item = fSharpFunc7.Invoke(null);
				StackItemType item2 = fSharpFunc7.Invoke(null);
				Tuple<StackItemType, StackItemType> tuple = new Tuple<StackItemType, StackItemType>(item, item2);
				Tuple<StackItemType, StackItemType> tuple2;
				StackItemType stackItemType;
				FSharpFunc<Tuple<StackItemType, StackItemType>, StackItemType> fSharpFunc9;
				Tuple<StackItemType, StackItemType> tuple3;
				StackItemType item3;
				StackItemType item4;
				switch (tuple.Item1.Tag)
				{
				case 0:
					switch (tuple.Item2.Tag)
					{
					case 2:
						goto IL_059a;
					case 0:
						goto IL_05b0;
					case 3:
						goto IL_05ee;
					}
					tuple2 = tuple;
					goto IL_0602;
				case 2:
					switch (tuple.Item2.Tag)
					{
					case 0:
						goto IL_05a5;
					case 2:
						goto IL_05bb;
					case 3:
						goto IL_05de;
					}
					tuple2 = tuple;
					goto IL_0602;
				case 4:
					if (tuple.Item2.Tag != 4)
					{
						tuple2 = tuple;
						goto IL_0602;
					}
					stackItemType = StackItemType.R4;
					break;
				case 5:
					if (tuple.Item2.Tag != 5)
					{
						tuple2 = tuple;
						goto IL_0602;
					}
					stackItemType = StackItemType.R8;
					break;
				case 3:
					switch (tuple.Item2.Tag)
					{
					case 3:
						goto IL_05d6;
					case 2:
						goto IL_05e6;
					case 0:
						goto IL_05f6;
					}
					tuple2 = tuple;
					goto IL_0602;
				default:
					{
						tuple2 = tuple;
						goto IL_0602;
					}
					IL_05f6:
					stackItemType = StackItemType.I;
					break;
					IL_05e6:
					stackItemType = StackItemType.I8;
					break;
					IL_05d6:
					stackItemType = StackItemType.I8;
					break;
					IL_0602:
					fSharpFunc9 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<Tuple<StackItemType, StackItemType>, StackItemType>, Unit, string, StackItemType, Tuple<StackItemType, StackItemType>>("match case %A"));
					tuple3 = tuple2;
					item3 = tuple3.Item1;
					item4 = tuple3.Item2;
					stackItemType = fSharpFunc9.Invoke(new Tuple<StackItemType, StackItemType>(item3, item4));
					break;
					IL_05ee:
					stackItemType = StackItemType.I;
					break;
					IL_05b0:
					stackItemType = StackItemType.I;
					break;
					IL_059a:
					stackItemType = StackItemType.I;
					break;
					IL_05de:
					stackItemType = StackItemType.I8;
					break;
					IL_05bb:
					stackItemType = StackItemType.I4;
					break;
					IL_05a5:
					stackItemType = StackItemType.I;
					break;
				}
				StackItemType func2 = stackItemType;
				fSharpFunc8.Invoke(func2);
				break;
			}
			case 5:
			{
				FirstClassType firstClassType = ((MyInstruction.BoxFC)instr).item;
				fSharpFunc6.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.O);
				break;
			}
			case 6:
			{
				Type type = ((MyInstruction.Box)instr).item;
				fSharpFunc6.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.O);
				break;
			}
			case 7:
			{
				Type type2 = ((MyInstruction.Unbox)instr).item;
				fSharpFunc4.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.I);
				break;
			}
			case 8:
			{
				Type t = ((MyInstruction.Unbox_Any)instr).item;
				fSharpFunc4.Invoke(null);
				StackItemType func14 = systype_to_stacktype(t);
				fSharpFunc8.Invoke(func14);
				break;
			}
			case 2:
			{
				Label label4 = ((MyInstruction.Br)instr).item;
				break;
			}
			case 117:
			{
				Label label3 = ((MyInstruction.Leave)instr).item;
				break;
			}
			case 118:
			{
				TryCatch tryCatch = ((MyInstruction.BeginTry)instr).item;
				break;
			}
			case 3:
			{
				Label label = ((MyInstruction.Brfalse)instr).item;
				goto IL_0728;
			}
			case 18:
				fSharpFunc6.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.I);
				break;
			case 19:
			case 20:
			case 21:
			case 23:
			case 24:
			case 25:
			{
				StackItemType stackItemType11 = fSharpFunc2.Invoke(null);
				StackItemType stackItemType12 = stackItemType11;
				fSharpFunc8.Invoke(StackItemType.I4);
				break;
			}
			case 22:
			case 26:
				fSharpFunc6.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.I8);
				break;
			case 27:
			case 29:
			{
				StackItemType stackItemType9 = fSharpFunc2.Invoke(null);
				StackItemType stackItemType10 = stackItemType9;
				fSharpFunc8.Invoke(StackItemType.R4);
				break;
			}
			case 28:
			{
				StackItemType stackItemType7 = fSharpFunc2.Invoke(null);
				StackItemType stackItemType8 = stackItemType7;
				fSharpFunc8.Invoke(StackItemType.R8);
				break;
			}
			case 30:
				fSharpFunc3.Invoke(null);
				fSharpFunc5.Invoke(null);
				fSharpFunc5.Invoke(null);
				break;
			case 34:
				fSharpFunc3.Invoke(null);
				fSharpFunc3.Invoke(null);
				fSharpFunc5.Invoke(null);
				break;
			case 33:
			{
				StackItemType func13 = fSharpFunc7.Invoke(null);
				fSharpFunc8.Invoke(func13);
				fSharpFunc8.Invoke(func13);
				break;
			}
			case 35:
			{
				Label label2 = ((MyInstruction.Label)instr).item;
				break;
			}
			case 36:
				fSharpFunc8.Invoke(StackItemType.I);
				break;
			case 37:
			{
				ParameterDefinition parameterDefinition2 = ((MyInstruction.Ldarg)instr).item;
				StackItemType func12 = cecil_type_to_stacktype(parameterDefinition2.ParameterType);
				fSharpFunc8.Invoke(func12);
				break;
			}
			case 38:
			{
				ParameterDefinition parameterDefinition = ((MyInstruction.Ldarga)instr).item;
				fSharpFunc8.Invoke(StackItemType.I);
				break;
			}
			case 39:
			{
				int num12 = ((MyInstruction.Ldc_I4)instr).item;
				fSharpFunc8.Invoke(StackItemType.I4);
				break;
			}
			case 40:
			{
				long num11 = ((MyInstruction.Ldc_I8)instr).item;
				fSharpFunc8.Invoke(StackItemType.I8);
				break;
			}
			case 41:
			{
				float num10 = ((MyInstruction.Ldc_R4)instr).item;
				fSharpFunc8.Invoke(StackItemType.R4);
				break;
			}
			case 42:
			{
				double num9 = ((MyInstruction.Ldc_R8)instr).item;
				fSharpFunc8.Invoke(StackItemType.R8);
				break;
			}
			case 44:
			{
				FieldSpec fieldSpec2 = ((MyInstruction.Ldflda)instr).item;
				fSharpFunc6.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.I);
				break;
			}
			case 45:
			{
				MethodReference methodReference5 = ((MyInstruction.Ldftn)instr).item;
				fSharpFunc8.Invoke(StackItemType.I);
				break;
			}
			case 46:
				fSharpFunc5.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.I);
				break;
			case 47:
			case 48:
			case 49:
			case 53:
			case 54:
			case 55:
				fSharpFunc5.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.I4);
				break;
			case 51:
				fSharpFunc5.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.R4);
				break;
			case 52:
				fSharpFunc5.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.R8);
				break;
			case 50:
				fSharpFunc5.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.I8);
				break;
			case 56:
			{
				Variable v = ((MyInstruction.Ldloc)instr).item;
				StackItemType func11 = get_var_stacktype(v);
				fSharpFunc8.Invoke(func11);
				break;
			}
			case 57:
			{
				Variable variable2 = ((MyInstruction.Ldloca)instr).item;
				fSharpFunc8.Invoke(StackItemType.I);
				break;
			}
			case 58:
				fSharpFunc8.Invoke(StackItemType.O);
				break;
			case 70:
			{
				FieldReference fieldReference4 = ((MyInstruction.Ldsfld)instr).item;
				StackItemType func10 = cecil_type_to_stacktype(fieldReference4.FieldType);
				fSharpFunc8.Invoke(func10);
				break;
			}
			case 71:
			{
				FieldReference fieldReference3 = ((MyInstruction.Ldsflda)instr).item;
				fSharpFunc8.Invoke(StackItemType.I);
				break;
			}
			case 72:
			{
				string text = ((MyInstruction.Ldstr)instr).item;
				fSharpFunc8.Invoke(StackItemType.O);
				break;
			}
			case 73:
			{
				TypeReference typeReference = ((MyInstruction.Ldtoken)instr).item;
				fSharpFunc8.Invoke(StackItemType.O);
				break;
			}
			case 74:
			{
				MethodReference methodReference4 = ((MyInstruction.LdtokenM)instr).item;
				fSharpFunc8.Invoke(StackItemType.O);
				break;
			}
			case 75:
				fSharpFunc3.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.I);
				break;
			case 77:
			{
				StackItemType func9 = fSharpFunc7.Invoke(null);
				fSharpFunc8.Invoke(func9);
				break;
			}
			case 12:
			{
				Type type4 = ((MyInstruction.Castclass)instr).item;
				fSharpFunc4.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.O);
				break;
			}
			case 78:
			{
				Type type3 = ((MyInstruction.Newarr)instr).item;
				fSharpFunc6.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.O);
				break;
			}
			case 82:
				fSharpFunc6.Invoke(null);
				break;
			case 59:
				fSharpFunc3.Invoke(null);
				fSharpFunc4.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.O);
				break;
			case 67:
				fSharpFunc3.Invoke(null);
				fSharpFunc4.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.I);
				break;
			case 60:
			case 61:
			case 62:
			case 64:
			case 65:
			case 66:
				fSharpFunc3.Invoke(null);
				fSharpFunc4.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.I4);
				break;
			case 63:
				fSharpFunc3.Invoke(null);
				fSharpFunc4.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.I8);
				break;
			case 68:
				fSharpFunc3.Invoke(null);
				fSharpFunc4.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.R4);
				break;
			case 69:
				fSharpFunc3.Invoke(null);
				fSharpFunc4.Invoke(null);
				fSharpFunc8.Invoke(StackItemType.R8);
				break;
			case 89:
			case 90:
			case 91:
			case 92:
			case 93:
			case 94:
			case 95:
			case 96:
				fSharpFunc6.Invoke(null);
				fSharpFunc3.Invoke(null);
				fSharpFunc4.Invoke(null);
				break;
			case 97:
			{
				FieldSpec fieldSpec = ((MyInstruction.Stfld)instr).item;
				fSharpFunc6.Invoke(null);
				break;
			}
			case 98:
			case 99:
			case 100:
			case 101:
			case 102:
			case 103:
			case 104:
				fSharpFunc6.Invoke(null);
				fSharpFunc5.Invoke(null);
				break;
			case 105:
			{
				Variable variable = ((MyInstruction.Stloc)instr).item;
				fSharpFunc6.Invoke(null);
				break;
			}
			case 106:
			{
				FieldReference fieldReference2 = ((MyInstruction.Stsfld)instr).item;
				fSharpFunc6.Invoke(null);
				break;
			}
			case 108:
				break;
			case 9:
			{
				MethodReference methodReference3 = ((MyInstruction.Call)instr).item;
				if (methodReference3.HasThis)
				{
					fSharpFunc6.Invoke(null);
				}
				int num7 = 0;
				int num8 = methodReference3.Parameters.Count - 1;
				if (num8 >= num7)
				{
					do
					{
						fSharpFunc6.Invoke(null);
						num7++;
					}
					while (num7 != num8 + 1);
				}
				if (!string.Equals(methodReference3.ReturnType.Name, "Void"))
				{
					StackItemType func6 = cecil_type_to_stacktype(methodReference3.ReturnType);
					fSharpFunc8.Invoke(func6);
				}
				break;
			}
			case 10:
			{
				Mono.Cecil.CallSite callSite = ((MyInstruction.Calli)instr).item;
				fSharpFunc6.Invoke(null);
				int num5 = 0;
				int num6 = callSite.Parameters.Count - 1;
				if (num6 >= num5)
				{
					do
					{
						fSharpFunc6.Invoke(null);
						num5++;
					}
					while (num5 != num6 + 1);
				}
				if (!string.Equals(callSite.ReturnType.Name, "Void"))
				{
					StackItemType func3 = cecil_type_to_stacktype(callSite.ReturnType);
					fSharpFunc8.Invoke(func3);
				}
				break;
			}
			case 11:
			{
				MethodReference methodReference2 = ((MyInstruction.Callvirt)instr).item;
				fSharpFunc6.Invoke(null);
				int num3 = 0;
				int num4 = methodReference2.Parameters.Count - 1;
				if (num4 >= num3)
				{
					do
					{
						fSharpFunc6.Invoke(null);
						num3++;
					}
					while (num3 != num4 + 1);
				}
				if (!string.Equals(methodReference2.ReturnType.Name, "Void"))
				{
					StackItemType func = cecil_type_to_stacktype(methodReference2.ReturnType);
					fSharpFunc8.Invoke(func);
				}
				break;
			}
			case 79:
			{
				MethodReference methodReference = ((MyInstruction.Newobj)instr).item;
				int num = 0;
				int num2 = methodReference.Parameters.Count - 1;
				if (num2 >= num)
				{
					do
					{
						fSharpFunc6.Invoke(null);
						num++;
					}
					while (num != num2 + 1);
				}
				fSharpFunc8.Invoke(StackItemType.O);
				break;
			}
			case 109:
				{
					fSharpFunc6.Invoke(null);
					break;
				}
				IL_0728:
				stackItemType5 = fSharpFunc.Invoke(null);
				stackItemType6 = stackItemType5;
				break;
			}
		}

		[CompilerGenerated]
		internal List<int> get_predecessors(int iblock)
		{
			List<int> list = new List<int>();
			int num = blocks[iblock];
			if (num - 1 >= 0)
			{
				MyInstruction myInstruction = a[num - 1];
				int num2;
				switch (myInstruction.Tag)
				{
				case 2:
				{
					MyInstruction.Br br = (MyInstruction.Br)myInstruction;
					Label label2 = br.item;
					num2 = 0;
					goto IL_025a;
				}
				case 117:
				{
					MyInstruction.Leave leave = (MyInstruction.Leave)myInstruction;
					Label label = leave.item;
					num2 = 0;
					goto IL_025a;
				}
				default:
					list.Add(iblock - 1);
					goto IL_02e2;
				case 109:
					break;
					IL_025a:
					if (num2 != 0)
					{
						goto default;
					}
					break;
				}
			}
			MyInstruction myInstruction2 = a[num];
			if (myInstruction2.Tag == 35)
			{
				MyInstruction.Label label3 = (MyInstruction.Label)myInstruction2;
				Label key = label3.item;
				List<int> value = null;
				Tuple<bool, List<int>> tuple = new Tuple<bool, List<int>>(branch_from_blocks.TryGetValue(key, out value), value);
				if (tuple.Item1)
				{
					List<int> item = tuple.Item2;
					list.AddRange(item);
				}
			}
			goto IL_02e2;
			IL_02e2:
			return list;
		}

		[CompilerGenerated]
		internal FSharpOption<Stack<StackItemType>>[] map_lookup_stack(IEnumerable<int> preds)
		{
			FSharpFunc<int, FSharpOption<Stack<StackItemType>>> mapping = new get_stack@1530(this);
			return ArrayModule.OfSeq(SeqModule.Map(mapping, preds));
		}

		[CompilerGenerated]
		internal void ensure_stacks_same(List<int> preds)
		{
			pred_blocks_with_stacks@1539 chooser = new pred_blocks_with_stacks@1539();
			FSharpFunc<int, FSharpOption<int>> mapping = new pred_blocks_with_stacks@1538-1(this);
			int[] array = ArrayModule.OfSeq(SeqModule.Choose(chooser, SeqModule.Map(mapping, preds)));
			FSharpTypeFunc fSharpTypeFunc = new are_stacks_equal@1541();
			FSharpFunc<int, Unit> fSharpFunc = new dump_block@1547(this);
			int num = array[0];
			for (int i = 1; i < array.Length; i++)
			{
				int num2 = array[i];
				Stack<StackItemType> stack = stacks[num];
				Stack<StackItemType> stack2 = stacks[num2];
				Stack<StackItemType> arg = stack2;
				Stack<StackItemType> arg2 = stack;
				if (!FSharpFunc<Stack<StackItemType>, Stack<StackItemType>>.InvokeFast((FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, bool>>)fSharpTypeFunc.Specialize<StackItemType>(), arg2, arg))
				{
					ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("STACK MISMATCH"));
					FSharpFunc<List<int>, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<List<int>, Unit>, TextWriter, Unit, Unit, List<int>>("    preds: %A"));
					fSharpFunc2.Invoke(preds);
					FSharpFunc<int[], Unit> fSharpFunc3 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<int[], Unit>, TextWriter, Unit, Unit, int[]>("    pred_blocks_with_stacks: %A"));
					int[] func = array;
					fSharpFunc3.Invoke(func);
					FSharpFunc<int, FSharpFunc<int, Unit>> clo = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<int, FSharpFunc<int, Unit>>, TextWriter, Unit, Unit, Tuple<int, int>>("    stack %d <> %d"));
					FSharpFunc<int, int>.InvokeFast(new clo@1570(clo), num, num2);
					fSharpFunc.Invoke(num);
					fSharpFunc.Invoke(num2);
					FSharpFunc<int, FSharpFunc<int, FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>>>> clo2 = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<int, FSharpFunc<int, FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>>>>, Unit, string, Unit, Tuple<int, int, Stack<StackItemType>, Stack<StackItemType>>>("stacks mismatch %d <> %d: %A <> %A"));
					FSharpFunc<int, int>.InvokeFast(new clo@1585-2(clo2), num, num2, stack, stack2);
				}
			}
		}

		[CompilerGenerated]
		internal FSharpOption<Stack<StackItemType>> verify_stack(List<int> preds)
		{
			pred_stacks@1588 chooser = new pred_stacks@1588();
			FSharpFunc<IEnumerable<int>, FSharpOption<Stack<StackItemType>>[]> fSharpFunc = new pred_stacks@1588-1(this);
			Stack<StackItemType>[] array = ArrayModule.Choose(chooser, fSharpFunc.Invoke(preds));
			if (array.Length > 0)
			{
				if (array.Length > 1)
				{
					ensure_stacks_same(preds);
				}
				Stack<StackItemType> stack = array[0];
				Stack<StackItemType> value = stack;
				return FSharpOption<Stack<StackItemType>>.Some(value);
			}
			return null;
		}

		[CompilerGenerated]
		internal Stack<a> copy_stack<a>(Stack<a> s)
		{
			return new Stack<a>(ArrayModule.Reverse(s.ToArray()));
		}

		[CompilerGenerated]
		internal void walk_block(int iblock)
		{
			Stack<StackItemType> stk = stacks[iblock];
			int num = blocks[iblock];
			int num2 = blocks[iblock + 1] - 1;
			int num3 = num;
			int num4 = num2;
			if (num4 >= num3)
			{
				do
				{
					MyInstruction instr = a[num3];
					fix_stack(stk, instr);
					num3++;
				}
				while (num3 != num4 + 1);
			}
		}

		[CompilerGenerated]
		internal void check_block_stack(int n)
		{
			List<int> preds = get_predecessors(n);
			Stack<StackItemType> value = null;
			Tuple<bool, Stack<StackItemType>> tuple = new Tuple<bool, Stack<StackItemType>>(stacks.TryGetValue(n, out value), value);
			if (tuple.Item1)
			{
				Stack<StackItemType> item = tuple.Item2;
				ensure_stacks_same(preds);
				return;
			}
			FSharpOption<Stack<StackItemType>> fSharpOption = verify_stack(preds);
			if (fSharpOption != null)
			{
				FSharpOption<Stack<StackItemType>> fSharpOption2 = fSharpOption;
				Stack<StackItemType> value2 = fSharpOption2.Value;
				Stack<StackItemType> s = value2;
				Stack<StackItemType> value3 = copy_stack(s);
				stacks.Add(n, value3);
				walk_block(n);
			}
		}

		[CompilationArgumentCounts(new int[] { 1, 1 })]
		[CompilerGenerated]
		internal void add_branch_from_to(int iblock, Label x)
		{
			List<int> list = d_get_or_add(branch_from_blocks, x, new q@1625());
			list.Add(iblock);
			int value = 0;
			Tuple<bool, int> tuple = new Tuple<bool, int>(label_to_block.TryGetValue(x, out value), value);
			if (tuple.Item1)
			{
				int item = tuple.Item2;
				check_block_stack(item);
			}
		}
	}

	[Serializable]
	internal sealed class a@1735-1 : FSharpFunc<Unit, List<Variable>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal a@1735-1()
		{
		}

		public override List<Variable> Invoke(Unit unitVar0)
		{
			return new List<Variable>();
		}
	}

	[Serializable]
	internal sealed class a@1742-2 : FSharpFunc<Unit, List<Variable>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal a@1742-2()
		{
		}

		public override List<Variable> Invoke(Unit unitVar0)
		{
			return new List<Variable>();
		}
	}

	[Serializable]
	internal sealed class a@1752-3 : FSharpFunc<Unit, List<Variable>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal a@1752-3()
		{
		}

		public override List<Variable> Invoke(Unit unitVar0)
		{
			return new List<Variable>();
		}
	}

	[Serializable]
	internal sealed class a@1805-4 : FSharpFunc<MyInstruction, bool>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal a@1805-4()
		{
		}

		public override bool Invoke(MyInstruction x)
		{
			return x.Tag != 80;
		}
	}

	[Serializable]
	internal sealed class make_labelmap@988 : FSharpFunc<Unit, Dictionary<Label, List<int>>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public CilWriter @this;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal make_labelmap@988(CilWriter @this)
		{
			this.@this = @this;
		}

		public override Dictionary<Label, List<int>> Invoke(Unit unitVar0)
		{
			Dictionary<Label, List<int>> dictionary = new Dictionary<Label, List<int>>();
			int num = 0;
			int num2 = this.@this.a.Count - 1;
			if (num2 >= num)
			{
				do
				{
					MyInstruction myInstruction = this.@this.a[num];
					MyInstruction myInstruction2 = myInstruction;
					Label key;
					switch (myInstruction2.Tag)
					{
					case 3:
						key = ((MyInstruction.Brfalse)myInstruction2).item;
						break;
					case 4:
						key = ((MyInstruction.Brtrue)myInstruction2).item;
						break;
					case 117:
						key = ((MyInstruction.Leave)myInstruction2).item;
						break;
					case 2:
						key = ((MyInstruction.Br)myInstruction2).item;
						break;
					default:
						goto IL_02c0;
					}
					List<int> value = null;
					Tuple<bool, List<int>> tuple = new Tuple<bool, List<int>>(dictionary.TryGetValue(key, out value), value);
					List<int> list;
					if (tuple.Item1)
					{
						List<int> item = tuple.Item2;
						list = item;
					}
					else
					{
						List<int> list2 = new List<int>();
						dictionary.Add(key, list2);
						list = list2;
					}
					List<int> list3 = list;
					list3.Add(num);
					goto IL_02c0;
					IL_02c0:
					num++;
				}
				while (num != num2 + 1);
			}
			return dictionary;
		}
	}

	[Serializable]
	internal sealed class pop_int@1136 : FSharpFunc<Unit, StackItemType>
	{
		public Stack<StackItemType> stk;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pop_int@1136(Stack<StackItemType> stk)
		{
			this.stk = stk;
		}

		public override StackItemType Invoke(Unit unitVar0)
		{
			StackItemType stackItemType = stk.Pop();
			StackItemType stackItemType2 = stackItemType;
			switch (stackItemType2.Tag)
			{
			case 2:
			case 3:
				return stackItemType;
			default:
			{
				StackItemType stackItemType3 = stackItemType2;
				FSharpFunc<StackItemType, StackItemType> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<StackItemType, StackItemType>, Unit, string, StackItemType, StackItemType>("pop %A but should be integer"));
				StackItemType func = stackItemType3;
				return fSharpFunc.Invoke(func);
			}
			}
		}
	}

	[Serializable]
	internal sealed class pop_num@1157-2 : FSharpFunc<Stack<StackItemType>, StackItemType>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<Stack<StackItemType>, StackItemType> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pop_num@1157-2(FSharpFunc<Stack<StackItemType>, StackItemType> clo2)
		{
			this.clo2 = clo2;
		}

		public override StackItemType Invoke(Stack<StackItemType> arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class pop_num@1157-1 : FSharpFunc<StackItemType, FSharpFunc<Stack<StackItemType>, StackItemType>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<StackItemType, FSharpFunc<Stack<StackItemType>, StackItemType>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pop_num@1157-1(FSharpFunc<StackItemType, FSharpFunc<Stack<StackItemType>, StackItemType>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<Stack<StackItemType>, StackItemType> Invoke(StackItemType arg10)
		{
			FSharpFunc<Stack<StackItemType>, StackItemType> clo = clo1.Invoke(arg10);
			return new pop_num@1157-2(clo);
		}
	}

	[Serializable]
	internal sealed class pop_num@1144 : FSharpFunc<Unit, StackItemType>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public CilWriter @this;

		public Stack<StackItemType> stk;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pop_num@1144(CilWriter @this, Stack<StackItemType> stk)
		{
			this.@this = @this;
			this.stk = stk;
		}

		public override StackItemType Invoke(Unit unitVar0)
		{
			StackItemType stackItemType = stk.Pop();
			StackItemType stackItemType2 = stackItemType;
			switch (stackItemType2.Tag)
			{
			case 0:
			case 2:
			case 3:
			case 4:
			case 5:
				return stackItemType;
			default:
			{
				StackItemType arg = stackItemType2;
				int num = Math.Max(this.@this.a.Count - 10, 0);
				ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<Unit, TextWriter, Unit, Unit, Unit>("FAILING:"));
				int num2 = num;
				int num3 = this.@this.a.Count - 1;
				if (num3 >= num2)
				{
					do
					{
						FSharpFunc<MyInstruction, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<MyInstruction, Unit>, TextWriter, Unit, Unit, MyInstruction>("%A"));
						MyInstruction func = this.@this.a[num2];
						fSharpFunc.Invoke(func);
						num2++;
					}
					while (num2 != num3 + 1);
				}
				FSharpFunc<StackItemType, FSharpFunc<Stack<StackItemType>, StackItemType>> clo = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<StackItemType, FSharpFunc<Stack<StackItemType>, StackItemType>>, Unit, string, StackItemType, Tuple<StackItemType, Stack<StackItemType>>>("pop %A but should be number, rest of stack: %A"));
				return FSharpFunc<StackItemType, Stack<StackItemType>>.InvokeFast(new pop_num@1157-1(clo), arg, stk);
			}
			}
		}
	}

	[Serializable]
	internal sealed class pop_I4@1160 : FSharpFunc<Unit, Unit>
	{
		public Stack<StackItemType> stk;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pop_I4@1160(Stack<StackItemType> stk)
		{
			this.stk = stk;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			StackItemType stackItemType = stk.Pop();
			if (stackItemType.Tag == 2)
			{
				return null;
			}
			StackItemType stackItemType2 = stackItemType;
			FSharpFunc<StackItemType, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<StackItemType, Unit>, Unit, string, Unit, StackItemType>("pop %A but should be I4"));
			StackItemType func = stackItemType2;
			return fSharpFunc.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class pop_O@1165 : FSharpFunc<Unit, Unit>
	{
		public Stack<StackItemType> stk;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pop_O@1165(Stack<StackItemType> stk)
		{
			this.stk = stk;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			StackItemType stackItemType = stk.Pop();
			if (stackItemType.Tag == 1)
			{
				return null;
			}
			StackItemType stackItemType2 = stackItemType;
			FSharpFunc<StackItemType, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<StackItemType, Unit>, Unit, string, Unit, StackItemType>("pop %A but should be O"));
			StackItemType func = stackItemType2;
			return fSharpFunc.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class pop_I@1170 : FSharpFunc<Unit, Unit>
	{
		public Stack<StackItemType> stk;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pop_I@1170(Stack<StackItemType> stk)
		{
			this.stk = stk;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			StackItemType stackItemType = stk.Pop();
			if (stackItemType.Tag == 0)
			{
				return null;
			}
			StackItemType stackItemType2 = stackItemType;
			FSharpFunc<StackItemType, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<StackItemType, Unit>, Unit, string, Unit, StackItemType>("pop %A but should be I"));
			StackItemType func = stackItemType2;
			return fSharpFunc.Invoke(func);
		}
	}

	[Serializable]
	internal sealed class pop@1175 : FSharpFunc<Unit, Unit>
	{
		public Stack<StackItemType> stk;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pop@1175(Stack<StackItemType> stk)
		{
			this.stk = stk;
		}

		public override Unit Invoke(Unit unitVar0)
		{
			StackItemType stackItemType = stk.Pop();
			StackItemType stackItemType2 = stackItemType;
			return null;
		}
	}

	[Serializable]
	internal sealed class pop_any@1178 : FSharpFunc<Unit, StackItemType>
	{
		public Stack<StackItemType> stk;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pop_any@1178(Stack<StackItemType> stk)
		{
			this.stk = stk;
		}

		public override StackItemType Invoke(Unit unitVar0)
		{
			return stk.Pop();
		}
	}

	[Serializable]
	internal sealed class push@1181 : FSharpFunc<StackItemType, Unit>
	{
		public Stack<StackItemType> stk;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal push@1181(Stack<StackItemType> stk)
		{
			this.stk = stk;
		}

		public override Unit Invoke(StackItemType t)
		{
			stk.Push(t);
			return null;
		}
	}

	[Serializable]
	internal sealed class get_stack@1530 : FSharpFunc<int, FSharpOption<Stack<StackItemType>>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public CilWriter @this;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal get_stack@1530(CilWriter @this)
		{
			this.@this = @this;
		}

		public override FSharpOption<Stack<StackItemType>> Invoke(int q)
		{
			Stack<StackItemType> value = null;
			Tuple<bool, Stack<StackItemType>> tuple = new Tuple<bool, Stack<StackItemType>>(this.@this.stacks.TryGetValue(q, out value), value);
			if (tuple.Item1)
			{
				Stack<StackItemType> item = tuple.Item2;
				return FSharpOption<Stack<StackItemType>>.Some(item);
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class pred_blocks_with_stacks@1539 : FSharpFunc<FSharpOption<int>, FSharpOption<int>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pred_blocks_with_stacks@1539()
		{
		}

		public override FSharpOption<int> Invoke(FSharpOption<int> x)
		{
			return x;
		}
	}

	[Serializable]
	internal sealed class pred_blocks_with_stacks@1538-1 : FSharpFunc<int, FSharpOption<int>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public CilWriter @this;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pred_blocks_with_stacks@1538-1(CilWriter @this)
		{
			this.@this = @this;
		}

		public override FSharpOption<int> Invoke(int q)
		{
			if (this.@this.stacks.ContainsKey(q))
			{
				return FSharpOption<int>.Some(q);
			}
			return null;
		}
	}

	[Serializable]
	internal sealed class are_stacks_equal@1541 : FSharpTypeFunc
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal are_stacks_equal@1541()
		{
		}

		[CompilerGenerated]
		[DebuggerNonUserCode]
		public override object Specialize<a>()
		{
			return new are_stacks_equal@1541T<a>(this);
		}
	}

	[Serializable]
	internal sealed class are_stacks_equal@1541T<a> : OptimizedClosures.FSharpFunc<Stack<a>, Stack<a>, bool>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public are_stacks_equal@1541 self0@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal are_stacks_equal@1541T(are_stacks_equal@1541 self0@)
		{
			this.self0@ = self0@;
		}

		public override bool Invoke(Stack<a> s1, Stack<a> s2)
		{
			are_stacks_equal@1541 are_stacks_equal@1542 = self0@;
			a[] x = s1.ToArray();
			a[] y = s2.ToArray();
			return LanguagePrimitives.HashCompare.GenericEqualityIntrinsic(x, y);
		}
	}

	[Serializable]
	internal sealed class dump_block@1552-2 : FSharpFunc<int, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal dump_block@1552-2(FSharpFunc<int, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(int arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class dump_block@1552-1 : FSharpFunc<int, FSharpFunc<int, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, FSharpFunc<int, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal dump_block@1552-1(FSharpFunc<int, FSharpFunc<int, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<int, Unit> Invoke(int arg10)
		{
			FSharpFunc<int, Unit> clo = clo1.Invoke(arg10);
			return new dump_block@1552-2(clo);
		}
	}

	[Serializable]
	internal sealed class dump_block@1547 : FSharpFunc<int, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public CilWriter @this;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal dump_block@1547(CilWriter @this)
		{
			this.@this = @this;
		}

		public override Unit Invoke(int n)
		{
			FSharpFunc<int, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<int, Unit>, TextWriter, Unit, Unit, int>("---- block: %d"));
			fSharpFunc.Invoke(n);
			Stack<StackItemType> stack = this.@this.stacks[n];
			FSharpFunc<Stack<StackItemType>, Unit> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<Stack<StackItemType>, Unit>, TextWriter, Unit, Unit, Stack<StackItemType>>("stack: %A"));
			Stack<StackItemType> func = stack;
			fSharpFunc2.Invoke(func);
			int num = this.@this.blocks[n];
			int num2 = this.@this.blocks[n + 1] - 1;
			FSharpFunc<int, FSharpFunc<int, Unit>> clo = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<int, FSharpFunc<int, Unit>>, TextWriter, Unit, Unit, Tuple<int, int>>("instructions %d through %d"));
			FSharpFunc<int, int>.InvokeFast(new dump_block@1552-1(clo), num, num2);
			int num3 = num;
			int num4 = num2;
			if (num4 >= num3)
			{
				do
				{
					MyInstruction myInstruction = this.@this.a[num3];
					FSharpFunc<MyInstruction, Unit> fSharpFunc3 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<MyInstruction, Unit>, TextWriter, Unit, Unit, MyInstruction>("%A"));
					MyInstruction func2 = myInstruction;
					fSharpFunc3.Invoke(func2);
					num3++;
				}
				while (num3 != num4 + 1);
			}
			List<int> list = this.@this.get_predecessors(n);
			FSharpFunc<List<int>, Unit> fSharpFunc4 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<List<int>, Unit>, TextWriter, Unit, Unit, List<int>>("preds: %A"));
			List<int> func3 = list;
			fSharpFunc4.Invoke(func3);
			List<int> list2 = list;
			List<int>.Enumerator enumerator = list2.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					Invoke(current);
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
	internal sealed class clo@1570-1 : FSharpFunc<int, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, Unit> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal clo@1570-1(FSharpFunc<int, Unit> clo2)
		{
			this.clo2 = clo2;
		}

		public override Unit Invoke(int arg20)
		{
			return clo2.Invoke(arg20);
		}
	}

	[Serializable]
	internal sealed class clo@1570 : FSharpFunc<int, FSharpFunc<int, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, FSharpFunc<int, Unit>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal clo@1570(FSharpFunc<int, FSharpFunc<int, Unit>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<int, Unit> Invoke(int arg10)
		{
			FSharpFunc<int, Unit> clo = clo1.Invoke(arg10);
			return new clo@1570-1(clo);
		}
	}

	[Serializable]
	internal sealed class clo@1585-5 : FSharpFunc<Stack<StackItemType>, Unit>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<Stack<StackItemType>, Unit> clo4;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal clo@1585-5(FSharpFunc<Stack<StackItemType>, Unit> clo4)
		{
			this.clo4 = clo4;
		}

		public override Unit Invoke(Stack<StackItemType> arg40)
		{
			return clo4.Invoke(arg40);
		}
	}

	[Serializable]
	internal sealed class clo@1585-4 : FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>> clo3;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal clo@1585-4(FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>> clo3)
		{
			this.clo3 = clo3;
		}

		public override FSharpFunc<Stack<StackItemType>, Unit> Invoke(Stack<StackItemType> arg30)
		{
			FSharpFunc<Stack<StackItemType>, Unit> clo = clo3.Invoke(arg30);
			return new clo@1585-5(clo);
		}
	}

	[Serializable]
	internal sealed class clo@1585-3 : FSharpFunc<int, FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>>> clo2;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal clo@1585-3(FSharpFunc<int, FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>>> clo2)
		{
			this.clo2 = clo2;
		}

		public override FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>> Invoke(int arg20)
		{
			FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>> clo = clo2.Invoke(arg20);
			return new clo@1585-4(clo);
		}
	}

	[Serializable]
	internal sealed class clo@1585-2 : FSharpFunc<int, FSharpFunc<int, FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>>>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public FSharpFunc<int, FSharpFunc<int, FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>>>> clo1;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal clo@1585-2(FSharpFunc<int, FSharpFunc<int, FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>>>> clo1)
		{
			this.clo1 = clo1;
		}

		public override FSharpFunc<int, FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>>> Invoke(int arg10)
		{
			FSharpFunc<int, FSharpFunc<Stack<StackItemType>, FSharpFunc<Stack<StackItemType>, Unit>>> clo = clo1.Invoke(arg10);
			return new clo@1585-3(clo);
		}
	}

	[Serializable]
	internal sealed class pred_stacks@1588 : FSharpFunc<FSharpOption<Stack<StackItemType>>, FSharpOption<Stack<StackItemType>>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pred_stacks@1588()
		{
		}

		public override FSharpOption<Stack<StackItemType>> Invoke(FSharpOption<Stack<StackItemType>> x)
		{
			return x;
		}
	}

	[Serializable]
	internal sealed class pred_stacks@1588-1 : FSharpFunc<IEnumerable<int>, FSharpOption<Stack<StackItemType>>[]>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
		public CilWriter arg00@;

		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal pred_stacks@1588-1(CilWriter arg00@)
		{
			this.arg00@ = arg00@;
		}

		public override FSharpOption<Stack<StackItemType>>[] Invoke(IEnumerable<int> preds)
		{
			return arg00@.map_lookup_stack(preds);
		}
	}

	[Serializable]
	internal sealed class q@1625 : FSharpFunc<Unit, List<int>>
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		internal q@1625()
		{
		}

		public override List<int> Invoke(Unit unitVar0)
		{
			return new List<int>();
		}
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

	public static int sizeof_funnyinteger_rounded_up(FunnyIntegerType t)
	{
		int num = t.bits@ / 8;
		if (t.bits@ % 8 == 0)
		{
			return num;
		}
		return num + 1;
	}

	public static PrimitiveType vecprim_to_prim(VecPrimitiveType t)
	{
		return t.Tag switch
		{
			1 => PrimitiveType.I16, 
			2 => PrimitiveType.I32, 
			3 => PrimitiveType.I64, 
			4 => PrimitiveType.F32, 
			5 => PrimitiveType.F64, 
			_ => PrimitiveType.I8, 
		};
	}

	public static FirstClassType vec_elem_type_to_first_class(VectorElementType t)
	{
		switch (t.Tag)
		{
		default:
			return FirstClassType.NewPrimitiveType(PrimitiveType.I1);
		case 1:
			return FirstClassType.Ptr;
		case 2:
		{
			VectorElementType.VecPrim vecPrim = (VectorElementType.VecPrim)t;
			VecPrimitiveType t2 = vecPrim.item;
			PrimitiveType primitiveType = vecprim_to_prim(t2);
			PrimitiveType item2 = primitiveType;
			return FirstClassType.NewPrimitiveType(item2);
		}
		case 3:
		{
			VectorElementType.VecFunny vecFunny = (VectorElementType.VecFunny)t;
			FunnyIntegerType item = vecFunny.item;
			return FirstClassType.NewFunnyIntegerType(item);
		}
		}
	}

	internal static string fctype_to_abbrev(FirstClassType ft)
	{
		switch (ft.Tag)
		{
		default:
		{
			FirstClassType.PrimitiveType primitiveType = (FirstClassType.PrimitiveType)ft;
			PrimitiveType pt = primitiveType.item;
			return prim_to_abbrev(pt);
		}
		case 2:
		{
			FirstClassType.StructType structType = (FirstClassType.StructType)ft;
			StructType st = structType.item;
			return struct_type_to_abbrev(st);
		}
		case 3:
		{
			FirstClassType.ArrayType arrayType = (FirstClassType.ArrayType)ft;
			ArrayType arrayType2 = arrayType.item;
			FSharpFunc<string, FSharpFunc<uint, string>> clo2 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, FSharpFunc<uint, string>>, Unit, string, string, Tuple<string, uint>>("arr_%s_%d"));
			return FSharpFunc<string, uint>.InvokeFast(new fctype_to_abbrev@124(clo2), fctype_to_abbrev(arrayType2.elemtype@), arrayType2.count@);
		}
		case 1:
		{
			FirstClassType.VectorType vectorType = (FirstClassType.VectorType)ft;
			VectorType vectorType2 = vectorType.item;
			FSharpFunc<string, FSharpFunc<uint, string>> clo = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, FSharpFunc<uint, string>>, Unit, string, string, Tuple<string, uint>>("vec_%s_%d"));
			return FSharpFunc<string, uint>.InvokeFast(new fctype_to_abbrev@126-2(clo), fctype_to_abbrev(vec_elem_type_to_first_class(vectorType2.elemtype@)), vectorType2.count@);
		}
		case 4:
		{
			FirstClassType.FunnyIntegerType funnyIntegerType = (FirstClassType.FunnyIntegerType)ft;
			FunnyIntegerType funnyIntegerType2 = funnyIntegerType.item;
			FSharpFunc<int, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<int, string>, Unit, string, string, int>("i%d"));
			int bits@ = funnyIntegerType2.bits@;
			return fSharpFunc.Invoke(bits@);
		}
		case 5:
			return "ptr";
		}
	}

	internal static string struct_type_to_abbrev(StructType st)
	{
		FSharpFunc<StructItem, string> fSharpFunc = new a@131();
		StructItem[] items@ = st.items@;
		if (items@ == null)
		{
			throw new ArgumentNullException("array");
		}
		string[] array = new string[items@.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = fSharpFunc.Invoke(items@[i]);
		}
		string[] value = array;
		string arg = string.Join(",", value);
		FSharpFunc<string, FSharpFunc<string, string>> clo = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, FSharpFunc<string, string>>, Unit, string, string, Tuple<string, string>>("struct_%s{%s}"));
		return FSharpFunc<string, string>.InvokeFast(new struct_type_to_abbrev@133(clo), (!string.Equals(st.name@, null)) ? st.name@ : "", arg);
	}

	internal static string prim_to_abbrev(PrimitiveType pt)
	{
		return pt.Tag switch
		{
			1 => "i8", 
			2 => "i16", 
			3 => "i32", 
			4 => "i64", 
			5 => "f32", 
			6 => "f64", 
			_ => "i1", 
		};
	}

	internal static string make_struct_name<a>(a st)
	{
		FSharpFunc<a, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<a, string>, Unit, string, string, a>("%A"));
		return fSharpFunc.Invoke(st).Replace(" ", "").Replace("\n", "")
			.Replace("\r", "")
			.Replace("\t", "")
			.Replace("[", "")
			.Replace("]", "")
			.Replace("|", "")
			.Replace("{", "")
			.Replace("}", "")
			.Replace(";", "");
	}

	public static int sizeof_vecprim(VecPrimitiveType pt)
	{
		return pt.Tag switch
		{
			1 => 2, 
			2 => 4, 
			3 => 8, 
			4 => 4, 
			5 => 8, 
			_ => 1, 
		};
	}

	public static int sizeof_primitive(PrimitiveType pt)
	{
		return pt.Tag switch
		{
			1 => 1, 
			2 => 2, 
			3 => 4, 
			4 => 8, 
			5 => 4, 
			6 => 8, 
			_ => 1, 
		};
	}

	public static int get_llvm_size_in_bytes(ulong llvm_size_in_bits)
	{
		int num = (int)(llvm_size_in_bits / 8);
		int num2 = (int)(llvm_size_in_bits % 8);
		return num + ((num2 != 0) ? 1 : 0);
	}

	public static int get_sizeof(FirstClassType t)
	{
		switch (t.Tag)
		{
		default:
		{
			FirstClassType.PrimitiveType primitiveType = (FirstClassType.PrimitiveType)t;
			PrimitiveType pt = primitiveType.item;
			return sizeof_primitive(pt);
		}
		case 1:
		{
			FirstClassType.VectorType vectorType = (FirstClassType.VectorType)t;
			VectorType t4 = vectorType.item;
			return sizeof_vector(t4);
		}
		case 3:
		{
			FirstClassType.ArrayType arrayType = (FirstClassType.ArrayType)t;
			ArrayType t3 = arrayType.item;
			return sizeof_array(t3);
		}
		case 2:
		{
			FirstClassType.StructType structType = (FirstClassType.StructType)t;
			StructType st = structType.item;
			return sizeof_struct(st);
		}
		case 4:
		{
			FirstClassType.FunnyIntegerType funnyIntegerType = (FirstClassType.FunnyIntegerType)t;
			FunnyIntegerType t2 = funnyIntegerType.item;
			return sizeof_funnyinteger_rounded_up(t2);
		}
		case 5:
			return 8;
		}
	}

	public static int sizeof_struct(StructType st)
	{
		if (st.items@.Length > 0)
		{
			StructItem structItem = st.items@[st.items@.Length - 1];
			int num = (int)structItem.off@ + get_sizeof(structItem.typ@);
			int num2 = get_llvm_size_in_bytes(st.llvm_size_in_bits@);
			if (num != num2)
			{
			}
			return num2;
		}
		return 0;
	}

	public static int sizeof_vec_elem(VectorElementType t)
	{
		switch (t.Tag)
		{
		default:
		{
			VectorElementType.VecPrim vecPrim = (VectorElementType.VecPrim)t;
			VecPrimitiveType t3 = vecPrim.item;
			return sizeof_primitive(vecprim_to_prim(t3));
		}
		case 3:
		{
			VectorElementType.VecFunny vecFunny = (VectorElementType.VecFunny)t;
			FunnyIntegerType t2 = vecFunny.item;
			return sizeof_funnyinteger_rounded_up(t2);
		}
		case 0:
			return sizeof_primitive(PrimitiveType.I1);
		case 1:
			return get_sizeof(FirstClassType.Ptr);
		}
	}

	public static int sizeof_vector(VectorType t)
	{
		return (int)t.count@ * sizeof_vec_elem(t.elemtype@);
	}

	public static int sizeof_array(ArrayType t)
	{
		return (int)t.count@ * get_sizeof(t.elemtype@);
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	internal static TypeReference create_funny_int_struct(GenTypes typs, FunnyIntegerType it)
	{
		string ns@ = typs.ns@;
		FSharpFunc<int, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<int, string>, Unit, string, string, int>("i%d"));
		int bits@ = it.bits@;
		TypeDefinition typeDefinition = new TypeDefinition(ns@, fSharpFunc.Invoke(bits@), TypeAttributes.SequentialLayout | TypeAttributes.Sealed, typs.md@.ImportReference(typeof(ValueType)));
		int num = sizeof_funnyinteger_rounded_up(it);
		int num2 = 0;
		int num3 = num - 1;
		if (num3 >= num2)
		{
			do
			{
				FieldAttributes attributes = FieldAttributes.Public;
				FSharpFunc<int, string> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<int, string>, Unit, string, string, int>("f%d"));
				int func = num2;
				FieldDefinition item = new FieldDefinition(fSharpFunc2.Invoke(func), attributes, typs.md@.TypeSystem.Byte);
				typeDefinition.Fields.Add(item);
				num2++;
			}
			while (num2 != num3 + 1);
		}
		typs.md@.Types.Add(typeDefinition);
		return typeDefinition;
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	internal static TypeReference get_or_create_funny_int_struct(GenTypes typs, FunnyIntegerType it)
	{
		uint bits@ = (uint)it.bits@;
		TypeReference value;
		Tuple<bool, TypeReference> tuple = new Tuple<bool, TypeReference>(typs.d_funnyints@.TryGetValue(bits@, out value), value);
		if (tuple.Item1)
		{
			return tuple.Item2;
		}
		TypeReference typeReference = create_funny_int_struct(typs, it);
		typs.d_funnyints@.Add(bits@, typeReference);
		return typeReference;
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	internal static TypeReference primitivetype_to_ceciltype(GenTypes typs, PrimitiveType vt)
	{
		return vt.Tag switch
		{
			1 => typs.md@.TypeSystem.SByte, 
			2 => typs.md@.TypeSystem.Int16, 
			3 => typs.md@.TypeSystem.Int32, 
			4 => typs.md@.TypeSystem.Int64, 
			5 => typs.md@.TypeSystem.Single, 
			6 => typs.md@.TypeSystem.Double, 
			_ => typs.md@.TypeSystem.SByte, 
		};
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static TypeReference firstclass_type_to_cecil_type(GenTypes typs, FirstClassType vt)
	{
		switch (vt.Tag)
		{
		case 1:
		{
			FirstClassType.VectorType vectorType = (FirstClassType.VectorType)vt;
			VectorType vectorType2;
			switch (vectorType.item.count@)
			{
			default:
				switch (vectorType.item.count@)
				{
				default:
					switch (vectorType.item.count@)
					{
					default:
						switch (vectorType.item.count@)
						{
						default:
							switch (vectorType.item.count@)
							{
							case 32u:
								if (vectorType.item.elemtype@.Tag == 2)
								{
									VectorElementType.VecPrim vecPrim5 = (VectorElementType.VecPrim)vectorType.item.elemtype@;
									if (vecPrim5.item.Tag == 0)
									{
										Type type16 = Vector256<sbyte>.Zero.GetType();
										return typs.md@.ImportReference(type16);
									}
									vectorType2 = vectorType.item;
								}
								else
								{
									vectorType2 = vectorType.item;
								}
								break;
							default:
								vectorType2 = vectorType.item;
								break;
							}
							break;
						case 16u:
							if (vectorType.item.elemtype@.Tag == 2)
							{
								VectorElementType.VecPrim vecPrim4 = (VectorElementType.VecPrim)vectorType.item.elemtype@;
								switch (vecPrim4.item.Tag)
								{
								case 0:
								{
									Type type15 = Vector128<sbyte>.Zero.GetType();
									return typs.md@.ImportReference(type15);
								}
								case 1:
								{
									Type type14 = Vector256<short>.Zero.GetType();
									return typs.md@.ImportReference(type14);
								}
								}
								vectorType2 = vectorType.item;
							}
							else
							{
								vectorType2 = vectorType.item;
							}
							break;
						}
						break;
					case 8u:
						if (vectorType.item.elemtype@.Tag == 2)
						{
							VectorElementType.VecPrim vecPrim3 = (VectorElementType.VecPrim)vectorType.item.elemtype@;
							switch (vecPrim3.item.Tag)
							{
							case 0:
							{
								Type type13 = Vector64<sbyte>.Zero.GetType();
								return typs.md@.ImportReference(type13);
							}
							case 1:
							{
								Type type12 = Vector128<short>.Zero.GetType();
								return typs.md@.ImportReference(type12);
							}
							case 2:
							{
								Type type11 = Vector256<int>.Zero.GetType();
								return typs.md@.ImportReference(type11);
							}
							case 4:
							{
								Type type10 = Vector256<float>.Zero.GetType();
								return typs.md@.ImportReference(type10);
							}
							}
							vectorType2 = vectorType.item;
						}
						else
						{
							vectorType2 = vectorType.item;
						}
						break;
					}
					break;
				case 4u:
					if (vectorType.item.elemtype@.Tag == 2)
					{
						VectorElementType.VecPrim vecPrim2 = (VectorElementType.VecPrim)vectorType.item.elemtype@;
						switch (vecPrim2.item.Tag)
						{
						case 1:
						{
							Type type9 = Vector64<short>.Zero.GetType();
							return typs.md@.ImportReference(type9);
						}
						case 2:
						{
							Type type8 = Vector128<int>.Zero.GetType();
							return typs.md@.ImportReference(type8);
						}
						case 3:
						{
							Type type7 = Vector256<long>.Zero.GetType();
							return typs.md@.ImportReference(type7);
						}
						case 4:
						{
							Type type6 = Vector128<float>.Zero.GetType();
							return typs.md@.ImportReference(type6);
						}
						case 5:
						{
							Type type5 = Vector256<double>.Zero.GetType();
							return typs.md@.ImportReference(type5);
						}
						}
						vectorType2 = vectorType.item;
					}
					else
					{
						vectorType2 = vectorType.item;
					}
					break;
				}
				break;
			case 2u:
				if (vectorType.item.elemtype@.Tag == 2)
				{
					VectorElementType.VecPrim vecPrim = (VectorElementType.VecPrim)vectorType.item.elemtype@;
					switch (vecPrim.item.Tag)
					{
					case 2:
					{
						Type type4 = Vector64<int>.Zero.GetType();
						return typs.md@.ImportReference(type4);
					}
					case 3:
					{
						Type type3 = Vector128<long>.Zero.GetType();
						return typs.md@.ImportReference(type3);
					}
					case 4:
					{
						Type type2 = Vector64<float>.Zero.GetType();
						return typs.md@.ImportReference(type2);
					}
					case 5:
					{
						Type type = Vector128<double>.Zero.GetType();
						return typs.md@.ImportReference(type);
					}
					}
					vectorType2 = vectorType.item;
				}
				else
				{
					vectorType2 = vectorType.item;
				}
				break;
			}
			TypeReference value2 = null;
			Tuple<bool, TypeReference> tuple2 = new Tuple<bool, TypeReference>(typs.d_vectors@.TryGetValue(vectorType2, out value2), value2);
			if (tuple2.Item1)
			{
				return tuple2.Item2;
			}
			string ns@2 = typs.ns@;
			FSharpFunc<VectorElementType, FSharpFunc<uint, string>> clo3 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<VectorElementType, FSharpFunc<uint, string>>, Unit, string, string, Tuple<VectorElementType, uint>>("vec_%A_%d"));
			TypeDefinition typeDefinition2 = new TypeDefinition(ns@2, FSharpFunc<VectorElementType, uint>.InvokeFast(new ty@336(clo3), vectorType2.elemtype@, vectorType2.count@), TypeAttributes.ExplicitLayout | TypeAttributes.Sealed, typs.md@.ImportReference(typeof(ValueType)));
			int num3 = get_llvm_size_in_bytes(vectorType2.llvm_size_in_bits@);
			int num4 = (int)vectorType2.count@ * sizeof_vec_elem(vectorType2.elemtype@);
			if (num4 != num3)
			{
				FSharpFunc<int, FSharpFunc<int, FSharpFunc<VectorType, Unit>>> clo4 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<int, FSharpFunc<int, FSharpFunc<VectorType, Unit>>>, TextWriter, Unit, Unit, Tuple<int, int, VectorType>>("VECTOR SIZE MISMATCH %d (calc) vs %d (llvm): %A"));
				FSharpFunc<int, int>.InvokeFast(new firstclass_type_to_cecil_type@344(clo4), num4, num3, vectorType2);
			}
			typeDefinition2.ClassSize = num4;
			typeDefinition2.PackingSize = (short)0;
			typs.d_vectors@.Add(vectorType2, typeDefinition2);
			typs.md@.Types.Add(typeDefinition2);
			return typeDefinition2;
		}
		default:
			return typs.md@.TypeSystem.IntPtr;
		case 0:
		{
			PrimitiveType vt2 = ((FirstClassType.PrimitiveType)vt).item;
			return primitivetype_to_ceciltype(typs, vt2);
		}
		case 2:
		{
			StructType st = ((FirstClassType.StructType)vt).item;
			return struct_type_to_cecil_type(typs, st);
		}
		case 4:
		{
			FunnyIntegerType it = ((FirstClassType.FunnyIntegerType)vt).item;
			return get_or_create_funny_int_struct(typs, it);
		}
		case 3:
		{
			ArrayType arrayType = ((FirstClassType.ArrayType)vt).item;
			TypeReference value = null;
			Tuple<bool, TypeReference> tuple = new Tuple<bool, TypeReference>(typs.d_arrays@.TryGetValue(arrayType, out value), value);
			if (tuple.Item1)
			{
				return tuple.Item2;
			}
			string ns@ = typs.ns@;
			FSharpFunc<string, FSharpFunc<uint, string>> clo = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<string, FSharpFunc<uint, string>>, Unit, string, string, Tuple<string, uint>>("arr_%s_%d"));
			TypeDefinition typeDefinition = new TypeDefinition(ns@, FSharpFunc<string, uint>.InvokeFast(new ty@377-2(clo), fctype_to_abbrev(arrayType.elemtype@), arrayType.count@), TypeAttributes.ExplicitLayout | TypeAttributes.Sealed, typs.md@.ImportReference(typeof(ValueType)));
			int num = get_llvm_size_in_bytes(arrayType.llvm_size_in_bits@);
			int num2 = (int)arrayType.count@ * get_sizeof(arrayType.elemtype@);
			if (num2 != num)
			{
				FSharpFunc<int, FSharpFunc<int, FSharpFunc<ArrayType, Unit>>> clo2 = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<int, FSharpFunc<int, FSharpFunc<ArrayType, Unit>>>, TextWriter, Unit, Unit, Tuple<int, int, ArrayType>>("ARRAY SIZE MISMATCH %d (calc) vs %d (llvm): %A"));
				FSharpFunc<int, int>.InvokeFast(new firstclass_type_to_cecil_type@385-3(clo2), num2, num, arrayType);
			}
			typeDefinition.ClassSize = num;
			typeDefinition.PackingSize = (short)0;
			typs.d_arrays@.Add(arrayType, typeDefinition);
			typs.md@.Types.Add(typeDefinition);
			return typeDefinition;
		}
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static TypeReference struct_type_to_cecil_type(GenTypes typs, StructType st)
	{
		TypeReference value = null;
		Tuple<bool, TypeReference> tuple = new Tuple<bool, TypeReference>(typs.d_structs@.TryGetValue(st, out value), value);
		if (tuple.Item1)
		{
			return tuple.Item2;
		}
		string name = struct_type_to_abbrev(st);
		TypeDefinition typeDefinition = new TypeDefinition(typs.ns@, name, TypeAttributes.ExplicitLayout | TypeAttributes.Sealed, typs.md@.ImportReference(typeof(ValueType)));
		typeDefinition.ClassSize = get_llvm_size_in_bytes(st.llvm_size_in_bits@);
		typeDefinition.PackingSize = (short)0;
		for (int i = 0; i < st.items@.Length; i++)
		{
			StructItem structItem = st.items@[i];
			TypeReference fieldType = firstclass_type_to_cecil_type(typs, structItem.typ@);
			FieldAttributes attributes = FieldAttributes.Public;
			FSharpFunc<int, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<int, string>, Unit, string, string, int>("f%d"));
			int func = i;
			FieldDefinition fieldDefinition = new FieldDefinition(fSharpFunc.Invoke(func), attributes, fieldType);
			fieldDefinition.Offset = (int)structItem.off@;
			typeDefinition.Fields.Add(fieldDefinition);
		}
		typs.d_structs@.Add(st, typeDefinition);
		typs.md@.Types.Add(typeDefinition);
		return typeDefinition;
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static TypeReference general_type_to_cecil_type(GenTypes typs, GeneralType vt)
	{
		switch (vt.Tag)
		{
		default:
		{
			GeneralType.SystemType systemType = (GeneralType.SystemType)vt;
			Type type = systemType.item;
			return typs.md@.ImportReference(type);
		}
		case 2:
		{
			GeneralType.TypeReference typeReference = (GeneralType.TypeReference)vt;
			return typeReference.item;
		}
		case 3:
		{
			GeneralType.FirstClassType firstClassType = (GeneralType.FirstClassType)vt;
			FirstClassType vt2 = firstClassType.item;
			return firstclass_type_to_cecil_type(typs, vt2);
		}
		case 0:
		{
			GeneralType.VarArgType varArgType = (GeneralType.VarArgType)vt;
			int num = varArgType.item;
			TypeReference value = null;
			Tuple<bool, TypeReference> tuple = new Tuple<bool, TypeReference>(typs.d_vararg@.TryGetValue(num, out value), value);
			if (tuple.Item1)
			{
				return tuple.Item2;
			}
			FSharpFunc<int, string> fSharpFunc = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<int, string>, Unit, string, string, int>("vararg_%d"));
			int func = num;
			string name = fSharpFunc.Invoke(func);
			TypeDefinition typeDefinition = new TypeDefinition(typs.ns@, name, TypeAttributes.SequentialLayout | TypeAttributes.Sealed, typs.md@.ImportReference(typeof(ValueType)));
			FieldAttributes attributes = FieldAttributes.Public;
			TypeReference @int = typs.md@.TypeSystem.Int64;
			typeDefinition.Fields.Add(new FieldDefinition("count", attributes, @int));
			int num2 = 0;
			int num3 = num - 1;
			if (num3 >= num2)
			{
				do
				{
					FSharpFunc<int, string> fSharpFunc2 = ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<int, string>, Unit, string, string, int>("f%d"));
					int func2 = num2;
					FieldDefinition item = new FieldDefinition(fSharpFunc2.Invoke(func2), attributes, @int);
					typeDefinition.Fields.Add(item);
					num2++;
				}
				while (num2 != num3 + 1);
			}
			typs.d_vararg@.Add(num, typeDefinition);
			typs.md@.Types.Add(typeDefinition);
			return typeDefinition;
		}
		}
	}

	public static StackItemType cecil_type_to_stacktype(TypeReference t)
	{
		if (string.Equals(t.FullName, "System.Int32"))
		{
			return StackItemType.I4;
		}
		if (string.Equals(t.FullName, "System.Int64"))
		{
			return StackItemType.I8;
		}
		if (string.Equals(t.FullName, "System.UInt32"))
		{
			return StackItemType.I4;
		}
		if (string.Equals(t.FullName, "System.UInt64"))
		{
			return StackItemType.I8;
		}
		if (string.Equals(t.FullName, "System.IntPtr"))
		{
			return StackItemType.I;
		}
		if (string.Equals(t.FullName, "System.Object"))
		{
			return StackItemType.O;
		}
		if (string.Equals(t.FullName, "System.SByte"))
		{
			return StackItemType.I4;
		}
		if (string.Equals(t.FullName, "System.Byte"))
		{
			return StackItemType.I4;
		}
		if (string.Equals(t.FullName, "System.Int16"))
		{
			return StackItemType.I4;
		}
		if (string.Equals(t.FullName, "System.TypeCode"))
		{
			return StackItemType.I4;
		}
		if (string.Equals(t.FullName, "System.Boolean"))
		{
			return StackItemType.I4;
		}
		if (string.Equals(t.FullName, "System.Double"))
		{
			return StackItemType.R8;
		}
		if (string.Equals(t.FullName, "System.Single"))
		{
			return StackItemType.R4;
		}
		if (string.Equals(t.FullName, "System.Runtime.InteropServices.GCHandle"))
		{
			return StackItemType.O;
		}
		if (string.Equals(t.FullName, "System.Type"))
		{
			return StackItemType.O;
		}
		if (t.IsGenericParameter)
		{
			return StackItemType.I8;
		}
		if (t.IsPointer)
		{
			return StackItemType.I;
		}
		return StackItemType.O;
	}

	public static StackItemType systype_to_stacktype(Type t)
	{
		if (string.Equals(t.FullName, "System.Int32"))
		{
			return StackItemType.I4;
		}
		if (string.Equals(t.FullName, "System.Int64"))
		{
			return StackItemType.I8;
		}
		if (string.Equals(t.FullName, "System.UInt64"))
		{
			return StackItemType.I8;
		}
		if (string.Equals(t.FullName, "System.UInt32"))
		{
			return StackItemType.I4;
		}
		return StackItemType.O;
	}

	public static StackItemType primitivetype_to_stacktype(PrimitiveType vt)
	{
		return vt.Tag switch
		{
			1 => StackItemType.I4, 
			2 => StackItemType.I4, 
			3 => StackItemType.I4, 
			4 => StackItemType.I8, 
			5 => StackItemType.R4, 
			6 => StackItemType.R8, 
			_ => StackItemType.I4, 
		};
	}

	public static StackItemType firstclass_type_to_stacktype(FirstClassType vt)
	{
		switch (vt.Tag)
		{
		default:
			return StackItemType.I;
		case 0:
		{
			FirstClassType.PrimitiveType primitiveType = (FirstClassType.PrimitiveType)vt;
			PrimitiveType vt2 = primitiveType.item;
			return primitivetype_to_stacktype(vt2);
		}
		case 1:
		{
			FirstClassType.VectorType vectorType = (FirstClassType.VectorType)vt;
			VectorType vectorType2 = vectorType.item;
			return StackItemType.O;
		}
		case 3:
		{
			FirstClassType.ArrayType arrayType = (FirstClassType.ArrayType)vt;
			ArrayType arrayType2 = arrayType.item;
			return StackItemType.O;
		}
		case 2:
		{
			FirstClassType.StructType structType = (FirstClassType.StructType)vt;
			StructType structType2 = structType.item;
			return StackItemType.O;
		}
		case 4:
		{
			FirstClassType.FunnyIntegerType funnyIntegerType = (FirstClassType.FunnyIntegerType)vt;
			FunnyIntegerType funnyIntegerType2 = funnyIntegerType.item;
			return StackItemType.S;
		}
		}
	}

	public static StackItemType get_var_stacktype(Variable v)
	{
		GeneralType typ@ = v.typ@;
		switch (typ@.Tag)
		{
		case 3:
		{
			GeneralType.FirstClassType firstClassType = (GeneralType.FirstClassType)typ@;
			FirstClassType vt = firstClassType.item;
			return firstclass_type_to_stacktype(vt);
		}
		case 1:
		{
			GeneralType.SystemType systemType = (GeneralType.SystemType)typ@;
			Type type = systemType.item;
			if (string.Equals(type.FullName, "System.IntPtr"))
			{
				return StackItemType.I;
			}
			if (string.Equals(type.FullName, "System.Int32"))
			{
				return StackItemType.I4;
			}
			if (string.Equals(type.FullName, "System.Boolean"))
			{
				return StackItemType.I4;
			}
			if (string.Equals(type.FullName, "System.Int64"))
			{
				return StackItemType.I8;
			}
			if (string.Equals(type.FullName, "System.UInt32"))
			{
				return StackItemType.I4;
			}
			return StackItemType.O;
		}
		case 2:
		{
			GeneralType.TypeReference typeReference = (GeneralType.TypeReference)typ@;
			TypeReference typeReference2 = typeReference.item;
			if (string.Equals(typeReference2.FullName, "System.IntPtr"))
			{
				return StackItemType.I;
			}
			if (string.Equals(typeReference2.FullName, "System.Int32"))
			{
				return StackItemType.I4;
			}
			if (string.Equals(typeReference2.FullName, "System.Boolean"))
			{
				return StackItemType.I4;
			}
			if (string.Equals(typeReference2.FullName, "System.Int64"))
			{
				return StackItemType.I8;
			}
			if (string.Equals(typeReference2.FullName, "System.UInt32"))
			{
				return StackItemType.I4;
			}
			return StackItemType.O;
		}
		default:
		{
			GeneralType generalType = typ@;
			FSharpFunc<GeneralType, StackItemType> fSharpFunc = ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<FSharpFunc<GeneralType, StackItemType>, Unit, string, StackItemType, GeneralType>("get_var_stacktype with not first class type: %A"));
			GeneralType func = generalType;
			return fSharpFunc.Invoke(func);
		}
		}
	}

	public static int get_sizeof_var(Variable v)
	{
		GeneralType typ@ = v.typ@;
		switch (typ@.Tag)
		{
		case 3:
		{
			GeneralType.FirstClassType firstClassType = (GeneralType.FirstClassType)typ@;
			FirstClassType t = firstClassType.item;
			return get_sizeof(t);
		}
		case 0:
		{
			GeneralType.VarArgType varArgType = (GeneralType.VarArgType)typ@;
			int num = varArgType.item;
			return (num + 1) * 8;
		}
		default:
			return ExtraTopLevelOperators.PrintFormatToStringThenFail(new PrintfFormat<int, Unit, string, int, Unit>("get_sizeof_var with not first class type"));
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static TypeReference find_struct(GenTypes typs, StructType typ)
	{
		return struct_type_to_cecil_type(typs, typ);
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static void check_zero_size_field(StructType typ, int i)
	{
		FirstClassType typ@ = typ.items@[i].typ@;
		if (typ@.Tag == 3)
		{
			FirstClassType.ArrayType arrayType = (FirstClassType.ArrayType)typ@;
			switch (arrayType.item.count@)
			{
			case 0u:
			{
				FSharpFunc<string, Unit> fSharpFunc = ExtraTopLevelOperators.PrintFormatLine(new PrintfFormat<FSharpFunc<string, Unit>, TextWriter, Unit, Unit, string>("WARNING: reference to zero size field: %s"));
				string func = typ.items@[i].ToString();
				fSharpFunc.Invoke(func);
				break;
			}
			}
		}
	}

	[CompilationArgumentCounts(new int[] { 1, 1 })]
	public static FieldNumber get_field_num(StructType typ, int i)
	{
		return new FieldNumber(typ, i);
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1 })]
	public static FieldDefinition find_struct_field(GenTypes typs, StructType typ, int i)
	{
		TypeReference typeReference = struct_type_to_cecil_type(typs, typ);
		TypeDefinition typeDefinition = (TypeDefinition)(object)typeReference;
		return typeDefinition.Fields[i];
	}

	[CompilationArgumentCounts(new int[] { 1, 1, 1, 1, 1, 1 })]
	public static Instruction make_cil_instruction(ILProcessor il, Dictionary<Variable, VariableDefinition> d_variables, Dictionary<Label, Instruction> d_labels, Dictionary<TryCatch, CecilTryCatch> d_trycatch, GenTypes typs, MyInstruction instr)
	{
		switch (instr.Tag)
		{
		case 43:
		{
			MyInstruction.Ldfld ldfld = (MyInstruction.Ldfld)instr;
			if (!(ldfld.item is FieldSpec.FieldReference))
			{
				FieldSpec.FieldNumber fieldNumber3 = (FieldSpec.FieldNumber)ldfld.item;
				StructType typ@3 = fieldNumber3.item.typ@;
				int i@3 = fieldNumber3.item.i@;
				return il.Create(OpCodes.Ldfld, find_struct_field(typs, typ@3, i@3));
			}
			FieldReference field6 = ((FieldSpec.FieldReference)ldfld.item).item;
			return il.Create(OpCodes.Ldfld, field6);
		}
		case 44:
		{
			MyInstruction.Ldflda ldflda = (MyInstruction.Ldflda)instr;
			if (!(ldflda.item is FieldSpec.FieldReference))
			{
				FieldSpec.FieldNumber fieldNumber2 = (FieldSpec.FieldNumber)ldflda.item;
				StructType typ@2 = fieldNumber2.item.typ@;
				int i@2 = fieldNumber2.item.i@;
				return il.Create(OpCodes.Ldflda, find_struct_field(typs, typ@2, i@2));
			}
			FieldReference field5 = ((FieldSpec.FieldReference)ldflda.item).item;
			return il.Create(OpCodes.Ldflda, field5);
		}
		case 97:
		{
			MyInstruction.Stfld stfld = (MyInstruction.Stfld)instr;
			if (!(stfld.item is FieldSpec.FieldReference))
			{
				FieldSpec.FieldNumber fieldNumber = (FieldSpec.FieldNumber)stfld.item;
				StructType typ@ = fieldNumber.item.typ@;
				int i@ = fieldNumber.item.i@;
				return il.Create(OpCodes.Stfld, find_struct_field(typs, typ@, i@));
			}
			FieldReference field4 = ((FieldSpec.FieldReference)stfld.item).item;
			return il.Create(OpCodes.Stfld, field4);
		}
		default:
			return il.Create(OpCodes.Add);
		case 1:
			return il.Create(OpCodes.And);
		case 2:
		{
			Label key11 = ((MyInstruction.Br)instr).item;
			return il.Create(OpCodes.Br, d_labels[key11]);
		}
		case 3:
		{
			Label key10 = ((MyInstruction.Brfalse)instr).item;
			return il.Create(OpCodes.Brfalse, d_labels[key10]);
		}
		case 4:
		{
			Label key9 = ((MyInstruction.Brtrue)instr).item;
			return il.Create(OpCodes.Brtrue, d_labels[key9]);
		}
		case 5:
		{
			FirstClassType vt = ((MyInstruction.BoxFC)instr).item;
			return il.Create(OpCodes.Box, firstclass_type_to_cecil_type(typs, vt));
		}
		case 6:
		{
			Type type6 = ((MyInstruction.Box)instr).item;
			return il.Create(OpCodes.Box, typs.md@.ImportReference(type6));
		}
		case 7:
		{
			Type type5 = ((MyInstruction.Unbox)instr).item;
			return il.Create(OpCodes.Unbox, typs.md@.ImportReference(type5));
		}
		case 8:
		{
			Type type4 = ((MyInstruction.Unbox_Any)instr).item;
			return il.Create(OpCodes.Unbox_Any, typs.md@.ImportReference(type4));
		}
		case 9:
		{
			MethodReference method5 = ((MyInstruction.Call)instr).item;
			return il.Create(OpCodes.Call, method5);
		}
		case 10:
		{
			Mono.Cecil.CallSite site = ((MyInstruction.Calli)instr).item;
			return il.Create(OpCodes.Calli, site);
		}
		case 11:
		{
			MethodReference method4 = ((MyInstruction.Callvirt)instr).item;
			return il.Create(OpCodes.Callvirt, method4);
		}
		case 12:
		{
			Type type3 = ((MyInstruction.Castclass)instr).item;
			return il.Create(OpCodes.Castclass, typs.md@.ImportReference(type3));
		}
		case 13:
			return il.Create(OpCodes.Ceq);
		case 14:
			return il.Create(OpCodes.Cgt);
		case 15:
			return il.Create(OpCodes.Cgt_Un);
		case 16:
			return il.Create(OpCodes.Clt);
		case 17:
			return il.Create(OpCodes.Clt_Un);
		case 18:
			return il.Create(OpCodes.Conv_I);
		case 19:
			return il.Create(OpCodes.Conv_I1);
		case 20:
			return il.Create(OpCodes.Conv_I2);
		case 21:
			return il.Create(OpCodes.Conv_I4);
		case 22:
			return il.Create(OpCodes.Conv_I8);
		case 27:
			return il.Create(OpCodes.Conv_R4);
		case 28:
			return il.Create(OpCodes.Conv_R8);
		case 29:
			return il.Create(OpCodes.Conv_R_Un);
		case 23:
			return il.Create(OpCodes.Conv_U1);
		case 24:
			return il.Create(OpCodes.Conv_U2);
		case 25:
			return il.Create(OpCodes.Conv_U4);
		case 26:
			return il.Create(OpCodes.Conv_U8);
		case 30:
			return il.Create(OpCodes.Cpblk);
		case 31:
			return il.Create(OpCodes.Div);
		case 32:
			return il.Create(OpCodes.Div_Un);
		case 33:
			return il.Create(OpCodes.Dup);
		case 34:
			return il.Create(OpCodes.Initblk);
		case 35:
		{
			Label key8 = ((MyInstruction.Label)instr).item;
			return d_labels[key8];
		}
		case 36:
			return il.Create(OpCodes.Ldarg_0);
		case 37:
		{
			ParameterDefinition parameter2 = ((MyInstruction.Ldarg)instr).item;
			return il.Create(OpCodes.Ldarg, parameter2);
		}
		case 38:
		{
			ParameterDefinition parameter = ((MyInstruction.Ldarga)instr).item;
			return il.Create(OpCodes.Ldarga, parameter);
		}
		case 39:
		{
			int value5 = ((MyInstruction.Ldc_I4)instr).item;
			return il.Create(OpCodes.Ldc_I4, value5);
		}
		case 40:
		{
			long value4 = ((MyInstruction.Ldc_I8)instr).item;
			return il.Create(OpCodes.Ldc_I8, value4);
		}
		case 41:
		{
			float value3 = ((MyInstruction.Ldc_R4)instr).item;
			return il.Create(OpCodes.Ldc_R4, value3);
		}
		case 42:
		{
			double value2 = ((MyInstruction.Ldc_R8)instr).item;
			return il.Create(OpCodes.Ldc_R8, value2);
		}
		case 45:
		{
			MethodReference method3 = ((MyInstruction.Ldftn)instr).item;
			return il.Create(OpCodes.Ldftn, method3);
		}
		case 46:
			return il.Create(OpCodes.Ldind_I);
		case 47:
			return il.Create(OpCodes.Ldind_I1);
		case 48:
			return il.Create(OpCodes.Ldind_I2);
		case 49:
			return il.Create(OpCodes.Ldind_I4);
		case 50:
			return il.Create(OpCodes.Ldind_I8);
		case 51:
			return il.Create(OpCodes.Ldind_R4);
		case 52:
			return il.Create(OpCodes.Ldind_R8);
		case 53:
			return il.Create(OpCodes.Ldind_U1);
		case 54:
			return il.Create(OpCodes.Ldind_U2);
		case 55:
			return il.Create(OpCodes.Ldind_U4);
		case 56:
		{
			Variable key7 = ((MyInstruction.Ldloc)instr).item;
			return il.Create(OpCodes.Ldloc, d_variables[key7]);
		}
		case 57:
		{
			Variable key6 = ((MyInstruction.Ldloca)instr).item;
			return il.Create(OpCodes.Ldloca, d_variables[key6]);
		}
		case 59:
			return il.Create(OpCodes.Ldelem_Ref);
		case 60:
			return il.Create(OpCodes.Ldelem_I1);
		case 61:
			return il.Create(OpCodes.Ldelem_I2);
		case 62:
			return il.Create(OpCodes.Ldelem_I4);
		case 63:
			return il.Create(OpCodes.Ldelem_I8);
		case 64:
			return il.Create(OpCodes.Ldelem_U1);
		case 65:
			return il.Create(OpCodes.Ldelem_U2);
		case 66:
			return il.Create(OpCodes.Ldelem_U4);
		case 68:
			return il.Create(OpCodes.Ldelem_R4);
		case 69:
			return il.Create(OpCodes.Ldelem_R8);
		case 67:
			return il.Create(OpCodes.Ldelem_I);
		case 58:
			return il.Create(OpCodes.Ldnull);
		case 70:
		{
			FieldReference field3 = ((MyInstruction.Ldsfld)instr).item;
			return il.Create(OpCodes.Ldsfld, field3);
		}
		case 71:
		{
			FieldReference field2 = ((MyInstruction.Ldsflda)instr).item;
			return il.Create(OpCodes.Ldsflda, field2);
		}
		case 72:
		{
			string value = ((MyInstruction.Ldstr)instr).item;
			return il.Create(OpCodes.Ldstr, value);
		}
		case 73:
		{
			TypeReference type2 = ((MyInstruction.Ldtoken)instr).item;
			return il.Create(OpCodes.Ldtoken, type2);
		}
		case 74:
		{
			MethodReference method2 = ((MyInstruction.LdtokenM)instr).item;
			return il.Create(OpCodes.Ldtoken, method2);
		}
		case 75:
			return il.Create(OpCodes.Localloc);
		case 76:
			return il.Create(OpCodes.Mul);
		case 77:
			return il.Create(OpCodes.Neg);
		case 78:
		{
			Type type = ((MyInstruction.Newarr)instr).item;
			return il.Create(OpCodes.Newarr, typs.md@.ImportReference(type));
		}
		case 79:
		{
			MethodReference method = ((MyInstruction.Newobj)instr).item;
			return il.Create(OpCodes.Newobj, method);
		}
		case 80:
			return il.Create(OpCodes.Nop);
		case 81:
			return il.Create(OpCodes.Or);
		case 82:
			return il.Create(OpCodes.Pop);
		case 83:
			return il.Create(OpCodes.Rem);
		case 84:
			return il.Create(OpCodes.Rem_Un);
		case 85:
			return il.Create(OpCodes.Shl);
		case 86:
			return il.Create(OpCodes.Shr);
		case 87:
			return il.Create(OpCodes.Shr_Un);
		case 88:
		{
			StructType typ = ((MyInstruction.Sizeof)instr).item;
			return il.Create(OpCodes.Sizeof, find_struct(typs, typ));
		}
		case 89:
			return il.Create(OpCodes.Stelem_Ref);
		case 90:
			return il.Create(OpCodes.Stelem_I);
		case 91:
			return il.Create(OpCodes.Stelem_I1);
		case 92:
			return il.Create(OpCodes.Stelem_I2);
		case 93:
			return il.Create(OpCodes.Stelem_I4);
		case 94:
			return il.Create(OpCodes.Stelem_I8);
		case 95:
			return il.Create(OpCodes.Stelem_R4);
		case 96:
			return il.Create(OpCodes.Stelem_R8);
		case 98:
			return il.Create(OpCodes.Stind_I);
		case 99:
			return il.Create(OpCodes.Stind_I1);
		case 100:
			return il.Create(OpCodes.Stind_I2);
		case 101:
			return il.Create(OpCodes.Stind_I4);
		case 102:
			return il.Create(OpCodes.Stind_I8);
		case 103:
			return il.Create(OpCodes.Stind_R4);
		case 104:
			return il.Create(OpCodes.Stind_R8);
		case 105:
		{
			Variable key5 = ((MyInstruction.Stloc)instr).item;
			return il.Create(OpCodes.Stloc, d_variables[key5]);
		}
		case 106:
		{
			FieldReference field = ((MyInstruction.Stsfld)instr).item;
			return il.Create(OpCodes.Stsfld, field);
		}
		case 107:
			return il.Create(OpCodes.Sub);
		case 108:
			return il.Create(OpCodes.Ret);
		case 109:
			return il.Create(OpCodes.Throw);
		case 110:
			return il.Create(OpCodes.Xor);
		case 111:
			return il.Create(OpCodes.Add_Ovf_Un);
		case 112:
			return il.Create(OpCodes.Sub_Ovf_Un);
		case 113:
			return il.Create(OpCodes.Mul_Ovf_Un);
		case 114:
			return il.Create(OpCodes.Add_Ovf);
		case 115:
			return il.Create(OpCodes.Sub_Ovf);
		case 116:
			return il.Create(OpCodes.Mul_Ovf);
		case 117:
		{
			Label key4 = ((MyInstruction.Leave)instr).item;
			return il.Create(OpCodes.Leave, d_labels[key4]);
		}
		case 118:
		{
			TryCatch key3 = ((MyInstruction.BeginTry)instr).item;
			return d_trycatch[key3].begin_try@;
		}
		case 119:
		{
			TryCatch key2 = ((MyInstruction.BeginCatch)instr).item;
			return d_trycatch[key2].begin_catch@;
		}
		case 120:
		{
			TryCatch key = ((MyInstruction.EndCatch)instr).item;
			return d_trycatch[key].end_catch@;
		}
		}
	}
}
