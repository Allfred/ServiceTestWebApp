using CartService.Reporting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CartService.Models.Carts;

namespace CartService.BackgroundServices
{
    public class ReportingService : BackgroundService
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly ICartRepository _cartRepository;

        public ReportingService(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, ICartRepository cartRepository)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _cartRepository = cartRepository;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("ReportingService is starting.");

            stoppingToken.Register(() => Console.WriteLine("ReportingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("ReportingService is doing background work.");

                var report = new Report(_hostingEnvironment.ContentRootPath);

                report.Create(_cartRepository);
                
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }

            Console.WriteLine("ReportingService background task is stopping.");
        }
    }
}
