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
        services.AddHttpClient("simple")
            .ConfigureHttpClient(client => client.DefaultRequestVersion = HttpVersion.Version20);

        services.AddHttpClient("decompress")
            .ConfigureHttpClient(client => client.DefaultRequestVersion = HttpVersion.Version20)
            .ConfigurePrimaryHttpMessageHandler(provider =>
                new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All });

        var provider = services.BuildServiceProvider();

        _factory = provider.GetRequiredService<IHttpClientFactory>();
    }

    public async Task Encoding_Test()
    {
        var client = _factory.CreateClient("simple");
        client.BaseAddress = new Uri("https://pss.bdstatic.com");
        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

        var response =
            await client.GetAsync("/r/www/cache/static/protocol/https/amd_modules/@baidu/search-sug_05232f9.js");
        //no DecompressionHandler, response.Content type {System.Net.Http.HttpConnectionResponseContent}
        //response.Content.Headers.ContentEncoding.FirstOrDefault() == "br"
    }

    public async Task Decompressg_Test()
    {
        var client = _factory.CreateClient("decompress");
        client.BaseAddress = new Uri("https://pss.bdstatic.com");

        var response =
            await client.GetAsync("/r/www/cache/static/protocol/https/amd_modules/@baidu/search-sug_05232f9.js");
        //响应内容自动解压，response.Content type {System.Net.Http.DecompressionHandler.BrotliDecompressedContent}
        //response.Content.Headers.ContentEncoding.Count == 0
    }
}
