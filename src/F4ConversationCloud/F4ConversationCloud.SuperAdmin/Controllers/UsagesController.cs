using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class UsagesController : BaseController
    {
        private readonly IUsageAndBillingService _usageAndBillingservice;
        public UsagesController(IUsageAndBillingService usageAndBillingService )
        {
            _usageAndBillingservice = usageAndBillingService;
        }
        [HttpGet("usage-list")]
        public async Task<IActionResult> List(UsageViewModel request)
        {
            var filter = new UsageListFilter
            {
                OrganizationsNameFilter = request.OrganizationsNameFilter,
                WabaPhoneNumberFilter = request.WabaPhoneNumberFilter,
                WhatsAppDisplayNameFilter = request.WhatsAppDisplayNameFilter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
            var response = await _usageAndBillingservice.GetUsageDetailsAsync(filter);

            return View(request);
        }
    }
}
