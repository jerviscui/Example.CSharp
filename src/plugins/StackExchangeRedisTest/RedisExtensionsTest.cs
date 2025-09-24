using MemoryPack;

namespace StackExchangeRedisTest;

public static class RedisExtensionsTest
{

    #region Constants & Statics

    public static async Task MaxLength_TestAsync()
    {
        var database = ExtensionDatabaseProvider.GetDatabase();
        _ = await database.AddAsync("test3", new MyClass2(1, "null", "abcdefg"));

        // System.ArgumentException: 'value cannot be longer than the MaxValueLength (Parameter 'test2')'
    }

    public static async Task MemoryPackSerializer_Null_TestAsync()
    {
        var database = ExtensionDatabaseProvider.GetDatabase();
        _ = await database.AddAsync<MyClass2>("test1", null!);

        var data = await database.GetAsync<MyClass2>("test1");
    }

    public static async Task MemoryPackSerializer_TestAsync()
    {
        var database = ExtensionDatabaseProvider.GetDatabase();
        _ = await database.AddAsync("test2", new MyClass2(1, "null", "abcdefg"));

        var data = await database.GetAsync<MyClass2>("test2");
    }

    public static async Task Tagging_TestAsync()
    {
        var database = ExtensionDatabaseProvider.GetDatabase();
        _ = await database.AddAsync("test4", new MyClass2(1, "null", "abcdefg"), tags: ["tag1"]);

        await database.RemoveByTagAsync("tag1");
    }

    #endregion

}

[MemoryPackable]
public partial class MyClass2
{
    public MyClass2(int age, string desc, string name)
    {
        Age = age;
        Desc = desc;
        Name = name;
    }

    #region Properties

    [MemoryPackOrder(0)]
    public int Age { get; set; }

    [MemoryPackOrder(1)]
    public string Desc { get; set; }

    [MemoryPackOrder(2)]
    public string Name { get; set; }

    #endregion

}
