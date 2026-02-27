using NodaMoney;
using NodaMoney.Context;
using System.Globalization;

namespace NodaMoneyTest;

public static class MoneyTest
{

    #region Constants & Statics

    public static void DefaultCurrency_Test()
    {
        _ = MoneyContext.CreateAndSetDefault(
            (options) =>
            {
                options.MaxScale = 4;
                options.DefaultCurrency = CurrencyInfo.FromCode(CurrencyCode.CNY);
            });

        // or
        //var myDefaultContext = MoneyContext.Create(
        //    opt =>
        //    {
        //        opt.MaxScale = 4;
        //        opt.DefaultCurrency = CurrencyInfo.FromCode(CurrencyCode.CNY);
        //        opt.EnforceZeroCurrencyMatching = true;
        //    });
        //MoneyContext.DefaultThreadContext = myDefaultContext;

        var money = new Money(1001.12346m); // Amount = 1001.1235
        Console.WriteLine(money); // ¥1,001.12
        Console.WriteLine(money.ToString("c", CultureInfo.CurrentCulture)); // ¥1K
        Console.WriteLine(money.ToString("C4", CultureInfo.CurrentCulture)); // ¥1,001.1235
    }

    public static void FastMoney_Test()
    {
        var eur = new FastMoney(10.12645m, CurrencyCode.EUR);
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
        _ = (total + 0.01m).Split(3);
        // [$5.24, $5.24, $5.25]
    }

    public static void Parse_Test()
    {
        var money = new Money(76543.21, Currency.FromCode(CurrencyCode.EUR));

        var str = money.ToString("R", CultureInfo.InvariantCulture);
        Console.WriteLine(str); // EUR 76543.21
        _ = Money.Parse(str, CultureInfo.InvariantCulture);

        str = money.ToString(CultureInfo.InvariantCulture);
        Console.WriteLine(str); // €76,543.21
        _ = Money.Parse(str, CultureInfo.InvariantCulture);
    }

    #endregion

}
