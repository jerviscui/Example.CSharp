using ExceptionHelperTest;

internal static class Program
{

    #region Constants & Statics

    private static void Main()
    {
        // ExceptionClass.ExceptionMethod();
        // output:
        // Unhandled exception. System.ArgumentNullException: Value cannot be null. (Parameter 'i')
        // at System.ArgumentNullException.Throw(String paramName)
        // at System.ArgumentNullException.ThrowIfNull(Object argument, String paramName)
        // at System.ArgumentException.ThrowNullOrEmptyException(String argument, String paramName)
        // at System.ArgumentException.ThrowIfNullOrEmpty(String argument, String paramName)
        // at ExceptionHelper.ExceptionClass.ExceptionMethod(String i) in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelper\ExceptionClass.cs:line 10
        // at Program.<Main>$(String[] args) in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelper\Program.cs:line 7
        // at Program.<Main>(String[] args)

        // await ExceptionClass.ExceptionMethodAsync();
        // output:
        // Unhandled exception. System.ArgumentNullException: Value cannot be null. (Parameter 'i')
        // at System.ArgumentNullException.Throw(String paramName)
        // at System.ArgumentNullException.ThrowIfNull(Object argument, String paramName)
        // at System.ArgumentException.ThrowNullOrEmptyException(String argument, String paramName)
        // at System.ArgumentException.ThrowIfNullOrEmpty(String argument, String paramName)
        // at ExceptionHelper.ExceptionClass.ExceptionMethodAsync(String i, CancellationToken cancellationToken) in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelper\ExceptionClass.cs:line 19
        // at Program.<Main>$(String[] args) in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelper\Program.cs:line 19
        // at Program.<Main>(String[] args)

        var exceptionClass = new ExceptionClass();

        // exceptionClass.ThrowTest();
        // output:
        // Unhandled exception. System.NotImplementedException: Exception test.
        // at ExceptionHelperTest.ExceptionClass.InnerThrow() in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelperTest\ExceptionClass.cs:line 32
        // at ExceptionHelperTest.ExceptionClass.ThrowTest() in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelperTest\ExceptionClass.cs:line 58
        // at Program.<Main>$(String[] args) in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelperTest\Program.cs:line 27

        // exceptionClass.ReThrowTest();
        // output:
        // Unhandled exception. System.NotImplementedException: Exception test.
        // at ExceptionHelperTest.ExceptionClass.ReThrowTest() in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelperTest\ExceptionClass.cs:line 61
        // at Program.Main() in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelperTest\Program.cs:line 4

        exceptionClass.CaptureTest();
        // output:
        // Unhandled exception. System.NotImplementedException: Exception test.
        // at ExceptionHelperTest.ExceptionClass.InnerThrow() in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelperTest\ExceptionClass.cs:line 32
        // at ExceptionHelperTest.ExceptionClass.CaptureTest() in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelperTest\ExceptionClass.cs:line 41
        // --- End of stack trace from previous location ---
        // at ExceptionHelperTest.ExceptionClass.CaptureTest() in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelperTest\ExceptionClass.cs:line 48
        // at Program.<Main>$(String[] args) in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelperTest\Program.cs:line 41
    }

    #endregion

}
