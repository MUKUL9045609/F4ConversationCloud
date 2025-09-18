using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class UserManagementController : BaseController
    {
        private readonly IUserManagementService _userManagementService;
        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public async Task<IActionResult> List()
        {
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List(UserListViewModel model)
        {
            var response = await _userManagementService.GetFilteredUsers(new MasterListFilter
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
            model.data = response.Item1.ToList().Select(x => new UserListViewModel.UserListViewItem()
            {
                Id = x.Id,
                SrNo = x.SrNo,
                Name = x.FirstName + " " + x.LastName,
                Email = x.Email,
                MobileNo = x.MobileNo,
                Role = x.Role,
                Designation = x.Designation,
                IPAddress = x.IPAddress,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate
            });

            return View(model);
        }
    }
}
