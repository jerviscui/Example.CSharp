using System.Threading.Channels;

namespace ThreadingTest
{
    internal class ChannelTest
    {
        public void Test()
        {
            var channel = Channel.CreateBounded<string>(new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.Wait,
                AllowSynchronousContinuations = false,
                SingleReader = true,
                SingleWriter = false
            });

            channel = Channel.CreateUnbounded<string>(new UnboundedChannelOptions
            {
                AllowSynchronousContinuations = false, SingleReader = true, SingleWriter = false
            });
        }
    }
}
