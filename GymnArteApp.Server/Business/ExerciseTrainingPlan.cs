using GymnArteApp.Server.Business.Interface;

namespace GymnArteApp.Server.Business
{
    public class ExerciseTrainingPlan : IExerciseTrainingPlan
    {
        private readonly Repo.Interface.IExerciseTrainingPlan _exerciseTrainingPlanRepo;

        public ExerciseTrainingPlan(Repo.Interface.IExerciseTrainingPlan exerciseTrainingPlanRepo)
        {
            _exerciseTrainingPlanRepo = exerciseTrainingPlanRepo;
        }

        public async Task<IEnumerable<Models.ExerciseTrainingPlan>> GetAllExerciseTrainingPlansAsync()
        {
            try
            {
                var relations = await _exerciseTrainingPlanRepo.GetAllExerciseTrainingPlansAsync();
                return relations;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while retrieving all exercise training plans.", ex);
            }
        }

        public async Task<Models.ExerciseTrainingPlan> GetExerciseTrainingPlanByIdAsync(int id)
        {
            try
            {
                var relation = await _exerciseTrainingPlanRepo.GetExerciseTrainingPlanByIdAsync(id);
                if (relation == null) throw new Exception($"Exercise training plan relation with ID {id} not found.");

                return relation;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving exercise training plan ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Models.ExerciseTrainingPlan>> GetExerciseTrainingPlansByExerciseIdAsync(int exerciseId)
        {
            try
            {
                var relations = await _exerciseTrainingPlanRepo.GetExerciseTrainingPlansByExerciseIdAsync(exerciseId);
                return relations;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving plans for exercise ID {exerciseId}.", ex);
            }
        }

        public async Task<IEnumerable<Models.ExerciseTrainingPlan>> GetExerciseTrainingPlansByTrainingPlanIdAsync(int trainingPlanId)
        {
            try
            {
                var relations = await _exerciseTrainingPlanRepo.GetExerciseTrainingPlansByTrainingPlanIdAsync(trainingPlanId);
                return relations;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving exercises for training plan ID {trainingPlanId}.", ex);
            }
        }

        public async Task<IEnumerable<Models.ExerciseTrainingPlan>> GetExerciseTrainingPlansByExerciseAndTrainingPlanIdAsync(int exerciseId, int trainingPlanId)
        {
            try
            {
                var relations = await _exerciseTrainingPlanRepo.GetExerciseTrainingPlansByExerciseAndTrainingPlanIdAsync(exerciseId, trainingPlanId);
                return relations;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while retrieving specific exercise and training plan association.", ex);
            }
        }

        public async Task<Models.ExerciseTrainingPlan> CreateExerciseTrainingPlanAsync(Models.ExerciseTrainingPlan exerciseTrainingPlan, string token)
        {
            try
            {
                var newRelation = await _exerciseTrainingPlanRepo.CreateExerciseTrainingPlanAsync(exerciseTrainingPlan, token);
                return newRelation;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while creating exercise training plan relation.", ex);
            }
        }

        public async Task<Models.ExerciseTrainingPlan> UpdateExerciseTrainingPlanAsync(Models.ExerciseTrainingPlan exerciseTrainingPlan, int id, string token)
        {
            try
            {
                var updatedRelation = await _exerciseTrainingPlanRepo.UpdateExerciseTrainingPlanAsync(exerciseTrainingPlan, id, token);
                return updatedRelation;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while updating exercise training plan ID {id}.", ex);
            }
        }

        public async Task<bool> DisableExerciseTrainingPlanAsync(int id, string token)
        {
            try
            {
                var result = await _exerciseTrainingPlanRepo.DisableExerciseTrainingPlanAsync(id, token);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while disabling exercise training plan ID {id}.", ex);
            }
        }
    }
}