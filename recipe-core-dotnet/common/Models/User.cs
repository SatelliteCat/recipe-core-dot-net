﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace recipe_core_dotnet.common.Models;

[Table("user")]
public class User : IdentityUser<int>
{
    [Column("uuid")] public string Uuid { get; set; } = null!;
}