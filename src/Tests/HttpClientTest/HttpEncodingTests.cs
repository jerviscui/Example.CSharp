using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientTest;

internal class HttpEncodingTests
{
    private readonly IHttpClientFactory _factory;

    public HttpEncodingTests()
    {
        var services = new ServiceCollection();
        services.AddHttpClient();

        var provider = services.BuildServiceProvider();

        _factory = provider.GetRequiredService<IHttpClientFactory>();
    }

    public async Task Encoding_Test()
    {
        var client = _factory.CreateClient();
        client.BaseAddress = new Uri("https://pss.bdstatic.com");
        client.DefaultRequestVersion = HttpVersion.Version20;
        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

        var response =
            await client.GetAsync("/r/www/cache/static/protocol/https/amd_modules/@baidu/search-sug_05232f9.js");
        //response.Content.Headers.ContentEncoding.FirstOrDefault() == "br"
    }
}
