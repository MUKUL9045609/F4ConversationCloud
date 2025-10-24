using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.Client
{
    public interface ICampaignRepository
    {
        Task<int> CreateCampaign(Campaign campaign);
        Task<List<Campaign>> GetCampaigns();
        Task<Campaign> GetCampaignById(int id);
        Task<int> UpdateCampaign(Campaign campaign);
        Task<bool> ActivateCampaign(int id);
        Task<bool> DeactivateCampaign(int id);
        Task<bool> DeactivateAudientMansterGroup(int id);
        Task<int> UpdateAudientMansterGroup(AudienceGroupMasterModel group);
        Task<IEnumerable<AudienceGroupMasterModel>> GetAllAudientMansterGroups();
        Task<AudienceGroupMasterModel> GetAudientMansterGroupById(int Id);
        Task<int> CreateAudientMansterGroup(AudienceGroupMasterModel group);
    }
}
