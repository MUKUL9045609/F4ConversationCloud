using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin
{
    public interface ISuperAdminAuthService
    {
        Task<Auth> CheckUserExists(string email);
        Task<bool> ValidateUserName(string userName);
        Task<bool> ConfirmPassword(ConfirmPasswordModel model);
        Task SendPasswordResetLink(string userName);
        Task<bool> CheckValidRole(IEnumerable<string> roles, string email);
    }
}
