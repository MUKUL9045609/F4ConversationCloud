using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class ConfigurationController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
