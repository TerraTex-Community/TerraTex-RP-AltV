using AltV.Net.Enums;
using AltV.Net;
using CustomCommandsSystem.Common.Attributes;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Lib.Helper;
using TerraTex_RolePlay_AltV_Server.Utils.CommandAttributes;

namespace TerraTex_RolePlay_AltV_Server.Lib.Admin.Commands;

public class Weather: IScript
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();


    [CustomCommand("weather")]
    [HasAdminLevel(AdminLevel.Administrator)]
    public static void WeatherCmd(TTPlayer player, string name)
    {
        try
        {
            bool type = Enum.TryParse<WeatherType>(name, out var parsed);

            if (!type)
            {
                player.SendChatMessage("Error: Weather Type not found.");
                return;
            }

            Alt.EmitAllClients("weather:set", name,
                DateTime.Now.ToString("O"));

            Logger.Info($"Set Weather by Admin: {parsed}", DateTime.Now.ToString("O"));
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }
}