using Microsoft.EntityFrameworkCore.Design.Internal;

namespace GymnArteApp.Server.Repo.Interface
{
    public interface IExercise
    {
        Task<IEnumerable<Models.Exercise>> GetAllExercisesAsync();
        Task<Models.Exercise> GetExerciseByIdAsync(int id);
        Task<Models.Exercise> CreateExerciseAsync(Models.Exercise exerc);
        Task<Models.Exercise> UpdateExerciseAsync(int id, Models.Exercise exerc, string token);
        Task<bool> DeleteExerciseAsync(int id, string token);
        Task<IEnumerable<Models.Exercise>> GetExercisesByTypeIdAsync(int typeId);
    }
}
