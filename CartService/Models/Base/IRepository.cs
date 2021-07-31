using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartService.Models.Base
{
    public interface IRepository<T> where T:class
    {
        Task<int> CreateAsync(T item);
        
        Task<T> GetAsync(int id);
        
        Task<IEnumerable<T>> GetAsync();

        Task<int> UpdateAsync(T item);

        Task<int> DeleteAsync(int id);

    }
}
