using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

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
            model.RolesList = EnumExtensions.ToSelectList<ClientRole>();

            var response = await _clientUserManagementService.GetFilteredUsers(new ClientUserManagementListFilter
            {
                BusinessFilter = model.BusinessFilter ?? String.Empty,
                NameFilter = model.NameFilter ?? String.Empty,
                EmailFilter = model.EmailFilter ?? String.Empty,
                RoleFilter = model.RoleFilter,
                CreatedOnFilter = model.CreatedOnFilter ?? String.Empty,
                UpdatedOnFilter = model.UpdatedOnFilter ?? String.Empty,
                Status = model.Status ?? String.Empty,
                PageNumber = model.PageNumber,
                PageSize = model.PageSize,
            });

            if (model.PageNumber > 1 && model.PageNumber > Math.Ceiling((decimal)response.Item2 / model.PageSize))
            {
                TempData["ErrorMessage"] = "Invalid Page";
                return RedirectToAction("List");
            }

            model.TotalCount = response.Item2;
            model.data = response.Item1.ToList().Select(x => new ClientUserListViewModel.ClientUserListViewItem()
            {
                Id = x.Id,
                SrNo = x.SrNo,
                Name = x.FirstName + " " + x.LastName,
                Email = x.Email,
                Role = ((ClientRole)(int)x.Role).GetDisplayName(),
                IsActive = x.IsActive,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                BusinessName = x.BusinessName,
                Category = x.Category,
                ClientId = x.ClientId
            });

            return View(model);
        }
    }
}
