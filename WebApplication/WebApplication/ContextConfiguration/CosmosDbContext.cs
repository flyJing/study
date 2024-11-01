
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using WebApplication.Cosmos;
using WebApplication.Cosmos.Entity;

namespace WebApplication.ContextConfiguration;

public  class CosmosDbContext: DbContext
{
    public DbSet<CosmosUser> MyEntities { get; set; }
    private readonly IConfiguration _configuration;
    
    public CosmosDbContext(DbContextOptions<CosmosDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var primaryKey = _configuration["CosmosDb:AccountKey"];
        var endpointUri = _configuration["CosmosDb:Endpoint"];

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