using F4ConversationCloud.ClientAdmin.Models.CampaignViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twilio.Annotations;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    [Authorize]
    public class CampaignsController : BaseController
    {
        public IActionResult CampaignList()
        {
            return View();
        }
        public IActionResult CreateCampaign()
        {
            CreateCampaignViewModel createCampaignView = new CreateCampaignViewModel();

            return View(createCampaignView);
        }

        [HttpPost]
        public IActionResult CreateCampaign(CreateCampaignViewModel request)
        {
            if (!ModelState.IsValid) {
                request.CurrentTab = "Preview";
                return View(request);
            }
            
            return View(request);
        }
        public IActionResult Guide()
        {
            return View();
        }
    }
}
