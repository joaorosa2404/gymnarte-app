namespace GymnArteApp.Server.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // FK para ExerciseType
        public int ExerciseTypeId { get; set; }
        public ExerciseType ExerciseType { get; set; } = null!;

        // Auditoria
        public int? UpdatedUserId { get; set; }
        public User? UpdatedUser { get; set; }

        // Tabela de junção (many-to-many com TrainingPlan)
        public ICollection<ExerciseTrainingPlan> ExerciseTrainingPlans { get; set; } = [];
    }
}
