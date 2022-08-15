namespace MiddlewareTest;

public class TestMiddleware // : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Console.WriteLine("TestMiddleware starting.");

        await next(context);

        Console.WriteLine("TestMiddleware completed.");
    }
}
