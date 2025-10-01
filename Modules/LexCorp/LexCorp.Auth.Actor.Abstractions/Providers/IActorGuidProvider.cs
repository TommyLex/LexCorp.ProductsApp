using System;

namespace LexCorp.Auth.Actor.Abstractions.Providers
{
  /// <summary>
  /// Provides functionality to retrieve the unique identifier of the current actor (e.g., user or system).
  /// </summary>
  public interface IActorGuidProvider

  {
    /// <summary>
    /// Retrieves the unique identifier of the current actor.
    /// </summary>
    /// <returns>
    /// A <see cref="Guid"/> representing the actor's unique identifier, or <c>null</c> if the actor is not available.
    /// </returns>
    Guid? GetActorId();
  }
}