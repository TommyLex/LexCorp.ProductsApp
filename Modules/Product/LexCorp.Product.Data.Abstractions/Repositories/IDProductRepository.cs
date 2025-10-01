using LexCorp.LazyLoading.Dto;
using LexCorp.Product.Dto;
using LexCorp.Product.Dto.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexCorp.Product.Data.Abstractions.Repositories
{
  /// <summary>
  /// Defines the contract for a repository that manages product entities.
  /// </summary>
  public interface IDProductRepository
  {
    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the product DTO.</returns>
    Task<ProductDetailDto> GetAsync(Guid id);

    /// <summary>
    /// Inserts a new product into the repository.
    /// </summary>
    /// <param name="objectToInsert">The product DTO to insert.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the inserted product DTO.</returns>
    Task<ProductDetailDto> InsertAsync(ProductDetailDto objectToInsert);

    /// <summary>
    /// Updates an existing product in the repository.
    /// </summary>
    /// <param name="objectToUpdate">The product DTO containing updated values.</param>
    /// <param name="id">The unique identifier of the product to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(ProductDetailDto objectToUpdate, Guid id);

    /// <summary>
    /// Deletes a product from the repository.
    /// </summary>
    /// <param name="objectToDelete">The product DTO to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(ProductDetailDto objectToDelete);

    /// <summary>
    /// Inserts multiple products into the database.
    /// </summary>
    /// <param name="products">Products to insert.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task InsertProducts(IEnumerable<ProductDetailDto> products);

    /// <summary>
    /// Determines whether there are any products available.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if there are any
    /// products; otherwise, <see langword="false"/>.</returns>
    Task<bool> HasAnyProducts();

    /// <summary>
    /// Retrieves a list of all products in the repository.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of 
    /// <see cref="ProductListDto"/> representing all products.
    /// </returns>
    Task<IEnumerable<ProductListDto>> ListAllAsync();

    /// <summary>
    /// Creates a new product into the repository.
    /// </summary>
    /// <param name="objectToInsert">The product DTO to insert.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the inserted product DTO.</returns>
    Task<ProductDetailDto> InsertAsync(ProductCreateDto objectToInsert);

    /// <summary>
    /// Updates product quantity in the repository.
    /// </summary>
    /// <param name="product">Product and quantity to update.</param>
    /// <returns>Updated product detail dto.</returns>
    /// <exception cref="ProductNotFoundException">If product is not found by its identificator.</exception>
    Task<ProductDetailDto> UpdateProductQty(ProductUpdateQtyDto product);
  }
}