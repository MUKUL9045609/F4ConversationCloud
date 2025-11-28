using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PuppeteerSharp;
using PuppeteerSharp.Media;
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
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
                string logoBase64 = "";
                if (System.IO.File.Exists(logoPath))
                {
                    var logoBytes = System.IO.File.ReadAllBytes(logoPath);
                    logoBase64 = Convert.ToBase64String(logoBytes);
                }
                var invoiceViewModel = new InvoiceViewModel
                {
                    MetaConfigid = model.MetaConfigid,
                    PhoneNumberId = model.PhoneNumberId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    InvoiceData = response.InvoiceDetails,
                    TemplateMessageInsights = response.TemplateMessageInsights,
                    LogoBase64 = logoBase64

                };
                return View(invoiceViewModel);
            }
            catch (Exception)
            {
                return View(new BillingDetailsViewModel());
            }
        }

        [HttpGet("DownloadInvoicePdf")]
        public async Task<IActionResult> DownloadInvoicePdf(string PhoneNumberId, string MetaConfigId, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
               
                var request = new InvoiceRequest
                {
                    PhoneNumberId = PhoneNumberId,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    MetaConfigid = MetaConfigId
                };

                var response = await _usageAndBillingservice.GenerateInvoiceAsync(request);
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
                string logoBase64 = "";
                if (System.IO.File.Exists(logoPath))
                {
                    var logoBytes = System.IO.File.ReadAllBytes(logoPath);
                    logoBase64 = Convert.ToBase64String(logoBytes);
                }
                var invoiceViewModel = new InvoiceViewModel
                {
                    MetaConfigid = MetaConfigId,
                    PhoneNumberId = PhoneNumberId,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    InvoiceData = response.InvoiceDetails,
                    TemplateMessageInsights = response.TemplateMessageInsights,
                    LogoBase64 = logoBase64
                };

                string htmlContent;
                using (var sw = new StringWriter())
                {
                    ViewData.Model = invoiceViewModel;

                    var viewResult = _viewEngine.FindView(ControllerContext, "_InvoicePdfPartial", false);
                    var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw, new HtmlHelperOptions());
                    await viewResult.View.RenderAsync(viewContext);
                    htmlContent = sw.ToString();
                }

                var browserFetcher = new BrowserFetcher();
                await browserFetcher.DownloadAsync();

                using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,             
                });

                //string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                //using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                //{
                //    Headless = true,
                //    ExecutablePath = chromePath
                //});

                using var page = await browser.NewPageAsync();
                await page.SetContentAsync(htmlContent);
                var pdfBytes = await page.PdfDataAsync(new PdfOptions
                {
                    Format = PaperFormat.A4,
                    DisplayHeaderFooter = true,
                    FooterTemplate = "<span style='font-size:10px;color:#555'>Page <span class='pageNumber'></span> / <span class='totalPages'></span></span>",
                    MarginOptions = new MarginOptions { Top = "20px", Bottom = "40px", Left = "10px", Right = "10px" }
                });

                string startDateStr = StartDate.HasValue ? StartDate.Value.ToString("ddMMyyyy") : "";

                string fileName = $"{invoiceViewModel.InvoiceData.WhatsAppDisplayName}_{startDateStr}_Invoice.pdf";

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return RedirectToAction("GenerateInvoice", "Billing", new { PhoneNumberId = PhoneNumberId, MetaConfigId = MetaConfigId });
            }
            

        }



    }
}
