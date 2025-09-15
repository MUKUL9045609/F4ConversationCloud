using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using Dapper;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;

namespace F4ConversationCloud.Infrastructure.Repositories
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly DbContext _context;
        private readonly IGenericRepository<Campaign> _repository;
        public CampaignRepository(IGenericRepository<Campaign> repository)
        {
            _repository = repository;
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
    }
}
