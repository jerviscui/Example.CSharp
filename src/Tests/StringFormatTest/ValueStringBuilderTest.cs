using System;

namespace StringFormatTest;

internal static partial class Program
{
    internal static class ValueStringBuilderTest
    {

        #region Constants & Statics

        public static void Test()
        {
            var vsb = new ValueStringBuilder(stackalloc char[512]);

            vsb.Append("abc");
            vsb.Append("\t");
            vsb.Append("def");

            Console.WriteLine(vsb.ToString());

            vsb.Dispose();
        }

        #endregion
    }
}
