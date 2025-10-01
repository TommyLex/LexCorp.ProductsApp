using LexCorp.LazyLoading.Dto.Options;
using LexCorp.LazyLoading.Filter.Abstractions.Services;
using LexCorp.LazyLoading.Filter.Providers;
using LexCorp.LazyLoading.Filter.Services;
using LexCorp.LazyLoading.Filter.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LexCorp.LazyLoading.Filter.Extensions
{
  /// <summary>
  /// Provides extension methods for registering lazy loading services and options in the dependency injection container.
  /// </summary>
  public static class ServiceCollectionExtension
  {
    /// <summary>
    /// Registers lazy loading services and configures default options in the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <param name="configuration">The application configuration containing the lazy loading options.</param>
    /// <remarks>
    /// This method performs the following actions:
    /// <list type="bullet">
    /// <item>
    /// <description>Configures <see cref="DefaultLazyLoadingOptions"/> using the <c>DefaultLazyLoadingOptions</c> section in <c>appsettings.json</c>.</description>
    /// </item>
    /// <item>
    /// <description>Registers the following services:</description>
    /// <list type="bullet">
    /// <item><see cref="ILazyLoadingDefaultDtoProvider"/>: Provides default lazy loading DTOs.</item>
    /// <item><see cref="ILazyLoadingDtoValidator"/>: Validates lazy loading DTOs.</item>
    /// <item><see cref="ILazyLoadingFilterService"/>: Applies lazy loading filters and sorting.</item>
    /// </list>
    /// </item>
    /// </list>
    /// <para>
    /// <strong>Important:</strong> Ensure that the <c>DefaultLazyLoadingOptions</c> section is properly configured in <c>appsettings.json</c>.
    /// Example configuration:
    /// <code>
    /// {
    ///   "DefaultLazyLoadingOptions": {
    ///     "First": 1,
    ///     "Rows": 10,
    ///     "SortField": "Guid",
    ///     "SortOrder": 1
    ///   }
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public static void AddLazyLoading(this IServiceCollection services, IConfiguration configuration)
    {
      services.Configure<DefaultLazyLoadingOptions>(opt => configuration.GetSection(nameof(DefaultLazyLoadingOptions)).Bind(opt));

      services.AddTransient<ILazyLoadingDefaultDtoProvider, LazyLoadingDefaultDtoProvider>();
      services.AddTransient<ILazyLoadingDtoValidator, LazyLoadingDtoValidator>();
      services.AddTransient<ILazyLoadingFilterService, LazyLoadingFilterService>();
    }
  }
}