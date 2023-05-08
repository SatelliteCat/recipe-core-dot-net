using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using recipe_core_dotnet.common.Models;

namespace recipe_core_dotnet.common;

public sealed class AppDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole,
    IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.ToTable("user");
            entity.Property(user => user.Id).HasColumnName("id").HasColumnType("serial");
            entity.Property(user => user.Email).HasColumnName("email");
            entity.Property(user => user.EmailConfirmed).HasColumnName("email_confirmed");
            entity.Property(user => user.NormalizedEmail).HasColumnName("normalized_email");
            entity.Property(user => user.UserName).HasColumnName("user_name").IsRequired(false);
            entity.Property(user => user.NormalizedUserName).HasColumnName("normalized_user_name").IsRequired(false);
            entity.Property(user => user.PasswordHash).HasColumnName("password");
            entity.Property(user => user.PhoneNumber).HasColumnName("phone");
            entity.Property(user => user.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
            entity.Property(user => user.AccessFailedCount).HasColumnName("access_failed_count");
            entity.Property(user => user.LockoutEnabled).HasColumnName("lockout_enabled");
            entity.Property(user => user.LockoutEnd).HasColumnName("lockout_end");
            entity.Property(user => user.TwoFactorEnabled).HasColumnName("two_factor_enabled");
            entity.Property(user => user.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            entity.Property(user => user.SecurityStamp).HasColumnName("security_stamp");
        });

        builder.Entity<Role>(entity =>
        {
            entity.ToTable("role");
            entity.Property(role => role.Id).HasColumnName("id");
            entity.Property(role => role.Name).HasColumnName("name");
            entity.Property(role => role.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            entity.Property(role => role.NormalizedName).HasColumnName("normalized_name");
        });

        builder.Entity<UserRole>(entity =>
        {
            entity.ToTable("user_role");
            entity.Property(role => role.UserId).HasColumnName("user_id");
            entity.Property(role => role.RoleId).HasColumnName("role_id");
        });
    }
}