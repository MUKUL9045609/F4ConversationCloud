using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Extension;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class UserManagementController : BaseController
    {
        private readonly IUserManagementService _userManagementService;
        private readonly ISuperAdminAuthService _superAdminAuthService;
        public UserManagementController(IUserManagementService userManagementService, ISuperAdminAuthService superAdminAuthService)
        {
            _userManagementService = userManagementService;
            _superAdminAuthService = superAdminAuthService;
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
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                RoleName = x.RoleName
            });

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = new SelectList(await _userManagementService.GetRolesAsync(), "Id", "Name");
            var viewModel = new CreateUpdateUserViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Auth user = await _superAdminAuthService.CheckUserExists(model.Email);

            if (user is not null)
            {
                ModelState.AddModelError("Email", "This Email is Already Registered.");
                ViewBag.Roles = new SelectList(await _userManagementService.GetRolesAsync(), "Id", "Name");
                return View(model);
            }

            int id = await _userManagementService.CreateUpdateAsync(new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                MobileNo = model.MobileNo,
                Password = model.Password,
                IPAddress = model.IPAddress,
                Role = model.Role,
                Designation = model.Designation
            });

            TempData["SuccessMessage"] = "User created successfully";

            return RedirectToAction("List");
        }

        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            User user = await _userManagementService.GetUserById(id);

            if (user is null)
            {
                TempData["ErrorMessage"] = "User not found";

                return RedirectToAction("List");
            }

            ViewBag.Roles = new SelectList(await _userManagementService.GetRolesAsync(), "Id", "Name");

            CreateUpdateUserViewModel model = new CreateUpdateUserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                MobileNo = user.MobileNo,
                Password = user.Password,
                IPAddress = user.IPAddress,
                Designation = user.Designation,
                Role = user.Role
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateUpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Auth user = await _superAdminAuthService.CheckUserExists(model.Email);

            if (user is not null)
            {
                ModelState.AddModelError("Email", "This Email is Already Registered.");
                return View(model);
            }

            int id = await _userManagementService.CreateUpdateAsync(new User()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                MobileNo = model.MobileNo,
                Password = model.Password,
                IPAddress = model.IPAddress,
                Role = model.Role,
                Designation = model.Designation
            });

            TempData["SuccessMessage"] = "User updated successfully";

            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate([FromRoute] int id)
        {
            try
            {
                var affectedRows = await _userManagementService.Deactivate(id);

                if (affectedRows)
                {
                    TempData["SuccessMessage"] = "User Deactivated successfully";
                    return Ok(true);
                }
                else
                {
                    TempData["ErrorMessage"] = "Error while Deactivating User";
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                var affectedRows = await _userManagementService.Activate(id);

                if (affectedRows)
                {
                    TempData["SuccessMessage"] = "User Activated successfully";
                    return Ok(true);
                }
                else
                {
                    TempData["ErrorMessage"] = "Error while Activating User";
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
