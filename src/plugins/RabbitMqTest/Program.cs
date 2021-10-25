using System;
using System.Threading.Tasks;

namespace RabbitMqTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //PublishTest.PublishOneMessage_Test();
            Task.Factory.StartNew(PublishTest.PublishNoWait_Test);

            Console.ReadLine();
        }
    }

    public class Consumer
    {
    }
}
