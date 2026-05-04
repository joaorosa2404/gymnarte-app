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
        private readonly IUser                         _userSvc;
        private readonly IPasswordHasher<Models.User>  _hasher;

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
            => Ok(await _userSvc.GetAllUsersAsync());

        // GET api/users/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Models.User>> GetById(int id)
        {
            try   { return Ok(await _userSvc.GetUserByIdAsync(id)); }
            catch (Exception ex) when (ex.Message.Contains("not found"))
                  { return NotFound(new { message = ex.Message }); }
        }

        // GET api/users/scope/{scopeId}
        [HttpGet("scope/{scopeId:int}")]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<ActionResult<IEnumerable<Models.User>>> GetByScope(int scopeId)
            => Ok(await _userSvc.GetUsersByScopeAsync(scopeId));

        // GET api/users/role/{role}   (Admin | PersonalTrainer | Partner)
        [HttpGet("role/{role}")]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<ActionResult<IEnumerable<Models.User>>> GetByRole(
            [FromRoute] Models.Enums.UserRole role)
            => Ok(await _userSvc.GetUsersByRoleAsync(role));

        // POST api/users  (registo — AllowAnonymous para auto-registo de clientes)
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Models.User>> Create([FromBody] Models.User user)
        {
            // Hash da password antes de persistir
            user.Password = _hasher.HashPassword(user, user.Password);
            var created   = await _userSvc.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
        }

        // PUT api/users/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Models.User>> Update(int id, [FromBody] Models.User user)
        {
            // Se a password for enviada, hash antes de guardar
            if (!string.IsNullOrWhiteSpace(user.Password))
                user.Password = _hasher.HashPassword(user, user.Password);

            try   { return Ok(await _userSvc.UpdateUserAsync(id, user, BearerToken)); }
            catch (Exception ex) when (ex.Message.Contains("not found"))
                  { return NotFound(new { message = ex.Message }); }
        }

        // DELETE api/users/{id}  (soft delete)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userSvc.DeleteUserAsync(id, BearerToken);
            return result ? NoContent() : NotFound();
        }
    }
}
