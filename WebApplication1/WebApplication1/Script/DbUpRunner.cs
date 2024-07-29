using System.Reflection;
using Autofac;
using DbUp;
using Serilog;

namespace WebApplication1.Script;

public class DbUpRunner: IStartable
{
    private readonly IConfiguration _configuration;
    
    public DbUpRunner(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Start()
    {
        var connectionString = _configuration.GetConnectionString("DbupConnectMysqlAddress");

        var upgrader =
            DeployChanges.To
                .MySqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Log.Error("db up running false :{result.Error}",result.Error);
        }
        Log.Information("db up running success!");
    }
}