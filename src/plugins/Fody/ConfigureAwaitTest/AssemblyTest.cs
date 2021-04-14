using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//[assembly: Fody.ConfigureAwait(true)]
namespace ConfigureAwaitTest
{
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
