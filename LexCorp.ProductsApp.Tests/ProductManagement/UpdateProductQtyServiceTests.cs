using LexCorp.Product.App.Services;
using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.Dto;
using LexCorp.Product.Dto.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace LexCorp.ProductsApp.Tests.Product
{
  /// <summary>
  /// Unit tests for the <see cref="UpdateProductQtyService"/> class.
  /// </summary>
  public class UpdateProductQtyServiceTests
  {
    /// <summary>
    /// Tests that <see cref="UpdateProductQtyService.UpdateProductQtyAsync"/> successfully updates the product quantity.
    /// </summary>
    [Fact]
    public async Task UpdateProductQtyAsync_ShouldUpdateQuantity_WhenProductExists()
    {
      var productUpdateDto = new ProductUpdateQtyDto { Guid = Guid.NewGuid(), Quantity = 50 };
      var updatedProduct = new ProductDetailDto { Guid = productUpdateDto.Guid, Quantity = 50 };

      var productRepositoryMock = new Mock<IDProductRepository>();
      productRepositoryMock.Setup(repo => repo.UpdateProductQty(productUpdateDto)).ReturnsAsync(updatedProduct);

      var loggerMock = new Mock<ILogger<UpdateProductQtyService>>();

      var service = new UpdateProductQtyService(productRepositoryMock.Object, loggerMock.Object);

      var result = await service.UpdateProductQtyAsync(productUpdateDto);

      Assert.True(result.Success);
      Assert.NotNull(result.Data);
      Assert.Equal(productUpdateDto.Guid, result.Data.Guid);
      Assert.Equal(50, result.Data.Quantity);
    }

    /// <summary>
    /// Tests that <see cref="UpdateProductQtyService.UpdateProductQtyAsync"/> returns an error when the product is not found.
    /// </summary>
    [Fact]
    public async Task UpdateProductQtyAsync_ShouldReturnError_WhenProductNotFound()
    {
      var productUpdateDto = new ProductUpdateQtyDto { Guid = Guid.NewGuid(), Quantity = 50 };

      var productRepositoryMock = new Mock<IDProductRepository>();
      productRepositoryMock.Setup(repo => repo.UpdateProductQty(productUpdateDto))
                           .ThrowsAsync(new ProductNotFoundException("Product not found."));

      var loggerMock = new Mock<ILogger<UpdateProductQtyService>>();

      var service = new UpdateProductQtyService(productRepositoryMock.Object, loggerMock.Object);

      var result = await service.UpdateProductQtyAsync(productUpdateDto);

      Assert.False(result.Success);
      Assert.Contains("Product wasn't found.", result.Messages[0]);
    }
  }
}