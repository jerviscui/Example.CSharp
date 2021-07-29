namespace DelegateTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //BenchmarkRunner.Run<BenchmarkTest>();

            //new DelegateTest().GetMehtodTest();
            //Console.WriteLine();
            //new DelegateTest().GetAndExecTest();

            //MethodDelegate.DelegatePerformanceTest();
            //MethodDelegate.StaticMethodTest();
            //MethodDelegate.ExpressionTest();
            //MethodDelegate.GenericMethod();

            new PropTest().PropAndDelegate_Exec_Test();
        }
    }
}
