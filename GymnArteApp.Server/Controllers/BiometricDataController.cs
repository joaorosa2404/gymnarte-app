using GymnArteApp.Server.Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymnArteApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BiometricDataController : ControllerBase
    {
        private readonly IBiometricData _svc;

        public BiometricDataController(IBiometricData svc) => _svc = svc;

        private string BearerToken =>
            Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

        // GET api/biometricdata/user/{userId}/latest
        [HttpGet("user/{userId:int}/latest")]
        public async Task<ActionResult<Models.BiometricData>> GetLatestByUser(int userId)
        {
            var data = await _svc.GetBiometricDataByUserIdAsync(userId, BearerToken);
            return data is null ? NotFound() : Ok(data);
        }

        // GET api/biometricdata/user/{userId}/history
        [HttpGet("user/{userId:int}/history")]
        public async Task<ActionResult<IEnumerable<Models.BiometricData>>> GetHistoryByUser(int userId)
            => Ok(await _svc.GetHistoryByUserIdAsync(userId));

        // POST api/biometricdata
        [HttpPost]
        public async Task<ActionResult<Models.BiometricData>> Create([FromBody] Models.BiometricData biometric)
        {
            var created = await _svc.CreateBiometricDataAsync(biometric);
            return CreatedAtAction(nameof(GetLatestByUser), new { userId = created.UserId }, created);
        }

        // PUT api/biometricdata/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Models.BiometricData>> Update(int id, [FromBody] Models.BiometricData biometric)
        {
            try   { return Ok(await _svc.UpdateBiometricDataAsync(id, biometric, BearerToken)); }
            catch (Exception ex) when (ex.Message.Contains("not found"))
                  { return NotFound(new { message = ex.Message }); }
        }

        // DELETE api/biometricdata/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _svc.DeleteBiometricDataAsync(id, BearerToken);
            return result ? NoContent() : NotFound();
        }

        // PATCH api/biometricdata/{id}/disable
        [HttpPatch("{id:int}/disable")]
        public async Task<IActionResult> Disable(int id)
        {
            var result = await _svc.DisableBiometricDataAsync(id, BearerToken);
            return result ? NoContent() : NotFound();
        }
    }
}
