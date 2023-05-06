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

        byte[] passwordSalt = Encoding.UTF8.GetBytes(GetPasswordHashSalt());

        return user != null && VerifyPassword(password, user.Password, passwordSalt);
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
        var key = Encoding.UTF8.GetBytes(GetPasswordHashSalt());
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Email, email) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
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

    private static bool VerifyPassword(string password, string passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var computedHashString = BitConverter.ToString(computedHash).Replace("-", "").ToLower();

        return passwordHash == computedHashString;
        return !computedHash.Where((t, i) => t != passwordHash[i]).Any();
    }
}