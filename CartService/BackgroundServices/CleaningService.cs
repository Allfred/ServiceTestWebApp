using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CartService.Logging;
using CartService.Models.Carts;
using CartService.Models.WebHook.CartDeleting;
using Microsoft.Extensions.Logging;

namespace CartService.BackgroundServices
{
    public class CleaningService : BackgroundService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartDeletingWebHook _cartDeletingWebHook;
        private readonly ILogger<FileLogger> _fileLogger;

        public CleaningService(ICartRepository cartRepository, ICartDeletingWebHook cartDeletingWebHook, ILogger<FileLogger> fileLogger)
        {
            _fileLogger = fileLogger ?? throw new ArgumentNullException(nameof(fileLogger));
            _cartRepository = cartRepository;
            _cartDeletingWebHook = cartDeletingWebHook;
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
            var carts = (await _cartRepository.GetAsync())
                .Where(cart => (dateTimeNow - cart.CreatedDateTime).Days > days)
                .Select(x => x.Id);

            foreach (var id in carts)
            {
                await _cartRepository.DeleteAsync(id);
                await _cartDeletingWebHook.Execute(id);
            }
        }
    }
}
