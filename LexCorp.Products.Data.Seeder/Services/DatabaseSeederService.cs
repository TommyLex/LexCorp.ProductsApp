using Microsoft.Extensions.Logging;

namespace LexCorp.Products.Data.Seeder.Services
{
  /// <summary>
  /// Orchestrates the database seeding process.
  /// </summary>
  /// <param name="_ProductSeeder">The product seeder.</param>
  /// <param name="_AdminUserSeeder">The admin user seeder.</param>
  /// <param name="_RoleSeeder">The role seeder.</param>
  /// <param name="_Logger">The logger for logging information.</param>
  public class DatabaseSeederService(ProductSeederService _ProductSeeder, AdminUserSeederService _AdminUserSeeder, RoleSeederService _RoleSeeder, ILogger<DatabaseSeederService> _Logger)
  {
    /// <summary>
    /// Seeds the database with initial data.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SeedDatabaseAsync()
    {
      _Logger.LogInformation("Starting database seeding...");
      await _RoleSeeder.SeedAsync();
      await _AdminUserSeeder.SeedAsync();
      await _ProductSeeder.SeedAsync();
      _Logger.LogInformation("Database seeding completed.");
    }
  }
}