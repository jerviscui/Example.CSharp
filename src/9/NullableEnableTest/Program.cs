namespace NullableEnableTest
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            //在全局开启了 <Nullable>enable</Nullable>

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            A a = A.GetOrDefault(false); //CS8600	将 null 文本或可能的 null 值转换为不可为 null 类型。
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            A? a1 = A.GetOrDefault(false);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            A a2 = A.GetOrDefault(true);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            A a3 = A.GetOrDefault(true)!; //明确不会返回 null 时，可以在结尾加 ！ 阻止 CS8600 警告

            //Console.WriteLine("Hello World!");
        }

        public class A
        {
            //CS8618	在退出构造函数时，不可为 null 的 属性“Name”必须包含非 null 值。
            //请考虑将 属性 声明为可以为 null。
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            private A()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            {
                //Name = string.Empty;
            }

            public string Name { get; set; }

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

        public class B
        {
            //for ef proxy
#pragma warning disable 8618
            protected B()
#pragma warning restore 8618
            {
            }

            public B(string s1, string s2, string s3)
            {
                S1 = s1;
                S2 = s2;
                S3 = s3;
            }

            public string S1 { get; set; }

            public string S2 { get; set; }

            public string S3 { get; set; }
        }
    }
}
