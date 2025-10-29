using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web.Helpers;

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
                model.RegistrationStatusList = EnumExtensions.ToSelectList<ClientRegistrationStatus>();

                var response = await _clientRegistrationService.GetFilteredRegistrations(new ClientRegistrationListFilter
                {
                    NameFilter = model.NameFilter ?? String.Empty,
                    EmailFilter = model.EmailFilter ?? String.Empty,
                    ContactNumberFilter = model.ContactNumberFilter ?? String.Empty,
                    RoleFilter = model.RoleFilter,
                    CreatedOnFilter = model.CreatedOnFilter ?? String.Empty,
                    UpdatedOnFilter = model.UpdatedOnFilter ?? String.Empty,
                    RegistrationStatusFilter = model.RegistrationStatusFilter,
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
                    ContactNumber = x.ContactNumber,
                    Role = x.Role,
                    RegistrationStatus = x.RegistrationStatus,
                    RegistrationStatusName = x.RegistrationStatus == 0 ? "" : ((ClientRegistrationStatus)x.RegistrationStatus).GetDisplayName(),
                    CreatedOn = x.CreatedOn,
                    UpdatedOn = x.UpdatedOn,
                    RoleName = ((ClientRole)x.Role).GetDisplayName()
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
                    ContactNumber = model.ContactNumber,
                    Role = model.Role,
                    RegistrationStatus = (int)ClientRegistrationStatus.PreRegistered
                });

                var name = model.FirstName + " " + model.LastName;
                await _clientRegistrationService.SendRegistrationEmailAsync(model.Email, name, id);

                
                TempData["SuccessMessage"] = "Client pre-registered successfully";

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }

        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            try
            {
                ClientRegistration cr = await _clientRegistrationService.GetByIdAsync(id);

                if (cr is null)
                {
                    TempData["ErrorMessage"] = "Details not found";

                    return RedirectToAction("List");
                }

                ViewBag.Roles = EnumExtensions.ToSelectList<ClientRole>();

                CreateUpdateClientRegistrationViewModel model = new CreateUpdateClientRegistrationViewModel()
                {
                    Id = cr.Id,
                    FirstName = cr.FirstName,
                    LastName = cr.LastName,
                    Email = cr.Email,
                    ContactNumber = cr.ContactNumber,
                    Role = cr.Role
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateUpdateClientRegistrationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                int id = await _clientRegistrationService.CreateUpdateAsync(new ClientRegistration()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    ContactNumber = model.ContactNumber,
                    Role = model.Role,
                    RegistrationStatus = (int)ClientRegistrationStatus.PreRegistered
                });

                TempData["SuccessMessage"] = "Client Registration updated successfully";

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