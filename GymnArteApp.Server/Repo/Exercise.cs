using Microsoft.EntityFrameworkCore;
using GymnArteApp.Server.Repo.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace GymnArteApp.Server.Repo
{
    public class Exercise : IExercise
    {
        private readonly Data.GymDbContext _context;

        public Exercise(Data.GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Exercise>> GetAllExercisesAsync()
        {
            try
            {
                return await _context.Exercises
                    .Include(e => e.ExerciseType)
                    .Include(e => e.UpdatedUser)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all exercises.", ex);
            }
        }

        public async Task<Models.Exercise> GetExerciseByIdAsync(int id)
        {
            try
            {
                var exercise = await _context.Exercises
                    .Include(e => e.ExerciseType)
                    .Include(e => e.UpdatedUser)
                    .Include(e => e.ExerciseTrainingPlans)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.ExerciseId == id);

                return exercise;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the exercise with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Models.Exercise>> GetExercisesByTypeIdAsync(int typeId)
        {
            try
            {
                return await _context.Exercises
                    .Where(e => e.ExerciseTypeId == typeId)
                    .Include(e => e.ExerciseType)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving exercises for type ID {typeId}.", ex);
            }
        }

        public async Task<Models.Exercise> CreateExerciseAsync(Models.Exercise exerc)
        {
            try
            {
                exerc.CreationDate = DateTime.UtcNow;

                _context.Exercises.Add(exerc);
                await _context.SaveChangesAsync();

                return exerc;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the exercise.", ex);
            }
        }

        public async Task<Models.Exercise> UpdateExerciseAsync(int id, Models.Exercise exerc, string token)
        {
            try
            {
                var existingExercise = await _context.Exercises.FirstOrDefaultAsync(e => e.ExerciseId == id);

                if (existingExercise == null)
                {
                    throw new Exception($"Exercise with ID {id} not found.");
                }

                existingExercise.Name = exerc.Name;
                existingExercise.Notes = exerc.Notes;
                existingExercise.ExerciseTypeId = exerc.ExerciseTypeId;
                existingExercise.UpdatedDate = DateTime.UtcNow;
                existingExercise.UpdatedUserId = GetUserIdFromToken(token);

                _context.Exercises.Update(existingExercise);
                await _context.SaveChangesAsync();

                return existingExercise;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the exercise with ID {id}.", ex);
            }
        }

        public async Task<bool> DeleteExerciseAsync(int id, string token)
        {
            try
            {
                var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.ExerciseId == id);

                if (exercise == null)
                {
                    throw new Exception($"Exercise with ID {id} not found.");
                }

                _context.Exercises.Remove(exercise);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the exercise with ID {id}.", ex);
            }
        }

        private int? GetUserIdFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type == "sub")?.Value;

                return userIdClaim != null ? int.Parse(userIdClaim) : null;
            }
            catch
            {
                return null;
            }
        }
    }
}