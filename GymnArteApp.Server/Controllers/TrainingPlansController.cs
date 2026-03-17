using System.Collections.Generic;
using System.Threading.Tasks;
using GymnArteApp.Server.Models;
using GymnArteApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymnArteApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingPlansController : ControllerBase
    {
        private readonly ITrainingPlanService _svc;
        public TrainingPlansController(ITrainingPlanService svc) => _svc = svc;

        [HttpGet]
        public Task<IEnumerable<TrainingPlan>> GetAll() => _svc.GetAllAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TrainingPlan>> GetById(int id)
        {
            var p = await _svc.GetByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        public async Task<ActionResult<TrainingPlan>> Create([FromBody] TrainingPlan plan)
        {
            var created = await _svc.CreateAsync(plan);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TrainingPlan plan)
        {
            if (id != plan.Id) return BadRequest();
            await _svc.UpdateAsync(plan);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public Task Delete(int id) => _svc.DeleteAsync(id);
    }
}