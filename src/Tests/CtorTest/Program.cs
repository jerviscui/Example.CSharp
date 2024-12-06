namespace CtorTest;

internal static class Program
{

    #region Constants & Statics

    private static void Main(string[] args)
    {
        //InitializerTest.Ctor_Initializer_ExecutionOrder();
        //VirtualMemberTest.BaseCtor_UseOverrideMember_Test();

        //BenchmarkRunner.Run<NullableReferenceTest>();

        //ReflectionTest.CreateInstance_UseProtectedCtor_Test();

        //ActivatorTest.CreateWithProtected();
        // throw System.MissingMethodException
        // No parameterless constructor defined for type 'CtorTest.ActivatorTest+MyData'.

        //ActivatorTest.CreateWithPrivate();
        // throw System.MissingMethodException
        // No parameterless constructor defined for type 'CtorTest.ActivatorTest+MyData2'.

        ActivatorTest.CreateWithProtectedSuccess();
    }

    #endregion

}
