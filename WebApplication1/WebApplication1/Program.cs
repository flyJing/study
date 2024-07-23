using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DbUp;
using GraphQL;
using GraphQL.Types;
using Hangfire;
using Hangfire.MySql;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;
using WebApplication1.AutoFac;
using WebApplication1.ContextConfiguration;
using WebApplication1.Cosmos;
using WebApplication1.GraphQL.Schema;
using WebApplication1.Job;

var builder = WebApplication.CreateBuilder(args);

// Use Serilog
builder.Host.UseSerilog();
// Use Autofac as the DI container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new AutoFacBusiness());
    });

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseStorage(new MySqlStorage("Server=127.0.0.1;Database=school;User Id=root;Password=123456; Allow User Variables=true", new MySqlStorageOptions())));

// Add Hangfire server
builder.Services.AddHangfireServer();

builder.Services.AddScoped<ISchema, AppSchema>();

builder.Services.AddDbContext<CosmosDbContext>(options =>
    options.UseCosmos(
        "https://localhost:8081",
        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
        "test_barak"));

// Add services to the container
builder.Services.AddControllers();

// Add Quartz
builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService(x =>
{
    x.WaitForJobsToComplete = true;
});

builder.Services.AddHostedService<QuartzJobRegister>();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGraphQL(x =>
{
    x.AddSystemTextJson();
    x.AddGraphTypes(typeof(AppSchema).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// DbUp configuration
var connectionString = "Server=127.0.0.1; Database=school; Uid=root; Pwd=123456;";
var upgrader = DeployChanges.To
    .MySqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();
var result = upgrader.PerformUpgrade();
if (result.Successful)
{
    Console.WriteLine("Database migration successful!");
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
}

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341/")
    .CreateLogger();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseGraphQL<ISchema>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard("/hangfire");
    endpoints.MapGraphQL("/graphql");
    endpoints.MapGraphQLPlayground("/ui/playground");
});

app.AddJob();

app.Run();

Log.Information("Web application start..");
Log.CloseAndFlush();
