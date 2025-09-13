

using Konscious.Security.Cryptography;
using System.Text;
namespace F4ConversationCloud.Domain.Helpers
{
    public static class PasswordHasherHelper
    {
        private static string salt = "024ca8e38b33f9116c151123eb432d20";

        public static string HashPassword(string password)
        {
           
            var saltBytes = Encoding.UTF8.GetBytes(salt);

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = saltBytes,
                DegreeOfParallelism = 8,
                MemorySize = 65536,
                Iterations = 4
            };

            byte[] hash = argon2.GetBytes(16);
            string hashedPassword = Convert.ToBase64String(hash);


            return hashedPassword;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                byte[] hashToVerify = Convert.FromBase64String(hashedPassword);
                var saltBytes = Encoding.UTF8.GetBytes(salt);

                var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = saltBytes,
                    DegreeOfParallelism = 8,
                    MemorySize = 65536,
                    Iterations = 4
                };

                byte[] computedHash = argon2.GetBytes(hashToVerify.Length);

                bool isEqual = ConstantTimeEquals(computedHash, hashToVerify);
                return isEqual;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool ConstantTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }
            return result == 0;
        }
    }

}

