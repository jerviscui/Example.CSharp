using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;

namespace CtorTest
{
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    [MemoryDiagnoser]
    public class NullableReferenceTest
    {
        [Benchmark]
        public void NoInit_Test()
        {
            var a = new A("p1");
        }

        [Benchmark]
        public void InitByNull_Test()
        {
            var a = new B("p1");
        }

        private class A
        {
            public string P1 { get; set; }

            public string P2 { get; set; }

            public string P3 { get; set; }

            public string P4 { get; set; }

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
            public A(string p1)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
            {
                P1 = p1;
            }
        }

        private class B
        {
            //赋值为 null 不会有性能影响，string.Empty 会额外生成初始化器赋值 IL
            public string P1 { get; set; } = null!;

            public string P2 { get; set; } = null!;

            public string P3 { get; set; } = null!;

            public string P4 { get; set; } = null!;

            public B(string p1)
            {
                P1 = p1;
            }
        }
    }
}
