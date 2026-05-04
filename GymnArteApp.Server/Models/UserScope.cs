using GymnArteApp.Server.Models.Enums;

namespace GymnArteApp.Server.Models
{
    /// <summary>
    /// Define a role/permissão de um utilizador.
    /// Relação 1:1 com User — a FK (UserId) está nesta entidade (dependent side).
    /// </summary>
    public class UserScope
    {
        public int      UserScopeId  { get; set; }
        public UserRole Role         { get; set; }
        public bool     IsActive     { get; set; } = true;
        public DateTime  CreationDate { get; set; }
        public DateTime? UpdatedDate  { get; set; }

        // FK principal (esta entidade é o lado "dependent" da relação 1:1)
        public int  UserId { get; set; }
        public User User   { get; set; } = null!;

        // Auditoria
        public int?  UpdatedUserId { get; set; }
        public User? UpdatedUser   { get; set; }
    }
}
