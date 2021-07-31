using System.Threading.Tasks;
using CartService.Models.CartSubscribers;

namespace CartService.Models.WebHook
{
    public interface IDelCartWebHook: IWebHook<CartSubscriber>
    {
        Task Execute(int cartId);
    }
}