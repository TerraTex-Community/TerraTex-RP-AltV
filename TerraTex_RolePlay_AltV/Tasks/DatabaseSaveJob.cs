using System.Collections.ObjectModel;
using Quartz;

namespace TerraTex_RolePlay_AltV_Server.Tasks;

public class DatabaseSaveJob: IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await Globals.TTDatabase!.SaveChangesAsync(true);
        Console.WriteLine("Database Changes stored.");
    }

    public static async void Init()
    {
        IJobDetail job = JobBuilder.Create<DatabaseSaveJob>()
            .WithIdentity("database_save", "database_save")
            .Build();

        // Trigger the job to run now, and then repeat every 10 seconds
        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("database_save_trigger", "database_save")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(10)
                .RepeatForever())
            .Build();

        await Globals.Scheduler!.ScheduleJob(job, trigger);
    }
}
