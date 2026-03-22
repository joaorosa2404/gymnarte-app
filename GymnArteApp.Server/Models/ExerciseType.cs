namespace GymnArteApp.Server.Models
{
    public class ExerciseType
    {
        public int ExerciseTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Auditoria
        public int? UpdatedUserId { get; set; }
        public User? UpdatedUser { get; set; }

        // Navegações inversas
        public ICollection<Exercise> Exercises { get; set; } = [];
        public ICollection<TrainingPlan> TrainingPlans { get; set; } = [];
    }
}
