using GymnArteApp.Server.Models.Enums;

namespace GymnArteApp.Server.Models
{
    /// <summary>
    /// Representa um utilizador do ginásio.
    /// A relação com UserScope é 1:1, sendo a FK definida no lado do UserScope (UserScope.UserId).
    /// </summary>
    public class User
    {
        public int    UserId       { get; set; }
        public int    PartnerNumber{ get; set; }
        public string UserName     { get; set; } = string.Empty;
        public string Email        { get; set; } = string.Empty;
        public string Password     { get; set; } = string.Empty; // sempre hashed, nunca plain text
        public DateTime BirthDate  { get; set; }
        public Gender?  Gender     { get; set; }
        public string Name         { get; set; } = string.Empty;
        public string Surname      { get; set; } = string.Empty;
        public string? Phone       { get; set; }
        public bool   IsActive     { get; set; } = true;
        public DateTime  CreationDate { get; set; }
        public DateTime? UpdatedDate  { get; set; }

        // Auto-referência de auditoria: quem fez a última atualização
        public int?  UpdatedUserId { get; set; }
        public User? UpdatedUser   { get; set; }

        // Navegações
        // NOTA: Sem UserScopeId aqui — a FK está em UserScope.UserId (evita dependência circular)
        public UserScope?            UserScope     { get; set; }
        public ICollection<BiometricData>  BiometricData  { get; set; } = [];
        public ICollection<Notification>   Notifications  { get; set; } = [];
        public ICollection<TrainingPlan>   TrainingPlans  { get; set; } = [];
    }
}
