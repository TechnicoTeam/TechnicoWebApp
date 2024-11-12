using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Technico.Main.Services;


namespace Technico.Main.Controllers;

public class AuthController : Controller
{

    private readonly IOwnerService _ownerService;

    public AuthController(IOwnerService ownerService)
    {
        _ownerService = ownerService;
    }

    [HttpPost("Auth/LogIn")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        Console.WriteLine($"Received Username: {request.Username}, Password: {request.Password}");

        var owners = await _ownerService.GetAllOwners();
        var owner = owners.Where(owner => owner.Email.Equals(request.Username)).FirstOrDefault();
        var user = new User
        {
            Password = owner.Email,
            Username = owner.Email,
        };

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
    public string ?Username { get; set; }
    public string ?Password { get; set; }
}

public class User
{
    public string ?Username { get; set; }
    public string ?Password { get; set; }
}


