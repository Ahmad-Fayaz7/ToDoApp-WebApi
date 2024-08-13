using System.ComponentModel.DataAnnotations;

namespace ToDoApp_API.Models;

public class SignUpModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}