namespace GymnArteApp.Server.Models
{
    public class TrainingPlan
    {
        //id; exercType; descricao; dtcriacao; dtmodificacao; usermodificacao nullable;
        public int TrainingPlanId { get; set; }
        public string? Notes { get; set; }
        public string TrainingPlanName { get; set; } = null!;
        public ExerciseType ExerciseType { get; set; } = null!;
        public User User { get; set; } = null!;
        public ICollection<ExerciseTrainingPlan> ExerciseTrainingPlan { get; set; } = null!;
    }
}
