using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace CartService.Models.WebHook.Common
{
    public class WebHookRepository: IWebHookRepository
    {
        private readonly string _connectionString;

        public WebHookRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public  async Task CreateAsync(WebHookProtocol item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspCreateWebHook = "uspCreateWebHook";
                
                await db.ExecuteAsync(uspCreateWebHook, new {item.Id, item.WebHookType, item.ItemId , item.ExecutingUri},
                    commandType: CommandType.StoredProcedure);
            } 
        }

        public async Task<WebHookProtocol> GetAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspGetWebHookById = "uspGetWebHookById";

                return await db.QueryFirstOrDefaultAsync<WebHookProtocol>(uspGetWebHookById, new { id },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<WebHookProtocol>> GetAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspGetWebHooks = "uspGetWebHooks";

                return await db.QueryAsync<WebHookProtocol>(uspGetWebHooks,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateAsync(WebHookProtocol item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspUpdateWebHook = "uspUpdateWebHook";

                await db.ExecuteAsync(uspUpdateWebHook, new {item.Id, item.WebHookType, item.ItemId, item.ExecutingUri},
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspDeleteWebHookById = "uspDeleteWebHookById";

                await db.ExecuteAsync(uspDeleteWebHookById, new { id }, commandType: CommandType.StoredProcedure);
            } 
        }
        
        public async Task<WebHookProtocol> GetAsync(int itemId, WebHookType webHookType)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspGetWebHookByItemIdAndType = "uspGetWebHookByItemIdAndType";

                return await db.QueryFirstOrDefaultAsync<WebHookProtocol>(uspGetWebHookByItemIdAndType, new { itemId, webHookType },
                    commandType: CommandType.StoredProcedure);
            }
        }

    }
}