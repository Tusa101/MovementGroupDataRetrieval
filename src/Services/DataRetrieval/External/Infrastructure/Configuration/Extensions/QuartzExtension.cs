using Infrastructure.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure.Configuration.Extensions;
public static class QuartzExtension
{
    public static IServiceCollection AddQuartzConfiguration(this IServiceCollection services)
    {
        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(FileSystemCacheManagementJob));

            config
                .AddJob<FileSystemCacheManagementJob>(jobKey)
                .AddTrigger(
                    trigger => trigger.ForJob(jobKey).WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(30).RepeatForever()));
        });

        services.AddQuartzHostedService(options => 
            options.WaitForJobsToComplete = true);
        return services;
    }
}
