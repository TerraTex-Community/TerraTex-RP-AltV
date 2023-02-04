using AltV.Net;
using AltV.Net.Elements.Entities;
using System.Text.RegularExpressions;

namespace TerraTex_RolePlay_AltV_Server.Chat
{

    public class Chat : IScript
    {

        private static Chat? _instance;

        public Chat()
        {
            Alt.OnClient<IPlayer, string>("chat:message", OnPlayerChatMessage);
            Console.WriteLine("Server side loaded");
            _instance = this;
        }

        private void OnPlayerChatMessage(IPlayer player, string msg)
        {
            msg = msg.Trim();
            if (!msg.StartsWith("/") && msg.Length > 0)
            {
                Alt.Log("[chat:msg] " + player.Name + ": " + msg);

                msg = Regex.Replace(msg, @"<", "&lt;");
                msg = Regex.Replace(msg, @" '", "&#39");
                msg = Regex.Replace(msg, "\"", "&#34");


                Alt.EmitAllClients("chat:message", player.Name, msg);
            }
        }

        public static void Send(IPlayer player, string msg)
        {
            Alt.EmitClients(new[] { player }, "chat:message", null, msg);
        }

        public static void BroadCast(string msg)
        {
            Alt.EmitAllClients("chat:message", null, msg);
        }
    }
}