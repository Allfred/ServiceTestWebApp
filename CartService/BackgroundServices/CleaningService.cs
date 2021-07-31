using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CartService.Models.Carts;
using CartService.Models.WebHook;

namespace CartService.BackgroundServices
{
    public class CleaningService : BackgroundService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IDelCartWebHook _delCartWebHook;

        public CleaningService(ICartRepository cartRepository, IDelCartWebHook delCartWebHook)
        {
            _cartRepository = cartRepository;
            _delCartWebHook = delCartWebHook;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("CleaningService is starting.");

            stoppingToken.Register(() => Console.WriteLine("CleaningService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("CleaningService is doing background work.");

                await CleanOldCarts();

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }

            Console.WriteLine("CleaningService background task is stopping.");
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
