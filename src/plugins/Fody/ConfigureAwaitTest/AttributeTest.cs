using System.Threading.Tasks;

namespace ConfigureAwaitTest
{
    [Fody.ConfigureAwait(true)]
    public class AttributeTest
    {
        public async Task Test2()
        {
            await Task.Delay(10);
            //编译后
            //await Task.Delay(10).ConfigureAwait(continueOnCapturedContext: true);
        }

        [Fody.ConfigureAwait(false)]
        public async Task Test3()
        {
            await Task.Delay(10);
            //编译后
            //await Task.Delay(10).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
