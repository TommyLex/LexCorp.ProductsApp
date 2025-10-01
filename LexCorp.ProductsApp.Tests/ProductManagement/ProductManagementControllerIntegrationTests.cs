using LexCorp.Jwt.Dto;
using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.Dto;
using LexCorp.Results.Dto;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace LexCorp.ProductsApp.Tests.ProductManagement
{
  /// <summary>
  /// Integration tests for the <see cref="ProductManagementController"/> class.
  /// </summary>
  public class ProductManagementControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
  {
    private readonly HttpClient _Client;
    private readonly WebApplicationFactory<Program> _Factory;
    private ProductDetailDto _TestProductDto;
    private ProductDetailDto _TestProductDtoCreated;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductManagementControllerIntegrationTests"/> class.
    /// </summary>
    /// <param name="factory">The web application factory for creating test clients.</param>
    public ProductManagementControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
      _Factory = factory;
      _Client = factory.CreateClient();
    }

    /// <summary>
    /// Sets up the test environment by adding a test product.
    /// </summary>
    public async Task InitializeAsync()
    {
      var loginRequest = new
      {
        username = "Admin",
        password = "Admin@123"
      };

      var loginContent = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
      var loginResponse = await _Client.PostAsync("/api/v1/Auth/Login", loginContent);
      loginResponse.EnsureSuccessStatusCode();

      var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
      var loginResult = JsonConvert.DeserializeObject<ResultInfoDto<JwtTokenDto>>(loginResponseContent);

      _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Data.AuthToken);

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
      if(_TestProductDtoCreated != null)
      {
        await dProductRepository.DeleteAsync(_TestProductDtoCreated);
      }
    }

    /// <summary>
    /// Tests that the CreateProduct endpoint successfully creates a product.
    /// </summary>
    [Fact]
    public async Task CreateProduct_ShouldCreateProduct_WhenValidDataProvided()
    {
      var product = new ProductCreateDto
      {
        Name = "New Product",
        ImageUrl = "http://example.com/new-image.jpg"
      };

      var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
      var response = await _Client.PostAsync("/api/v1/ProductManagement/CreateProduct", content);
      response.EnsureSuccessStatusCode();

      var responseContent = await response.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<ResultInfoDto<ProductDetailDto>>(responseContent);

      _TestProductDtoCreated = result.Data;

      Assert.True(result.Success);
      Assert.NotNull(result.Data);
      Assert.Equal(product.Name, result.Data.Name);
      Assert.Equal(product.ImageUrl, result.Data.ImageUrl);
    }

    /// <summary>
    /// Tests that the CreateProduct endpoint returns an error when the ImageUrl is invalid.
    /// </summary>
    [Fact]
    public async Task CreateProduct_ShouldReturnError_WhenImageUrlIsInvalid()
    {
      var product = new ProductCreateDto
      {
        Name = "Invalid Product",
        ImageUrl = "invalid-url"
      };

      var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
      var response = await _Client.PostAsync("/api/v1/ProductManagement/CreateProduct", content);

      Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

      var responseContent = await response.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<ResultInfoDto<ProductDetailDto>>(responseContent);

      Assert.False(result.Success);
      Assert.Contains("The provided ImageUrl is not a valid URL.", result.Messages[0]);
    }

    /// <summary>
    /// Tests that the UpdateProductQty endpoint successfully updates the product quantity.
    /// </summary>
    [Fact]
    public async Task UpdateProductQty_ShouldUpdateQuantity_WhenProductExists()
    {
      var updateDto = new ProductUpdateQtyDto
      {
        Guid = _TestProductDto.Guid,
        Quantity = 20
      };

      var content = new StringContent(JsonConvert.SerializeObject(updateDto), Encoding.UTF8, "application/json");
      var response = await _Client.PutAsync("/api/v1/ProductManagement/UpdateProductQty", content);
      response.EnsureSuccessStatusCode();

      var responseContent = await response.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<ResultInfoDto>(responseContent);

      Assert.True(result.Success);
    }

    /// <summary>
    /// Tests that the UpdateProductQty endpoint returns an error when the product is not found.
    /// </summary>
    [Fact]
    public async Task UpdateProductQty_ShouldReturnError_WhenProductNotFound()
    {
      var updateDto = new ProductUpdateQtyDto
      {
        Guid = Guid.NewGuid(),
        Quantity = 20
      };

      var content = new StringContent(JsonConvert.SerializeObject(updateDto), Encoding.UTF8, "application/json");
      var response = await _Client.PutAsync("/api/v1/ProductManagement/UpdateProductQty", content);

      Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

      var responseContent = await response.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<ResultInfoDto>(responseContent);

      Assert.False(result.Success);
      Assert.Contains("Product wasn't found.", result.Messages[0]);
    }

    /// <summary>
    /// Tests that the CreateProduct endpoint returns 401 Unauthorized when no Bearer token is provided.
    /// </summary>
    [Fact]
    public async Task CreateProduct_ShouldReturnUnauthorized_WhenNoBearerTokenProvided()
    {
      var product = new ProductCreateDto
      {
        Name = "Unauthorized Product",
        ImageUrl = "http://example.com/unauthorized-image.jpg"
      };

      var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

      _Client.DefaultRequestHeaders.Authorization = null;

      var response = await _Client.PostAsync("/api/v1/ProductManagement/CreateProduct", content);

      Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    /// <summary>
    /// Tests that the UpdateProductQty endpoint returns 401 Unauthorized when no Bearer token is provided.
    /// </summary>
    [Fact]
    public async Task UpdateProductQty_ShouldReturnUnauthorized_WhenNoBearerTokenProvided()
    {
      var updateDto = new ProductUpdateQtyDto
      {
        Guid = _TestProductDto.Guid,
        Quantity = 20
      };

      var content = new StringContent(JsonConvert.SerializeObject(updateDto), Encoding.UTF8, "application/json");

      _Client.DefaultRequestHeaders.Authorization = null;

      var response = await _Client.PutAsync("/api/v1/ProductManagement/UpdateProductQty", content);

      Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
  }
}