using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace MethodDecoratorTest
{
    [Log(30, 1)]
    public class Test
    {
        [Log(20, 10)]
        public string Name { get; set; }

        public string Text { get; set; }

        public Test(string name, string text)
        {
            Name = name;
            Text = text;
        }

        [Log(10, 1)]
        public Test Show()
        {
            //MethodBase methodFromHandle = MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/, typeof(Test).TypeHandle);
            //Attribute[] customAttributes = Attribute.GetCustomAttributes(methodFromHandle, typeof(LogAttribute));
            IEnumerable<LogAttribute> customAttributes = typeof(Test).GetMethod(nameof(Show))!.GetCustomAttributes<LogAttribute>();

            foreach (var customAttribute in customAttributes)
            {
                Console.WriteLine($"{customAttribute.MaximumLength}+{customAttribute.MinimumLength}");
            }

            Console.WriteLine($"{Name} {Text}");

            return this;
        }

        [Log(10, 2)]
        public Task ShowAsync()
        {
            Console.WriteLine($"{Name} {Text}");

            return Task.CompletedTask;
        }

        [Log(10, 3)]
        public async Task ShowAsync2()
        {
            Console.WriteLine($"{Name} {Text}");
            await Task.Delay(100);
        }
    }
}
