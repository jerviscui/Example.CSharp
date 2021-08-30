using System;
using System.Threading.Tasks;

namespace DelegateTest
{
    internal class FuncDelegateTest
    {
        private static async Task<TResult> FuncTaskParameterMethod<TResult>(Func<Task<TResult>> func)
        {
            return await func();
        }

        public static async Task Exec_WithFuncDelegate()
        {
            var result = await FuncTaskParameterMethod(() => Task.FromResult(1));
        }

        public static async Task Exec_WithFuncResult()
        {
            var result = await FuncTaskParameterMethod(async () => await Task.FromResult(1));
        }

        public static async Task Exec_WithLocalFunction()
        {
            Task<int> Func() => Task.FromResult(1);

            var result = await FuncTaskParameterMethod(Func);
        }

        public static async Task Exec_WithFuncDelegateCache()
        {
            Func<Task<int>> func = () => Task.FromResult(1);

            var result = await FuncTaskParameterMethod(func);
        }
    }
}
