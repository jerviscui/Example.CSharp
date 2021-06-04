using System;
using System.Diagnostics;
using System.Threading;
using PostSharp.Aspects;
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
#if DEBUG
            Console.WriteLine("OnGetValue");
#endif

            args.ProceedGetValue();
        }

        /// <inheritdoc />
        public override void OnSetValue(LocationInterceptionArgs args)
        {
#if DEBUG
            Console.WriteLine("OnSetValue");
#endif

            var s = (string?)args.Value;

            if (s is not null)
            {
                if (s!.Length < Min)
                {
#if DEBUG
                    Console.WriteLine($"{args.LocationName} must greater or equal {Min}");
#endif
                    return;
                }

                if (s!.Length > Max)
                {
#if DEBUG
                    Console.WriteLine($"{args.LocationName} must less or equal {Max}");
#endif
                    return;
                }
            }

            args.ProceedSetValue();
        }

        /// <inheritdoc />
        public override void OnInstanceLocationInitialized(LocationInitializationArgs args)
        {
            base.OnInstanceLocationInitialized(args);
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

        private bool AllowNull;

        private int Max = 5;
        private int Min = 1;

        public string S
        {
            get => _s;

            set
            {
#if DEBUG
                Console.WriteLine("OnSetValue");
#endif
                var s = (string?)value;

                if (s is not null)
                {
                    if (s!.Length < Min)
                    {
#if DEBUG
                        Console.WriteLine($"S must greater or equal {Min}");
#endif
                        return;
                    }

                    if (s!.Length > Max)
                    {
#if DEBUG
                        Console.WriteLine($"S must less or equal {Max}");
#endif
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
#if DEBUG
                Console.WriteLine("OnSetValue");
#endif

                var s = (string?)value;

                if (s is not null)
                {
                    if (s!.Length < Min)
                    {
#if DEBUG
                        Console.WriteLine($"N must greater or equal {Min}");
#endif
                        return;
                    }

                    if (s!.Length > Max)
                    {
#if DEBUG
                        Console.WriteLine($"N must less or equal {Max}");
#endif
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
