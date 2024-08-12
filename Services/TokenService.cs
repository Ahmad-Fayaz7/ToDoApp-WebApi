namespace ToDoApp_API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
public class TokenService(IConfiguration configuration)
{


public string GenerateJwtToken(string username)
{
    var jwtSettings = configuration.GetSection("JwtSettings");
    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

    var tokenOptions = new JwtSecurityToken(
        issuer: jwtSettings["Issuer"],
        audience: jwtSettings["Audience"],
        claims: new List<Claim>
        {
            new Claim(ClaimTypes.Name, username)
        },
        expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
        signingCredentials: signinCredentials
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    return tokenString;
}

}