using MethodBoundaryAspect.Fody.Attributes;
using System;
using System.Transactions;

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

    [TransactionScope(AttributeTargetMemberAttributes = MulticastAttributes.Public)]
    public class SyncTest
    {
        private string PrivateProp { get; set; }

        public string Prop { get; set; }

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
