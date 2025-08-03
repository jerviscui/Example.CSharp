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
using System.IO.Pipelines;
using System.Text;
using System.Text.Json;

namespace MessagePackTest;

/// <summary>
/// Compares Orleans serialization performance against other popular serializers for array types.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[Config(typeof(BenchmarkConfig))]
[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
public class ArraySerializeBenchmark
{

    #region Constants & Statics

    private static readonly MyVector3[] _value = Enumerable.Repeat(
        new MyVector3 { X = 10.3f, Y = 40.5f, Z = 13411.3f },
        1000)
        .ToArray();

    private static readonly ArrayBufferWriter<byte> _arrayBufferWriter;
    private static readonly Utf8JsonWriter _jsonWriter;

    private static readonly SerializerSession _session;
    private static readonly Serializer<MyVector3[]> _orleansSerializer;

    private static readonly MemoryStream _stream = new();
    private static readonly Pipe _pipe = new(
        new PipeOptions(
            readerScheduler: PipeScheduler.Inline,
            writerScheduler: PipeScheduler.Inline,
            pauseWriterThreshold: 0));

    static ArraySerializeBenchmark()
    {
        var serviceProvider = new ServiceCollection()
            .AddSerializer()
            .BuildServiceProvider();
        _orleansSerializer = serviceProvider.GetRequiredService<Serializer<MyVector3[]>>();
        _session = serviceProvider.GetRequiredService<SerializerSessionPool>().GetSession();

        var serialize1 = _orleansSerializer.SerializeToArray(_value);
        var serialize2 = MessagePackSerializer.Serialize(_value);
        var serialize3 = MemoryPackSerializer.Serialize(_value);
        var serialize4 = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(_value));
        _arrayBufferWriter = new ArrayBufferWriter<byte>(
            new[] { serialize1, serialize2, serialize3, serialize4 }.Max(x => x.Length));

        _jsonWriter = new Utf8JsonWriter(_arrayBufferWriter);
    }

    #endregion

    #region Methods

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(" byte[]")]
    public byte[] MemoryPackSerialize()
    {
        return MemoryPackSerializer.Serialize(_value, MemoryPackSerializerOptions.Default);
    }

    [Benchmark]
    [BenchmarkCategory(" byte[]")]
    public byte[] MemoryPackSerializeUtf16()
    {
        return MemoryPackSerializer.Serialize(_value, MemoryPackSerializerOptions.Utf16);
    }

    [Benchmark]
    [BenchmarkCategory(" byte[]")]
    public byte[] MessagePackSerialize()
    {
        return MessagePackSerializer.Serialize(_value, cancellationToken: default);
    }

    [Benchmark]
    [BenchmarkCategory(" byte[]")]
    public byte[] OrleansSerialize()
    {
        return _orleansSerializer.SerializeToArray(_value);
    }

    [Benchmark]
    [BenchmarkCategory(" byte[]")]
    public byte[] SystemTextJsonSerialize()
    {
        JsonSerializer.Serialize(_stream, _value);
        var array = _stream.ToArray();
        _stream.Position = 0;
        return array;
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
    public void MemoryPackBufferWriterUtf16()
    {
        MemoryPackSerializer.Serialize(_arrayBufferWriter, _value, MemoryPackSerializerOptions.Utf16);
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
        var writer = Writer.CreatePooled(_session);
        try
        {
            _orleansSerializer.Serialize(_value, ref writer);
        }
        finally
        {
            writer.Dispose();
            _session.Reset();
            //_session.PartialReset();
        }
    }

    [Benchmark]
    [BenchmarkCategory("BufferWriter")]
    public void OrleansWriterArrayBufferWriter()
    {
        var writer = _arrayBufferWriter.CreateWriter(_session);
        try
        {
            _orleansSerializer.Serialize(_value, ref writer);
        }
        finally
        {
            writer.Dispose();
            _session.Reset();
            //_session.PartialReset();
        }

        _arrayBufferWriter.Clear(); // clear ArrayBufferWriter<byte>
    }

    [Benchmark]
    [BenchmarkCategory("BufferWriter")]
    public void OrleansPipeWriter()
    {
        var writer = _pipe.Writer.CreateWriter(_session);
        try
        {
            _orleansSerializer.Serialize(_value, ref writer);
        }
        finally
        {
            writer.Dispose();
            _session.Reset();

            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pipe.Reset();
        }
    }

    [Benchmark]
    [BenchmarkCategory("BufferWriter")]
    public void SystemTextJsonBufferWriter()
    {
        JsonSerializer.Serialize(_jsonWriter, _value);
        _jsonWriter.Flush();
        _arrayBufferWriter.Clear();
        _jsonWriter.Reset(_arrayBufferWriter);
    }

    #endregion

}
