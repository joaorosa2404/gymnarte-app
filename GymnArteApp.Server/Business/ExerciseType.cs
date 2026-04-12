using GymnArteApp.Server.Business.Interface;

namespace GymnArteApp.Server.Business
{
    public class ExerciseType : IExerciseType
    {
        private readonly Repo.Interface.IExerciseType _exerciseTypeRepo;

        public ExerciseType(Repo.Interface.IExerciseType exerciseTypeRepo)
        {
            _exerciseTypeRepo = exerciseTypeRepo;
        }

        public async Task<IEnumerable<Models.ExerciseType>> GetAllExerciseTypesAsync()
        {
            try
            {
                var types = await _exerciseTypeRepo.GetAllExerciseTypesAsync();
                return types;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while retrieving all exercise types.", ex);
            }
        }

        public async Task<Models.ExerciseType> GetExerciseTypeByIdAsync(int id)
        {
            try
            {
                var type = await _exerciseTypeRepo.GetExerciseTypeByIdAsync(id);
                if (type == null) throw new Exception($"Exercise type with ID {id} not found.");

                return type;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while retrieving exercise type ID {id}.", ex);
            }
        }

        public async Task<Models.ExerciseType> CreateExerciseTypeAsync(Models.ExerciseType exerc)
        {
            try
            {
                var newType = await _exerciseTypeRepo.CreateExerciseTypeAsync(exerc);
                return newType;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business layer while creating exercise type.", ex);
            }
        }

        public async Task<Models.ExerciseType> UpdateExerciseTypeAsync(int id, Models.ExerciseType exerc, string token)
        {
            try
            {
                var updatedType = await _exerciseTypeRepo.UpdateExerciseTypeAsync(id, exerc, token);
                return updatedType;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while updating exercise type ID {id}.", ex);
            }
        }

        public async Task<bool> DisableExerciseTypeAsync(int id, string token)
        {
            try
            {
                var result = await _exerciseTypeRepo.DisableExerciseTypeAsync(id, token);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Business layer while disabling exercise type ID {id}.", ex);
            }
        }
    }
}