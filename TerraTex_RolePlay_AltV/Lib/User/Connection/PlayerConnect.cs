using AltV.Net;
using AltV.Net.Data;
using Microsoft.EntityFrameworkCore;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Helper;

namespace TerraTex_RolePlay_AltV_Server.Lib.User.Connection;

public class PlayerConnect : IScript
{

    [ScriptEvent(ScriptEventType.PlayerConnect)]
    public async void OnPlayerConnected(TTPlayer player, string reason)
    {
        player.Spawn(new Position(0,0,0));
        player.Frozen = true;
        player.Visible = false;
        player.Dimension = 1;
        player.Emit("Connect:LoginByDiscord");
    }

}