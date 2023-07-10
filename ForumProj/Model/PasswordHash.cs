using System;
using System.Security.Cryptography;

namespace ForumProj.Model;

public static class PasswordHash
{
    private const int SaltSize = 16;
    private const int HashSize = 20;
    private const int Iterations = 10000;

    public static string HashPassword(string password,out byte[] saltOut)
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
        saltOut = salt;
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
        {
            byte[] hash = pbkdf2.GetBytes(HashSize);

            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
        
            return Convert.ToBase64String(hashBytes);
        }
    }
    public static bool VerifyPassword(string enteredPassword, string storedHash,string saltIn)
    {
        byte[] hashBytes = Convert.FromBase64String(storedHash);

        byte[] salt = Convert.FromBase64String(saltIn);
        
        using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations))
        {
            byte[] hash = pbkdf2.GetBytes(HashSize);

            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}