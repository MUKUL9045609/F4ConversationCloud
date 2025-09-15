using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public class CustomerDetails
    {
        [JsonProperty("data")]
        public CustomerData Data { get; set; }  = new CustomerData();
        public string errorCode { get; set; } = string.Empty;
        public bool Status { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }

    public class CustomerData
    {
        [JsonProperty("ChkCustomerDetailsAPI")]
        public List<ChkCustDetailsItem> ChkCustROAPI { get; set; } = new List<ChkCustDetailsItem>();
    }
    public class ChkCustDetailsItem
    {
        [JsonProperty("CustName")]
        public string CustomerName { get; set; } = string.Empty;

        [JsonProperty("CustAddress")]
        public string CustomerAddress { get; set; } = string.Empty;

        [JsonProperty("CustMobNo")]
        public string CustomerMobile { get; set; } = string.Empty;

    }

}
