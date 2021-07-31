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
           
            var result = await _cartSubscriberRepository.CreateAsync(cartSubscriber);
            
            if (result)
            {
                return Ok();
            }

            return Problem();
        }

        [HttpDelete("{cartId}")]
        public async Task<ActionResult> Delete(int cartId)
        {
            
            var result = await _cartSubscriberRepository.DeleteAsync(cartId);

            if (result)
            {
                return Ok();
            }

            return Problem();
 
        }
    }
}
