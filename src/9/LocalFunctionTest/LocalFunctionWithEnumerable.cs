using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LocalFunctionTest
{
    [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
    public class LocalFunctionWithEnumerable
    {
        public static void Test()
        {
            //IEnumerable<int> xs = NoLocalFunc(50, 110);
            IEnumerable<int> xs = LocalFunc(50, 110); //LocalFunc throw here
            Console.WriteLine("Retrieved enumerator...");

            foreach (var x in xs) //NoLocalFunc throw here
            {
                Console.Write($"{x} ");
            }
        }

        private static IEnumerable<int> NoLocalFunc(int start, int end)
        {
            if (start < 0 || start > 99)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "start must be between 0 and 99.");
            }

            if (end > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(end), "end must be less than or equal to 100.");
            }

            if (start >= end)
            {
                throw new ArgumentException("start must be less than end.");
            }

            for (int i = start; i <= end; i++)
            {
                if (i % 2 == 1)
                {
                    yield return i;
                }
            }
        }

        private static IEnumerable<int> LocalFunc(int start, int end)
        {
            if (start < 0 || start > 99)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "start must be between 0 and 99.");
            }

            if (end > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(end), "end must be less than or equal to 100.");
            }

            if (start >= end)
            {
                throw new ArgumentException("start must be less than end.");
            }

            return GetOddSequenceEnumerator();

            IEnumerable<int> GetOddSequenceEnumerator()
            {
                for (int i = start; i <= end; i++)
                {
                    if (i % 2 == 1)
                    {
                        yield return i;
                    }
                }
            }
        }
    }
}
