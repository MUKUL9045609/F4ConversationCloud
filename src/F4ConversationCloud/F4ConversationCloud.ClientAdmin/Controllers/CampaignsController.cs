using F4ConversationCloud.ClientAdmin.Models.CampaignViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }

        [HttpPost]
        public IActionResult CreateCampaign(CreateCampaignViewModel request)
        {
            if (!ModelState.IsValid) {
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
