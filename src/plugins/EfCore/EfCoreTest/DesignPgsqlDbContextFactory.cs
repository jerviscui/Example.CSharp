using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace EfCoreTest;

//PM> Add-Migration init -Context PgsqlDbContext -OutputDir Migrations\Pgsql
//PM> Script-Migration -Context PgsqlDbContext -From 20250115080136_init

public class DesignPgsqlDbContextFactory : IDesignTimeDbContextFactory<PgsqlDbContext>
{

    #region IDesignTimeDbContextFactory implementations

    public PgsqlDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        _ = optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        DbContextTest.UseNpgsql(optionsBuilder);

        return new PgsqlDbContext(optionsBuilder.Options);
    }

    #endregion

}

public class PgsqlDbContext : TestDbContext
{
    /// <inheritdoc/>
    public PgsqlDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }
}
