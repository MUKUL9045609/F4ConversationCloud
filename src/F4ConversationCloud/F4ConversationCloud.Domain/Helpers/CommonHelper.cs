using Microsoft.AspNetCore.Http;
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

        public static async Task<string> GenerateFileToBase64String(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(fileBytes);

                string dataUri = $"data:{file.ContentType};base64,{base64String}";
                return dataUri;
            }
        }
    }
}
