using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Exceptions
{
    public class WhatsappBusinessCloudAPIException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public WhatsAppErrorResponse whatsAppErrorResponse { get; set; }

        public WhatsappBusinessCloudAPIException()
        {

        }

        public WhatsappBusinessCloudAPIException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public WhatsappBusinessCloudAPIException(Exception ex, HttpStatusCode statusCode, WhatsAppErrorResponse whatsAppError) : base(ex.Message)
        {
            StatusCode = statusCode;
            whatsAppErrorResponse = whatsAppError;
        }
    }
}
