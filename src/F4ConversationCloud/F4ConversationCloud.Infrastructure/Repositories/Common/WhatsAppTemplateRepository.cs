using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Repositories.Common
{
    public class WhatsAppTemplateRepository: IWhatsAppTemplateRepository
    {
        private readonly IGenericRepository<string> _repository;
        public WhatsAppTemplateRepository(IGenericRepository<string> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<WhatsappTemplateList>> GetTemplatesListAsync(WhatsappTemplateListFilter filter)
        {
            try
            {
                DynamicParameters Dp = new DynamicParameters();

                Dp.Add("@SearchText", filter.SearchText);
                Dp.Add("@LanguageCode", filter.LanguageCode);
                Dp.Add("@Status", filter.Status);
                Dp.Add("@PageNumber", filter.PageNumber);
                Dp.Add("@PageSize", filter.PageSize);



                return await _repository.GetListByValuesAsync<WhatsappTemplateList>("[sp_GetWhatsappTemplateList]", Dp);
            }
            catch (Exception)
            {

                return Enumerable.Empty<WhatsappTemplateList>();
            }
           
        }
    }
}
