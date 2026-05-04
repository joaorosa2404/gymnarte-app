using Microsoft.EntityFrameworkCore;
using GymnArteApp.Server.Repo.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace GymnArteApp.Server.Repo
{
    public class Notifications : INotifications
    {
        private readonly Data.GymDbContext _context;

        public Notifications(Data.GymDbContext context) => _context = context;

        public async Task<Models.Notification?> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NotificationId == id);
        }

        // Retorna TODAS as notificações do utilizador (corrigido — o original só devolvia a primeira)
        public async Task<IEnumerable<Models.Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreationDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Notification>> GetUnreadByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.Read)
                .OrderByDescending(n => n.CreationDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Models.Notification> CreateNotificationAsync(Models.Notification notif, string token)
        {
            notif.CreationDate = DateTime.UtcNow;
            notif.Read         = false;

            _context.Notifications.Add(notif);
            await _context.SaveChangesAsync();
            return notif;
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notif = await _context.Notifications.FindAsync(id);
            if (notif is null) return false;

            notif.Read = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int id, string token)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification is null) return false;

            _context.Notifications.Remove(notification);
            return await _context.SaveChangesAsync() > 0;
        }

        private int? GetUserIdFromToken(string token)
        {
            try
            {
                var handler     = new JwtSecurityTokenHandler();
                var jwtToken    = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims
                    .FirstOrDefault(c => c.Type == "userId" || c.Type == "sub")?.Value;
                return userIdClaim != null ? int.Parse(userIdClaim) : null;
            }
            catch { return null; }
        }
    }
}
