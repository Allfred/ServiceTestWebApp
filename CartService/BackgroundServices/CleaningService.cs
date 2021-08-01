using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CartService.Logging;
using CartService.Models.Carts;
using CartService.Models.WebHook;
using Microsoft.Extensions.Logging;

namespace CartService.BackgroundServices
{
    public class CleaningService : BackgroundService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IDelCartWebHook _delCartWebHook;
        private readonly ILogger<FileLogger> _fileLogger;

        public CleaningService(ICartRepository cartRepository, IDelCartWebHook delCartWebHook, ILogger<FileLogger> fileLogger)
        {
            _fileLogger = fileLogger ?? throw new ArgumentNullException(nameof(fileLogger));
            _cartRepository = cartRepository;
            _delCartWebHook = delCartWebHook;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _fileLogger.LogInformation("CleaningService is starting");

            stoppingToken.Register(() => _fileLogger.LogInformation("CleaningService is stopping"));

            while (!stoppingToken.IsCancellationRequested)
            {
                _fileLogger.LogInformation("CleaningService is doing background work");

                await CleanOldCarts();

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }

            _fileLogger.LogInformation("CleaningService background task is stopping");
        }

        private async Task CleanOldCarts()
        {
            var days = 30;
            var dateTimeNow = DateTime.Now;
            var carts = _cartRepository.GetAsync()
                .Result
                .Where(cart => (dateTimeNow - cart.CreatedDateTime).Days > days)
                .Select(x => x.Id);
            
            foreach (var id in carts)
            {
                await _cartRepository.DeleteAsync(id);
                await _delCartWebHook.Execute(id);
            }
        }
    }
}
