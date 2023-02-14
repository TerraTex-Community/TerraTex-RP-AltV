using System.Numerics;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Interfaces;
using TerraTex_RolePlay_AltV_Client.utils;

namespace TerraTex_RolePlay_AltV_Client.User;

[TerraTexClientInit()]
public class Register
{
    private IWebView view;

    public Register()
    {
        Alt.OnServer("Connect:Login", CreateLoginWindow);

    }

    private void CreateLoginWindow()
    {
        view = Alt.CreateWebView(
            url: "http://resource/client/html/index.html#/login"
        );

        
        Alt.GameControlsEnabled = false;
        Alt.Core.ShowCursor(true);

        view.Visible = true;
        view.Focused = true;
        view.Focus();

        view.On("login:ready", SendNickname);
        view.On<string>("login:submit", SubmitLogin);

        Alt.OnServer<bool>("login:result", ReceiveLoginResult);
    }

    private void ReceiveLoginResult(bool result)
    {
        if (result)
        {
            view.Unfocus();
            view.Visible = false;
            view.Remove();
        }
        else
        {
            view.Emit("login:error");
        }
    }

    private void SubmitLogin(string password)
    {
        Alt.EmitServer("login:submit", password);
    }

    private void SendNickname()
    {
        // @todo: set is dev server in future
        view.Emit("login:nickname", Alt.LocalPlayer.Name);
    }
}
