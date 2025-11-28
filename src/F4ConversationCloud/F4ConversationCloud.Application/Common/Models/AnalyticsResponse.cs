using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{

    public class AnalyticsResponse
    {
        public TemplateAnalyticsResponse Data { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
    }
    public class TemplateAnalyticsResponse
    {
        public TemplateAnalytics template_analytics { get; set; }
        public string id { get; set; }
    }


    public class Clicked
    {
        public string type { get; set; }
        public string button_content { get; set; }
        public int count { get; set; }
    }

    public class Cost
    {
        public string type { get; set; }
        public double value { get; set; }
    }

   

    public class DataPoint
    {
        public string template_id { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public int sent { get; set; }
        public int delivered { get; set; }
        public int read { get; set; }
        public int replied { get; set; }
        public List<Clicked> clicked { get; set; }
        public List<Cost> cost { get; set; }
    }

    public class Datum
    {
        public string granularity { get; set; }
        public string product_type { get; set; }
        public List<DataPoint> data_points { get; set; }
    }

 

    public class TemplateAnalytics
    {
        public List<Datum> data { get; set; }
        public Paging paging { get; set; }
    }
}
