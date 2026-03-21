namespace GymnArteApp.Server.Models
{
    public class ExerciseTrainingPlan
    {
        public int ExerciseTrainingPlanId { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public string? Notes { get; set; }
        // FKs
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;
        public int TrainingPlanId { get; set; }
        public TrainingPlan TrainingPlan { get; set; } = null!;
    }
}
