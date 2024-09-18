using System.Net.Http.Headers;
using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
using NLog.Targets;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Lib.BaseSystem;
using TerraTex_RolePlay_AltV_Server.Lib.User.SpawnAndDeath;
using TerraTex_RolePlay_AltV_Server.Utils.Helper;

namespace TerraTex_RolePlay_AltV_Server.Lib.User.Authentication;

public class Login : IScript
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    Login()
    {
        Alt.OnClient<TTPlayer, string>("Discord:Token", TokenDiscord);
    }

    private async void TokenDiscord(TTPlayer user, string token)
    {
        try
        {
            HttpClient client = new HttpClient();
            // client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer {token}");
            // string response = await client.GetStringAsync("https://discordapp.com/api/users/@me");

            var request = new HttpRequestMessage(HttpMethod.Get, "https://discordapp.com/api/users/@me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            
            HttpResponseMessage response = await client.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonSerializer.Deserialize<DiscordApiResponse>(content)!;
                Console.WriteLine(parsedResponse);
                
                Database.Entities.User? dbUserEntry = await Globals.TTDatabase!.Users.Where(dbUserEntry => dbUserEntry.DiscordId == parsedResponse.id).FirstOrDefaultAsync() ??
                                                      new Database.Entities.User();

                dbUserEntry.DiscordId = parsedResponse.id;
                dbUserEntry.DiscordMFAEnabled = parsedResponse.mfa_enabled;
                dbUserEntry.DiscordUsername = parsedResponse.username;
                
                // hardware data @todo: do check here?
                dbUserEntry.LastHardwareIdExHash = user.HardwareIdExHash;
                dbUserEntry.LastHardwareIdHash = user.HardwareIdHash;
                dbUserEntry.LastIp = user.Ip;
                dbUserEntry.LastSocialClubId = user.SocialClubId;

                if (dbUserEntry.CreatedAt == null)
                {
                    await Globals.TTDatabase!.AddAsync(dbUserEntry);
                }
                
                await Globals.TTDatabase!.SaveChangesAsync();
                user.DbUser = dbUserEntry;
                
                Logger.Info($"Account {user.Name} ({user.DbUser!.Id}) connected.");
                
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                user.Kick("Authorization failed");
            }
            

            // Console.WriteLine(response);
            // if (!request || !request.data || !request.data.id || !request.data.username) {
            //     player.kick('Authorization failed');
            //     return;
            // }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
}

public class DiscordApiResponse
{
    public string id { get; set; }
    public string username { get; set; }
    public bool mfa_enabled { get; set; }
}