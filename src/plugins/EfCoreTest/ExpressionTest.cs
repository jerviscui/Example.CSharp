using System;
using System.Linq;
using System.Linq.Expressions;

namespace EfCoreTest
{
    internal class ExpressionTest : DbContextTest
    {
        public static void Query_Sql_Test()
        {
            using var dbContext = CreateSqliteMemoryDbContext();

            var query = dbContext.Persons.Where(o => o.Id == 1);
            var person = query.FirstOrDefault();

            //EF Core 将常量条件写入 Sql
            //Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT "p"."Id", "p"."Decimal", "p"."FamilyId", "p"."Long", "p"."Name", "p"."TeacherId"
            //FROM "Persons" AS "p"
            //WHERE "p"."Id" = 1
            //LIMIT 1
        }

        public static void Query_ParameterSql_Test()
        {
            using var dbContext = CreateSqliteMemoryDbContext();

            var id = 1;
            var query = dbContext.Persons.Where(o => o.Id == id);
            var person = query.FirstOrDefault();

            //变量条件会被翻译成参数化 Sql
            //Executed DbCommand (2ms) [Parameters=[@__p_0='1' (DbType = String)], CommandType='Text', CommandTimeout='30']
            //SELECT "p"."Id", "p"."Decimal", "p"."FamilyId", "p"."Long", "p"."Name", "p"."TeacherId"
            //FROM "Persons" AS "p"
            //WHERE "p"."Id" = @__p_0
            //LIMIT 1
        }

        private sealed class AnonymousClass
        {
            public long Id;
        }

        public static void Query_ParameterSql_UseAnonymousClass()
        {
            using var dbContext = CreateSqliteMemoryDbContext();

            var anonymousClass = new AnonymousClass { Id = 1 };

            Expression<Func<Person, bool>> CreateExpression()
            {
                var para = Expression.Parameter(typeof(Person), "o");

                var propInfo = para.Type.GetProperty("Id")!;
                var memberExpression = Expression.Property(para, propInfo); // o.Id

                var anonymous = Expression.Constant(anonymousClass, typeof(AnonymousClass)); //Constant(AnonymousClass)
                var fieldInfo = anonymous.Type.GetField("Id")!;
                var field = Expression.Field(anonymous, fieldInfo);            //Constant(AnonymousClass).Id
                var unaryExpression = Expression.Convert(field, typeof(long)); // (long)Constant(AnonymousClass).Id

                var binaryExpression =
                    Expression.Equal(memberExpression, unaryExpression); // o.Id == (long)Constant(AnonymousClass).Id

                var lambdaExpression =
                    Expression.Lambda<Func<Person, bool>>(binaryExpression,
                        para); // o => o.Id == (long)Constant(AnonymousClass).Id

                return lambdaExpression;
            }

            var expression = CreateExpression();

            var query = dbContext.Persons.Where(expression);
            var person = query.FirstOrDefault();

            //参考参数化 IL 执行过程，可以生成参数化 Sql
            //Executed DbCommand (2ms) [Parameters=[@__Id_0='1' (DbType = String)], CommandType='Text', CommandTimeout='30']
            //SELECT "p"."Id", "p"."Decimal", "p"."FamilyId", "p"."Long", "p"."Name", "p"."TeacherId"
            //FROM "Persons" AS "p"
            //WHERE "p"."Id" = @__Id_0
            //LIMIT 1
        }
    }
}
