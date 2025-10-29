using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public class PhoneRegistrationOnMeta
    {
        [Required]
        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Enter valid phone Number Id.")]
        public string PhoneNumberId { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Enter valid pin.")]
        public string Pin { get; set; } = "313466";

        [Required]
        [RegularExpression("^(whatsapp)$", ErrorMessage = "messaging_product must be whatsapp.")]

        public string messaging_product { get; set; } = "whatsapp";
    }


    public class PhoneRegistrationOnMetaResponse
    {
        public string status { get; set; }
    }
}
