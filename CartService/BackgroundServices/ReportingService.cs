using CartService.Reporting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using CartService.Logging;
using CartService.Models.Carts;
using Microsoft.Extensions.Logging;

namespace CartService.BackgroundServices
{
    public class ReportingService : BackgroundService
    {
        private readonly ICartRepository _cartRepository;
        private ILogger<FileLogger> _fileLogger;
        private readonly IReport<ICartRepository> _cartRepositoryReport;

        public ReportingService(ICartRepository cartRepository, ILogger<FileLogger> fileLogger,IReport<ICartRepository> cartRepositoryReport)
        {
            _fileLogger = fileLogger ?? throw new ArgumentNullException(nameof(fileLogger));
            _cartRepositoryReport = cartRepositoryReport;
            _cartRepository = cartRepository;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _fileLogger.LogInformation("ReportingService is starting");
            
            stoppingToken.Register(() =>_fileLogger.LogInformation("ReportingService is stopping"));

            while (!stoppingToken.IsCancellationRequested)
            {
                _fileLogger.LogInformation("ReportingService is doing background work");

                _cartRepositoryReport.Create(_cartRepository);
                
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }

            _fileLogger.LogInformation("ReportingService background task is stopping");
        }
    }
}
