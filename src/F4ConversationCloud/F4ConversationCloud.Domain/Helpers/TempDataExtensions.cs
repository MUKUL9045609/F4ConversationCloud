using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace F4ConversationCloud.Domain.Helpers
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

      
        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            if (!tempData.TryGetValue(key, out var o)) return null;
            var value = o as string;
            if (value == null) return null;
            tempData.Keep(key); 
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
