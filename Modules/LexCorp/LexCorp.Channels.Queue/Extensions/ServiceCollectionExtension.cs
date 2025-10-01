using LexCorp.Channels.Queue.Abstractions.Providers;
using LexCorp.Channels.Queue.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace LexCorp.Channels.Queue.Extensions
{
  /// <summary>
  /// Provides extension methods for registering channel-based queue services in the dependency injection container.
  /// </summary>
  public static class ServiceCollectionExtension
  {
    /// <summary>
    /// Registers a generic channel-based queue provider as a singleton in the dependency injection container.
    /// </summary>
    /// <typeparam name="T">The type of items stored in the queue.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    public static void AddChannelQueue<T>(this IServiceCollection services)
    {
      services.AddSingleton<IGenericQueueChannelProvider<T>, GenericQueueChannelProvider<T>>();
    }
  }
}