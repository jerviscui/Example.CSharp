using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;

namespace HttpClientTest;

internal sealed class Http2Tests
{

    private readonly IHttpClientFactory _factory;

    public Http2Tests()
    {
        var services = new ServiceCollection();
        services.AddHttpClient();

        var provider = services.BuildServiceProvider();

        _factory = provider.GetRequiredService<IHttpClientFactory>();
    }

    #region Methods
    public async Task Http11_Test()
    {
        //https://pss.bdstatic.com/r/www/cache/static/protocol/https/amd_modules/@baidu/search-sug_05232f9.js

        using var client = _factory.CreateClient();
        client.BaseAddress = new Uri("https://pss.bdstatic.com");

        var response =
            await client.GetAsync("/r/www/cache/static/protocol/https/amd_modules/@baidu/search-sug_05232f9.js");
        //response.RequestMessage.Version == HttpVersion.Version11
    }

    public async Task Http2_Get_Test()
    {
        //https://parkingdev.fangte.com/CarService/api/ParkingRecordTest/GetId

        using var client = _factory.CreateClient();
        client.BaseAddress = new Uri("https://parkingdev.fangte.com");
        client.DefaultRequestVersion = HttpVersion.Version20;

        var response =
            await client.GetAsync("CarService/api/ParkingRecordTest/GetId");

        var data = await JsonSerializer.DeserializeAsync<Result<long>>(await response.Content.ReadAsStreamAsync(),
            SerializeTests.Options);
    }

    public async Task Http2_Test()
    {
        using var client = _factory.CreateClient();
        client.BaseAddress = new Uri("https://pss.bdstatic.com"); //http2 必须使用 https
        client.DefaultRequestVersion = HttpVersion.Version20;

        var response =
            await client.GetAsync("/r/www/cache/static/protocol/https/amd_modules/@baidu/search-sug_05232f9.js");
        //response.RequestMessage.Version == HttpVersion.Version20
    }
    #endregion

}
