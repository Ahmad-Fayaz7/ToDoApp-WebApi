using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToDoApp_API.Models;
using ToDoApp_API.Services;

namespace ToDoApp_API.Controllers;
[Route("api/accounts")]
[ApiController]
public class AccountController(TokenService tokenService): ControllerBase
{
    [HttpPost]
    public IActionResult Login([FromBody] LoginModel login)
    {
        // Validate the user credentials (this is just an example)
        if (login.Email == "testuser@example.com" && login.Password == "password")
        {
            var token = tokenService.GenerateJwtToken(login.Email);
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }
    
    
    
}