using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Domain.Extension;
using F4ConversationCloud.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Models.OnBoardingModel;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
namespace F4ConversationCloud.Onboarding.Controllers
{
    public class OnboardingController:Controller
    {

      
            private readonly IOnboardingService _onboardingService;
            private readonly IAuthRepository _authRepository;
            public OnboardingController(IOnboardingService onboardingService,IAuthRepository authRepository)
            {
                _onboardingService = onboardingService;
                 _authRepository = authRepository;
            }
                public IActionResult Index()
                {
                    return View();
                }
               
                [HttpGet("Login")]
                public async Task<IActionResult> Login()
                {
                
                    return View();
                }

                 [HttpPost("Login")]
                 public async Task<IActionResult> Login(Loginrequest requst)
                 {
                    try
                    {
                        if (!ModelState.IsValid)
                        {
                            return View(requst);
                        }
                         var response = await _onboardingService.OnboardingLogin(requst);
                        if (response.IsSuccess)
                        {
                            if (response.Data.Stage.Equals(ClientFormStage.draft)) {
                                var clientdetails = await _onboardingService.GetCustomerByIdAsync(response.Data.UserId);

                                        clientdetails.TermsCondition = true;
                                    TempData.Put("registrationform", clientdetails);
                                TempData["WarningMessage"] = "You have already registered Please Complete Meta Onboarding !";
                                return RedirectToAction("BankVerification");
                            }
                            else if (response.Data.Stage.Equals(ClientFormStage.metaregistered))
                            {
                                TempData["Info"] = "You have already registered please Wait For Admin Approval !";         
                                return View(requst);
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
                                ModelState.AddModelError(nameof(requst.PassWord), "Invalid Password");
                            }

                            return View(requst);

                         }
                    }
                    catch (Exception)
                    {

                       return View();
                    }
                   return View(requst);
                 }
            
            [HttpGet("register-Client-Info")]
            public async Task<IActionResult> RegisterIndividualAccount()
            {
                
                var step1form = TempData.Get<RegisterUserModel>("registrationform");
                ViewBag.IsReadOnly = false;

                if (step1form != null)
                {
                        var existingData = new RegisterUserModel();
                        existingData = step1form;
                        ViewBag.IsReadOnly = true;
                    return View(existingData);
                }
                
                     var model = new RegisterUserModel { 
                        TimeZones = await _authRepository.GetTimeZonesAsync()
                     };
                

            return View(model);

            }

            [HttpPost("Register-Client-Info")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> RegisterIndividualAccount(RegisterUserModel command)
             {
                try
                { // command.CurrentStep = 1;
                    if (!command.EmailOtpVerified && command.Email != null)
                    {
                        ModelState.AddModelError(nameof(command.EmailOtpVerified), "Please verify your Email before proceeding.");
                    }
                    if (!ModelState.IsValid)
                    {
                        ViewBag.IsReadOnly = false;
                        return View(command);
                    }
                     command.Stage = ClientFormStage.draft;
                    var isregister = await _onboardingService.RegisterUserAsync(command);
                    if (isregister.IsSuccess)
                    {
                       command.UserId = isregister.NewUserId;

                        TempData.Put("registrationform", command);
                        ViewBag.IsReadOnly = true;

                        await _onboardingService.SendRegistrationSuccessEmailAsync(command);

                        TempData["SuccessMessage"] = "Registration successful! Please complete Meta Onboarding !";
                    
                     
                        return RedirectToAction("BankVerification");

                    }
                    else
                    {
                        ViewBag.IsReadOnly = false;
                        TempData["ErrorMessage"] = "Registration failed. Please try again.";
                       return View(command);
                    }

                }
                catch (Exception)
                {

                   return View(command);
                }
            }
            [HttpGet("meta-onboarding")]
            public IActionResult BankVerification()
            {
                var step1form = TempData.Get<RegisterUserModel>("registrationform");
                if (step1form is null)
                {
                    TempData["WarningMessage"] = "Register details first!";
                    return RedirectToAction("RegisterIndividualAccount");
                }
                return View();
            }

            
        public async Task<IActionResult> VarifyMail([FromBody] VarifyMobileNumberModel command)
        {
            // var response = await Mediator.Send(command);

            var response = await _onboardingService.CheckMailOrPhoneNumberAsync(command);

            return Json(new { response.status, response.message });
        }
        public async Task<IActionResult> VerifyOTP([FromBody] ValidateRegistrationOTPModel command)
        {
            var response = await _onboardingService.VerifyOTPAsync(command);
            return Json(new { response.status });
        }

        [HttpPost]
        public async Task<IActionResult> SaveMetaUserConfigurationDetails([FromBody] MetaUsersConfiguration command)
        {
            try
            {
                if (command.PhoneNumberId != null && command.WabaId != null && command.BusinessId != null)
                {
                    var registertemp = TempData.Get<RegisterUserModel>("registrationform");
                    if (registertemp != null)
                    {
                        command.ClientId = registertemp.UserId;

                        var metaresult = await _onboardingService.InsertMetaUsersConfigurationAsync(command);
                        var metaresponse = metaresult.status;

                        bool ConfirmationEmail = await _onboardingService.SendOnboardingConfirmationEmail(new VarifyMobileNumberModel { UserEmailId = registertemp.Email });

                        int UpdateDraft = await _authRepository.UpdateClientFormStageAsync(command.ClientId, ClientFormStage.metaregistered);

                        var message = "success";
                        TempData.Remove("registrationform");
                        return Json(new { result = true, message });
                    }
                    else
                    {
                        var message = "Registration failed please contact to admin!";
                        return Json(new { result = false, message });
                    }

                }
                else
                {
                    var message = " Meta registration failed please contact to admin ";
                    return Json(new { result = false });

                }

            }
            catch (Exception)
            {
                var message = "Technical error! Please Contact To Admin ";
                return Json(new { result = false });
            }
           

        }
        [HttpGet("thank-you")]
        public IActionResult ThankYouPage()
        {
            return View();
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
