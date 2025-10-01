using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.App.Services;
using LexCorp.Product.Dto;
using Microsoft.Extensions.Logging;
using Moq;

namespace LexCorp.ProductsApp.Tests.Product
{
  /// <summary>
  /// Unit tests for the <see cref="GetProductDetailService"/> class.
  /// </summary>
  public class GetProductDetailServiceTests
  {
    /// <summary>
    /// Tests that <see cref="GetProductDetailService.GetProductDetailAsync"/> returns product details when the product exists.
    /// </summary>
    [Fact]
    public async Task GetProductDetailAsync_ShouldReturnProductDetails_WhenProductExists()
    {
      var productId = Guid.NewGuid();
      var productDetail = new ProductDetailDto { Guid = productId, Name = "Test Product" };

      var productRepositoryMock = new Mock<IDProductRepository>();
      productRepositoryMock.Setup(repo => repo.GetAsync(productId)).ReturnsAsync(productDetail);

      var loggerMock = new Mock<ILogger<GetProductDetailService>>();

      var service = new GetProductDetailService(productRepositoryMock.Object, loggerMock.Object);

      var result = await service.GetProductDetailAsync(productId);

      Assert.True(result.Success);
      Assert.NotNull(result.Data);
      Assert.Equal(productId, result.Data.Guid);
      Assert.Equal("Test Product", result.Data.Name);
    }

    /// <summary>
    /// Tests that <see cref="GetProductDetailService.GetProductDetailAsync"/> returns an error when the product does not exist.
    /// </summary>
    [Fact]
    public async Task GetProductDetailAsync_ShouldReturnError_WhenProductDoesNotExist()
    {
      var productId = Guid.NewGuid();

      var productRepositoryMock = new Mock<IDProductRepository>();
      productRepositoryMock.Setup(repo => repo.GetAsync(productId)).ReturnsAsync((ProductDetailDto)null);

      var loggerMock = new Mock<ILogger<GetProductDetailService>>();

      var service = new GetProductDetailService(productRepositoryMock.Object, loggerMock.Object);

      var result = await service.GetProductDetailAsync(productId);

      Assert.False(result.Success);
      Assert.Contains("Product not found", result.Messages[0]);
    }

    /// <summary>
    /// Tests that <see cref="GetProductDetailService.GetProductDetailAsync"/> logs and returns an error when an exception occurs.
    /// </summary>
    [Fact]
    public async Task GetProductDetailAsync_ShouldLogAndReturnError_WhenExceptionOccurs()
    {
      var productId = Guid.NewGuid();

      var productRepositoryMock = new Mock<IDProductRepository>();
      productRepositoryMock.Setup(repo => repo.GetAsync(productId)).ThrowsAsync(new Exception("Database error"));

      var loggerMock = new Mock<ILogger<GetProductDetailService>>();

      var service = new GetProductDetailService(productRepositoryMock.Object, loggerMock.Object);

      var result = await service.GetProductDetailAsync(productId);

      Assert.False(result.Success);
      Assert.Contains("An unexpected error occurred", result.Messages[0]);
    }
  }
}