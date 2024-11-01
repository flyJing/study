using Autofac;
using Nest;

namespace WebApplication.ContextConfiguration;

public static class ElasticsearchRegister
{
    public static ContainerBuilder RegisterElasticClient(this ContainerBuilder builder)
    {
        builder.Register(c =>
        {
            var configuration = c.Resolve<IConfiguration>();
            var elasticsearchUri = configuration["ElasticSearch:Uri"];
            var settings = new ConnectionSettings(new Uri(elasticsearchUri))
                .DefaultIndex(configuration["ElasticSearch:DefaultIndex"]); 
            
            var client = new ElasticClient(settings);
            return client;
        }).As<IElasticClient>().SingleInstance();

        return builder;
    }
}