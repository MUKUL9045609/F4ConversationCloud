using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Models.OnBoardingModel;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Domain.Extension;
using F4ConversationCloud.Domain.Helpers;
using F4ConversationCloud.Onboarding.Models;
using Microsoft.AspNetCore.Mvc;
using Onboarding.Models;
using System;
namespace F4ConversationCloud.Onboarding.Controllers
{
    public class OnboardingController : Controller
    {
        private readonly IOnboardingService _onboardingService;
        private readonly IAuthRepository _authRepository;
        private IConfiguration _configuration { get; }
        public OnboardingController(IOnboardingService onboardingService, IAuthRepository authRepository, IConfiguration configuration)
        {
            _onboardingService = onboardingService;
            _authRepository = authRepository;
            _configuration = configuration;
        }

        [HttpGet("Id={id}")]
        public async Task<IActionResult>  Index([FromRoute] string id)
        {

            string DecryptId = id.ToString().Decrypt();
            int Userid = Convert.ToInt32(DecryptId);

            try
            {
                var clientdetails = await _onboardingService.GetCustomerByIdAsync(Userid);
                
                var command = new RegisterUserViewModel
                {
                    UserId = clientdetails.UserId,
                    FirstName = clientdetails.FirstName,
                    LastName = clientdetails.LastName,
                    Email = clientdetails.Email,
                    PhoneNumber = clientdetails.PhoneNumber,
                    Stage = clientdetails.Stage
                };

                TempData.Put("registrationform", command);
              

                return View();

            }
            catch (Exception)
            {

                return View();
            }
            
           
           
           
        }

        [HttpGet("register-Client-Info")]
        public async Task<IActionResult> RegisterIndividualAccount()
        {

            var step1form = TempData.Get<RegisterUserViewModel>("registrationform");
            ViewBag.IsReadOnly = false;
            //step1form = null;
            if (step1form != null)
            {
                var existingData = new RegisterUserViewModel
                {
                    TimeZones = await _authRepository.GetTimeZonesAsync(),
                    Cities = await _authRepository.GetCitiesAsync(),
                    States = await _authRepository.GetStatesAsync(),
                    FirstName = step1form.FirstName + step1form.LastName,               
                    Email = step1form.Email,
                    PhoneNumber = step1form.PhoneNumber,

                };
                ViewBag.IsReadOnly = true;
                
                return View(existingData);
            }
           
            var model = new RegisterUserViewModel
            {
                TimeZones = await _authRepository.GetTimeZonesAsync(),
                Cities = await _authRepository.GetCitiesAsync(),
                States = await _authRepository.GetStatesAsync(),
            };


            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> GetCitiesByStateId(int stateId)
        {
            try
            {
                var cities = await _authRepository.GetCitiesByStatesIdAsync(stateId);
                return Json(cities.Select(c => new { id = c.Id, name = c.Name }));
            }
            catch (Exception)
            {
                return Json(new { message = "Technical Error!" });
            }
        }


        [HttpPost("Register-Client-Info")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterIndividualAccount(RegisterUserViewModel command)
        {
            try
            {
                if (!command.PhoneNumberOtpVerified)
                {
                    ModelState.AddModelError(nameof(command.PhoneNumber), "Please verify your Contact Number before proceeding.");
                }
                var ClientTempData = TempData.Get<RegisterUserViewModel>("registrationform");
                if (!ModelState.IsValid)
                {
                    ViewBag.IsReadOnly = true;
                    
                        command.FirstName = ClientTempData.FirstName;
                        command.LastName = ClientTempData.LastName;
                        command.Email = ClientTempData.Email;
                        command.PhoneNumber = ClientTempData.PhoneNumber;
                        command.TimeZones = await _authRepository.GetTimeZonesAsync();
                        command.Cities = await _authRepository.GetCitiesAsync();
                        command.States = await _authRepository.GetStatesAsync();
                    return View(command);
                }

                int TotalRegisteredClient = await _authRepository.GetRegisteredClientCountAsync();

                command.Stage = ClientFormStage.draft;
                var registerRequest = new RegisterUserModel
                {
                   
                    UserId = ClientTempData.UserId,
                    Address = command.Address,
                    Country = command.Country,
                    Timezone = command.Timezone,
                    CityId= command.CityId,
                    StateId= command.StateId,
                    ZipCode= command.ZipCode,
                    OptionalAddress = command.OptionalAddress,
                    OrganizationsName = command.OrganizationsName,
                    PassWord = PasswordHasherHelper.HashPassword(command.PassWord),
                    IsActive = command.IsActive,
                    Stage = command.Stage,
                    Role = ClientRole.Admin,
                    RegistrationStatus = ClientRegistrationStatus.Pending,
                    ClientId = CommonHelper.GenerateClientId(TotalRegisteredClient)
                };
                
                var isregister = await _onboardingService.RegisterUserAsync(registerRequest);
                if (isregister.IsSuccess)
                {
                    command.UserId = isregister.NewUserId;

                    TempData["SuccessMessage"] = "Registration successful! Please complete Meta Onboarding !";

                    string RedirecttoClientAppLoginPage = _configuration["ClientAppUrlPath:LoginPath"];
                    return Redirect(RedirecttoClientAppLoginPage);

                }
                else
                {

                    command.FirstName = ClientTempData.FirstName;
                    command.LastName = ClientTempData.LastName;
                    command.Email = ClientTempData.Email;
                    command.PhoneNumber = ClientTempData.PhoneNumber;
                    command.TimeZones = await _authRepository.GetTimeZonesAsync();
                    command.Cities = await _authRepository.GetCitiesAsync();
                    command.States = await _authRepository.GetStatesAsync();
                    ViewBag.IsReadOnly = true;
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
            var step1form = TempData.Get<RegisterUserViewModel>("registrationform");
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
            if ( command.UserEmailId != null)
            {
                ModelState.AddModelError(nameof(command.UserEmailId), "Please verify your Email before proceeding.");
            }
            var response = await _onboardingService.CheckIsMailExitsAsync(command);

            return Json(new { response.status, response.message });
        }

        public async Task<IActionResult> VarifyWhatsAppNumber( VarifyMobileNumberModel command)
        {
            if (string.IsNullOrWhiteSpace(command.UserPhoneNumber))
            {
                return Json(new { status = false, message = "Please enter a valid WhatsApp number." });
            }

            var response = await _onboardingService.VarifyWhatsAppContactNoAsync(command);
            return Json(new { response.status, response.message });
        }



        [HttpGet("Login")]
        public async Task<IActionResult> Login()
        {

            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestModel requst)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(requst);
                }
                var response = await _onboardingService.OnboardingLogin(new Loginrequest()
                {
                    Email = requst.Email,
                    PassWord = requst.Password
                });

                if (response.IsSuccess)
                {
                    if (response.Data.Stage.Equals(ClientFormStage.draft))
                    {
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
                        ModelState.AddModelError(nameof(requst.Password), "Invalid Password");
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


        //public async Task<IActionResult> VerifyOTP(ValidateRegistrationOTPModel command)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError(nameof(command.OTP));
        //    }
        //    if (string.IsNullOrWhiteSpace(command.OTP))
        //    {
        //        return Json(new { status = false, message = "Enter A Valid OTP" });
        //    }
        //    var response = await _onboardingService.VerifyOTPAsync(command);
        //    return Json(new { response.status, response.message });
        //}
        public async Task<IActionResult> VerifyOTP(ValidateRegistrationOTPModel command)
        {
          
            if (!ModelState.IsValid)
            {
                
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new
                {
                    status = false,
                    message = errors.Any() ? string.Join(", ", errors) : "Invalid input."
                });
            }

            
            if (string.IsNullOrWhiteSpace(command.OTP))
            {
                return Json(new
                {
                    status = false,
                    message = "Enter a valid OTP"
                });
            }

            
            var response = await _onboardingService.VerifyOTPAsync(command);

            return Json(new
            {
                status = response.status,
                message = response.message
            });
        }

        [HttpPost]
        public async Task<IActionResult> SaveMetaUserConfigurationDetails([FromBody] MetaUsersConfiguration command)
        {
            try
            {
                if (command.PhoneNumberId != null && command.WabaId != null && command.BusinessId != null)
                {
                    var registertemp = TempData.Get<RegisterUserViewModel>("registrationform");
                    if (registertemp != null)
                    {
                        command.ClientInfoId = registertemp.UserId;

                        var metaresult = await _onboardingService.InsertMetaUsersConfigurationAsync(command);


                        bool ConfirmationEmail = await _onboardingService.SendOnboardingConfirmationEmail(new VarifyMobileNumberModel { UserEmailId = registertemp.Email });

                            int UpdateDraft = await _authRepository.UpdateClientFormStageAsync(command.ClientInfoId, ClientFormStage.metaregistered);

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
