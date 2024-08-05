using Quartz;

namespace WebApplication1.Job;

public class SimpleQuartzJob: IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("简单触发器");
        return Task.CompletedTask;
    }
    
    public static ITrigger GetTrigger()
    {
        
        return TriggerBuilder.Create()
            .WithIdentity($"{nameof(SimpleQuartzJob)}.trigger")
            .WithSimpleSchedule()
            .Build();
    }
}