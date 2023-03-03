using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Lib.System;
using TerraTex_RolePlay_AltV_Server.Lib.User.SpawnAndDeath;
using TerraTex_RolePlay_AltV_Server.Utils.Helper;

namespace TerraTex_RolePlay_AltV_Server.Lib.User.Authentication;

public class Login : IScript
{
    Login()
    {
        Alt.OnClient<TTPlayer, string>("login:submit", SubmitLogin);
        Alt.OnClient<TTPlayer>("login:sendConfirmationCode", SendConfirmationCode);
        Alt.OnClient<TTPlayer, string, string>("login:TryChangePassword", ChangePassword);
    }

    private async void ChangePassword(TTPlayer user, string password, string code)
    {
        Database.Entities.User? dbUser = await Globals.TTDatabase!.Users.Where(dbUser => dbUser.Nickname == user.Name).FirstOrDefaultAsync();

        if (ConfirmationSystem.CheckCodeAndRemoveOnSuccess(user, code))
        {
            string salt = PasswordHelper.GenerateSalt();
            string passwordHash = PasswordHelper.Hash(password, salt);

            // @FIXME: DbUser is not valid here as it is only set AFTER Login

            dbUser!.Salt = salt;
            dbUser.Password = passwordHash;

            // @todo: is this needed after adding #31 ? do we need also to mark update? 
            await Globals.TTDatabase!.SaveChangesAsync();

            user.Emit("login:passwordForgottenResult", true);
        }
        else
        {
            user.Emit("login:passwordForgottenResult", false);
        }
    }

    private async void SendConfirmationCode(TTPlayer user)
    {
        Database.Entities.User? dbUser = await Globals.TTDatabase!.Users.Where(dbUser => dbUser.Nickname == user.Name).FirstOrDefaultAsync();

        ConfirmationSystem.SendNewGeneratedKey(user, dbUser!.Email);
    }

    private async void SubmitLogin(TTPlayer player, string passwordTry)
    {
        Database.Entities.User? user = await Globals.TTDatabase!.Users.Where(user => user.Nickname == player.Name).FirstOrDefaultAsync();

        if (PasswordHelper.Hash(passwordTry, user.Salt) == user.Password)
        {
            // Save Last connection data - will be used for bans or auto login later
            user.LastHardwareIdExHash = player.HardwareIdExHash;
            user.LastHardwareIdHash = player.HardwareIdHash;
            user.LastIp = player.Ip;
            user.LastSocialClubId = player.SocialClubId;

            // set db data on factory
            player.DbUser = user;
            player.LoggedIn = true;

            await Globals.TTDatabase!.SaveChangesAsync();
            player.Emit("login:result", true);

            Console.WriteLine($"Account {player.Name} ({player.DbUser.Id}) logged in.");
            ProcessLogin(player);
        }
        else
        {
            player.Emit("login:result", false);
        }
    }

    private async void ProcessLogin(TTPlayer player)
    {
        // load additional data here

        // start after Login calculations
        PlayerSpawnManager.SpawnPlayer(player);

    }
}