using System.Runtime.CompilerServices;

internal static class Check
{
    public static void LessThan(this string str, int max, [CallerArgumentExpression("str")] string? message = null)
    {
        if (str.Length > max)
        {
            throw new ArgumentException($"{message} must less than {max}");
        }
    }
}
