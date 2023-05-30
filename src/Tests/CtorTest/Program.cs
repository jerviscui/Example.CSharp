using System;

namespace CtorTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //InitializerTest.Ctor_Initializer_ExecutionOrder();
            //VirtualMemberTest.BaseCtor_UseOverrideMember_Test();

            //BenchmarkRunner.Run<NullableReferenceTest>();

            //ReflectionTest.CreateInstance_UseProtectedCtor_Test();

            //catch when test
            try
            {
                throw new ArgumentException("test", nameof(args));
            }
            catch (ArgumentException e) when (e.ParamName == nameof(args))
            {
                Console.WriteLine(e.ParamName);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.ParamName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
