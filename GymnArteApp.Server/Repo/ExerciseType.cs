using Microsoft.EntityFrameworkCore;
using GymnArteApp.Server.Repo.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace GymnArteApp.Server.Repo
{
    public class ExerciseType : IExerciseType
    {
        private readonly Data.GymDbContext _context;

        public ExerciseType(Data.GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.ExerciseType>> GetAllExerciseTypesAsync()
        {
            try
            {
                return await _context.ExerciseTypes
                    .Include(et => et.UpdatedUser)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all exercise types.", ex);
            }
        }

        public async Task<Models.ExerciseType> GetExerciseTypeByIdAsync(int id)
        {
            try
            {
                var exerciseType = await _context.ExerciseTypes
                    .Include(et => et.UpdatedUser)
                    .Include(et => et.Exercises)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(et => et.ExerciseTypeId == id);

                return exerciseType;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the exercise type with ID {id}.", ex);
            }
        }

        public async Task<Models.ExerciseType> CreateExerciseTypeAsync(Models.ExerciseType exerc)
        {
            try
            {
                exerc.CreationDate = DateTime.UtcNow;

                _context.ExerciseTypes.Add(exerc);
                await _context.SaveChangesAsync();

                return exerc;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the exercise type.", ex);
            }
        }

        public async Task<Models.ExerciseType> UpdateExerciseTypeAsync(int id, Models.ExerciseType exerc, string token)
        {
            try
            {
                var existingType = await _context.ExerciseTypes.FirstOrDefaultAsync(et => et.ExerciseTypeId == id);

                if (existingType == null)
                {
                    throw new Exception($"Exercise type with ID {id} not found.");
                }

                existingType.Name = exerc.Name;
                existingType.Description = exerc.Description;
                existingType.UpdatedDate = DateTime.UtcNow;
                existingType.UpdatedUserId = GetUserIdFromToken(token);

                _context.ExerciseTypes.Update(existingType);
                await _context.SaveChangesAsync();

                return existingType;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the exercise type with ID {id}.", ex);
            }
        }

        public async Task<bool> DisableExerciseTypeAsync(int id, string token)
        {
            try
            {
                var exerciseType = await _context.ExerciseTypes.FirstOrDefaultAsync(et => et.ExerciseTypeId == id);

                if (exerciseType == null)
                {
                    throw new Exception($"Exercise type with ID {id} not found.");
                }

                exerciseType.UpdatedDate = DateTime.UtcNow;
                exerciseType.UpdatedUserId = GetUserIdFromToken(token);

                _context.ExerciseTypes.Update(exerciseType);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while disabling the exercise type with ID {id}.", ex);
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