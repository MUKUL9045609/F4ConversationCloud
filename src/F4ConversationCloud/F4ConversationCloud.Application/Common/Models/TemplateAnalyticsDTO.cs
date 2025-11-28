using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public class TemplateAnalyticsDTO
    {
        public int TemplateRead { get; set; }
        public int TemplateSend { get; set; }
        public int TemplateDelivered { get; set; }
        public int TemplateReplied { get; set; }
        public decimal TemplateAmountSpent { get; set; }
        public decimal TemplateCostPerDelivered { get; set; }
        public decimal TemplateCostPerUrlButtonClick { get; set; }
        public int TemplateUrlButtonClickCount { get; set; }
        public int TemplateUniqueUrlButtonClickCount { get; set; }
        public DateTime TemplateSendFrom { get; set; }
        public DateTime TemplateSendTo { get; set; }
        public string WABAID { get; set; }
        public int ClientInfoId { get; set; }
        public int TemplateId { get; set; }
    }
}
