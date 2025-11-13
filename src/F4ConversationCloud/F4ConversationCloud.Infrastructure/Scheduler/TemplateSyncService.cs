using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Scheduler
{
    public class TemplateSyncService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);

        public TemplateSyncService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _interval = configuration["TemplateSync:APIIntervalInMinutes"] is not null ? TimeSpan.FromMinutes(Convert.ToInt32(configuration["TemplateSync:APIIntervalInMinutes"])) : TimeSpan.FromMinutes(10);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _ = Task.Run(async () =>
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var SyncService = scope.ServiceProvider.GetRequiredService<ITemplateRepositories>();
                        await SyncService.MetaSyncTemplate();
                    }
                }, stoppingToken);

                try
                {
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (TaskCanceledException ex)
                {

                }
            }
        }
    }
}
