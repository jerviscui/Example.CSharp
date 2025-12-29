namespace NodaMoneyTest;

internal static class Program
{

    #region Constants & Statics

    private static void Main()
    {
        //MoneyTest.Money_Test();
        //MoneyTest.Parse_Test();
        //MoneyTest.FastMoney_Test();
        MoneyTest.DefaultCurrency_Test();

        SerializationTest.Serialize_Test();
    }

    #endregion

}
