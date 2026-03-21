using GymnArteApp.Server.Models;

namespace GymnArteApp.Server.Repo.Interface
{
    public interface ITrainingPlan
    {
        Task<IEnumerable<TrainingPlan>> GetAllTrainingPlansAsync();
        Task<TrainingPlan> GetTrainingPlanByIdAsync(int id);
        Task<TrainingPlan> GetTrainingPlanByUserIdAsync(int userId);
        Task<TrainingPlan> GetTrainingPlanByExerciseTypeAsync(int exerciseTypeId);
        Task<TrainingPlan> GetTrainingPlanByUserAndExerciseTypeAsync(int userId, int exerciseTypeId);
        Task<bool> TrainingPlanHasExercises(TrainingPlan trainingPlan);
        Task<TrainingPlan> CreateTrainingPlanAsync(TrainingPlan trainingPlan, string token);
        Task<TrainingPlan> UpdateTrainingPlanAsync(TrainingPlan trainingPlan, int id, string token);
        Task<bool> DisableTrainingPlanAsync(int id, string token);
    }
}
