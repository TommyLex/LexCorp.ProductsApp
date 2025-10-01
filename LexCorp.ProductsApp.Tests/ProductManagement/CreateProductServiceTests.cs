using LexCorp.Product.App.Services;
using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.Dto;
using Microsoft.Extensions.Logging;
using Moq;

namespace LexCorp.ProductsApp.Tests.Product
{
  /// <summary>
  /// Unit tests for the <see cref="CreateProductService"/> class.
  /// </summary>
  public class CreateProductServiceTests
  {
    /// <summary>
    /// Tests that <see cref="CreateProductService.CreateProductAsync"/> successfully creates a product.
    /// </summary>
    [Fact]
    public async Task CreateProductAsync_ShouldCreateProduct_WhenValidDataProvided()
    { 
      var productCreateDto = new ProductCreateDto { Name = "Test Product", ImageUrl = "https://example.com/image.jpg" };
      var createdProduct = new ProductDetailDto { Guid = Guid.NewGuid(), Name = "Test Product", ImageUrl = "https://example.com/image.jpg" };

      var productRepositoryMock = new Mock<IDProductRepository>();
      productRepositoryMock.Setup(repo => repo.InsertAsync(productCreateDto)).ReturnsAsync(createdProduct);

      var loggerMock = new Mock<ILogger<CreateProductService>>();

      var service = new CreateProductService(productRepositoryMock.Object, loggerMock.Object);

      var result = await service.CreateProductAsync(productCreateDto);

      Assert.True(result.Success);
      Assert.NotNull(result.Data);
      Assert.Equal(productCreateDto.Name, result.Data.Name);
      Assert.Equal(productCreateDto.ImageUrl, result.Data.ImageUrl);
    }

    /// <summary>
    /// Tests that <see cref="CreateProductService.CreateProductAsync"/> returns an error when the ImageUrl is invalid.
    /// </summary>
    [Fact]
    public async Task CreateProductAsync_ShouldReturnError_WhenImageUrlIsInvalid()
    {
      var productCreateDto = new ProductCreateDto { Name = "Test Product", ImageUrl = "invalid-url" };

      var productRepositoryMock = new Mock<IDProductRepository>();
      var loggerMock = new Mock<ILogger<CreateProductService>>();

      var service = new CreateProductService(productRepositoryMock.Object, loggerMock.Object);

      var result = await service.CreateProductAsync(productCreateDto);

      Assert.False(result.Success);
      Assert.Contains("The provided ImageUrl is not a valid URL.", result.Messages[0]);
    }
  }
}