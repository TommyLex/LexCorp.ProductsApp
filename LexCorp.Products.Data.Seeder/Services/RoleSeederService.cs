using LexCorp.Products.Auth.Dto.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LexCorp.Products.Data.Seeder.Services
{
  /// <summary>
  /// Seeds the database with roles if none exist.
  /// </summary>
  /// <param name="_RoleManager">The role manager for managing roles.</param>
  /// <param name="_Logger">The logger for logging information.</param>
  public class RoleSeederService(RoleManager<IdentityRole<Guid>> _RoleManager, ILogger<RoleSeederService> _Logger)
  {
    /// <summary>
    /// Seeds the database with roles if none exist.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SeedAsync()
    {
      try
      {
        _Logger.LogInformation("Checking if roles need to be seeded...");

        foreach (var role in Enum.GetValues(typeof(RoleType)).Cast<RoleType>())
        {
          if (!await _RoleManager.RoleExistsAsync(role.ToString()))
          {
            var result = await _RoleManager.CreateAsync(new IdentityRole<Guid> { Name = role.ToString() });
            if (result.Succeeded)
            {
              _Logger.LogInformation($"Role '{role}' created successfully.");
            }
            else
            {
              _Logger.LogError($"Failed to create role '{role}':");
              foreach (var error in result.Errors)
              {
                _Logger.LogError($"- {error.Description}");
              }
            }
          }
        }
      }
      catch(Exception ex)
      {
        _Logger.LogError(ex, "Unexpected error while seeding roles"); 
      }
    }
  }
}