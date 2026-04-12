using GymnArteApp.Server.Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymnArteApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExercisesController : ControllerBase
    {
        private readonly IExercise _svc;

        public ExercisesController(IExercise svc) => _svc = svc;

        private string BearerToken =>
            Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

        // GET api/exercises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Exercise>>> GetAll()
            => Ok(await _svc.GetAllExercisesAsync());

        // GET api/exercises/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Models.Exercise>> GetById(int id)
        {
            try   { return Ok(await _svc.GetExerciseByIdAsync(id)); }
            catch (Exception ex) when (ex.Message.Contains("not found"))
                  { return NotFound(new { message = ex.Message }); }
        }

        // GET api/exercises/type/{typeId}
        [HttpGet("type/{typeId:int}")]
        public async Task<ActionResult<IEnumerable<Models.Exercise>>> GetByType(int typeId)
            => Ok(await _svc.GetExercisesByTypeIdAsync(typeId));

        // POST api/exercises
        [HttpPost]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<ActionResult<Models.Exercise>> Create([FromBody] Models.Exercise exercise)
        {
            var created = await _svc.CreateExerciseAsync(exercise);
            return CreatedAtAction(nameof(GetById), new { id = created.ExerciseId }, created);
        }

        // PUT api/exercises/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<ActionResult<Models.Exercise>> Update(int id, [FromBody] Models.Exercise exercise)
        {
            try   { return Ok(await _svc.UpdateExerciseAsync(id, exercise, BearerToken)); }
            catch (Exception ex) when (ex.Message.Contains("not found"))
                  { return NotFound(new { message = ex.Message }); }
        }

        // DELETE api/exercises/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _svc.DeleteExerciseAsync(id, BearerToken);
            return result ? NoContent() : NotFound();
        }
    }
}
