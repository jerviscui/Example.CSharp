using BenchmarkDotNet.Attributes;
using MemoryPack;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Serialization;
using Orleans.Serialization.Buffers;
using Orleans.Serialization.Session;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace MessagePackTest;

/// <summary>
/// Compares Orleans deserialization performance against other popular serializers for class types.
/// </summary>
[Config(typeof(BenchmarkConfig))]
[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
public class StructDeserializeBenchmark
{

    #region Constants & Statics

    private static readonly IntStruct _value = IntStruct.Create();

    private static readonly Serializer<IntStruct> OrleansSerializer;
    private static readonly SerializerSession Session;
    private static readonly byte[] _orleansPayload;

    private static readonly byte[] MsgPackInput = MessagePackSerializer.Serialize(_value);
    private static readonly byte[] _memoryPackPayload = MemoryPackSerializer.Serialize(_value);
    private static readonly byte[] _stjPayload = JsonSerializer.SerializeToUtf8Bytes(_value);

    static StructDeserializeBenchmark()
    {
        var services = new ServiceCollection()
            .AddSerializer()
            .BuildServiceProvider();
        OrleansSerializer = services.GetRequiredService<Serializer<IntStruct>>();
        Session = services.GetRequiredService<SerializerSessionPool>().GetSession();

        _orleansPayload = OrleansSerializer.SerializeToArray(_value);
    }

    private static int SumResult(IntStruct result)
    {
        return result.MyProperty1 + result.MyProperty2 + result.MyProperty3 + result.MyProperty4 + result.MyProperty5
               + result.MyProperty6 + result.MyProperty7 + result.MyProperty8 + result.MyProperty9;
    }

    #endregion

    [Benchmark(Baseline = true)]
    public int MemoryPack()
    {
        return SumResult(MemoryPackSerializer.Deserialize<IntStruct>(_memoryPackPayload)!);
    }

    [Benchmark]
    public int MessagePackCSharp()
    {
        return SumResult(MessagePackSerializer.Deserialize<IntStruct>(MsgPackInput, cancellationToken: default));
    }

    [Benchmark]
    public int Orleans()
    {
        Session.Reset();
        var instance = OrleansSerializer.Deserialize(_orleansPayload, Session);
        return SumResult(instance);
    }

    [Benchmark]
    public int OrleansReader()
    {
        Session.Reset();
        var reader = Reader.Create(_orleansPayload, Session);
        var instance = OrleansSerializer.Deserialize(ref reader);
        return SumResult(instance);
    }

    [Benchmark]
    public int SystemTextJson()
    {
        return SumResult(JsonSerializer.Deserialize<IntStruct>(_stjPayload)!);
    }
}
