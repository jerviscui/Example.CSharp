using System;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Log]
    public class SyncBoundaryTest
    {
        public void PublicTest()
        {

        }

        private void PrivateTest()
        {

        }

        public string PropTest { get; set; } = string.Empty;
    }
}