using System.Numerics;
using AltV.Net;
using AltV.Net.Elements.Entities;
using System.Text.RegularExpressions;
using AltV.Net.Data;
using TerraTex_RolePlay_AltV_Server.Lib.Helper;

namespace TerraTex_RolePlay_AltV_Server.Chat
{

    public class Chat : IScript
    {
        private static float ChatDistance = 20;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
                Logger.Info("[chat:msg] " + player.Name + ": " + msg);

                msg = Regex.Replace(msg, @"<", "&lt;");
                msg = Regex.Replace(msg, @" '", "&#39");
                msg = Regex.Replace(msg, "\"", "&#34");

                var pos = player.Position;

                SendChatMessage(PlayerHelper.GetPlayersNearPosition(pos, ChatDistance).ToArray(), player, msg);
            }
        }

        public static void SendChatMessage(IPlayer receiver, IPlayer sender, string msg, ChatTypes? type = null)
        {
            receiver.Emit("chat:message", sender.Name, msg,
                type != null ? type.ToString() : null);
        }

        public static void SendChatMessage(IPlayer[] receiver, IPlayer sender, string msg, ChatTypes? type = null)
        {
            Alt.EmitClients(receiver, "chat:message", sender.Name, msg,
                type != null ? type.ToString() : null);
        }

        public static void BroadcastChatMessage(IPlayer sender, string msg, ChatTypes? type = null)
        {
            Alt.EmitAllClients("chat:message", sender.Name, msg,
                type != null ? type.ToString() : null);
        }

        // @todo: extend by message type or color?
        public static void Send(IPlayer player, string msg)
        {
            player.Emit("chat:message", null, msg);
        }

        public static void BroadCast(string msg)
        {
            Alt.EmitAllClients("chat:message", null, msg);
        }
        

        public static void SendAlert(IPlayer player, string msg, AlertType alertVariant, string? header = null,
            bool? dismissable = null)
        {
            player.Emit("chat:addAlert", msg, alertVariant.ToString().ToLower(), header, dismissable);
        }

        public static void BroadCastAlert(string msg, AlertType alertVariant, string? header = null,
            bool? dismissable = null)
        {
            Alt.EmitAllClients("chat:addAlert", msg, alertVariant.ToString().ToLower(), header, dismissable);
        }
    }
}