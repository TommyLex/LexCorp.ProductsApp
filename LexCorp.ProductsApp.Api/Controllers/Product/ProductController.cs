using Asp.Versioning;
using LexCorp.LazyLoading.Dto;
using LexCorp.Product.App.Abstractions.Services;
using LexCorp.Product.Dto;
using LexCorp.Results.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexCorp.ProductsApp.Api.Controllers.Product
{
  /// <summary>
  /// Controller for handling product-related operations.
  /// </summary>
  /// <param name="_GetProductListService">The service for retrieving a list of products.</param>
  /// <param name="_GetProductDetailService">The service for retrieving product details.</param>
  /// <param name="_GetProductsLazyListService">The service for retrieving a list of products with pagination.</param>
  [ApiController]
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiVersion("1.0")]
  [ApiVersion("2.0")]
  [AllowAnonymous]
  public class ProductController(IGetProductListService _GetProductListService, 
    IGetProductDetailService _GetProductDetailService,
    IGetProductsLazyListService _GetProductsLazyListService) : Controller
  {

    /// <summary>
    /// Retrieves a list of all products.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto{T}"/>
    /// with a collection of <see cref="ProductListDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    [HttpGet("[action]")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<ResultInfoDto<IEnumerable<ProductListDto>>>> GetProductsList()
    {
      var result = await _GetProductListService.GetProductsListAsync();
      if (!result.Success)
      {
        return BadRequest(result);
      }
      return Ok(result);
    }

    /// <summary>
    /// Retrieves the details of a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto{T}"/>
    /// with the <see cref="ProductDetailDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    [HttpGet("[action]/{id}")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<ResultInfoDto<ProductDetailDto>>> GetProductDetail(Guid id)
    {
      var result = await _GetProductDetailService.GetProductDetailAsync(id);
      if (!result.Success)
      {
        return BadRequest(result);
      }
      return Ok(result);
    }

    /// <summary>
    /// Retrieves a lazy-loaded list of products based on the provided lazy loading parameters.
    /// </summary>
    /// <param name="lazyLoadingDto">The lazy loading DTO containing filtering, sorting, and pagination parameters.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto{T}"/>
    /// with a collection of <see cref="ProductListDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    /// <remarks>
    /// This method uses lazy loading to retrieve a subset of products based on the specified parameters.
    /// Ensure that the <paramref name="lazyLoadingDto"/> contains valid values for pagination and sorting.
    /// If the DTO is null, default lazy loading parameters will be used.
    /// </remarks>
    [HttpPost("[action]")]
    [MapToApiVersion("2.0")]
    public async Task<ActionResult<ResultInfoDto<IEnumerable<ProductListDto>>>> GetLazyProductsList([FromBody]LazyLoadingDto lazyLoadingDto)
    {
      var result = await _GetProductsLazyListService.GetProductsLazyListAsync(lazyLoadingDto);
      if (!result.Success)
      {
        return BadRequest(result);
      }
      return Ok(result);
    }
  }
}