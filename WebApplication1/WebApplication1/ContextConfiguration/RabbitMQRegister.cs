using Autofac;
using Nest;
using RabbitMQ.Client;

namespace WebApplication1.ContextConfiguration;

public static class RabbitMQRegister
{
    public static ContainerBuilder RegisterRabbitMQClient(this ContainerBuilder builder)
    {
        builder.Register(c =>
        {
            var factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost",
                Port = 5673
            };
            return  factory.CreateConnection();
        }).As<IConnection>().SingleInstance();

        return builder;
    }
}