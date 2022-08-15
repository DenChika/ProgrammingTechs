```cs
// Decompiled with JetBrains decompiler
// Type: Program
// Assembly: FSharpTrial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D85670CC-88CF-0636-AC1A-2F1C7D3E921B
// Assembly location: C:\Users\volok\RiderProjects\Solution1\CSharpTesting\bin\Debug\net6.0\FSharpTrial.dll

using Microsoft.FSharp.Core;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[CompilationMapping(SourceConstructFlags.Module)]
public static class Program
{
  [CompilationArgumentCounts(new int[] {1, 1})]
  public static int multi(int x, int y) => x * y;

  [CompilationArgumentCounts(new int[] {1, 1})]
  public static int plus(int x, int y) => x + y;

  [CompilationArgumentCounts(new int[] {1, 1, 1})]
  public static int result(int x, int y, int z)
  {
    int y1 = Program.multi(x, y);
    return Program.plus(z, y1);
  }

  public static void unionTrial(int x)
  {
    Program.IntOrBool intOrBool1 = Program.IntOrBool.NewI(x);
    Program.IntOrBool intOrBool2 = Program.IntOrBool.NewB(x % 2 == 0);
    ExtraTopLevelOperators.PrintFormatLine<Unit>((PrintfFormat<Unit, TextWriter, Unit, Unit>) new PrintfFormat<Unit, TextWriter, Unit, Unit, int>("%d%P()", new object[1]
    {
      (object) x
    }, (Type[]) null));
    ExtraTopLevelOperators.PrintFormatLine<Unit>((PrintfFormat<Unit, TextWriter, Unit, Unit>) new PrintfFormat<Unit, TextWriter, Unit, Unit, Program.IntOrBool>("%P() is odd?", new object[1]
    {
      (object) intOrBool1
    }, (Type[]) null));
    ExtraTopLevelOperators.PrintFormatLine<Unit>((PrintfFormat<Unit, TextWriter, Unit, Unit>) new PrintfFormat<Unit, TextWriter, Unit, Unit, Program.IntOrBool>("%P()!", new object[1]
    {
      (object) intOrBool2
    }, (Type[]) null));
  }

  [DebuggerDisplay("{__DebugDisplay(),nq}")]
  [CompilationMapping(SourceConstructFlags.SumType)]
  [Serializable]
  [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
  public abstract class IntOrBool : 
    IEquatable<Program.IntOrBool>,
    IStructuralEquatable,
    IComparable<Program.IntOrBool>,
    IComparable,
    IStructuralComparable
  {
    [CompilerGenerated]
    [DebuggerNonUserCode]
    internal IntOrBool()
    {
    }

    [CompilationMapping(SourceConstructFlags.UnionCase, 0)]
    public static Program.IntOrBool NewI(int item) => (Program.IntOrBool) new Program.IntOrBool.I(item);

    [CompilerGenerated]
    [DebuggerNonUserCode]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsI
    {
      [DebuggerNonUserCode] get => this is Program.IntOrBool.I;
    }

    [DebuggerNonUserCode]
    public bool get_IsI() => this is Program.IntOrBool.I;

    [CompilationMapping(SourceConstructFlags.UnionCase, 1)]
    public static Program.IntOrBool NewB(bool item) => (Program.IntOrBool) new Program.IntOrBool.B(item);

    [CompilerGenerated]
    [DebuggerNonUserCode]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsB
    {
      [DebuggerNonUserCode] get => this is Program.IntOrBool.B;
    }

    [DebuggerNonUserCode]
    public bool get_IsB() => this is Program.IntOrBool.B;

    [CompilerGenerated]
    [DebuggerNonUserCode]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Tag
    {
      [DebuggerNonUserCode] get => this is Program.IntOrBool.B ? 1 : 0;
    }

    [DebuggerNonUserCode]
    public int get_Tag() => this is Program.IntOrBool.B ? 1 : 0;

    [CompilerGenerated]
    [DebuggerNonUserCode]
    [SpecialName]
    internal object __DebugDisplay() => (object) ExtraTopLevelOperators.PrintFormatToString<FSharpFunc<Program.IntOrBool, string>>((PrintfFormat<FSharpFunc<Program.IntOrBool, string>, Unit, string, string>) new PrintfFormat<FSharpFunc<Program.IntOrBool, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);

    [CompilerGenerated]
    public override string ToString() => ExtraTopLevelOperators.PrintFormatToString<FSharpFunc<Program.IntOrBool, string>>((PrintfFormat<FSharpFunc<Program.IntOrBool, string>, Unit, string, string>) new PrintfFormat<FSharpFunc<Program.IntOrBool, string>, Unit, string, string, Program.IntOrBool>("%+A")).Invoke(this);

    [CompilerGenerated]
    public virtual int CompareTo(Program.IntOrBool obj)
    {
      if (this != null)
      {
        if (obj == null)
          return 1;
        int num1 = !(this is Program.IntOrBool.B) ? 0 : 1;
        int num2 = !(obj is Program.IntOrBool.B) ? 0 : 1;
        if (num1 != num2)
          return num1 - num2;
        if (this is Program.IntOrBool.I)
        {
          Program.IntOrBool.I i1 = (Program.IntOrBool.I) this;
          Program.IntOrBool.I i2 = (Program.IntOrBool.I) obj;
          IComparer genericComparer = LanguagePrimitives.GenericComparer;
          int num3 = i1.item;
          int num4 = i2.item;
          return num3 < num4 ? -1 : (num3 > num4 ? 1 : 0);
        }
        Program.IntOrBool.B b1 = (Program.IntOrBool.B) this;
        Program.IntOrBool.B b2 = (Program.IntOrBool.B) obj;
        IComparer genericComparer1 = LanguagePrimitives.GenericComparer;
        bool flag1 = b1.item;
        bool flag2 = b2.item;
        return (flag1 ? 1 : 0) < (flag2 ? 1 : 0) ? -1 : ((flag1 ? 1 : 0) > (flag2 ? 1 : 0) ? 1 : 0);
      }
      return obj != null ? -1 : 0;
    }

    [CompilerGenerated]
    public virtual int CompareTo(object obj) => this.CompareTo((Program.IntOrBool) obj);

    [CompilerGenerated]
    public virtual int CompareTo(object obj, IComparer comp)
    {
      Program.IntOrBool intOrBool = (Program.IntOrBool) obj;
      if (this != null)
      {
        if ((Program.IntOrBool) obj == null)
          return 1;
        int num1 = !(this is Program.IntOrBool.B) ? 0 : 1;
        int num2 = !(intOrBool is Program.IntOrBool.B) ? 0 : 1;
        if (num1 != num2)
          return num1 - num2;
        if (this is Program.IntOrBool.I)
        {
          Program.IntOrBool.I i1 = (Program.IntOrBool.I) this;
          Program.IntOrBool.I i2 = (Program.IntOrBool.I) intOrBool;
          int num3 = i1.item;
          int num4 = i2.item;
          return num3 < num4 ? -1 : (num3 > num4 ? 1 : 0);
        }
        Program.IntOrBool.B b1 = (Program.IntOrBool.B) this;
        Program.IntOrBool.B b2 = (Program.IntOrBool.B) intOrBool;
        bool flag1 = b1.item;
        bool flag2 = b2.item;
        return (flag1 ? 1 : 0) < (flag2 ? 1 : 0) ? -1 : ((flag1 ? 1 : 0) > (flag2 ? 1 : 0) ? 1 : 0);
      }
      return (Program.IntOrBool) obj != null ? -1 : 0;
    }

    [CompilerGenerated]
    public virtual int GetHashCode(IEqualityComparer comp)
    {
      if (this == null)
        return 0;
      if (this is Program.IntOrBool.I)
      {
        Program.IntOrBool.I i = (Program.IntOrBool.I) this;
        int num = 0;
        return i.item + ((num << 6) + (num >> 2)) - 1640531527;
      }
      Program.IntOrBool.B b = (Program.IntOrBool.B) this;
      int num1 = 1;
      return (b.item ? 1 : 0) + ((num1 << 6) + (num1 >> 2)) - 1640531527;
    }

    [CompilerGenerated]
    public override sealed int GetHashCode() => this.GetHashCode(LanguagePrimitives.GenericEqualityComparer);

    [CompilerGenerated]
    public virtual bool Equals(object obj, IEqualityComparer comp)
    {
      if (this == null)
        return obj == null;
      if (!(obj is Program.IntOrBool intOrBool) || (!(this is Program.IntOrBool.B) ? 0 : 1) != (!(intOrBool is Program.IntOrBool.B) ? 0 : 1))
        return false;
      return this is Program.IntOrBool.I ? ((Program.IntOrBool.I) this).item == ((Program.IntOrBool.I) intOrBool).item : ((Program.IntOrBool.B) this).item == ((Program.IntOrBool.B) intOrBool).item;
    }

    [CompilerGenerated]
    public virtual bool Equals(Program.IntOrBool obj)
    {
      if (this == null)
        return obj == null;
      if (obj == null || (!(this is Program.IntOrBool.B) ? 0 : 1) != (!(obj is Program.IntOrBool.B) ? 0 : 1))
        return false;
      return this is Program.IntOrBool.I ? ((Program.IntOrBool.I) this).item == ((Program.IntOrBool.I) obj).item : ((Program.IntOrBool.B) this).item == ((Program.IntOrBool.B) obj).item;
    }

    [CompilerGenerated]
    public override sealed bool Equals(object obj) => obj is Program.IntOrBool intOrBool && this.Equals(intOrBool);

    public static class Tags
    {
      public const int I = 0;
      public const int B = 1;
    }

    [DebuggerTypeProxy(typeof (Program.IntOrBool.I\u0040DebugTypeProxy))]
    [DebuggerDisplay("{__DebugDisplay(),nq}")]
    [Serializable]
    [SpecialName]
    public class I : Program.IntOrBool
    {
      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      [CompilerGenerated]
      [DebuggerNonUserCode]
      internal readonly int item;

      [CompilerGenerated]
      [DebuggerNonUserCode]
      internal I(int item) => this.item = item;

      [CompilationMapping(SourceConstructFlags.Field, 0, 0)]
      [CompilerGenerated]
      [DebuggerNonUserCode]
      public int Item
      {
        [DebuggerNonUserCode] get => this.item;
      }

      [DebuggerNonUserCode]
      public int get_Item() => this.item;
    }

    [DebuggerTypeProxy(typeof (Program.IntOrBool.B\u0040DebugTypeProxy))]
    [DebuggerDisplay("{__DebugDisplay(),nq}")]
    [Serializable]
    [SpecialName]
    public class B : Program.IntOrBool
    {
      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      [CompilerGenerated]
      [DebuggerNonUserCode]
      internal readonly bool item;

      [CompilerGenerated]
      [DebuggerNonUserCode]
      internal B(bool item) => this.item = item;

      [CompilationMapping(SourceConstructFlags.Field, 1, 0)]
      [CompilerGenerated]
      [DebuggerNonUserCode]
      public bool Item
      {
        [DebuggerNonUserCode] get => this.item;
      }

      [DebuggerNonUserCode]
      public bool get_Item() => this.item;
    }

    [SpecialName]
    internal class I\u0040DebugTypeProxy
    {
      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      [CompilerGenerated]
      [DebuggerNonUserCode]
      internal Program.IntOrBool.I _obj;

      [CompilerGenerated]
      [DebuggerNonUserCode]
      public I\u0040DebugTypeProxy(Program.IntOrBool.I obj) => this._obj = obj;

      [CompilationMapping(SourceConstructFlags.Field, 0, 0)]
      [CompilerGenerated]
      [DebuggerNonUserCode]
      public int Item
      {
        [DebuggerNonUserCode] get => this._obj.item;
      }

      [DebuggerNonUserCode]
      public int get_Item() => this._obj.item;
    }

    [SpecialName]
    internal class B\u0040DebugTypeProxy
    {
      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      [CompilerGenerated]
      [DebuggerNonUserCode]
      internal Program.IntOrBool.B _obj;

      [CompilerGenerated]
      [DebuggerNonUserCode]
      public B\u0040DebugTypeProxy(Program.IntOrBool.B obj) => this._obj = obj;

      [CompilationMapping(SourceConstructFlags.Field, 1, 0)]
      [CompilerGenerated]
      [DebuggerNonUserCode]
      public bool Item
      {
        [DebuggerNonUserCode] get => this._obj.item;
      }

      [DebuggerNonUserCode]
      public bool get_Item() => this._obj.item;
    }
  }
}
```
