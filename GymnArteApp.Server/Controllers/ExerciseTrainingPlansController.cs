using GymnArteApp.Server.Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymnArteApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExerciseTrainingPlansController : ControllerBase
    {
        private readonly IExerciseTrainingPlan _svc;

        public ExerciseTrainingPlansController(IExerciseTrainingPlan svc) => _svc = svc;

        private string BearerToken =>
            Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

        // GET api/exercisetrainingplans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.ExerciseTrainingPlan>>> GetAll()
            => Ok(await _svc.GetAllExerciseTrainingPlansAsync());

        // GET api/exercisetrainingplans/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Models.ExerciseTrainingPlan>> GetById(int id)
        {
            try   { return Ok(await _svc.GetExerciseTrainingPlanByIdAsync(id)); }
            catch (Exception ex) when (ex.Message.Contains("not found"))
                  { return NotFound(new { message = ex.Message }); }
        }

        // GET api/exercisetrainingplans/exercise/{exerciseId}
        [HttpGet("exercise/{exerciseId:int}")]
        public async Task<ActionResult<IEnumerable<Models.ExerciseTrainingPlan>>> GetByExercise(int exerciseId)
            => Ok(await _svc.GetExerciseTrainingPlansByExerciseIdAsync(exerciseId));

        // GET api/exercisetrainingplans/trainingplan/{trainingPlanId}
        [HttpGet("trainingplan/{trainingPlanId:int}")]
        public async Task<ActionResult<IEnumerable<Models.ExerciseTrainingPlan>>> GetByTrainingPlan(int trainingPlanId)
            => Ok(await _svc.GetExerciseTrainingPlansByTrainingPlanIdAsync(trainingPlanId));

        // GET api/exercisetrainingplans/exercise/{exerciseId}/trainingplan/{trainingPlanId}
        [HttpGet("exercise/{exerciseId:int}/trainingplan/{trainingPlanId:int}")]
        public async Task<ActionResult<IEnumerable<Models.ExerciseTrainingPlan>>> GetByBoth(int exerciseId, int trainingPlanId)
            => Ok(await _svc.GetExerciseTrainingPlansByExerciseAndTrainingPlanIdAsync(exerciseId, trainingPlanId));

        // POST api/exercisetrainingplans
        [HttpPost]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<ActionResult<Models.ExerciseTrainingPlan>> Create([FromBody] Models.ExerciseTrainingPlan etp)
        {
            var created = await _svc.CreateExerciseTrainingPlanAsync(etp, BearerToken);
            return CreatedAtAction(nameof(GetById), new { id = created.ExerciseTrainingPlanId }, created);
        }

        // PUT api/exercisetrainingplans/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<ActionResult<Models.ExerciseTrainingPlan>> Update(int id, [FromBody] Models.ExerciseTrainingPlan etp)
        {
            try   { return Ok(await _svc.UpdateExerciseTrainingPlanAsync(etp, id, BearerToken)); }
            catch (Exception ex) when (ex.Message.Contains("not found"))
                  { return NotFound(new { message = ex.Message }); }
        }

        // DELETE api/exercisetrainingplans/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<IActionResult> Disable(int id)
        {
            var result = await _svc.DisableExerciseTrainingPlanAsync(id, BearerToken);
            return result ? NoContent() : NotFound();
        }
    }
}
