using ExceptionHelperTest;

#pragma warning disable S6966 // Awaitable method should be used
ExceptionClass.ExceptionMethod();
#pragma warning restore S6966 // Awaitable method should be used
// output:
// Unhandled exception. System.ArgumentNullException: Value cannot be null. (Parameter 'i')
// at System.ArgumentNullException.Throw(String paramName)
// at System.ArgumentNullException.ThrowIfNull(Object argument, String paramName)
// at System.ArgumentException.ThrowNullOrEmptyException(String argument, String paramName)
// at System.ArgumentException.ThrowIfNullOrEmpty(String argument, String paramName)
// at ExceptionHelper.ExceptionClass.ExceptionMethod(String i) in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelper\ExceptionClass.cs:line 10
// at Program.<Main>$(String[] args) in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelper\Program.cs:line 7
// at Program.<Main>(String[] args)

await ExceptionClass.ExceptionMethodAsync();
// output:
// Unhandled exception. System.ArgumentNullException: Value cannot be null. (Parameter 'i')
// at System.ArgumentNullException.Throw(String paramName)
// at System.ArgumentNullException.ThrowIfNull(Object argument, String paramName)
// at System.ArgumentException.ThrowNullOrEmptyException(String argument, String paramName)
// at System.ArgumentException.ThrowIfNullOrEmpty(String argument, String paramName)
// at ExceptionHelper.ExceptionClass.ExceptionMethodAsync(String i, CancellationToken cancellationToken) in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelper\ExceptionClass.cs:line 19
// at Program.<Main>$(String[] args) in D:\Project\GitHub\jerviscui\Example.CSharp\src\11\ExceptionHelper\Program.cs:line 19
// at Program.<Main>(String[] args)
