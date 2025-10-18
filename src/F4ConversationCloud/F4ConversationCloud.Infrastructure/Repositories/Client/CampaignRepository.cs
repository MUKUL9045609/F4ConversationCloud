using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Client;
using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;
using Twilio.Rest.Api.V2010.Account.Usage.Record;

namespace F4ConversationCloud.Infrastructure.Repositories.Client
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly DbContext _context;
        private readonly IGenericRepository<Campaign> _repository;
        private readonly IGenericRepository<AudienceGroupMasterModel> _audienceGroupRepository;
        public CampaignRepository(IGenericRepository<Campaign> repository, IGenericRepository<AudienceGroupMasterModel> audienceGroupRepository)
        {
            _repository = repository;
            _audienceGroupRepository = audienceGroupRepository;
        }

        public async Task<int> CreateCampaign(Campaign campaign)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("fileName", campaign.FileName);
            parameters.Add("fileUrl", campaign.FileUrl);
            parameters.Add("fromDate", campaign.FromDate);
            parameters.Add("toDate", campaign.ToDate);
            parameters.Add("message", campaign.Message);

            return await _repository.InsertUpdateAsync("sp_Create_Campaign", parameters);
        }

        public async Task<List<Campaign>> GetCampaigns()
        {
            DynamicParameters parameters = new DynamicParameters();
            return (await _repository.GetListByParamAsync<Campaign>("sp_GetCampaigns", parameters)).ToList();
        }

        public async Task<Campaign> GetCampaignById(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("id", id);

            return await _repository.GetByValuesAsync("sp_GetCampaignById", parameters);
        }

        public async Task<int> UpdateCampaign(Campaign campaign)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("id", campaign.Id);
            parameters.Add("fileName", campaign.FileName);
            parameters.Add("fileUrl", campaign.FileUrl);
            parameters.Add("fromDate", campaign.FromDate);
            parameters.Add("toDate", campaign.ToDate);
            parameters.Add("message", campaign.Message);

            return await _repository.InsertUpdateAsync("sp_Update_Campaign", parameters);
        }

        public async Task<bool> ActivateCampaign(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", id);

            return await _repository.RestoreAsync("sp_ActivateCampaign", parameters);
        }

        public async Task<bool> DeactivateCampaign(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", id);

            return await _repository.DeleteAsync("sp_DeactivateCampaign", parameters);
        }


        public async Task<int> CreateAudientMansterGroup(AudienceGroupMasterModel group)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("groupName", group.GroupName);
                parameters.Add("exelFileUrl", group.ExelFileUrl);
                parameters.Add("exelFileName", group.ExelFileName);
                parameters.Add("clientInfoId", group.ClientInfoId);
                parameters.Add("createdBy", group.CreatedBy);


                return await _repository.InsertUpdateAsync("sp_Create_AudientMansterGroup", parameters);
            }
            catch (Exception)
            {

                return 0;
            }

        }
        public async Task<AudienceGroupMasterModel> GetAudientMansterGroupById(int Id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                return await _audienceGroupRepository.GetByValuesAsync("sp_GetAudientMansterGroupById", parameters);
            }
            catch (Exception)
            {

                return new AudienceGroupMasterModel();
            }

        }
        public async Task<IEnumerable<AudienceGroupMasterModel>> GetAllAudientMansterGroups()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                return await _audienceGroupRepository.GetListByParamAsync<AudienceGroupMasterModel>("sp_GetAllAudientMansterGroups", parameters);
            }
            catch (Exception)
            {
                return Enumerable.Empty<AudienceGroupMasterModel>();
            }

        }
        public async Task<int> UpdateAudientMansterGroup(AudienceGroupMasterModel group)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("id", group.Id);
                parameters.Add("groupName", group.GroupName);
                parameters.Add("exelFileUrl", group.ExelFileUrl);
                parameters.Add("exelFileName", group.ExelFileName);
                parameters.Add("updatedBy", group.UpdatedBy);
                return await _repository.InsertUpdateAsync("sp_Update_AudientMansterGroup", parameters);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<bool> DeactivateAudientMansterGroup(int id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", id);
                return await _audienceGroupRepository.DeleteAsync("sp_DeactivateAudientMansterGroup", parameters);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
