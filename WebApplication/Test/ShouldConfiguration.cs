using Autofac;
using AutoMapper;
using Mediator.Net;
using Mediator.Net.Autofac;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Respawn;
using Respawn.Graph;
using WebApplication.Common;
using WebApplication.ContextConfiguration;
using WebApplication.Handler.StudentHandler;
using WebApplication.Service;
using WebApplication.Service.Impl;
using WebApplication.Validator;

namespace Test;

public class ShouldConfiguration
{
    private const string _mysqlConnectionStr = 
        "server=localhost;Database=school;userId=root;Password=123456;port=3306;";

    private const string _mysqlVersion = "8.0.33";

    public static MySqlConnection Connection;
    
    public static ContainerBuilder ShouldConfig()
    {
        var _builder = new ContainerBuilder();
        
        _builder.RegisterType<StudentService>().As<IStudentService>().InstancePerLifetimeScope();
        _builder.RegisterType<TeacherService>().As<ITeacherService>().InstancePerLifetimeScope();
        // 批量注册
        _builder.RegisterTypes(typeof(AddStudentValidator),
            typeof(DeleteStudentValidator),
            typeof(UpdateStudentValidator),
            typeof(AddTeacherValidator)).InstancePerLifetimeScope();
        
        // schoolcontext
        _builder.Register(ctx => new SchoolContext(_mysqlConnectionStr, _mysqlVersion)).AsSelf()
            .As<DbContext>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        // automapper
        _builder.Register(ctx => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(StudentProfile));
            cfg.AddProfile(typeof(TeacherProfile));
        }));
        _builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().InstancePerLifetimeScope();
        
        //Mediator 
        var mediatorBuilder = new MediatorBuilder();
        mediatorBuilder.RegisterHandlers(typeof(AddStudentCommandHandler).Assembly).Build();
        _builder.RegisterMediator(mediatorBuilder);

        
        return _builder;
    }

    private static void MySqlConnectionOpen()
    {
        Connection = new MySqlConnection(_mysqlConnectionStr);
        Connection.Open();
    }
    
    public static async Task<Respawner> ShouldRespawnerConfig(Table[] tables)
    {
        // respawner
        MySqlConnectionOpen();
        var respawner = await Respawner.CreateAsync(Connection, new RespawnerOptions()
        {
            DbAdapter = DbAdapter.MySql,
            TablesToInclude = tables
        });
        return respawner;
    }
}