using LexCorp.Products.Auth.Dto.Roles;
using LexCorp.Products.Data.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LexCorp.Products.Data.Seeder.Services
{
  /// <summary>
  /// Seeds the database with an admin user if no users exist.
  /// </summary>
  /// <param name="_UserManager">The user manager for managing users.</param>
  /// <param name="_Logger">The logger for logging information.</param>
  public class AdminUserSeederService(UserManager<User> _UserManager, ILogger<AdminUserSeederService> _Logger)
  {
    /// <summary>
    /// Seeds the database with an admin user if no users exist.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SeedAsync()
    {
      try
      {
        _Logger.LogInformation("Checking if admin user needs to be seeded...");

        if (_UserManager.Users.Any())
        {
          _Logger.LogInformation("Users already exist. Skipping admin user seeding.");
          return;
        }

        var adminUser = new User
        {
          UserName = "admin",
          Email = "admin@example.com",
          EmailConfirmed = true
        };

        var result = await _UserManager.CreateAsync(adminUser, "Admin@123");
        if (result.Succeeded)
        {
          await _UserManager.AddToRoleAsync(adminUser, RoleType.Admin.ToString());
          _Logger.LogInformation("Admin user created successfully.");
        }
        else
        {
          _Logger.LogError("Failed to create admin user:");
          foreach (var error in result.Errors)
          {
            _Logger.LogError($"- {error.Description}");
          }
        }
      }
      catch(Exception ex)
      {
        _Logger.LogError(ex, "Unexpected error while seeding admin user");  
      }
    }
  }
}