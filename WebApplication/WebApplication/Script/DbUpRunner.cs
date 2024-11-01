using System.Reflection;
using Autofac;
using DbUp;
using Serilog;

namespace WebApplication.Script;

public class DbUpRunner: IStartable
{
    private readonly IConfiguration _configuration;
    
    public DbUpRunner(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Start()
    {
        var connectionString = _configuration["MysqlAddress:Url"];

        EnsureDatabase.For.MySqlDatabase(connectionString);
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