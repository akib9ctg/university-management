using System.Security.Cryptography;

namespace UniversityManagement.Application.Common.Utilities
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: salt,
                iterations: 100000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32
            );

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string storedHash, string inputPassword)
        {
            try
            {
                var parts = storedHash.Split('.', 2);
                if (parts.Length != 2)
                    return false;

                var salt = Convert.FromBase64String(parts[0]);
                var stored = parts[1];

                byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                    password: inputPassword,
                    salt: salt,
                    iterations: 100000,
                    hashAlgorithm: HashAlgorithmName.SHA256,
                    outputLength: 32
                );

                return stored == Convert.ToBase64String(hash);
            }
            catch
            {
                return false;
            }
        }
    }

}
