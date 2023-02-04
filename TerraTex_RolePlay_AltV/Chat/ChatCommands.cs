using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Attributes;
using TerraTex_RolePlay_AltV_Server.CustomFactories;

namespace TerraTex_RolePlay_AltV_Server.Chat;

public class ChatCommands
{
    [CustomCommand("testcmd")]
    public void TestCmd(TTPlayer player)
    {
        //Chat.BroadCast("Test Call: " + player.Name + " " + msg);
        player.SendChatMessage("test");
    }
}