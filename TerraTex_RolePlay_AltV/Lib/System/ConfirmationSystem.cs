using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Helper;

namespace TerraTex_RolePlay_AltV_Server.Lib.System;

public class ConfirmationSystem
{
    private class GeneratedKey
    {

        public DateTime GeneratedAt { get; }
        public string Code { get; }

        public GeneratedKey(string code)
        {
            Code = code;
            GeneratedAt = DateTime.Now;
        }
    }

    private static readonly Dictionary<TTPlayer, GeneratedKey> GeneratedKeys = new ();

    public static void SendNewGeneratedKey(TTPlayer user, string email)
    {
        string newKey = TextHelper.GenerateRandomString(6);

        if (GeneratedKeys.ContainsKey(user))
        {
            GeneratedKey key = GeneratedKeys[user];
            TimeSpan time = DateTime.Now.Subtract(key.GeneratedAt);
            if (time.TotalMinutes <= 5)
            {
                newKey = key.Code;
            }
            else
            {
                GeneratedKeys.Remove(user);
            }
        }

        if (!GeneratedKeys.ContainsKey(user))
        {
            GeneratedKeys.Add(user, new GeneratedKey(newKey));
        }

        new Email(email, "Bestätigungscode für Accountänderung auf dem TerraTex Roleplay Reallife Server",
                $"Hallo {user.Name}, <br/>" +
                $"Nutze bitte den folgenden Bestätigungscode um die Änderung von Accountdaten auf dem Terratex Roleplay Reallife Server durchzuführen: <br/>" +
                $"<br/>" +
                $"<pre>{newKey}</pre>" +
                $"<br/>" +
                $"Viele Grüße<br/>" +
                $"dein TerraTex-Team"
            ).Send();
    }

    public static bool CheckCodeAndRemoveOnSuccess(TTPlayer user, string code)
    {
        if (!GeneratedKeys.ContainsKey(user)) return false;
        if (!code.Equals(GeneratedKeys[user].Code, StringComparison.CurrentCultureIgnoreCase)) return false;

        GeneratedKeys.Remove(user);

        return true;
    }
    
}