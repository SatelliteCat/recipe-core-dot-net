using Microsoft.EntityFrameworkCore;
using recipe_core_dotnet.common.Models;

namespace recipe_core_dotnet.common;

public sealed class RecipeDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}