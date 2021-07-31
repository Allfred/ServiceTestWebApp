using System;
using System.Collections.Generic;
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
        [ProducesResponseType(typeof(IEnumerable<Cart>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAsync()
        {
            var carts = await _cartRepository.GetAsync();

            if (carts != null)
            {
                return Ok(carts);
            }

            return NotFound();
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetAsync(int id)
        {
            var cart = await _cartRepository.GetAsync(id);

            if (cart != null)
            {
                return Ok(cart);
            }

            return NotFound();
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Cart cart)
        {
            var result = await _cartRepository.CreateAsync(cart);
            
            if (result)
            {
                return Ok();
            }

            return Problem();
        }

        [HttpPatch]
        public async Task<ActionResult> Patch([FromBody] Cart cart)
        {
            var result = await _cartRepository.UpdateAsync(cart);
            
            if (result)
            {
                return Ok();
            }

            return Problem();
        }

        [HttpPatch("{cartId}/products/{productId}")]
        public async Task<ActionResult> Patch(int cartId,int productId)
        {
            var result = await _cartRepository.AddProduct(cartId,productId);

            if (result)
            {
                return Ok();
            }

            return Problem();
        }

        [HttpDelete("{cartId}/products/{productId}")]
        public async Task<ActionResult> DeleteAsync(int cartId, int productId)
        {
            var result= await _cartRepository.DeleteProduct(cartId, productId);

            if (result)
            {
                return Ok();
            }

            return Problem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result=await _cartRepository.DeleteAsync(id);
            
            if (result)
            {
                return Ok();
            }

            return Problem();
        }
    }
}
