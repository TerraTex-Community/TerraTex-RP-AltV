using System.Text;

namespace TerraTex_RolePlay_AltV_Server.Utils.Helper
{
    static class PasswordHelper
    {
        public static string Hash(string password, string salt)
        {
            byte[] bytes = new UTF8Encoding().GetBytes(salt + password);
            byte[] hashBytes = System.Security.Cryptography.SHA512.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static string GenerateSalt()
        {
            const int length = 128;
            const string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-+.:;?&$!_";
            Random randomizer = new Random();

            StringBuilder bld = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                bld.Append(digits[randomizer.Next(digits.Length - 1)]);
            }
            return bld.ToString();
        }
    }
}
