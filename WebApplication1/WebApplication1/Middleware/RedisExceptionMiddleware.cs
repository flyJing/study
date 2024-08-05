using Serilog;
using StackExchange.Redis;

namespace WebApplication1.Middleware;

public class RedisExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public RedisExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (RedisException ex)
        {
            // 处理 Redis 连接异常
            context.Response.StatusCode = 500;
            Log.Error("Redis error message : {message}", ex);
        }
        
    }
}