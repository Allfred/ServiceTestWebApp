using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CartService.Models.Products;

namespace CartService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository )
        {
            _productRepository = productRepository;
        }

        // GET api/v1/[controller]/items
        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ItemsAsync()
        {
            var products = await _productRepository.GetAsync();

            if (products == null || !products.Any()) return NotFound();

            return Ok(products);
        }
        
        // GET api/v1/[controller]/products/id
        [HttpGet]
        [Route("items/{id:int}", Name="ProductByIdAsync")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> ProductByIdAsync(int id)
        {
            if (id <= 0) return BadRequest(); 
                
            var product = await _productRepository.GetAsync(id);

            if (product == null) return NotFound();
            
            return product;
        }

        //POST api/v1/[controller]/products
        [Route("items")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateProductAsync([FromBody] Product product)
        {
            await _productRepository.CreateAsync(product);

            return CreatedAtRoute(nameof(ProductByIdAsync), new { id = product.Id }, product);
        }

        //PUT api/v1/[controller]/products
        [Route("items")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult>  UpdateProductAsync([FromBody] Product product)
        {
            if (await _productRepository.GetAsync(product.Id) == null) return NotFound();
            
            await _productRepository.UpdateAsync(product);
            
            return CreatedAtRoute(nameof(ProductByIdAsync), new { id = product.Id }, product);
        }

        //DELETE api/v1/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            if (await _productRepository.GetAsync(id) == null) return NotFound();
            
            await _productRepository.DeleteAsync(id);
            
            return NoContent();
        }
    }
}
