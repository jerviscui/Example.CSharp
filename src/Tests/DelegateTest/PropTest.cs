using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DelegateTest
{
    public class PropTest
    {
        class A
        {
            public string S { get; set; } = null!;

            public string Sp { get; protected set; } = null!;
        }

        private PropertyInfo GetProp() => typeof(A).GetProperty("S")!;

        private PropertyInfo GetProtectedProp() => typeof(A).GetProperty("Sp")!;

        private PropertyInfo GetPropCache() => c ??= typeof(A).GetProperty("S")!;

        private PropertyInfo? c;

        private Action<A, string> GetPropDelegate() => d ??= typeof(A).GetProperty("S")!.SetMethod!.CreateDelegate<Action<A, string>>();

        private Action<A, string>? d;

        private A a = new();

        string PropSetTest()
        {
            var propS = GetProp();

            propS.SetValue(a, "s");

            return a.S;
        }

        string PropProtectedSetTest()
        {
            var propSp = GetProtectedProp();

            propSp.SetValue(a, "sp");

            return a.Sp;
        }

        string PropCacheSetTest()
        {
            var propS = GetPropCache();

            propS.SetValue(a, "s");

            return a.S;
        }

        string PropDelegateSetTest()
        {
            var method = GetPropDelegate();

            method(a, "s");

            return a.S;
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
    }
}
