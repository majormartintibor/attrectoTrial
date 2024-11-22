using Microsoft.Extensions.Options;
using Quartz;
using Wolverine;

namespace Feed.API.BackgroundJobs;

public class MidnightCleanupJob(IMessageBus bus) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await bus.InvokeAsync(new Core.FeedDomain.FeedCommand.HardDeleteFeeds());
    }
}

public class MidnightCleanupJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(MidnightCleanupJob));
        options
            .AddJob<MidnightCleanupJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger
                .ForJob(jobKey)
                .StartAt(DateBuilder.DateOf(0, 0, 0))
                .WithSimpleSchedule(schedule =>
                    schedule.WithIntervalInHours(24).RepeatForever()));
    }
}

public static class MidnightCleanupConfiguration
{
    public static IServiceCollection ConfigureMidnightCleanup(this IServiceCollection services)
    {
        services.AddQuartz(opt => { });
        services.AddQuartzHostedService(opt => 
        { 
            opt.WaitForJobsToComplete = true;
        });
        services.ConfigureOptions<MidnightCleanupJobSetup>();

        return services;
    }
}