using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace recipe_core_dotnet.common.Models;

[Table("role")]
public class Role : IdentityRole<int>
{
    [Column("description")] public string? Description { get; set; } = null;
}