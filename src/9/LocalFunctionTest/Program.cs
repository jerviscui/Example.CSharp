using System;
using System.Threading;
using System.Threading.Tasks;

namespace LocalFunctionTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //LocalFunctionWithEnumerable.Test();
            await LocalFunctionWithTask.Test();
        }
    }
}
