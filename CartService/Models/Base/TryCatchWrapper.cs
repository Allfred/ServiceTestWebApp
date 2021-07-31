using System;
using System.Threading.Tasks;

namespace CartService.Models.Base
{
    public static class TryCatchWrapper
    {
        public static async Task<bool> Execute(Func<Task<int>> func)
        {
            try
            {
                await func.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
        
        public static async Task<bool> Execute<T>(Func<T,Task> func, T item)
        {
            try
            {
                await func.Invoke(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
        
        public static async Task<bool> Execute<T>(Func<T,Task<int>> func, T item)
        {
            try
            {
                await func.Invoke(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
        
        public static async Task<K> Execute<T,K>(Func<T, Task<K>> func, T item) where K: class
        {
            try
            {
                return await func.Invoke(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return await Task.FromResult<K>(null);
            }
        }
        
        public static async Task<K> Execute<K>(Func<Task<K>> func) where K: class
        {
            try
            {
                return await func.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return await Task.FromResult<K>(null);
            }
        }
    }
}