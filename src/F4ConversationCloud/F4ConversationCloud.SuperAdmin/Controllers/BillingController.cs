using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using IronPdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class BillingController : BaseController
    {
        private readonly IUsageAndBillingService _usageAndBillingservice;
        private readonly ICompositeViewEngine _viewEngine;

        public BillingController(IUsageAndBillingService usageAndBillingService, ICompositeViewEngine compositeView)
        {
            _usageAndBillingservice = usageAndBillingService;
            _viewEngine = compositeView;
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
                {   MetaConfigid = request.MetaConfigid,
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

        [HttpGet("Invoice")]
        public async Task<IActionResult> GenerateInvoice(InvoiceViewModel model)
        {
            try
            {
                var request = new InvoiceRequest
                {
                    PhoneNumberId = model.PhoneNumberId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    MetaConfigid=model.MetaConfigid
                };
                var response = await _usageAndBillingservice.GenerateInvoiceAsync(request);

                var invoiceViewModel = new InvoiceViewModel
                {
                    MetaConfigid = model.MetaConfigid,
                    PhoneNumberId = model.PhoneNumberId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    InvoiceData = response.InvoiceDetails,
                    TemplateMessageInsights = response.TemplateMessageInsights


                };
                return View(invoiceViewModel);
            }
            catch (Exception)
            {
                return View(new BillingDetailsViewModel());
            }
        }

        protected async Task<string> RenderViewAsync(string viewName, object model, bool partial = false)
        {
            ViewData.Model = model;
            using var writer = new StringWriter();
            var viewResult = _viewEngine.FindView(ControllerContext, viewName, !partial);

            var viewContext = new ViewContext(
                 ControllerContext,
                 viewResult.View,
                 ViewData,
                 TempData,
                 writer,
                 new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return writer.ToString();
        }


        public async Task<IActionResult> DownloadInvoice(InvoiceViewModel model)
        {
            try
            {
                // You must reload data or InvoiceData is null
                var request = new InvoiceRequest
                {
                    PhoneNumberId = model.PhoneNumberId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    MetaConfigid = model.MetaConfigid
                };

                var response = await _usageAndBillingservice.GenerateInvoiceAsync(request);

                if (response.InvoiceDetails == null)
                    throw new Exception("InvoiceData returned NULL from service");

                var invoiceViewModel = new InvoiceViewModel
                {
                    MetaConfigid = model.MetaConfigid,
                    PhoneNumberId = model.PhoneNumberId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    InvoiceData = response.InvoiceDetails,
                    TemplateMessageInsights = response.TemplateMessageInsights
                };

                string html = await RenderViewAsync("GenerateInvoice", invoiceViewModel, true);

                var renderer = new ChromePdfRenderer();
                var pdf = renderer.RenderHtmlAsPdf(html);

                var fileName = $"Invoice_{invoiceViewModel.InvoiceData.WhatsAppDisplayName}_from{invoiceViewModel.StartDate}_to{invoiceViewModel.EndDate}.pdf";

                return File(pdf.BinaryData, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to generate invoice PDF: " + ex.Message;
                return RedirectToAction("BillingDetails");
            }
        }




    }
}
