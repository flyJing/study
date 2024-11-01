namespace WebApplication.Job;

public interface IHangFireJob
{
    Task ScheduleRecurringJob();
    
    string  CronExpression { get; }
}