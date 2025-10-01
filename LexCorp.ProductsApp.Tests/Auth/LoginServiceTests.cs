using LexCorp.Auth.Jwt.Abstractions.Providers;
using LexCorp.Jwt.Dto;
using LexCorp.Products.Auth.App.Services;
using LexCorp.Products.Data.Models.Auth;
using LexCorp.Results.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace LexCorp.ProductsApp.Tests.Auth
{
  /// <summary>
  /// Unit tests for the <see cref="LoginService"/> class.
  /// </summary>
  public class LoginServiceTests
  {
    /// <summary>
    /// Tests that <see cref="LoginService.LoginAsync"/> returns success when the credentials are valid.
    /// </summary>
    [Fact]
    public async Task LoginAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
      var userManagerMock = new Mock<UserManager<User>>(
          Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
      var jwtResultProviderMock = new Mock<IJwtResultProvider<User, Guid>>();
      var loggerMock = new Mock<ILogger<LoginService>>();

      var user = new User { Id = Guid.NewGuid(), UserName = "testuser" };
      var jwtToken = new JwtTokenDto { AuthToken = "test-token", ExpiresIn = 3600 };

      userManagerMock.Setup(x => x.FindByNameAsync("testuser")).ReturnsAsync(user);
      userManagerMock.Setup(x => x.CheckPasswordAsync(user, "password")).ReturnsAsync(true);
      jwtResultProviderMock.Setup(x => x.GetJwtToken(user)).ReturnsAsync(new ResultInfoDto<JwtTokenDto>(jwtToken));

      var loginService = new LoginService(userManagerMock.Object, jwtResultProviderMock.Object, loggerMock.Object);

      var result = await loginService.LoginAsync("testuser", "password");

      Assert.True(result.Success);
      Assert.Equal("test-token", result.Data.AuthToken);
    }

    /// <summary>
    /// Tests that <see cref="LoginService.LoginAsync"/> returns an error when the user does not exist.
    /// </summary>
    [Fact]
    public async Task LoginAsync_ShouldReturnError_WhenUserDoesNotExist()
    {
      var userManagerMock = new Mock<UserManager<User>>(
          Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
      var jwtResultProviderMock = new Mock<IJwtResultProvider<User, Guid>>();
      var loggerMock = new Mock<ILogger<LoginService>>();

      userManagerMock.Setup(x => x.FindByNameAsync("testuser")).ReturnsAsync((User)null);

      var loginService = new LoginService(userManagerMock.Object, jwtResultProviderMock.Object, loggerMock.Object);

      var result = await loginService.LoginAsync("testuser", "password");

      Assert.False(result.Success);
      Assert.Contains("Invalid username or password.", result.Messages[0]);
    }

    /// <summary>
    /// Tests that <see cref="LoginService.LoginAsync"/> returns an error when the password is incorrect.
    /// </summary>
    [Fact]
    public async Task LoginAsync_ShouldReturnError_WhenPasswordIsIncorrect()
    {
      var userManagerMock = new Mock<UserManager<User>>(
          Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
      var jwtResultProviderMock = new Mock<IJwtResultProvider<User, Guid>>();
      var loggerMock = new Mock<ILogger<LoginService>>();

      var user = new User { Id = Guid.NewGuid(), UserName = "testuser" };

      userManagerMock.Setup(x => x.FindByNameAsync("testuser")).ReturnsAsync(user);
      userManagerMock.Setup(x => x.CheckPasswordAsync(user, "password")).ReturnsAsync(false);

      var loginService = new LoginService(userManagerMock.Object, jwtResultProviderMock.Object, loggerMock.Object);

      var result = await loginService.LoginAsync("testuser", "password");

      Assert.False(result.Success);
      Assert.Contains("Invalid username or password.", result.Messages[0]);
    }
  }
}