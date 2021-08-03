using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CartService.Models.WebHook.Common;

namespace CartService.Models.WebHook.CartDeleting
{
    public class CartDeletingWebHook: ICartDeletingWebHook
    {
        private readonly IWebHookRepository _webHookRepository;
        private readonly HttpClient _client;

        public CartDeletingWebHook(IWebHookRepository webHookRepository, IHttpClientFactory httpClientFactory)
        {
            _webHookRepository = webHookRepository;
            _client = httpClientFactory.CreateClient();
        }

        public WebHookType WebHookType => WebHookType.CartDeleting;

        public async Task Execute(int cartId)
        {
            var webHook = await _webHookRepository.GetAsync(cartId, WebHookType);

            if (webHook != null)
            {
                var message = $"Cart:{cartId} was deleted";

                string jsonRequest = JsonSerializer.Serialize(message);
                
                await PostRequestAsync(webHook.ExecutingUri, jsonRequest);
            }
        }
        
        private async Task PostRequestAsync(string url, string json)
        {
            using HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _client.PostAsync(url, content).ConfigureAwait(false);
            await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}