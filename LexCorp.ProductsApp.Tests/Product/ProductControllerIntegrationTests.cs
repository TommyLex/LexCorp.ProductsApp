using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.Dto;
using LexCorp.Products.Data.Models.Auth;
using LexCorp.Results.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LexCorp.ProductsApp.Tests.Product
{
  /// <summary>
  /// Integration tests for the <see cref="ProductController"/> class.
  /// </summary>
  public class ProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
  {
    private readonly HttpClient _Client;
    private readonly WebApplicationFactory<Program> _Factory;
    private ProductDetailDto _TestProductDto;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductControllerIntegrationTests"/> class.
    /// </summary>
    /// <param name="factory">The web application factory for creating test clients.</param>
    public ProductControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
      _Factory = factory;
      _Client = factory.CreateClient();
    }

    /// <summary>
    /// Sets up the test environment by adding a test product.
    /// </summary>
    public async Task InitializeAsync()
    {
      using var scope = _Factory.Services.CreateScope();
      var dProductRepository = scope.ServiceProvider.GetRequiredService<IDProductRepository>();

      var product = new ProductDetailDto()
      {
        Guid = Guid.NewGuid(),
        ImageUrl = "http://example.com/image.jpg",
        Name = "Test Product",
        Description = "Test Description",
        Price = (decimal)99.99,
        Quantity = 10
      };

      product = await dProductRepository.InsertAsync(product);

      _TestProductDto = product;
    }

    /// <summary>
    /// Cleans up the test environment by deleting the test product.
    /// </summary>
    public async Task DisposeAsync()
    {
      using var scope = _Factory.Services.CreateScope();
      var dProductRepository = scope.ServiceProvider.GetRequiredService<IDProductRepository>();
      await dProductRepository.DeleteAsync(_TestProductDto);
    }

    /// <summary>
    /// Tests that the GetProductsList endpoint returns a list of products.
    /// </summary>
    [Fact]
    public async Task GetProductsList_ShouldReturnProductList_WhenProductsExist()
    {
      var response = await _Client.GetAsync("/api/v1/Product/GetProductsList");
      response.EnsureSuccessStatusCode();

      var responseContent = await response.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<ResultInfoDto<IEnumerable<ProductListDto>>>(responseContent);

      Assert.True(result.Success);
      Assert.NotNull(result.Data);
      Assert.NotEmpty(result.Data);
    }

    /// <summary>
    /// Tests that the GetProductDetail endpoint returns product details for a valid product ID.
    /// </summary>
    [Fact]
    public async Task GetProductDetail_ShouldReturnProductDetails_WhenProductExists()
    {
      var response = await _Client.GetAsync($"/api/v1/Product/GetProductDetail/{_TestProductDto.Guid}");
      response.EnsureSuccessStatusCode();

      var responseContent = await response.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<ResultInfoDto<ProductDetailDto>>(responseContent);

      Assert.True(result.Success);
      Assert.NotNull(result.Data);
      Assert.Equal(_TestProductDto.Guid, result.Data.Guid);
    }

    /// <summary>
    /// Tests that the GetProductDetail endpoint returns an error for a non-existent product ID.
    /// </summary>
    [Fact]
    public async Task GetProductDetail_ShouldReturnError_WhenProductDoesNotExist()
    {
      var nonExistentProductId = Guid.NewGuid();
      var response = await _Client.GetAsync($"/api/v1/Product/GetProductDetail/{nonExistentProductId}");

      Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

      var responseContent = await response.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<ResultInfoDto<ProductDetailDto>>(responseContent);

      Assert.False(result.Success);
      Assert.Contains("Product not found", result.Messages[0]);
    }
  }
}