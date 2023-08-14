using System.Security.Cryptography.X509Certificates;
using AltV.Net;
using Quartz;
using TimeZoneConverter;

namespace TerraTex_RolePlay_AltV_Server.Tasks;

public class DailyAutomaticShutdown: IJob
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    public static async void Init()
    {
        IJobDetail job = JobBuilder.Create<DailyAutomaticShutdown>()
            .WithIdentity("daily_automated_restart", "daily_automated_restart")
            .Build();

        // Trigger the job to run now, and then repeat every 10 seconds
        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("daily_automated_restart_trigger", "daily_automated_restart")
            .StartNow()
            .WithSchedule(
                CronScheduleBuilder.DailyAtHourAndMinute(4,0)
                    .InTimeZone(TZConvert.GetTimeZoneInfo("Europe/Berlin"))
            )
            .Build();

        await Globals.Scheduler!.ScheduleJob(job, trigger);
    }

    public Task Execute(IJobExecutionContext context)
    {
        Logger.Info("Automatic Restart: Shutdown Server completely");

        Alt.Core.StopServer();
        return Task.CompletedTask;
    }
}