using System.Threading.Tasks;
using CartService.Models.Base;

namespace CartService.Models.WebHook.Common
{
    public interface IWebHookRepository:IRepository<WebHookProtocol>
    {   
        /// <summary>
        /// Get webhook by itemId and webhooktype
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="webHookType"></param>
        /// <returns></returns>
        Task<WebHookProtocol> GetAsync(int itemId, WebHookType webHookType);
    }
}