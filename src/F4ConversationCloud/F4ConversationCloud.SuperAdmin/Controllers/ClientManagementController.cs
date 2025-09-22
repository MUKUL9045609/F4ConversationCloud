using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class ClientManagementController : Controller
    {
        private readonly IClientManagementService _clientManagement;
        public ClientManagementController(IClientManagementService clientManagement)
        {
            _clientManagement = clientManagement;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
