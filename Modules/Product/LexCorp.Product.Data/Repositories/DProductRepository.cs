using AutoMapper.QueryableExtensions;
using LexCorp.Automapper.Abstractions.Map;
using LexCorp.Data.Abstractions.Repository;
using LexCorp.LazyLoading.Dto;
using LexCorp.LazyLoading.Filter.Abstractions.Services;
using LexCorp.Product.Data.Abstractions.Repositories;
using LexCorp.Product.Dto;
using LexCorp.Product.Dto.Exceptions;
using LexCorp.Products.Data;
using LexCorp.Products.Data.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace LexCorp.Product.Data.Repositories
{
  ///<inheritdoc cref="IDProductRepository"/>
  public class DProductRepository(AppDbContext context,
      IToDto<DProduct, ProductDetailDto> toDto,
      IFromDto<DProduct, ProductDetailDto> fromDto,
      IToDto<DProduct, ProductListDto> _ToListDto,
      IFromDto<DProduct, ProductCreateDto> _FromCreateDto,
      ILazyLoadingFilterService _LazyLoadingFilterService) : ABaseRepository<DProduct, Guid, ProductDetailDto>(context, toDto, fromDto), IDProductRepository
  {
    ///<inheritdoc/>
    public async Task<bool> HasAnyProducts()
    {
      return await Context.Set<DProduct>().AnyAsync();
    }

    ///<inheritdoc/>
    public async Task<ProductDetailDto> InsertAsync(ProductCreateDto objectToInsert)
    {
      var model = await InsertAsync(_FromCreateDto.MapFromDto(objectToInsert));
      return _ToDto.MapToDto(model);
    }

    ///<inheritdoc/>
    public async Task InsertProducts(IEnumerable<ProductDetailDto> products)
    {
      var productModels = _FromDto.MapFromDto(products);
      await Context.Set<DProduct>().AddRangeAsync(productModels);
      await Context.SaveChangesAsync();
    }

    ///<inheritdoc/>
    public async Task<IEnumerable<ProductListDto>> ListAllAsync()
    {
      return await Context.Set<DProduct>()
        .OrderBy(p => p.Name)
        .ProjectTo<ProductListDto>(_ToListDto.ConfigurationProvider)
        .ToListAsync();
    }

    ///<inheritdoc/>
    public async Task<ProductDetailDto> UpdateProductQty(ProductUpdateQtyDto product)
    {
      var model = await Context.Set<DProduct>().FirstOrDefaultAsync(x=> x.Guid == product.Guid);
      if (model == null)
        throw new ProductNotFoundException($"Product with {product.Guid} not found.");

      model.Quantity = product.Quantity;
      await Context.SaveChangesAsync();

      return _ToDto.MapToDto(model);
    }

    ///<inheritdoc/>
    public async Task<LazyLoadingResultDto<ProductListDto[]>> LazyListAsync(LazyLoadingDto lazyLoad)
    {
      IQueryable<ProductListDto> query = Context.Set<DProduct>()
        .ProjectTo<ProductListDto>(_ToListDto.ConfigurationProvider);

      query = _LazyLoadingFilterService.FilterAndOrder(lazyLoad, query);

      return new LazyLoadingResultDto<ProductListDto[]>()
      {
        Total = query.Count(),
        Data = await query.Skip(lazyLoad.First.Value).Take(lazyLoad.Rows.Value).ToArrayAsync(),
        Success = true
      };
    }
  }
}
