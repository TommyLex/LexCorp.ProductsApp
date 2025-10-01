using LexCorp.Products.Auth.App.Services;
using LexCorp.Products.Data.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace LexCorp.ProductsApp.Tests.Auth
{
  /// <summary>
  /// Unit tests for the <see cref="RegisterService"/> class.
  /// </summary>
  public class RegisterServiceTests
  {
    /// <summary>
    /// Tests that <see cref="RegisterService.RegisterUserAsync"/> returns success when a user is successfully registered.
    /// </summary>
    [Fact]
    public async Task RegisterUserAsync_ShouldReturnSuccess_WhenUserIsRegistered()
    {
      var userManagerMock = new Mock<UserManager<User>>(
          Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
      var loggerMock = new Mock<ILogger<RegisterService>>();

      userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), "password")).ReturnsAsync(IdentityResult.Success);

      var registerService = new RegisterService(userManagerMock.Object, loggerMock.Object);

      var result = await registerService.RegisterUserAsync("testuser", "password", "test@example.com");

      Assert.True(result.Success);
      Assert.Contains("Successfully registered.", result.Messages[0]);
    }

    /// <summary>
    /// Tests that <see cref="RegisterService.RegisterUserAsync"/> returns an error when registration fails.
    /// </summary>
    [Fact]
    public async Task RegisterUserAsync_ShouldReturnError_WhenRegistrationFails()
    {
      var userManagerMock = new Mock<UserManager<User>>(
          Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
      var loggerMock = new Mock<ILogger<RegisterService>>();

      userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), "password"))
          .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Username already exists." }));

      var registerService = new RegisterService(userManagerMock.Object, loggerMock.Object);

      var result = await registerService.RegisterUserAsync("testuser", "password", "test@example.com");

      Assert.False(result.Success);
      Assert.Contains("Username already exists.", result.Messages[0]);
    }
  }
}