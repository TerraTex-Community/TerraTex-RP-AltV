using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using TerraTex_RolePlay_AltV_Server.CustomFactories;

namespace TerraTex_RolePlay_AltV_Server.Lib.User;

public class PlayerConnect : IScript
{

    [ScriptEvent(ScriptEventType.PlayerConnect)]
    public void OnPlayerConnected(TTPlayer player, string reason)
    {

        

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