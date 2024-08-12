using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ToDoApp_API.DTOs;
using ToDoApp_API.Models;
using Task = ToDoApp_API.Models.Task; // Ensure Task is imported correctly

namespace ToDoApp_API.Controllers
{
    [Route("api/tasks")]
    [ApiController] // Add the ApiController attribute for automatic model state validation
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TaskController(ApplicationDbContext context, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TaskCreationDTO taskCreationDto)
        {
            if (!ModelState.IsValid) // Ensure model state is valid
                return BadRequest(ModelState);

            var task = mapper.Map<Task>(taskCreationDto); // Correct mapping
            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync(); // Don't forget to save changes

            return NoContent(); // Return NoContent or CreatedAtAction if you want to return the created resource
        }

        [HttpGet]
        public async Task<ActionResult<List<Task>>> Get()
        {
            var tasks = await context.Tasks.OrderBy(x => x.CreatedAt).ToListAsync();
            return tasks; // Use ActionResult to return an appropriate HTTP status code or other types
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Task>> GetById(int id)
        {
            var task = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            return task;
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var task = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task != null)
            {
                context.Tasks.Remove(task);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] TaskCreationDTO taskCreationDto)
        {
            var task = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            task = mapper.Map(taskCreationDto, task);
            context.Tasks.Update(task);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}