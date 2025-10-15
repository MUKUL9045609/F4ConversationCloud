using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {

        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                string successMsg = TempData["SuccessMessage"]?.ToString();
                string errorMsg = TempData["ErrorMessage"]?.ToString();

                if (!string.IsNullOrEmpty(successMsg))
                {
                    ViewData["ToastMsg"] = new { Type = "Success", Message = successMsg };
                    TempData.Remove("SuccessMessage");
                }
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    ViewData["ToastMsg"] = new { Type = "Error", Message = errorMsg };
                    TempData.Remove("ErrorMessage");
                }

                base.OnActionExecuting(context);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
