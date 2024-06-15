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
            var conn = ConnectionMultiplexer.Connect("localhost:6379");
            return conn;
        }).As<ConnectionMultiplexer>().SingleInstance();

        return builder;
    }
}