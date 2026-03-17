namespace GymnArteApp.Server.Repo.Interface
{
    public interface IExerciseType
    {
        Task<IEnumerable<Models.ExerciseType>> GetAllExerciseTypesAsync();
        Task<Models.ExerciseType> GetExerciseTypeByIdAsync(int id);
        Task<Models.ExerciseType> CreateExerciseTypeAsync(Models.ExerciseType exerc);
        Task<Models.ExerciseType> UpdateExerciseTypeAsync(int id, Models.ExerciseType exerc, string token);
        Task<bool> DisableExerciseTypeAsync(int id, string token);
    }
}
