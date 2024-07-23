using Autofac;
using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Cosmos;
using WebApplication1.Cosmos.Entity;

namespace WebApplication1.ContextConfiguration;

public  class CosmosDbContext: DbContext
{
    public DbSet<CosmosUser> MyEntities { get; set; }
    
    public CosmosDbContext(DbContextOptions<CosmosDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var primaryKey =
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        var endpointUri = "https://localhost:8081";

        optionsBuilder.UseCosmos(
            endpointUri,
            primaryKey,
            "test_barak", cosmosOptionsAction =>
            {
                cosmosOptionsAction.ConnectionMode(ConnectionMode.Gateway);
                cosmosOptionsAction.LimitToEndpoint();
                cosmosOptionsAction.HttpClientFactory(() => new HttpClient(new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }));
            });
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityList = typeof(ICosmosEntity).Assembly.GetTypes()
            .Where(x=>x.IsAssignableTo(typeof(ICosmosEntity)))
            .Where(x=>x.IsClass && !x.IsAbstract)
            .ToList();
        foreach (var entity in entityList)
        {
            var entityType = typeof(CosmosDbContext).Assembly.GetType(entity.FullName);
            if (entityType == null) continue;

            modelBuilder.Entity(entityType).ToContainer(entity.Name);
            // 为实体类配置主键
            modelBuilder.Entity(entityType).HasKey("Id");
            
            modelBuilder.Entity(entityType).HasPartitionKey("Id");
        }
    }
}