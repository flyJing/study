using System.Reflection;
using Autofac;
using Quartz;
using WebApplication1.Service;

namespace WebApplication1.Job;

public class QuartzJobRegister : IHostedService
{
    private readonly IScheduler _scheduler;

    public QuartzJobRegister(IScheduler scheduler)
    {
        _scheduler = scheduler;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var jobTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IJob).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();

        foreach (var jobType in jobTypes)
        {
            var jobDetail = JobBuilder.Create(jobType)
                .WithIdentity(jobType.FullName)
                .Build();

            var getTriggerMethod = jobType.GetMethod("GetTrigger", BindingFlags.Public | BindingFlags.Static);
            if (getTriggerMethod != null)
            {
                var trigger = (ITrigger)getTriggerMethod.Invoke(null, null);
                await _scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
            }
        }

        await _scheduler.Start(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}