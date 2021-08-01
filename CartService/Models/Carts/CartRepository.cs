using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CartService.Models.Base;
using CartService.Models.Products;
using Dapper;

namespace CartService.Models.Carts
{
    public class CartRepository : ICartRepository
    {
        private readonly string _connectionString;
        private readonly ITryCatchWrapper _tryCatchWrapper;

        public CartRepository(string connectionString, ITryCatchWrapper tryCatchWrapper)
        {
            _connectionString = connectionString;
            _tryCatchWrapper = tryCatchWrapper;
        }

        public async Task<bool> CreateAsync(Cart cart)
        {
            var func = new Func<Cart,Task>(async cart =>
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var uspCreateCart = "uspCreateCart";

                    await db.ExecuteAsync(uspCreateCart, new {cart.Id, cart.Name, cart.CreatedDateTime},
                        commandType: CommandType.StoredProcedure);

                    await AddToCartProduct(db, cart);
                }
            });
                 
            return await _tryCatchWrapper.Execute(func, cart);
        }

        public async Task<Cart> GetAsync(int id)
        {
            var func = new Func<int, Task<Cart>>(async id =>
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
            });

            return await _tryCatchWrapper.Execute(func, id);
        }

        public async Task<IEnumerable<Cart>> GetAsync()
        {
            var func = new Func<Task<IEnumerable<Cart>>>(async () =>
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
            });

            return await _tryCatchWrapper.Execute(func);
        }

        public async Task<bool> UpdateAsync(Cart cart)
        {
            var func = new Func<Cart, Task<int>>(async cart =>
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var uspUpdateCart = "uspUpdateCart";

                    var result = await db.ExecuteAsync(uspUpdateCart, new { cart.Id, cart.Name, cart.CreatedDateTime },
                        commandType: CommandType.StoredProcedure);

                    var uspDeleteCartProductsByCartId = "uspDeleteCartProductsByCartId";

                    await db.ExecuteAsync(uspDeleteCartProductsByCartId, new {cart.Id},
                        commandType: CommandType.StoredProcedure);

                    await AddToCartProduct(db,cart);

                    return result;
                }
            });
            
            return await _tryCatchWrapper.Execute(func, cart);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var func = new Func<Task<int>>(async () =>
            {
                var uspDeleteCartById = "uspDeleteCartById";
            
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    return await db.ExecuteAsync(uspDeleteCartById, new { id }, commandType: CommandType.StoredProcedure);
                }
            });

            return await _tryCatchWrapper.Execute(func);
        }

        public async Task<bool> AddProduct(int cartId, int productId)
        {
            var func = new Func<(int cartId,int productId),Task<int>>(async item =>
            {
                var uspAddToCartProduct = "uspAddToCartProduct";
            
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    return await db.ExecuteAsync(uspAddToCartProduct, new { item.cartId, item.productId },
                        commandType: CommandType.StoredProcedure);
                }
            });

            return await _tryCatchWrapper.Execute(func, (cartId, productId));
        }

        public async Task<bool> DeleteProduct(int cartId, int productId)
        {
            var func = new Func<(int cartId, int productId), Task<int>>(async item =>
            {
                var uspDeleteFromCartProduct = "uspDeleteFromCartProduct";

                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                   return await db.ExecuteAsync(uspDeleteFromCartProduct, new {item.cartId, item.productId},
                        commandType: CommandType.StoredProcedure);
                }
            });
            
            return await _tryCatchWrapper.Execute(func, (cartId, productId));
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
