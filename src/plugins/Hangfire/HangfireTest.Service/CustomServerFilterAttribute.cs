using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;

namespace HangfireTest.Service;

public class CustomServerFilterAttribute : JobFilterAttribute, IServerFilter
{

    #region Constants & Statics

    //尽量少用 LogProvider
    private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

    #endregion

    #region IServerFilter implementations

    public void OnPerformed(PerformedContext context)
    {
        Logger.InfoFormat("Job `{0}` has been performed", context.BackgroundJob.Id);
        Logger.InfoFormat(
            $"{context.BackgroundJob.Job.Method.DeclaringType}.{context.BackgroundJob.Job.Method.Name} @{context.ServerId} completed.");
    }

    public void OnPerforming(PerformingContext context)
    {
        Logger.InfoFormat("Starting to perform job `{0}`", context.BackgroundJob.Id);
    }

    #endregion

}
