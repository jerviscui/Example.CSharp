using System;
using System.Threading.Tasks;

namespace MethodDecoratorTest
{
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
        public void Show()
        {
            Console.WriteLine($"{Name} {Text}");
        }

        [Log(10, 1)]
        public Task ShowAsync()
        {
            Console.WriteLine($"{Name} {Text}");

            return Task.CompletedTask;
        }

        [Log(10, 1)]
        public async Task ShowAsync2()
        {
            Console.WriteLine($"{Name} {Text}");
            await Task.Delay(100);
        }
    }
}