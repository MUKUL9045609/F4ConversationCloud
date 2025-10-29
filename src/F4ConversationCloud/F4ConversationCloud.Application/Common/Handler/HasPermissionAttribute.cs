using Microsoft.AspNetCore.Authorization;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Handler
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        private const string PolicyPrefix = "Permission_";

        public HasPermissionAttribute(string permission)
        {
            Policy = $"{PolicyPrefix}{permission}";
        }
    }
}
