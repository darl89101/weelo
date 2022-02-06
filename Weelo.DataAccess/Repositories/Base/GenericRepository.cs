using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Weelo.Domain.Exceptions;
using Weelo.Domain.Interfaces.Base;
using Weelo.Domain.Interfaces.Entities;

namespace Weelo.DataAccess.Repositories.Base
{
    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public class GenericRepository<TEntity, TId> :
        IGenericRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
    {
        #region Properties
        /// <summary>
        /// DbSet of entity
        /// </summary>
        internal DbSet<TEntity> _dbSet;
        /// <summary>
        /// Context application
        /// </summary>
        protected readonly DbContext _context;
        /// <summary>
        /// Unit of work
        /// </summary>
        protected readonly IUnitOfWork _uow;
        /// <summary>
        /// return the db context
        /// </summary>
        public DbContext Context => _context;
        /// <summary>
        /// return the current unit of work
        /// </summary>
        public IUnitOfWork UnitOfWork => _uow;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericRepository(DbContext context)
        {
            _uow = new UnitOfWork(context);
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add new entity to the context
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            AddAuditFieldEntity(entity);
            var res = await _dbSet.AddAsync(entity);

            return res.Entity;
        }

        /// <summary>
        /// Add a list of entities
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities) AddAuditFieldEntity(entity);

            await _dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Verify if exists an entity by filter
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? predicate = null, IEnumerable<string>? includes = null)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            AddTrackingAndIncludes(ref query, includes, false);

            return predicate == null ? await query.AnyAsync() : await query.Where(predicate).AnyAsync();
        }

        /// <summary>
        /// Returns all entities by filter
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? predicate = null, IEnumerable<string>? includes = null, bool tracking = true)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            AddTrackingAndIncludes(ref query, includes, tracking);

            return predicate == null ? await query.ToListAsync() : await query.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Find an entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual async Task<TEntity?> FindByIdAsync(TId id,
            IEnumerable<string>? includes = null,
            bool tracking = true
        )
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            AddTrackingAndIncludes(ref query, includes, tracking);

            return await query.Where(m => m.Id != null && m.Id.Equals(id)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get one entity by filter expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, IEnumerable<string>? includes = null, bool tracking = true)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            AddTrackingAndIncludes(ref query, includes, tracking);

            return predicate == null ? await query.FirstOrDefaultAsync() : await query.Where(predicate).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Create a custom query in memory
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? predicate = null, IEnumerable<string>? includes = null, bool tracking = true)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            AddTrackingAndIncludes(ref query, includes, tracking);

            return predicate == null ? query : query.Where(predicate);
        }

        /// <summary>
        /// Remove an entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<TEntity> RemoveAsync(TId id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null) throw new NotFoundException($"{nameof(entity)} not found");

            TEntity res = _dbSet.Remove(entity).Entity;

            return res;
        }

        /// <summary>
        /// update an existing entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            TEntity? oEntity = await _dbSet.FindAsync(entity.Id);

            if (oEntity != null)
            {
                if (entity is IAudit audit)
                {
                    audit.CreatedAt = ((IAudit)oEntity).CreatedAt;
                    audit.UpdatedAt = DateTime.Now;
                }

                _context.Entry(oEntity).CurrentValues.SetValues(entity);
            }

            return entity;
        }
        #endregion

        #region Privates
        /// <summary>
        /// Add Audit fields
        /// </summary>
        /// <param name="entity"></param>
        private static void AddAuditFieldEntity(TEntity entity)
        {
            if (entity is IAudit audit) audit.CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// agrega includes, tracking y query filters
        /// </summary>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <param name="tracking"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <returns></returns>
        private static void AddTrackingAndIncludes(ref IQueryable<TEntity> query, IEnumerable<string>? includes, bool tracking)
        {
            if (!tracking) query = query.AsNoTracking();

            if (includes != null)
                foreach (string include in includes)
                    query = tracking ? query.Include(include) : query.Include(include).AsNoTracking();
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _uow.Dispose();
                    _context.Dispose();
                }
            }
            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
