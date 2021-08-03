using CartService.Reporting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using CartService.Logging;
using Microsoft.Extensions.Logging;

namespace CartService.BackgroundServices
{
    public class ReportingService : BackgroundService
    {
        private ILogger<FileLogger> _fileLogger;
        private readonly IReport _cartReport;

        public ReportingService(ILogger<FileLogger> fileLogger,IReport cartReport)
        {
            _fileLogger = fileLogger ?? throw new ArgumentNullException(nameof(fileLogger));
            _cartReport = cartReport;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _fileLogger.LogInformation("ReportingService is starting");
            
            stoppingToken.Register(() =>_fileLogger.LogInformation("ReportingService is stopping"));

            while (!stoppingToken.IsCancellationRequested)
            {
                _fileLogger.LogInformation("ReportingService is doing background work");

                await _cartReport.Create();
                
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }

            _fileLogger.LogInformation("ReportingService background task is stopping");
        }
    }
}
