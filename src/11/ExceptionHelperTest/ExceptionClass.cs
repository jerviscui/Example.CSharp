namespace ExceptionHelperTest;

public static class ExceptionClass
{

    #region Constants & Statics

    public static void ExceptionMethod(string? i = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(i);
    }

#pragma warning disable CRR0038 // CancellationToken parameter is never used.
#pragma warning disable IDE0060 // Remove unused parameter
    public static Task ExceptionMethodAsync(string? i = null, CancellationToken cancellationToken = default)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CRR0038 // CancellationToken parameter is never used.
    {
        ArgumentException.ThrowIfNullOrEmpty(i);

        return Task.CompletedTask;
    }

    #endregion

}
