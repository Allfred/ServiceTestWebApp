using System;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        [ProducesResponseType(typeof(CartSubscriber), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Post([FromBody] CartSubscriber cartSubscriber)
        {
            try
            {
                await _cartSubscriberRepository.CreateAsync(cartSubscriber);
                return Accepted();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Problem();
            }
        }

        [HttpDelete("{cartId}")]
        [ProducesResponseType(typeof(CartSubscriber), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Delete(int cartId)
        {
            try
            {
                await _cartSubscriberRepository.DeleteAsync(cartId);
                return Accepted();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Problem();
            }
        }
    }
}
