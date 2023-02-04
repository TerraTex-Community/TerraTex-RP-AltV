using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using AltV.Net.Shared.Elements.Data;
using CustomCommandsSystem.Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Database;
using static System.Net.Mime.MediaTypeNames;

namespace TerraTex_RolePlay_AltV_Server
{
    public class TerraTexRolePlayResource: AsyncResource
    {
        public override void OnStart()
        {
            Alt.Core.RegisterCustomCommands();
            Console.WriteLine("TerraTex Server started");

            using var db = new TerraTexDatabaseContext();
            db.Database.Migrate();

        }

        public override void OnStop()
        {
            Console.WriteLine("TerraTex Server Stopped");
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new TTPlayerFactory();
        }
    }
}