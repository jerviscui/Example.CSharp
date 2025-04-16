using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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
                _channel?.Dispose();
                _channel = null;
                _connection?.Dispose();
                _connection = null;
            }
        }

        public void Connect(bool useAsyncConsumer)
        {
            if (_connection is not null && !_connection.IsOpen)
            {
                _connection.Dispose();
                _connection = null;
            }

            _connection ??= RabbitMq.CreateConnection("Consumer", useAsyncConsumer);

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
            Connect(false);

            Debug.Assert(_channel is not null);

            RabbitMq.AutoCreate(_channel, exchange, routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                Console.WriteLine($"receive: {args.Exchange} {args.RoutingKey} {args.DeliveryTag} {args.Redelivered}");
                OnReceived?.Invoke(sender, args);

                _channel.BasicAck(args.DeliveryTag, false);
            };
            consumer.Registered += (sender, args) => { };
            consumer.Shutdown += (sender, args) => { };
            consumer.Unregistered += (sender, args) => { };
            consumer.ConsumerCancelled += (sender, args) => { };

            var consumerTag = IModelExensions.BasicConsume(_channel, RabbitMq.AutoQueueName(exchange, routingKey), false, consumer);
        }

        public void SubscribeAsyncConsumer(string exchange, string routingKey)
        {
            Connect(true);

            Debug.Assert(_channel is not null);

            RabbitMq.AutoCreate(_channel, exchange, routingKey);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                Console.WriteLine($"receive: {args.Exchange} {args.RoutingKey} {args.DeliveryTag} {args.Redelivered}");

                if (OnReceivedAsync is null)
                {
                    _channel.BasicAck(args.DeliveryTag, false);
                    return Task.CompletedTask;
                }

                var task = OnReceivedAsync.Invoke(sender, args);
                task = task.ContinueWith(_ => _channel.BasicAck(args.DeliveryTag, false));

                return task;
            };
            consumer.Registered += (sender, args) => Task.CompletedTask;
            consumer.Shutdown += (sender, args) => Task.CompletedTask;
            consumer.Unregistered += (sender, args) => Task.CompletedTask;
            consumer.ConsumerCancelled += (sender, args) => Task.CompletedTask;

            _channel.BasicQos(0, 2, false);

            var arguments = new Dictionary<string, object> { { "x-priority", 10 } };
            var consumerTag =
                _channel.BasicConsume(RabbitMq.AutoQueueName(exchange, routingKey), false, "", arguments, consumer);
        }

        public event EventHandler<BasicDeliverEventArgs>? OnReceived;

        public event AsyncEventHandler<BasicDeliverEventArgs>? OnReceivedAsync;
    }
}
