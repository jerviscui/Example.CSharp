using System;
using Common;

namespace RangeIndexTest
{
    internal class Program
    {
        private readonly string[] _words =
        {
            // index from start    index from end
            "The",    // 0                   ^9
            "quick",  // 1                   ^8
            "brown",  // 2                   ^7
            "fox",    // 3                   ^6
            "jumped", // 4                   ^5
            "over",   // 5                   ^4
            "the",    // 6                   ^3
            "lazy",   // 7                   ^2
            "dog"     // 8                   ^1
        };            // 9 (or words.Length) ^0

        private static void Main(string[] args)
        {
            Index_Test();

            Console.WriteLine();
            Range_Test();
        }

        private static unsafe void Index_Test()
        {
            Index i1 = 0;  // number 3 from beginning
            Index i2 = ^4; // number 4 from end
            int[] a = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            fixed (int* p = a, pp = &a[i1])
            {
                Print.Address((long)p);
                Print.Address((long)pp);
            }

            Console.WriteLine($"{a[i1]}, {a[i2]}"); // "3, 6"
        }

        private static unsafe void Range_Test()
        {
            Range r = new Range(Index.Start, Index.End); //等价于 0..^0

            int[] a = { 0, 1, 2, 3 };

            var aa = a[r];
            Span<int> s = a[r];

            a[0] = 10;

            fixed (int* p = a, pp = aa, ps = s)
            {
                Print.Address((long)p);
                Print.Address((long)pp);
                Print.Address((long)ps);
            }

            foreach (var i in aa)
            {
                Console.WriteLine(i);
            }
        }

        private void Methods_Test()
        {
            var allWords = _words[..];     // contains "The" through "dog".
            var firstPhrase = _words[..4]; // contains "The" through "fox"
            var firstPhrase2 = Range.EndAt(4);
            var lastPhrase = _words[6..]; // contains "the", "lazy" and "dog"
            var lastPhrase2 = Range.StartAt(6);
        }
    }
}
