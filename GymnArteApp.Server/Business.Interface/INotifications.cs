namespace GymnArteApp.Server.Business.Interface
{
    public interface INotifications
    {
        Task<Models.Notification> GetNotificationByIdAsync(int id);
        Task<IEnumerable<Models.Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<IEnumerable<Models.Notification>> GetUnreadByUserIdAsync(int userId);
        Task<Models.Notification> CreateNotificationAsync(Models.Notification notif, string token);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> DeleteNotificationAsync(int id, string token);
    }
}
