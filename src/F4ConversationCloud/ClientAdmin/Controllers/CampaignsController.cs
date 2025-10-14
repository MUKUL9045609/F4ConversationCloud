using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    public class CampaignsController : Controller
    {
        public IActionResult CaList()
        {
            return View();
        }
    }
}
