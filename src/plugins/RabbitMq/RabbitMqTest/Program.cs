using System;
using System.Threading;

namespace RabbitMqTest
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            using var cts = new CancellationTokenSource();
            //Task.Factory.StartNew(() => ConsumerTest.ReceiveQueueMessage_Test(cts.Token), cts.Token);
            //Task.Factory.StartNew(() => ConsumerTest.ReceiveQueueMessage_AsyncConsumer_Test(cts.Token), cts.Token);

            //PublishTest.PublishOneMessage_Test();
            //PublishTest.PublishNoWait_Test();

            DelayQueueTest.Test();

            Console.ReadLine();
            cts.Cancel();
        }
    }
}
