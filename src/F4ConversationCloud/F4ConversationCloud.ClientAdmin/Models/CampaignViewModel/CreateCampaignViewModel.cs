using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.ClientAdmin.Models.CampaignViewModel
{
    public class CreateCampaignViewModel
    {
        public int CampaignId { get; set; }

        [Required(ErrorMessage = "Campaign name is required.")]
        public string CampaignName { get; set; }


       
    }
}
