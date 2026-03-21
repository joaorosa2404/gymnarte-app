using GymnArteApp.Server.Models.Enums;

namespace GymnArteApp.Server.Models
{
    public class UserScope
    {
        public int UserScopeId { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // FK para o User "dono" deste scope
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Audit
        public int? UpdatedUserId { get; set; }
        public User? UpdatedUser { get; set; }
    }
}
