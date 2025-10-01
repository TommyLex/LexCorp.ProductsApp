using LexCorp.Channels.Queue.Abstractions.Providers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LexCorp.Channels.Queue.Providers
{
  /// <summary>
  /// Provides a generic in-memory queue channel for asynchronous communication between producers and consumers.
  /// </summary>
  /// <typeparam name="T">The type of items stored in the queue.</typeparam>
  public class GenericQueueChannelProvider<T> : IGenericQueueChannelProvider<T>
  {
    private readonly Channel<T> _Queue = Channel.CreateUnbounded<T>();

    /// <summary>
    /// Writes an item to the queue asynchronously.
    /// </summary>
    /// <param name="item">The item to write to the queue.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public ValueTask WriteAsync(T item, CancellationToken cancellationToken = default)
    {
      return _Queue.Writer.WriteAsync(item, cancellationToken);
    }

    /// <summary>
    /// Reads an item from the queue asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation. The task result contains the item read from the queue.</returns>
    public ValueTask<T> ReadAsync(CancellationToken cancellationToken = default)
    {
      return _Queue.Reader.ReadAsync(cancellationToken);
    }

    /// <summary>
    /// Reads all items from the queue asynchronously as they become available.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An asynchronous enumerable that provides all items from the queue.</returns>
    public async IAsyncEnumerable<T> ReadAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
      await foreach (var item in _Queue.Reader.ReadAllAsync(cancellationToken))
      {
        yield return item;
      }
    }
  }
}