using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CartService.Models.Carts;
using CartService.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartsController(ICartRepository cartRepository,IProductRepository productRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _productRepository = productRepository;
        }

        // GET api/v1/[controller]/items
        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(IEnumerable<Cart>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ItemsAsync()
        {
            var carts = await _cartRepository.GetAsync();

            if (carts == null || !carts.Any()) return NotFound();

            return Ok(carts);
        }
        
        // GET api/v1/[controller]/items/id
        [HttpGet]
        [Route("items/{id:int}", Name="CartByIdAsync")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> CartByIdAsync(int id)
        {
            if (id <= 0) return BadRequest();
            
            var cart = await _cartRepository.GetAsync(id);

            if (cart == null) return NotFound();
            
            return cart;
        }
        
        //POST api/v1/[controller]/items
        [Route("items")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateCartAsync([FromBody] Cart cart)
        {
            await _cartRepository.CreateAsync(cart);
           
            return CreatedAtRoute(nameof(CartByIdAsync), new { id = cart.Id }, cart);
        }

        //PUT api/v1/[controller]/items
        [Route("items")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateCartAsync([FromBody] Cart cart)
        {
            if (await _cartRepository.GetAsync(cart.Id) == null) return NotFound();
            
            await _cartRepository.UpdateAsync(cart);
            
            return CreatedAtRoute(nameof(CartByIdAsync), new { id = cart.Id }, cart);
        }

        [HttpPatch("items/{cartId:int}/products/{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Patch(int cartId, int productId)
        {
            if (_productRepository.GetAsync(productId) == null) return NotFound();
            
            if (_cartRepository.GetAsync(cartId) == null) return NotFound();
            
            await _cartRepository.AddProduct(cartId,productId);

            var cart = await _cartRepository.GetAsync(cartId);

            return Ok(cart);
            
        }

        [HttpDelete("items/{cartId:int}/products/{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteAsync(int cartId, int productId)
        {   
            if (_productRepository.GetAsync(productId) == null) return NotFound();
            
            if (_cartRepository.GetAsync(cartId) == null) return NotFound();

            await _cartRepository.DeleteProduct(cartId, productId);
 
            var cart = await _cartRepository.GetAsync(cartId);

            return Ok(cart);
        }

        //DELETE api/v1/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteCartAsync(int id)
        {
            if (await _cartRepository.GetAsync(id) == null) return NotFound();
            
            await _cartRepository.DeleteAsync(id);
            
            return NoContent();
        }
    }
}
