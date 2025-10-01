using LexCorp.Products.Data.Seeder.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LexCorp.Products.Data.Seeder.Extensions
{
  /// <summary>
  /// Provides extension methods for registering database seeder services in the dependency injection container.
  /// </summary>
  public static class ServiceCollectionExtension
  {
    /// <summary>
    /// Registers the database seeder services in the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <remarks>
    /// This method registers the following services:
    /// <list type="bullet">
    /// <item>
    /// <description><see cref="RoleSeederService"/>: Seeds roles into the database if they do not exist.</description>
    /// </item>
    /// <item>
    /// <description><see cref="AdminUserSeederService"/>: Seeds an admin user into the database if no users exist.</description>
    /// </item>
    /// <item>
    /// <description><see cref="ProductSeederService"/>: Seeds products into the database from a CSV file if no products exist.</description>
    /// </item>
    /// <item>
    /// <description><see cref="DatabaseSeederService"/>: Orchestrates the seeding process by invoking the individual seeders.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static void AddDataSeeder(this IServiceCollection services)
    {
      services.AddTransient<RoleSeederService, RoleSeederService>();
      services.AddTransient<AdminUserSeederService, AdminUserSeederService>();
      services.AddTransient<ProductSeederService, ProductSeederService>();
      services.AddTransient<DatabaseSeederService, DatabaseSeederService>();
    }
  }
}