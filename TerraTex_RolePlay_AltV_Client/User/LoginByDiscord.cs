using AltV.Net.Client;

namespace TerraTex_RolePlay_AltV_Client.User;

public class LoginByDiscord
{
    public LoginByDiscord()
    {
        Alt.OnServer("Connect:LoginByDiscord", LoadDiscordLogin);
        Alt.OnServer("Connect:StartCharacterSelection", StartCharacterSelection);
        Alt.OnServer("Connect:SpawnReady", SpawnReady);
    }

    // @todo: Add Character Data to function (Names, IDs, Can New Char be created?)
    private async void StartCharacterSelection()
    {
        Console.WriteLine("Start Spawn temporary");
        
        Alt.Natives.SwitchToMultiFirstpart(Alt.LocalPlayer.ScriptId, 0, 1);
        await AwaitMoveCam();
        
        LoadingScreen.GetLastActiveLoadingScreen()!.Visible = false;
        LoadingScreen.GetLastActiveLoadingScreen()!.Destroy();
        
        new Chat();
        
        
        // @todo: add selected char
        Alt.EmitServer("Connect:SelectedChar");
    }

    private async void SpawnReady()
    {
        Alt.GameControlsEnabled = true;
        
        Alt.Natives.SwitchToMultiSecondpart(Alt.LocalPlayer.ScriptId);
    }

    async Task AwaitMoveCam()
    {
        Console.WriteLine("State:" + Alt.Natives.IsSwitchToMultiFirstpartFinished());
        while (Alt.Natives.IsSwitchToMultiFirstpartFinished())
        {
            await Task.Delay(2000);
        }
        await Task.Delay(1000);
    }
    
    private async void LoadDiscordLogin()
    {
        
        Alt.Natives.SwitchToMultiFirstpart(Alt.LocalPlayer.ScriptId, 0, 1);
        await AwaitMoveCam();
        Alt.GameControlsEnabled = false;

        var applicationId = "1285886826841640981"; // should that really be hardcoded?

        try
        {
            var token= await Alt.Discord.RequestOAuth2Token(applicationId);
            Alt.EmitServer("Discord:Token", token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
}