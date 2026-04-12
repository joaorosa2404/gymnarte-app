using GymnArteApp.Server.Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymnArteApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExerciseTypesController : ControllerBase
    {
        private readonly IExerciseType _svc;

        public ExerciseTypesController(IExerciseType svc) => _svc = svc;

        private string BearerToken =>
            Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

        // GET api/exercisetypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.ExerciseType>>> GetAll()
            => Ok(await _svc.GetAllExerciseTypesAsync());

        // GET api/exercisetypes/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Models.ExerciseType>> GetById(int id)
        {
            try   { return Ok(await _svc.GetExerciseTypeByIdAsync(id)); }
            catch (Exception ex) when (ex.Message.Contains("not found"))
                  { return NotFound(new { message = ex.Message }); }
        }

        // POST api/exercisetypes
        [HttpPost]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<ActionResult<Models.ExerciseType>> Create([FromBody] Models.ExerciseType exerciseType)
        {
            var created = await _svc.CreateExerciseTypeAsync(exerciseType);
            return CreatedAtAction(nameof(GetById), new { id = created.ExerciseTypeId }, created);
        }

        // PUT api/exercisetypes/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,PersonalTrainer")]
        public async Task<ActionResult<Models.ExerciseType>> Update(int id, [FromBody] Models.ExerciseType exerciseType)
        {
            try   { return Ok(await _svc.UpdateExerciseTypeAsync(id, exerciseType, BearerToken)); }
            catch (Exception ex) when (ex.Message.Contains("not found"))
                  { return NotFound(new { message = ex.Message }); }
        }

        // DELETE api/exercisetypes/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Disable(int id)
        {
            var result = await _svc.DisableExerciseTypeAsync(id, BearerToken);
            return result ? NoContent() : NotFound();
        }
    }
}
