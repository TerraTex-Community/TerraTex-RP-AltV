using System.Numerics;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Interfaces;
using TerraTex_RolePlay_AltV_Client.utils;

namespace TerraTex_RolePlay_AltV_Client.User;

[TerraTexClientInit()]
public class Login
{
    private IWebView view;

    public Login()
    {
        Alt.OnServer("Connect:Register", CreateRegistrationWindow);

    }

    private void CreateRegistrationWindow()
    {
        view = Alt.CreateWebView(
            url: "http://resource/client/html/index.html#/register"
        );
        
        Alt.GameControlsEnabled = false;
        Alt.Core.ShowCursor(true);

        view.Visible = true;
        view.Focused = true;
        view.Focus();

        view.On("register:ready", SendNickname);
        view.On<string>("register:submit", SubmitRegistration);
    }

    private void SubmitRegistration(string obj)
    {
        view.Unfocus();
        view.Visible = false;
        view.Remove();
        Alt.EmitServer("register:submit", obj);
    }

    private void SendNickname()
    {
        view.Emit("register:nickname", Alt.LocalPlayer.Name);
    }
}