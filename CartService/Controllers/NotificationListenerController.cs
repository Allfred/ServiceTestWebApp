using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CartService.Models.Notifications;

namespace CartService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationListenerController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationListenerController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        
        //POST api/v1/[controller]/items
        [HttpPost]
        [Route("items")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateNotificationAsync([FromBody] string message)
        {
            var item = new Notification
            {
                Message = message
            };
            
            await _notificationRepository.CreateAsync(item);

            return Ok();
        }
    }
}
