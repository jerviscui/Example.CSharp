using Hangfire.Server;

namespace HangfireTest.WebServer;

internal sealed class CustomBackgroundJobPerformer : IBackgroundJobPerformer
{
    private readonly IBackgroundJobPerformer _inner;

    public CustomBackgroundJobPerformer(IBackgroundJobPerformer inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    #region IBackgroundJobPerformer implementations

    public object Perform(PerformContext context)
    {
        Console.WriteLine(
            $"Perform {context.BackgroundJob.Id} ({context.BackgroundJob.Job.Type.FullName}.{context.BackgroundJob.Job.Method.Name})");
        return _inner.Perform(context);
    }

    #endregion

}
