using System.Collections.Generic;
using RabbitMQ.Client;

namespace RabbitMqTest
{
    public static class RabbitMq
    {
        public static int Day = 1000 * 60 * 60 * 24;

        public static Dictionary<string, object> AutoQueueArguments = new() { { "x-message-ttl", 3 * Day } };

        public static string UserName { get; set; } = "guest";

        public static string Password { get; set; } = "guest";

        public static string VirtualHost { get; set; } = "/";

        public static string HostName { get; set; } = "localhost";

        //5672 for regular ("plain TCP") connections, 5671 for connections with TLS enabled
        public static int Port { get; set; } = -1;

        public static IConnection CreateConnection(string? provider = null, bool useAsyncConsumer = false)
        {
            var factory = new ConnectionFactory
            {
                UserName = UserName,
                Password = Password,
                VirtualHost = VirtualHost,
                HostName = HostName,
                Port = Port,
                DispatchConsumersAsync = useAsyncConsumer
            };

            if (provider is not null)
            {
                factory.ClientProvidedName = provider;
            }

            return factory.CreateConnection();
        }

        public static string AutoQueueName(string exchange, string routingKey) => $"{exchange}-{routingKey}-queue";

        public static void AutoCreate(IModel channel, string exchange, string routingKey)
        {
            channel.ExchangeDeclare(exchange, ExchangeType.Topic, true);

            var queue = AutoQueueName(exchange, routingKey);
            var queueDeclare = channel.QueueDeclare(queue, true, false, false, AutoQueueArguments);

            channel.QueueBind(queue, exchange, routingKey);
        }
    }
}
