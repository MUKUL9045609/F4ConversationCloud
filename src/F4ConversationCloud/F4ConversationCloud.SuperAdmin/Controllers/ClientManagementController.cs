using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;

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

            if (model.PageNumber > 1 && Math.Ceiling((decimal)response.Item2 / (decimal)model.PageSize) < model.PageNumber)
            {
                if (model.PageNumber > 1)
                {
                    TempData["ErrorMessage"] = "Invalid Page";
                }
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
        public IActionResult SaveClientDetails([FromBody] ClientDetailsViewModel model)
        {

            return Ok(new { message = "Saved successfully" });
        }
    }
}
