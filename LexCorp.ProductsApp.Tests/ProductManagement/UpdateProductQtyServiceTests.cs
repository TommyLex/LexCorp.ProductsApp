using LexCorp.Channels.Queue.Abstractions.Providers;
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
      var queueMock = new Mock<IGenericQueueChannelProvider<ProductUpdateQtyDto>>();

      var service = new UpdateProductQtyService(productRepositoryMock.Object, loggerMock.Object, queueMock.Object);

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
      var queueMock = new Mock<IGenericQueueChannelProvider<ProductUpdateQtyDto>>();

      var service = new UpdateProductQtyService(productRepositoryMock.Object, loggerMock.Object, queueMock.Object);

      var result = await service.UpdateProductQtyAsync(productUpdateDto);

      Assert.False(result.Success);
      Assert.Contains("Product wasn't found.", result.Messages[0]);
    }

    /// <summary>
    /// Tests that <see cref="UpdateProductQtyService.ValidateAndEnqueueAsync"/> enqueues the product update successfully.
    /// </summary>
    [Fact]
    public async Task ValidateAndEnqueueAsync_ShouldEnqueueProductUpdate_WhenValidationPasses()
    {
      var productUpdateDto = new ProductUpdateQtyDto { Guid = Guid.NewGuid(), Quantity = 10 };

      var productRepositoryMock = new Mock<IDProductRepository>();
      productRepositoryMock.Setup(repo => repo.GetAsync(productUpdateDto.Guid)).ReturnsAsync(new ProductDetailDto { Guid = productUpdateDto.Guid });

      var loggerMock = new Mock<ILogger<UpdateProductQtyService>>();
      var queueMock = new Mock<IGenericQueueChannelProvider<ProductUpdateQtyDto>>();

      var service = new UpdateProductQtyService(productRepositoryMock.Object, loggerMock.Object, queueMock.Object);

      var result = await service.ValidateAndEnqueueAsync(productUpdateDto);

      Assert.True(result.Success);
      Assert.Contains("Message enqueued successfully.", result.Messages[0]);
    }

    /// <summary>
    /// Tests that <see cref="UpdateProductQtyService.ValidateAndEnqueueAsync"/> returns an error when the product does not exist.
    /// </summary>
    [Fact]
    public async Task ValidateAndEnqueueAsync_ShouldReturnError_WhenProductDoesNotExist()
    {
      var productUpdateDto = new ProductUpdateQtyDto { Guid = Guid.NewGuid(), Quantity = 10 };

      var productRepositoryMock = new Mock<IDProductRepository>();
      productRepositoryMock.Setup(repo => repo.GetAsync(productUpdateDto.Guid)).ReturnsAsync((ProductDetailDto)null);

      var loggerMock = new Mock<ILogger<UpdateProductQtyService>>();
      var queueMock = new Mock<IGenericQueueChannelProvider<ProductUpdateQtyDto>>();

      var service = new UpdateProductQtyService(productRepositoryMock.Object, loggerMock.Object, queueMock.Object);

      var result = await service.ValidateAndEnqueueAsync(productUpdateDto);

      Assert.False(result.Success);
      Assert.Contains("Product not found.", result.Messages[0]);
    }

    /// <summary>
    /// Tests that <see cref="UpdateProductQtyService.ValidateAndEnqueueAsync"/> returns an error when the quantity is negative.
    /// </summary>
    [Fact]
    public async Task ValidateAndEnqueueAsync_ShouldReturnError_WhenQuantityIsNegative()
    {
      var productUpdateDto = new ProductUpdateQtyDto { Guid = Guid.NewGuid(), Quantity = -5 };

      var productRepositoryMock = new Mock<IDProductRepository>();
      var loggerMock = new Mock<ILogger<UpdateProductQtyService>>();
      var queueMock = new Mock<IGenericQueueChannelProvider<ProductUpdateQtyDto>>();

      var service = new UpdateProductQtyService(productRepositoryMock.Object, loggerMock.Object, queueMock.Object);

      var result = await service.ValidateAndEnqueueAsync(productUpdateDto);

      Assert.False(result.Success);
      Assert.Contains("Quantity cannot be negative.", result.Messages[0]);
    }
  }
}