using MemoryPack;

namespace Orleans.Serialization.MemoryPack;

[MemoryPackable]
public sealed partial record MyMessagePackClass
{

    #region Properties

    [MemoryPackOrder(0)]
    public int IntProperty { get; set; }

    [MemoryPackOrder(1)]
    public required string StringProperty { get; set; }

    [MemoryPackOrder(2)]
    public required MyMessagePackSubClass SubClass { get; set; }

    [MemoryPackOrder(3)]
    public required IMyMessagePackUnion Union { get; set; }

    #endregion

}

[MemoryPackable]
public sealed partial record MyMessagePackSubClass
{

    #region Properties

    [MemoryPackOrder(0)]
    public Guid Id { get; set; }

    #endregion

}

[MemoryPackable]
[MemoryPackUnion(0, typeof(MyMessagePackUnionVariant1))]
[MemoryPackUnion(1, typeof(MyMessagePackUnionVariant2))]
public partial interface IMyMessagePackUnion
{
}

[MemoryPackable]
public sealed partial record MyMessagePackUnionVariant1 : IMyMessagePackUnion
{

    #region Properties

    [MemoryPackOrder(0)]
    public int IntProperty { get; set; }

    #endregion

}

[MemoryPackable]
public sealed partial record MyMessagePackUnionVariant2 : IMyMessagePackUnion
{

    #region Properties

    [MemoryPackOrder(0)]
    public required string StringProperty { get; set; }

    #endregion

}
