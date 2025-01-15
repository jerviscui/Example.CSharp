using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EfCoreTest
{
    internal sealed class ExpressionTest : DbContextTest
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

        private sealed class ParameterClass
        {
            public long Id;
        }

        public static void Query_ParameterSql_UseClass()
        {
            using var dbContext = CreateSqliteMemoryDbContext();

            var anonymousClass = new ParameterClass { Id = 1 };

            Expression<Func<Person, bool>> CreateExpression()
            {
                var para = Expression.Parameter(typeof(Person), "o");

                var propInfo = para.Type.GetProperty("Id")!;
                var memberExpression = Expression.Property(para, propInfo); // o.Id

                var constant = Expression.Constant(anonymousClass, typeof(ParameterClass)); // Constant(AnonymousClass)
                var fieldInfo = constant.Type.GetField("Id")!;
                var field = Expression.Field(constant, fieldInfo); // Constant(AnonymousClass).Id
                var unaryExpression =
                    Expression.Convert(field, typeof(long)); // Convert(Constant(AnonymousClass).Id, Int64)

                var binaryExpression =
                    Expression.Equal(memberExpression,
                        unaryExpression); // o.Id == Convert(Constant(AnonymousClass).Id, Int64)

                var lambdaExpression =
                    Expression.Lambda<Func<Person, bool>>(binaryExpression,
                        para); // o => o.Id == Convert(Constant(AnonymousClass).Id, Int64)

                return lambdaExpression;
            }

            var expression =
                CreateExpression(); //{o => (o.Id == Convert(value(EfCoreTest.ExpressionTest+AnonymousClass).Id, Int64))}

            var query = dbContext.Persons.Where(expression);
            var person = query.FirstOrDefault();

            //参考 Query_ParameterSql_Test IL 执行过程，可以生成参数化 Sql
            //必须将参数包装到一个类中，或者使用匿名类，才能生成参数化 Sql
            //Executed DbCommand (2ms) [Parameters=[@__Id_0='1' (DbType = String)], CommandType='Text', CommandTimeout='30']
            //SELECT "p"."Id", "p"."Decimal", "p"."FamilyId", "p"."Long", "p"."Name", "p"."TeacherId"
            //FROM "Persons" AS "p"
            //WHERE "p"."Id" = @__Id_0
            //LIMIT 1
        }

        public static void Query_ParameterSql_UseAnonymousClass()
        {
            using var dbContext = CreateSqliteMemoryDbContext();

            var anonymousClass = new { Id = 1 };

            Expression<Func<Person, bool>> CreateExpression()
            {
                var para = Expression.Parameter(typeof(Person), "o");

                var propInfo = para.Type.GetProperty("Id")!;
                var memberExpression = Expression.Property(para, propInfo); // o.Id

                var type = anonymousClass.GetType();
                var constant = Expression.Constant(anonymousClass, type); // { Id = 1 }
                var paraInfo = constant.Type.GetProperty("Id")!;
                var property = Expression.Property(constant, paraInfo); // { Id = 1 }.Id
                var unaryExpression =
                    Expression.Convert(property, typeof(long)); // Convert({ Id = 1 }.Id, Int64)

                var binaryExpression =
                    Expression.Equal(memberExpression,
                        unaryExpression); // o.Id == Convert({ Id = 1 }.Id, Int64)

                var lambdaExpression =
                    Expression.Lambda<Func<Person, bool>>(binaryExpression,
                        para); // o => o.Id == Convert({ Id = 1 }.Id, Int64)

                return lambdaExpression;
            }

            var expression = CreateExpression(); //{o => (o.Id == Convert({ Id = 1 }.Id, Int64))}

            var query = dbContext.Persons.Where(expression);
            var person = query.FirstOrDefault();

            //使用匿名类作为参数也可以生成参数化 Sql
            //Executed DbCommand (2ms) [Parameters=[@__p_0='1' (DbType = String)], CommandType='Text', CommandTimeout='30']
            //SELECT "p"."Id", "p"."Decimal", "p"."FamilyId", "p"."Long", "p"."Name", "p"."TeacherId"
            //FROM "Persons" AS "p"
            //WHERE "p"."Id" = @__p_0
            //LIMIT 1
        }

        public static void Query_Contains_NoParameterSql()
        {
            using var dbContext = CreateSqliteMemoryDbContext();

            var list = new List<long> { 1, 2, 3 };

            var query1 = dbContext.Persons.Where(o => list.Contains(o.Id));

            var person1 = query1.FirstOrDefault();

            //不会生成参数化 Sql
            //Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            //SELECT "p"."Id", "p"."Decimal", "p"."FamilyId", "p"."Long", "p"."Name", "p"."TeacherId"
            //FROM "Persons" AS "p"
            //WHERE "p"."Id" IN (1, 2, 3)
            //LIMIT 1
        }
    }
}
