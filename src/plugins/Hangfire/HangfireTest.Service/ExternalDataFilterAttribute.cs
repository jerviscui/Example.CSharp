using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace HangfireTest.Service;

public class ExternalDataFilterAttribute : JobFilterAttribute, IClientFilter, IServerFilter
{
    private readonly ILogger<ExternalDataFilterAttribute> _logger;

    public ExternalDataFilterAttribute(ILogger<ExternalDataFilterAttribute> logger)
    {
        _logger = logger;
    }

    #region IClientFilter implementations

    public void OnCreated(CreatedContext context)
    {
        _logger.LogInformation(
            "Job that is based on method `{0}` has been created with id `{1}`",
            context.Job.Method.Name,
            context.BackgroundJob?.Id);
    }

    public void OnCreating(CreatingContext context)
    {
        _logger.LogInformation("Creating a job based on method `{0}`...", context.Job.Method.Name);

        context.SetJobParameter("externalData", DateTime.Now.ToString("O"));
    }

    #endregion

    #region IServerFilter implementations

    public void OnPerformed(PerformedContext context)
    {
    }

    public void OnPerforming(PerformingContext context)
    {
        var time = context.GetJobParameter<DateTime>("externalData");
        _logger.LogInformation("get externalData from context, {0}", time.ToString("O"));
    }

    #endregion

}
