using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.EntityFrameworkCore;
using TerraTex_RolePlay_AltV_Server.Utils.Helper;

namespace TerraTex_RolePlay_AltV_Server.Lib.User.Authentication;

public class Login : IScript
{
    Login()
    {
        Alt.OnClient<IPlayer, string>("login:submit", SubmitLogin);
    }

    private async void SubmitLogin(IPlayer player, string passwordTry)
    {
        Database.Entities.User? user = await Globals.TTDatabase!.Users.Where(user => user.Nickname == player.Name).FirstOrDefaultAsync();

        if (PasswordHelper.Hash(passwordTry, user.Salt) == user.Password)
        {
            // @todo: start Loading data


            player.Emit("login:result", true);
        }
        else
        {
            player.Emit("login:result", false);
        }

    }
}