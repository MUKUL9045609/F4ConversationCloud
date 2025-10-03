using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class BillingController : BaseController
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
