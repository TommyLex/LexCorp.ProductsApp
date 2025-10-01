using AutoMapper;
using AutoMapper.EquivalencyExpression;
using System;
using System.Collections.Generic;

namespace LexCorp.Automapper.Abstractions.Map
{
  /// <summary>
  /// Base class for mapping between database models and DTOs using AutoMapper.
  /// </summary>
  /// <typeparam name="TMod">The database model type.</typeparam>
  /// <typeparam name="TDto">The Data Transfer Object (DTO) type.</typeparam>
  public abstract class AMapBase<TMod, TDto>
    where TMod : class
    where TDto : class
  {
    private IMapper _ToDto;
    private IMapper _FromDto;

    /// <summary>
    /// Gets the mapper for converting models to DTOs.
    /// </summary>
    protected IMapper ToDto
    {
      get
      {
        if (_ToDto == null)
        {
          _ToDto = InitMap(InitToDto);
        }

        return _ToDto;
      }
    }

    /// <summary>
    /// Gets the mapper for converting DTOs to models.
    /// </summary>
    protected IMapper FromDto
    {
      get
      {
        if (_FromDto == null)
        {
          _FromDto = InitMap(InitFromDto);
        }

        return _FromDto;
      }
    }

    /// <summary>
    /// Gets the configuration provider for the mapping.
    /// </summary>
    public IConfigurationProvider ConfigurationProvider => ToDto.ConfigurationProvider;

    /// <summary>
    /// Initializes the mapper with the specified configuration.
    /// </summary>
    /// <param name="configureMaps">The configuration action for the mapper.</param>
    /// <returns>The initialized mapper.</returns>
    /// <exception cref="Exception">Thrown if the mapping configuration is invalid.</exception>
    protected IMapper InitMap(Action<IMapperConfigurationExpression> configureMaps)
    {
      var conf = new MapperConfiguration(cfg =>
      {
        configureMaps(cfg);
      });
      try
      {
        conf.AssertConfigurationIsValid();
      }
      catch (Exception e)
      {
        throw new Exception("Error in mapping configuration for " + GetType().FullName, e);
      }
      return conf.CreateMapper();
    }

    /// <summary>
    /// Configures the mapping from DTO to model.
    /// </summary>
    /// <param name="cfg">The mapper configuration expression.</param>
    protected virtual void InitFromDto(IMapperConfigurationExpression cfg)
    {
      var expression = cfg.CreateMap<TDto, TMod>();

      if (typeof(TDto).GetProperty("Guid") != null && typeof(TMod).GetProperty("Guid") != null)
      {
        cfg.AddCollectionMappers();
        expression.EqualityComparison((odto, o) => odto.GetType().GetProperty("Guid").GetValue(odto) == o.GetType().GetProperty("Guid").GetValue(o));
      }

      ConfigureMapsFromDto(expression);
    }

    /// <summary>
    /// Configures the mapping from model to DTO.
    /// </summary>
    /// <param name="cfg">The mapper configuration expression.</param>
    protected virtual void InitToDto(IMapperConfigurationExpression cfg)
    {
      var expression = cfg.CreateMap<TMod, TDto>();
      ConfigureMapsToDto(expression);
    }

    /// <summary>
    /// Allows additional configuration for mapping from DTO to model.
    /// </summary>
    /// <param name="expression">The mapping expression.</param>
    protected virtual void ConfigureMapsFromDto(IMappingExpression<TDto, TMod> expression)
    {
    }

    /// <summary>
    /// Allows additional configuration for mapping from model to DTO.
    /// </summary>
    /// <param name="expression">The mapping expression.</param>
    protected virtual void ConfigureMapsToDto(IMappingExpression<TMod, TDto> expression)
    {
    }

    /// <summary>
    /// Maps a single DTO to a model.
    /// </summary>
    /// <param name="dto">The DTO to map.</param>
    /// <returns>The mapped model.</returns>
    public TMod MapFromDto(TDto dto)
    {
      return FromDto.Map<TMod>(dto);
    }

    /// <summary>
    /// Maps a DTO to an existing model, updating its properties.
    /// </summary>
    /// <param name="model">The existing model to update.</param>
    /// <param name="dto">The DTO containing the updated data.</param>
    /// <returns>The updated model.</returns>
    public TMod MapFromDto(TMod model, TDto dto)
    {
      return FromDto.Map(dto, model);
    }

    /// <summary>
    /// Maps a collection of DTOs to models.
    /// </summary>
    /// <param name="dtos">The collection of DTOs to map.</param>
    /// <returns>The mapped collection of models.</returns>
    public IEnumerable<TMod> MapFromDto(IEnumerable<TDto> dtos)
    {
      return FromDto.Map<IEnumerable<TMod>>(dtos);
    }

    /// <summary>
    /// Maps a collection of DTOs to an existing collection of models, updating their properties.
    /// </summary>
    /// <param name="model">The existing collection of models to update.</param>
    /// <param name="dtos">The collection of DTOs containing the updated data.</param>
    /// <returns>The updated collection of models.</returns>
    public IEnumerable<TMod> MapFromDto(IEnumerable<TMod> model, IEnumerable<TDto> dtos)
    {
      return FromDto.Map<IEnumerable<TDto>, IEnumerable<TMod>>(dtos, model);
    }

    /// <summary>
    /// Maps a single model to a DTO.
    /// </summary>
    /// <param name="model">The model to map.</param>
    /// <returns>The mapped DTO.</returns>
    public TDto MapToDto(TMod model)
    {
      return ToDto.Map<TDto>(model);
    }

    /// <summary>
    /// Maps a collection of models to DTOs.
    /// </summary>
    /// <param name="models">The collection of models to map.</param>
    /// <returns>The mapped collection of DTOs.</returns>
    public IEnumerable<TDto> MapToDto(IEnumerable<TMod> models)
    {
      return ToDto.Map<IEnumerable<TDto>>(models);
    }
  }
}
