using System.Text.Json.Nodes;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;
using WebApplication.Common;

namespace WebApplication.ContextConfiguration;

public class SchoolContext : DbContext
{
    private string _mySqlStr;
    private string _mySqlVersion;
    private const string MYSQL = "MySQL";
    private const string MYSQLURL = "Url";
    private const string MYSQLVERSION = "Version";
    
    
    public SchoolContext(IConfiguration configuration)
    {
        _mySqlStr = configuration[MYSQL + ":" + MYSQLURL];
        _mySqlVersion = configuration[MYSQL + ":" + MYSQLVERSION];
    }


    public SchoolContext(string mySqlStr, string mySqlVersion)
    {
        _mySqlStr = mySqlStr;
        _mySqlVersion = mySqlVersion;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //c# 链接mysql的 连接字符串怎么写
        var mysqlConnectString=_mySqlStr;
        optionsBuilder.UseMySql(mysqlConnectString,ServerVersion.Parse(_mySqlVersion));
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        typeof(SchoolContext).Assembly.GetTypes().Where(x => x.IsClass && !x.IsInterface && !x.IsAbstract)
            .Where(x => x.IsAssignableTo(typeof(IEntity)))
            .ToList().ForEach(r => {
                if(modelBuilder.Model.FindEntityType(r) is null)
                    modelBuilder.Model.AddEntityType(r);
            });
        //base.OnModelCreating(modelBuilder);
    }
}