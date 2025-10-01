using LexCorp.Product.App.Abstractions.Services;
using LexCorp.Product.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LexCorp.Product.App.Extensions
{
  /// <summary>
  /// Provides extension methods for registering product-related services in the dependency injection container.
  /// </summary>
  public static class ServiceCollectionExtension
  {
    /// <summary>
    /// Registers product application services in the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <remarks>
    /// This method registers the following services:
    /// <list type="bullet">
    /// <item>
    /// <description><see cref="IGetProductDetailService"/>: Service for retrieving product details.</description>
    /// </item>
    /// <item>
    /// <description><see cref="IGetProductListService"/>: Service for retrieving a list of products.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static void AddProductsApp(this IServiceCollection services)
    {
      services.AddTransient<IGetProductDetailService, GetProductDetailService>();
      services.AddTransient<IGetProductListService, GetProductListService>();
      services.AddTransient<ICreateProductService, CreateProductService>();
      services.AddTransient<IUpdateProductQtyService, UpdateProductQtyService>();
    }
  }
}