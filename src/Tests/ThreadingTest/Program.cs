namespace ThreadingTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //TaskDelayTest.Test();

            //ReaderWriterLockTest.ReaderWriterLock_Test();
            //ReaderWriterLockTest.UpgradeToWriterLock_Test();
            //ReaderWriterLockTest.ReleaseLock_Test();

            //ReaderWriterLockSlimTest.TryEnterReadLock_Test();
            //ReaderWriterLockSlimTest.EnterUpgradeableReadLock_WhenHasRead_Test();
            //ReaderWriterLockSlimTest.EnterUpgradeableReadLock_WhenHasWrite_Test();
            //ReaderWriterLockSlimTest.ExitUpgradeableReadLock_Test();
            //ReaderWriterLockSlimTest.EnterUpgradeableReadLock_OnlyOne_Test();
            //ReaderWriterLockSlimTest.Upgradeable_ToRead_Test();

            ChannelTest.Reader_Test();
        }
    }
}
