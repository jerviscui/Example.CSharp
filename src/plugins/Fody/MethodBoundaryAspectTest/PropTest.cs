using MethodBoundaryAspect.Fody.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MethodBoundaryAspectTest
{
    public sealed class StringLengthCheckAttribute : OnMethodBoundaryAspect
    {
        public int Max { get; set; }

        /// <inheritdoc />
        public StringLengthCheckAttribute(int max)
        {
            Max = max;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            //set;
            if (args.Arguments.Length == 1)
            {
                var value = (string?)args.Arguments[0];

                if (value != null && value.Length > Max)
                {
                    //throw new ArgumentException();
                    //"set_" 
                    Console.WriteLine($"{args.Method.Name.Substring(4)} must less than {Max}");
                }
            }

            //else get;
        }
    }

    public class PropTest
    {
        [StringLength(1)]
        [StringLengthCheck(1)]
        public string Prop { get; set; }

        public void Method()
        {

        }
    }

    public class PropTest2
    {
        private int _max = 1;

        private string _p;

        private static MethodBase var_2 = typeof(PropTest2).GetMethod("set_Prop");

        [StringLength(1)]
        public string Prop
        {
            get
            {
                return _p;
            }
            set
            {
                MethodExecutionArgs __var_ = new MethodExecutionArgs();
                __var_.Arguments = new object[0];
                MethodBase __var_2 = var_2;//55,867 us
                //MethodBase __var_2 = typeof(PropTest2).GetMethod("set_Prop");//137,632 us
                //MethodBase __var_2 = MethodBase.GetCurrentMethod();//1,072,483 us
                __var_.Method = __var_2;
                __var_.Instance = this;
                StringLengthCheckAttribute __var_3 = new StringLengthCheckAttribute(1);

                if (value.Length > _max)
                {
                    Console.WriteLine($"Prop must less than {_max}");
                }

                _p = value;
            }
        }

        public void Method()
        {

        }
    }
}
