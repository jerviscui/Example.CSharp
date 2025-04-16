namespace WinFormsAppTest;

internal static class Program
{

    #region Constants & Statics

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        using var mainForm = new Form1();
        Application.Run(mainForm);
    }

    #endregion

}