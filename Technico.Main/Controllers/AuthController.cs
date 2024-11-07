using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Security.Cryptography;

namespace Technico.Main.Controllers;

public class AuthController : Controller
{
    private readonly List<User> _users = new List<User>
        {
            new User { Username = "test@gmail.com", Password = "test" }
        };

    [HttpPost("Auth/LogIn")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        Console.WriteLine($"Received Username: {request.Username}, Password: {request.Password}");

        var user = _users.SingleOrDefault(u => u.Username == request.Username && u.Password == request.Password);
        if (user == null)
        {
            Console.WriteLine("Invalid credentials");
            return Unauthorized("Invalid credentials");
        }

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    public string GenerateSecretKey()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] secretKey = new byte[32];  // 256 bits
            rng.GetBytes(secretKey);
            return Convert.ToBase64String(secretKey);  // Return as a base64 string
        }
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                // Additional claims can be added here
            };

        var secretKey = GenerateSecretKey();
        var key = new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "YourIssuer",
            audience: "YourAudience",
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
}


