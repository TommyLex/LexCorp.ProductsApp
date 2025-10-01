using System.Threading.Tasks;

namespace LexCorp.Data.Abstractions.Repository
{
  /// <summary>
  /// Generic repository interface - defines basic operations for database manipulation.
  /// </summary>
  /// <typeparam name="TMod">The database model type.</typeparam>
  /// <typeparam name="TId">The type of the identifier for the database model.</typeparam>
  public interface IRepository<TMod, TId>
    where TMod : class
  {
    /// <summary>
    /// Asynchronously retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>The entity matching the given ID.</returns>
    Task<TMod> GetAsync(TId id);

    /// <summary>
    /// Asynchronously inserts a new entity into the database.
    /// </summary>
    /// <param name="objectToInsert">The entity to insert.</param>
    /// <returns>The inserted entity.</returns>
    Task<TMod> InsertAsync(TMod objectToInsert);

    /// <summary>
    /// Asynchronously updates an existing entity in the database.
    /// </summary>
    /// <param name="objectToUpdate">The entity to update.</param>
    Task UpdateAsync(TMod objectToUpdate);

    /// <summary>
    /// Asynchronously deletes an entity from the database.
    /// </summary>
    /// <param name="objectToDelete">The entity to delete.</param>
    Task DeleteAsync(TMod objectToDelete);

    /// <summary>
    /// Begins a database transaction.
    /// </summary>
    Task BeginTransaction();

    /// <summary>
    /// Commits the current transaction. If an exception occurs, a rollback is performed.
    /// Saves changes before committing.
    /// </summary>
    Task CommitTransaction();

    /// <summary>
    /// Commits the current transaction. If an exception occurs, no rollback is performed.
    /// Saves changes before committing.
    /// </summary>
    Task CommitTransactionNoRollback();

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    Task RollbackTransaction();
  }

  /// <summary>
  /// Generic repository interface with DTO support - defines basic operations for database manipulation.
  /// </summary>
  /// <typeparam name="TMod">The database model type.</typeparam>
  /// <typeparam name="TId">The type of the identifier for the database model.</typeparam>
  /// <typeparam name="TDto">The Data Transfer Object type.</typeparam>
  public interface IRepository<TMod, TId, TDto> : IRepository<TMod, TId>
    where TMod : class
    where TDto : class
  {
    /// <summary>
    /// Asynchronously retrieves a DTO by its ID.
    /// </summary>
    /// <param name="id">The identifier of the DTO to retrieve.</param>
    /// <returns>The DTO matching the given ID.</returns>
    new Task<TDto> GetAsync(TId id);

    /// <summary>
    /// Asynchronously inserts a new DTO into the database.
    /// </summary>
    /// <param name="objectToInsert">The DTO to insert.</param>
    /// <returns>The inserted DTO.</returns>
    Task<TDto> InsertAsync(TDto objectToInsert);

    /// <summary>
    /// Asynchronously updates an existing DTO in the database.
    /// </summary>
    /// <param name="objectToUpdate">The DTO to update.</param>
    /// <param name="id">The identifier of the entity to update.</param>
    Task UpdateAsync(TDto objectToUpdate, TId id);

    /// <summary>
    /// Asynchronously deletes a DTO from the database.
    /// </summary>
    /// <param name="objectToDelete">The DTO to delete.</param>
    Task DeleteAsync(TDto objectToDelete);
  }
}
