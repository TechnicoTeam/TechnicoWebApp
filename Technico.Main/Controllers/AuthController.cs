using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Technico.Main.DTOs;
using Technico.Main.Repositories.Implementations;
using Technico.Main.Services;
using Technico.Main.Services.Implementations;


namespace Technico.Main.Controllers;

[Route("Auth")]
public class AuthController : Controller
{
    private readonly IOwnerService _ownerService;
    private readonly IConfiguration _configuration;

    public AuthController(IOwnerService ownerService, IConfiguration configuration)
    {
        _ownerService = ownerService;
        _configuration = configuration;
    }

    [HttpPost("LogIn")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var owner = await _ownerService.GetOwnerWithIdByEmailAndPassword(request.Username, request.Password);

        if (owner == null)
        {
            return Unauthorized("Invalid credentials");
        }

        var token = owner.Id.ToString(); // Using ID as token
                                         // Store token in HTTP context
        HttpContext.Items["authToken"] = token;
        return Ok(new { token });
    }

    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class User
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
