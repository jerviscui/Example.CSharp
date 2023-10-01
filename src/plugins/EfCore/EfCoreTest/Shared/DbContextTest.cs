using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfCoreTest
{
    public abstract class DbContextTest
    {
        public static SqliteConnection? SqliteConnection;

        protected static DbContextOptionsBuilder<TestDbContext> CreateBuilder()
        {
            var builder = new DbContextOptionsBuilder<TestDbContext>();
            SettingBuilder(builder);

            return builder;
        }

        public static void SettingBuilder(DbContextOptionsBuilder builder)
        {
            builder.UseLoggerFactory(LoggerFactory.Create(loggingBuilder =>
                loggingBuilder.SetMinimumLevel(LogLevel.Information).AddConsole()));
            builder.EnableSensitiveDataLogging();
            builder.EnableDetailedErrors();
        }

        protected static TestDbContext CreateSqliteMemoryDbContext()
        {
            bool createDb = false;
            if (SqliteConnection is null)
            {
                SqliteConnection ??= new SqliteConnection("Filename=:memory:");
                createDb = true;
            }

            var builder = CreateBuilder();
            builder.UseSqlite(SqliteConnection);

            if (SqliteConnection.State != ConnectionState.Open)
            {
                SqliteConnection.Open();
            }
            var dbContext = new TestDbContext(builder.Options);

            if (createDb)
            {
                dbContext.Database.EnsureCreated();
            }

            return dbContext;
        }

        public static void UseSqlServer(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=.\sql2017;Initial Catalog=EfCoreTest;User ID=sa;Password=123456");
            //builder.UseSqlServer(@"Server=localhost;Initial Catalog=EfCoreTest;User ID=sa;Password=qwer@1234");
        }

        protected static TestDbContext CreateMsSqlDbContext()
        {
            var builder = CreateBuilder();
            UseSqlServer(builder);

            var dbContext = new TestDbContext(builder.Options);

            return dbContext;
        }

        public static void UseNpgsql(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(@"Host=localhost;Port=5432;Database=EfCoreTest;Username=postgres;Password=123456;");
        }

        protected static TestDbContext CreatePostgreSqlDbContext()
        {
            var builder = CreateBuilder();
            UseNpgsql(builder);

            var dbContext = new TestDbContext(builder.Options);

            return dbContext;
        }

        protected static void AddIfNotExists<T>(TestDbContext dbContext, T entity) where T : Entity
        {
            var dbSet = dbContext.Set<T>();
            if (!dbSet.Any(arg => EF.Property<long>(arg, "Id") == entity.Id))
            {
                dbSet.Add(entity);
            }
        }
    }
}
