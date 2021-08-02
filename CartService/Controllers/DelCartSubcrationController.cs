using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CartService.Models.CartSubscribers;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DelCartSubcrationController : ControllerBase
    {
        private readonly ICartSubscriberRepository _cartSubscriberRepository;

        public DelCartSubcrationController(ICartSubscriberRepository cartSubscriberRepository)
        {
            _cartSubscriberRepository = cartSubscriberRepository;
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CartSubscriber cartSubscriber)
        {
            await _cartSubscriberRepository.CreateAsync(cartSubscriber);
            return Ok();
        }

        [HttpDelete("{cartId}")]
        public async Task<ActionResult> Delete(int cartId)
        {
            await _cartSubscriberRepository.DeleteAsync(cartId);
            return Ok();
        }
    }
}
