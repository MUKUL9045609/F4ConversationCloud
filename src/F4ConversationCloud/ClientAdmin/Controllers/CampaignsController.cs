using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    public class CampaignsController : Controller
    {
        public IActionResult CampaignList()
        {
            return View();
        }
        public IActionResult CreateCampaign()
        {
            return View();
        }

        public IActionResult Guide()
        {
            return View();
        }
    }
}
