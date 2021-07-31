﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CartService.Models.Base;
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
        
        public async Task<bool> CreateAsync(DelCartNotification item)
        {

            var func = new Func<DelCartNotification, Task<int>>(async item =>
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var uspCreateDelCartNotification = "uspCreateDelCartNotification";
                
                    return await db.ExecuteAsync(uspCreateDelCartNotification, new { item.Message },
                        commandType: CommandType.StoredProcedure);

                }
            });

            return await TryCatchWrapper.Execute(func,item);
        }

        public Task<DelCartNotification> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DelCartNotification>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(DelCartNotification item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}