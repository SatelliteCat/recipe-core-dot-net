namespace recipe_core_dotnet.ApiV1.Dto;

public class AuthLoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}