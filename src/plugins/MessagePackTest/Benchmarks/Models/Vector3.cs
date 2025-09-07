using MemoryPack;
using MessagePack;
using Orleans;

namespace MessagePackTest;

[MessagePackObject]
[MemoryPackable]
[GenerateSerializer]
[Alias("MessagePackTest.MyVector3")]
public readonly partial struct MyVector3
{
    [Key(0)]
    [Id(0)]
    [MemoryPackOrder(0)]
    public readonly float X;

    [Key(1)]
    [Id(1)]
    [MemoryPackOrder(1)]
    public readonly float Y;

    [Key(2)]
    [Id(2)]
    [MemoryPackOrder(2)]
    public readonly float Z;

    public MyVector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}

[Immutable]
[MessagePackObject]
[MemoryPackable]
[GenerateSerializer]
[Alias("MessagePackTest.ImmutableVector3")]
public readonly partial struct ImmutableVector3
{
    [Key(0)]
    [Id(0)]
    [MemoryPackOrder(0)]
    public readonly float X;

    [Key(1)]
    [Id(1)]
    [MemoryPackOrder(1)]
    public readonly float Y;

    [Key(2)]
    [Id(2)]
    [MemoryPackOrder(2)]
    public readonly float Z;

    public ImmutableVector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}
