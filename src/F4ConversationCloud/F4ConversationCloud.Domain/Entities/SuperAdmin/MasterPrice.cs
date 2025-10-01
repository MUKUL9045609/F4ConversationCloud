using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Entities.SuperAdmin
{
    public class MasterPrice
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public int ConversationType { get; set; }
        public decimal Price { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
