using GymnArteApp.Server.Repo.Interface;
using GymnArteApp.Server.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using GymnArteApp.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace GymnArteApp.Server.Repo
{
    public class UserScope : IUserScope
    {
        private readonly GymDbContext _context;

        public UserScope(GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.UserScope>> GetAllUserScopesAsync()
        {
            try
            {
                var userScopes = await _context.UserScopes
                .Include(u => u.User)
                .AsNoTracking()
                .ToListAsync();
                return userScopes;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving user scopes.", ex);
            }
        }

        public async Task<Models.UserScope> GetUserScopeByIdAsync(int id)
        {
            try
            {
                var userScope = await _context.UserScopes
                .Include(u => u.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserScopeId == id);
                return userScope;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the user scope with ID {id}.", ex);
            }
        }

        public async Task<Models.UserScope> GetUserScopeByUserIdAsync(int userId)
        {
            try
            {
                var userScope = await _context.UserScopes
                    .Include(u => u.User)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == userId);
                return userScope;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the user scope for user ID {userId}.", ex);
            }
        }

        public async Task<Models.UserScope> CreateUserScopeAsync(Models.UserScope userScope, string token)
        {
            try
            {
                userScope.CreationDate = DateTime.UtcNow;

                _context.UserScopes.Add(userScope);
                await _context.SaveChangesAsync();

                return userScope;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user scope.", ex);
            }
        }

        public async Task<Models.UserScope> UpdateUserScopeAsync(Models.UserScope userScope, int id, string token)
        {
            try
            {
                var existingScope = await _context.UserScopes.FindAsync(id);
                if (existingScope == null) return null;

                existingScope.Role = userScope.Role;
                existingScope.UpdatedDate = DateTime.UtcNow;
                existingScope.UpdatedUserId = GetUserIdFromToken(token);

                _context.UserScopes.Update(existingScope);
                await _context.SaveChangesAsync();

                return existingScope;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the user scope with ID {id}.", ex);
            }
        }

        public async Task<bool> DisableUserScopeAsync(int id, string token)
        {

            try
            {
                var scope = await _context.UserScopes.FindAsync(id);
                if (scope == null) return false;

                scope.UpdatedDate = DateTime.UtcNow;
                scope.UpdatedUserId = GetUserIdFromToken(token);
                scope.IsActive = false;

                var result = _context.UserScopes.Update(scope);

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while disabling the user scope with ID {id}.", ex);
            }
        }

        // Helper para extrair o ID do usuário do JWT
        private int? GetUserIdFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type == "sub")?.Value;

                return userIdClaim != null ? int.Parse(userIdClaim) : null;
            }
            catch
            {
                return null;
            }
        }
    }
}
