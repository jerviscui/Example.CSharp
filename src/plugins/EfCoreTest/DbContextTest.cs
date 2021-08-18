using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfCoreTest
{
    internal abstract class DbContextTest
    {
        protected static DbContextOptionsBuilder<TestDbContext> CreateBuilder()
        {
            var builder = new DbContextOptionsBuilder<TestDbContext>();
            builder.UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole()));
            builder.EnableSensitiveDataLogging();
            builder.EnableDetailedErrors();

            return builder;
        }

        protected static TestDbContext CreateMsSqlDbContext()
        {
            var builder = CreateBuilder();
            //builder.UseSqlServer(@"Server=.\sql2017;Initial Catalog=EfCoreTest;User ID=sa;Password=123456");
            builder.UseSqlServer(@"Server=localhost;Initial Catalog=EfCoreTest;User ID=sa;Password=qwer@1234");

            var dbContext = new TestDbContext(builder.Options);

            return dbContext;
        }

        protected static TestDbContext CreatePostgreSqlDbContext()
        {
            var builder = CreateBuilder();
            builder.UseNpgsql(@"Host=localhost;Port=5432;Database=EfCoreTest;Username=postgres;Password=123456;");

            var dbContext = new TestDbContext(builder.Options);

            return dbContext;
        }
    }
}
