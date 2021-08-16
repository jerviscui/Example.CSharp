using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

//[assembly: Fody.ConfigureAwait(true)]
namespace ConfigureAwaitTest
{
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class AssemblyTest
    {
        public async Task Test2()
        {
            await Task.Delay(10);
            //编译后
            //await Task.Delay(10).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
