using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using CartService.Models.DelCartNotifications;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DelCartNotificationController : ControllerBase
    {
        private readonly IDelCartNotificationRepository _delCartNotificationRepository;

        public DelCartNotificationController(IDelCartNotificationRepository delCartNotificationRepository)
        {
            _delCartNotificationRepository = delCartNotificationRepository;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Post([FromBody] string message)
        {
            try
            {
                await _delCartNotificationRepository.CreateAsync(new DelCartNotification() { Message=message});
                return Accepted();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Problem();
            }
        }

    }
}
