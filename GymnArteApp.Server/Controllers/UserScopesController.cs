using GymnArteApp.Server.Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymnArteApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserScopesController : ControllerBase
    {
        private readonly IUserScope _svc;

        public UserScopesController(IUserScope svc) => _svc = svc;

        private string BearerToken =>
            Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

        // GET api/userscopes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.UserScope>>> GetAll()
            => Ok(await _svc.GetAllUserScopesAsync());

        // GET api/userscopes/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Models.UserScope>> GetById(int id)
        {
            try   { return Ok(await _svc.GetUserScopeByIdAsync(id)); }
            catch (Exception ex) when (ex.Message.Contains("not found"))
                  { return NotFound(new { message = ex.Message }); }
        }

        // GET api/userscopes/user/{userId}
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<Models.UserScope>> GetByUser(int userId)
        {
            var scope = await _svc.GetUserScopeByUserIdAsync(userId);
            return scope is null ? NotFound() : Ok(scope);
        }

        // POST api/userscopes
        [HttpPost]
        public async Task<ActionResult<Models.UserScope>> Create([FromBody] Models.UserScope userScope)
        {
            var created = await _svc.CreateUserScopeAsync(userScope, BearerToken);
            return CreatedAtAction(nameof(GetById), new { id = created.UserScopeId }, created);
        }

        // PUT api/userscopes/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Models.UserScope>> Update(int id, [FromBody] Models.UserScope userScope)
        {
            try   { return Ok(await _svc.UpdateUserScopeAsync(userScope, id, BearerToken)); }
            catch (Exception ex) when (ex.Message.Contains("not found"))
                  { return NotFound(new { message = ex.Message }); }
        }

        // DELETE api/userscopes/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Disable(int id)
        {
            var result = await _svc.DisableUserScopeAsync(id, BearerToken);
            return result ? NoContent() : NotFound();
        }
    }
}
