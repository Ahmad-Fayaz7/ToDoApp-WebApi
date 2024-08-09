using Microsoft.EntityFrameworkCore;

namespace ToDoApp_API.Models;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Task> Tasks {
        get;
        set;
    }
}
