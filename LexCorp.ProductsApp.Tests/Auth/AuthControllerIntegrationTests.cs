using LexCorp.Jwt.Dto;
using LexCorp.Products.Data.Models.Auth;
using LexCorp.Results.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;


namespace LexCorp.ProductsApp.Tests.Auth
{
  /// <summary>
  /// Integration tests for the <see cref="AuthController"/> class.
  /// </summary>
  public class AuthControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
  {
    private readonly HttpClient _Client;
    private readonly WebApplicationFactory<Program> _Factory;
    private readonly string _TestUsername = "userTester";
    private readonly string _TestPassword = "Password123#";
    private readonly string _TestEmail = "userTester@example.com";

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthControllerIntegrationTests"/> class.
    /// </summary>
    /// <param name="factory">The web application factory for creating test clients.</param>
    public AuthControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
      _Factory = factory;
      _Client = factory.CreateClient();
    }

    /// <summary>
    /// Sets up the test environment by registering a test user.
    /// </summary>
    public async Task InitializeAsync()
    {
      var registerRequest = new
      {
        username = _TestUsername,
        password = _TestPassword,
        email = _TestEmail
      };
      var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(registerRequest), Encoding.UTF8, "application/json");

      var response = await _Client.PostAsync("/api/v1/Auth/Register", content);
      response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Cleans up the test environment by deleting the test user.
    /// </summary>
    public async Task DisposeAsync()
    {
      using var scope = _Factory.Services.CreateScope();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

      var user = await userManager.FindByNameAsync(_TestUsername);
      if (user != null)
      {
        var deleteResult = await userManager.DeleteAsync(user);
        if (!deleteResult.Succeeded)
        {
          throw new Exception($"Failed to delete user {_TestUsername}: {string.Join(", ", deleteResult.Errors.Select(e => e.Description))}");
        }
      }
    }

    /// <summary>
    /// Tests that the login endpoint returns a JWT token for valid credentials.
    /// </summary>
    [Fact]
    public async Task Login_ShouldReturnJwtToken_WhenCredentialsAreValid()
    {
      var loginRequest = new
      {
        username = _TestUsername,
        password = _TestPassword
      };
      var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

      var response = await _Client.PostAsync("/api/v1/Auth/Login", content);

      response.EnsureSuccessStatusCode();
      var responseContent = await response.Content.ReadAsStringAsync();
      Assert.Contains("authToken", responseContent);
    }

    /// <summary>
    /// Tests that the logout endpoint returns 200 when a valid token is provided.
    /// </summary>
    [Fact]
    public async Task Logout_ShouldReturnSuccess_WhenTokenIsValid()
    {
      var loginRequest = new
      {
        username = _TestUsername,
        password = _TestPassword
      };
      var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

      var loginResponse = await _Client.PostAsync("/api/v1/Auth/Login", content);
      loginResponse.EnsureSuccessStatusCode();

      var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<ResultInfoDto<JwtTokenDto>>(loginResponseContent);

      _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.AuthToken.ToString());
      var logoutResponse = await _Client.PostAsync("/api/v1/Auth/Logout", null);

      logoutResponse.EnsureSuccessStatusCode();
    }
  }
}