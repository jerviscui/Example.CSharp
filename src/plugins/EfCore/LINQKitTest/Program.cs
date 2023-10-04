using LINQKitTest;

using var dbContext = new MsSqlDbContext();
dbContext.Database.EnsureCreated();

//var linqTest = new SystemLinqTest(dbContext);
//linqTest.Queryable_Default_Test();
//linqTest.Queryable_Index_Test();
//linqTest.Enumerable_Func_Test();

//var filterTest = new NavigationFilterTest(dbContext);
//filterTest.Navigation_Func_Test();
//filterTest.Navigation_Expression_Failed_Test();
//filterTest.Navigation_Expression_AsExpandable_Test();
//filterTest.Navigation_Expression_Expand_Test();
//filterTest.Subqueries_Expression_Test();
//filterTest.Subqueries_Expression_AsExpandable_Test();

var combiningTest = new CombiningTest(dbContext);
//combiningTest.Combine_Test();
combiningTest.PredicateBuilder_Test();
