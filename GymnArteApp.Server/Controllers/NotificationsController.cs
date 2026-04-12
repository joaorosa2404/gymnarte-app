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
        public async Task<ActionResult<Models.Notification>> GetByUser(int userId)
        {
            var notif = await _svc.GetNotificationByUserIdAsync(userId);
            return notif is null ? NotFound() : Ok(notif);
        }

        // POST api/notifications
        [HttpPost]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<ActionResult<Models.Notification>> Create([FromBody] Models.Notification notification)
        {
            var created = await _svc.CreateNotificationAsync(notification, BearerToken);
            return CreatedAtAction(nameof(GetById), new { id = created.NotificationId }, created);
        }

        // DELETE api/notifications/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _svc.DeleteNotificationsAsync(id, BearerToken);
            return result ? NoContent() : NotFound();
        }
    }
}
