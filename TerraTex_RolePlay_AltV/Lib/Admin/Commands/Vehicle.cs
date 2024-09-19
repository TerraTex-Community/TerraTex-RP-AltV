
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using CustomCommandsSystem.Common.Attributes;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Lib.Helper;
using TerraTex_RolePlay_AltV_Server.Utils.CommandAttributes;

namespace TerraTex_RolePlay_AltV_Server.Lib.Admin.Commands;

public class Vehicle: IScript
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    [CustomCommand("veh")]
    [HasAdminLevel(AdminLevel.Administrator)]
    public static async void VehicleCmd(TTPlayer player, string name, bool matt)
    {
        try
        {
            bool found = Enum.TryParse<VehicleModel>(name, out var model);

            if (!found)
            {
                
                player.SendChatMessage($"Error: Vehicle Model not found.");
                return;
            }

            IVehicle veh = Alt.CreateVehicle(model, player.Position, player.Rotation);
            player.SetIntoVehicle(veh, 0);
            if (matt)
            {
                veh.PrimaryColor = 13;
            }
            veh.PrimaryColorRgb = new Rgba(255, 100, 255, 255);
            
            player.SendChatMessage($"Success: Vehicle Model {Alt.GetVehicleModelInfo((uint)model).Title} Spawned.");
            
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }
}