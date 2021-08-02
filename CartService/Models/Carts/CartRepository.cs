using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CartService.Models.Products;
using Dapper;

namespace CartService.Models.Carts
{
    public class CartRepository : ICartRepository
    {
        private readonly string _connectionString;

        public CartRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreateAsync(Cart cart)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspCreateCart = "uspCreateCart";

                await db.ExecuteAsync(uspCreateCart, new {cart.Id, cart.Name, cart.CreatedDateTime},
                    commandType: CommandType.StoredProcedure);

                await AddToCartProduct(db, cart);
            }
        }

        public async Task<Cart> GetAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspGetCartById = "uspGetCartById";

                var cart = await db.QueryFirstOrDefaultAsync<Cart>(uspGetCartById, new { id },
                    commandType: CommandType.StoredProcedure);

                if (cart == null) return null;
                
                var uspGetProductsByCartId = "uspGetProductsByCartId";

                cart.Products = await db.QueryAsync<Product>(uspGetProductsByCartId, new { id },
                    commandType: CommandType.StoredProcedure);

                return cart;
            }
        }

        public async Task<IEnumerable<Cart>> GetAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspGetCarts = "uspGetCarts";

                var carts = await db.QueryAsync<Cart>(uspGetCarts,
                    commandType: CommandType.StoredProcedure);

                if (carts == null) return carts; 
                
                foreach (var cart in carts)
                {
                    var uspGetProductsByCartId = "uspGetProductsByCartId";
                    var id = cart.Id;
                    cart.Products = await db.QueryAsync<Product>(uspGetProductsByCartId, new { id },
                        commandType: CommandType.StoredProcedure);
                }
                
                return carts;
            }
        }

        public async Task UpdateAsync(Cart cart)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspUpdateCart = "uspUpdateCart";

                await db.ExecuteAsync(uspUpdateCart, new { cart.Id, cart.Name, cart.CreatedDateTime },
                    commandType: CommandType.StoredProcedure);

                var uspDeleteCartProductsByCartId = "uspDeleteCartProductsByCartId";

                await db.ExecuteAsync(uspDeleteCartProductsByCartId, new {cart.Id},
                    commandType: CommandType.StoredProcedure);

                await AddToCartProduct(db,cart);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var uspDeleteCartById = "uspDeleteCartById";
            
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync(uspDeleteCartById, new { id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task AddProduct(int cartId, int productId)
        {
            var uspAddToCartProduct = "uspAddToCartProduct";
            
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync(uspAddToCartProduct, new { cartId, productId },
                    commandType: CommandType.StoredProcedure); 
            }
        }

        public async Task DeleteProduct(int cartId, int productId)
        {
            var uspDeleteFromCartProduct = "uspDeleteFromCartProduct";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync(uspDeleteFromCartProduct, new {cartId, productId},
                    commandType: CommandType.StoredProcedure);
            }
        }

        private async Task AddToCartProduct(IDbConnection db, Cart cart)
        {
            var uspAddToCartProduct = "uspAddToCartProduct";

            var cartId = cart.Id;

            foreach (var product in cart.Products)
            {
                var productId = product.Id;
                await db.ExecuteAsync(uspAddToCartProduct, new { cartId, productId },
                                      commandType: CommandType.StoredProcedure);
            }
        }
    }
}
