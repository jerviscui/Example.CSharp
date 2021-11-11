using System.Threading;

namespace ThreadingTest
{
    internal class ReaderWriterLockTest
    {
        public static void ReaderWriterLock_Test()
        {
            var rwLock = new ReaderWriterLock();
        }

        public void Test()
        {
            var rwLock = new ReaderWriterLock();

            var rwLockSlim = new ReaderWriterLockSlim();
        }
    }
}
