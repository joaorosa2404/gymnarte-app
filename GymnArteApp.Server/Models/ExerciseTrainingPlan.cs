namespace GymnArteApp.Server.Models
{
    public class ExerciseTrainingPlan
    {
        //id; exercise; trainingplan; dtcriacao; dtmodificacao; usermodificacao nullable;
        public int ExerciseTrainingPlanId { get; set; }
        public Exercise Exercise { get; set; } = null!;
        public TrainingPlan TrainingPlan { get; set; } = null!;
        public int SetsNumber { get; set; }
        public int RepsNumber { get; set; }
        public string Notes { get; set; } = null!;
    }
}
