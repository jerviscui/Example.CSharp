using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest
{
    internal sealed class EntityMappingTest : DbContextTest
    {
        public static void OnDelete_SqliteMemory_Test()
        {
            using var dbContext = CreateSqliteMemoryDbContext();
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

        public static async Task ManyToMany_Query_Test()
        {
            await using var dbContext = CreateSqliteMemoryDbContext();

            var list = await dbContext.Posts.Include(o => o.Tags).ToListAsync();

            //Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT "p"."PostId", "p"."Content", "p"."Title", "t0"."PostId", "t0"."TagId", "t0"."TagId0"
            //FROM "Posts" AS "p"
            //LEFT JOIN (
            //  SELECT "p0"."PostId", "p0"."TagId", "t"."TagId" AS "TagId0"
            //  FROM "PostTag" AS "p0"
            //  INNER JOIN "Tags" AS "t" ON "p0"."TagId" = "t"."TagId"
            //) AS "t0" ON "p"."PostId" = "t0"."PostId"
            //ORDER BY "p"."PostId", "t0"."PostId", "t0"."TagId", "t0"."TagId0"
        }

        public static async Task ManyToMany_Query_AsSplitQuery_Test()
        {
            await using var dbContext = CreateSqliteMemoryDbContext();

            var list = await dbContext.Posts.Include(o => o.Tags).AsSplitQuery().ToListAsync();

            //todo: last sql is redundant, issue:https://github.com/dotnet/efcore/issues/25767
            //Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT "p"."PostId", "p"."Content", "p"."Title"
            //FROM "Posts" AS "p"
            //ORDER BY "p"."PostId"
            //Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT "t0"."PostId", "t0"."TagId", "t0"."TagId0", "p"."PostId"
            //FROM "Posts" AS "p"
            //INNER JOIN (
            //  SELECT "p0"."PostId", "p0"."TagId", "t"."TagId" AS "TagId0"
            //  FROM "PostTag" AS "p0"
            //  INNER JOIN "Tags" AS "t" ON "p0"."TagId" = "t"."TagId"
            //) AS "t0" ON "p"."PostId" = "t0"."PostId"
            //ORDER BY "p"."PostId"
        }

        public static async Task ManyToMany_Insert_Test()
        {
            await using var dbContext = CreateSqliteMemoryDbContext();

            var post = await dbContext.Posts.FirstAsync();

            post.AddTag(new Tag { TagId = "new tag" });

            await dbContext.SaveChangesAsync();

            //Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT "p"."PostId", "p"."Content", "p"."Title"
            //FROM "Posts" AS "p"
            //LIMIT 1
            //Executed DbCommand (2ms) [Parameters=[@p0='new tag' (Nullable = false) (Size = 7)], CommandType='Text', CommandTimeout='30']
            //INSERT INTO "Tags" ("TagId")
            //VALUES (@p0);
            //Executed DbCommand (0ms) [Parameters=[@p1='1' (DbType = String), @p2='new tag' (Nullable = false) (Size = 7)], CommandType='Text', CommandTimeout='30']
            //INSERT INTO "PostTag" ("PostId", "TagId")
            //VALUES (@p1, @p2);
        }
    }
}
