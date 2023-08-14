using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Attributes;
using CustomCommandsSystem.Common.Models;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Lib.Helper;

namespace TerraTex_RolePlay_AltV_Server.Utils.CommandAttributes;

public class HasAdminLevelAttribute : CustomCommandRequirementBaseAttribute
{
    public AdminLevel AdminLevel { get; set; }
    public bool DisableCheckOnDevelopmentServer { get; set; } = false;

    public HasAdminLevelAttribute(AdminLevel adminLevel, bool disableCheckOnDevelopmentServer = false)
    {
        AdminLevel = adminLevel;
        DisableCheckOnDevelopmentServer = disableCheckOnDevelopmentServer;
    }

    public HasAdminLevelAttribute(AdminLevel adminLevel) => AdminLevel = adminLevel;

    public override bool CanExecute(IPlayer player, CustomCommandInfo? info, ArraySegment<object?> methodArgs)
    {
        return AdminHelper.IsAdmin((TTPlayer)player, AdminLevel, DisableCheckOnDevelopmentServer);
    }
}