using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Helpers
{
    public static class OtpGenerator
    {
        public static string GenerateRandomOTP()
        {
            var chars1 = "1234567890";
            var stringChars1 = new char[6];
            var random1 = new Random();

            for (int i = 0; i < stringChars1.Length; i++)
            {
                stringChars1[i] = chars1[random1.Next(chars1.Length)];
            }

            var str = new String(stringChars1);
            return str;
        }
    }
}
