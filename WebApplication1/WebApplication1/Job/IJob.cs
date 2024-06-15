namespace WebApplication1.Job;

public interface IJob
{
    Task ScheduleRecurringJob();
    
    string  CronExpression { get; }
}