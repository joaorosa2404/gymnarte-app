using GymnArteApp.Server.Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymnArteApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotifications _svc;

        public NotificationsController(INotifications svc) => _svc = svc;

        private string BearerToken =>
            Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

        // GET api/notifications/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Models.Notification>> GetById(int id)
        {
            try   { return Ok(await _svc.GetNotificationByIdAsync(id)); }
            catch (Exception ex) when (ex.Message.Contains("not found"))
                  { return NotFound(new { message = ex.Message }); }
        }

        // GET api/notifications/user/{userId}
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<Models.Notification>>> GetByUser(int userId)
            => Ok(await _svc.GetNotificationsByUserIdAsync(userId));

        // GET api/notifications/user/{userId}/unread
        [HttpGet("user/{userId:int}/unread")]
        public async Task<ActionResult<IEnumerable<Models.Notification>>> GetUnreadByUser(int userId)
            => Ok(await _svc.GetUnreadByUserIdAsync(userId));

        // POST api/notifications
        [HttpPost]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<ActionResult<Models.Notification>> Create([FromBody] Models.Notification notification)
        {
            var created = await _svc.CreateNotificationAsync(notification, BearerToken);
            return CreatedAtAction(nameof(GetById), new { id = created.NotificationId }, created);
        }

        // PATCH api/notifications/{id}/read
        [HttpPatch("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _svc.MarkAsReadAsync(id);
            return result ? NoContent() : NotFound();
        }

        // DELETE api/notifications/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _svc.DeleteNotificationAsync(id, BearerToken);
            return result ? NoContent() : NotFound();
        }
    }
}
