using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    public class TemplateManagemetController : BaseController
    {
        public TemplateManagemetController()
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
