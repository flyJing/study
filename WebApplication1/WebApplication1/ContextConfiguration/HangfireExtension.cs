using System.Reflection;
using Autofac;
using Hangfire;
using Org.BouncyCastle.Tls;
using WebApplication1.Job;

namespace WebApplication1.ContextConfiguration;

public static class HangfireExtension
{
    public static void AddJob(this IApplicationBuilder app)
    {
        foreach (var type in typeof(IHangFireJob).GetTypeInfo().Assembly.GetTypes()
                     .Where(t => typeof(IHangFireJob).IsAssignableFrom(t) && t.IsClass))
        {
           
            var job = (IHangFireJob) app.ApplicationServices.GetRequiredService(type);
           
            RecurringJob.AddOrUpdate<IJobSafeRunner>(type.Name, r => r.Run(type),
                job.CronExpression);
        }
    }
}