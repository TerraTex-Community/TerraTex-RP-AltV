using AltV.Net;
using System.Text;
using AltV.Net.Elements.Entities;
using NLog.Targets;
using TerraTex_RolePlay_AltV_Server.Lib.System.ConsoleInput;

namespace TerraTex_RolePlay_AltV_Server.Lib.Admin;

public class GeneralConsoleCommands : IScript
{
    [ConsoleCommand("players")]
    public void PlayersCmd()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Players Online ");

        int maxPlayers = (int) Alt.GetServerConfig().Get("players").GetInt()!;
        
        var players = Alt.GetAllPlayers();

        sb.Append(players.Count + "/" + maxPlayers + ": ");

        foreach (IPlayer client in players)
        {
            sb.Append(client.Name);
            if (players.Last() != client)
            {
                sb.Append(", ");
            }
        }

        Console.WriteLine(sb.ToString());
    }
}