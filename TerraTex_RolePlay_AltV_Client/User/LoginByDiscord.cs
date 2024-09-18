using AltV.Net.Client;

namespace TerraTex_RolePlay_AltV_Client.User;

public class LoginByDiscord
{
    public LoginByDiscord()
    {
        Alt.OnServer("Connect:LoginByDiscord", LoadDiscordLogin);
    }

    private async void LoadDiscordLogin()
    {
        Alt.Natives.SwitchToMultiFirstpart(Alt.LocalPlayer.ScriptId, 0, 2);

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

        // Enter the client id here
        // const DISCORD_APP_ID = '1285886826841640981';
        //
        // async function getOAuthToken() {
        //     try {
        //         const token = await alt.Discord.requestOAuth2Token(DISCORD_APP_ID);
        //         alt.emitServer('token', token);
        //     } catch (e) {
        //         // Error can be due invalid app id, discord server issues or the user denying access.
        //         alt.logError(e);
        //     }
        // }
        //
        // getOAuthToken();
    }
    
}