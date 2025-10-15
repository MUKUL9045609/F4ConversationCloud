using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.ClientAdmin.Models.AuthViewModel;
using F4ConversationCloud.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IOnboardingService _onboardingService;
        private readonly IAuthRepository _authRepository;
        public AuthController(IOnboardingService onboardingService, IAuthRepository authRepository)
        {
            _onboardingService = onboardingService;
            _authRepository = authRepository;
        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public async Task<IActionResult> Login()
        {

            return View();
        }


        [HttpGet("Login")]
        public async Task<IActionResult> Login(ClientLoginViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var response = await _onboardingService.OnboardingLogin(new Loginrequest()
            {
                Email = request.Email,
                PassWord = request.Password
            });
            if (response.IsSuccess)
            {
                if (response.Data.Stage.Equals(ClientFormStage.adminapproval))
                {
                    var clientdetails = await _onboardingService.GetCustomerByIdAsync(response.Data.UserId);

                    clientdetails.TermsCondition = true;
                    
                    TempData["WarningMessage"] = "You have already registered Please Complete Meta Onboarding !";
                    return RedirectToAction("BankVerification");
                }
                else if (response.Data.Stage.Equals(ClientFormStage.metaregistered))
                {
                    TempData["Info"] = "You have already registered please Wait For Admin Approval !";
                  
                }

            }
            else
            {
                if (response.Message.Equals("InvalidEmail"))
                {
                    ModelState.AddModelError(nameof(requst.Email), "Invalid Email");
                }
                if (response.Message.Equals("InvalidPassword"))
                {
                    ModelState.AddModelError(nameof(requst.Password), "Invalid Password");
                }

                return View(requst);

            }
            return View(request);
        }
    }
}
