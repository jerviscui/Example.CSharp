using NodaMoney;
using NodaMoney.Context;
using System.Globalization;

namespace NodaMoneyTest;

public static partial class MoneyTest
{

    #region Constants & Statics

    public static void DefaultCurrency_Test()
    {
        var myDefaultContext = MoneyContext.Create(
            opt =>
            {
                opt.MaxScale = 4;
                opt.DefaultCurrency = CurrencyInfo.FromCode(CurrencyCode.CNY);
                opt.EnforceZeroCurrencyMatching = true;
            });
        MoneyContext.DefaultThreadContext = myDefaultContext;

        var money = new Money(10.1234m);
        Console.WriteLine(money); // ¥10.1234
    }

    public static void FastMoney_Test()
    {
        var eur = new FastMoney(10.1264m, CurrencyCode.EUR);
        var fee = new FastMoney(0.1000m, CurrencyCode.EUR);
        var total = eur + fee;

        var str = total.ToString();
        Console.WriteLine(str);

        str = total.ToMoney().ToString("R", CultureInfo.InvariantCulture);
        Console.WriteLine(str);
    }

    public static void Money_Test()
    {
        var price = new Money(12.99m, CurrencyCode.USD);
        var tax = price * 0.21m;
        var total = price + tax;

        var text = total.ToString("C", new CultureInfo("en-US"));
        Console.WriteLine(text);
        // $15.72

        // Split without losing cents
        var shares = (total + 0.01m).Split(3);
        // [$5.24, $5.24, $5.25]
    }

    public static void Parse_Test()
    {
        var money = new Money(76543.21, Currency.FromCode(CurrencyCode.EUR));

        var str = money.ToString("R", CultureInfo.InvariantCulture);
        Console.WriteLine(str); // EUR 76543.21
        var euro = Money.Parse(str, CultureInfo.InvariantCulture);

        str = money.ToString(CultureInfo.InvariantCulture);
        Console.WriteLine(str); // €76,543.21
        euro = Money.Parse(str, CultureInfo.InvariantCulture);
    }

    #endregion

}
