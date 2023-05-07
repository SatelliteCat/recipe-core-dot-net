using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace recipe_core_dotnet.ApiV1.Dto;

public class AuthLoginDto
{
    [Required, EmailAddress] public string Email { get; set; } = null!;
    [Required, PasswordPropertyText] public string Password { get; set; } = null!;
}