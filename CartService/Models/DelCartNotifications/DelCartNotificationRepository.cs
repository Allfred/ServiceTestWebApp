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
        
        public async Task<int> CreateAsync(DelCartNotification item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspCreateDelCartNotification = "uspCreateDelCartNotification";
                
                var result = await db.ExecuteAsync(uspCreateDelCartNotification, new { item.Message },
                    commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public Task<DelCartNotification> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<DelCartNotification>> GetAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateAsync(DelCartNotification item)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}