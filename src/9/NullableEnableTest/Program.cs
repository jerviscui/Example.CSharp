using System;

namespace NullableEnableTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //在全局开启了 <Nullable>enable</Nullable>

            A a = A.GetOrDefault(false);//CS8600	将 null 文本或可能的 null 值转换为不可为 null 类型。
            A? a1 = A.GetOrDefault(false);

            A a2 = A.GetOrDefault(true);
            A a3 = A.GetOrDefault(true)!;//明确不会返回 null 时，可以在结尾加 ！ 阻止 CS8600 警告

            //Console.WriteLine("Hello World!");
        }

        public class A
        {
            private A()
            {
                Name = string.Empty;
            }

            public string Name { get; set; }//CS8618	在退出构造函数时，不可为 null 的 属性“Name”必须包含非 null 值。
                                            //请考虑将 属性 声明为可以为 null。
            
            public string? NameOrNull { get; set; }

            public static A? GetOrDefault(bool create)
            {
                if (create)
                {
                    return new A();
                }

                return default;
            }
        }
    }
}
