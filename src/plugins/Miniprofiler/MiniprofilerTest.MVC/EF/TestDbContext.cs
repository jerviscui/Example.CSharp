using Microsoft.EntityFrameworkCore;

namespace MiniprofilerTest.MVC;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<MyClass> MyClasses { get; set; } = null!;
}
