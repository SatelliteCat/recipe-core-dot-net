using System.ComponentModel.DataAnnotations.Schema;

namespace recipe_core_dotnet.common.Models;

[Table("user")]
public class User
{
    [Column("id")]
    public int Id { get; set; }
    [Column("email")]
    public string? Email { get; set; } = null;
    [Column("password")]
    public string Password { get; set; } = null!;
    [Column("uuid")]
    public string Uuid { get; set; } = null!;
    [Column("phone")]
    public string? Phone { get; set; } = null;
}