using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqTest
{
    public class Consumer : IDisposable
    {
        //Model
        private IModel? _channel;

        //Connection
        private IConnection? _connection;

        private bool _disposed;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        public void Connect()
        {
            if (_connection is not null && !_connection.IsOpen)
            {
                _connection.Dispose();
                _connection = null;
            }

            _connection ??= RabbitMq.CreateConnection("Consumer");

            if (_channel is not null && !_channel.IsOpen)
            {
                _channel.Dispose();
                _channel = null;
            }

            if (_channel is null)
            {
                _channel = _connection.CreateModel();
                _channel.ConfirmSelect();
            }
        }

        public void Subscribe(string exchange, string routingKey)
        {
            Connect();

            RabbitMq.AutoCreate(_channel!, exchange, routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                OnReceived?.Invoke(sender, args);

                _channel.BasicAck(args.DeliveryTag, false);
            };
            consumer.Registered += (sender, args) => { };
            consumer.Shutdown += (sender, args) => { };
            consumer.Unregistered += (sender, args) => { };
            consumer.ConsumerCancelled += (sender, args) => { };

            //var consumer = new AsyncEventingBasicConsumer(_channel);
            //consumer.Received += (sender, args) => Task.CompletedTask;
            //consumer.Registered += (sender, args) => Task.CompletedTask;
            //consumer.Shutdown += (sender, args) => Task.CompletedTask;
            //consumer.Unregistered += (sender, args) => Task.CompletedTask;
            //consumer.ConsumerCancelled += (sender, args) => Task.CompletedTask;

            var consumerTag = _channel.BasicConsume(RabbitMq.AutoQueueName(exchange, routingKey), false, consumer);
        }

        public event EventHandler<BasicDeliverEventArgs>? OnReceived;

        public event AsyncEventHandler<BasicDeliverEventArgs>? OnReceivedAsync;
    }
}
