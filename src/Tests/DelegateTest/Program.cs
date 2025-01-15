using System.Threading.Tasks;

namespace DelegateTest
{
    internal sealed class Program
    {
        private static async Task Main(string[] args)
        {
            //BenchmarkRunner.Run<BenchmarkTest>();

            //new DelegateTest().GetMehtodTest();
            //Console.WriteLine();
            //new DelegateTest().GetAndExecTest();

            //MethodDelegate.DelegatePerformanceTest();
            //MethodDelegate.StaticMethodTest();
            //MethodDelegate.ExpressionTest();
            //MethodDelegate.GenericMethod();

            //new PropTest().PropAndDelegate_Exec_Test();

            await FuncDelegateTest.Exec_WithFuncDelegate();
            await FuncDelegateTest.Exec_WithFuncResult();
        }
    }
}
