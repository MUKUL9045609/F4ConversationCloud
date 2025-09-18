using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Domain.Entities;

namespace F4ConversationCloud.Application.Common.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaign;

        public CampaignService(ICampaignRepository campaign)
        {
            _campaign = campaign;
        }

        public async Task<int> CreateCampaign(Campaign campaign)
        {
            return await _campaign.CreateCampaign(campaign);
        }

        public async Task<List<Campaign>> GetCampaigns()
        {
            return await _campaign.GetCampaigns();
        }

        public async Task<Campaign> GetCampaignById(int id)
        {
            return await _campaign.GetCampaignById(id);
        }

        public async Task<int> UpdateCampaign(Campaign campaign)
        {
            return await _campaign.UpdateCampaign(campaign);
        }

        public async Task<bool> ActivateCampaign(int id)
        {
            return await _campaign.ActivateCampaign(id);
        }

        public async Task<bool> DeactivateCampaign(int id)
        {
            return await _campaign.DeactivateCampaign(id);
        }
    }
}
