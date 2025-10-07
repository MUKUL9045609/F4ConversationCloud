using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.Domain.Enum
{
    public enum SuperAdminRole
    {
        [Description("Admin")]
        [Display(Name = "Admin")]
        Admin = 1
    }
}
