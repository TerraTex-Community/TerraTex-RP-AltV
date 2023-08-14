using AltV.Net;
using TerraTex_RolePlay_AltV_Server.CustomFactories;

namespace TerraTex_RolePlay_AltV_Server.Lib.Helper;

public enum AdminLevel
{
    None,
    Supporter,
    Moderator,
    Administrator,
    Serverleitung
}

public enum DeveloperLevel
{
    None,
    Tester,
    Developer
}


public static class AdminHelper
{
    public static bool IsAdmin(TTPlayer player, AdminLevel minAdminLvl = (AdminLevel) 1, bool disableCheckOnDevelopmentServer = false)
    {
        return (disableCheckOnDevelopmentServer && IsDevelopmentServer()) || player.DbUser!.AdminLevel >= minAdminLvl;
    }

    public static bool IsDevelopmentServer()
    {
        var generalConfigNode = Alt.Core.GetServerConfig().Get("TerraTex").Get("General");
        bool isDevServer = generalConfigNode.Get("isDevelopmentServer").GetBoolean().GetValueOrDefault(false);


        return isDevServer;
    }

    public static bool IsDeveloper(TTPlayer player, DeveloperLevel minDeveloperLevel = (DeveloperLevel)1)
    {
        return player.DbUser!.DevLevel >= minDeveloperLevel;
    }
}