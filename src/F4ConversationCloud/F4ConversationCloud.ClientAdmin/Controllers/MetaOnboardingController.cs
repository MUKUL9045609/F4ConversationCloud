using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    public class MetaOnboardingController : BaseController
    {
        private readonly IOnboardingService _onboardingService;
        private readonly IAuthRepository _authRepository;
        public MetaOnboardingController(IOnboardingService onboardingService, IAuthRepository authRepository)
        {
            _onboardingService = onboardingService;
            _authRepository = authRepository;
        }

        [HttpGet("client-onboarding-list")]
        public IActionResult ClientOnboardingList()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveMetaUserConfigurationDetails([FromBody] MetaUsersConfiguration command)
        {
            var message = "";
            try
            {
                if (command.PhoneNumberId != null && command.WabaId != null && command.BusinessId != null)
                {
                    var clientEmail = HttpContext.Session.GetString("Username");
                    var userId = HttpContext.Session.GetInt32("UserId");
                    if (userId.HasValue && !string.IsNullOrEmpty(clientEmail))
                    {
                        command.ClientInfoId = userId.Value;

                        var metaresult = await _onboardingService.InsertMetaUsersConfigurationAsync(command);

                        bool ConfirmationEmail = await _onboardingService.SendOnboardingConfirmationEmail(new VarifyMobileNumberModel { UserEmailId = clientEmail });

                        int UpdateDraft = await _authRepository.UpdateClientFormStageAsync(command.ClientInfoId, ClientFormStage.metaregistered);

                        HttpContext.Session.SetInt32("StageId", (int)ClientFormStage.metaregistered);

                        message = "success";

                        return Json(new { result = true, message });
                    }
                    else
                    {
                        message = "Registration failed please contact to admin!";
                        return Json(new { result = false, message });
                    }
                }
                else
                {
                    message = " Meta registration failed please contact to admin ";
                    return Json(new { result = false });
                }
            }
            catch (Exception)
            {
                message = "Technical error! Please Contact To Admin ";
                return Json(new { result = false });
            }
        }
    }
}
