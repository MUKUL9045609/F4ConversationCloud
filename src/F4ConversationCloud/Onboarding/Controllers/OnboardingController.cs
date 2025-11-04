using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;

using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Domain.Extension;
using F4ConversationCloud.Domain.Helpers;
using F4ConversationCloud.Onboarding.Models;
using Microsoft.AspNetCore.Mvc;
using Onboarding.Models;
using System;
using Twilio.Jwt.AccessToken;
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

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string token)
        {

            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    TempData["ErrorMessage"] = "This link is no longer active for security reasons .";
                    return RedirectToAction("InvalidUrl");
                }
                var decrypted = token.Decrypt();

             

                string[] tokenParts = decrypted.Split("|");
                string stringUserid = tokenParts[0];
               
                
                if (tokenParts.Length != 2)
                {
                    TempData["ErrorMessage"] = "This link is no longer active for security reasons.";
                    return RedirectToAction("InvalidUrl");
                }
                DateTime expiryTime = DateTime.Parse(tokenParts[1]);
                if (expiryTime < DateTime.UtcNow)
                {
                    TempData["ErrorMessage"] = "This link is no longer active for security reasons.";
                    return RedirectToAction("InvalidUrl");
                }
                int UserId = Convert.ToInt32(stringUserid);
                HttpContext.Session.SetInt32("UserId", UserId);
                var clientdetails = await _onboardingService.GetCustomerByIdAsync(UserId);
                if (clientdetails == null)
                {
                    TempData["ErrorMessage"] = "This link is no longer active for security reasons.";
                    return RedirectToAction("InvalidUrl");
                }
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
              

                return View(command);

            }
            catch (Exception)
            {

                TempData["ErrorMessage"] = "This link is no longer active for security reasons.";
                return RedirectToAction("InvalidUrl");
            }

        }

        [HttpGet("register-Client-Info")]
        public async Task<IActionResult> RegisterIndividualAccount()
        {
            var step1form = TempData.Get<RegisterUserViewModel>("registrationform");
            var clientdetails = await _onboardingService.GetCustomerByIdAsync(step1form.UserId);
            if (clientdetails.Stage == ClientFormStage.ClientRegistered)
            {
                var clientinfo = new RegisterUserViewModel
                {
                    FirstName = clientdetails.FirstName+" "+clientdetails.LastName,
                    Email=clientdetails.Email,
                    PhoneNumber= clientdetails.PhoneNumber,
                    Address = clientdetails.Address,
                    Country = clientdetails.Country,
                    Timezone = clientdetails.Timezone,
                    CityId = clientdetails.CityName,
                    StateId = clientdetails.StateName,
                    ZipCode = clientdetails.ZipCode,
                    OptionalAddress = clientdetails.OptionalAddress,
                    OrganizationsName = clientdetails.OrganizationsName,
                    TermsCondition=true,
                };
                ViewBag.IsReadOnly = true;
                ViewBag.DisableButtons = true;
                TempData["WarningMessage"] = "You have already registered Please Complete Meta Onboarding !";
                return View(clientinfo);
            }
            
            
            if (step1form != null)
            {
                var existingData = new RegisterUserViewModel
                {
                    TimeZones = await _authRepository.GetTimeZonesAsync(),
                    Cities = await _authRepository.GetCitiesAsync(),
                    States = await _authRepository.GetStatesAsync(),
                    FirstName =step1form.FirstName +" "+ step1form.LastName,               
                    Email = step1form.Email,
                    PhoneNumber = step1form.PhoneNumber,

                };
                ViewBag.IsReadOnly = true;
                ViewBag.DisableButtons = false;
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

        [HttpPost("Register-Client-Info")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterIndividualAccount(RegisterUserViewModel command)
        {
            try
            {
                var ClientTempData = TempData.Get<RegisterUserViewModel>("registrationform");
                
                if (!command.PhoneNumberOtpVerified)
                {
                    ModelState.AddModelError(nameof(command.PhoneNumber), "Please verify your Contact Number before proceeding.");
                }
               
                if (!ModelState.IsValid)
                {
                    ViewBag.IsReadOnly = true;
                    ViewBag.DisableButtons = false;
                        command.FirstName = ClientTempData.FirstName + " " + ClientTempData.LastName;
                        command.LastName = ClientTempData.LastName;
                        command.Email = ClientTempData.Email;
                        command.PhoneNumber = ClientTempData.PhoneNumber;
                        command.TimeZones = await _authRepository.GetTimeZonesAsync();
                        command.Cities = await _authRepository.GetCitiesAsync();
                        command.States = await _authRepository.GetStatesAsync();
                    return View(command);
                }

                int TotalRegisteredClient = await _authRepository.GetRegisteredClientCountAsync();

               
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
                    PassWord = command.PassWord.Encrypt(),
                    IsActive = command.IsActive,
                    Stage = ClientFormStage.ClientRegistered,
                    Role = ClientRole.Admin,
                    RegistrationStatus = ClientRegistrationStatus.Pending,
                    ClientId = CommonHelper.GenerateClientId(TotalRegisteredClient)
                };
                
                var isregister = await _onboardingService.RegisterUserAsync(registerRequest);
                if (isregister.IsSuccess)
                {
                    command.UserId = isregister.NewUserId;

                   // TempData["SuccessMessage"] = "Registration successful! Please complete Meta Onboarding !";

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
                    ViewBag.DisableButtons = false;
                    TempData["ErrorMessage"] = "Registration failed. Please try again.";
                    return View(command);
                }

            }
            catch (Exception)
            {
                ViewBag.IsReadOnly = true;
                ViewBag.DisableButtons = false;
                TempData["ErrorMessage"] = "Technical Error";
                return View(command);
            }
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

        public async Task<IActionResult> VarifyWhatsAppNumber( VarifyMobileNumberModel command)
        {
            if (string.IsNullOrWhiteSpace(command.UserPhoneNumber))
            {
                return Json(new { status = false, message = "Please enter a valid WhatsApp number." });
            }

            var response = await _onboardingService.VarifyWhatsAppContactNoAsync(command);
            return Json(new { response.status, response.message });
        }

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
                return Json(new{ status = false,    message = "Enter a valid OTP"});
            }
            var response = await _onboardingService.VerifyOTPAsync(command);

            return Json(new{status = response.status,message = response.message});
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


                           // bool ConfirmationEmail = await _onboardingService.SendOnboardingConfirmationEmail(new VarifyMobileNumberModel { UserEmailId = registertemp.Email });

                            int UpdateDraft = await _authRepository.UpdateClientFormStageAsync(command.ClientInfoId, ClientFormStage.MetaRegistered);

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
        [HttpGet("Invalid-Link")]
        public IActionResult InvalidUrl()
        {
            return View();
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
            if (command.UserEmailId != null)
            {
                ModelState.AddModelError(nameof(command.UserEmailId), "Please verify your Email before proceeding.");
            }
            var response = await _onboardingService.CheckIsMailExitsAsync(command);

            return Json(new { response.status, response.message });
        }


    }

}
