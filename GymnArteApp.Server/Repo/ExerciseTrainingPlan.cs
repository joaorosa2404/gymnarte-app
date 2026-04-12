using Microsoft.EntityFrameworkCore;
using GymnArteApp.Server.Repo.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace GymnArteApp.Server.Repo
{
    public class ExerciseTrainingPlan : IExerciseTrainingPlan
    {
        private readonly Data.GymDbContext _context;

        public ExerciseTrainingPlan(Data.GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.ExerciseTrainingPlan>> GetAllExerciseTrainingPlansAsync()
        {
            try
            {
                return await _context.ExerciseTrainingPlans
                    .Include(etp => etp.Exercise)
                    .Include(etp => etp.TrainingPlan)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all exercise training plans.", ex);
            }
        }

        public async Task<Models.ExerciseTrainingPlan> GetExerciseTrainingPlanByIdAsync(int id)
        {
            try
            {
                return await _context.ExerciseTrainingPlans
                    .Include(etp => etp.Exercise)
                    .Include(etp => etp.TrainingPlan)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(etp => etp.ExerciseTrainingPlanId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the exercise training plan with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Models.ExerciseTrainingPlan>> GetExerciseTrainingPlansByExerciseIdAsync(int exerciseId)
        {
            try
            {
                return await _context.ExerciseTrainingPlans
                    .Where(etp => etp.ExerciseId == exerciseId)
                    .Include(etp => etp.Exercise)
                    .Include(etp => etp.TrainingPlan)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving plans for exercise ID {exerciseId}.", ex);
            }
        }

        public async Task<IEnumerable<Models.ExerciseTrainingPlan>> GetExerciseTrainingPlansByTrainingPlanIdAsync(int trainingPlanId)
        {
            try
            {
                return await _context.ExerciseTrainingPlans
                    .Where(etp => etp.TrainingPlanId == trainingPlanId)
                    .Include(etp => etp.Exercise)
                    .Include(etp => etp.TrainingPlan)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving exercises for training plan ID {trainingPlanId}.", ex);
            }
        }

        public async Task<IEnumerable<Models.ExerciseTrainingPlan>> GetExerciseTrainingPlansByExerciseAndTrainingPlanIdAsync(int exerciseId, int trainingPlanId)
        {
            try
            {
                return await _context.ExerciseTrainingPlans
                    .Where(etp => etp.ExerciseId == exerciseId && etp.TrainingPlanId == trainingPlanId)
                    .Include(etp => etp.Exercise)
                    .Include(etp => etp.TrainingPlan)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the specific exercise and training plan association.", ex);
            }
        }

        public async Task<Models.ExerciseTrainingPlan> CreateExerciseTrainingPlanAsync(Models.ExerciseTrainingPlan exerciseTrainingPlan, string token)
        {
            try
            {
                _context.ExerciseTrainingPlans.Add(exerciseTrainingPlan);
                await _context.SaveChangesAsync();
                return exerciseTrainingPlan;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the exercise training plan.", ex);
            }
        }

        public async Task<Models.ExerciseTrainingPlan> UpdateExerciseTrainingPlanAsync(Models.ExerciseTrainingPlan exerciseTrainingPlan, int id, string token)
        {
            try
            {
                var existing = await _context.ExerciseTrainingPlans.FirstOrDefaultAsync(etp => etp.ExerciseTrainingPlanId == id);

                if (existing == null)
                {
                    throw new Exception($"Exercise training plan with ID {id} not found.");
                }

                existing.Sets = exerciseTrainingPlan.Sets;
                existing.Reps = exerciseTrainingPlan.Reps;
                existing.Notes = exerciseTrainingPlan.Notes;
                existing.ExerciseId = exerciseTrainingPlan.ExerciseId;
                existing.TrainingPlanId = exerciseTrainingPlan.TrainingPlanId;

                _context.ExerciseTrainingPlans.Update(existing);
                await _context.SaveChangesAsync();

                return existing;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the exercise training plan with ID {id}.", ex);
            }
        }

        public async Task<bool> DisableExerciseTrainingPlanAsync(int id, string token)
        {
            try
            {
                var existing = await _context.ExerciseTrainingPlans.FirstOrDefaultAsync(etp => etp.ExerciseTrainingPlanId == id);

                if (existing == null)
                {
                    throw new Exception($"Exercise training plan with ID {id} not found.");
                }

                existing.IsActive = true;

                _context.ExerciseTrainingPlans.Update(existing);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while disabling the exercise training plan with ID {id}.", ex);
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