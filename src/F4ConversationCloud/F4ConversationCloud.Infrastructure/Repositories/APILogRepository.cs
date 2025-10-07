using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Repositories
{
    public class APILogRepository : IAPILogRepository
    {

        private readonly IGenericRepository<string> _repository;

        public APILogRepository(IGenericRepository<string> repository)
        {
            _repository = repository;
        }

        public async Task<int> SaveApiResponseAsync(bool isSuccess, string errorMessage, string rawResponse, int statusCode)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("IsSuccess", isSuccess);
                parameters.Add("ErrorMessage", errorMessage);
                parameters.Add("RawResponse", rawResponse);
                parameters.Add("StatusCode", statusCode);

                var insertedId = await _repository.InsertUpdateAsync("[sp_InsertApiResponseLog]", parameters);

                return insertedId;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
