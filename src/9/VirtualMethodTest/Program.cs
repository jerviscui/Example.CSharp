namespace VirtualMethodTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CallvirtTests.InstanceMethod_CallvirtPerformance_Test();
            CallvirtTests.VirtualMethod_CallvirtPerformance_Test();

            //var t = new VirtualMethodTest();
            //t.OverrideEmptyMethodTest();
            //t.OverrideMethodTest();
            //t.CovariantExecMethodTest();
            //t.ChangeRuntimeTypeAndCovariantExecMethodTest();
            //t.NoInlineTest();
            //t.InterfaceExecTest();
        }
    }
}
