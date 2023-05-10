using System.Diagnostics;
using System.Reflection.Emit;
using System.Threading;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using AltV.Net.Shared.Elements.Data;
using CustomCommandsSystem.Common.Enums;
using CustomCommandsSystem.Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using Quartz;
using Quartz.Impl;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Database;
using TerraTex_RolePlay_AltV_Server.Lib.Environment.Weather;
using TerraTex_RolePlay_AltV_Server.Lib.System.ConsoleInput;
using TerraTex_RolePlay_AltV_Server.Tasks;

namespace TerraTex_RolePlay_AltV_Server
{
    public class TerraTexRolePlayResource: AsyncResource
    {
        public override async void OnStart()
        {
            LoggerConfiguration.ConfigureLogger();

            Process currentProcess = Process.GetCurrentProcess();
            if (File.Exists("pid.txt"))
            {
                File.Delete("pid.txt");
            }
            await File.WriteAllTextAsync("pid.txt", currentProcess.Id.ToString());

            CustomCommandsSystem.Integration.Settings.Config.CommandDoesNotExistError = "~r~Der Befehl existiert nicht.";
            CustomCommandsSystem.Integration.Settings.Config.CommandUsedIncorrectly = "~r~Der Befehl wurde nicht korrekt benutzt.";
            CustomCommandsSystem.Integration.Settings.Config.PlayerNotFoundErrorMessage = "~r~Der Spieler konnte nicht gefunden werden.";
            CustomCommandsSystem.Integration.Settings.Config.UsageOutputType = UsageOutputType.AllUsages;
            CustomCommandsSystem.Integration.Settings.Config.MultipleUsagesOutputPrefix = "~r~Nutzung: ";
            CustomCommandsSystem.Integration.Settings.Config.SingleUsageOutputPrefix = "~r~Nutzung: ";
            
            Alt.Core.RegisterCustomCommands();
            
            Console.WriteLine("TerraTex Server started");

            Globals.TTDatabase = new TerraTexDatabaseContext();
            await Globals.TTDatabase.Database.MigrateAsync();

            await Scheduler();

            // Start Restart Checker Task
            RestartChecker.Init();
            DatabaseSaveJob.Init();
            DailyAutomaticShutdown.Init();

            // additional stuff
            //  @todo: allow scripts to selfregister for onServerStartUpFinish Event
            Weather.Init();
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
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("Start stopping Resource TerraTex");

            await Globals.Scheduler!.Shutdown();
            logger.Info("Scheduler Stopped");
            await Globals.TTDatabase!.SaveChangesAsync(true);
            logger.Info("Changed Data stored");

            File.Delete("pid.txt");
            File.Delete("stop.command");

            logger.Info("Resource Stopped completely");
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new TTPlayerFactory();
        }
    }
}