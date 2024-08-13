using Microsoft.AspNetCore.Identity;

namespace ToDoApp_API.Models;

public class ApplicationUser: IdentityUser
{
    public ICollection<Task> Tasks { get; } = new List<Task>();
}