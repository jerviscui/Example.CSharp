namespace EfCoreTest;

internal class PagingTest : DbContextTest
{
    public static void Test()
    {
        var dbContext = CreatePostgreSqlDbContext();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }

    public static void MsSql_Test()
    {
        var dbContext = CreateMsSqlDbContext();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
}
