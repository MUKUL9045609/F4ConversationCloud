using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class ClientUserManagementController : BaseController
    {
        private readonly IClientUserManagementService _clientUserManagementService;
        public ClientUserManagementController(IClientUserManagementService clientUserManagementService)
        {
            _clientUserManagementService = clientUserManagementService;
        }

        public async Task<IActionResult> List(ClientUserListViewModel model)
        {
            var response = await _clientUserManagementService.GetFilteredUsers(new MasterListFilter
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
            model.data = response.Item1.ToList().Select(x => new ClientUserListViewModel.ClientUserListViewItem()
            {
                Id = x.Id,
                SrNo = x.SrNo,
                Name = x.FirstName + " " + x.LastName,
                Email = x.Email,
                Role = x.Role,
                IsActive = x.IsActive,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn
            });

            return View(model);
        }
    }
}
