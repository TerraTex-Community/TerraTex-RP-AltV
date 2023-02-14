using AltV.Net;
using AltV.Net.Data;
using Microsoft.EntityFrameworkCore;
using TerraTex_RolePlay_AltV_Server.CustomFactories;

namespace TerraTex_RolePlay_AltV_Server.Lib.User;

public class PlayerConnect : IScript
{

    [ScriptEvent(ScriptEventType.PlayerConnect)]
    public async void OnPlayerConnected(TTPlayer player, string reason)
    {
        Database.Entities.User? user = await Globals.TTDatabase!.Users.Where(user => user.Nickname == player.Name).FirstOrDefaultAsync();

        player.Emit(user == null ? "Connect:Register" : "Connect:Login");
    }

    [ScriptEvent(ScriptEventType.PlayerBeforeConnect)]
    public string OnPlayerBeforeConnect(PlayerConnectionInfo connectionInfo, string reason)
    {

        // stuff to check a ban:
        // connectionInfo.HwidExHash
        // connectionInfo.SocialId
        // connectionInfo.HwidHash

        // ...
        // null to allow the connection
        return null;
    }

}