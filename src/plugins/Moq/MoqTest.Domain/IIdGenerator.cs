using System.Threading;

namespace MoqTest.Domain
{
    public interface IIdGenerator
    {
        public long Create();
    }

    public class DefaultIdGenerator : IIdGenerator
    {
        private static long _id;

        public long Create()
        {
            return Interlocked.Increment(ref _id);
        }
    }
}
