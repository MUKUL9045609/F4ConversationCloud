using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.ClientAdmin.Models.AuthViewModel;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Domain.Extension;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    public class AuthController :Controller
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
                if (response.IsSuccess)
                {
                    //if (response.Data.Stage.Equals(ClientFormStage.metaregistered))
                    //{
                        var clientdetails = await _onboardingService.GetCustomerByIdAsync(response.Data.UserId);

                        var userClaims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, clientdetails.FirstName + " " + clientdetails.LastName),
                            new Claim(ClaimTypes.Email, clientdetails.Email),
                            new Claim(ClaimTypes.MobilePhone, clientdetails.PhoneNumber),
                            new Claim(ClaimTypes.Role, clientdetails.Role),
                            new Claim(ClaimTypes.NameIdentifier, clientdetails.UserId.ToString()),
                        };
                        var claimsIdentity = new ClaimsIdentity(userClaims, "CookieAuthentication");
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    

                        await HttpContext.SignInAsync("CookieAuthentication", claimsPrincipal);
                       
                        HttpContext.Session.SetString("Username", clientdetails.Email);
                        HttpContext.Session.SetString("ClientMobileNO", clientdetails.PhoneNumber);
                        HttpContext.Session.SetInt32("UserId", clientdetails.UserId);
                        HttpContext.Session.SetInt32("StageId", (int)clientdetails.Stage);

                        TempData["WarningMessage"] = "Welcome";
                         var stageValue = HttpContext.Session.GetInt32("StageId");
                         
                                ClientFormStage stage = (ClientFormStage)stageValue.Value;

                                if (stage == ClientFormStage.draft)
                                {
                                    return RedirectToAction("ClientOnboardingList", "MetaOnboarding");
                                }
                                else {
                                    return RedirectToAction("ClientOnboardingList", "MetaOnboarding");

                                }



                    //}


                }
                else
                {
                    TempData["WarningMessage"] = "error";
                    return View(request);
                }
                return View(request);
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

                    return RedirectToAction("Login");
                }

                DateTime expiryTime = DateTime.Parse(decToken.Split("|")[1]);

                if (expiryTime < DateTime.UtcNow)
                {
                    TempData["ErrorMessage"] = "Link Has been expired";

                    return RedirectToAction("Login");
                }

                return View(new ConfirmPasswordViewModel { UserId = userId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Invalid Url";

                return RedirectToAction("Login");
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

    }
}
