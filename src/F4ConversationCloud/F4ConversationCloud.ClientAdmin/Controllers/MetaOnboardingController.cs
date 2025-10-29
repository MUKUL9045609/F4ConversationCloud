using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.ClientAdmin.Models;
using F4ConversationCloud.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    public class MetaOnboardingController : BaseController
    {
        private readonly IOnboardingService _onboardingService;
        private readonly IAuthRepository _authRepository;
        private readonly IAddPhoneNumberService _addPhoneNumberService;
        public MetaOnboardingController(IOnboardingService onboardingService, IAuthRepository authRepository, IAddPhoneNumberService addPhoneNumberService)
        {
            _onboardingService = onboardingService;
            _authRepository = authRepository;
            _addPhoneNumberService = addPhoneNumberService;
        }

        [HttpGet("client-onboarding-list")]
        public async Task<IActionResult> ClientOnboardingList()
        {
            var viewModel = new PhoneNumberListViewModel();
            try
            {
                viewModel.addPhoneNumberModels = await _addPhoneNumberService.GetWhatsAppProfilesByUserId();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return View(viewModel);
            }
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
