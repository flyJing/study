using Autofac;
using Azure.Identity;
using Microsoft.Azure.Cosmos;
using WebApplication1.Cosmos;

namespace WebApplication1.ContextConfiguration;

public class CosmosInit: IStartable
{
    public async void Start()
    {
        var primaryKey =
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        var endpointUri = "https://localhost:8081";
        
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
}