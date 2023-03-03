using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Enums;
using TerraTex_RolePlay_AltV_Server.CustomFactories;

namespace TerraTex_RolePlay_AltV_Server.Lib.User.SpawnAndDeath;

public class PlayerSpawnManager : IScript
{

    public static void SpawnPlayer(TTPlayer player)
    {
        SetPlayerSkin(player);
        player.Spawn(new Position(259.8162f, -1204.156f, 29.28907f));
    }

    public static void SetPlayerSkin(TTPlayer player)
    {
        // int skin = player.getSyncedData("Skin"); // not implemented yet

        // if (skin == 0)
        // {
            if (player.DbUser!.Gender.Equals("male"))
            {
                player.Model = (uint) PedModel.Michael;
            }
            else
            {
                player.Model = (uint) PedModel.AnitaCutscene;
            }
        // }
        // else
        // {
            // player.setSkin((PedHash)skin);
        // }
    }
}