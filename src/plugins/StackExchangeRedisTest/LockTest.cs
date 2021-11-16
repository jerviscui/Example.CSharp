using System;

namespace StackExchangeRedisTest
{
    internal class LockTest
    {
        public void Test()
        {
            var database = DatabaseProvider.GetDatabase();

            database.LockTake("", "", TimeSpan.Zero);

            database.LockExtend("", "", TimeSpan.Zero);

            database.LockRelease("", "");
        }
    }
}
