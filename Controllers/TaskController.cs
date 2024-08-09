using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoApp_API.DTOs;
using ToDoApp_API.Models;
using Task = ToDoApp_API.Models.Task; // Ensure Task is imported correctly

namespace ToDoApp_API.Controllers
{
    [Route("api/tasks")]
    [ApiController] // Add the ApiController attribute for automatic model state validation
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaskController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromQuery] TaskCreationDTO taskCreationDto)
        {
            if (!ModelState.IsValid) // Ensure model state is valid
                return BadRequest(ModelState);

            var task = _mapper.Map<Task>(taskCreationDto); // Correct mapping
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync(); // Don't forget to save changes

            return NoContent(); // Return NoContent or CreatedAtAction if you want to return the created resource
        }

        [HttpGet]
        public async Task<ActionResult<List<Task>>> Get()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return tasks; // Use ActionResult to return an appropriate HTTP status code or other types
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Task>> GetById(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            return task;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
             _context.Tasks.Remove(task);
             return NoContent();
        }
    }
}