using GymnArteApp.Server.Models;

namespace GymnArteApp.Server.Repo.Interface
{
    public interface ITrainingPlan
    {
        Task<IEnumerable<TrainingPlan>> GetAllTrainingPlansAsync();
        Task<TrainingPlan> GetTrainingPlanByIdAsync(int id);
        Task<TrainingPlan> GetTrainingPlanByUserAsync(int id);
        Task<TrainingPlan> CreateTrainingPlanAsync(TrainingPlan trainingPlan, string token);
        Task<TrainingPlan> UpdateTrainingPlanAsync(int id, TrainingPlan trainingPlan, string token);
        Task<bool> DisableTrainingPlanAsync(int id, string token);
    }
}
