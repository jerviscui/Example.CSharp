using System;
using System.Text.Json;
using System.Threading;

namespace RabbitMqTest
{
    internal class PublishTest
    {
        public static void PublishOneMessage_Test()
        {
            using var publisher = new Publisher();

            var data = JsonSerializer.SerializeToUtf8Bytes(DateTime.Now);
            publisher.Publish("test.exchange", "test.route", null, null, data);
        }

        public static Publisher PublishNoWait_Test()
        {
            using var publisher = new Publisher();

            var data = JsonSerializer.SerializeToUtf8Bytes(DateTime.Now);
            var data1 = JsonSerializer.SerializeToUtf8Bytes(DateTime.Now.AddMinutes(1));
            var data2 = JsonSerializer.SerializeToUtf8Bytes(DateTime.Now.AddMinutes(2));
            publisher.PublishNoWait("test.exchange", "test.route.multi", null, null, data, data1, data2);

            Thread.Sleep(1000 * 30);
            var failed = publisher.Failed;

            return publisher;
        }
    }
}
