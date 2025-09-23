using F4ConversationCloud.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace F4ConversationCloud.Onboarding.Controllers
{
    public class ValidationController : Controller
    {
        [AcceptVerbs("GET", "POST")]
        public async Task<JsonResult> IsValidTermsCondition(bool TermsCondition)
        {
            try
            {
                if (TermsCondition)
                {
                    return Json(true); 
                }
                return Json(false);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        public async Task<JsonResult> IsValidNoWhitespace(string FirstName)
       {
            try
            {
                bool isValid = !string.IsNullOrWhiteSpace(FirstName);
                if (!isValid)
                {
                     return Json(false);
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        
        public async Task<JsonResult> IsValidEmailOtpVerify(bool EmailOtpVerified, string Email)
        {
            try
            {
                bool IsValid = true;

                if (!EmailOtpVerified && string.IsNullOrEmpty(Email))
                {
                    IsValid = false;
                }

                return Json(IsValid);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
    }
}
