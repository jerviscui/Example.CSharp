using Hangfire.Dashboard;

namespace HangfireTest.Service;

/// <summary>
/// 允许所有
/// </summary>
public class AllowAnonymousAuthorizationFilter : IDashboardAuthorizationFilter
{
    /// <inheritdoc />
    public bool Authorize(DashboardContext context) => true;
}
