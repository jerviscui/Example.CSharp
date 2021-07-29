using System;
using System.Diagnostics;
using System.Threading;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;
using PostSharp.Reflection;
using PostSharp.Serialization;

namespace LocationInterceptionTest
{
    [PSerializable]
    public class MyLocationInterceptionAttribute : LocationInterceptionAspect
    {
        public int Max { get; set; }

        public int Min { get; set; }

        /// <inheritdoc />
        public override void OnGetValue(LocationInterceptionArgs args)
        {
            //Console.WriteLine("OnGetValue");

            args.ProceedGetValue();
        }

        /// <inheritdoc />
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            //Console.WriteLine("OnSetValue");

            var s = (string?)args.Value;

            if (s is not null)
            {
                if (s!.Length < Min)
                {
                    //Console.WriteLine($"{args.LocationName} must greater or equal {Min}");
                    return;
                }

                if (s!.Length > Max)
                {
                    //Console.WriteLine($"{args.LocationName} must less or equal {Max}");
                    return;
                }
            }

            args.ProceedSetValue();
        }

        /// <inheritdoc />
        public override bool CompileTimeValidate(LocationInfo locationInfo)
        {
            return locationInfo.PropertyInfo.PropertyType == typeof(string);
        }
    }

    public class PropTest
    {
        [MyLocationInterception(Min = 1, Max = 5)]
        public string S { get; set; }

        [MyLocationInterception(Min = 1)]
        public string? N { get; set; }

        [MyLocationInterception]
        public int I { get; set; }

        public PropTest(string s, string? n = null)
        {
            //S = s;
            //N = n;
        }
    }

    public class PropTest2
    {
        private string _s;

        private int Max = 5;
        private int Min = 1;

        public string S
        {
            get => _s;

            set
            {
                //Console.WriteLine("OnSetValue");
                var s = (string?)value;

                if (s is not null)
                {
                    if (s!.Length < Min)
                    {
                        //Console.WriteLine($"S must greater or equal {Min}");
                        return;
                    }

                    if (s!.Length > Max)
                    {
                        //Console.WriteLine($"S must less or equal {Max}");
                        return;
                    }
                }

                _s = value;
            }
        }

        private string _n;

        public string? N
        {
            get => _n;

            set
            {
                //Console.WriteLine("OnSetValue");
                var s = (string?)value;

                if (s is not null)
                {
                    if (s!.Length < Min)
                    {
                        //Console.WriteLine($"N must greater or equal {Min}");
                        return;
                    }

                    if (s!.Length > Max)
                    {
                        //Console.WriteLine($"N must less or equal {Max}");
                        return;
                    }
                }

                _n = value;
            }
        }

        public int I { get; set; }

        public PropTest2(string s, string? n = null)
        {
            //S = s;
            //N = n;
        }
    }
}
