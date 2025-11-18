using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class ClientRegistrationController : BaseController
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IClientRegistrationService _clientRegistrationService;
        private readonly IClientManagementService _clientManagement;
        public ClientRegistrationController(IClientRegistrationService clientRegistrationService, IClientManagementService clientManagement,ICurrentUserService currentUserService)
        {
            _clientRegistrationService = clientRegistrationService;
            _clientManagement = clientManagement;
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> List(ClientRegistrationListViewModel model)
        {
            try
            {
                model.RolesList = EnumExtensions.ToSelectList<ClientRole>();
                model.RegistrationStatusList = EnumExtensions.ToSelectList<ClientRegistrationStatus>();

                var response = await _clientRegistrationService.GetFilteredRegistrations(new ClientRegistrationListFilter
                {
                    OrganizationsNameFilter = model.OrganizationsNameFilter ?? String.Empty,
                    AccountStatusFilter = model.AccountStatusFilter,
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
                    AccountStatus = x.AccountStatus,
                    OrganizationsName = x.OrganizationsName,
                    ClientId = x.ClientId,
                    Category = x.Category,

                    RoleName = ((ClientRole)x.Role).GetDisplayName()
                });

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return View(new ClientRegistrationListViewModel());
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
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateClientRegistrationViewModel model)
        {
            try
            {
                ViewBag.Roles = EnumExtensions.ToSelectList<ClientRole>();
                if (!ModelState.IsValid)
                    return View(model);

                var emailExist = await _clientRegistrationService.CheckEmailExist(model.Email);

                if (emailExist)
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(model);
                }

                var contactExist = await _clientRegistrationService.CheckContactNumberExist(model.ContactNumber);

                if (contactExist)
                {
                    ModelState.AddModelError("ContactNumber", "This contact number is already registered.");
                    return View(model);
                }

                int id = await _clientRegistrationService.CreateUpdateAsync(new ClientRegistration()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    ContactNumber = model.ContactNumber,
                    Role = model.Role,
                    RegistrationStatus = (int)ClientRegistrationStatus.Pending
                });

                if (id > 0)
                {
                    var name = model.FirstName + " " + model.LastName;
                    await _clientRegistrationService.SendRegistrationEmailAsync(model.Email, name, id, model.ContactNumber);

                    TempData["SuccessMessage"] = "Client pre-registered successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error occured while pre-registation";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
            }
            return RedirectToAction("List");
        }

        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            try
            {
                var cr = await _clientRegistrationService.GetByIdAsync(id);

                if (cr is null)
                {
                    TempData["ErrorMessage"] = "Error occured while fetching record";

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
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateUpdateClientRegistrationViewModel model)
        {
            try
            {
                ViewBag.Roles = EnumExtensions.ToSelectList<ClientRole>();
                if (!ModelState.IsValid)
                    return View(model);

                var cr = await _clientRegistrationService.GetByIdAsync(model.Id);

                if (cr.FirstName == model.FirstName && cr.LastName == model.LastName
                    && cr.ContactNumber == model.ContactNumber && cr.Role == model.Role)
                {
                    var emailExist = await _clientRegistrationService.CheckEmailExist(model.Email);

                    if (emailExist)
                    {
                        ModelState.AddModelError("Email", "This email is already registered.");
                        return View(model);
                    }
                }

                if (cr.FirstName == model.FirstName && cr.LastName == model.LastName
                    && cr.Email == model.Email && cr.Role == model.Role)
                {
                    var contactExist = await _clientRegistrationService.CheckContactNumberExist(model.ContactNumber);

                    if (contactExist)
                    {
                        ModelState.AddModelError("ContactNumber", "This contact number is already registered.");
                        return View(model);
                    }
                }

                int id = await _clientRegistrationService.CreateUpdateAsync(new ClientRegistration()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    ContactNumber = model.ContactNumber,
                    Role = model.Role,
                    RegistrationStatus = (int)ClientRegistrationStatus.Pending
                });

                if (id > 0)
                {
                    //var name = model.FirstName + " " + model.LastName;
                    //await _clientRegistrationService.SendRegistrationEmailAsync(model.Email, name, id, model.ContactNumber);

                    TempData["SuccessMessage"] = "Client Registration updated successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error occured while updating record";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
            }
            return RedirectToAction("List");
        }

        public async Task<IActionResult> RegisteredBusinessDetails(int id)
        {
            try
            {
                var data = await _clientRegistrationService.GetRegisteredBusinessDetail(id);

                if (data is not null)
                {
                    var viewModel = new RegisteredBusinessDetailViewModel()
                    {
                        RegistrationId = data.RegistrationId,
                        OrganisationName = data.OrganisationName,
                        FullName = data.FullName,
                        Email = data.Email,
                        ContactNumber = data.ContactNumber,
                        AddressLine1 = data.AddressLine1,
                        AddressLine2 = data.AddressLine2,
                        Country = data.Country,
                        State = data.State,
                        City = data.City,
                        ZipCode = data.ZipCode
                    };

                    var model = new ClientManagementViewModel();
                    var response = await _clientManagement.GetFilteredUsers(new ClientManagementListFilter
                    {
                        ClientNameSearch = model.ClientNameSearch ?? String.Empty,
                        StatusFilter = model.StatusFilter ?? String.Empty,
                        OnboardingOnFilter = model.OnboardingOnFilter ?? String.Empty,
                        PhoneNumberFilter = model.PhoneNumberFilter ?? String.Empty,
                        PageNumber = model.PageNumber,
                        PageSize = model.PageSize,
                        RegistrationId = id
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
                        IsActive = x.IsActive,
                        CreatedAt = x.CreatedAt,
                        UpdatedOn = x.UpdatedOn,
                        Category = x.Category,
                        ClientId = x.ClientId,
                        PhoneNumber = x.PhoneNumber
                    });
                    viewModel.clientManagementViewModel = model;

                    return View(viewModel);
                }
                else
                {
                    TempData["ErrorMessage"] = "Error occured while fetching record";
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return RedirectToAction("List");
            }
        }

        public async Task<IActionResult> BusinessAccountList(ClientManagementViewModel model)
        {
            var response = await _clientManagement.GetFilteredUsers(new ClientManagementListFilter
            {
                ClientNameSearch = model.ClientNameSearch ?? String.Empty,
                StatusFilter = model.StatusFilter ?? String.Empty,
                OnboardingOnFilter = model.OnboardingOnFilter ?? String.Empty,
                PhoneNumberFilter = model.PhoneNumberFilter ?? String.Empty,
                WabaAccountStatusFilter=model.WabaAccountStatusFilter,
                PageNumber = model.PageNumber,
                PageSize = model.PageSize,
                RegistrationId = model.RegistrationId,
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
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedOn = x.UpdatedOn,
                Category = x.Category,
                ClientId = x.ClientId,
                PhoneNumber = x.PhoneNumber
            });

            return PartialView("_BusinessAccountsListPartial", model);
        }

        public async Task<IActionResult> ResendEmail(int id)
        {
            try
            {
                var cr = await _clientRegistrationService.GetByIdAsync(id);

                if (cr is null)
                {
                    return Ok(false);
                }

                await _clientRegistrationService.SendRegistrationEmailAsync(cr.Email, cr.FirstName + " " + cr.LastName, id, cr.ContactNumber);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }


        public async Task<IActionResult> ActivateClientAccount(int ClientId)
        {

            try
            {
                var request = new ActivateDeactivateClientAccountRequest
                {
                    ClientId = ClientId,
                    DeactivatedBy = Convert.ToInt32(_currentUserService.UserId),
                    AccountStatus = ClientRegistrationStatus.Active
                };
                var isDeletedResponce = await _clientRegistrationService.ActivateClientAccountAsync(request);

                if (isDeletedResponce.success)
                {
                    return Json(new DeleteTemplateResponse { success = true, message = isDeletedResponce.message });
                }
                else
                {
                    return Json(new DeleteTemplateResponse { success = false, message = isDeletedResponce.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new DeleteTemplateResponse { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> DeactivateClientAccount(int ClientId)
        {
            try
            {
                var request  = new ActivateDeactivateClientAccountRequest
                {
                    ClientId = ClientId,
                    DeactivatedBy = Convert.ToInt32(_currentUserService.UserId),
                    AccountStatus = ClientRegistrationStatus.Deactivated
                };

                var isDeletedResponce = await _clientRegistrationService.DeactivateClientAccountAsync(request);
                if (isDeletedResponce.success)
                {
                    return Json(new DeleteTemplateResponse { success = true, message = isDeletedResponce.message });
                }
                else
                {
                    return Json(new DeleteTemplateResponse { success = false, message = isDeletedResponce.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new DeleteTemplateResponse { success = false, message = ex.Message });
            }


        }


        public async Task<IActionResult> ActivateWaBaAccount(int Id)
        {

            try
            {
                var request = new ActivateDeactivateWaBaAccountRequest
                {
                    Id = Id,
                    DeactivatedBy = Convert.ToInt32(_currentUserService.UserId),
                };
                var isDeletedResponce = await _clientRegistrationService.ActivateWaBaAccountAsync(request);

                if (isDeletedResponce.success)
                {
                    return Json(new CommonSuperAdminServiceResponse { success = true, message = isDeletedResponce.message });
                }
                else
                {
                    return Json(new CommonSuperAdminServiceResponse { success = false, message = isDeletedResponce.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new CommonSuperAdminServiceResponse { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> DeactivateWaBaAccount(int Id)
        {
            try
            {
                var request = new ActivateDeactivateWaBaAccountRequest
                {
                    Id = Id,
                    DeactivatedBy = Convert.ToInt32(_currentUserService.UserId),
                };

                var isDeletedResponce = await _clientRegistrationService.DeactivateWaBaAccountAsync(request);
                if (isDeletedResponce.success)
                {
                    return Json(new CommonSuperAdminServiceResponse { success = true, message = isDeletedResponce.message });
                }
                else
                {
                    return Json(new CommonSuperAdminServiceResponse { success = false, message = isDeletedResponce.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new CommonSuperAdminServiceResponse { success = false, message = ex.Message });
            }


        }





    }
}