using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.App.Services;
using LexCorp.Product.Dto;
using Microsoft.Extensions.Logging;
using Moq;

namespace LexCorp.ProductsApp.Tests.Product
{
  /// <summary>
  /// Unit tests for the <see cref="GetProductListService"/> class.
  /// </summary>
  public class GetProductListServiceTests
  {
    /// <summary>
    /// Tests that <see cref="GetProductListService.GetProductsListAsync"/> returns a list of products when products exist.
    /// </summary>
    [Fact]
    public async Task GetProductsListAsync_ShouldReturnProductList_WhenProductsExist()
    {
      var products = new List<ProductListDto>
      {
        new ProductListDto { Guid = Guid.NewGuid(), Name = "Product 1" },
        new ProductListDto { Guid = Guid.NewGuid(), Name = "Product 2" }
      };

      var productRepositoryMock = new Mock<IDProductRepository>();
      productRepositoryMock.Setup(repo => repo.ListAllAsync()).ReturnsAsync(products);

      var loggerMock = new Mock<ILogger<GetProductListService>>();

      var service = new GetProductListService(productRepositoryMock.Object, loggerMock.Object);

      var result = await service.GetProductsListAsync();

      Assert.True(result.Success);
      Assert.NotNull(result.Data);
      Assert.Equal(2, result.Data.Count());
    }

    /// <summary>
    /// Tests that <see cref="GetProductListService.GetProductsListAsync"/> returns an empty list when no products exist.
    /// </summary>
    [Fact]
    public async Task GetProductsListAsync_ShouldReturnEmptyList_WhenNoProductsExist()
    {
      var productRepositoryMock = new Mock<IDProductRepository>();
      productRepositoryMock.Setup(repo => repo.ListAllAsync()).ReturnsAsync(new List<ProductListDto>());

      var loggerMock = new Mock<ILogger<GetProductListService>>();

      var service = new GetProductListService(productRepositoryMock.Object, loggerMock.Object);

      var result = await service.GetProductsListAsync();

      Assert.True(result.Success);
      Assert.NotNull(result.Data);
      Assert.Empty(result.Data);
    }

    /// <summary>
    /// Tests that <see cref="GetProductListService.GetProductsListAsync"/> logs and returns an error when an exception occurs.
    /// </summary>
    [Fact]
    public async Task GetProductsListAsync_ShouldLogAndReturnError_WhenExceptionOccurs()
    {
      var productRepositoryMock = new Mock<IDProductRepository>();
      productRepositoryMock.Setup(repo => repo.ListAllAsync()).ThrowsAsync(new Exception("Database error"));

      var loggerMock = new Mock<ILogger<GetProductListService>>();

      var service = new GetProductListService(productRepositoryMock.Object, loggerMock.Object);

      var result = await service.GetProductsListAsync();

      Assert.False(result.Success);
      Assert.Contains("An unexpected error occurred", result.Messages[0]);
    }
  }
}