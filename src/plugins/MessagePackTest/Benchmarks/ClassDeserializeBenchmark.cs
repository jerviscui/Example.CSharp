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
public class ClassDeserializeBenchmark
{

    #region Constants & Statics

    private static readonly IntClass _value = IntClass.Create();

    private static readonly Serializer<IntClass> OrleansSerializer;
    private static readonly SerializerSession Session;
    private static readonly byte[] _orleansPayload;

    private static readonly byte[] MsgPackInput = MessagePackSerializer.Serialize(_value);
    private static readonly byte[] _memoryPackPayload = MemoryPackSerializer.Serialize(_value);
    private static readonly byte[] _stjPayload = JsonSerializer.SerializeToUtf8Bytes(_value);

    static ClassDeserializeBenchmark()
    {
        var services = new ServiceCollection()
            .AddSerializer()
            .BuildServiceProvider();
        OrleansSerializer = services.GetRequiredService<Serializer<IntClass>>();
        Session = services.GetRequiredService<SerializerSessionPool>().GetSession();

        _orleansPayload = OrleansSerializer.SerializeToArray(_value);
    }

    private static int SumResult(IntClass result)
    {
        return result.MyProperty1 + result.MyProperty2 + result.MyProperty3 + result.MyProperty4 + result.MyProperty5
               + result.MyProperty6 + result.MyProperty7 + result.MyProperty8 + result.MyProperty9;
    }

    #endregion

    [Benchmark(Baseline = true)]
    public int MemoryPack()
    {
        return SumResult(MemoryPackSerializer.Deserialize<IntClass>(_memoryPackPayload)!);
    }

    [Benchmark]
    public int MessagePackCSharp()
    {
        return SumResult(MessagePackSerializer.Deserialize<IntClass>(MsgPackInput, cancellationToken: default));
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
        return SumResult(JsonSerializer.Deserialize<IntClass>(_stjPayload)!);
    }
}
