using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using CustomCommandsSystem.Common.Attributes;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Lib.Helper;
using TerraTex_RolePlay_AltV_Server.Utils.CommandAttributes;

namespace TerraTex_RolePlay_AltV_Server.Chat;

public class ChatCommands
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    //
    // [CustomCommand("testcmd")]
    // public void TestCmd(TTPlayer player)
    // {
    //     //Chat.BroadCast("Test Call: " + player.Name + " " + msg);
    //     player.SendChatMessage("test");
    // }
    [CustomCommand("global")]
    public void GlobalChat(TTPlayer player, [CustomCommandRemainingText] string msg)
    {
        Chat.BroadcastChatMessage(player, msg, ChatTypes.Global);
        Logger.Info("[chat:msg:global] " + player.Name + ": " + msg);
    }
}