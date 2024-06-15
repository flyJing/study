using Hangfire;
using Serilog;

namespace WebApplication1.Job;

public class Job: IJob
{
    public Task ScheduleRecurringJob()
    {
        Console.WriteLine("job 被执行了....");
        return Task.CompletedTask;
    }

    public string CronExpression => "* * * * *";
}