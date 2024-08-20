using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace ExceptionHelperTest;

public class ExceptionClass
{

    #region Constants & Statics

    public static void ExceptionMethod(string? i = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(i);
    }

    public static Task ExceptionMethodAsync(string? i = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentException.ThrowIfNullOrEmpty(i);

        return Task.CompletedTask;
    }

    #endregion

    #region Methods

    [DoesNotReturn]
    private void InnerThrow()
    {
        throw new NotImplementedException("Exception test.");
    }

    public void CaptureTest()
    {
        Exception? exception = null;

        try
        {
            InnerThrow();
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        ExceptionDispatchInfo.Capture(exception).Throw();
    }

    public void ReThrowTest()
    {
        try
        {
            InnerThrow();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
#pragma warning disable CA2200 // Rethrow to preserve stack details
            throw ex; // 堆栈跟踪将在当前方法处重新启动，并且引发异常的原始方法与当前方法之间的方法调用列表将丢失。
#pragma warning restore CA2200 // Rethrow to preserve stack details
        }
    }

    public void ThrowTest()
    {
        try
        {
            InnerThrow();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    #endregion

}
