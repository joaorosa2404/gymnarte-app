namespace GymnArteApp.Server.Business.Interface
{
    public interface INotifications
    {
        Task<Models.Notification> GetNotificationByIdAsync(int id);
        Task<Models.Notification> GetNotificationByUserIdAsync(int id);
        Task<Models.Notification> CreateNotificationAsync(Models.Notification notif, string token);
        Task<bool> DeleteNotificationsAsync(int id, string token);
    }
}
