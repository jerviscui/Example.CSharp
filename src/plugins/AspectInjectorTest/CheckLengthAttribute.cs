using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.PerInstance)]
    public class PropAspect
    {
        [Advice(Kind.Before, Targets = Target.Setter | Target.Public | Target.Instance)]
        public void OnSet([Argument(Source.Triggers)] Attribute[] attributes, 
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)]string name)
        {
            var checkLength = attributes.Reverse().OfType<CheckLengthAttribute>().FirstOrDefault();

            if (checkLength != null)
            {
                if (((string)arguments[0]).Length > checkLength.MaximumLength || 
                    ((string)arguments[0]).Length < checkLength.MinimumLength)
                {
                    throw new ArgumentException($"{name} length must >= {checkLength.MinimumLength} and <= {checkLength.MaximumLength}");
                }
            }
        }
    }

    [Injection(typeof(PropAspect), Propagation = PropagateTo.Properties)]
    public class CheckLengthAttribute : Attribute
    {
        public uint MaximumLength { get; set; }

        public uint MinimumLength { get; set; }

        public CheckLengthAttribute(uint maximumLength = 0, uint minimumLength = 0)
        {
            MaximumLength = maximumLength;
            MinimumLength = minimumLength;
        }
    }
}
