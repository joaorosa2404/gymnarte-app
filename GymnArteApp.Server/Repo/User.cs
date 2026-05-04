using GymnArteApp.Server.Repo.Interface;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace GymnArteApp.Server.Repo
{
    public class User : IUser
    {
        private readonly Data.GymDbContext _context;

        public User(Data.GymDbContext context) => _context = context;

        public async Task<IEnumerable<Models.User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.UserScope)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Models.User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.UserScope)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<IEnumerable<Models.User>> GetUsersByScopeAsync(int scopeId)
        {
            // scopeId aqui é o UserScope.UserScopeId (PK do scope)
            return await _context.UserScopes
                .Where(us => us.UserScopeId == scopeId)
                .Include(us => us.User)
                .Select(us => us.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.User>> GetUsersByRoleAsync(Models.Enums.UserRole role)
        {
            return await _context.UserScopes
                .Where(us => us.Role == role && us.IsActive)
                .Include(us => us.User)
                .Select(us => us.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Models.User> CreateUserAsync(Models.User user)
        {
            user.CreationDate = DateTime.UtcNow;
            user.IsActive     = true;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Models.User> UpdateUserAsync(int id, Models.User user, string token)
        {
            // Buscar entidade tracked (sem AsNoTracking) para o Update funcionar
            var existing = await _context.Users.FindAsync(id)
                ?? throw new Exception($"User with ID {id} not found.");

            existing.UserName    = user.UserName;
            existing.Email       = user.Email;
            existing.Name        = user.Name;
            existing.Surname     = user.Surname;
            existing.BirthDate   = user.BirthDate;
            existing.Gender      = user.Gender;
            existing.Phone       = user.Phone;
            existing.UpdatedDate  = DateTime.UtcNow;
            existing.UpdatedUserId = GetUserIdFromToken(token);

            // Password só é atualizada se vier preenchida no payload
            if (!string.IsNullOrWhiteSpace(user.Password))
                existing.Password = user.Password;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteUserAsync(int id, string token)
        {
            // Soft delete — preserva histórico (biometria, planos, etc.)
            var user = await _context.Users.FindAsync(id);
            if (user is null) return false;

            user.IsActive      = false;
            user.UpdatedDate   = DateTime.UtcNow;
            user.UpdatedUserId = GetUserIdFromToken(token);

            await _context.SaveChangesAsync();
            return true;
        }

        private int? GetUserIdFromToken(string token)
        {
            try
            {
                var handler      = new JwtSecurityTokenHandler();
                var jwtToken     = handler.ReadJwtToken(token);
                var userIdClaim  = jwtToken.Claims
                    .FirstOrDefault(c => c.Type == "userId" || c.Type == "sub")?.Value;
                return userIdClaim != null ? int.Parse(userIdClaim) : null;
            }
            catch { return null; }
        }
    }
}
