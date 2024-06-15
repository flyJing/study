using Autofac;
using Serilog.Context;
using WebApplication1.Service;
namespace WebApplication1.Job;

public interface IJobSafeRunner: IService
{
    Task Run(Type jobType);
}

public class JobSafeRunner : IJobSafeRunner
{
    private readonly ILifetimeScope _lifetimeScope;
   

    public JobSafeRunner(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }
        
    public async Task Run(Type jobType)
    {
        await using var newScope = _lifetimeScope.BeginLifetimeScope();
            
        var job = (IJob)newScope.Resolve(jobType);
            
        using (LogContext.PushProperty("JobId", Guid.NewGuid()))
        {
            await job.ScheduleRecurringJob();
        }
    }
}