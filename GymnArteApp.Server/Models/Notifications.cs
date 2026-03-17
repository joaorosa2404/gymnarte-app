namespace GymnArteApp.Server.Models
{
    public class Notifications
    {
        //descricao; dtcriacao; lida; user;
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Read { get; set; } = false;
        public User User { get; set; } = null!;
        public DateTime CreationDate { get; set; }

    }
}
