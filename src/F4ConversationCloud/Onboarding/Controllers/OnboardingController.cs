using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using Microsoft.AspNetCore.Mvc;
// Remove or comment out the following line as the 'Helpers' namespace does not exist:
using F4ConversationCloud.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
namespace F4ConversationCloud.Onboarding.Controllers
{
    public class OnboardingController:Controller
    {

      
            private readonly IOnboardingService _onboardingService;

            public OnboardingController(IOnboardingService onboardingService)
            {
              
                _onboardingService = onboardingService;
            }
            public IActionResult Index()
            {
                return View();
            }


            public async Task<IActionResult> Login()
            {

                return View();
            }

            [HttpGet("register-step1")]
            public IActionResult RegisterIndividualAccount()
            {
                var step1form = TempData.Get<RegisterUserModel>("step1form");
                if (step1form != null)
                {
                    var existingData = new RegisterUserModel();
                    existingData = step1form;
                    return View(existingData);
                }
                var model = new RegisterUserModel();

                return View(model);

            }

            [HttpPost("register-step1")]
            [ValidateAntiForgeryToken]
            public IActionResult RegisterIndividualAccount(RegisterUserModel command)
            {
               // command.CurrentStep = 1;
                if (!command.EmailOtpVerified && command.Email != null)
                {
                    ModelState.AddModelError(nameof(command.EmailOtpVerified), "Please verify your Email before proceeding.");
                }
                if (!ModelState.IsValid)
                {

                    return View(command);
                }
                TempData.Put("step1form", command);



                return RedirectToAction("BankVerification","Onboarding");
            }
            [HttpGet("register-step2")]
            public IActionResult BankVerification()
            {
                var step1form = TempData.Get<RegisterUserModel>("step1form");
                if (step1form is null)
                {
                    TempData["SuccessMessage"] = "Register details first!";
                    return RedirectToAction("RegisterIndividualAccount");
                }
                return View();
            }

            [HttpGet("thank-you")]
            public IActionResult ThankYouPage()
            {
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
            if (command.PhoneNumberId != null && command.WabaId != null && command.BusinessId != null)
            {

                var storeFinalform = TempData.Get<RegisterUserModel>("step2form");
                if (storeFinalform == null)
                {

                    var message = "Session expired, please restart registration.";

                    return Json(new { result = false, message });
                }
                var result = await _onboardingService.RegisterUserAsync(storeFinalform);

                TempData.Remove("step2form");

                var status = result.IsSuccess;

                if (status != false)
                {
                    command.OnboardingUserId = result.NewUserId;

                    var metaresult = await _onboardingService.InsertMetaUsersConfigurationAsync(command);
                    var metaresponse = metaresult.status;

                    bool ConfirmationEmail = await _onboardingService.SendOnboardingConfirmationEmail(new VarifyMobileNumberModel { UserEmailId = storeFinalform.Email });

                    var message = "success";

                    return Json(new { result = true, message });
                }
                else
                {
                    var message = "User Registration Failed.";
                    return Json(new { result = false, message });
                }

            }
            else
            {
                var message = "failed from register from meta";
                return Json(new { result = false });

            }

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
                var step1form = TempData.Get<RegisterUserModel>("step1form");
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
            var step1form = TempData.Get<RegisterUserModel>("step1form");

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
