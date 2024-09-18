

using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Entities;
using AltV.Net.Client.Elements.Interfaces;

public static class LoadingScreen
{
    private static IWebView? _activeScreen;
    
    public static void ShowDiscordLoadingScreen()
    {
        _activeScreen = Alt.CreateWebView(
            url: "http://resource/client/html/index.html#/LoadingScreen",
            isOverlay: false,
            pos: Vector2.Zero,
            size: Alt.ScreenResolution
        );
        _activeScreen.Visible = true;
    }

    public static IWebView? GetLastActiveLoadingScreen()
    {
        return _activeScreen;
    }
}