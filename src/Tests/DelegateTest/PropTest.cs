using System;
using System.Diagnostics;
using System.Reflection;
using Common;

namespace DelegateTest
{
    public class PropTest
    {
        private readonly A _a = new();

        private PropertyInfo? _c;

        private Action<A, string>? _d;

        private PropertyInfo GetProp() => typeof(A).GetProperty("S")!;

        private PropertyInfo GetProtectedProp() => typeof(A).GetProperty("Sp")!;

        private PropertyInfo GetPropCache() => _c ??= typeof(A).GetProperty("S")!;

        private Action<A, string> GetPropDelegate() =>
            _d ??= typeof(A).GetProperty("S")!.SetMethod!.CreateDelegate<Action<A, string>>();

        private string PropSetTest()
        {
            var propS = GetProp();

            propS.SetValue(_a, "s");

            return _a.S;
        }

        private string PropProtectedSetTest()
        {
            var propSp = GetProtectedProp();

            propSp.SetValue(_a, "sp");

            return _a.Sp;
        }

        private string PropCacheSetTest()
        {
            var propS = GetPropCache();

            propS.SetValue(_a, "s");

            return _a.S;
        }

        private string PropDelegateSetTest()
        {
            var method = GetPropDelegate();

            method(_a, "s");

            return _a.S;
        }

        public void PropAndDelegate_Exec_Test()
        {
            var watch = new Stopwatch();
            var p = new PropTest();

            Console.WriteLine(p.PropProtectedSetTest());

            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                p.PropSetTest();
            }
            watch.Stop();
            Print.Microsecond(watch);

            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                p.PropCacheSetTest();
            }
            watch.Stop();
            Print.Microsecond(watch);

            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                p.PropDelegateSetTest();
            }
            watch.Stop();
            Print.Microsecond(watch);
            //242,679 us
            //160,200 us
            // 15,583 us
        }

        private class A
        {
            public string S { get; } = null!;

            public string Sp { get; } = null!;
        }
    }
}
