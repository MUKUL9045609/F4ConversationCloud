using F4ConversationCloud.Application.Common.Interfaces.IWebServices;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using Microsoft.AspNetCore.Mvc;
// Remove or comment out the following line as the 'Helpers' namespace does not exist:
using F4ConversationCloud.Domain.Helpers;
using System.Threading.Tasks;
using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Domain.Enum;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
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
                                    TempData.Put("registrationform", clientdetails);
                                return RedirectToAction("BankVerification");
                            }
                            else if (response.Data.Stage.Equals(ClientFormStage.metaregistered))
                            {
                                TempData["WarringMessage"] = "You have already registered please Wait For Admin Approval !";         
                                return View();
                            }

                         }
                        else
                        {
                            //TempData["ErrorMessage"] = response.Message;

                            if (response.Message.Equals("InvalidEmail")) ;
                                        ModelState.AddModelError(nameof(requst.Email),"Invalid Email");
                            if (response.Message.Equals("InvalidPassword"));
                                        ModelState.AddModelError(nameof(requst.PassWord),"Invalid Password");
                                        return View(response);
                         }
                    }
                    catch (Exception)
                    {

                       return View();
                    }
                   return View();
                 }
            
            [HttpGet("RegisterClientInfo")]
            public IActionResult RegisterIndividualAccount()
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
                var model = new RegisterUserModel();

                return View(model);

            }

            [HttpPost("RegisterClientInfo")]
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
                        TempData["SuccessMessage"] = "Registration successful! Please complete your profile.";
                     
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
            [HttpGet("MetaOnBoarding")]
            public IActionResult BankVerification()
            {
                var step1form = TempData.Get<RegisterUserModel>("registrationform");
                if (step1form is null)
                {
                    TempData["SuccessMessage"] = "Register details first!";
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMetaUserConfigurationDetails([FromBody] MetaUsersConfiguration command)
        {
            try
            {
                if (command.PhoneNumberId != null && command.WabaId != null && command.BusinessId != null)
                {
                    var registertemp = TempData.Get<RegisterUserModel>("registrationform");
                    if (registertemp != null)
                    {
                        command.OnboardingUserId = registertemp.UserId;

                        var metaresult = await _onboardingService.InsertMetaUsersConfigurationAsync(command);
                        var metaresponse = metaresult.status;

                        bool ConfirmationEmail = await _onboardingService.SendOnboardingConfirmationEmail(new VarifyMobileNumberModel { UserEmailId = registertemp.Email });
                            
                        var formstage = ClientFormStage.metaregistered;

                        int UpdateDraft = await _authRepository.UpdateClientFormStageAsync(registertemp.UserId, formstage);

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


        [HttpGet("register-step2")]
        public IActionResult CompleteYourProfile()
        {
            var step2form = TempData.Get<RegisterUserModel>("step2form");
            if (step2form != null)
            {
                var existingData = new RegisterUserModel();
                existingData = step2form;
                return View(existingData);
            }
            var step1form = TempData.Get<RegisterUserModel>("registrationform");
            if (step1form is null)
            {
                TempData["SuccessMessage"] = "Register details first!";
                return RedirectToAction("RegisterIndividualAccount");


            }
            var model = new RegisterUserModel();
            return View(model);
        }


        [HttpPost("register-step2")]
        [ValidateAntiForgeryToken]
        public IActionResult CompleteProfile(RegisterUserModel command)
        {
            var step1form = TempData.Get<RegisterUserModel>("registrationform");

            if (step1form == null)
            {
                ViewBag.ErrorMessage = "Session expired, please restart registration.";
                return View("CompleteYourProfile", command);
            }
            step1form.PhoneNumber = command.PhoneNumber;
            step1form.Address = command.Address;
            step1form.Country = command.Country;
            // step1form.CurrentStep = 2;

            ModelState.Clear();
            //if (!command.PhoneOtpVerified && !string.IsNullOrEmpty(command.PhoneNumber))
            //{
            //    ModelState.AddModelError(nameof(command.PhoneOtpVerified), "Phone number verification required");
            //}
            if (!TryValidateModel(step1form))
            {
                return View("CompleteYourProfile", command);
            }

            TempData.Put("step2form", step1form);
            return RedirectToAction("BankVerification");
        }






        public async Task<IActionResult> VarifyMobileNo([FromBody] VarifyMobileNumberModel command)
            {
                var response = await _onboardingService.CheckMailOrPhoneNumberAsync(command);
                //var response = await Mediator.Send(command);

                return Json(new { response.status, response.message });
            }



            
            [HttpPost]
            public async Task<IActionResult> SubmitRegisterIndividualAccount(RegisterUserModel command)
            {



                var firstForm = TempData.Get<RegisterUserModel>("step2form");

                if (firstForm == null)
                {
                    //return BadRequest("Session expired, please restart registration.");
                    return View("CompleteYourProfile", command);
                }
                firstForm.PhoneNumber = command.PhoneNumber;
                firstForm.Address = command.Address;
                firstForm.Country = command.Country;
                //firstForm.CurrentStep = 2;

                ModelState.Clear();

                if (!TryValidateModel(firstForm))
                {

                    return View("CompleteYourProfile", command);
                }

                TempData.Put("firstForm", firstForm);


                //var result = await Mediator.Send(firstForm);
                var result = await _onboardingService.RegisterUserAsync(firstForm);

                TempData.Remove("RegisterUser");

                //return Json(new { success = true });
                return RedirectToAction("ThankYouPage");
                //  return RedirectToAction("RegisterIndividualAccount");*/ 
            }

            public ActionResult Success()
            {
                return View();
            }
        }
    
}
