using System.ComponentModel;
using AltV.Net;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Data;
using AltV.Net.Client.Elements.Interfaces;

namespace TerraTex_RolePlay_AltV_Client
{
    public class Chat
    {
        private IWebView view;
        private bool loaded = false;
        private bool opened = false;

        private Queue<(string? name, string msg, string? type)> buffer = new();

        public Chat()
        {
            Console.WriteLine("Try to load TerraTex Client side");

            view = Alt.CreateWebView("http://resource/client/html/index.html#/Chat");
            view.On("chat:loaded", ChatLoaded);
            view.On("chat:message", (string msg) => ChatMessage(msg));

            Alt.OnServer<string?, string, string?>("chat:message", PushMessage);
            Alt.OnServer<string,string,string?,bool?>("chat:addAlert", SendAlert);
            Alt.OnKeyUp += ChatKeyUp;

            Console.WriteLine("TerraTex Client side loaded");
        }

        private void SendAlert(string msg, string variant, string? header = null, bool? dismissAble = false)
        {
            view.Emit("chat:addAlert", msg, variant, header, dismissAble);
        }


        private void ChatKeyUp(Key key)
        {
            if (loaded)
            {
                if (!opened && key == Key.T && Alt.GameControlsEnabled)
                {
                    opened = true;
                    view.Emit("chat:open", false);
                    Alt.GameControlsEnabled = false;
                    view.Focus();
                }
                else if (opened && key == Key.Escape)
                {
                    opened = false;
                    view.Emit("chat:close");
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
            (string? name, string msg, string? type) queueElement;
            while (buffer.TryDequeue(out queueElement))
            {
                AddMessage(queueElement.msg, queueElement.name, queueElement.type);
            }

            loaded = true;
            Console.WriteLine("Chat Loaded");
        }

        private void AddMessage(string msg, string? name, string? type = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                view.Emit("chat:addMessage", name, msg, type);
            }
            else
            {
                view.Emit("chat:addString", msg);
            }
        }

        public void PushMessage(string? name, string msg, string? type = "Normal")
        {
            if (!loaded)
            {
                buffer.Enqueue((name, msg, type));
            }
            else
            {
                AddMessage(msg, name, type);
            }
        }

        public void PushLine(string msg)
        {
            PushMessage(null, msg, null);
        }

        
    }
}