namespace ConsolePollyTest;

internal static class Program
{

    #region Constants & Statics

    private static async Task Main(string[] args)
    {
        //await PollyRetryTest.NoStrategy_TestAsync();
        //await PollyRetryTest.Retry_ExecThree_TestAsync();
        //await PollyRetryTest.Retry_UseOnePipeline_TestAsync();

        await PollyCircuitTest.Circuit_3Per2sec_TestAsync();

        _ = Console.ReadKey();
    }

    #endregion

}
