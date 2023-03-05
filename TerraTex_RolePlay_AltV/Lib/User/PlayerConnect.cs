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

        if (user == null)
        {
            player.Emit("Connect:Register");
        }
        else
        {
            player.Emit("Connect:Login", MaskPlayerEmail(user.Email));
        }
    }

    private string MaskPlayerEmail(string email)
    {
        string[] parts = email.Split("@");

        string firstPart = parts[0].Substring(0, 2) + "***" + parts[0].Substring(parts[0].Length - 2);

        return $"{firstPart}@{parts[1]}";
    }
}