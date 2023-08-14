using AltV.Net;
using AltV.Net.Data;
using TerraTex_RolePlay_AltV_Server.CustomFactories;

namespace TerraTex_RolePlay_AltV_Server.Lib.Helper;

public static class PlayerHelper
{
    public static List<TTPlayer> GetPlayersNearPosition(Position pos, float distance)
    {
        List<TTPlayer> players = new List<TTPlayer>();

        foreach (var player in Alt.GetAllPlayers())
        {
            if (pos.Distance(player.Position) <= distance)
            {
                players.Add((TTPlayer) player);
            }
        }

        return players;
    }
}