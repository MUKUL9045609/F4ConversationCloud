using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.ClientAdmin.Models.AuthViewModel;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Domain.Extension;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    public class AuthController:BaseController
    {
        private readonly IOnboardingService _onboardingService;
        private readonly IAuthRepository _authRepository;
        public AuthController(IOnboardingService onboardingService, IAuthRepository authRepository)
        {
            _onboardingService = onboardingService;
            _authRepository = authRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(ClientLoginViewModel request)
        {
            try
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

                if (response.Data is null && !response.IsSuccess)
                {
                    ModelState.AddModelError("Email", "This Email is not Registered.");
                    return View(request);
                }
                if (response.Data.Password.Decrypt() != request.Password)
                {

                    ModelState.AddModelError("Password", "Please enter a valid password.");
                    return View(request);
                }

                var clientdetails = await _onboardingService.GetCustomerByIdAsync(response.Data.UserId);

                var RoleName = Enum.GetName(typeof(ClientRole), Convert.ToInt32(clientdetails.ClientRoles));

                var userClaims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, clientdetails.FirstName + " " + clientdetails.LastName),
                        new Claim(ClaimTypes.Email, clientdetails.Email),
                        new Claim(ClaimTypes.MobilePhone, clientdetails.PhoneNumber),
                        new Claim(ClaimTypes.Role, RoleName),
                        new Claim(ClaimTypes.NameIdentifier, clientdetails.UserId.ToString()),
                    };
                var claimsIdentity = new ClaimsIdentity(userClaims, "CookieAuthentication");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


                await HttpContext.SignInAsync("CookieAuthentication", claimsPrincipal);

                HttpContext.Session.SetString("Username", clientdetails.Email);
                HttpContext.Session.SetString("ClientMobileNO", clientdetails.PhoneNumber);
                HttpContext.Session.SetInt32("UserId", clientdetails.UserId);
                HttpContext.Session.SetInt32("StageId", (int)clientdetails.Stage);

                var stageValue = HttpContext.Session.GetInt32("StageId");

                ClientFormStage stage = (ClientFormStage)stageValue.Value;

                if (stage == ClientFormStage.ClientRegistered)
                {
                    return RedirectToAction("ClientOnboardingList", "MetaOnboarding");
                }
                else
                {
                    return RedirectToAction("ClientOnboardingList", "MetaOnboarding");

                }
            }
            catch (Exception)
            {
                TempData["WarningMessage"] = "Error";
                return View(request);
            }


        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync("CookieAuthentication");
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }
        [AllowAnonymous]
        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        [AllowAnonymous]
        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                bool success = await _onboardingService.ValidateClientEmailAsync(model.EmailId);

                if (!success)
                {
                    ModelState.AddModelError("EmailId", "This Email is not Registered.");
                    return View(model);
                }

                await _onboardingService.SendResetPasswordLink(model.EmailId);

                TempData["SuccessMessage"] = "A reset link has been sent to your email address";

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        [AllowAnonymous]
        [HttpGet("confirmpassword/{id}")]
        public async Task<IActionResult> ConfirmPassword([FromRoute] string id)
        {
            int userId = 0;

            try
            {
                id = id.Replace("thisisslash", "/").Replace("thisisbackslash", @"\").Replace("thisisplus", "+");
                string decToken = id.Decrypt();

                int.TryParse(decToken.Split("|")[0], out userId);

                if (userId == 0)
                {
                    TempData["ErrorMessage"] = "Invalid Url";

                    return RedirectToAction("InvalidUrl");
                }

                DateTime expiryTime = DateTime.Parse(decToken.Split("|")[1]);

                if (expiryTime < DateTime.UtcNow)
                {
                    TempData["ErrorMessage"] = "Link Has been expired";

                    return RedirectToAction("InvalidUrl");
                }

                return View(new ConfirmPasswordViewModel { UserId = userId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Invalid Url";

                return RedirectToAction("InvalidUrl");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetPassword(ConfirmPasswordViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                    return View(model);

                bool success = await _onboardingService.SetNewPassword(new ConfirmPasswordModel { UserId = model.UserId, Password = model.Password });

                if (!success)
                {
                    TempData["ErrorMessage"] = "Something went wrong!";
                    ModelState.AddModelError(string.Empty, "Something went wrong!");
                    return View(model);
                }

                TempData["SuccessMessage"] = "Password reset successfull";

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "technical error ";

                return View(model);
            }
        }
        
        [AllowAnonymous]
        [HttpGet("invalid-token")]
        public IActionResult InvalidUrl()
        {
            return View();
        }
    }
}
