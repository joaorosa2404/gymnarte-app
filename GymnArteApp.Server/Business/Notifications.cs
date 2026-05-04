using GymnArteApp.Server.Business.Interface;

namespace GymnArteApp.Server.Business
{
    public class Notifications : INotifications
    {
        private readonly Repo.Interface.INotifications _notificationsRepo;

        public Notifications(Repo.Interface.INotifications notificationsRepo)
            => _notificationsRepo = notificationsRepo;

        public async Task<Models.Notification> GetNotificationByIdAsync(int id)
        {
            var notification = await _notificationsRepo.GetNotificationByIdAsync(id);
            if (notification is null) throw new Exception($"Notification with ID {id} not found.");
            return notification;
        }

        public async Task<IEnumerable<Models.Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _notificationsRepo.GetNotificationsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Models.Notification>> GetUnreadByUserIdAsync(int userId)
        {
            return await _notificationsRepo.GetUnreadByUserIdAsync(userId);
        }

        public async Task<Models.Notification> CreateNotificationAsync(Models.Notification notif, string token)
        {
            return await _notificationsRepo.CreateNotificationAsync(notif, token);
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            return await _notificationsRepo.MarkAsReadAsync(id);
        }

        public async Task<bool> DeleteNotificationAsync(int id, string token)
        {
            return await _notificationsRepo.DeleteNotificationAsync(id, token);
        }
    }
}
