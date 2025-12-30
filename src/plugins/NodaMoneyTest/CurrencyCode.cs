using NodaMoney;
using System.Collections.Frozen;
using System.Diagnostics;
using System.Reflection;

namespace NodaMoneyTest;

public static class CurrencyCode
{

    #region Constants & Statics

    // "¥"
    public static readonly string CNY = "CNY";

    // "€"
    public static readonly string EUR = "EUR";

    // "HK$"
    public static readonly string HKD = "HKD";

    // "¥"
    public static readonly string JPY = "JPY";

    // "MOP$"
    public static readonly string MOP = "MOP";

    // "NT$"
    public static readonly string TWD = "TWD";

    // "US$"
    public static readonly string USD = "USD";

    static CurrencyCode()
    {
        var props = typeof(CurrencyCode).GetFields(BindingFlags.Static | BindingFlags.DeclaredOnly);

        var list = new List<Currency>();
        foreach (var field in props)
        {
            var value = field.GetValue(null) as string;
            Debug.Assert(value is not null, $"{nameof(value)} is null.");

            list.Add(Currency.FromCode(value));
        }

        var dic = list.ToFrozenDictionary(static o => o.Code, o => o);
    }

    #endregion

}
