using GymnArteApp.Server.Business.Interface;

namespace GymnArteApp.Server.Business
{
    public class TrainingPlan : ITrainingPlan
    {
        private readonly Repo.Interface.ITrainingPlan _trainingPlanRepo;

        public TrainingPlan(Repo.Interface.ITrainingPlan trainingPlanRepo)
        {
            _trainingPlanRepo = trainingPlanRepo;
        }

        public async Task<IEnumerable<Models.TrainingPlan>> GetAllTrainingPlansAsync()
        {
            try
            {
                var plans = await _trainingPlanRepo.GetAllTrainingPlansAsync();
                return plans;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while retrieving all training plans.", ex);
            }
        }

        public async Task<Models.TrainingPlan> GetTrainingPlanByIdAsync(int id)
        {
            try
            {
                var plan = await _trainingPlanRepo.GetTrainingPlanByIdAsync(id);
                if (plan == null) throw new Exception($"Training plan with ID {id} not found.");
                return plan;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving training plan ID {id}.", ex);
            }
        }

        public async Task<Models.TrainingPlan> GetTrainingPlanByUserIdAsync(int userId)
        {
            try
            {
                var plan = await _trainingPlanRepo.GetTrainingPlanByUserIdAsync(userId);
                return plan;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving training plan for user ID {userId}.", ex);
            }
        }

        public async Task<Models.TrainingPlan> GetTrainingPlanByExerciseTypeAsync(int exerciseTypeId)
        {
            try
            {
                var plan = await _trainingPlanRepo.GetTrainingPlanByExerciseTypeAsync(exerciseTypeId);
                return plan;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving training plan for exercise type ID {exerciseTypeId}.", ex);
            }
        }

        public async Task<Models.TrainingPlan> GetTrainingPlanByUserAndExerciseTypeAsync(int userId, int exerciseTypeId)
        {
            try
            {
                var plan = await _trainingPlanRepo.GetTrainingPlanByUserAndExerciseTypeAsync(userId, exerciseTypeId);
                return plan;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while retrieving training plan for specific user and exercise type.", ex);
            }
        }

        public async Task<bool> TrainingPlanHasExercises(Models.TrainingPlan trainingPlan)
        {
            try
            {
                var hasExercises = await _trainingPlanRepo.TrainingPlanHasExercises(trainingPlan);
                return hasExercises;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while checking for exercises in training plan.", ex);
            }
        }

        public async Task<Models.TrainingPlan> CreateTrainingPlanAsync(Models.TrainingPlan trainingPlan, string token)
        {
            try
            {
                var newPlan = await _trainingPlanRepo.CreateTrainingPlanAsync(trainingPlan, token);
                return newPlan;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while creating training plan.", ex);
            }
        }

        public async Task<Models.TrainingPlan> UpdateTrainingPlanAsync(Models.TrainingPlan trainingPlan, int id, string token)
        {
            try
            {
                var updatedPlan = await _trainingPlanRepo.UpdateTrainingPlanAsync(trainingPlan, id, token);
                return updatedPlan;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while updating training plan ID {id}.", ex);
            }
        }

        public async Task<bool> DisableTrainingPlanAsync(int id, string token)
        {
            try
            {
                var result = await _trainingPlanRepo.DisableTrainingPlanAsync(id, token);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while disabling training plan ID {id}.", ex);
            }
        }
    }
}