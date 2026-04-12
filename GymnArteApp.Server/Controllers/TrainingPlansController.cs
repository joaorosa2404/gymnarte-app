using GymnArteApp.Server.Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymnArteApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TrainingPlansController : ControllerBase
    {
        private readonly ITrainingPlan _svc;

        public TrainingPlansController(ITrainingPlan svc) => _svc = svc;

        private string BearerToken =>
            Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

        // GET api/trainingplans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.TrainingPlan>>> GetAll()
        {
            var plans = await _svc.GetAllTrainingPlansAsync();
            return Ok(plans);
        }

        // GET api/trainingplans/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Models.TrainingPlan>> GetById(int id)
        {
            try
            {
                var plan = await _svc.GetTrainingPlanByIdAsync(id);
                return Ok(plan);
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET api/trainingplans/user/{userId}
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<Models.TrainingPlan>> GetByUser(int userId)
        {
            var plan = await _svc.GetTrainingPlanByUserIdAsync(userId);
            return plan is null ? NotFound() : Ok(plan);
        }

        // GET api/trainingplans/exercisetype/{exerciseTypeId}
        [HttpGet("exercisetype/{exerciseTypeId:int}")]
        public async Task<ActionResult<Models.TrainingPlan>> GetByExerciseType(int exerciseTypeId)
        {
            var plan = await _svc.GetTrainingPlanByExerciseTypeAsync(exerciseTypeId);
            return plan is null ? NotFound() : Ok(plan);
        }

        // GET api/trainingplans/user/{userId}/exercisetype/{exerciseTypeId}
        [HttpGet("user/{userId:int}/exercisetype/{exerciseTypeId:int}")]
        public async Task<ActionResult<Models.TrainingPlan>> GetByUserAndType(int userId, int exerciseTypeId)
        {
            var plan = await _svc.GetTrainingPlanByUserAndExerciseTypeAsync(userId, exerciseTypeId);
            return plan is null ? NotFound() : Ok(plan);
        }

        // POST api/trainingplans
        [HttpPost]
        public async Task<ActionResult<Models.TrainingPlan>> Create([FromBody] Models.TrainingPlan plan)
        {
            var created = await _svc.CreateTrainingPlanAsync(plan, BearerToken);
            return CreatedAtAction(nameof(GetById), new { id = created.TrainingPlanId }, created);
        }

        // PUT api/trainingplans/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Models.TrainingPlan>> Update(int id, [FromBody] Models.TrainingPlan plan)
        {
            try
            {
                var updated = await _svc.UpdateTrainingPlanAsync(plan, id, BearerToken);
                return Ok(updated);
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE api/trainingplans/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Disable(int id)
        {
            var result = await _svc.DisableTrainingPlanAsync(id, BearerToken);
            return result ? NoContent() : NotFound();
        }
    }
}
