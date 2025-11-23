using System;
using System.Security.Cryptography;

namespace Dev4Tech
{
    public static class SenhasHash
    {
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 32; // 256 bits
        private const int Iterations = 100000;

        public static string HashPassword(string password)
        {
            // Gera um salt aleatório
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Cria o hash da senha
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Combina salt e hash
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Converte para Base64
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerificarSenha(string password, string hashedPassword)
        {
            // Converte de volta para bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Extrai o salt
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Cria o hash da senha fornecida
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Compara os hashes
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }
            return true;
        }
    }
}