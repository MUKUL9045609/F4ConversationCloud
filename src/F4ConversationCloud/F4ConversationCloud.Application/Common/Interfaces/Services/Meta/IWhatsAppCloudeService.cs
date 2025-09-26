﻿using F4ConversationCloud.Application.Common.Models.MetaModel.BussinessProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.Meta
{
    public interface IWhatsAppCloudeService
    {
        Task<WhatsAppPhoneNumberInfoViewModel> GetWhatsAppPhoneNumberDetailsAsync(string phoneNumberId);
    }
}
