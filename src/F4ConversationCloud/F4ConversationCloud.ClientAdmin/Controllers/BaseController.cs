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
                var path = context.HttpContext.Request.Path.Value?.ToLower() ?? "";

                if (path.Contains("/auth/login") ||
                    path.Contains("/auth/logout") ||
                    path.Contains("/auth/issessionactive") ||
                    path.Contains(".js") ||
                    path.Contains(".css") ||
                    path.Contains(".png") ||
                    path.Contains(".jpg") ||
                    path.Contains(".ico"))
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
