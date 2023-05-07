using Microsoft.EntityFrameworkCore;
using recipe_core_dotnet.common.Models;

namespace recipe_core_dotnet.common;

public sealed class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}