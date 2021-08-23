namespace EfCoreTest
{
    internal class EntityMappingTest : DbContextTest
    {
        public static void OnDelete_SqliteMemory_Test()
        {
            using var dbContext = CreateSqliteMemoryDbContext();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        public static void OnDelete_MsSql_Test()
        {
            using var dbContext = CreateMsSqlDbContext();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        public static void OnDelete_PostgreSql_Test()
        {
            using var dbContext = CreatePostgreSqlDbContext();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }
}
