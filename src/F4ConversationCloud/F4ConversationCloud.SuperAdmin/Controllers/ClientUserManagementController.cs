using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Authorization;
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
            try
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }

        public async Task<IActionResult> Deactivate([FromRoute] int id)
        {
            try
            {
                var affectedRows = await _clientUserManagementService.Deactivate(id);

                if (affectedRows)
                {
                    TempData["SuccessMessage"] = "Account Deactivated successfully";
                    return Ok(true);
                }
                else
                {
                    TempData["ErrorMessage"] = "Error while Deactivating Account";
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }

        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                var affectedRows = await _clientUserManagementService.Activate(id);

                if (affectedRows)
                {
                    TempData["SuccessMessage"] = "Account Activated successfully";
                    return Ok(true);
                }
                else
                {
                    TempData["ErrorMessage"] = "Error while Activating Account";
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }
    }
}
