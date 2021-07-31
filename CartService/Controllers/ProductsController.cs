using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Get()
        {
            var products = await _productRepository.GetAsync();

            return Ok(products ?? Enumerable.Empty<Product>());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _productRepository.GetAsync(id);
            
            return Ok(product ?? new Product());
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Post([FromBody] Product product)
        {
            try
            {
                await _productRepository.CreateAsync(product);
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
        public async Task<ActionResult> Patch([FromBody] Product product)
        {
            try
            {
                await _productRepository.UpdateAsync(product);
                return Accepted();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Problem();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _productRepository.DeleteAsync(id);
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
