using Microsoft.EntityFrameworkCore;
using GymnArteApp.Server.Repo.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace GymnArteApp.Server.Repo
{
    public class ExerciseTrainingPlan : IExerciseTrainingPlan
    {
        private readonly Data.GymDbContext _context;

        public ExerciseTrainingPlan(Data.GymDbContext context) => _context = context;

        public async Task<IEnumerable<Models.ExerciseTrainingPlan>> GetAllExerciseTrainingPlansAsync()
        {
            return await _context.ExerciseTrainingPlans
                .Include(etp => etp.Exercise)
                .Include(etp => etp.TrainingPlan)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Models.ExerciseTrainingPlan?> GetExerciseTrainingPlanByIdAsync(int id)
        {
            return await _context.ExerciseTrainingPlans
                .Include(etp => etp.Exercise)
                .Include(etp => etp.TrainingPlan)
                .AsNoTracking()
                .FirstOrDefaultAsync(etp => etp.ExerciseTrainingPlanId == id);
        }

        public async Task<IEnumerable<Models.ExerciseTrainingPlan>> GetExerciseTrainingPlansByExerciseIdAsync(int exerciseId)
        {
            return await _context.ExerciseTrainingPlans
                .Where(etp => etp.ExerciseId == exerciseId)
                .Include(etp => etp.Exercise)
                .Include(etp => etp.TrainingPlan)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.ExerciseTrainingPlan>> GetExerciseTrainingPlansByTrainingPlanIdAsync(int trainingPlanId)
        {
            return await _context.ExerciseTrainingPlans
                .Where(etp => etp.TrainingPlanId == trainingPlanId)
                .Include(etp => etp.Exercise)
                .Include(etp => etp.TrainingPlan)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.ExerciseTrainingPlan>> GetExerciseTrainingPlansByExerciseAndTrainingPlanIdAsync(int exerciseId, int trainingPlanId)
        {
            return await _context.ExerciseTrainingPlans
                .Where(etp => etp.ExerciseId == exerciseId && etp.TrainingPlanId == trainingPlanId)
                .Include(etp => etp.Exercise)
                .Include(etp => etp.TrainingPlan)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Models.ExerciseTrainingPlan> CreateExerciseTrainingPlanAsync(
            Models.ExerciseTrainingPlan exerciseTrainingPlan, string token)
        {
            exerciseTrainingPlan.IsActive = true;
            _context.ExerciseTrainingPlans.Add(exerciseTrainingPlan);
            await _context.SaveChangesAsync();
            return exerciseTrainingPlan;
        }

        public async Task<Models.ExerciseTrainingPlan> UpdateExerciseTrainingPlanAsync(
            Models.ExerciseTrainingPlan exerciseTrainingPlan, int id, string token)
        {
            var existing = await _context.ExerciseTrainingPlans.FindAsync(id)
                ?? throw new Exception($"Exercise training plan with ID {id} not found.");

            existing.Sets          = exerciseTrainingPlan.Sets;
            existing.Reps          = exerciseTrainingPlan.Reps;
            existing.Notes         = exerciseTrainingPlan.Notes;
            existing.ExerciseId    = exerciseTrainingPlan.ExerciseId;
            existing.TrainingPlanId = exerciseTrainingPlan.TrainingPlanId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DisableExerciseTrainingPlanAsync(int id, string token)
        {
            var existing = await _context.ExerciseTrainingPlans.FindAsync(id);
            if (existing is null) return false;

            existing.IsActive = false; // BUG ORIGINAL ERA true — corrigido
            await _context.SaveChangesAsync();
            return true;
        }

        private int? GetUserIdFromToken(string token)
        {
            try
            {
                var handler     = new JwtSecurityTokenHandler();
                var jwtToken    = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims
                    .FirstOrDefault(c => c.Type == "userId" || c.Type == "sub")?.Value;
                return userIdClaim != null ? int.Parse(userIdClaim) : null;
            }
            catch { return null; }
        }
    }
}
