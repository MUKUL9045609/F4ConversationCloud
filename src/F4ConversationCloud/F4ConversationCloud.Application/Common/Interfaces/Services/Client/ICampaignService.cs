using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static F4ConversationCloud.Application.Common.Models.ClientModel.CampaignDTO;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.Client
{
    public interface ICampaignService
    {
        Task<int> CreateCampaign(Campaign campaign);
        Task<List<Campaign>> GetCampaigns();
        Task<Campaign> GetCampaignById(int id);
        Task<int> UpdateCampaign(Campaign campaign);
        Task<bool> ActivateCampaign(int id);
        Task<bool> DeactivateCampaign(int id);
        Task<int> UpdateAudientMansterGroupAsync(AudienceGroupMasterModel group);
        Task<IEnumerable<AudienceGroupMasterModel>> GetAllAudientMansterGroupsAsync();
        Task<AudienceGroupMasterModel> GetAudientMansterGroupByIdAsync(int Id);
        Task<int> CreateAudientMansterGroupAsync(AudienceGroupMasterModel group);
        Task<bool> DeactivateAudientMansterGroupAsync(int id);
    }
}
