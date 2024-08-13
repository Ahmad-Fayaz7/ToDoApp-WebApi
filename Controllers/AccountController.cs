using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToDoApp_API.Models;
using ToDoApp_API.Services;

namespace ToDoApp_API.Controllers;
[Route("api/accounts")]
[ApiController]
public class AccountController(TokenService tokenService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager): ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] SignUpModel model)
    {
        // Set the Username to null if it's empty
       // var username = string.IsNullOrWhiteSpace(model.Username) ? null : model.Username;
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email};
        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);
        var token = tokenService.GenerateJwtToken(user.Email);
        return Ok(new { Token = token });

    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: false,
            lockoutOnFailure: false);
        // Validate the user credentials 
        if (!result.Succeeded) return Unauthorized();
        var token = tokenService.GenerateJwtToken(login.Email);
        return Ok(new { Token = token });

    }
    
    
    
}