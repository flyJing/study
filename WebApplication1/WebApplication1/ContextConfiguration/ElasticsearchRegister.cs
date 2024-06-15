using Autofac;
using Nest;

namespace WebApplication1.ContextConfiguration;

public static class ElasticsearchRegister
{
    public static ContainerBuilder RegisterElasticClient(this ContainerBuilder builder)
    {
        builder.Register(c =>
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200/"))
                .DefaultIndex("barak"); 
            
            var client = new ElasticClient(settings);
            return client;
        }).As<IElasticClient>().SingleInstance();

        return builder;
    }
}