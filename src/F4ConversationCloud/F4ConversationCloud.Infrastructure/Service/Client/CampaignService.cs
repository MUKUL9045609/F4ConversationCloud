using F4ConversationCloud.Application.Common.Interfaces.Repositories.Client;
using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Domain.Entities;

namespace F4ConversationCloud.Infrastructure.Service.Client
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

        public async Task<bool> DeactivateAudientMansterGroupAsync(int id)
        {
            return await _campaign.DeactivateAudientMansterGroup(id);
        }
        public async Task<int> UpdateAudientMansterGroupAsync(AudienceGroupMasterModel group)
        {
            return await _campaign.UpdateAudientMansterGroup(group);
        }
        public async Task<IEnumerable<AudienceGroupMasterModel>> GetAllAudientMansterGroupsAsync()
        {
            return await _campaign.GetAllAudientMansterGroups();
        }
        public async Task<AudienceGroupMasterModel> GetAudientMansterGroupByIdAsync(int Id)
        {
            return await _campaign.GetAudientMansterGroupById(Id);
        }
        public async Task<int> CreateAudientMansterGroupAsync(AudienceGroupMasterModel group)
        {
            return await _campaign.CreateAudientMansterGroup(group);
        }
        

    }
}
