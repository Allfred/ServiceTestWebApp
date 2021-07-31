using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CartService.Models.CartSubscribers;

namespace CartService.Models.WebHook
{
    public class DelCartWebHook: IDelCartWebHook
    {
        private readonly ICartSubscriberRepository _cartSubscriberRepository;
        private readonly HttpClient _client;


        public DelCartWebHook(ICartSubscriberRepository cartSubscriberRepository,IHttpClientFactory httpClientFactory)
        {
            _cartSubscriberRepository = cartSubscriberRepository;
            _client = httpClientFactory.CreateClient();
        }

        public async Task Execute(int cartId)
        {

            var cartSubscriber = _cartSubscriberRepository.GetAsync(cartId);

            if (cartSubscriber.Result != null)
            {
                var message = $"Cart:{cartId} was deleted";
                
                string jsonRequest = JsonSerializer.Serialize(message);
                
                await PostRequestAsync(cartSubscriber.Result.ExecutingUrl, jsonRequest);
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