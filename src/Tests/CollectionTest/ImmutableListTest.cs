using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CollectionTest;

internal class ImmutableListTest
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
        ImmutableInterlocked.Update(ref list, l => l.Add(1));
        ImmutableInterlocked.Update(ref list, l => l.Add(2));

        Console.WriteLine(string.Join(", ", list)); // 1, 2
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
