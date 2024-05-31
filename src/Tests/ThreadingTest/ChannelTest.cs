using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ThreadingTest
{
    internal sealed class ChannelTest
    {
        private static Channel<int>? _channel;

        private static Channel<int> GetBoundedChannel() => _channel ??=
            Channel.CreateBounded<int>(new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.Wait,
                AllowSynchronousContinuations = false,
                SingleReader = false,
                SingleWriter = true
            });

        private static Channel<int> GetUnbounded() => _channel ??=
            Channel.CreateUnbounded<int>(new UnboundedChannelOptions
            {
                AllowSynchronousContinuations = false, SingleReader = true, SingleWriter = false
            });

        public static void Reader_Test()
        {
            var channel = GetBoundedChannel();
            var writer = channel.Writer;
            var reader = channel.Reader;

            var producer = Task.Run(async () =>
            {
                for (int i = 0; i < 200; i++)
                {
                    while (await writer.WaitToWriteAsync())
                    {
                        if (writer.TryWrite(i))
                        {
                            Thread.Sleep(10);
                            break;
                        }
                    }
                }

                writer.Complete();
            });

            var consumers = new Task[2];
            for (int i = 0; i < consumers.Length; i++)
            {
                var i1 = i + 1;
                var consumer = Task.Run(async () =>
                {
                    while (await reader.WaitToReadAsync())
                    {
                        while (reader.TryRead(out var item))
                        {
                            Console.WriteLine($"consumer{i1}: {item}");
                        }
                    }
                });
                consumers[i] = consumer;
            }

            Task.WaitAll(consumers);
        }

        public static async Task Test()
        {
            var channel = GetUnbounded();

            ChannelWriter<int> writer = channel.Writer;
            ChannelReader<int> reader = channel.Reader;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < 10_000_000; i++)
            {
                //1:
                //writer.TryWrite(i);
                //await reader.ReadAsync();

                //2:
                var task = reader.ReadAsync();
                writer.TryWrite(i);
                await task;
            }

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);

            //1 比 2 节省一倍的时间，
            //原因是 ReadAsync() 构造异步对象产生一些开销
        }
    }
}
