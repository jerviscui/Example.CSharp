using System.Runtime.CompilerServices;

namespace CollectionTest;

public static class ConditionalWeakTableTests
{
    public static void Test()
    {
        var mc1 = new ManagedClass { I = 1 };
        var mc2 = new ManagedClass { I = 2 };
        var mc3 = new ManagedClass { I = 3 };
        var mc4 = new ManagedClass { I = 4 };

        var cwt = new ConditionalWeakTable<ManagedClass, ClassData>
        {
            { mc1, new ClassData() }, { mc2, new ClassData() }, { mc3, new ClassData() }, { mc4, new ClassData() }
        };

        var wr2 = new WeakReference(mc2);
        mc2 = null;

        foreach (var ele in cwt)
        {
            Console.WriteLine("{0} Data created at {1}", ele.Key.I, ele.Value.CreationTime);
        }

        GC.Collect();

        Thread.Sleep(100);

        var count = 10;
        while (count-- > 0) //release 下运行
        {
            if (wr2.Target == null)
            {
                Console.WriteLine("No strong reference to mc2 exists.");
            }
            else if (cwt.TryGetValue((ManagedClass)wr2.Target, out var data))
            {
                Console.WriteLine("mc2 Data created at {0}", data.CreationTime);
            }
            else
            {
                Console.WriteLine("mc2 not found in the table.");
            }

            if (cwt.TryGetValue(mc1, out var data2))
            {
                Console.WriteLine("mc1 Data created at {0}", data2.CreationTime);
            }

            foreach (var ele in cwt)
            {
                Console.WriteLine("{0} Data created at {1}", ele.Key.I, ele.Value.CreationTime);
            }

            Console.WriteLine("once");
            GC.Collect();
            Thread.Sleep(100);

            //Release 输出：为什么只有 mc1 mc4 不被释放，
            //1 Data created at 2022-09-13 17:38:19
            //2 Data created at 2022-09-13 17:38:19
            //3 Data created at 2022-09-13 17:38:19
            //4 Data created at 2022-09-13 17:38:19
            //No strong reference to mc2 exists.
            //mc1 Data created at 2022-09-13 17:38:19
            //1 Data created at 2022-09-13 17:38:19
            //4 Data created at 2022-09-13 17:38:19
            //once
        }
    }

    private class ManagedClass
    {
        public int I { get; set; }
    }

    private class ClassData
    {
        public DateTime CreationTime;

        public object Data;

        public ClassData()
        {
            CreationTime = DateTime.Now;
            Data = new object();
        }
    }
}


