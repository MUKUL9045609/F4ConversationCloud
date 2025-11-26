using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Infrastructure.Repositories.SuperAdmin;
using Microsoft.AspNetCore.Http.Timeouts;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Operators;
using Polly.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class UsageAndBillingService: IUsageAndBillingService
    {
       private readonly IUsageAndBillingRepository _usageAndBillingRepository;
        private readonly ILogService _logService;
        public UsageAndBillingService(IUsageAndBillingRepository usageAndBillingRepository, ILogService logService)
        {
            _usageAndBillingRepository = usageAndBillingRepository;
            _logService = logService;
        }
        public async Task<UsageModelResponse> GetUsageListAsync(UsageListFilter filter)
        {
            try
            {

                var (list, count) = await _usageAndBillingRepository.GetUsageDetailsAsync(filter);

                var usageModels = new List<UsageModel>();

                foreach (var item in list)
                {
                    var phoneNumbers = (item.phoneNumberId ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .ToList();

                    var templateResults = new List<TemplateMessageInsightsListViewItem>();
                    foreach (var phone in phoneNumbers)
                    {
                        var result = await _usageAndBillingRepository.GetTemplateMessageInsightsListAsync(new TemplateMessageInsightsFilter
                        {
                            StartDate = filter.StartDate,
                            EndDate = filter.EndDate,
                            PhoneNumberId = phone
                        });

                        templateResults.AddRange(result);
                    }
                    var phoneIds = string.Join(",", phoneNumbers);
                    usageModels.Add(new UsageModel
                    {
                        SrNo = item.SrNo,
                        OrganizationsName = item.OrganizationsName,
                        ClientInfoId = item.ClientInfoId,
                        ClientId = item.ClientId,
                        WabaPhoneNumber = string.Join(",", (item.WabaPhoneNumber ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())),
                        WhatsAppDisplayName = string.Join(",", (item.WhatsAppDisplayName ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())),
                        MetaConfigid = string.Join(",", (item.MetaConfigid ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())),
                        phoneNumberId = phoneIds,
                        TemplateInsightsList = templateResults
                    });
                }
                return new UsageModelResponse
                {
                    TotalCount = count,
                    usageModelsItems = usageModels
                };
            }
            catch (Exception ex)
            {
                
                var logModel = new LogModel();
                logModel.Source = "UsageAndBilling/GetUsageDetailsAsync";
                logModel.AdditionalInfo = $"Model: {JsonConvert.SerializeObject(filter)}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
                return new UsageModelResponse
                {
                    TotalCount = 0,
                    usageModelsItems = null
                };
            }
        }
        
        public async Task<BillingListModelResponse> GetBillingListAsync(BillingListFilter filter)
        {
            try
            {
                var (list, count) = await _usageAndBillingRepository.GetBillingListAsync(filter);

                var BillingModel = new List<BillingListItem>();

                foreach (var item in list)
                {
                    var phoneNumbers = (item.phoneNumberId ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .ToList();

                    var templateResults = new List<TemplateMessageInsightsListViewItem>();
                    foreach (var phone in phoneNumbers)
                    {
                        var result = await _usageAndBillingRepository.GetTemplateMessageInsightsListAsync(new TemplateMessageInsightsFilter
                        {
                            StartDate = filter.StartDate,
                            EndDate = filter.EndDate,
                            PhoneNumberId = phone
                        });

                        int totalMessageSentCount = result.Sum(x => x.TotalMessageSentCount);
                        decimal totalAmount = result.Sum(x => x.TotalAmount);

                        templateResults.Add(new TemplateMessageInsightsListViewItem
                        {  
                            ConversationType = "All",
                            TotalMessageSentCount = totalMessageSentCount,
                            TotalAmount = totalAmount
                        });

                    }
                    var phoneIds = string.Join(",", phoneNumbers);

                    BillingModel.Add(new BillingListItem
                    {
                        SrNo = item.SrNo,
                        OrganizationsName = item.OrganizationsName,
                        ClientInfoId = item.ClientInfoId,
                        ClientId = item.ClientId,
                        WabaPhoneNumber = string.Join(",", (item.WabaPhoneNumber ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())),
                        WhatsAppDisplayName = string.Join(",", (item.WhatsAppDisplayName ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())),
                        MetaConfigid = string.Join(",", (item.MetaConfigid ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())),
                        phoneNumberId = string.Join(",", (item.phoneNumberId ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())),
                        TemplateInsightsList = templateResults,

                    });
                }
                    return new BillingListModelResponse
                    {
                        TotalCount = count,
                        billingListItems = BillingModel
                    };
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "UsageAndBilling/GetBillingListAsync";
                logModel.AdditionalInfo = $"Model: {JsonConvert.SerializeObject(filter)}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
                return new BillingListModelResponse
                {
                    TotalCount = 0,
                    billingListItems = null
                };
            }
        }


        public async Task<IEnumerable<TemplateMessageInsightsListViewItem>> GetTemplateMessageInsightsListAsync(TemplateMessageInsightsFilter filter) {
            try
            {
                return  await _usageAndBillingRepository.GetTemplateMessageInsightsListAsync(filter);
            }
            catch (Exception ex )
            {

                var logModel = new LogModel();
                logModel.Source = "UsageAndBilling/GetTemplateInsightsList";
                logModel.AdditionalInfo = $"Model: {JsonConvert.SerializeObject(filter)}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);

                return Enumerable.Empty<TemplateMessageInsightsListViewItem>();
            }
            
        }


        public async Task<InvoiceResponse> GenerateInvoiceAsync(InvoiceRequest request) {
            try
            {
                var filter = new TemplateMessageInsightsFilter
                {
                    PhoneNumberId = request.PhoneNumberId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate
                };
                var templateMessageInsights = await _usageAndBillingRepository.GetTemplateMessageInsightsListAsync(filter);
                var invoiceDetails = await _usageAndBillingRepository.GetInvoiceDetailsAsync(request);
                
                return  new InvoiceResponse
                {
                    InvoiceDetails = invoiceDetails,
                    TemplateMessageInsights = templateMessageInsights
                };
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "UsageAndBilling/GenerateInvoiceAsync";
                logModel.AdditionalInfo = $"Model: {JsonConvert.SerializeObject(request)}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);

                return new InvoiceResponse();
            }
        
        }

    }
}

