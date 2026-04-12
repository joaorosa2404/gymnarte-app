using Microsoft.EntityFrameworkCore;
using GymnArteApp.Server.Repo.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace GymnArteApp.Server.Repo
{
    public class TrainingPlan : ITrainingPlan
    {
        private readonly Data.GymDbContext _context;

        public TrainingPlan(Data.GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.TrainingPlan>> GetAllTrainingPlansAsync()
        {
            try
            {
                var plans = await _context.TrainingPlans
                    .Include(tp => tp.User)
                    .Include(tp => tp.ExerciseType)
                    .AsNoTracking()
                    .ToListAsync();
                return plans;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all training plans.", ex);
            }
        }

        public async Task<Models.TrainingPlan> GetTrainingPlanByIdAsync(int id)
        {
            try
            {
                var plan = await _context.TrainingPlans
                    .Include(tp => tp.User)
                    .Include(tp => tp.ExerciseType)
                    .Include(tp => tp.ExerciseTrainingPlans)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(tp => tp.TrainingPlanId == id);
                return plan;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the training plan with ID {id}.", ex);
            }
        }

        public async Task<Models.TrainingPlan> GetTrainingPlanByUserIdAsync(int userId)
        {
            try
            {
                var plans = await _context.TrainingPlans
                    .Where(tp => tp.UserId == userId)
                    .Include(tp => tp.User)
                    .Include(tp => tp.ExerciseType)
                    .AsNoTracking()
                    .ToListAsync();
                return plans.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the training plan for user ID {userId}.", ex);
            }
        }

        public async Task<Models.TrainingPlan> GetTrainingPlanByExerciseTypeAsync(int exerciseTypeId)
        {
            try
            {
                var plans = await _context.TrainingPlans
                    .Where(tp => tp.ExerciseTypeId == exerciseTypeId)
                    .Include(tp => tp.User)
                    .Include(tp => tp.ExerciseType)
                    .AsNoTracking()
                    .ToListAsync();
                return plans.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the training plan for exercise type ID {exerciseTypeId}.", ex);
            }
        }

        public async Task<Models.TrainingPlan> GetTrainingPlanByUserAndExerciseTypeAsync(int userId, int exerciseTypeId)
        {
            try
            {
                var plans = await _context.TrainingPlans
                    .Where(tp => tp.UserId == userId && tp.ExerciseTypeId == exerciseTypeId)
                    .Include(tp => tp.User)
                    .Include(tp => tp.ExerciseType)
                    .AsNoTracking()
                    .ToListAsync();
                return plans.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the training plan for the specified user and exercise type.", ex);
            }
        }

        public async Task<bool> TrainingPlanHasExercises(Models.TrainingPlan trainingPlan)
        {
            try
            {
                // Verifica se existe algum registro na tabela de junção para este plano
                var exerciseCount = await _context.ExerciseTrainingPlans
                    .CountAsync(etp => etp.TrainingPlanId == trainingPlan.TrainingPlanId);
                return exerciseCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while checking if the training plan has exercises.", ex);
            }
        }

        public async Task<Models.TrainingPlan> CreateTrainingPlanAsync(Models.TrainingPlan trainingPlan, string token)
        {
            try
            {
                trainingPlan.CreationDate = DateTime.UtcNow;

                _context.TrainingPlans.Add(trainingPlan);
                await _context.SaveChangesAsync();

                return trainingPlan;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the training plan.", ex);
            }
        }

        public async Task<Models.TrainingPlan> UpdateTrainingPlanAsync(Models.TrainingPlan trainingPlan, int id, string token)
        {
            try
            {
                var existingPlan = await _context.TrainingPlans.FirstOrDefaultAsync(tp => tp.TrainingPlanId == id);

                if (existingPlan == null)
                {
                    throw new Exception($"Training plan with ID {id} not found.");
                }

                existingPlan.TrainingPlanName = trainingPlan.TrainingPlanName;
                existingPlan.Notes = trainingPlan.Notes;
                existingPlan.ExerciseTypeId = trainingPlan.ExerciseTypeId;
                existingPlan.UserId = trainingPlan.UserId; // Geralmente planos não mudam de dono, mas mantive conforme o objeto enviado
                existingPlan.UpdatedDate = DateTime.UtcNow;
                existingPlan.UpdatedUserId = GetUserIdFromToken(token);

                _context.TrainingPlans.Update(existingPlan);
                await _context.SaveChangesAsync();

                return existingPlan;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the training plan with ID {id}.", ex);
            }
        }

        public async Task<bool> DisableTrainingPlanAsync(int id, string token)
        {
            try
            {
                var plan = await _context.TrainingPlans.FirstOrDefaultAsync(tp => tp.TrainingPlanId == id);

                if (plan == null)
                {
                    throw new Exception($"Training plan with ID {id} not found.");
                }

                plan.UpdatedDate = DateTime.UtcNow;
                plan.UpdatedUserId = GetUserIdFromToken(token);

                _context.TrainingPlans.Update(plan);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while disabling the training plan with ID {id}.", ex);
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
