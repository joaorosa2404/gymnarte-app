using GymnArteApp.Server.Models.Enums;

namespace GymnArteApp.Server.Models
{
    public class User
    {
        public int UserId {  get; set; }
        public int PartnerNumber { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public User? UpdatedUser { get; set; }
        public UserScope Scope { get; set; } = null!;
        public BiometricData BiometricData { get; set; } = null!;
        public Notifications? Notifications { get; set; } 
        public TrainingPlan? TrainingPlan { get; set; }

    }
}
