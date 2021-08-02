using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
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

        public async Task CreateAsync(Product product)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspCreateProduct = "uspCreateProduct";
                
                await db.ExecuteAsync(uspCreateProduct, new { product.Id, product.Name, product.Cost, product.ForBonusPoints},
                    commandType: CommandType.StoredProcedure);
            } 
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspGetProducts = "uspGetProducts";

                return await db.QueryAsync<Product>(uspGetProducts,
                    commandType: CommandType.StoredProcedure);
            }
        }
        
        public async Task<Product> GetAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspGetProductById = "uspGetProductById";

                return await db.QueryFirstOrDefaultAsync<Product>(uspGetProductById, new { id },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateAsync(Product product)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspUpdateProduct = "uspUpdateProduct";

                await db.ExecuteAsync(uspUpdateProduct, new { product.Id, product.Name, product.Cost, product.ForBonusPoints },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspDeleteProductById = "uspDeleteProductById";

                await db.ExecuteAsync(uspDeleteProductById, new { id }, commandType: CommandType.StoredProcedure);
            } 
        }
    }
}