using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
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
/// Compares Orleans deserialization performance against other popular serializers for array types.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[Config(typeof(BenchmarkConfig))]
[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
public class ArrayDeserializeBenchmark
{

    #region Constants & Statics

    private static readonly MyVector3[] _value = Enumerable.Repeat(new MyVector3(10.3f, 40.5f, 13411.3f), 1000)
        .ToArray();

    private static readonly SerializerSession _session;
    private static readonly Serializer<MyVector3[]> _orleansSerializer;
    private static readonly byte[] _orleansPayload;

    private static readonly byte[] _stjPayload = JsonSerializer.SerializeToUtf8Bytes(_value);
    private static readonly byte[] _messagePackPayload = MessagePackSerializer.Serialize(_value);
    private static readonly byte[] _memoryPackPayload = MemoryPackSerializer.Serialize(_value);

    static ArrayDeserializeBenchmark()
    {
        var serviceProvider = new ServiceCollection()
            .AddSerializer()
            .BuildServiceProvider();
        _orleansSerializer = serviceProvider.GetRequiredService<Serializer<MyVector3[]>>();
        _session = serviceProvider.GetRequiredService<SerializerSessionPool>().GetSession();

        _orleansPayload = _orleansSerializer.SerializeToArray(_value);
    }

    #endregion

    #region Methods

    [Benchmark(Baseline = true)]
    public MyVector3[] MemoryPackDeserialize()
    {
        return MemoryPackSerializer.Deserialize<MyVector3[]>(_memoryPackPayload)!;
    }

    [Benchmark]
    public MyVector3[] MessagePackDeserialize()
    {
        return MessagePackSerializer.Deserialize<MyVector3[]>(_messagePackPayload, cancellationToken: default);
    }

    [Benchmark]
    public MyVector3[] OrleansDeserialize()
    {
        return _orleansSerializer.Deserialize(_orleansPayload);
    }

    [Benchmark]
    public MyVector3[] OrleansReaderDeserialize()
    {
        //_session.PartialReset();
        _session.Reset();
        var reader = Reader.Create(_orleansPayload, _session);
        return _orleansSerializer.Deserialize(ref reader);
    }

    [Benchmark]
    public MyVector3[] SystemTextJsonDeserialize()
    {
        return JsonSerializer.Deserialize<MyVector3[]>(_stjPayload)!;
    }

    #endregion

}
