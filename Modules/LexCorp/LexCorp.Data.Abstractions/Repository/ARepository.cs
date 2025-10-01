using LexCorp.Automapper.Abstractions.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace LexCorp.Data.Abstractions.Repository
{
  /// <summary>
  /// Abstract base repository providing basic database operations.
  /// </summary>
  /// <typeparam name="TMod">The database model type.</typeparam>
  /// <typeparam name="TId">The type of the identifier for the database model.</typeparam>
  public abstract class ABaseRepository<TMod, TId> : IRepository<TMod, TId>
        where TMod : class
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ABaseRepository{TMod, TId}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ABaseRepository(DbContext context)
    {
      Context = context;
    }

    /// <summary>
    /// Gets the database context.
    /// </summary>
    protected DbContext Context { get; }

    /// <summary>
    /// Asynchronously deletes an entity from the database.
    /// </summary>
    /// <param name="objectToDelete">The entity to delete.</param>
    public virtual async Task DeleteAsync(TMod objectToDelete)
    {
      Context.Set<TMod>().Remove(objectToDelete);
      await Context.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>The entity matching the given ID.</returns>
    public virtual async Task<TMod> GetAsync(TId id)
    {
      TMod result = await Context.Set<TMod>().FindAsync(id);
      return result;
    }

    /// <summary>
    /// Asynchronously inserts a new entity into the database.
    /// </summary>
    /// <param name="objectToInsert">The entity to insert.</param>
    /// <returns>The inserted entity.</returns>
    public virtual async Task<TMod> InsertAsync(TMod objectToInsert)
    {
      Context.Set<TMod>().Add(objectToInsert);
      await Context.SaveChangesAsync();
      return objectToInsert;
    }

    /// <summary>
    /// Asynchronously updates an existing entity in the database.
    /// </summary>
    /// <param name="objectToUpdate">The entity to update.</param>
    public virtual async Task UpdateAsync(TMod objectToUpdate)
    {
      await Context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task BeginTransaction()
    {
      return Context.Database.BeginTransactionAsync();
    }

    /// <inheritdoc/>
    public async Task CommitTransaction()
    {
      var trx = GetTransaction();
      if (trx == null)
        throw new NullReferenceException("Transaction commit error. Transaction not found.");

      try
      {
        await Context.SaveChangesAsync();
        await trx.CommitAsync();
      }
      catch
      {
        await trx.RollbackAsync();
        throw;
      }
    }

    /// <inheritdoc/>
    public async Task CommitTransactionNoRollback()
    {
      var trx = GetTransaction();
      if (trx == null)
        throw new NullReferenceException("Transaction commit error. Transaction not found.");

      await Context.SaveChangesAsync();
      await trx.CommitAsync();
    }

    /// <inheritdoc/>
    public Task RollbackTransaction()
    {
      var trx = GetTransaction();
      if (trx == null)
        throw new NullReferenceException("Transaction rollback error. Transaction not found.");

      return trx.RollbackAsync();
    }

    /// <summary>
    /// Retrieves the currently active transaction.
    /// </summary>
    /// <returns>The current database transaction.</returns>
    protected DbTransaction GetTransaction()
    {
      return Context.Database.CurrentTransaction?.GetDbTransaction();
    }
  }

  /// <summary>
  /// Abstract base repository with DTO support, providing basic database operations.
  /// </summary>
  /// <typeparam name="TMod">The database model type.</typeparam>
  /// <typeparam name="TId">The type of the identifier for the database model.</typeparam>
  /// <typeparam name="TDto">The Data Transfer Object type.</typeparam>
  public class ABaseRepository<TMod, TId, TDto> : ABaseRepository<TMod, TId>, IRepository<TMod, TId, TDto>
    where TMod : class
    where TDto : class
  {
    protected static IToDto<TMod, TDto> _ToDto;
    protected static IFromDto<TMod, TDto> _FromDto;

    /// <summary>
    /// Initializes a new instance of the <see cref="ABaseRepository{TMod, TId, TDto}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="mappingToDto">The mapping service for converting models to DTOs.</param>
    public ABaseRepository(
      DbContext context,
      IToDto<TMod, TDto> mappingToDto)
      : base(context)
    {
      _ToDto = mappingToDto;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ABaseRepository{TMod, TId, TDto}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="toDto">The mapping service for converting models to DTOs.</param>
    /// <param name="fromDto">The mapping service for converting DTOs to models.</param>
    public ABaseRepository(DbContext context, IToDto<TMod, TDto> toDto, IFromDto<TMod, TDto> fromDto)
      : this(context, toDto)
    {
      _FromDto = fromDto;
    }

    /// <summary>
    /// Asynchronously retrieves a DTO by its ID.
    /// </summary>
    /// <param name="id">The identifier of the DTO to retrieve.</param>
    /// <returns>The DTO matching the given ID.</returns>
    public virtual new async Task<TDto> GetAsync(TId id)
    {
      TMod result = await Context.Set<TMod>().FindAsync(id);
      TDto resultDto = _ToDto.MapToDto(result);
      return resultDto;
    }

    /// <summary>
    /// Asynchronously inserts a new DTO into the database.
    /// </summary>
    /// <param name="objectToInsert">The DTO to insert.</param>
    /// <returns>The inserted DTO.</returns>
    public virtual async Task<TDto> InsertAsync(TDto objectToInsert)
    {
      if (_FromDto == null) throw new Exception($"Mapping from DTO is not configured for {typeof(TDto)}.");
      TMod modelToInsert = _FromDto.MapFromDto(objectToInsert);
      Context.Set<TMod>().Add(modelToInsert);
      await Context.SaveChangesAsync();
      return _ToDto.MapToDto(modelToInsert);
    }

    /// <summary>
    /// Asynchronously updates an existing DTO in the database.
    /// </summary>
    /// <param name="objectToUpdate">The DTO to update.</param>
    /// <param name="id">The identifier of the entity to update.</param>
    public virtual async Task UpdateAsync(TDto objectToUpdate, TId id)
    {
      if (_FromDto == null) throw new Exception($"Mapping from DTO is not configured for {typeof(TDto)}.");
      TMod dbEntity = await Context.Set<TMod>().FindAsync(id);

      _FromDto.MapFromDto(dbEntity, objectToUpdate);

      await Context.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously updates an object and returns the updated DTO.
    /// </summary>
    /// <param name="objectToUpdate">The DTO to update.</param>
    /// <param name="id">The identifier of the entity to update.</param>
    /// <returns>The updated DTO.</returns>
    public virtual async Task<TDto> Update2Async(TDto objectToUpdate, TId id)
    {
      await UpdateAsync(objectToUpdate, id);
      return await GetAsync(id);
    }

    /// <summary>
    /// Asynchronously deletes a DTO from the database.
    /// </summary>
    /// <param name="objectToDelete">The DTO to delete.</param>
    public virtual async Task DeleteAsync(TDto objectToDelete)
    {
      if (_FromDto == null) throw new Exception($"Mapping from DTO is not configured for {typeof(TDto)}.");
      TMod modelToDelete = _FromDto.MapFromDto(objectToDelete);
      Context.Set<TMod>().Remove(modelToDelete);
      await Context.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously deletes an entity by its ID.
    /// </summary>
    /// <param name="idToDelete">The identifier of the entity to delete.</param>
    /// <param name="autocommit">If true, saves changes after deletion.</param>
    public async Task DeleteAsync(TId idToDelete, bool autocommit)
    {
      TMod modelToDelete = Context.Set<TMod>().Find(idToDelete);
      Context.Set<TMod>().Remove(modelToDelete);
      if (autocommit) await Context.SaveChangesAsync();
    }
  }
}
