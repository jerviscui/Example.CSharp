using NodaMoney;

namespace NodaMoneyTest;

public class FastMoneyDto
{

    #region Properties

    public decimal Amount { get; init; }

    public string Currency { get; init; } = CurrencyCode.CNY;

    public string Symbol { get; init; } = "¥";

    #endregion

}

public static class FastMoneyExtensions
{

    #region Constants & Statics

    public static FastMoneyDto ToDto(this FastMoney fastMoney)
    {
        return new FastMoneyDto
        {
            Amount = fastMoney.Amount,
            Currency = fastMoney.Currency.Code,
            Symbol = fastMoney.Currency.Symbol
        };
    }

    #endregion

}
