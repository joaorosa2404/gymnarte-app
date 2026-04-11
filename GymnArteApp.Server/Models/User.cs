using GymnArteApp.Server.Models.Enums;

namespace GymnArteApp.Server.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int PartnerNumber { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // hash aqui, nunca plain text
        public DateTime BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Auto-referência: quem fez a última atualização
        public int? UpdatedUserId { get; set; }
        public User? UpdatedUser { get; set; }

        // Navegações
        public UserScope UserScope { get; set; } = null!;
        public int UserScopeId { get; set; }
        public ICollection<BiometricData> BiometricData { get; set; } = [];        // 1 User → N BiometricData
        public ICollection<Notification> Notifications { get; set; } = [];         // 1 User → N Notifications
        public ICollection<TrainingPlan> TrainingPlans { get; set; } = [];         // 1 User → N TrainingPlans
    }
}
