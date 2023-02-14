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
        Alt.OnServer("Connect:Register", CreateRegistrationWindow);

    }

    private void CreateRegistrationWindow()
    {
        Vector2 resolution = Alt.Core.ScreenResolution;

        float width = (float)800;
        if (resolution.X <= 800) width = resolution.X;

        float height = (float)750;
        if (resolution.Y <= 750) height = resolution.Y;

        float x = (float) (resolution.X - width) / 2;
        float y = (float) (resolution.Y - height) / 2;

        view = Alt.CreateWebView(
            url: "http://resource/client/html/index.html#/register"
            // pos: new Vector2(x, y),
            // size: new Vector2(width, height)
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