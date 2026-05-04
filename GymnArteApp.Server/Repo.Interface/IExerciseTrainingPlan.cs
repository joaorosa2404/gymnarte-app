namespace GymnArteApp.Server.Repo.Interface
{
    public interface IExerciseTrainingPlan
    {
        Task<IEnumerable<Models.ExerciseTrainingPlan>> GetAllExerciseTrainingPlansAsync();
        Task<Models.ExerciseTrainingPlan?> GetExerciseTrainingPlanByIdAsync(int id);
        Task<IEnumerable<Models.ExerciseTrainingPlan>> GetExerciseTrainingPlansByExerciseIdAsync(int exerciseId);
        Task<IEnumerable<Models.ExerciseTrainingPlan>> GetExerciseTrainingPlansByTrainingPlanIdAsync(int trainingPlanId);
        Task<IEnumerable<Models.ExerciseTrainingPlan>> GetExerciseTrainingPlansByExerciseAndTrainingPlanIdAsync(int exerciseId, int trainingPlanId);
        Task<Models.ExerciseTrainingPlan> CreateExerciseTrainingPlanAsync(Models.ExerciseTrainingPlan exerciseTrainingPlan, string token);
        Task<Models.ExerciseTrainingPlan> UpdateExerciseTrainingPlanAsync(Models.ExerciseTrainingPlan exerciseTrainingPlan, int id, string token);
        Task<bool> DisableExerciseTrainingPlanAsync(int id, string token);
    }
}
