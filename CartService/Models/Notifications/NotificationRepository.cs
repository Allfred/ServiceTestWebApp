using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace CartService.Models.Notifications
{
    public class NotificationRepository: INotificationRepository
    {
        private readonly string _connectionString;

        public NotificationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task CreateAsync(Notification item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspCreateNotification = "uspCreateNotification";
                
                await db.ExecuteAsync(uspCreateNotification, new { item.Message },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public Task<Notification> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notification>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Notification item)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}