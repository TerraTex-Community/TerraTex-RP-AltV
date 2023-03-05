using System.Text.Json;
using AltV.Net;
using AltV.Net.Elements.Entities;
using TerraTex_RolePlay_AltV_Server.Utils.Helper;

namespace TerraTex_RolePlay_AltV_Server.Lib.User.Authentication;

public class Registration : IScript
{
    Registration()
    {
        Alt.OnClient<IPlayer, string>("register:submit", SubmitRegistration);
    }

    async void SubmitRegistration(IPlayer player, string jsonRegistrationData)
    {
        RegistrationData data = JsonSerializer.Deserialize<RegistrationData>(jsonRegistrationData)!;

        string salt = PasswordHelper.GenerateSalt();
        string password = PasswordHelper.Hash(data.Password!, salt);

        var userEntity = new Database.Entities.User
        {
            BirthDay = data.Birthday,
            Email = data.Email!,
            Forename = data.Forename!,
            Lastname = data.Lastname!,
            Gender = data.Gender!,
            Nickname = player.Name,
            Salt = salt,
            Password = password
        };
        
        var resultEntityEntry = await Globals.TTDatabase!.Users.AddAsync(userEntity);
        await Globals.TTDatabase!.SaveChangesAsync();

        Console.WriteLine($"New Account {player.Name} Id {resultEntityEntry.Entity.Id} created");

        player.Emit("Connect:Login");
    }
    
}