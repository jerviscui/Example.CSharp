using System;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using MethodBoundaryAspect.Fody.Attributes;

namespace MethodBoundaryAspectTest
{
    [AspectSkipProperties(true)]
    public sealed class TransactionScopeAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = new TransactionScope();
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            var transactionScope = (TransactionScope)args.MethodExecutionTag;

            transactionScope.Complete();
            transactionScope.Dispose();
        }

        public override void OnException(MethodExecutionArgs args)
        {
            var transactionScope = (TransactionScope)args.MethodExecutionTag;

            transactionScope.Dispose();
        }
    }

    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>", Scope = "member")]
    [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
    [TransactionScope(AttributeTargetMemberAttributes = MulticastAttributes.Public)]
    public class SyncTest
    {
        private string PrivateProp { get; set; } = null!;

        public string Prop { get; set; } = null!;

        private void PrivateMethod()
        {
            Console.WriteLine("PrivateMethod");
        }

        //[TransactionScope]
        public void Method()
        {
            Console.WriteLine("Do some database stuff isolated in surrounding transaction");
        }

        public void StructParaMethod(int i)
        {
            Console.WriteLine("StructParaMethod");
        }

        public void RefParaMethod(string s)
        {
            Console.WriteLine("RefParaMethod");
        }

        public string ReturnMethod(string s)
        {
            Console.WriteLine("ReturnMethod");
            return s;
        }
    }
}
