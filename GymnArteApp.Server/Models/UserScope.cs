using GymnArteApp.Server.Models.Enums;

namespace GymnArteApp.Server.Models
{
    public class UserScope
    {
        //id; user; scope; dtcriacao;
        public int ScopeId { get; set; }
        public UserRole Scope { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public User? UpdatedUser { get; set; }
    }
}
