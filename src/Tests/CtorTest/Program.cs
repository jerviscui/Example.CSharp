namespace CtorTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //InitializerTest.Ctor_Initializer_ExecutionOrder();
            //VirtualMemberTest.BaseCtor_UseOverrideMember_Test();

            //BenchmarkRunner.Run<NullableReferenceTest>();

            ReflectionTest.CreateInstance_UseProtectedCtor_Test();
        }
    }
}
