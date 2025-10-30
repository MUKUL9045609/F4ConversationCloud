using F4ConversationCloud.ClientAdmin.Models.CampaignViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Twilio.Annotations;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    [Authorize]
    public class CampaignsController : BaseController
    {
        public IActionResult CampaignList()
        {
            return View();
        }
        public IActionResult CreateCampaign()
        {
            CreateCampaignViewModel createCampaignView = new CreateCampaignViewModel();

            return View(createCampaignView);
        }

        [HttpPost]
        public IActionResult CreateCampaign(CreateCampaignViewModel request)
        {
            if (!ModelState.IsValid) {
                return View(request);
            }
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult>  GetCampaignFlowTabPartialView(CreateCampaignViewModel request)
        {
            var PartialviewName = "";
            
            if (request != null)
            {
                var currentTabName = request.NextTabName;
                if (currentTabName == "Audience") {
                    PartialviewName = "_AudiencePartial";
                }
                if (currentTabName == "Template")
                {
                    PartialviewName = "_TemplateListPartial";
                }
                return PartialView(PartialviewName, request);

            }
            return PartialView(PartialviewName, request);
        }
        public IActionResult Guide()
        {
            return View();
        }
    }
}
