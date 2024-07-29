using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using WebApplication1;
using WebApplication1.AutoFac;

public class Program
{
    public static void Main(string[] args)
    {
        // Configure Serilog
        Log.Information("Web application start..");

        // Create the host
        CreateHostBuilder(args).Build().Run();
        
        // Flush Serilog
        Log.CloseAndFlush();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>(); // Use Startup class
            });
}