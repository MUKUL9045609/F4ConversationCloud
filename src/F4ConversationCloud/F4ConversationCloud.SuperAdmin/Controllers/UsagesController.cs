using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class UsagesController : BaseController
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
