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
                string infoMsg = TempData["InfoMessage"]?.ToString();
                string warningMsg = TempData["WarningMessage"]?.ToString();

                if (!string.IsNullOrEmpty(successMsg))
                {
                    ViewData["ToastMsg"] = new { Type = "Success", Message = successMsg };
                    TempData.Remove("SuccessMessage");
                }
                else if (!string.IsNullOrEmpty(errorMsg))
                {
                    ViewData["ToastMsg"] = new { Type = "Error", Message = errorMsg };
                    TempData.Remove("ErrorMessage");
                }
                else if (!string.IsNullOrEmpty(infoMsg))
                {
                    ViewData["ToastMsg"] = new { Type = "Info", Message = infoMsg };
                    TempData.Remove("InfoMessage");
                }
                else if (!string.IsNullOrEmpty(warningMsg))
                {
                    ViewData["ToastMsg"] = new { Type = "Warning", Message = warningMsg };
                    TempData.Remove("WarningMessage");
                }

                var controllerName = context.RouteData.Values["controller"]?.ToString();
                var actionName = context.RouteData.Values["action"]?.ToString();


                if (controllerName == "Auth" && (actionName == "Login" || actionName == "Logout" || actionName == "ForgotPassword" || actionName== "ConfirmPassword" || actionName == "SetPassword" || actionName == "InvalidUrl"))
                {
                    base.OnActionExecuting(context);
                    return;
                }

                var username = context.HttpContext.Session.GetString("Username");

                if (string.IsNullOrEmpty(username))
                {
                    context.Result = new RedirectToActionResult("Login", "Auth", null);
                    return;
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
