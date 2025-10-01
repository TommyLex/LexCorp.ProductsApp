using System.Collections.Generic;

namespace LexCorp.Automapper.Abstractions.Map
{
  /// <summary>
  /// Interface for mapping from a DTO to a database model.
  /// </summary>
  /// <typeparam name="TMod">The database model.</typeparam>
  /// <typeparam name="TDto">The Data Transfer Object.</typeparam>
  public interface IFromDto<TMod, TDto>
    where TMod : class
    where TDto : class
  {
    /// <summary>
    /// Converts a DTO to a new database model and populates its properties based on the DTO.
    /// </summary>
    /// <param name="dto">The Data Transfer Object to convert.</param>
    /// <returns>An instance of the database model.</returns>
    TMod MapFromDto(TDto dto);

    /// <summary>
    /// Converts a DTO to a database model and updates the database model based on the DTO.
    /// </summary>
    /// <param name="model">The database model to update.</param>
    /// <param name="dto">The object containing transformation data.</param>
    /// <returns>The updated database model.</returns>
    TMod MapFromDto(TMod model, TDto dto);

    /// <summary>
    /// Converts a collection of DTOs to database models.
    /// </summary>
    /// <param name="dtos">The objects containing transformation data.</param>
    /// <returns>A collection of database models.</returns>
    IEnumerable<TMod> MapFromDto(IEnumerable<TDto> dtos);

    /// <summary>
    /// Converts a collection of DTOs to a collection of database models and updates the database models based on the DTOs.
    /// </summary>
    /// <param name="models">The database models to update.</param>
    /// <param name="dtos">The objects containing transformation data.</param>
    /// <returns>A collection of updated database models.</returns>
    IEnumerable<TMod> MapFromDto(IEnumerable<TMod> models, IEnumerable<TDto> dtos);
  }
}
