using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class ClientManagementController : BaseController
    {
        private readonly IClientManagementService _clientManagement;
        private readonly IMasterPriceService _masterPriceService;
        private readonly ITemplateManagementService _templateManagementService;
        private readonly IWhatsAppTemplateService _whatsAppTemplateService;
        public ClientManagementController(IClientManagementService clientManagement, IMasterPriceService masterPriceService, ITemplateManagementService templateManagementService
            , IWhatsAppTemplateService whatsAppTemplateService)
        {
            _clientManagement = clientManagement;
            _masterPriceService = masterPriceService;
            _templateManagementService = templateManagementService;
            _whatsAppTemplateService = whatsAppTemplateService;
        }

        public async Task<IActionResult> List(ClientManagementViewModel model)
        {
            try
            {
                var response = await _clientManagement.GetFilteredUsers(new ClientManagementListFilter
                {
                    ClientNameSearch = model.ClientNameSearch ?? String.Empty,
                    StatusFilter = model.StatusFilter ?? String.Empty,
                    OnboardingOnFilter = model.OnboardingOnFilter ?? String.Empty,
                    PhoneNumberFilter = model.PhoneNumberFilter ?? String.Empty,
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize,
                });

                if (model.PageNumber > 1 && model.PageNumber > Math.Ceiling((decimal)response.Item2 / model.PageSize))
                {
                    TempData["ErrorMessage"] = "Invalid Page";
                    return RedirectToAction("List");
                }

                model.TotalCount = response.Item2;
                model.data = response.Item1.ToList().Select(x => new ClientManagementViewModel.ClientManagementListViewItem()
                {
                    Id = x.Id,
                    SrNo = x.SrNo,
                    ClientName = x.ClientName,
                    Status = x.Status,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedOn = x.UpdatedOn,
                    Category = x.Category,
                    ClientId = x.ClientId,
                    PhoneNumber = x.PhoneNumber
                });

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return View(new ClientManagementViewModel());
            }
        }

        public async Task<IActionResult> ClientDetails(int Id)
        {
            try
            {
                var response = await _clientManagement.GetClientDetailsById(Id);

                if (response is null)
                {
                    TempData["ErrorMessage"] = "Error occured while fetching record";
                    return RedirectToAction("List");
                }

                var filter = new TemplatesListFilter
                {
                    ClientInfoId = Id

                };
                var templates = await _templateManagementService.TemplateListAsync(filter);
                var model = new ClientDetailsViewModel()
                {
                    Id = response.Id,
                    PhoneNumberId = response.PhoneNumberId,
                    WABAId = response.WABAId,
                    BusinessId = response.BusinessId,
                    ClientInfoId = response.ClientInfoId,
                    BusinessName = response.BusinessName,
                    Status = response.Status,
                    PhoneNumber = response.PhoneNumber,
                    AppVersion = response.AppVersion,
                    ApprovalStatus = response.ApprovalStatus,
                    Category = response.Category,
                    IsActive = response.IsActive,
                    CreatedAt = response.CreatedAt,
                    UpdatedOn = response.UpdatedOn,
                    RegisteredFirstName = response.RegisteredFirstName,
                    RegisteredLastName = response.RegisteredLastName,
                    RegisteredEmail = response.RegisteredEmail,
                    RegisteredPhoneNumber = response.RegisteredPhoneNumber,
                    RegisteredAddress = response.RegisteredAddress,
                    RegisteredCountry = response.RegisteredCountry,
                    RegisteredTimeZone = response.RegisteredTimeZone,
                    //TemplatesList = templates.Data,
                    OrganizationName = response.OrganizationName
                };

                var masterPriceData = await _masterPriceService.GetLatestRecordsByConversationType();
                var mappedMasterPrices = masterPriceData.Select(x => new MasterPrice
                {
                    Id = x.Id,
                    SrNo = x.SrNo,
                    ConversationType = x.ConversationType,
                    Price = x.Price,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    CreatedOn = x.CreatedOn,
                    IsActive = x.IsActive,
                    ConversationTypeName = ((TemplateModuleType)x.ConversationType).GetDisplayName()
                }).ToList();

                model.masterPrices = mappedMasterPrices;

                model.TemplatesList.StatusList = EnumExtensions.ToSelectList<TemplateApprovalStatus>();
                model.TemplatesList.LanguageList = EnumExtensions.ToSelectList<TemplateLanguages>();
                model.TemplatesList.TemplateCategoryList = EnumExtensions.ToSelectList<TemplateModuleType>();
                
                var templateListResponse = await _whatsAppTemplateService.GetFilteredTemplatesByWABAId(new TemplateListFilter
                {
                    WABAId = model.WABAId,
                    TemplateNameFilter = model.TemplatesList.TemplateNameFilter ?? String.Empty,
                    TemplateCategoryFilter = model.TemplatesList.TemplateCategoryFilter,
                    LanguageFilter = model.TemplatesList.LanguageFilter,
                    CreatedOnFilter = model.TemplatesList.CreatedOnFilter ?? String.Empty,
                    StatusFilter = model.TemplatesList.StatusFilter,
                    PageNumber = model.TemplatesList.PageNumber,
                    PageSize = model.TemplatesList.PageSize
                });

                model.TemplatesList.TotalCount = templateListResponse.Item2;
                model.TemplatesList.data = templateListResponse.Item1.ToList().Select(x => new TemplatesListViewModel.TemplateListViewItem()
                {
                    Id = x.Id,
                    SrNo = x.SrNo,
                    TemplateName = x.TemplateName,
                    TemplateCategory = ((TemplateModuleType)Convert.ToInt32(x.TemplateCategory)).GetDisplayName(),
                    Language = ((TemplateLanguages)Convert.ToInt32(x.Language)).GetDisplayName(),
                    CreatedOn = x.CreatedOn,
                    Status = ((TemplateApprovalStatus)Convert.ToInt32(x.Status)).GetDisplayName(),
                });

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveClientDetails([FromBody] ClientDetailsViewModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest(new { message = "Invalid request data." });

                var permissionsList = new List<ClientDetails>();

                if (model.IsMarketing)
                {
                    permissionsList.Add(new ClientDetails
                    {
                        Id = model.Id,
                        TemplateType = (int)TemplateModuleType.Marketing,
                        Create = model.MarketingCreate,
                        Add = model.MarketingAdd,
                        Edit = model.MarketingEdit,
                        Delete = model.MarketingDelete,
                        All = model.MarketingAll,
                        AllowUserManagement = model.AllowUserManagement
                    });
                }

                if (model.IsAuthentication)
                {
                    permissionsList.Add(new ClientDetails
                    {
                        Id = model.Id,
                        TemplateType = (int)TemplateModuleType.Authentication,
                        Create = model.AuthenticationCreate,
                        Add = model.AuthenticationAdd,
                        Edit = model.AuthenticationEdit,
                        Delete = model.AuthenticationDelete,
                        All = model.AuthenticationAll,
                        AllowUserManagement = model.AllowUserManagement
                    });
                }

                if (model.IsUtility)
                {
                    permissionsList.Add(new ClientDetails
                    {
                        Id = model.Id,
                        TemplateType = (int)TemplateModuleType.Utility,
                        Create = model.UtilityCreate,
                        Add = model.UtilityAdd,
                        Edit = model.UtilityEdit,
                        Delete = model.UtilityDelete,
                        All = model.UtilityAll,
                        AllowUserManagement = model.AllowUserManagement
                    });
                }

                foreach (var permission in permissionsList)
                {
                    var id = await _clientManagement.SaveClientPermission(permission);
                    if (id == 0)
                        return BadRequest(new { message = "Error occurred while setting permissions." });
                }

                return Ok(new { message = "Approved successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong. Please contact your administrator." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, string rejectComment)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rejectComment))
                {
                    return BadRequest("Comment is required.");
                }

                var status = "Reject";
                var response = await _clientManagement.Reject(id, status, rejectComment);

                if (response)
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }

        public async Task<IActionResult> GetFilteredTemplates(TemplateListFilter filter)
        {
            var model = new TemplatesListViewModel();
            model.StatusList = EnumExtensions.ToSelectList<TemplateApprovalStatus>();
            model.LanguageList = EnumExtensions.ToSelectList<TemplateLanguages>();
            model.TemplateCategoryList = EnumExtensions.ToSelectList<TemplateModuleType>();

            var templateListResponse = await _whatsAppTemplateService.GetFilteredTemplatesByWABAId(new TemplateListFilter
            {
                WABAId = filter.WABAId,
                TemplateNameFilter = filter.TemplateNameFilter ?? String.Empty,
                TemplateCategoryFilter = filter.TemplateCategoryFilter,
                LanguageFilter = filter.LanguageFilter,
                CreatedOnFilter = filter.CreatedOnFilter ?? String.Empty,
                StatusFilter = filter.StatusFilter,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            });

            model.TotalCount = templateListResponse.Item2;
            model.data = templateListResponse.Item1.ToList().Select(x => new TemplatesListViewModel.TemplateListViewItem()
            {
                Id = x.Id,
                SrNo = x.SrNo,
                TemplateName = x.TemplateName,
                TemplateCategory = ((TemplateModuleType)Convert.ToInt32(x.TemplateCategory)).GetDisplayName(),
                Language = ((TemplateLanguages)Convert.ToInt32(x.Language)).GetDisplayName(),
                CreatedOn = x.CreatedOn,
                Status = ((TemplateApprovalStatus)Convert.ToInt32(x.Status)).GetDisplayName()
            });

            return PartialView("_TemplateList", model);
        }
    }
}
