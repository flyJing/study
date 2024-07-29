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
using WebApplication1.GraphQL.Schema;
using WebApplication1.Job;

namespace WebApplication1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // ConfigureServices method is used to register services
        public void ConfigureServices(IServiceCollection services)
        {
            // Serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341/")
                .CreateLogger();

            // Use Autofac as the DI container
            services.AddAutofac(builder =>
            {
                builder.RegisterModule(new AutoFacBusiness());
            });

            // Hangfire configuration
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(new MySqlStorage("Server=127.0.0.1;Database=school;User Id=root;Password=123456; Allow User Variables=true", new MySqlStorageOptions())));

            // Add Hangfire server
            services.AddHangfireServer();

            services.AddScoped<ISchema, AppSchema>();

            // Cosmos DB context configuration
            services.AddDbContext<CosmosDbContext>(options =>
                options.UseCosmos(
                    "https://localhost:8081",
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                    "test_barak"));

            // Add services to the container
            services.AddControllers();

            // Add Quartz
            services.AddQuartz();
            services.AddQuartzHostedService(x =>
            {
                x.WaitForJobsToComplete = true;
            });

            services.AddHostedService<QuartzJobRegister>();

            // Configure Swagger/OpenAPI
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddGraphQL(x =>
            {
                x.AddSystemTextJson();
                x.AddGraphTypes(typeof(AppSchema).Assembly);
            });
        }

        // Configure method is used to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline
            if (env.IsDevelopment())
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
        }
        
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new AutoFacBusiness());
        }
    }
}
