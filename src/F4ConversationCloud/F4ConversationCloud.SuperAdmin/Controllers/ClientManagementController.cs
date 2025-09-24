using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class ClientManagementController : BaseController
    {
        private readonly IClientManagementService _clientManagement;
        public ClientManagementController(IClientManagementService clientManagement)
        {
            _clientManagement = clientManagement;
        }

        public async Task<IActionResult> List(ClientManagementViewModel model)
        {
            var response = await _clientManagement.GetFilteredUsers(new MasterListFilter
            {
                SearchString = model.SearchString ?? String.Empty,
                Status = model.Status,
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
                ApprovalStatus = x.ApprovalStatus,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedOn = x.UpdatedOn,
                Category = x.Category
            });

            return View(model);
        }

        public async Task<IActionResult> ClientDetails(int Id)
        {
            var response = await _clientManagement.GetClientDetailsById(Id);

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
                RegisteredTimeZone = response.RegisteredTimeZone
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveClientDetails([FromBody] ClientDetailsViewModel model)
        {
            if (model.IsMarketing)
            {
                var marketingPermissions = new ClientDetails
                {
                    Id = model.Id,
                    TemplateType = (int)TemplateModuleType.Marketing,
                    Create = model.MarketingCreate,
                    Add = model.MarketingAdd,
                    Edit = model.MarketingEdit,
                    Delete = model.MarketingDelete,
                    All = model.MarketingAll,
                    AllowUserManagement = model.AllowUserManagement
                };

                await _clientManagement.SaveClientPermission(marketingPermissions);
            }

            if (model.IsAuthentication)
            {
                var authPermissions = new ClientDetails
                {
                    Id = model.Id,
                    TemplateType = (int)TemplateModuleType.Authentication,
                    Create = model.AuthenticationCreate,
                    Add = model.AuthenticationAdd,
                    Edit = model.AuthenticationEdit,
                    Delete = model.AuthenticationDelete,
                    All = model.AuthenticationAll,
                    AllowUserManagement = model.AllowUserManagement
                };

                await _clientManagement.SaveClientPermission(authPermissions);
            }

            if (model.IsUtility)
            {
                var utilityPermissions = new ClientDetails
                {
                    Id = model.Id,
                    TemplateType = (int)TemplateModuleType.Utility,
                    Create = model.UtilityCreate,
                    Add = model.UtilityAdd,
                    Edit = model.UtilityEdit,
                    Delete = model.UtilityDelete,
                    All = model.UtilityAll,
                    AllowUserManagement = model.AllowUserManagement
                };

                await _clientManagement.SaveClientPermission(utilityPermissions);
            }

            return Ok(new { message = "Saved successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, string rejectComment)
        {
            if (string.IsNullOrWhiteSpace(rejectComment))
            {
                return BadRequest("Comment is required.");
            }

            var status = "Rejected";
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
    }
}
