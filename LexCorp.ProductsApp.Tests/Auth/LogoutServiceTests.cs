using LexCorp.Auth.Actor.Abstractions.Providers;
using LexCorp.Auth.Jwt.Options;
using LexCorp.Products.Auth.App.Services;
using LexCorp.Products.Data.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace LexCorp.ProductsApp.Tests.Auth
{
  /// <summary>
  /// Unit tests for the <see cref="LogoutService"/> class.
  /// </summary>
  public class LogoutServiceTests
  {
    /// <summary>
    /// Tests that <see cref="LogoutService.LogoutAsync"/> returns success when the user is successfully logged out.
    /// </summary>
    [Fact]
    public async Task LogoutAsync_ShouldReturnSuccess_WhenUserIsLoggedOut()
    {
      var userManagerMock = new Mock<UserManager<User>>(
          Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
      var actorGuidProviderMock = new Mock<IActorGuidProvider>();
      var loggerMock = new Mock<ILogger<LogoutService>>();
      var constructTokenOptionsMock = Options.Create(new ConstructTokenOptions
      {
        LoginProvider = "TestProvider",
        TokenName = "JWT"
      });

      var user = new User { Id = Guid.NewGuid(), UserName = "testuser" };
      actorGuidProviderMock.Setup(x => x.GetActorId()).Returns(user.Id);
      userManagerMock.Setup(x => x.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);
      userManagerMock.Setup(x => x.RemoveAuthenticationTokenAsync(user, "TestProvider", "JWT"))
          .ReturnsAsync(IdentityResult.Success);

      var logoutService = new LogoutService(userManagerMock.Object, actorGuidProviderMock.Object, loggerMock.Object, constructTokenOptionsMock);

      var result = await logoutService.LogoutAsync();

      Assert.True(result.Success);
    }

    /// <summary>
    /// Tests that <see cref="LogoutService.LogoutAsync"/> returns an error when the user is not found.
    /// </summary>
    [Fact]
    public async Task LogoutAsync_ShouldReturnError_WhenUserNotFound()
    {
      var userManagerMock = new Mock<UserManager<User>>(
          Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
      var actorGuidProviderMock = new Mock<IActorGuidProvider>();
      var loggerMock = new Mock<ILogger<LogoutService>>();
      var constructTokenOptionsMock = Options.Create(new ConstructTokenOptions
      {
        LoginProvider = "TestProvider",
        TokenName = "JWT"
      });

      actorGuidProviderMock.Setup(x => x.GetActorId()).Returns(Guid.NewGuid());
      userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null);

      var logoutService = new LogoutService(userManagerMock.Object, actorGuidProviderMock.Object, loggerMock.Object, constructTokenOptionsMock);

      var result = await logoutService.LogoutAsync();

      Assert.False(result.Success);
      Assert.Contains("User not found.", result.Messages[0]);
    }
  }
}