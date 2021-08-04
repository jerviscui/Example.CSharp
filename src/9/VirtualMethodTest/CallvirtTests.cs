using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Common;

namespace VirtualMethodTest
{
    public class CallvirtTests
    {
        public static void Call_Or_Callvirt_Test()
        {
            var sealedClass = new SealedClass();
            SealedClass.StaticMethod(); //call      void SealedClass::StaticMethod()
            sealedClass.Method();       //callvirt  instance void SealedClass::Method()

            var baseClass = new BaseClass();
            BaseClass.StaticMethod();  //call      void BaseClass::StaticMethod()
            baseClass.Method();        //callvirt  instance void BaseClass::Method()
            baseClass.VirtualMethod(); //callvirt  instance void BaseClass::VirtualMethod()

            var deriveClass = new DeriveClass();
            deriveClass.Method();        //callvirt  instance void BaseClass::Method()
            deriveClass.VirtualMethod(); //callvirt  instance void BaseClass::VirtualMethod()//override method
            deriveClass.ToString();      //callvirt  instance string DeriveClass::ToString()//new method

            //只有 static 方式使用 call 指令调用
        }

        public static void InstanceMethod_CallvirtPerformance_Test()
        {
            int i = 1_000_000;
            var stopwatch = new Stopwatch();
            var sealedClass = new SealedClass();

            stopwatch.Start();
            for (int j = 0; j < i; j++)
            {
                //sealedClass.Method();
                SealedClass.StaticMethod();
            }
            stopwatch.Stop();
            Print.Microsecond(stopwatch);

            stopwatch.Restart();
            for (int j = 0; j < i; j++)
            {
                //SealedClass.StaticMethod();
                sealedClass.Method();
            }
            stopwatch.Stop();
            Print.Microsecond(stopwatch);

            //call 比 callvirt 基本没有性能差距
            //263 us
            //251 us
        }

        public static void VirtualMethod_CallvirtPerformance_Test()
        {
            int i = 1_000_000;
            var stopwatch = new Stopwatch();
            var deriveClass = new DeriveClass();

            stopwatch.Start();
            for (int j = 0; j < i; j++)
            {
                BaseClass.StaticMethod();
            }
            stopwatch.Stop();
            Print.Microsecond(stopwatch);

            stopwatch.Restart();
            for (int j = 0; j < i; j++)
            {
                deriveClass.VirtualMethod();
            }
            stopwatch.Stop();
            Print.Microsecond(stopwatch);

            //call 比 callvirt 基本没有性能差距
            //251 us
            //251 us
        }

        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        private sealed class SealedClass
        {
            public void Method()
            {
                //Thread.Sleep(TimeSpan.FromTicks(1));
            }

            public static void StaticMethod()
            {
                //Thread.Sleep(TimeSpan.FromTicks(1));
            }
        }

        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        private class BaseClass
        {
            public void Method()
            {
            }

            public static void StaticMethod()
            {
            }

            public virtual void VirtualMethod()
            {
            }
        }

        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        private class DeriveClass : BaseClass
        {
            public override void VirtualMethod()
            {
            }

            public new string ToString()
            {
                return string.Empty;
            }
        }
    }
}
