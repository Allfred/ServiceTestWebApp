using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CartService.Models.Base;
using Dapper;

namespace CartService.Models.Products
{
    public class ProductRepository:IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> CreateAsync(Product product)
        {
            var func = new Func<Task<int>>(async () =>
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var uspCreateProduct = "uspCreateProduct";
                
                    return await db.ExecuteAsync(uspCreateProduct, new { product.Id, product.Name, product.Cost, product.ForBonusPoints},
                        commandType: CommandType.StoredProcedure);
                } 
                
            });

            return await TryCatchWrapper.Execute(func);
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            var func = new Func<Task<IEnumerable<Product>>>(async () =>
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var uspGetProducts = "uspGetProducts";

                    return await db.QueryAsync<Product>(uspGetProducts,
                        commandType: CommandType.StoredProcedure);
                }
            });

            return await TryCatchWrapper.Execute(func);
        }
        
        public async Task<Product> GetAsync(int id)
        {
            var func = new Func<int, Task<Product>>(async id =>
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var uspGetProductById = "uspGetProductById";

                    return await db.QueryFirstOrDefaultAsync<Product>(uspGetProductById, new { id },
                        commandType: CommandType.StoredProcedure);
                
                }
            });
            
            return await TryCatchWrapper.Execute(func, id);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var func = new Func<Task<int>>(async () =>
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var uspUpdateProduct = "uspUpdateProduct";

                    return await db.ExecuteAsync(uspUpdateProduct, new { product.Id, product.Name, product.Cost, product.ForBonusPoints },
                        commandType: CommandType.StoredProcedure);

                }
            });

            return await TryCatchWrapper.Execute(func);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var func = new Func<int, Task<int>>(async id =>
            {
                var uspDeleteProductById = "uspDeleteProductById";
            
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    return await db.ExecuteAsync(uspDeleteProductById, new { id }, commandType: CommandType.StoredProcedure);
                }  
            });

            return await TryCatchWrapper.Execute(func,id);
        }
    }
}