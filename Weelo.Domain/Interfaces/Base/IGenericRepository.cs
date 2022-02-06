using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Weelo.Domain.Interfaces.Entities;

namespace Weelo.Domain.Interfaces.Base
{
    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TId">Id type</typeparam>
    public interface IGenericRepository<TEntity, TId> :
        IDisposable where TEntity : class, IEntity<TId>
    {
        #region Properties
        /// <summary>
        /// Context
        /// </summary>
        public DbContext Context { get; }
        /// <summary>
        /// Instance of uow
        /// </summary>
        public IUnitOfWork UnitOfWork { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Add entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        /// <summary>
        /// Create a new entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity entity);
        /// <summary>
        /// update an existing entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity);
        /// <summary>
        /// Remove an entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> RemoveAsync(TId id);
        /// <summary>
        /// Get one entity by filter expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            IEnumerable<string>? includes = null,
            bool tracking = true
        );
        /// <summary>
        /// Find an entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        Task<TEntity?> FindByIdAsync(TId id,
            IEnumerable<string>? includes = null,
            bool tracking = true
        );
        /// <summary>
        /// Returns all entities by filter
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            IEnumerable<string>? includes = null,
            bool tracking = true
        );
        /// <summary>
        /// Verify if exists an entity by filter
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            IEnumerable<string>? includes = null
        );
        /// <summary>
        /// Create a custom query in memory
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <param name="tracking"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>>? predicate = null,
            IEnumerable<string>? includes = null,
            bool tracking = true
        );
        #endregion
    }
}
