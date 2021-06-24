using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DelegateTest
{
    public class PropTest
    {
        class A
        {
            public string S { get; set; }

            public string Sp { get; protected set; }
        }

        private PropertyInfo GetProp() => typeof(A).GetProperty("S")!;

        private PropertyInfo GetProtectedProp() => typeof(A).GetProperty("Sp")!;

        private PropertyInfo GetPropCache() => c ??= typeof(A).GetProperty("S")!;

        private PropertyInfo? c;

        private Action<A, string> GetPropDelegate() => d ??= typeof(A).GetProperty("S")!.SetMethod!.CreateDelegate<Action<A, string>>();

        private Action<A, string>? d;

        private A a = new();

        public string PropSetTest()
        {
            var propS = GetProp();

            propS.SetValue(a, "s");

            return a.S;
        }

        public string PropProtectedSetTest()
        {
            var propSp = GetProtectedProp();

            propSp.SetValue(a, "sp");

            return a.Sp;
        }

        public string PropCacheSetTest()
        {
            var propS = GetPropCache();

            propS.SetValue(a, "s");

            return a.S;
        }

        public string PropDelegateSetTest()
        {
            var method = GetPropDelegate();

            method(a, "s");

            return a.S;
        }
    }
}
