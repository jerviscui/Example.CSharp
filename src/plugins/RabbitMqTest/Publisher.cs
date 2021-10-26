using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client;

namespace RabbitMqTest
{
    public class Publisher : IDisposable
    {
        public readonly ConcurrentDictionary<ulong, ReadOnlyMemory<byte>> Failed = new();

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

        public void Connect()
        {
            if (_connection is not null && !_connection.IsOpen)
            {
                _connection.Dispose();
                _connection = null;
            }

            _connection ??= RabbitMq.CreateConnection("Publisher");

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

        public void Publish(string exchange, string routingKey, Dictionary<string, string>? headers,
            string? expiration, params ReadOnlyMemory<byte>[] bodies)
        {
            Verify(exchange, routingKey);

            InternalPublish(exchange, routingKey, headers, expiration, bodies);

            try
            {
                _channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(bodies.Length * 3));
                Failed.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void PublishNoWait(string exchange, string routingKey, Dictionary<string, string>? headers,
            string? expiration, params ReadOnlyMemory<byte>[] bodies)
        {
            Verify(exchange, routingKey);

            _channel.BasicAcks += (sender, args) =>
            {
                if (!args.Multiple)
                {
                    Failed.TryRemove(args.DeliveryTag, out _);
                }
                else
                {
                    foreach (var keyValuePair in Failed.Where(o => o.Key <= args.DeliveryTag))
                    {
                        Failed.TryRemove(keyValuePair.Key, out _);
                    }
                }
            };
            _channel.BasicNacks += (sender, args) =>
            {
                Console.WriteLine($"{args.Multiple} {args.DeliveryTag}");
            };

            InternalPublish(exchange, routingKey, headers, expiration, bodies);
        }

        private void Verify(string exchange, string routingKey)
        {
            Connect();

            RabbitMq.AutoCreate(_channel!, exchange, routingKey);
        }

        private void InternalPublish(string exchange, string routingKey, Dictionary<string, string>? headers,
            string? expiration, params ReadOnlyMemory<byte>[] bodies)
        {
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

            for (int i = 0; i < bodies.Length; i++)
            {
                Failed.TryAdd(_channel.NextPublishSeqNo, bodies[i]);
                _channel.BasicPublish(exchange, routingKey, props, bodies[i]);
            }
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
    }
}
