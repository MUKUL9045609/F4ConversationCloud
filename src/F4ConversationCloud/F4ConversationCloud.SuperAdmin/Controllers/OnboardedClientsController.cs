using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class OnboardedClientsController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> List()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
