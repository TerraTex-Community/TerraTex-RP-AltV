using System.Timers;
using AltV.Net;
using Quartz;

namespace TerraTex_RolePlay_AltV_Server.Tasks;

public class RestartChecker: IJob
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    public static async void Init()
    {
        IJobDetail job = JobBuilder.Create<RestartChecker>()
            .WithIdentity("restart_checker", "restart_checker")
            .Build();

        // Trigger the job to run now, and then repeat every 10 seconds
        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("restart_checker_trigger", "restart_checker")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(5)
                .RepeatForever())
            .Build();

        await Globals.Scheduler!.ScheduleJob(job, trigger);
    }

    public Task Execute(IJobExecutionContext context)
    {
        if (File.Exists("stop.command"))
        {
            Logger.Info("Found Stop Command ... Start Server Shutdown");
            // Console.WriteLine("Found Stop Command ... Start Server Shutdown");
            Alt.Core.StopServer();
        }

        return Task.CompletedTask;
    }

}