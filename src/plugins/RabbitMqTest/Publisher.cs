using System;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client;

namespace RabbitMqTest
{
    public class Publisher : IDisposable
    {
        //Connection
        private IConnection? _connection;

        //Model
        private IModel? _channel;

        public void Connect()
        {
            if (_connection is not null && !_connection.IsOpen)
            {
                _connection.Dispose();
                _connection = null;
            }

            _connection ??= RabbitMq.CreateConnection();

            if (_channel is not null && !_channel.IsOpen)
            {
                _channel.Dispose();
                _channel = null;
            }

            _channel ??= _connection.CreateModel();
        }

        private static void AutoCreate(IModel channel, string exchange, string routingKey)
        {
            channel.ExchangeDeclare(exchange, ExchangeType.Topic, true);

            var queue = RabbitMq.AutoQueueName(exchange, routingKey);
            var queueDeclare = channel.QueueDeclare(queue, true, false, false, RabbitMq.AutoQueueArguments);

            channel.QueueBind(queue, exchange, routingKey);
        }

        public void Publish(string exchange, string routingKey, Dictionary<string, string>? headers,
            string? expiration, params ReadOnlyMemory<byte>[] bodies)
        {
            Connect();

            AutoCreate(_channel, exchange, routingKey);

            _channel.ConfirmSelect();

            var props = _channel.CreateBasicProperties();
            props.DeliveryMode = 2;
            if (expiration is not null)
            {
                props.Expiration = expiration;
            }
            if (headers is not null)
            {
                props.Headers = headers.ToDictionary(x => x.Key, x => (object)x.Value);
            }

            try
            {
                for (int i = 0; i < bodies.Length; i++)
                {
                    _channel.BasicPublish(exchange, routingKey, props, bodies[i]);
                }

                _channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(bodies.Length * 3));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (disposing)
            {
                _connection?.Dispose();
                _connection = null;
            }
        }
    }
}
