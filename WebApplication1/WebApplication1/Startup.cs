using Autofac;
using Autofac.Extensions.DependencyInjection;
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
using WebApplication1.Middleware;

namespace WebApplication1
{
    public class Startup
    {
        private  IConfiguration Configuration { get; }
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
                .WriteTo.Seq(Configuration["Serilog:Url"])
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
                .UseStorage(new MySqlStorage(Configuration["MysqlAddress:Url"], new MySqlStorageOptions())));

            // Add Hangfire server
            services.AddHangfireServer();

            services.AddScoped<ISchema, AppSchema>();

            // Cosmos DB context configuration
            services.AddDbContext<CosmosDbContext>(options =>
                options.UseCosmos(
                    Configuration["CosmosDb:Endpoint"],
                    Configuration["CosmosDb:AccountKey"],
                    Configuration["CosmosDb:DatabaseName"]));

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
            
            app.UseSwagger();
            app.UseSwaggerUI();
            
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
            app.UseMiddleware<RedisExceptionMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
        }
        
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new AutoFacBusiness());
        }
    }
}
