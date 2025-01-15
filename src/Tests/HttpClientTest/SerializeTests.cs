using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientTest;

internal sealed class SerializeTests
{
    private readonly IHttpClientFactory _factory;

    public SerializeTests()
    {
        var services = new ServiceCollection();
        services.AddHttpClient("test", client =>
        {
            //https: //parkingdev.fangte.com/PlaceService/api/CarParkingOpen/GetParkingInfos
            client.BaseAddress = new Uri("https://parkingdev.fangte.com/");
            var a = client.DefaultRequestHeaders.Accept;
        }).SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        var provider = services.BuildServiceProvider();

        _factory = provider.GetRequiredService<IHttpClientFactory>();
    }

    public async Task NoOptions_Test()
    {
        var client = _factory.CreateClient("test");

        var response = await client.GetAsync("PlaceService/api/CarParkingOpen/GetParkingInfos");

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        if (response.IsSuccessStatusCode)
        {
            var stream = await response.Content.ReadAsStreamAsync();

            var a = await JsonSerializer.DeserializeAsync<Result>(stream);
            //System.Text.Json 序列化不会返回 null, 创建一个实例并返回
            //"{\"Data\":null,\"Code\":0,\"Message\":null,\"Success\":false}"
        }
    }

    public static JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    public async Task WithOptions_Test()
    {
        var client = _factory.CreateClient("test");

        var response = await client.GetAsync("PlaceService/api/CarParkingOpen/GetParkingInfos");

        if (response.IsSuccessStatusCode)
        {
            var stream = await response.Content.ReadAsStreamAsync();

            //var a = await JsonSerializer.DeserializeAsync<Result>(stream, Options);
            //stream.Seek(0, SeekOrigin.Begin);

            var b = await JsonSerializer.DeserializeAsync<Result>(stream,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
            //"{\"Data\":[],\"Code\":0,\"Message\":\"\\u64CD\\u4F5C\\u6210\\u529F\",\"Success\":true}"
        }
    }
}
