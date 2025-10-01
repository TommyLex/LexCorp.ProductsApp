using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LexCorp.Products.Data.Extensions
{
  /// <summary>
  /// Provides extension methods for registering data-related services in the dependency injection container.
  /// </summary>
  public static class ServiceCollectionExtension
  {
    /// <summary>
    /// Registers the database context and related services in the dependency injection container.
    /// </summary>
    /// <param name="serviceCollection">The service collection to which the services will be added.</param>
    /// <param name="configuration">The application configuration containing the connection string.</param>
    /// <remarks>
    /// This method configures the <see cref="AppDbContext"/> to use SQL Server with lazy loading proxies.
    /// The connection string is retrieved from the "DefaultConnection" key in the configuration.
    /// </remarks>
    public static void AddDatabaseContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
      serviceCollection.AddDbContext<AppDbContext>(options => {
        options.UseLazyLoadingProxies()
          .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
      })
      .BuildServiceProvider();
    }
  }
}