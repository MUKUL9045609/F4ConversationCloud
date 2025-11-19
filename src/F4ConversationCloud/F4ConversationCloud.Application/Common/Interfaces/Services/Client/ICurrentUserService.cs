using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.Client
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Name { get; }
        string? Role { get; }
        string? CurrentUserPhoneNumber { get; }
        string? BusinessId { get; }
        string? ClientInfoId { get; }
        string? CurrentUserEmail { get; }
    }
}
