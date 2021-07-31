using System.Threading.Tasks;
using CartService.Models.Base;

namespace CartService.Models.Carts
{
    public interface ICartRepository:IRepository<Cart>
    {
       Task<int> AddProduct(int cartId, int productId);
       Task<int> DeleteProduct(int cartId, int productId);
    }
}
