using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqTest
{
    internal class DelayQueueTest
    {
        public static void Test()
        {
            CreateConsumer();

            var channel = CreatePublisher();

            var data = JsonSerializer.SerializeToUtf8Bytes(DateTime.Now);
            channel.BasicPublish("Ex.DelayQueue", "route.DelayQueue.Order", body: data);
            Thread.Sleep(5000);
            data = JsonSerializer.SerializeToUtf8Bytes(DateTime.Now);
            channel.BasicPublish("Ex.DelayQueue", "route.DelayQueue.Order", body: data);

            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        private static IModel CreatePublisher()
        {
            var connection = RabbitMq.CreateConnection("DelayQueuePublisher");
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("Ex.DelayQueue", ExchangeType.Topic, true);

            var arguments = new Dictionary<string, object>
            {
                { "x-message-ttl", 10 * 1000 }, { "x-dead-letter-exchange", "Ex.dl.Order" }
            };
            channel.QueueDeclare("Mq.DelayQueue", true, false, false, arguments);

            channel.QueueBind("Mq.DelayQueue", "Ex.DelayQueue", "route.DelayQueue.Order");

            return channel;
        }

        private static void CreateConsumer()
        {
            var connection = RabbitMq.CreateConnection("DeadLetterConsumer");
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("Ex.dl.Order", ExchangeType.Topic, true);

            channel.QueueDeclare("Mq.dl.Order", true, false, false);

            channel.QueueBind("Mq.dl.Order", "Ex.dl.Order", "route.DelayQueue.Order");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ConsumerOnReceived;

            channel.BasicConsume("Mq.dl.Order", false, consumer);
        }

        private static void ConsumerOnReceived(object? sender, BasicDeliverEventArgs e)
        {
            if (sender is EventingBasicConsumer consumer)
            {
                var channel = consumer.Model;

                channel.BasicAck(e.DeliveryTag, false);
            }
        }
    }
}
