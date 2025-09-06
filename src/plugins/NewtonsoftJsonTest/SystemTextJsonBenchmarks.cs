using BenchmarkDotNet.Attributes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NewtonsoftJsonTest;

//[SimpleJob]
[MemoryDiagnoser]
public class SystemTextJsonBenchmarks
{
    // Adjust buffer size
    private readonly JsonSerializerOptions _optionsBuffer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultBufferSize = 2 * 1024 * 1024
    };

    #region Methods

    [Benchmark]
    public async Task MyDto()
    {
        await JsonSerializer.SerializeAsync(Stream.Null, TestData.Instance.WithPlainData(), MyContext.Default.ListMyDto);
    }

    [Benchmark]
    public async Task MyDtoBuffer()
    {
        await JsonSerializer.SerializeAsync(
            Stream.Null,
            TestData.Instance.WithPlainData(),
            MyContextLargeBuffer.Default.ListMyDto);
    }

    [Benchmark]
    public async Task MyDtoNoSG()
    {
        await JsonSerializer.SerializeAsync(Stream.Null, TestData.Instance.WithPlainData(), _optionsBuffer);
    }

    [Benchmark]
    public async Task MyDtoLarge()
    {
        await JsonSerializer.SerializeAsync(
            Stream.Null,
            TestData.Instance.WithLargeData(),
            MyContext.Default.ListMyDtoLarge);
    }

    [Benchmark]
    public async Task MyDtoLargeBuffer()
    {
        await JsonSerializer.SerializeAsync(
            Stream.Null,
            TestData.Instance.WithLargeData(),
            MyContextLargeBuffer.Default.ListMyDtoLarge);
    }

    [Benchmark]
    public async Task MyDtoLargeNoSG()
    {
        await JsonSerializer.SerializeAsync(Stream.Null, TestData.Instance.WithLargeData(), _optionsBuffer);
    }

    #endregion

}

public class TestData
{

    #region Constants & Statics

    public static TestData Instance { get; } = new TestData();

    #endregion

    private readonly List<MyDtoLarge> _withLargeData;
    private readonly List<MyDto> _withPlainData;
    private readonly MyDto _withPlain;
    private readonly MyDtoLarge _withLarge;

    private TestData()
    {
        _withPlain = new MyDto();
        for (var i = 0; i < 7; i++)
        {
            _withPlain.Text
                .Add(
                    "The DevOps engineer deployed containerized microservices to a Kubernetes while monitoring system metrics carefully.");
        }

        _withLarge = new MyDtoLarge();
        for (var i = 0; i < 4; i++)
        {
            _withLarge.Text
                .Add(
                    "A software developer implemented error robust handling in the API gateway to ensure reliable microservice");
        }

        _withPlainData = [];
        for (var i = 0; i < 1000; i++)
        {
            var current = new MyDto();
            for (var j = 0; j < 7; j++)
            {
                current.Text
                    .Add(
                        "The DevOps engineer deployed containerized microservices to a Kubernetes while monitoring system metrics carefully.");
            }

            _withPlainData.Add(current);
        }

        _withLargeData = [];
        for (var i = 0; i < 1000; i++)
        {
            var current = new MyDtoLarge();
            for (var j = 0; j < 4; j++)
            {
                current.Text
                    .Add(
                        "A software developer implemented error robust handling in the API gateway to ensure reliable microservice");
            }

            _withLargeData.Add(current);
        }
    }

    #region Methods

    public MyDto WithPlain()
    {
        return _withPlain;
    }

    public MyDtoLarge WithLarge()
    {
        return _withLarge;
    }

    public List<MyDtoLarge> WithLargeData()
    {
        return _withLargeData;
    }

    public List<MyDto> WithPlainData()
    {
        return _withPlainData;
    }

    #endregion

}

public class MyDto
{

    #region Properties

    public string Description { get; set; } = "hello";

    public List<string> Text { get; set; } = [];

    #endregion

}

public class MyDtoLarge
{

    #region Properties

    public string Description { get; set; } = "hello";

    public string Description1 { get; set; } = "hello";

    public string Description2 { get; set; } = "hello";

    public string Description3 { get; set; } = "hello";

    public string Name { get; set; } = "binarytree";

    public string Name1 { get; set; } = "binarytree";

    public string Name2 { get; set; } = "binarytree";

    public string Name3 { get; set; } = "binarytree";

    public DateTime Now { get; set; } = DateTime.Now;

    public DateTime Now1 { get; set; } = DateTime.Now;

    public DateTime Now2 { get; set; } = DateTime.Now;

    public DateTime Now3 { get; set; } = DateTime.Now;

    public List<string> Text { get; set; } = [];

    public List<string> Text1 { get; set; } = [];

    public List<string> Text2 { get; set; } = [];

    public List<string> Text3 { get; set; } = [];

    public bool What { get; set; } = true;

    public bool What1 { get; set; } = true;

    public bool What2 { get; set; } = true;

    public bool What3 { get; set; } = true;

    #endregion

    public double PreciseValue = Random.Shared.NextDouble();
    public double PreciseValue1 = Random.Shared.NextDouble();
    public double PreciseValue2 = Random.Shared.NextDouble();
    public double PreciseValue3 = Random.Shared.NextDouble();
}

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(MyDto))]
[JsonSerializable(typeof(MyDtoLarge))]
[JsonSerializable(typeof(List<MyDtoLarge>))]
[JsonSerializable(typeof(List<MyDto>))]
public partial class MyContext : JsonSerializerContext
{
}

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultBufferSize = 2 * 1024 * 1024)]
[JsonSerializable(typeof(MyDto))]
[JsonSerializable(typeof(MyDtoLarge))]
[JsonSerializable(typeof(List<MyDtoLarge>))]
[JsonSerializable(typeof(List<MyDto>))]
public partial class MyContextLargeBuffer : JsonSerializerContext
{
}
