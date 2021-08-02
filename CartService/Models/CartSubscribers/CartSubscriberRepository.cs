using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
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
        
        public async Task CreateAsync(CartSubscriber cartSubscriber)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspCreateCartSubscriber = "uspCreateCartSubscriber";
                
                await db.ExecuteAsync(uspCreateCartSubscriber, new { cartSubscriber.CartId, cartSubscriber.ExecutingUrl},
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<CartSubscriber> GetAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {

                var uspGetCartSubscriberById = "uspGetCartSubscriberById";

                var cartSubscriber = await db.QueryFirstOrDefaultAsync<CartSubscriber>(uspGetCartSubscriberById, new { id },
                    commandType: CommandType.StoredProcedure);
                
                return cartSubscriber;
            }
        }

        public async Task<IEnumerable<CartSubscriber>> GetAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspGetCartSubscribers = "uspGetCartSubscribers";

                var cartSubscribers = await db.QueryAsync<CartSubscriber>(uspGetCartSubscribers, 
                    commandType: CommandType.StoredProcedure);
                
                return cartSubscribers;
            }
        }

        public Task UpdateAsync(CartSubscriber item)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            var uspDeleteCartSubscriberById = "uspDeleteCartSubscriberById";
            
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync(uspDeleteCartSubscriberById, new {id}, commandType: CommandType.StoredProcedure);
            }  
        }
    }
}