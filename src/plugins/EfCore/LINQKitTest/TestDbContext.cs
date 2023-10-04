using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LINQKitTest;

internal abstract class TestDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }
}

internal class MsSqlDbContext : TestDbContext
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer(@"Server=.\sql2017;Initial Catalog=LINQKitTest;User ID=sa;Password=123456");
        //optionsBuilder.UseSqlServer(@"Server=localhost;Initial Catalog=LINQKitTest;User ID=sa;Password=qwer@1234");
    }
}

internal class PgSqlDbContext : TestDbContext
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=LINQKitTest;Username=postgres;Password=123456;");
    }
}
