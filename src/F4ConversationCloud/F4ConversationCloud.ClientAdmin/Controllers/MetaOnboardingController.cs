using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.ClientAdmin.Models;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Infrastructure.Service.Client;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    [Route("MetaOnboarding")]
    public class MetaOnboardingController : BaseController
    {
        private readonly IOnboardingService _onboardingService;
        private readonly IAuthRepository _authRepository;
        private readonly IAddPhoneNumberService _addPhoneNumberService;
        private readonly ICurrentUserService _currentUser;
        public MetaOnboardingController(IOnboardingService onboardingService, IAuthRepository authRepository, IAddPhoneNumberService addPhoneNumberService,ICurrentUserService currentUser)
        {
            _onboardingService = onboardingService;
            _authRepository = authRepository;
            _addPhoneNumberService = addPhoneNumberService;
            _currentUser = currentUser;
        }

        [HttpGet("client-onboarding-list")]
        public async Task<IActionResult> ClientOnboardingList(PhoneNumberListViewModel model) 
        {
            try
            {
                var response = await _addPhoneNumberService.GetFilteredWhatsAppProfilesByUserId(new AddPhoneNumberListFilter
                {
                    WABAIdFilter = model.WABAIdFilter ?? String.Empty,
                    BusinessIdFilter = model.BusinessIdFilter ?? String.Empty,
                    BusinessCategoryFilter = model.BusinessCategoryFilter ?? String.Empty,
                    WhatsappDisplayNameFilter = model.WhatsappDisplayNameFilter ?? String.Empty,
                    PhoneNumberIdFilter = model.PhoneNumberIdFilter ?? String.Empty,
                    PhoneNumberFilter = model.PhoneNumberFilter ?? String.Empty,
                    StatusFilter = model.StatusFilter ?? String.Empty,
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize,
                });

                if (model.PageNumber > 1 && model.PageNumber > Math.Ceiling((decimal)response.Item2 / model.PageSize))
                {
                    TempData["ErrorMessage"] = "Invalid Page";
                    return RedirectToAction("List");
                }

                model.TotalCount = response.Item2;
                model.data = response.Item1.ToList().Select(x => new PhoneNumberListViewModel.PhoneNumberListViewItem()
                {
                    SrNo = x.SrNo,
                    WABAId = x.WABAId,
                    BusinessId = x.BusinessId,
                    BusinessCategory = x.BusinessCategory,
                    WhatsAppDisplayName = x.WhatsAppDisplayName,
                    PhoneNumberId = x.PhoneNumberId,
                    PhoneNumber = x.PhoneNumber,
                    Status = x.Status,
                });

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return View(new PhoneNumberListViewModel());
            }
        }

        [HttpPost("save-meta-user-configuration-details")]
        public async Task<IActionResult> SaveMetaUserConfigurationDetails([FromBody] MetaUsersConfiguration command)
        {
            var message = "";
            try
            {
                if (command.PhoneNumberId != null && command.WabaId != null && command.BusinessId != null)
                {
                    var clientEmail = _currentUser.CurrentUserEmail;
                    var userId = _currentUser.ClientInfoId;
                    if (!string.IsNullOrEmpty(userId)  && !string.IsNullOrEmpty(clientEmail))
                    {
                        command.ClientInfoId = Convert.ToInt32(userId);

                        var metaresult = await _onboardingService.InsertMetaUsersConfigurationAsync(command);

                        // bool ConfirmationEmail = await _onboardingService.SendOnboardingConfirmationEmail(new VarifyMobileNumberModel { UserEmailId = clientEmail });

                        int UpdateDraft = await _authRepository.UpdateClientFormStageAsync(command.ClientInfoId, ClientFormStage.MetaRegistered);

                        HttpContext.Session.SetInt32("StageId", (int)ClientFormStage.MetaRegistered);

                        message = "Your WhatsApp Business account has been successfully linked!";

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



        [HttpPost("Sync-Meta-list")]
        public async Task<IActionResult> SyncMetaDetails(string BusinessId)
        {
            try
            {
                var result =  _addPhoneNumberService.SyncWhatsAppAccountsAsync(BusinessId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
