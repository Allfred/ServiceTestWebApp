using System.Threading.Tasks;
using CartService.Models.Base;

namespace CartService.Models.Carts
{
    public interface ICartRepository:IRepository<Cart>
    {
       Task AddProduct(int cartId, int productId);
       Task DeleteProduct(int cartId, int productId);
    }
}
