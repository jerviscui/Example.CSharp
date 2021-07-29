using System.Threading.Tasks;

namespace ValueTaskTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await MultiplyAsync(0, 1);
            await MultiplyAsync(1, 1);
        }

        private static async ValueTask<int> MultiplyAsync(int x, int y)
        {
            if (x == 0 || y == 0)
            {
                return 0;
            }

            return await Task.Run(() => x * y);
        }
    }
}
