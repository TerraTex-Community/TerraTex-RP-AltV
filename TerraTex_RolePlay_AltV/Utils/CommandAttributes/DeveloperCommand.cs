using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Attributes;
using CustomCommandsSystem.Common.Models;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Lib.Helper;

namespace TerraTex_RolePlay_AltV_Server.Utils.CommandAttributes;

public class DeveloperCommandAttribute : CustomCommandRequirementBaseAttribute
{
    public DeveloperLevel MinProductiveModeDeveloperLevel { get; set; }
    public DeveloperLevel MinDevelopmentModeDeveloperLevel { get; set; }
    public bool AllowedForDevelopersOnProductive { get; set; }

    public DeveloperCommandAttribute(
        bool allowedForDevelopersOnProductive = false,
        DeveloperLevel minProductiveModeDeveloperLevel = DeveloperLevel.Developer, 
        DeveloperLevel minDevelopmentModeDeveloperLevel = DeveloperLevel.Tester)
    {
        MinProductiveModeDeveloperLevel = minProductiveModeDeveloperLevel;
        MinDevelopmentModeDeveloperLevel = minDevelopmentModeDeveloperLevel;
        AllowedForDevelopersOnProductive = allowedForDevelopersOnProductive;
    }

    /// <summary>
    /// Checks:
    /// - if Productive Server:
    ///     - is generally forbidden = false
    ///     - allowed for developer => check min prod dev level?
    /// - if Developer Server
    ///     - check min dev level
    /// </summary>
    /// <param name="player"></param>
    /// <param name="info"></param>
    /// <param name="methodArgs"></param>
    /// <returns></returns>
    public override bool CanExecute(IPlayer player, CustomCommandInfo? info, ArraySegment<object?> methodArgs)
    {
        if (!AllowedForDevelopersOnProductive && 
            !AdminHelper.IsDevelopmentServer()) return false;

        if (AllowedForDevelopersOnProductive &&
            !AdminHelper.IsDevelopmentServer() &&
            !AdminHelper.IsDeveloper((TTPlayer)player, MinProductiveModeDeveloperLevel)) return false;

        if (AdminHelper.IsDevelopmentServer() &&
            !AdminHelper.IsDeveloper((TTPlayer)player, MinDevelopmentModeDeveloperLevel)) return false;

        return true;
    }
}