using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Extension;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ISuperAdminAuthService _superAdminAuthService;
        public AuthController(ISuperAdminAuthService superAdminAuthService)
        {
            _superAdminAuthService = superAdminAuthService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            try
            {
                var viewModel = new LoginViewModel();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                Auth user = await _superAdminAuthService.CheckUserExists(model.UserName);

                if (user is null)
                {
                    ModelState.AddModelError("UserName", "This Email is not Registered.");
                    return View(model);
                }

                if (user.Password.Decrypt() != model.Password)
                {
                    ModelState.AddModelError("Password", "Please enter a valid password.");
                    return View(model);
                }

                if (!user.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "Please contact Administrator.");
                    return View(model);
                }

                var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.MobileNo),
                new Claim(ClaimTypes.Role, user.RoleName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

                var userIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });

                await HttpContext.SignInAsync(userPrincipal);

                return RedirectToAction("List", "ClientManagement");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                bool success = await _superAdminAuthService.ValidateUserName(model.UserName);

                if (!success)
                {
                    ModelState.AddModelError("UserName", "This Email is not Registered.");
                    return View(model);
                }

                await _superAdminAuthService.SendPasswordResetLink(model.UserName);

                TempData["SuccessMessage"] = "A reset link has been sent to your email address";

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPassword(ConfirmPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                bool success = await _superAdminAuthService.ConfirmPassword(new ConfirmPasswordModel
                {
                    UserId = model.UserId,
                    Password = model.Password.Encrypt()
                });

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
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IActionResult AccessDenied()
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
