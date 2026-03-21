namespace GymnArteApp.Server.Models
{
    public class TrainingPlan
    {
        public int TrainingPlanId { get; set; }
        public string TrainingPlanName { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // FK para User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // FK para ExerciseType
        public int ExerciseTypeId { get; set; }
        public ExerciseType ExerciseType { get; set; } = null!;

        // Auditoria
        public int? UpdatedUserId { get; set; }
        public User? UpdatedUser { get; set; }

        // Tabela de junção (many-to-many com Exercise)
        public ICollection<ExerciseTrainingPlan> ExerciseTrainingPlans { get; set; } = [];
    }
}
