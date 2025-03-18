using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;


namespace WebApplication1;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public AuthController(IConfiguration config)
    {
        _config = config;
        _connectionString =
            "Data Source=DESKTOP-PSDS24G\\SQLEXPRESS;Initial Catalog=Done&Dusted;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        Console.WriteLine($"Login Attempt: Username: {request.UserName}, Password: {request.Password}");
        
        var sql = "SELECT * FROM Users WHERE LOWER(userName) = LOWER(@Username)";
        using var conn = new SqlConnection(_connectionString);
        var user = await conn.QueryFirstOrDefaultAsync<Users>(sql, new { request.UserName });

        if (user == null)
        {
            Console.WriteLine("User Not Found");
        }
        
        Console.WriteLine($"User found: {user.userName}, PasswordHash: {user.passwordHash}");

        if (!VerifyPassword(request.Password, user.passwordHash))
        {
            Console.WriteLine("Password verification failed");
            return Unauthorized(new {messsage ="Username or password is incorrect"});
        }
        
        var token = GenerateJwtToken(user);
        Console.WriteLine($"Token generated: {token}");
        return Ok(new { token });
    }

    private string GenerateJwtToken(Users user)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        Console.WriteLine($"SecretKey: {jwtSettings["SecretKey"]}");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.userName),
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString())
        };
        
        var token = new JwtSecurityToken(
            jwtSettings["Issuer"],
            jwtSettings["Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
            signingCredentials: creds);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        if (string.IsNullOrEmpty(passwordHash))
        {
            return false;
        }
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}