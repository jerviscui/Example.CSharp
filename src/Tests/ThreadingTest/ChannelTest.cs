using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ThreadingTest
{
    internal class ChannelTest
    {
        private static Channel<string>? _channel;

        private static Channel<string> GetBoundedChannel() => _channel ??=
            Channel.CreateBounded<string>(new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.Wait,
                AllowSynchronousContinuations = false,
                SingleReader = true,
                SingleWriter = false
            });

        private static Channel<string> GetUnbounded() => _channel ??=
            Channel.CreateUnbounded<string>(new UnboundedChannelOptions
            {
                AllowSynchronousContinuations = false, SingleReader = true, SingleWriter = false
            });

        public static async Task Test()
        {
            var channel = GetBoundedChannel();
            var writer = channel.Writer;

            await writer.WriteAsync("");

            while (await writer.WaitToWriteAsync())
            {
                if (writer.TryWrite(""))
                {
                    return;
                }
            }
        }

        public static async Task Read()
        {
            var channel = GetBoundedChannel();
            var reader = channel.Reader;

            while (await reader.WaitToReadAsync())
            {
                while (reader.TryRead(out var item))
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
