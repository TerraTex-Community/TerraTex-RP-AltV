using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using CustomCommandsSystem.Common.Attributes;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Lib.Helper;
using TerraTex_RolePlay_AltV_Server.Utils.CommandAttributes;

namespace TerraTex_RolePlay_AltV_Server.Lib.Admin.Commands;

public class Teleport: IScript
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    [CustomCommand("tpc")]
    [HasAdminLevel(AdminLevel.Administrator)]
    public static async void TpcCmd(TTPlayer player, float x, float y, float z)
    {
        try
        {
            player.Position = new Position(x, y, z);
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }
    
    [CustomCommand("getpos")]
    [HasAdminLevel(AdminLevel.Administrator)]
    public static async void TpcCmd(TTPlayer player)
    {
        try
        {
            Console.WriteLine($"Current Pos / Rot: {player.Position} {player.Rotation}");
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }
}