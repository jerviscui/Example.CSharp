using Microsoft.Extensions.DependencyInjection;

namespace EfCorePoolTest;

internal static class DependencyInjection
{

    #region Constants & Statics

    internal static ServiceProvider ServiceProvider { get; set; } = null!;

    #endregion

}
