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

        public IActionResult Guide()
        {
            return View();
        }
    }
}
