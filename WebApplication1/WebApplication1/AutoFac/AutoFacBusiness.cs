using System.Reflection;
using Autofac;
using AutoMapper;
using Hangfire;
using Mediator.Net;
using Mediator.Net.Autofac;
using WebApplication1.Common;
using WebApplication1.ContextConfiguration;
using WebApplication1.Job;
using WebApplication1.Service;
using WebApplication1.Validator;
using Module = Autofac.Module;

namespace WebApplication1.AutoFac;

public class AutoFacBusiness : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        builder.RegisterAssemblyTypes(assembly)
            .Where(t => typeof(IService).IsAssignableFrom(t) && t.IsClass)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        foreach (var type in typeof(IJob).GetTypeInfo().Assembly.GetTypes()
                     .Where(t => typeof(IJob).IsAssignableFrom(t) && t.IsClass))
        {
            builder.RegisterType(type).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        // Bulk register validators and other specific types
        builder.RegisterTypes(
            typeof(AddStudentValidator),
            typeof(DeleteStudentValidator),
            typeof(UpdateStudentValidator),
            typeof(AddTeacherValidator),
            typeof(SchoolContext))
            .InstancePerLifetimeScope();

        // Register clients and external services
        builder.RegisterElasticClient();
        builder.RegisterRedisClient();
        builder.RegisterRabbitMQClient();

        // Register Hangfire clients
        builder.RegisterType<BackgroundJobClient>().As<IBackgroundJobClient>().InstancePerLifetimeScope();
        builder.RegisterType<RecurringJobManager>().As<IRecurringJobManager>().InstancePerLifetimeScope();

        // Automapper
        builder.Register(context => new MapperConfiguration(config =>
        {
            config.AddProfile(typeof(TeacherProfile));
            config.AddProfile(typeof(StudentProfile));
        }));
        builder.Register(context => context.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().InstancePerLifetimeScope();

        // Mediator
        var mediatorBuilder = new MediatorBuilder();
        mediatorBuilder.RegisterHandlers(typeof(AutoFacBusiness).Assembly).Build();
        builder.RegisterMediator(mediatorBuilder);
    }
}
