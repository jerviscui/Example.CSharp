using System;
using System.Text.Json;

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
    }
}
