using LexCorp.Automapper.Abstractions.Map;
using LexCorp.Product.Dto;
using LexCorp.Products.Data.Models.Product;

namespace LexCorp.Product.Data.Automapper
{
  /// <summary>
  /// Serves for mapping between DProduct (database model) and ProductDetailDto (data transfer object).
  /// </summary>
  public class DProductDetailMap : AMapBase<DProduct, ProductDetailDto>, IToDto<DProduct, ProductDetailDto>, IFromDto<DProduct, ProductDetailDto>
  {
  }
}
