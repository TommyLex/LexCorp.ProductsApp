using LexCorp.Automapper.Abstractions.Map;
using LexCorp.Product.Dto;
using LexCorp.Products.Data.Models.Product;

namespace LexCorp.Product.Data.Automapper
{
  /// <summary>
  /// Serves for mapping between DProduct (database model) and ProductListDto (data transfer object).
  /// </summary>
  public class DProductListMap : AMapBase<DProduct, ProductListDto>, IToDto<DProduct, ProductListDto>
  {
  }
}
