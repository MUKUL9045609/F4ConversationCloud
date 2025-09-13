using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static F4ConversationCloud.Application.Common.Models.CampaignDTO;

namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface ICampaignService
    {
        Task<int> CreateCampaign(Campaign campaign);
        Task<List<Campaign>> GetCampaigns();
        Task<Campaign> GetCampaignById(int id);
        Task<int> UpdateCampaign(Campaign campaign);
        Task<bool> ActivateCampaign(int id);
        Task<bool> DeactivateCampaign(int id);
    }
}
