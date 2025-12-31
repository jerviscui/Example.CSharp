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

        //SerializationTest.Serialize_Test();
        SerializationTest.Output_Serialization_Test();
        SerializationTest.Input_Validation_ModelState_Test();
    }

    #endregion

}
