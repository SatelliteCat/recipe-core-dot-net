namespace recipe_core_dotnet.ApiV1.Dto;

public class UserRegisterDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}