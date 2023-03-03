using System.Text.RegularExpressions;
using System.Text;

namespace TerraTex_RolePlay_AltV_Server.Helper;

static class TextHelper
{
    public static string RemoveColorStrings(string text)
    {
        Regex rgx = new Regex("~[A-Za-z]~|~#[0-9A-Za-z]{3,6}~");
        return rgx.Replace(text, "");
    }

    public static string GenerateRandomString(int length, bool onlyCapitalLetters = true)
    {
        string digits = onlyCapitalLetters
            ? "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            : "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        Random randomizer = new Random();

        StringBuilder bld = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            bld.Append(digits[randomizer.Next(digits.Length - 1)]);
        }
        return bld.ToString();
    }
}