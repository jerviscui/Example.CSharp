using System;
using System.Text.Json;
using System.Threading;

namespace RabbitMqTest
{
    internal class ConsumerTest
    {
        public static void ReceiveQueueMessage_Test(CancellationToken token)
        {
            using var consumer = new Consumer();
            consumer.OnReceived += (sender, args) =>
            {
                var properties = args.BasicProperties;
                var body = args.Body;
                var date = JsonSerializer.Deserialize<DateTime>(body.Span);
            };
            consumer.Subscribe("test.exchange", "test.route.multi");

            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(1000 * 10);
            }
        }
    }
}
