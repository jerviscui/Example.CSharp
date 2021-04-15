using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodBoundaryAspectTest
{
    public class Test
    {
        [TransactionScope]
        public void Method()
        {
            Console.WriteLine("Do some database stuff isolated in surrounding transaction");
        }
    }
}
