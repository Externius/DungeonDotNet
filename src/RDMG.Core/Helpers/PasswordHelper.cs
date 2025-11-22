using System.Security.Cryptography;

namespace RDMG.Core.Helpers;

public static class PasswordHelper
{
    private const int Iterations = 10000;
    private const int HashLength = 20;
    private const int SaltLength = 16;
    private static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA512;

    public static string EncryptPassword(string password)
    {
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        var salt = new byte[SaltLength];
        randomNumberGenerator.GetBytes(salt);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, HashLength);
        var hashBytes = new byte[SaltLength + HashLength];
        Array.Copy(salt, 0, hashBytes, 0, SaltLength);
        Array.Copy(hash, 0, hashBytes, SaltLength, HashLength);
        return Convert.ToBase64String(hashBytes);
    }

    public static bool CheckPassword(string savedPasswordHash, string password)
    {
        var result = true;
        var hashBytes = Convert.FromBase64String(savedPasswordHash);
        var salt = new byte[SaltLength];
        Array.Copy(hashBytes, 0, salt, 0, SaltLength);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, HashLength);

        for (var i = 0; i < HashLength; i++)
        {
            if (hashBytes[i + SaltLength] == hash[i])
                continue;
            result = false;
            break;
        }

        return result;
    }
}