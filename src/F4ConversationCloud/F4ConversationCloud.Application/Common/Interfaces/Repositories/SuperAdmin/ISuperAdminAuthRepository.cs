using F4ConversationCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin
{
    public interface ISuperAdminAuthRepository
    {
        Task<Auth> CheckUserExists(string email);
        Task<int> UpdatePasswordAsync(ConfirmPasswordModel model);
    }
}
