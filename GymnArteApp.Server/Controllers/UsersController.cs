using GymnArteApp.Server.Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymnArteApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUser                        _userSvc;
        private readonly IPasswordHasher<Models.User> _hasher;

        public UsersController(IUser userSvc, IPasswordHasher<Models.User> hasher)
        {
            _userSvc = userSvc;
            _hasher  = hasher;
        }

        private string BearerToken =>
            Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

        // GET api/users
        [HttpGet]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<ActionResult<IEnumerable<Models.User>>> GetAll()
        {
            var users = await _userSvc.GetAllUsersAsync();
            return Ok(users);
        }

        // GET api/users/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Models.User>> GetById(int id)
        {
            try
            {
                var user = await _userSvc.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET api/users/scope/{scopeId}
        [HttpGet("scope/{scopeId:int}")]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<ActionResult<IEnumerable<Models.User>>> GetByScope(int scopeId)
        {
            var users = await _userSvc.GetUsersByScopeAsync(scopeId);
            return Ok(users);
        }

        // POST api/users
        [HttpPost]
        [AllowAnonymous] // registo aberto; podes mudar para [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Models.User>> Create([FromBody] Models.User user)
        {
            // Hash da password antes de guardar
            user.Password = _hasher.HashPassword(user, user.Password);

            var created = await _userSvc.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
        }

        // PUT api/users/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Models.User>> Update(int id, [FromBody] Models.User user)
        {
            try
            {
                var updated = await _userSvc.UpdateUserAsync(id, user, BearerToken);
                return Ok(updated);
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE api/users/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userSvc.DeleteUserAsync(id, BearerToken);
            return result ? NoContent() : NotFound();
        }
    }
}
