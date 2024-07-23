namespace WebApplication1.Job;

public interface IHangFireJob
{
    Task ScheduleRecurringJob();
    
    string  CronExpression { get; }
}