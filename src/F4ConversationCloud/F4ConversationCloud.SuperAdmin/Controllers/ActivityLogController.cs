using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class ActivityLogController : BaseController
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
