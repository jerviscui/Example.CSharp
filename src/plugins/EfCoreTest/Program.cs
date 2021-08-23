using System.Threading.Tasks;

namespace EfCoreTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //EntityMappingTest.OnDelete_MsSql_Test();
            //EntityMappingTest.OnDelete_PostgreSql_Test();
            EntityMappingTest.OnDelete_SqliteMemory_Test();

            //await SearchTest.ProtectedProp_Test();

            //await LinqTranslationTest.ListContains_Predicate_Test();
            //await LinqTranslationTest.ListAny_Predicate_ThrowInvalidOperationException_Test();
            //await LinqTranslationTest.ListAny_Predicate_Test();

            //await JoinTest.SelfJoin_LeftJoin_Test();

            //await OwnedTypeTest.Search_QueryByOwnedType_Test();
        }
    }
}
