using System.Threading.Tasks;
using CartService.Models.Base;

namespace CartService.Models.Carts
{
    public interface ICartRepository:IRepository<Cart>
    {
       Task<bool> AddProduct(int cartId, int productId);
       Task<bool> DeleteProduct(int cartId, int productId);
    }
}
