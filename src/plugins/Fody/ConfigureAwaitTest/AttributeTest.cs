using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fody;

namespace ConfigureAwaitTest
{
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    [ConfigureAwait(true)]
    public class AttributeTest
    {
        public async Task Test2()
        {
            await Task.Delay(10);
            //编译后
            //await Task.Delay(10).ConfigureAwait(continueOnCapturedContext: true);
        }

        [ConfigureAwait(false)]
        public async Task Test3()
        {
            await Task.Delay(10);
            //编译后
            //await Task.Delay(10).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
