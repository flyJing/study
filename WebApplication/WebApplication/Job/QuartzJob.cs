using Quartz;

namespace WebApplication.Job;

public class QuartzJob: IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        int retryCount = 0;
        bool success = false;

        while (retryCount < 3 && !success)
        {
            try
            {
                // 执行作业逻辑
                Console.WriteLine("cron表达式");
                success = true;
            }
            catch (Exception ex)
            {
                retryCount++;
                if (retryCount >= 3)
                {
                    throw; // 超过最大重试次数，抛出异常
                }
            }
        }
        
        return Task.CompletedTask;
    }
    
    public static ITrigger GetTrigger()
    {
        return TriggerBuilder.Create()
            .WithIdentity($"{nameof(QuartzJob)}.trigger")
            .WithCronSchedule("3/5 * * * * ?")
            .Build();
    }
}

