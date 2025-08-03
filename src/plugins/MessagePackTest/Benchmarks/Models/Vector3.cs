using MemoryPack;
using MessagePack;
using Orleans;

namespace MessagePackTest;

[MessagePackObject]
[MemoryPackable]
[GenerateSerializer]
[Alias("MessagePackTest.MyVector3")]
public partial struct MyVector3
{
    [Key(0)]
    [Id(0)]
    [MemoryPackOrder(0)]
    public float X;

    [Key(1)]
    [Id(1)]
    [MemoryPackOrder(1)]
    public float Y;

    [Key(2)]
    [Id(2)]
    [MemoryPackOrder(2)]
    public float Z;
}

[Immutable]
[MessagePackObject]
[MemoryPackable]
[GenerateSerializer]
[Alias("MessagePackTest.ImmutableVector3")]
public partial struct ImmutableVector3
{
    [Key(0)]
    [Id(0)]
    [MemoryPackOrder(0)]
    public float X;

    [Key(1)]
    [Id(1)]
    [MemoryPackOrder(1)]
    public float Y;

    [Key(2)]
    [Id(2)]
    [MemoryPackOrder(2)]
    public float Z;
}
