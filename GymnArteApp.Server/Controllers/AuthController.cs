using GymnArteApp.Server.Data;
using GymnArteApp.Server.DTOs.Auth;
using GymnArteApp.Server.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymnArteApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly GymDbContext _context;
        private readonly IPasswordHasher<Models.User> _hasher;
        private readonly ITokenService _tokenService;

        public AuthController(
            GymDbContext context,
            IPasswordHasher<Models.User> hasher,
            ITokenService tokenService)
        {
            _context = context;
            _hasher = hasher;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Login — devolve um JWT Bearer token.
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.UserScope)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user is null)
                return Unauthorized(new { message = "Credenciais inválidas." });

            var result = _hasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Credenciais inválidas." });

            var role = user.UserScope?.Role ?? Models.Enums.UserRole.Partner;
            var token = _tokenService.GenerateToken(user, role);

            return Ok(new LoginResponse
            {
                Token = token,
                Email = user.Email,
                FullName = $"{user.Name} {user.Surname}",
                Role = role.ToString(),
                UserId = user.UserId
            });
        }
    }
}
