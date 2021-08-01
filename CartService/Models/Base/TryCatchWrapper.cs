using System;
using System.Threading.Tasks;
using CartService.Logging;
using Microsoft.Extensions.Logging;

namespace CartService.Models.Base
{
    public class TryCatchWrapper: ITryCatchWrapper
    {
        private readonly ILogger<FileLogger> _fileLogger;

        public TryCatchWrapper(ILogger<FileLogger> fileLogger)
        {
            _fileLogger = fileLogger ?? throw new ArgumentNullException(nameof(fileLogger));
        }
        
        public async Task<bool> Execute(Func<Task<int>> func)
        {
            try
            {
                await func.Invoke();
            }
            catch (Exception e)
            {
                _fileLogger.LogError(e.Message);
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
        
        public async Task<bool> Execute<T>(Func<T,Task> func, T item)
        {
            try
            {
                await func.Invoke(item);
            }
            catch (Exception e)
            {
                _fileLogger.LogError(e.Message);
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
        
        public async Task<bool> Execute<T>(Func<T,Task<int>> func, T item)
        {
            try
            {
                await func.Invoke(item);
            }
            catch (Exception e)
            {
                _fileLogger.LogError(e.Message);
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
        
        public async Task<K> Execute<T,K>(Func<T, Task<K>> func, T item) where K: class
        {
            try
            {
                return await func.Invoke(item);
            }
            catch (Exception e)
            {
                _fileLogger.LogError(e.Message);
                return await Task.FromResult<K>(null);
            }
        }
        
        public async Task<K> Execute<K>(Func<Task<K>> func) where K: class
        {
            try
            {
                return await func.Invoke();
            }
            catch (Exception e)
            {
                _fileLogger.LogError(e.Message);
                return await Task.FromResult<K>(null);
            }
        }
    }
}