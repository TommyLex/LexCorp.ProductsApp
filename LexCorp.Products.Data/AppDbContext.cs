using LexCorp.Products.Data.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace LexCorp.Products.Data
{
  /// <summary>
  /// Application database context for managing users, roles, and other entities.
  /// </summary>
  public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
  {
    /// <summary>
    /// Constructor for AppDbContext.
    /// </summary>
    /// <param name="options">Set up options for db context.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
  : base(options)
    { }

    /// <summary>
    /// Configures the model for the database context.
    /// </summary>
    /// <remarks>This method sets the default schema for the database to "auth" and applies all entity
    /// configurations  from the current assembly. It is called by the Entity Framework runtime during model
    /// creation.</remarks>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure the entity framework model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.HasDefaultSchema("auth");
      modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
  }
}
