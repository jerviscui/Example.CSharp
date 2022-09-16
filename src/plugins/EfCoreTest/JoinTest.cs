using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest
{
    internal class JoinTest : DbContextTest
    {
        public static async Task Search_InnerJoinPredicate_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            var count = await dbContext.Persons.Join(dbContext.Families.Where(o => o.Address == null),
                outer => outer.FamilyId, inner => inner.Id, (l, r) => l).CountAsync();

            //Executed DbCommand (92ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT COUNT(*)
            //FROM [Persons] AS [p]
            //INNER JOIN (
            //  SELECT [f].[Id]
            //  FROM [Families] AS [f]
            //  WHERE [f].[Address] IS NULL
            //) AS [t] ON [p].[FamilyId] = [t].[Id]
        }

        public static async Task Search_WithInnerQueryable_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            var families = dbContext.Families.Where(o => o.Address == null).Select(o => o.Id);
            var count = await dbContext.Persons.Where(o => families.Contains(o.FamilyId)).CountAsync();

            //Executed DbCommand (43ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT COUNT(*)
            //FROM [Persons] AS [p]
            //WHERE EXISTS (
            //  SELECT 1
            //  FROM [Families] AS [f]
            //  WHERE [f].[Address] IS NULL AND ([f].[Id] = [p].[FamilyId]))

            //todo: 和 Search_InnerJoinPredicate_Test 查询计划没有区别。  INNER JOIN 和 WHERE EXISTS 的区别？
        }

        public static async Task Search_InnerJoinPredicate_AsSplitQuery_NotSplit()
        {
            //AsSplitQuery() 只用于 Include 语句

            await using var dbContext = CreateMsSqlDbContext();

            var count = await dbContext.Persons.Join(dbContext.Families.Where(o => o.Address == null),
                outer => outer.FamilyId, inner => inner.Id, (l, r) => l).AsSplitQuery().CountAsync();

            //Executed DbCommand (92ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT COUNT(*)
            //FROM [Persons] AS [p]
            //INNER JOIN (
            //  SELECT [f].[Id]
            //  FROM [Families] AS [f]
            //  WHERE [f].[Address] IS NULL
            //) AS [t] ON [p].[FamilyId] = [t].[Id]
        }

        public static async Task Search_WithInnerCollectionResult_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            var families = await dbContext.Families.Where(o => o.Address == null).Select(o => o.Id).ToListAsync();
            var count = await dbContext.Persons.Where(o => families.Contains(o.FamilyId)).CountAsync();

            //Executed DbCommand (41ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT [f].[Id]
            //FROM [Families] AS [f]
            //WHERE [f].[Address] IS NULL
            //
            //Executed DbCommand (8ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT COUNT(*)
            //FROM [Persons] AS [p]
            //WHERE [p].[FamilyId] = CAST(2 AS bigint)
        }

        public static async Task SelfJoin_InnerJoin_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            var id = 1;
            var dic = await dbContext.Families.Where(o => o.Id == id)
                .Join(dbContext.Families, outer => outer.OldFamilyId, inner => inner.Id,
                    (l, r) => new { l.Id, l.Address, l.OldFamilyId, OldFamilyAddress = r.Address })
                .ToDictionaryAsync(o => o.Id);

            //Executed DbCommand (32ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT [f].[Id], [f].[Address], [f].[OldFamilyId], [f0].[Address] AS [OldFamilyAddress]
            //FROM [Families] AS [f]
            //INNER JOIN [Families] AS [f0] ON [f].[OldFamilyId] = [f0].[Id]
            //WHERE [f].[Id] = CAST(1 AS bigint)
        }

        public static async Task SelfJoin_LeftJoin_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            var id = 1;
            var dic = await dbContext.Families.Where(o => o.Id == id)
                .GroupJoin(dbContext.Families, outer => outer.OldFamilyId, inner => inner.Id,
                    (l, r) => new { l.Id, l.Address, r })
                .SelectMany(arg => arg.r.DefaultIfEmpty(),
                    (arg, oldFamily) => new
                    {
                        arg.Id,
                        arg.Address,
                        OldFamilyId = oldFamily == null ? (long?)null : oldFamily.Id,
                        OldFamilyAddress = oldFamily == null ? (string?)null : oldFamily.Address
                    })
                .ToDictionaryAsync(o => o.Id);

            var queryable =
                from left in dbContext.Families.Where(o => o.Id == id)
                join right in dbContext.Families on left.OldFamilyId equals right.Id
                    into j
                from r in j.DefaultIfEmpty()
                select new
                {
                    left.Id,
                    left.Address,
                    OldFamilyId = r == null ? (long?)null : r.Id,
                    OldFamilyAddress = r == null ? (string?)null : r.Address
                };
            var dictionary = queryable.ToDictionary(o => o.Id);

            //Executed DbCommand (21ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT [f].[Id], [f].[Address], [f0].[Id] AS [OldFamilyId], [f0].[Address] AS [OldFamilyAddress]
            //FROM [Families] AS [f]
            //LEFT JOIN [Families] AS [f0] ON [f].[OldFamilyId] = [f0].[Id]
            //WHERE [f].[Id] = CAST(1 AS bigint)
        }
    }
}
