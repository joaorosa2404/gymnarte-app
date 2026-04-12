using Microsoft.EntityFrameworkCore;
using GymnArteApp.Server.Repo.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace GymnArteApp.Server.Repo
{
    public class Notifications : INotifications
    {
        private readonly Data.GymDbContext _context;

        public Notifications(Data.GymDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Notification> GetNotificationByIdAsync(int id)
        {
            try
            {
                var notification = await _context.Notifications
                    .Include(n => n.User)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n => n.NotificationId == id);

                return notification;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the notification with ID {id}.", ex);
            }
        }

        public async Task<Models.Notification> GetNotificationByUserIdAsync(int userId)
        {
            try
            {
                // Seguindo o padrão do exemplo que retorna FirstOrDefault mesmo em listas
                var notification = await _context.Notifications
                    .Where(n => n.UserId == userId)
                    .Include(n => n.User)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                return notification;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the notification for user ID {userId}.", ex);
            }
        }

        public async Task<Models.Notification> CreateNotificationAsync(Models.Notification notif, string token)
        {
            try
            {
                notif.CreationDate = DateTime.UtcNow;
                notif.Read = false;

                _context.Notifications.Add(notif);
                await _context.SaveChangesAsync();

                return notif;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the notification.", ex);
            }
        }

        public async Task<bool> DeleteNotificationsAsync(int id, string token)
        {
            try
            {
                var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == id);

                if (notification == null)
                {
                    throw new Exception($"Notification with ID {id} not found.");
                }

                _context.Notifications.Remove(notification);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the notification with ID {id}.", ex);
            }
        }

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