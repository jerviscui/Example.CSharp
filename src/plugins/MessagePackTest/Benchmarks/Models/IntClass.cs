using MemoryPack;
using MessagePack;
using Orleans;
using System.Text.Json.Serialization;

namespace MessagePackTest;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(IntClass))]
//[JsonSerializable(typeof(int))]
public sealed partial class CustomSerializerContext : JsonSerializerContext
{
}

[Serializable]
[MemoryPackable]
[MessagePackObject]
[GenerateSerializer]
[Alias("MessagePackTest.Benchmarks.Models.IntClass")]
public partial class IntClass
{

    #region Constants & Statics

    public static IntClass Create()
    {
        var result = new IntClass();
        result.Initialize();
        return result;
    }

    #endregion

    #region Properties

    [Id(0)]
    [Key(0)]
    [MemoryPackOrder(0)]
    public int MyProperty1 { get; set; }

    [Id(1)]
    [Key(1)]
    [MemoryPackOrder(1)]
    public int MyProperty2 { get; set; }

    [Id(2)]
    [Key(2)]
    [MemoryPackOrder(2)]
    public int MyProperty3 { get; set; }

    [Id(3)]
    [Key(3)]
    [MemoryPackOrder(3)]
    public int MyProperty4 { get; set; }

    [Id(4)]
    [Key(4)]
    [MemoryPackOrder(4)]
    public int MyProperty5 { get; set; }

    [Id(5)]
    [Key(5)]
    [MemoryPackOrder(5)]
    public int MyProperty6 { get; set; }

    [Id(6)]
    [Key(6)]
    [MemoryPackOrder(6)]
    public int MyProperty7 { get; set; }

    [Id(7)]
    [Key(7)]
    [MemoryPackOrder(7)]
    public int MyProperty8 { get; set; }

    [Id(8)]
    [Key(8)]
    [MemoryPackOrder(8)]
    public int MyProperty9 { get; set; }

    #endregion

    #region Methods

    public void Initialize()
    {
        MyProperty1 = MyProperty2 =
            MyProperty3 = MyProperty4 = MyProperty5 = MyProperty6 = MyProperty7 = MyProperty8 = MyProperty9 = 10;
    }

    #endregion

}
