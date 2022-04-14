using System;
using System.Buffers;
using System.Threading.Tasks;
using Common;

namespace MemoryModelTest;

public static class ArrayPoolTest
{
    private static readonly ArrayPool<byte> Pool1 = ArrayPool<byte>.Create();

    public static async Task ArrayPool_Test()
    {
        int num = 1_000_000;

        for (int i = 0; i < 10; i++)
        {
            int j = i;

#pragma warning disable CS4014
            Task.Run(async () =>
#pragma warning restore CS4014
            {
                var random = new Random(j);
                while (true)
                {
                    var bytes = GetArray(1_000_000 + random.Next(100_000), true);
                    await Task.Delay(10);
                    Pool1.Return(bytes);
                }
            });
        }

        while (true)
        {
            Print.MemoryInfo();
            await Task.Delay(1000);
        }
    }

    private static byte[] GetArray(int length, bool usePool)
    {
        if (usePool)
        {
            return Pool1.Rent(length);
        }

        return new byte[length];
    }

    public static async Task Array_Test()
    {
        int num = 1_000_000;

        for (int i = 0; i < 10; i++)
        {
            int j = i;

#pragma warning disable CS4014
            Task.Run(async () =>
#pragma warning restore CS4014
            {
                var random = new Random(j);
                while (true)
                {
                    var bytes = GetArray(1_000_000 + random.Next(100_000), false);
                    await Task.Delay(10);
                }
            });
        }

        while (true)
        {
            Print.MemoryInfo();
            await Task.Delay(1000);
        }
    }
}
