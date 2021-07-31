using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartService.Models.Base
{
    public interface IRepository<T> where T:class
    {
        Task<bool> CreateAsync(T item);
        
        Task<T> GetAsync(int id);
        
        Task<IEnumerable<T>> GetAsync();

        Task<bool> UpdateAsync(T item);

        Task<bool> DeleteAsync(int id);

    }
}
