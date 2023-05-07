using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace recipe_core_dotnet.common.Services.auth;

public class AuthService : IAuthService
{
    private readonly RecipeDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthService(RecipeDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<bool> IsValidUserAsync(string email, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        return user != null && VerifyPassword(password, user.Password);
    }

    public string HashPassword(string password)
    {
        var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(GetPasswordHashSalt()));

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var hash = BitConverter.ToString(computedHash).Replace("-", "").ToLower();

        return hash;
    }

    public string CreateToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetPasswordHashSalt()));
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, email)
        };

        var tokenDescriptor = new JwtSecurityToken(
            issuer: GetJwtIssuer(),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
        );

        return tokenHandler.WriteToken(tokenDescriptor);
    }

    public string GetPasswordHashSalt()
    {
        string? passwordHashSalt = _configuration.GetValue<string>("PasswordHashSalt");

        if (string.IsNullOrEmpty(passwordHashSalt))
        {
            throw new Exception("Not found password hash salt.");
        }

        return passwordHashSalt!;
    }

    public string GetJwtIssuer()
    {
        string? issuer = _configuration.GetValue<string>("JwtTokenIssuer");

        if (string.IsNullOrEmpty(issuer))
        {
            throw new Exception("Not found JWT issuer.");
        }

        return issuer!;
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        Console.WriteLine(passwordHash);
        Console.WriteLine(HashPassword(password));
        
        return passwordHash == HashPassword(password);
    }
}