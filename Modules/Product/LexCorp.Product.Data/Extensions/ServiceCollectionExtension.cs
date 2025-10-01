using LexCorp.Automapper.Abstractions.Map;
using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.Data.Automapper;
using LexCorp.Product.Data.Repositories;
using LexCorp.Product.Dto;
using LexCorp.Products.Data.Extensions;
using LexCorp.Products.Data.Models.Product;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LexCorp.Product.Data.Extensions
{
  /// <summary>
  /// Provides extension methods for registering product data-related services in the dependency injection container.
  /// </summary>
  public static class ServiceCollectionExtension
  {
    /// <summary>
    /// Registers the product data module services in the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <remarks>
    /// This method performs the following registrations:
    /// <list type="bullet">
    /// <item>
    /// <description>Registers database context services using <see cref="LexCorp.Products.Data.Extensions.ServiceCollectionExtension.AddDatabaseContext(IServiceCollection, IConfiguration)"/>.</description>
    /// </item>
    /// <item>
    /// <description>Registers <see cref="IDProductRepository"/> with its implementation <see cref="DProductRepository"/> as a transient service.</description>
    /// </item>
    /// <item>
    /// <description>Registers <see cref="IToDto{TSource, TDestination}"/> for mapping <see cref="DProduct"/> to <see cref="ProductDetailDto"/> using <see cref="DProductDetailMap"/>.</description>
    /// </item>
    /// <item>
    /// <description>Registers <see cref="IFromDto{TSource, TDestination}"/> for mapping <see cref="ProductDetailDto"/> to <see cref="DProduct"/> using <see cref="DProductDetailMap"/>.</description>
    /// </item>
    /// <item>
    /// <description>Registers <see cref="IToDto{TSource, TDestination}"/> for mapping <see cref="DProduct"/> to <see cref="ProductListDto"/> using <see cref="DProductListMap"/>.</description>
    /// </item>
    /// <item>
    /// <description>Registers <see cref="IFromDto{TSource, TDestination}"/> for mapping <see cref="ProductCreateDto"/> to <see cref="DProduct"/> using <see cref="DProductCreateMap"/>.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static void AddProductsData(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDatabaseContext(configuration);
      services.AddTransient<IDProductRepository, DProductRepository>();
      services.AddTransient<IToDto<DProduct, ProductDetailDto>, DProductDetailMap>();
      services.AddTransient<IFromDto<DProduct, ProductDetailDto>, DProductDetailMap>();
      services.AddTransient<IToDto<DProduct, ProductListDto>, DProductListMap>();
      services.AddTransient<IFromDto<DProduct, ProductCreateDto>, DProductCreateMap>();
    }
  }
}