using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CartService.Models.Base;
using Dapper;

namespace CartService.Models.CartSubscribers
{
    public class CartSubscriberRepository: ICartSubscriberRepository
    {
        private readonly string _connectionString;

        public CartSubscriberRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task<bool> CreateAsync(CartSubscriber cartSubscriber)
        {
            var func = new Func<Task<int>>(async () =>
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var uspCreateCartSubscriber = "uspCreateCartSubscriber";
                
                    return await db.ExecuteAsync(uspCreateCartSubscriber, new { cartSubscriber.CartId, cartSubscriber.ExecutingUrl},
                        commandType: CommandType.StoredProcedure);

                }
            });
            
            return await TryCatchWrapper.Execute(func);
        }

        public async Task<CartSubscriber> GetAsync(int id)
        {
            var func = new Func<int, Task<CartSubscriber>>(async id =>
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {

                    var uspGetCartSubscriberById = "uspGetCartSubscriberById";

                    var cartSubscriber = await db.QueryFirstOrDefaultAsync<CartSubscriber>(uspGetCartSubscriberById, new { id },
                        commandType: CommandType.StoredProcedure);
                
                    return cartSubscriber;
                }
            });

            return await TryCatchWrapper.Execute(func, id);
        }

        public async Task<IEnumerable<CartSubscriber>> GetAsync()
        {

            var func = new Func<Task<IEnumerable<CartSubscriber>>>(async () =>
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var uspGetCartSubscribers = "uspGetCartSubscribers";

                    var cartSubscribers = await db.QueryAsync<CartSubscriber>(uspGetCartSubscribers, 
                        commandType: CommandType.StoredProcedure);
                
                    return cartSubscribers;
                }
            });
            return await TryCatchWrapper.Execute(func);
        }

        public Task<bool> UpdateAsync(CartSubscriber item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var func = new Func<int, Task<int>>(async id =>
            {
                var uspDeleteCartSubscriberById = "uspDeleteCartSubscriberById";
            
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    return await db.ExecuteAsync(uspDeleteCartSubscriberById, new {id}, commandType: CommandType.StoredProcedure);
                }  
            });

            return await TryCatchWrapper.Execute(func,id);
        }
    }
}