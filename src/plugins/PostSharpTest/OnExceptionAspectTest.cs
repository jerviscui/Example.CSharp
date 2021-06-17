using System;
using PostSharp.Aspects;

namespace PostSharpTest
{
    [Serializable]
    public class ExceptionAspectDemoAttribute : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            var msg = $"时间[{DateTime.Now:yyyy年MM月dd日 HH时mm分}]方法{args.Method.Name}发生异常: {args.Exception.Message}\n{args.Exception.StackTrace}";
            Console.WriteLine(msg);

            args.FlowBehavior = FlowBehavior.Continue;
        }
        public override Type GetExceptionType(System.Reflection.MethodBase targetMethod)
        {
            return typeof(NullReferenceException);
        }
    }

    public class OnExceptionAspectTest
    {
        [ExceptionAspectDemo]
        public void Test()
        {
            try
            {
                throw new NullReferenceException("arugment error!");
                //Console.WriteLine("after");
            }
            catch (Exception)
            {
                Console.WriteLine("catch 1");
                throw;
            }
        }
    }
}
