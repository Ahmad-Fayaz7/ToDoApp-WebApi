namespace ToDoApp_API.DTOs;

public class TaskCreationDTO
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
   // public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}