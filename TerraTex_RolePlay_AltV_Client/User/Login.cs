﻿using System.Numerics;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Interfaces;

namespace TerraTex_RolePlay_AltV_Client.User;

public class Login
{
    private IWebView? _view;

    private string? _maskedEmail;

    public Login()
    {
        Alt.OnServer<string>("Connect:Login", CreateLoginWindow);
    }

    private void CreateLoginWindow(string maskedEmail)
    {
        _maskedEmail = maskedEmail;

        _view = Alt.CreateWebView(
            url: "http://resource/client/html/index.html#/login"
        );
        
        Alt.GameControlsEnabled = false;
        Alt.Core.ShowCursor(true);

        _view.Visible = true;
        _view.Focused = true;
        _view.Focus();

        _view.On("login:ready", SendNickname);
        _view.On<string>("login:submit", SubmitLogin);

        _view.On("login:getMail", GetMaskedEmail);

        _view.On<string, string>("login:changePassword", TryChangePassword);
        _view.On("login:sendConfirmCode", SendConfirmCode);

        Alt.OnServer<bool>("login:result", ReceiveLoginResult);
        Alt.OnServer<bool>("login:passwordForgottenResult", ReceivePasswordForgottenResult);
    }

    private void ReceivePasswordForgottenResult(bool result)
    {
        _view!.Emit("login:passwordForgottenResult", result);
    }

    private void SendConfirmCode()
    {
        Alt.EmitServer("login:sendConfirmationCode");
    }

    private void TryChangePassword(string newPassword, string confirmCode)
    {
        Alt.EmitServer("login:TryChangePassword", newPassword, confirmCode);
    }

    private void GetMaskedEmail()
    {
        _view!.Emit("login:email", _maskedEmail);
    }

    private void ReceiveLoginResult(bool result)
    {
        if (result)
        {
            Alt.GameControlsEnabled = true;
            Alt.Core.ShowCursor(false);
            _view!.Unfocus();
            _view.Visible = false;
            _view.Destroy();
        }
        else
        {
            _view!.Emit("login:error");
        }
    }

    private void SubmitLogin(string password)
    {
        Alt.EmitServer("login:submit", password);
    }

    private void SendNickname()
    {
        // @todo: set is dev server in future 
        _view!.Emit("login:nickname", Alt.LocalPlayer.Name);
    }
}
