namespace TerraTex_RolePlay_AltV_Server.Helper;

public class EmailHelper
{
    public static string MaskPlayerEmail(string email)
    {
        string[] parts = email.Split("@");

        string firstPart = parts[0].Substring(0, 2) + "***" + parts[0].Substring(parts[0].Length - 2);

        return $"{firstPart}@{parts[1]}";
    }
}