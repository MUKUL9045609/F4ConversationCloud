using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class MasterTaxController : BaseController
    {
        private readonly IMasterTaxService _masterTaxService;
        public MasterTaxController(IMasterTaxService masterTaxService)
        {
            _masterTaxService = masterTaxService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var viewModel = new MasterTaxViewModel();
                var masterTaxData = await _masterTaxService.GetMasterTaxAsync();
                if (masterTaxData is not null)
                {
                    viewModel.Id = masterTaxData.Id;
                    viewModel.SGST = masterTaxData.SGST;
                    viewModel.CGST = masterTaxData.CGST;
                    viewModel.IGST = masterTaxData.IGST;
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(MasterTaxViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                int id = await _masterTaxService.CreateUpdateAsync(new MasterTax()
                {
                    Id = model.Id,
                    SGST = model.SGST,
                    CGST = model.CGST,
                    IGST = model.IGST
                });

                if (id > 0)
                {
                    TempData["SuccessMessage"] = "Master Tax updated successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error occured while updating Master Tax";
                }

                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }
    }
}
