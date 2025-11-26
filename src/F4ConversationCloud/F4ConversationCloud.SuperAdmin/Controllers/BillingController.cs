using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using static F4ConversationCloud.SuperAdmin.Models.BillingDetailsViewModel;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class BillingController : BaseController
    {
        private readonly IUsageAndBillingService _usageAndBillingservice;
        public BillingController(IUsageAndBillingService usageAndBillingService)
        {
            _usageAndBillingservice = usageAndBillingService;
        }
        [HttpGet("billing-list")]
        public async Task<IActionResult> List(BillingListViewModel request)
        {
            try
            {
                var filter = new BillingListFilter
                {
                    OrganizationsNameFilter = request.OrganizationsNameFilter,
                    WabaPhoneNumberFilter = request.WabaPhoneNumberFilter,
                    WhatsAppDisplayNameFilter = request.WhatsAppDisplayNameFilter,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
                var response = await _usageAndBillingservice.GetBillingListAsync(filter);

                var BillingViewModel = new BillingListViewModel
                {
                    OrganizationsNameFilter = request.OrganizationsNameFilter,
                    WabaPhoneNumberFilter = request.WabaPhoneNumberFilter,
                    WhatsAppDisplayNameFilter = request.WhatsAppDisplayNameFilter,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = response.TotalCount,
                    data = response.billingListItems
                };

                return View(BillingViewModel);
            }
            catch (Exception)
            {
                    return View(new BillingListViewModel());
            }
            
        }

        [HttpGet("billing-Details")]
        public async Task<IActionResult> billingDetails(TemplateMessageInsightsFilter request)
        {
            try
            {
                var response = await _usageAndBillingservice.GetTemplateMessageInsightsListAsync(request);

                var billingDetailsViewModel = new BillingDetailsViewModel
                {
                    PhoneNumberId = request.PhoneNumberId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    data = response
                };
                return View(billingDetailsViewModel);
            }
            catch (Exception)
            {
                return View(new BillingDetailsViewModel());
            }
        }
    }
}
