using System.Threading.Tasks;

namespace ThreadingTest
{
    internal class TaskDelayTest
    {
        public static void Test()
        {
            var task = Task.Run(async () =>
            {
                await Task.Delay(1000 * 10);
                return 100;
            });

            task.Wait();
            var r = task.Result;
        }
    }
}
