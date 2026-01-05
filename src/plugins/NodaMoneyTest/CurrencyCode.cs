using NodaMoney;
using System.Collections.Frozen;

namespace NodaMoneyTest;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Critical Code Smell",
    "S2339:Public constant members should not be used",
    Justification = "<Pending>")]
public static class CurrencyCode
{

    #region Constants & Statics

    // "¥"
    public const string CNY = "CNY";

    // "€"
    public const string EUR = "EUR";

    // "HK$"
    public const string HKD = "HKD";

    // "¥"
    public const string JPY = "JPY";

    // "MOP$"
    public const string MOP = "MOP";

    // "NT$"
    public const string TWD = "TWD";

    // "US$"
    public const string USD = "USD";

    public static readonly FrozenDictionary<string, Currency> SupportedCurrency = typeof(CurrencyCode).GetFields()
        .Where(static o => o.IsLiteral)
        .Select(static o => Currency.FromCode((string)(o.GetValue(null) ?? throw new InvalidOperationException())))
        .ToFrozenDictionary(static o => o.Code, o => o);

    #endregion

}
