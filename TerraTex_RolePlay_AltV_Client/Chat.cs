using System.ComponentModel;
using AltV.Net;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Data;
using AltV.Net.Client.Elements.Interfaces;
using TerraTex_RolePlay_AltV_Client.utils;

namespace TerraTex_RolePlay_AltV_Client
{
    [TerraTexClientInit()]
    public class Chat
    {
        private IWebView view;
        private bool loaded = false;
        private bool opened = false;

        private Queue<(string? name, string msg)> buffer = new();

        public Chat()
        {
            Console.WriteLine("Try to load TerraTex Client side");

            view = Alt.CreateWebView("http://resource/client/html/chat/index.html");
            view.On("chatloaded", ChatLoaded);
            view.On("chatmessage", (string msg) => ChatMessage(msg));

            Alt.OnServer("chat:message", (string? name, string msg) => PushMessage(name, msg));
            Alt.OnKeyUp += ChatKeyUp;

            Console.WriteLine("TerraTex Client side loaded");
        }


        private void ChatKeyUp(Key key)
        {
            Console.WriteLine("Key Pressed: " + key);

            if (loaded)
            {
                if (!opened && key == Key.T && Alt.GameControlsEnabled)
                {
                    opened = true;
                    view.Emit("openChat", false);
                    Alt.GameControlsEnabled = false;
                    view.Focus();
                }
                else if (opened && key == Key.Escape)
                {
                    opened = false;
                    view.Emit("closeChat");
                    Alt.GameControlsEnabled = true;
                    view.Unfocus();
                }
            }
        }

        private void ChatMessage(string txt)
        {
            Alt.EmitServer("chat:message", txt);
            opened = false;
            Alt.GameControlsEnabled = true;
            view.Unfocus();
        }

        private void ChatLoaded()
        {
            (string? name, string msg) queueElement;
            while (buffer.TryDequeue(out queueElement))
            {
                AddMessage(queueElement.msg, queueElement.name);
            }

            loaded = true;
            Console.WriteLine("Chat Loaded");
        }

        private void AddMessage(string msg, string? name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                view.Emit("addMessage", name, msg);
            }
            else
            {
                view.Emit("addString", msg);
            }
        }

        public void PushMessage(string? name, string msg)
        {
            if (!loaded)
            {
                buffer.Enqueue((name, msg));
            }
            else
            {
                AddMessage(msg, name);
            }
        }

        public void PushLine(string msg)
        {
            PushMessage(null, msg);
        }

        
    }
}