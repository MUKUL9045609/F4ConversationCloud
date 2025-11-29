using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace F4ConversationCloud.Infrastructure.Scheduler
{
    public class TemplateAnalyticsSyncService : BackgroundService
    {
        //private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);
        private readonly IServiceProvider _serviceProvider;
        public TemplateAnalyticsSyncService(IServiceProvider serviceProvider) { 
        
        
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken )
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;

                if (now.Hour == 0 && now.Minute == 0)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var syncservice = scope.ServiceProvider.GetRequiredService<ITemplateRepositories>();
                        await syncservice.MetaTemplateAnalyticsSync();
                    }
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
