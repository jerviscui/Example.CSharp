using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MethodBoundaryAspect.Fody.Attributes;

namespace MethodBoundaryAspectTest
{
    public sealed class StringLengthCheckAttribute : OnMethodBoundaryAspect
    {
        /// <inheritdoc />
        public StringLengthCheckAttribute(int max) => Max = max;

        public int Max { get; set; }

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
                    Console.WriteLine($"{args.Method.Name[4..]} must less than {Max.ToString()}");
                }
            }

            //else get;
        }
    }

    public class PropTest
    {
        [StringLength(1)]
        [StringLengthCheck(1)]
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public string Prop { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        public void Method()
        {
        }
    }

    public class PropTest2
    {
        private static readonly MethodBase Ms = typeof(PropTest2).GetMethod("set_Prop")!;

        private readonly int _max = 1;

        private string _p = null!;

        [StringLength(1)]
        public string Prop
        {
            get => _p;
            set
            {
                var v = new MethodExecutionArgs { Arguments = Array.Empty<object>() };
                var m = Ms; //55,867 us
                //MethodBase m = typeof(PropTest2).GetMethod("set_Prop"); //137,632 us
                //MethodBase m = MethodBase.GetCurrentMethod();           //1,072,483 us
                v.Method = m;
                v.Instance = this;
                var attr = new StringLengthCheckAttribute(1);

                if (value.Length > _max)
                {
                    Console.WriteLine($"Prop must less than {_max}");
                }

                _p = value;
            }
        }

        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        public void Method()
        {
        }
    }
}
