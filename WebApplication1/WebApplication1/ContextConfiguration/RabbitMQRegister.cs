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
            var configuration = c.Resolve<IConfiguration>();
            var factory = new ConnectionFactory
            {
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"],
                HostName = configuration["RabbitMQ:Host"],
                Port = int.Parse(configuration["RabbitMQ:Port"])
            };
            return  factory.CreateConnection();
        }).As<IConnection>().SingleInstance();

        return builder;
    }
}