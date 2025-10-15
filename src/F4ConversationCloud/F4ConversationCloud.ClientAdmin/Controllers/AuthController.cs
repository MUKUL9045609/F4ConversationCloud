using F4ConversationCloud.ClientAdmin.Models.AuthViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    public class AuthController : Controller
    {
        
        
        [AllowAnonymous]
        [HttpGet("Login")]
        public async Task<IActionResult> Login()
        {

            return View();
        }
        

        /*[HttpGet("Login")]
        public async Task<IActionResult> Login(LoginViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var response = await _onboardingService.OnboardingLogin(new Loginrequest()
            {
                Email = requst.Email,
                PassWord = requst.Password
            });
            return View(request);
        }*/
    }
}
