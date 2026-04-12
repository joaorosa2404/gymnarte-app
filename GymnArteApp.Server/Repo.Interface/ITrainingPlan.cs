using GymnArteApp.Server.Models;
namespace GymnArteApp.Server.Repo.Interface
{
    public interface ITrainingPlan
    {
        Task<IEnumerable<Models.TrainingPlan>> GetAllTrainingPlansAsync();
        Task<Models.TrainingPlan> GetTrainingPlanByIdAsync(int id);
        Task<Models.TrainingPlan> GetTrainingPlanByUserIdAsync(int userId);
        Task<Models.TrainingPlan> GetTrainingPlanByExerciseTypeAsync(int exerciseTypeId);
        Task<Models.TrainingPlan> GetTrainingPlanByUserAndExerciseTypeAsync(int userId, int exerciseTypeId);
        Task<bool> TrainingPlanHasExercises(Models.TrainingPlan trainingPlan);
        Task<Models.TrainingPlan> CreateTrainingPlanAsync(Models.TrainingPlan trainingPlan, string token);
        Task<Models.TrainingPlan> UpdateTrainingPlanAsync(Models.TrainingPlan trainingPlan, int id, string token);
        Task<bool> DisableTrainingPlanAsync(int id, string token);
    }
}
