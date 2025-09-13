using BuldanaUrban.Application.Common.Interfaces.IWebServices;
using BuldanaUrban.Application.Common.Interfaces.Services;
using BuldanaUrban.Application.Common.Models;
using BuldanaUrban.Application.Common.OnBoardingRequestResposeModel;
using BuldanaUrban.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Twilio.Types;

namespace BuldanaUrban.Onboarding.Controllers
{
   // [Route("onboarding")]
    public class OnboardingController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOnboardingService _onboardingService;

        public OnboardingController(IUserService userService,IOnboardingService onboardingService)
        {
            _userService = userService;
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
            command.CurrentStep = 1;
            if (!command.EmailOtpVerified && command.Email != null)
            {
                ModelState.AddModelError(nameof(command.EmailOtpVerified), "Please verify your Email before proceeding.");
            }
            if (!ModelState.IsValid)
            {
              
                return View(command);
            }
            TempData.Put("step1form", command);
           


            return RedirectToAction("CompleteYourProfile");
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
            step1form.CurrentStep = 2;

            ModelState.Clear();
            if (!command.PhoneOtpVerified && !string.IsNullOrEmpty(command.PhoneNumber))
            {
                ModelState.AddModelError(nameof(command.PhoneOtpVerified), "Phone number verification required");
            }
            if (!TryValidateModel(step1form))
            {
                return View("CompleteYourProfile", command);
            }

            TempData.Put("step2form", step1form);
            return RedirectToAction("BankVerification");
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
        
        
        [HttpGet("register-step3")]
        public IActionResult BankVerification()
        {
            var step2form = TempData.Get<RegisterUserModel>("step2form");
            if (step2form is null)
            {
                TempData["SuccessMessage"] = "Register details first!";
                return RedirectToAction("CompleteYourProfile");
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
        public async Task<IActionResult> VarifyMobileNo([FromBody] VarifyMobileNumberModel command)
        {
            var response = await _onboardingService.CheckMailOrPhoneNumberAsync(command);
            //var response = await Mediator.Send(command);

            return Json(new { response.status, response.message });
        }



        public async Task<IActionResult> VerifyOTP([FromBody] ValidateRegistrationOTPModel command)
        {
            var response = await _onboardingService.VerifyOTPAsync(command);
            return Json(new { response.status });
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
            firstForm.CurrentStep = 2;

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


        // Helper to get/set model from Session

        private RegistrationModel GetModelFromSession()
        {
            var jsonData = HttpContext.Session.GetString("RegistrationModel");
            return jsonData != null
                ? JsonSerializer.Deserialize<RegistrationModel>(jsonData)
                : new RegistrationModel();
        }

        private void SaveModelToSession(RegistrationModel model)
        {
            var jsonData = JsonSerializer.Serialize(model);
            HttpContext.Session.SetString("RegistrationModel", jsonData);
        }

        // Step 1: GET - Choose Account Type
        [HttpGet]
        public ActionResult Step1()
        {
            var model = GetModelFromSession();
            return View(model);
        }

        // Step 1: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step1(RegistrationModel model)
        {


            return RedirectToAction("RegisterIndividualAccount");

            if (ModelState.IsValid)
            {
                SaveModelToSession(model);
                return RedirectToAction("Step2");
            }
            return View(model);
        }

        // Step 2: GET - Registration Form
        [HttpGet]
        public ActionResult Step2()
        {
            var model = GetModelFromSession();
            return View(model);
        }

        // Step 2: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step2(RegistrationModel model)
        {
            var sessionModel = GetModelFromSession();
            // Merge only Step 2 fields
            sessionModel.FullName = model.FullName;
            sessionModel.Email = model.Email;
            sessionModel.Password = model.Password;
            sessionModel.AgreeToTerms = model.AgreeToTerms;

            //if (ModelState.IsValidField("FullName") && ModelState.IsValidField("Email") &&
            //    ModelState.IsValidField("Password") && ModelState.IsValidField("AgreeToTerms"))
            //{
                _userService.RegisterUser(sessionModel); // Call service
                SaveModelToSession(sessionModel);
                return RedirectToAction("Step3");
            //}
            return View(sessionModel);
        }

        // Step 3: GET - Profile Completion
        [HttpGet]
        public ActionResult Step3()
        {
            var model = GetModelFromSession();
            return View(model);
        }

        // Step 3: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step3(RegistrationModel model)
        {
            var sessionModel = GetModelFromSession();
            // Merge Step 3 fields
            sessionModel.PhoneNumber = model.PhoneNumber;
            sessionModel.Address = model.Address;
            sessionModel.Country = model.Country;

            //if (ModelState.IsValidField("PhoneNumber") && ModelState.IsValidField("Address") &&
            //    ModelState.IsValidField("Country"))
            //{
                _userService.UpdateProfile(sessionModel); // Call service
                SaveModelToSession(sessionModel);
                return RedirectToAction("Step4");
            //}
            return View(sessionModel);
        }

        // Step 4: GET - Bank Verification
        [HttpGet]
        public ActionResult Step4()
        {
            var model = GetModelFromSession();
            return View(model);
        }

        // Step 4: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step4(RegistrationModel model)
        {
            var sessionModel = GetModelFromSession();
            // Merge Step 4 field
            sessionModel.BVN = model.BVN;

            //if (ModelState.IsValidField("BVN"))
            //{
                _userService.VerifyBank(sessionModel); // Call service
                SaveModelToSession(sessionModel);
                // Clear session after completion
                HttpContext.Session.Remove("RegistrationModel");
                // Redirect to success or dashboard (you can implement further)
                return RedirectToAction("Success"); // Placeholder
            //}
            return View(sessionModel);
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}
