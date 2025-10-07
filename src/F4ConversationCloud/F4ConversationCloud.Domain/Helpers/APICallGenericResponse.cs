using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Helpers
{
    public class APICallGenericResponse
    {
        public bool Status { get; set; }
        public HttpResponseMessage Response { get; set; }
        public string ErrorMessage { get; set; }
    }
}
