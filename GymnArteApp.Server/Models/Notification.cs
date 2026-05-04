namespace GymnArteApp.Server.Models
{
    public class Notification
    {
        public int    NotificationId { get; set; }
        public string Title          { get; set; } = string.Empty;
        public string Description    { get; set; } = string.Empty;
        public bool   Read           { get; set; } = false;
        public DateTime CreationDate { get; set; }

        // FK
        public int  UserId { get; set; }
        public User User   { get; set; } = null!;
    }
}
