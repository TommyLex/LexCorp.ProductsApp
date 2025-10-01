using Microsoft.Extensions.DependencyInjection;

namespace LexCorp.Product.ProductQtyQueueService.Extensions
{
  /// <summary>
  /// Provides extension methods for registering services related to product quantity queue processing.
  /// </summary>
  public static class ServiceCollectionExtension
  {
    /// <summary>
    /// Registers the product quantity queue processing service as a hosted service in the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    public static void AddProductsQtyQueueService(this IServiceCollection services)
    {
      services.AddHostedService<ProductQtyProcessorService>();
    }
  }
}