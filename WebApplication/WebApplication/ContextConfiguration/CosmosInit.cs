using Autofac;
using Microsoft.Azure.Cosmos;
using Serilog;
using WebApplication.Cosmos;

namespace WebApplication.ContextConfiguration;

public class CosmosInit: IStartable
{
    private readonly IConfiguration _configuration;

    public CosmosInit(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async void Start()
    {
        var primaryKey = _configuration["CosmosDb:AccountKey"];
        var endpointUri = _configuration["CosmosDb:Endpoint"];

        CosmosClientOptions options = new ()
        {
            HttpClientFactory = () => new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            }),
            ConnectionMode = ConnectionMode.Gateway,
            LimitToEndpoint = true
        };
                
        var cosmosClient = new CosmosClient(endpointUri, primaryKey, options);
        try
        {
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync("test_barak");
            var cosmosEntityList = typeof(ICosmosEntity).Assembly.GetTypes()
                .Where(x=>x.IsAssignableTo(typeof(ICosmosEntity)))
                .Where(x=>x.IsClass && !x.IsAbstract)
                .ToList();
            foreach (var cosmosEntity in cosmosEntityList)
            {
                await database.CreateContainerIfNotExistsAsync(cosmosEntity.Name, "/Id");
                // var containerProperties = await container.ReadContainerAsync();
                //
                // IndexingPolicy indexingPolicy = new IndexingPolicy
                // {
                //     ExcludedPaths =
                //     {
                //         new ExcludedPath { Path = "/*" }
                //     }
                // };
                //
                // containerProperties.Resource.IndexingPolicy = indexingPolicy;
                // await container.ReplaceContainerAsync(containerProperties);
            }
        }
        catch (Exception e)
        {
            Log.Error("cosmos init error : {message}", e);
        }
        
    }
}