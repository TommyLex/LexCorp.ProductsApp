using LexCorp.Channels.Queue.Abstractions.Providers;
using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.Dto;
using LexCorp.Product.Dto.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Background service responsible for processing product quantity updates from an in-memory queue.
/// </summary>
/// <param name="_ServiceProvider">Service provider.</param>
/// <param name="_Logger">The logger for logging information and errors.</param>
/// <param name="_Queue">The in-memory queue provider for processing product updates.</param>
public class ProductQtyProcessorService(IServiceProvider _ServiceProvider,
    ILogger<ProductQtyProcessorService> _Logger,
    IGenericQueueChannelProvider<ProductUpdateQtyDto> _Queue) : BackgroundService
{
  /// <summary>
  /// Executes the background service, continuously processing messages from the queue.
  /// </summary>
  /// <param name="stoppingToken">A token to monitor for cancellation requests.</param>
  /// <returns>A task that represents the asynchronous operation.</returns>
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      await _ProcessQueueAsync(stoppingToken);
      await Task.Delay(1000, stoppingToken);
    }
  }

  /// <summary>
  /// Reads and processes messages from the in-memory queue.
  /// </summary>
  /// <returns>A task representing the asynchronous operation.</returns>
  private async Task _ProcessQueueAsync(CancellationToken cancellationToken)
  {
    await foreach (var productUpdate in _Queue.ReadAllAsync(cancellationToken))
    {
      try
      {
        _Logger.LogInformation($"Processing product update: {productUpdate.Guid}");

        using (var scope = _ServiceProvider.CreateScope())
        {
          var productRepository = scope.ServiceProvider.GetRequiredService<IDProductRepository>();

          var updatedProduct = await productRepository.UpdateProductQty(productUpdate);

          _Logger.LogInformation($"Successfully updated quantity for product with ID: {updatedProduct.Guid}, updated Qty = {updatedProduct.Quantity}");
        }
      }
      catch (ProductNotFoundException ex)
      {
        _Logger.LogError(ex, $"Product not found. ID: {productUpdate.Guid}");
      }
      catch (Exception ex)
      {
        _Logger.LogError(ex, $"An error occurred while updating quantity for product with ID: {productUpdate.Guid}");
      }
    }
  }
}