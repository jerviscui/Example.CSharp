using System;
using System.Buffers;

namespace StringFormatTest;

internal static class SearchValuesTest
{

    #region Constants & Statics

    private const string Haystack = """
        The Project Gutenberg eBook of The Adventures of Sherlock Holmes,
        by Arthur Conan Doyle

        This eBook is for the use of anyone anywhere in the United States and
        most other parts of the world at no cost and with almost no restrictions
        whatsoever.
         You may copy it, give it away or re-use it under the terms
        of the Project Gutenberg License included with this eBook or online at
        www.gutenberg.org. If you are not located in the United States, you
        will have to check the laws of the country where you are located before
        using this eBook.

        Title: The Adventures of Sherlock Holmes

        Author: Arthur Conan Doyle

        Release Date: November 29, 2002 [eBook #1661]
        [Most recently updated: October 10, 2023]

        Language: English

        Character set encoding: UTF-8

        Produced by: an anonymous Project Gutenberg volunteer and Jose Menendez

        *** START OF THE PROJECT GUTENBERG EBOOK THE ADVENTURES OF SHERLOCK
        HOLMES ***
        """;

    private static readonly SearchValues<char> LineEndings = SearchValues.Create("\n\r\f\u0085\u2028\u2029");

    #endregion

    #region Methods

    internal static void Test()
    {
        ReadOnlySpan<char> haystack = Haystack;
        var pos = haystack.IndexOfAny(LineEndings);
        Console.WriteLine(pos);
    }

    #endregion

}
