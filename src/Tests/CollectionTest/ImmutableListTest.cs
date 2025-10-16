using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CollectionTest;

public static class ImmutableListTest
{

    #region Constants & Statics

    public static void CopyOnWriteTest()
    {
        var array = new[] { 1, 2, 3, 4 };
        var immutable = array.ToImmutableList();

        var asImmutable = ImmutableCollectionsMarshal.AsImmutableArray(array);
        var as2 = Unsafe.As<int[], ImmutableArray<int>>(ref array);
        var as3 = ImmutableArray.Create(array);

        array[0] = 100;
        Console.WriteLine(immutable[0]); // 1 发生复制
        Console.WriteLine(asImmutable[0]); // 100
        Console.WriteLine(as2[0]); // 100
        Console.WriteLine(as3[0]); // 1 发生复制

        Console.WriteLine();
        var newImmutable = asImmutable.SetItem(1, 200);
        Console.WriteLine(array[1]); // 2
        Console.WriteLine(newImmutable[1]); // 200
    }

    public static void ImmutableInterlockedTest()
    {
        var list = ImmutableList<int>.Empty;

        // 多线程安全添加
        _ = ImmutableInterlocked.Update(ref list, l => l.Add(1));
        _ = ImmutableInterlocked.Update(ref list, l => l.Add(2));

        Console.WriteLine(string.Join(", ", list)); // 1, 2
    }

    public static void RefChange_Test()
    {
        var list = new List<RefType>([new RefType(), new RefType(), new RefType(), new RefType()]);

        var readOnly = list.AsReadOnly();
        var immutable = list.ToImmutableList();

        immutable[0].StrProp = "abc";

        Console.WriteLine(list[0].StrProp);
        Console.WriteLine(readOnly[0].StrProp);
        Console.WriteLine(immutable[0].StrProp);
    }

    public static void RefCopy_Test()
    {
        var list = new List<RefType>([new RefType(), new RefType(), new RefType(), new RefType()]);

        var readOnly = list.AsReadOnly();
        var immutable = list.ToImmutableList();

        list.Add(new RefType());
        Console.WriteLine(readOnly.Count);  // 5
        Console.WriteLine(immutable.Count); // 4

        var newList = immutable.Add(new RefType());
        Console.WriteLine(immutable.Count); // 4
        Console.WriteLine(newList.Count); // 5
    }

    public static void RefRoType_Test()
    {
        var list = new List<RefRoType>([new RefRoType(), new RefRoType(), new RefRoType(), new RefRoType()]);

        var readOnly = list.AsReadOnly();
        var immutable = list.ToImmutableList();

        //immutable[0].StrProp = "abc"; //CS8852 Init-only property or indexer 'RefRoType.StrProp' can only be assigned in an object initializer, or on 'this' or 'base' in an instance constructor or an 'init' accessor.
        //immutable[0] = immutable[0] with { StrProp = "abc" }; //CS0200 Property or indexer 'ImmutableList<RefRoType>.this[int]' cannot be assigned to -- it is read only
    }

    public static void Test()
    {
        var list = new List<int>();
        var readOnly = list.AsReadOnly();
        var immutable = list.ToImmutableList();

        list.Add(1);
        Console.WriteLine(readOnly.Count);  // 1
        Console.WriteLine(immutable.Count); // 0

        var newList = immutable.Add(2);
        Console.WriteLine(immutable.Count); // 0
        Console.WriteLine(newList.Count); // 1
    }

    #endregion

}

public class RefType
{
    public RefType()
    {
        IntProp = 42;
        StrProp = "Hello, World!";
    }

    #region Properties

    public int IntProp { get; set; }

    public string StrProp { get; set; }

    #endregion

}

public record RefRoType(int IntProp, string StrProp)
{
    public RefRoType() : this(42, "RefRoType")
    {
    }
}
