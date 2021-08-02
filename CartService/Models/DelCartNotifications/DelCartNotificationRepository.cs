using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace CartService.Models.DelCartNotifications
{
    public class DelCartNotificationRepository: IDelCartNotificationRepository
    {
        private readonly string _connectionString;

        public DelCartNotificationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task CreateAsync(DelCartNotification item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspCreateDelCartNotification = "uspCreateDelCartNotification";
                
                await db.ExecuteAsync(uspCreateDelCartNotification, new { item.Message },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public Task<DelCartNotification> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DelCartNotification>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(DelCartNotification item)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}