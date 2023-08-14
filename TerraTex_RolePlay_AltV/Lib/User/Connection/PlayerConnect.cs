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
        Database.Entities.User? user = await Globals.TTDatabase!.Users.Where(user => user.Nickname == player.Name).FirstOrDefaultAsync();

        if (user == null)
        {
            player.Emit("Connect:Register");
        }
        else
        {
            player.Emit("Connect:Login", EmailHelper.MaskPlayerEmail(user.Email));
        }
    }

}