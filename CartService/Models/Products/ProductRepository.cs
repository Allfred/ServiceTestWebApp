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

        public async Task<int> CreateAsync(Product product)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspCreateProduct = "uspCreateProduct";
                
                var result = await db.ExecuteAsync(uspCreateProduct, new { product.Id, product.Name, product.Cost, product.ForBonusPoints},
                    commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspGetProducts = "uspGetProducts";

                var products = await db.QueryAsync<Product>(uspGetProducts,
                    commandType: CommandType.StoredProcedure);
                
                return products;
            }
        }
        
        public async Task<Product> GetAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {

                var uspGetProductById = "uspGetProductById";

                var product = await db.QueryFirstOrDefaultAsync<Product>(uspGetProductById, new { id },
                    commandType: CommandType.StoredProcedure);
                
                return product;
            }
        }

        public async Task<int> UpdateAsync(Product product)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var uspUpdateProduct = "uspUpdateProduct";

                var result = await db.ExecuteAsync(uspUpdateProduct, new { product.Id, product.Name, product.Cost, product.ForBonusPoints },
                                                      commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var uspDeleteProductById = "uspDeleteProductById";
            
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.ExecuteAsync(uspDeleteProductById, new { id }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}