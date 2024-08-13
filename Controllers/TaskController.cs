using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoApp_API.DTOs;
using ToDoApp_API.Models;
using Task = ToDoApp_API.Models.Task; // Ensure Task is imported correctly

namespace ToDoApp_API.Controllers
{
    [Route("api/tasks")]
    [ApiController] // Add the ApiController attribute for automatic model state validation
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AllowAngularApp")]
    public class TaskController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TaskCreationDTO taskCreationDto)
        {
            if (!ModelState.IsValid) // Ensure model state is valid
                return BadRequest(ModelState);
          
            
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            var user = context.Users.Include(u => u.Tasks).FirstOrDefault(u => u.Email == userEmail);
            var task = mapper.Map<Task>(taskCreationDto); // Correct mapping
            task.UserId = user.Id;
            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync(); // Don't forget to save changes

            return NoContent(); // Return NoContent or CreatedAtAction if you want to return the created resource
        }

        [HttpGet]
        public async Task<ActionResult<List<Task>>> Get()
        {
            //var user2 = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var userClaims = User.Claims;
            var email = userClaims.FirstOrDefault(c => c.Type == "Email");
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            var user = context.Users.Include(u => u.Tasks).FirstOrDefault(u => u.Email == userEmail);
            
            //var tasks = await context.Tasks.Where(t => t.UserId == user.Id).OrderBy(x => x.CreatedAt).ToListAsync();
            return user.Tasks.ToList(); // Use ActionResult to return an appropriate HTTP status code or other types
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