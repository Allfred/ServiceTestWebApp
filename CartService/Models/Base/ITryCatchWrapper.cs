using System;
using System.Threading.Tasks;

namespace CartService.Models.Base
{
    public interface ITryCatchWrapper
    {
        Task<bool> Execute(Func<Task<int>> func);
        Task<bool> Execute<T>(Func<T, Task> func, T item);
        Task<bool> Execute<T>(Func<T, Task<int>> func, T item);
        Task<K> Execute<T, K>(Func<T, Task<K>> func, T item) where K: class;
        Task<K> Execute<K>(Func<Task<K>> func) where K: class;
    }
}