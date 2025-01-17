namespace WebApplication.Job;

public class HangFireJob: IHangFireJob
{
    public Task ScheduleRecurringJob()
    {
        Console.WriteLine("job 被执行了....");
        return Task.CompletedTask;
    }

    public string CronExpression => "* * * * *";
}