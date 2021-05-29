using System;
using System.Security.Cryptography;

namespace MvcRDMG.Core.Helpers
{
    public static class PasswordHelper
    {
        public static string EncryptPassword(string password)
        {
            var result = "";

            using (var rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var salt = new byte[16];
                rNGCryptoServiceProvider.GetBytes(salt);
                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                var hash = pbkdf2.GetBytes(20);
                var hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                result = Convert.ToBase64String(hashBytes);
            }

            return result;
        }

        public static bool CheckPassword(string savedPasswordHash, string password)
        {
            var result = true;
            var hashBytes = Convert.FromBase64String(savedPasswordHash);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(20);

            for (var i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}
