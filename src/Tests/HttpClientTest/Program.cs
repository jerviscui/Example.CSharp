// See https://aka.ms/new-console-template for more information

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();
services.AddHttpClient("test", client =>
{
    //https: //parkingdev.fangte.com/PlaceService/api/CarParkingOpen/GetParkingInfos
    client.BaseAddress = new Uri("https://parkingdev.fangte.com/");
    var a = client.DefaultRequestHeaders.Accept;
}).SetHandlerLifetime(Timeout.InfiniteTimeSpan);

var provider = services.BuildServiceProvider();

var factory = provider.GetRequiredService<IHttpClientFactory>();
var client = factory.CreateClient("test");

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
    //System.Text.Json 序列化不会返回 null
    //"{\"Data\":null,\"Code\":0,\"Message\":null,\"Success\":false}"

    stream.Seek(0, SeekOrigin.Begin);
    var b = await JsonSerializer.DeserializeAsync<Result>(stream,
        new JsonSerializerOptions(JsonSerializerDefaults.Web));
    //"{\"Data\":[],\"Code\":0,\"Message\":\"\\u64CD\\u4F5C\\u6210\\u529F\",\"Success\":true}"
}

internal class Result
{
    public object[] Data { get; set; }

    public int Code { get; set; }

    public string Message { get; set; }

    public bool Success { get; set; }
}
