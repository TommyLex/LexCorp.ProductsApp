using LexCorp.LazyLoading.Dto;
using LexCorp.LazyLoading.Filter.Providers;
using LexCorp.LazyLoading.Filter.Validators;
using LexCorp.Product.App.Services;
using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.Dto;
using Microsoft.Extensions.Logging;
using Moq;

namespace LexCorp.ProductsApp.Tests.Product
{
  /// <summary>
  /// Unit tests for the <see cref="GetProductsLazyListService"/> class.
  /// </summary>
  public class GetProductsLazyListServiceTests
  {
    /// <summary>
    /// Tests that <see cref="GetProductsLazyListService.GetProductsLazyListAsync"/> returns a successful result
    /// when a valid lazy loading DTO is provided.
    /// </summary>
    [Fact]
    public async Task GetProductsLazyListAsync_ShouldReturnSuccess_WhenDtoIsValid()
    {
      var lazyLoadingDto = new LazyLoadingDto { First = 1, Rows = 10 };
      var products = new LazyLoadingResultDto<ProductListDto[]>
      {
        Success = true,
        Total = 2,
        Data = new[]
        {
          new ProductListDto { Guid = Guid.NewGuid(), Name = "Product 1" },
          new ProductListDto { Guid = Guid.NewGuid(), Name = "Product 2" }
        }
      };

      var validatorMock = new Mock<ILazyLoadingDtoValidator>();
      validatorMock.Setup(v => v.IsValid(lazyLoadingDto)).Returns(true);

      var providerMock = new Mock<ILazyLoadingDefaultDtoProvider>();

      var repositoryMock = new Mock<IDProductRepository>();
      repositoryMock.Setup(r => r.LazyListAsync(lazyLoadingDto)).ReturnsAsync(products);

      var loggerMock = new Mock<ILogger<GetProductsLazyListService>>();

      var service = new GetProductsLazyListService(validatorMock.Object, providerMock.Object, repositoryMock.Object, loggerMock.Object);

      var result = await service.GetProductsLazyListAsync(lazyLoadingDto);

      Assert.True(result.Success);
      Assert.NotNull(result.Data);
      Assert.Equal(2, result.Data.Length);
    }

    /// <summary>
    /// Tests that <see cref="GetProductsLazyListService.GetProductsLazyListAsync"/> returns an error
    /// when an invalid lazy loading DTO is provided.
    /// </summary>
    [Fact]
    public async Task GetProductsLazyListAsync_ShouldReturnError_WhenDtoIsInvalid()
    {
      var lazyLoadingDto = new LazyLoadingDto { First = null, Rows = null };

      var validatorMock = new Mock<ILazyLoadingDtoValidator>();
      validatorMock.Setup(v => v.IsValid(lazyLoadingDto)).Returns(false);

      var providerMock = new Mock<ILazyLoadingDefaultDtoProvider>();

      var repositoryMock = new Mock<IDProductRepository>();

      var loggerMock = new Mock<ILogger<GetProductsLazyListService>>();

      var service = new GetProductsLazyListService(validatorMock.Object, providerMock.Object, repositoryMock.Object, loggerMock.Object);

      var result = await service.GetProductsLazyListAsync(lazyLoadingDto);

      Assert.False(result.Success);
      Assert.Contains("Parameters for lazy loading are not valid", result.Messages[0]);
    }

    /// <summary>
    /// Tests that <see cref="GetProductsLazyListService.GetProductsLazyListAsync"/> uses the default lazy loading DTO
    /// when the provided DTO is null.
    /// </summary>
    [Fact]
    public async Task GetProductsLazyListAsync_ShouldUseDefaultDto_WhenDtoIsNull()
    {
      var defaultDto = new LazyLoadingDto { First = 1, Rows = 10 };
      var products = new LazyLoadingResultDto<ProductListDto[]>
      {
        Success = true,
        Total = 1,
        Data = new[] { new ProductListDto { Guid = Guid.NewGuid(), Name = "Default Product" } }
      };

      var validatorMock = new Mock<ILazyLoadingDtoValidator>();

      var providerMock = new Mock<ILazyLoadingDefaultDtoProvider>();
      providerMock.Setup(p => p.GetDefaultLazyLoadingDto()).Returns(defaultDto);

      var repositoryMock = new Mock<IDProductRepository>();
      repositoryMock.Setup(r => r.LazyListAsync(defaultDto)).ReturnsAsync(products);

      var loggerMock = new Mock<ILogger<GetProductsLazyListService>>();

      var service = new GetProductsLazyListService(validatorMock.Object, providerMock.Object, repositoryMock.Object, loggerMock.Object);

      var result = await service.GetProductsLazyListAsync(null);

      Assert.True(result.Success);
      Assert.NotNull(result.Data);
      Assert.Single(result.Data);
    }

    /// <summary>
    /// Tests that <see cref="GetProductsLazyListService.GetProductsLazyListAsync"/> logs an error
    /// and returns a failure result when an exception occurs in the repository.
    /// </summary>
    [Fact]
    public async Task GetProductsLazyListAsync_ShouldLogAndReturnError_WhenExceptionOccurs()
    {
      var lazyLoadingDto = new LazyLoadingDto { First = 1, Rows = 10 };

      var validatorMock = new Mock<ILazyLoadingDtoValidator>();
      validatorMock.Setup(v => v.IsValid(lazyLoadingDto)).Returns(true);

      var providerMock = new Mock<ILazyLoadingDefaultDtoProvider>();

      var repositoryMock = new Mock<IDProductRepository>();
      repositoryMock.Setup(r => r.LazyListAsync(lazyLoadingDto)).ThrowsAsync(new Exception("Database error"));

      var loggerMock = new Mock<ILogger<GetProductsLazyListService>>();

      var service = new GetProductsLazyListService(validatorMock.Object, providerMock.Object, repositoryMock.Object, loggerMock.Object);

      var result = await service.GetProductsLazyListAsync(lazyLoadingDto);

      Assert.False(result.Success);
      Assert.Contains("An error occurred while retrieving the lazy list of products", result.Messages[0]);
    }
  }
}