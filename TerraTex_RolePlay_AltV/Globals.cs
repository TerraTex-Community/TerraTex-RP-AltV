using Quartz;
using Quartz.Impl;
using TerraTex_RolePlay_AltV_Server.Database;

namespace TerraTex_RolePlay_AltV_Server;

static class Globals
{
    // ReSharper disable once InconsistentNaming
    public static TerraTexDatabaseContext? TTDatabase;
    public static IScheduler? Scheduler;
    public static StdSchedulerFactory? Factory;
}