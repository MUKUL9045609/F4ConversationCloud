using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace F4ConversationCloud.Domain.Helpers
{
    public static class CommonHelper
    {

        public static string GenerateClientId(int current)
        {      

            current = current + 1;
            string formattedNumber = string.Format("{0:D5}", current);
            string result = $"CL-{formattedNumber}";

            
            return result;
        }
    }
}
