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
  ///<inheritdoc cref="IDProductLazyRepository"/>
  public class DProductLazyRepository(AppDbContext _Context,
      IToDto<DProduct, ProductListDto> _ToListDto,
      ILazyLoadingFilterService _LazyLoadingFilterService) : IDProductLazyRepository
  {
    ///<inheritdoc/>
    public async Task<LazyLoadingResultDto<ProductListDto[]>> LazyListAsync(LazyLoadingDto lazyLoad)
    {
      IQueryable<ProductListDto> query = _Context.Set<DProduct>()
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
