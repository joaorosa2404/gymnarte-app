using GymnArteApp.Server.Business.Interface;

namespace GymnArteApp.Server.Business
{
    public class Exercise : IExercise
    {
        private readonly Repo.Interface.IExercise _exerciseRepo;

        public Exercise(Repo.Interface.IExercise exerciseRepo)
        {
            _exerciseRepo = exerciseRepo;
        }

        public async Task<IEnumerable<Models.Exercise>> GetAllExercisesAsync()
        {
            try
            {
                var exercises = await _exerciseRepo.GetAllExercisesAsync();
                return exercises;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while retrieving all exercises.", ex);
            }
        }

        public async Task<Models.Exercise> GetExerciseByIdAsync(int id)
        {
            try
            {
                var exercise = await _exerciseRepo.GetExerciseByIdAsync(id);
                if (exercise == null) throw new Exception($"Exercise with ID {id} not found.");

                return exercise;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving exercise ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Models.Exercise>> GetExercisesByTypeIdAsync(int typeId)
        {
            try
            {
                var exercises = await _exerciseRepo.GetExercisesByTypeIdAsync(typeId);
                return exercises;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving exercises for type ID {typeId}.", ex);
            }
        }

        public async Task<Models.Exercise> CreateExerciseAsync(Models.Exercise exerc)
        {
            try
            {
                var newExercise = await _exerciseRepo.CreateExerciseAsync(exerc);
                return newExercise;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while creating exercise.", ex);
            }
        }

        public async Task<Models.Exercise> UpdateExerciseAsync(int id, Models.Exercise exerc, string token)
        {
            try
            {
                var updatedExercise = await _exerciseRepo.UpdateExerciseAsync(id, exerc, token);
                return updatedExercise;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while updating exercise ID {id}.", ex);
            }
        }

        public async Task<bool> DeleteExerciseAsync(int id, string token)
        {
            try
            {
                var result = await _exerciseRepo.DeleteExerciseAsync(id, token);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while deleting exercise ID {id}.", ex);
            }
        }
    }
}