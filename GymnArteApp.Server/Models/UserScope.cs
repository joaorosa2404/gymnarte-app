using GymnArteApp.Server.Models.Enums;

namespace GymnArteApp.Server.Models
{
    public class UserScope
    {
        public int ScopeId { get; set; }
        public UserRole Scope { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; } //data em que o registro foi atualizado pela última vez
        public User? UpdatedUser { get; set; } //user que fez a última atualização
    }
}
