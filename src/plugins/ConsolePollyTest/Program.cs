namespace ConsolePollyTest;

internal static class Program
{

    #region Constants & Statics

    private static async Task Main(string[] args)
    {
        //await PollyTest.NoStrategy_TestAsync();

        await PollyTest.Retry_ExecThree_TestAsync();

        //await PollyTest.Retry_UseOnePipeline_TestAsync();

        _ = Console.ReadKey();
    }

    #endregion

}
