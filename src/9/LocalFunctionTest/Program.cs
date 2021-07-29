using System.Threading.Tasks;

namespace LocalFunctionTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //LocalFunctionWithEnumerable.Test();
            await LocalFunctionWithTask.Test();
        }
    }
}
