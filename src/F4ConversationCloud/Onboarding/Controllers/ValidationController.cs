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

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsValidEmailOtpVerify(bool EmailOtpVerified, string Email)
        {
            if (!EmailOtpVerified)
            {
                return Json($"Email OTP for {Email} is not verified.");
            }

            return Json(true);
        }

    }
}
