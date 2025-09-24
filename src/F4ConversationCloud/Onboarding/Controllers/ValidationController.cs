using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.Onboarding.Controllers
{
    public class ValidationController : Controller
    {
        public async Task<JsonResult> IsValidTermsCondition(bool termsAndCondition)
        {
            try
            {
                bool IsValid = true;

                if (!termsAndCondition)
                {
                    IsValid = false;
                }

                return Json(IsValid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<JsonResult> IsValidEmailOtpVerified(bool termsAndCondition, string email)
        {
            try
            {
                bool IsValid = true;

                if (!termsAndCondition && string.IsNullOrEmpty(email))
                {
                    IsValid = false;
                }

                return Json(IsValid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
