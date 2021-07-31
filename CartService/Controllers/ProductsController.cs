using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CartService.Models.Carts;
using CartService.Models.Products;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository )
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Cart>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Get()
        {
            var products = await _productRepository.GetAsync();

            if (products != null)
            {
                return Ok(products);
            }
               
            return NotFound();
            
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _productRepository.GetAsync(id);

            if (product != null)
            {
                return Ok(product);
            }
               
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Product product)
        {
            var result = await _productRepository.CreateAsync(product);
            
            if (result)
            { 
                return Ok();
            }

            return Problem();
        }

        [HttpPatch]
        public async Task<ActionResult> Patch([FromBody] Product product)
        {
            var result = await _productRepository.UpdateAsync(product);
            
            if (result)
            {
                return Ok();
            }

            return Problem();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _productRepository.DeleteAsync(id);

            if (result)
            {
                return Ok();
            }

            return Problem();
        }
    }
}
