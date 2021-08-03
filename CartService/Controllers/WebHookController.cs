using System.Net;
using System.Threading.Tasks;
using CartService.Models.WebHook.Common;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase
    {
        private readonly IWebHookRepository _webHookRepository;

        public WebHookController(IWebHookRepository webHookRepository)
        {
            _webHookRepository = webHookRepository;
        }
        
        //Post //api/v1/[controller]/items
        [HttpPost]
        [Route("items")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateWebHookAsync([FromBody] WebHookProtocol webHookProtocol)
        {
            var item = new WebHookProtocol
            {
                Id = webHookProtocol.Id,
                WebHookType = webHookProtocol.WebHookType,
                ItemId = webHookProtocol.ItemId,
                ExecutingUri = webHookProtocol.ExecutingUri
            };
            
            await _webHookRepository.CreateAsync(item);

            if (await _webHookRepository.GetAsync(item.Id) == null) return NotFound();

            return Ok();
        }
        
        //DELETE api/v1/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteWebHookAsync(int id)
        {
            if (await _webHookRepository.GetAsync(id) == null) return NotFound();
            
            await _webHookRepository.DeleteAsync(id);
            
            return NoContent();
        }
    }
}