using GymnArteApp.Server.Business.Interface;

namespace GymnArteApp.Server.Business
{
    public class Notifications : INotifications
    {
        private readonly Repo.Interface.INotifications _notificationsRepo;

        public Notifications(Repo.Interface.INotifications notificationsRepo)
        {
            _notificationsRepo = notificationsRepo;
        }

        public async Task<Models.Notification> GetNotificationByIdAsync(int id)
        {
            try
            {
                var notification = await _notificationsRepo.GetNotificationByIdAsync(id);
                if (notification == null) throw new Exception($"Notification with ID {id} not found.");

                return notification;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving notification ID {id}.", ex);
            }
        }

        public async Task<Models.Notification> GetNotificationByUserIdAsync(int id)
        {
            try
            {
                var notification = await _notificationsRepo.GetNotificationByUserIdAsync(id);
                return notification;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving notifications for user ID {id}.", ex);
            }
        }

        public async Task<Models.Notification> CreateNotificationAsync(Models.Notification notif, string token)
        {
            try
            {
                var newNotification = await _notificationsRepo.CreateNotificationAsync(notif, token);
                return newNotification;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while creating notification.", ex);
            }
        }

        public async Task<bool> DeleteNotificationsAsync(int id, string token)
        {
            try
            {
                var result = await _notificationsRepo.DeleteNotificationsAsync(id, token);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while deleting notification ID {id}.", ex);
            }
        }
    }
}