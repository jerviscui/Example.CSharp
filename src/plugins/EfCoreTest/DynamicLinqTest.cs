using System.Linq;
using System.Linq.Dynamic.Core;

namespace EfCoreTest
{
    internal class DynamicLinqTest : DbContextTest
    {
        public static void Query_NoArg_GenerateParameterSql()
        {
            using var dbContext = CreateSqliteMemoryDbContext();

            var config = ParsingConfig.Default;
            config.UseParameterizedNamesInDynamicQuery = true;

            var query = dbContext.Persons.Where(config, "Id == 1");

            var person = query.FirstOrDefault();

            //UseParameterizedNamesInDynamicQuery 可以生成参数化 Sql
            //Executed DbCommand (2ms) [Parameters=[@__p_0='1' (DbType = String)], CommandType='Text', CommandTimeout='30']
            //SELECT "p"."Id", "p"."Decimal", "p"."FamilyId", "p"."Long", "p"."Name", "p"."TeacherId"
            //FROM "Persons" AS "p"
            //WHERE "p"."Id" = @__p_0
            //LIMIT 1
        }

        public static void Query_WithArg_GenerateParameterSql()
        {
            using var dbContext = CreateSqliteMemoryDbContext();

            var config = ParsingConfig.Default;
            config.UseParameterizedNamesInDynamicQuery = true;

            var query1 = dbContext.Persons.Where(config, "Id == @0", 1);
            //var query2 = dbContext.Persons.Where(config, "o => o.Id == @0", 1);

            var person = query1.FirstOrDefault();

            //query1、query2 都会翻译成下面 Sql
            //Executed DbCommand (2ms) [Parameters=[@__p_0='1' (DbType = String)], CommandType='Text', CommandTimeout='30']
            //SELECT "p"."Id", "p"."Decimal", "p"."FamilyId", "p"."Long", "p"."Name", "p"."TeacherId"
            //FROM "Persons" AS "p"
            //WHERE "p"."Id" = @__p_0
            //LIMIT 1
        }
    }
}
