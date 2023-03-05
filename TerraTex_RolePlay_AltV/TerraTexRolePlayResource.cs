using System.Diagnostics;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using AltV.Net.Shared.Elements.Data;
using CustomCommandsSystem.Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Database;
using TerraTex_RolePlay_AltV_Server.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TerraTex_RolePlay_AltV_Server
{
    public class TerraTexRolePlayResource: AsyncResource
    {

        public override async void OnStart()
        {
            Process currentProcess = Process.GetCurrentProcess();
            if (File.Exists("pid.txt"))
            {
                File.Delete("pid.txt");
            }
            await File.WriteAllTextAsync("pid.txt", currentProcess.Id.ToString());

            // Start Restart Checker Task
            new RestartChecker();


            Alt.Core.RegisterCustomCommands();
            Console.WriteLine("TerraTex Server started");

            Globals.TTDatabase = new TerraTexDatabaseContext();
            Globals.TTDatabase.Database.Migrate();

            await Scheduler();

            DatabaseSaveJob.Init();
        }

        private async Task<bool> Scheduler()
        {
            // init scheduler
            Globals.Factory = new StdSchedulerFactory();
            Globals.Scheduler = await Globals.Factory.GetScheduler();

            await Globals.Scheduler.Start();

            return true;
        }

        public override async void OnStop()
        {
            
            await Globals.Scheduler!.Shutdown();
            await Globals.TTDatabase!.SaveChangesAsync(true);

            Console.WriteLine("TerraTex Server Stopped");

            File.Delete("pid.txt");
            File.Delete("stop.command");
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new TTPlayerFactory();
        }
    }
}