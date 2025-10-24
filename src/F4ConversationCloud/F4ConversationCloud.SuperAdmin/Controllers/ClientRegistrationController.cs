using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class ClientRegistrationController : BaseController
    {
        private readonly IClientRegistrationService _clientRegistrationService;
        public ClientRegistrationController(IClientRegistrationService clientRegistrationService)
        {
            _clientRegistrationService = clientRegistrationService;
        }

        public async Task<IActionResult> List(ClientRegistrationListViewModel model)
        {
            try
            {
                model.RolesList = EnumExtensions.ToSelectList<ClientRole>();

                var response = await _clientRegistrationService.GetFilteredRegistrations(new ClientRegistrationListFilter
                {
                    NameFilter = model.NameFilter ?? String.Empty,
                    EmailFilter = model.EmailFilter ?? String.Empty,
                    RoleFilter = model.RoleFilter,
                    CreatedOnFilter = model.CreatedOnFilter ?? String.Empty,
                    UpdatedOnFilter = model.UpdatedOnFilter ?? String.Empty,
                    RegistrationStatusFilter = model.RegistrationStatusFilter ?? String.Empty,
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
                model.data = response.Item1.ToList().Select(x => new ClientRegistrationListViewModel.ClientRegistrationListViewItem()
                {
                    Id = x.Id,
                    SrNo = x.SrNo,
                    Name = x.FirstName + " " + x.LastName,
                    Email = x.Email,
                    Role = x.Role,
                    RegistrationStatus = x.RegistrationStatus,
                    CreatedOn = x.CreatedOn,
                    UpdatedOn = x.UpdatedOn,
                    RoleName = x.RoleName
                });

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.Roles = EnumExtensions.ToSelectList<ClientRole>();
                var viewModel = new CreateUpdateClientRegistrationViewModel();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateClientRegistrationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                int id = await _clientRegistrationService.CreateUpdateAsync(new ClientRegistration()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Role = model.Role,
                    RegistrationStatus = (int)ClientRegistrationStatus.PreRegistration
                });

                TempData["SuccessMessage"] = "Client pre-registered successfully";

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }
    }
}
