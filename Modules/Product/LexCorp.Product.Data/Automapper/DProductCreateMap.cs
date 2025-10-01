using AutoMapper;
using LexCorp.Automapper.Abstractions.Map;
using LexCorp.Product.Dto;
using LexCorp.Products.Data.Models.Product;

namespace LexCorp.Product.Data.Automapper
{
  /// <summary>
  /// Serves for mapping between DProduct (database model) and ProductCreateDto (data transfer object).
  /// </summary>
  public class DProductCreateMap : AMapBase<DProduct, ProductCreateDto>, IFromDto<DProduct, ProductCreateDto>
  {
    /// <summary>
    /// Overrides mapping configuration from ProductCreateDto to DProduct to set the Guid property to a new GUID upon mapping.
    /// </summary>
    /// <param name="expression">Expression to configure.</param>
    protected override void ConfigureMapsFromDto(IMappingExpression<ProductCreateDto, DProduct> expression)
    {
      expression.ForMember(dst => dst.Guid, opt => opt.MapFrom(src => Guid.NewGuid()))
        .ForMember(dst => dst.Quantity, opt => opt.Ignore())
        .ForMember(dst => dst.Price, opt => opt.Ignore())
        .ForMember(dst => dst.Description, opt => opt.Ignore());

      base.ConfigureMapsFromDto(expression);
    }
  }
}
