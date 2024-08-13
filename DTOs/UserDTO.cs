namespace ToDoApp_API.DTOs;

public class UserDTO
{
    public string Id { get; set; }
    public string Email { get; set; }
    public List<TaskDTO> Tasks { get; set; }
}