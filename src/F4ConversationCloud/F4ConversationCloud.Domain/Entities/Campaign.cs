using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Entities
{
    public class Campaign
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsActive { get; set; }


    }
}
