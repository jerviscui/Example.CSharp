using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace WebApplicationTest.Controllers;

[ApiController]
[Route("[controller]")]
public class KestrelTestController : ControllerBase
{
    private readonly IHttpContextAccessor _contextAccessor;

    public KestrelTestController(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    #region Methods

    [HttpGet]
    [Route("DisableBuffering")]
    public async Task<IActionResult> DisableBufferingAsync(CancellationToken cancellation)
    {
        //curl http://localhost:5095/KestrelTest/DisableBuffering -v

        var context = _contextAccessor.HttpContext!;
        var feature = context.Features.GetRequiredFeature<IHttpResponseBodyFeature>();
        feature.DisableBuffering(); // 无效果，掉不掉用都不使用响应缓冲区

        context.Response.StatusCode = 200;
        context.Response.ContentType = "text/plain; charset=utf-8";
        //context.Response.ContentLength = null;

        await feature.StartAsync(cancellation);

        for (var i = 0; i < 5; ++i)
        {
            var line = $"this is line {i}\r\n";
            var bytes = Encoding.UTF8.GetBytes(line);
            // it seems context.Response.Body.WriteAsync() and
            // context.Response.BodyWriter.WriteAsync() work exactly the same
            var flushResult = await feature.Writer.WriteAsync(new ReadOnlyMemory<byte>(bytes), cancellation); //此时发送响应头
            if (flushResult.IsCompleted)
            {
                // 此处不会执行
                _ = await feature.Writer.FlushAsync(cancellation);
            }

            await Task.Delay(1000, cancellation);
        }

        await feature.CompleteAsync();

        return Empty;
    }

    [HttpGet]
    [Route("EnableBuffering")]
    public async Task<IActionResult> EnableBufferingAsync(CancellationToken cancellation)
    {
        //curl http://localhost:5095/KestrelTest/EnableBuffering -v

        var builder = new StringBuilder();
        for (var i = 0; i < 5; ++i)
        {
            var s = $"this is line {i}\r\n";
            _ = builder.Append(s);
            await Task.Delay(1000, cancellation);
        }

        return Ok(builder.ToString());
    }

    #endregion

}
