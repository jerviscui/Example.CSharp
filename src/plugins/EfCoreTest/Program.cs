using System.Threading.Tasks;

namespace EfCoreTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //EntityMappingTest.OnDelete_MsSql_Test();
            //EntityMappingTest.OnDelete_PostgreSql_Test();
            //EntityMappingTest.OnDelete_SqliteMemory_Test();

            //await SearchTest.ProtectedProp_Test();

            //await LinqTranslationTest.ListContains_Predicate_Test();
            //await LinqTranslationTest.ListAny_Predicate_ThrowInvalidOperationException_Test();
            //await LinqTranslationTest.ListAny_Predicate_Test();

            //await JoinTest.SelfJoin_LeftJoin_Test();

            //await OwnedTypeTest.Search_QueryByOwnedType_Test();

            //ExpressionTest.Query_ParameterSql_Test();
            //ExpressionTest.Query_Sql_Test();
            //ExpressionTest.Query_ParameterSql_UseAnonymousClass();
            ExpressionTest.Query_Contains_NoParameterSql();

            //DynamicLinqTest.Query_NoArg_GenerateParameterSql();
            //DynamicLinqTest.Query_WithArg_GenerateParameterSql();
            DynamicLinqTest.Query_Contains_WithArg_NoParameterSql();

            OnExit();
        }

        private static void OnExit()
        {
            DbContextTest.SqliteConnection?.Dispose();
        }
    }
}
