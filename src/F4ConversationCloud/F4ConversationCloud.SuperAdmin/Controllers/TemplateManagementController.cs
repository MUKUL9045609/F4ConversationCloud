using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class TemplateManagementController : Controller
    {
        private readonly ITemplateManagementService _templateManagementService;
        public TemplateManagementController(ITemplateManagementService templateManagementService)
        {
            _templateManagementService = templateManagementService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("create-template")]
        public IActionResult CreateTemplate()
        {
            return View();
        }
    }
}
