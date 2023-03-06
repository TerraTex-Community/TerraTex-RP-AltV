using System.Numerics;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Interfaces;
using TerraTex_RolePlay_AltV_Client.utils;

namespace TerraTex_RolePlay_AltV_Client.User;

[TerraTexClientInit()]
public class Login
{
    private IWebView? _view;

    public Login()
    {
        Alt.OnServer("Connect:Register", CreateRegistrationWindow);

    }

    private void CreateRegistrationWindow()
    {
        _view = Alt.CreateWebView(
            url: "http://resource/client/html/index.html#/register"
        );
        
        Alt.GameControlsEnabled = false;
        Alt.Core.ShowCursor(true);

        _view.Visible = true;
        _view.Focused = true;
        _view.Focus();

        _view.On("register:ready", SendNickname);
        _view.On<string>("register:submit", SubmitRegistration);
    }

    private void SubmitRegistration(string obj)
    {
        _view!.Unfocus();
        _view!.Visible = false;
        _view!.Remove();
        Alt.EmitServer("register:submit", obj);
    }

    private void SendNickname()
    {
        _view!.Emit("register:nickname", Alt.LocalPlayer.Name);
    }
}