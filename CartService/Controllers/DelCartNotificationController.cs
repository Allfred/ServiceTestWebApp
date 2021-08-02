using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> Post([FromBody] string message)
        {
            await _delCartNotificationRepository.CreateAsync(new DelCartNotification{ Message=message});
            return Ok();
        }
    }
}
