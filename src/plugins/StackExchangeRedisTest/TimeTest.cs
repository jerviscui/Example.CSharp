using System;
using System.Diagnostics;

namespace StackExchangeRedisTest;

internal sealed class TimeTest
{
    public static void TimeToDateTime_Test()
    {
        var database = DatabaseProvider.GetDatabase();

        var result = database.Execute("time");
        if (!result.IsNull)
        {
            var values = (long[])result!;

            if (values.Length == 2)
            {
                var seconds = values[0];
                var micro = values[1];

                var time = DateTime.UnixEpoch.AddSeconds(seconds) +
                    TimeSpan.FromTicks(micro * Stopwatch.Frequency / 1000_000);
                var s = time.ToString("O");
            }
        }
    }
}
