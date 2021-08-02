using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartService.Models.Base
{
    public interface IRepository<T> where T:class
    {
        Task CreateAsync(T item);
        
        Task<T> GetAsync(int id);
        
        Task<IEnumerable<T>> GetAsync();

        Task UpdateAsync(T item);

        Task DeleteAsync(int id);
    }
}
