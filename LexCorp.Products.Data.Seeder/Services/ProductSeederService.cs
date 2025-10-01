using CsvHelper;
using CsvHelper.Configuration;
using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.Dto;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace LexCorp.Products.Data.Seeder.Services
{
  /// <summary>
  /// Seeds the products table with data from products.csv.
  /// </summary>
  /// <param name="_DProductRepository">The repository for managing products.</param>
  /// <param name="_Logger">The logger for logging information.</param>
  public class ProductSeederService(IDProductRepository _DProductRepository, ILogger<ProductSeederService> _Logger)
  {
    /// <summary>
    /// Seeds the products table with data from products.csv if no products exist.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SeedAsync()
    {
      try
      {
        _Logger.LogInformation("Checking if products need to be seeded...");

        if (_DProductRepository == null)
        {
          _Logger.LogWarning("Product repository is null. Skipping product seeding.");
          return;
        }

        if (await _DProductRepository.HasAnyProducts())
        {
          _Logger.LogInformation("Products already exist. Skipping seeding.");
          return;
        }

        var filePath = Path.Combine(AppContext.BaseDirectory, "SeedData\\products.csv");
        if (!File.Exists(filePath))
        {
          _Logger.LogWarning($"File {filePath} not found. Skipping product seeding.");
          return;
        }

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
          HasHeaderRecord = true,
          MissingFieldFound = null,
          Delimiter = "~"
        });

        csv.Read();
        csv.ReadHeader();
        var products = csv.GetRecords<ProductDetailDto>().ToList();

        foreach (var product in products)
        {
          product.Guid = Guid.NewGuid();
        }

        await _DProductRepository.InsertProducts(products);
        _Logger.LogInformation("Products seeded successfully.");
      }
      catch(Exception ex)
      {
        _Logger.LogError(ex, "Unexpected error while seeding products");
      }
    }
  }
}