using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Infrastructure.Service.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class MasterPriceController : BaseController
    {
        private readonly IMasterPriceService _masterPriceService;
        public MasterPriceController(IMasterPriceService masterPriceService)
        {
            _masterPriceService = masterPriceService;
        }

        public async Task<IActionResult> List(MasterPriceFilterModel model)
        {
            try
            {
                var response = await _masterPriceService.GetFilteredMasterPrices(new MasterListFilter
                {
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize,
                });

                if (model.PageNumber > 1 && model.PageNumber > Math.Ceiling((decimal)response.Item2 / model.PageSize))
                {
                    TempData["ErrorMessage"] = "Invalid Page";
                    return RedirectToAction("List");
                }

                model.TotalCount = response.Item2;
                model.data = response.Item1.ToList().Select(x => new MasterPriceFilterModel.MasterPriceListViewItem()
                {
                    Id = x.Id,
                    SrNo = x.SrNo,
                    ConversationType = ((TemplateModuleType)(int)x.ConversationType).GetDisplayName(),
                    Price = x.Price,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate
                });

                var viewModel = new MasterPriceViewModel();
                viewModel.MasterPriceFilterModel = model;
                viewModel.ConversationTypeList = EnumExtensions.ToSelectList<TemplateModuleType>();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> List([FromForm] MasterPriceViewModel model)
        {
            try
            {
                model.ConversationTypeList = EnumExtensions.ToSelectList<TemplateModuleType>();
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                int id = await _masterPriceService.CreateAsync(new MasterPrice() 
                { 
                    ConversationType = model.ConversationType,
                    Price = model.Price,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate
                });

                TempData["SuccessMessage"] = "Master price created successfully";
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
