namespace NodaMoneyTest;

internal static class Program
{

    #region Constants & Statics

    private static void Main()
    {
        MoneyTest.DefaultCurrency_Test();
        //MoneyTest.Money_Test();
        //MoneyTest.Parse_Test();
        //MoneyTest.FastMoney_Test();

        SerializationTest.Serialize_Test();
    }

    #endregion

}
