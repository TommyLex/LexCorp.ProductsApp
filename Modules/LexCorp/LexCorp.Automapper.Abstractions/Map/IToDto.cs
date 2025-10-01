using AutoMapper;
using System.Collections.Generic;

namespace LexCorp.Automapper.Abstractions.Map
{
  /// <summary>
  /// Interface for mapping from a database model to a DTO.
  /// </summary>
  /// <typeparam name="TMod">The database model.</typeparam>
  /// <typeparam name="TDto">The object to map data into.</typeparam>
  public interface IToDto<TMod, TDto>
    where TMod : class
    where TDto : class
  {
    /// <summary>
    /// Converts a database model to a DTO.
    /// </summary>
    /// <param name="model">The database model to convert.</param>
    /// <returns>The mapped DTO.</returns>
    TDto MapToDto(TMod model);

    /// <summary>
    /// The IConfigurationProvider for the given mapping to a DTO.
    /// </summary>
    IConfigurationProvider ConfigurationProvider { get; }

    /// <summary>
    /// Converts a collection of database models to DTOs.
    /// </summary>
    /// <param name="models">The collection of database models to convert.</param>
    /// <returns>A collection of mapped DTOs.</returns>
    IEnumerable<TDto> MapToDto(IEnumerable<TMod> models);
  }
}
