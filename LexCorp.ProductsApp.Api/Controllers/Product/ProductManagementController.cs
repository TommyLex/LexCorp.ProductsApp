using Asp.Versioning;
using LexCorp.Product.App.Abstractions.Services;
using LexCorp.Product.Dto;
using LexCorp.Products.Auth.Dto.Roles;
using LexCorp.Results.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LexCorp.ProductsApp.Api.Controllers.Product
{
  /// <summary>
  /// Controller for managing product-related operations.
  /// </summary>
  [ApiController]
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiVersion("1.0")]
  [Authorize(Roles = nameof(RoleType.Admin) + "," + nameof(RoleType.ProductManager))]
  public class ProductManagementController : Controller
  {
    private readonly ICreateProductService _CreateProductService;
    private readonly IUpdateProductQtyService _UpdateProductQtyService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductManagementController"/> class.
    /// </summary>
    /// <param name="createProductService">The service for creating products.</param>
    /// <param name="updateProductQtyService">The service for updating product quantities.</param>
    public ProductManagementController(ICreateProductService createProductService, IUpdateProductQtyService updateProductQtyService)
    {
      _CreateProductService = createProductService;
      _UpdateProductQtyService = updateProductQtyService;
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="product">The product creation data transfer object.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto{T}"/>
    /// with the created <see cref="ProductDetailDto"/> if successful, or error messages if the operation fails.
    /// </returns>
    [MapToApiVersion("1.0")]
    [HttpPost("[action]")]
    public async Task<ActionResult<ResultInfoDto<ProductDetailDto>>> CreateProduct([FromBody] ProductCreateDto product)
    {
      var result = await _CreateProductService.CreateProductAsync(product);
      if (!result.Success)
      {
        return BadRequest(result);
      }
      return Ok(result);
    }

    /// <summary>
    /// Updates the quantity of a product.
    /// </summary>
    /// <param name="product">The product quantity update data transfer object.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="ResultInfoDto"/>
    /// with success property and error messages if the operation fails.
    /// </returns>
    [MapToApiVersion("1.0")]
    [HttpPut("[action]")]
    public async Task<ActionResult<ResultInfoDto>> UpdateProductQty([FromBody] ProductUpdateQtyDto product)
    {
      var result = await _UpdateProductQtyService.UpdateProductQtyAsync(product);
      if (!result.Success)
      {
        return BadRequest(result);
      }
      return Ok(result);
    }
  }
}