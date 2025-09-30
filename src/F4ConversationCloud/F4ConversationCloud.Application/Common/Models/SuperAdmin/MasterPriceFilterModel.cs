using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class MasterPriceFilterModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public MasterPriceListViewItem Columns { get; set; } = new MasterPriceListViewItem();
        public IEnumerable<MasterPriceListViewItem> data { get; set; } = Enumerable.Empty<MasterPriceListViewItem>();
        public IEnumerable<SelectListItem>? ConversationTypeList { get; set; }
        public class MasterPriceListViewItem
        {
            [Display(Name = "Sr No")]
            public int SrNo { get; set; }
            public int Id { get; set; }

            [Display(Name = "Conversation Type")]
            public string ConversationType { get; set; } = string.Empty;

            [Display(Name = "Price")]
            public decimal Price { get; set; }

            [Display(Name = "Duration")]
            public DateTime? FromDate { get; set; }

            [Display(Name = "To Date")]
            public DateTime ToDate { get; set; }
        }
    }
}
