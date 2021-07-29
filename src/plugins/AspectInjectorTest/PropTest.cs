using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.PerInstance)]
    public class PropAspect
    {
        [Advice(Kind.Before, Targets = Target.Setter | Target.Instance)]
        public void OnSet([Argument(Source.Triggers)] Attribute[] attributes,
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)] string name)
        {
            var checkLength = attributes.OfType<CheckLengthAttribute>().FirstOrDefault();

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

    [AttributeUsage(AttributeTargets.Property)]
    [Injection(typeof(PropAspect))]
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

    public class PropTest
    {
        [CheckLength(10, 1)]
        [StringLength(10, MinimumLength = 1)]
        public string S { get; set; }

        private string S1 { get; set; }
    }
}