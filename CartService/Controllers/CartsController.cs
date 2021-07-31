using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CartService.Models.Carts;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartsController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        [HttpGet]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetAsync()
        {
            var carts = await _cartRepository.GetAsync();

            return Ok(carts ?? Enumerable.Empty<Cart>());
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetAsync(int id)
        {
            var cart = await _cartRepository.GetAsync(id);

            return Ok(cart ?? new Cart());
        }
        

        [HttpPost]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Post([FromBody] Cart cart)
        {
            try
            {
                await _cartRepository.CreateAsync(cart);
                return Accepted();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Problem();
            }
            
        }

        [HttpPatch]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Patch([FromBody] Cart cart)
        {
            await _cartRepository.UpdateAsync(cart);
            return Ok();
        }

        [HttpPatch("{cartId}/products/{productId}")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Patch(int cartId,int productId)
        {
            await _cartRepository.AddProduct(cartId,productId);

            return Ok();
        }

        [HttpDelete("{cartId}/products/{productId}")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteAsync(int cartId, int productId)
        {
            await _cartRepository.DeleteProduct(cartId, productId);

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _cartRepository.DeleteAsync(id);
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
