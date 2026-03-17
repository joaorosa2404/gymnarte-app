namespace GymnArteApp.Server.Repo.Interface
{
    public interface INotifications
    {
        Task<Models.Notifications> GetNotificationByIdAsync(int id);
        Task<Models.Notifications> GetNotificationByUserIdAsync(int id);
        Task<Models.Notifications> CreateNotificationAsync(Models.Notifications notif, string token);
        Task<bool> DeleteNotificationsAsync(int id, string token);
    }
}
