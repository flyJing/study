using Autofac;
using Nest;
using StackExchange.Redis;

namespace WebApplication1.ContextConfiguration;

public static class RedisRegister
{
    public static ContainerBuilder RegisterRedisClient(this ContainerBuilder builder)
    {
        builder.Register(c =>
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { "localhost:6379" },
                ConnectTimeout = 5000, // 连接超时时间（毫秒），
                AbortOnConnectFail = false, // 连接失败时不终止
                ReconnectRetryPolicy = new ExponentialRetry(5000), // 指数重试策略
                ConnectRetry = 3 // 重试次数
            };
            var conn = ConnectionMultiplexer.Connect("localhost:6379");
            conn.ConnectionFailed += (sender, args) =>
            {
                // 记录连接失败日志
                Console.WriteLine($"Redis 连接失败: {args.Exception.Message}");
            };

            conn.ConnectionRestored += (sender, args) =>
            {
                // 记录连接恢复日志
                Console.WriteLine("Redis 连接已恢复");
            };
            return conn;
        }).As<ConnectionMultiplexer>().SingleInstance();

        return builder;
    }
}