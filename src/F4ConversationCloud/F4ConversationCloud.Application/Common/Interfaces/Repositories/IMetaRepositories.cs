using F4ConversationCloud.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories
{
    public interface IMetaRepositories
    {
        Task<int> UpdateMetaUsersConfigurationDetails(WhatsAppAccountTableModel request);
    }
}
