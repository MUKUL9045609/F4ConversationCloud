using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Infrastructure.Service.SuperAdmin;
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
                CreatedAt = x.OnboardingOn,
                UpdatedOn = x.UpdatedOn,
                Category = x.Category
            });

            return View(model);
        }
    }
}
