using System.ComponentModel.DataAnnotations;

namespace AspectInjectorTest
{
    [CheckLength]
    public class PropTest
    {
        [CheckLength(10, 1)]
        [StringLength(10, MinimumLength = 1)]
        [Log]
        public string S { get; set; }

        private string S1 { get; set; }
    }
}