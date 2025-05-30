using Microsoft.EntityFrameworkCore;

namespace EfCoreTest;

internal static class Program
{

    #region Constants & Statics

    private static async Task Main(string[] args)
    {
        using var dbContext = new DesignPgsqlDbContextFactory().CreateDbContext([]);
        _ = await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();

        //EntityMappingTest.OnDelete_MsSql_Test();
        //EntityMappingTest.OnDelete_PostgreSql_Test();
        //EntityMappingTest.OnDelete_SqliteMemory_Test();

        //await EntityMappingTest.ManyToMany_Query_Test();
        //await EntityMappingTest.ManyToMany_Query_AsSplitQuery_Test();
        //await EntityMappingTest.ManyToMany_Insert_Test();

        //await RowVersionTest.MssqlRowVersion_Update_Test();
        await RowVersionTest.PgsqlRowVersion_Update_Test();

        //await SearchTest.ProtectedProp_Test();
        //await SearchTest.CompileQuery_Test();
        //await SearchTest.CompileQuery_Include_Test();
        await SearchTest.Exists_Test();
        //await SearchTest.CompileQuery_WithSelect_Test();
        //await SearchTest.StreamingQuery_Test();

        //await LinqTranslationTest.ListContains_Predicate_Test();
        //await LinqTranslationTest.ListAny_Predicate_ThrowInvalidOperationException_Test();
        //await LinqTranslationTest.ListAny_Predicate_Test();

        //await JoinTest.SelfJoin_LeftJoin_Test();

        //await OwnedTypeTest.Search_QueryByOwnedType_Test();

        //ExpressionTest.Query_Sql_Test();
        //ExpressionTest.Query_ParameterSql_Test();
        //ExpressionTest.Query_ParameterSql_UseClass();
        //ExpressionTest.Query_ParameterSql_UseAnonymousClass();
        //ExpressionTest.Query_Contains_NoParameterSql();

        //DynamicLinqTest.Query_NoArg_GenerateParameterSql();
        //DynamicLinqTest.Query_WithArg_GenerateParameterSql();
        //DynamicLinqTest.Query_Contains_WithArg_NoParameterSql();

        //await DeleteTest.DeleteItems_FromPrimaryTable_OneToMany();

        //await ExceptionTest.DbUpdateException_Retry_Test();

        //TableSplittingTests.TableSplitting_Sqlite_Test();
        //TableSplittingTests.TableSplitting_MsSql_Test();
        //await TableSplittingTests.Insert_Test();
        //await TableSplittingTests.Search_SplitOrder_Test();
        //await TableSplittingTests.Search_DetailedSplitOrder_Test();

        //await PagingTest.MsSql_Test();
        //await PagingTest.PostgreSql_Test();
        //await PagingTest.PostgreSql_NoOffset_Test();
        //await PagingTest.MsSql_NoOffset_Test();
        //await PagingTest.PostgreSql_Opetimization_Test();

        OnExit();
    }

    private static void OnExit()
    {
        DbContextTest.SqliteConnection?.Dispose();
    }

    #endregion

}
