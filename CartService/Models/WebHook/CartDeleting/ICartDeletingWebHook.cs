using System.Threading.Tasks;
using CartService.Models.WebHook.Common;

namespace CartService.Models.WebHook.CartDeleting
{
    public interface ICartDeletingWebHook: IWebHook
    {
        Task Execute(int id);
    }
}