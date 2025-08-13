using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using MemoryPack;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Serialization;
using Orleans.Serialization.Buffers;
using Orleans.Serialization.Session;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace MessagePackTest;

/// <summary>
/// Compares Orleans serialization performance against other popular serializers for class types.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[Config(typeof(BenchmarkConfig))]
[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
public class ClassSerializeBenchmark
{

    #region Constants & Statics

    private static readonly IntClass _value = IntClass.Create();

    private static readonly Serializer<IntClass> OrleansSerializer;
    private static readonly SerializerSession Session;

    private static readonly ArrayBufferWriter<byte> _arrayBufferWriter;
    private static readonly Utf8JsonWriter SystemTextJsonWriter;

    private static readonly MemoryStream SystemTextJsonOutput = new();

    static ClassSerializeBenchmark()
    {
        var services = new ServiceCollection()
            .AddSerializer()
            .BuildServiceProvider();
        OrleansSerializer = services.GetRequiredService<Serializer<IntClass>>();
        Session = services.GetRequiredService<SerializerSessionPool>().GetSession();

        var serialize1 = OrleansSerializer.SerializeToArray(_value);
        var serialize2 = MessagePackSerializer.Serialize(_value);
        var serialize3 = MemoryPackSerializer.Serialize(_value);
        var serialize4 = JsonSerializer.SerializeToUtf8Bytes(_value);
        _arrayBufferWriter = new ArrayBufferWriter<byte>(
            new[] { serialize1, serialize2, serialize3, serialize4 }.Max(x => x.Length));

        SystemTextJsonWriter = new Utf8JsonWriter(_arrayBufferWriter);
    }

    #endregion

    #region Methods

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(" byte[]")]
    public int MemoryPack()
    {
        var bytes = MemoryPackSerializer.Serialize(_value, MemoryPackSerializerOptions.Default);
        return bytes.Length;
    }

    [Benchmark]
    [BenchmarkCategory(" byte[]")]
    public int MessagePackCSharp()
    {
        var bytes = MessagePackSerializer.Serialize(_value, cancellationToken: default);
        return bytes.Length;
    }

    [Benchmark]
    [BenchmarkCategory(" byte[]")]
    public int OrleansSerialize()
    {
        var bytes = OrleansSerializer.SerializeToArray(_value);
        return bytes.Length;
    }

    [Benchmark]
    [BenchmarkCategory(" byte[]")]
    public int SystemTextJson()
    {
        JsonSerializer.Serialize(SystemTextJsonOutput, _value);
        var array = SystemTextJsonOutput.ToArray();
        SystemTextJsonOutput.Position = 0;
        return array.Length;
    }

    [Benchmark]
    [BenchmarkCategory(" byte[]")]
    public int SystemTextJsonGenerator()
    {
        JsonSerializer.Serialize(SystemTextJsonOutput, _value, CustomSerializerContext.Default.IntClass);
        var array = SystemTextJsonOutput.ToArray();
        SystemTextJsonOutput.Position = 0;
        return array.Length;
    }

    #endregion

    #region BufferWriter

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("BufferWriter")]
    public void MemoryPackBufferWriter()
    {
        MemoryPackSerializer.Serialize(_arrayBufferWriter, _value, MemoryPackSerializerOptions.Default);
        _arrayBufferWriter.Clear();
    }

    [Benchmark]
    [BenchmarkCategory("BufferWriter")]
    public void MessagePackBufferWriter()
    {
        MessagePackSerializer.Serialize(_arrayBufferWriter, _value, cancellationToken: default);
        _arrayBufferWriter.Clear();
    }

    [Benchmark]
    [BenchmarkCategory("BufferWriter")]
    public void OrleansWriterPooledArrayBufferWriter()
    {
        var writer = Writer.CreatePooled(Session);
        try
        {
            OrleansSerializer.Serialize(_value, ref writer);
        }
        finally
        {
            writer.Dispose();
            Session.Reset();
        }
    }

    [Benchmark]
    [BenchmarkCategory("BufferWriter")]
    public void OrleansWriterArrayBufferWriter()
    {
        var writer = _arrayBufferWriter.CreateWriter(Session);
        try
        {
            OrleansSerializer.Serialize(_value, ref writer);
        }
        finally
        {
            writer.Dispose();
            Session.Reset();
        }

        _arrayBufferWriter.Clear();
    }

    [Benchmark]
    [BenchmarkCategory("BufferWriter")]
    public void SystemTextJsonBufferWriter()
    {
        JsonSerializer.Serialize(SystemTextJsonWriter, _value);
        SystemTextJsonWriter.Flush();
        _arrayBufferWriter.Clear();
        SystemTextJsonWriter.Reset(_arrayBufferWriter);
    }

    [Benchmark]
    [BenchmarkCategory("BufferWriter")]
    public void SystemTextJsonBufferWriterGenerator()
    {
        JsonSerializer.Serialize(SystemTextJsonWriter, _value, CustomSerializerContext.Default.IntClass);
        SystemTextJsonWriter.Flush();
        _arrayBufferWriter.Clear();
        SystemTextJsonWriter.Reset(_arrayBufferWriter);
    }

    #endregion
}
