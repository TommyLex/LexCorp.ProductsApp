using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace LexCorp.Channels.Queue.Abstractions.Providers
{
  /// <summary>
  /// Defines the contract for a generic in-memory queue channel provider.
  /// </summary>
  /// <typeparam name="T">The type of items stored in the queue.</typeparam>
  public interface IGenericQueueChannelProvider<T>
  {
    /// <summary>
    /// Writes an item to the queue asynchronously.
    /// </summary>
    /// <param name="item">The item to write to the queue.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    ValueTask WriteAsync(T item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads an item from the queue asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation. The task result contains the item read from the queue.</returns>
    ValueTask<T> ReadAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads all items from the queue asynchronously as they become available.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An asynchronous enumerable that provides all items from the queue.</returns>
    IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default);
  }
}