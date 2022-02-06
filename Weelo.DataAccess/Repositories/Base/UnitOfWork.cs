using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Weelo.Domain.Interfaces.Base;

namespace Weelo.DataAccess.Repositories.Base
{
    /// <summary>
    /// Implementation of unit of work
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Properties
        /// <summary>
        /// Contexto de la aplicación
        /// </summary>
        public DbContext Context { get; }
        /// <summary>
        /// Transacción
        /// </summary>
        private TransactionScope? _transaction;

        /// <summary>
        /// TimeOut de la transacción
        /// </summary>
        private readonly int _defaultTimeOut = 2;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseContext"></param>
        public UnitOfWork(DbContext baseContext) => Context = baseContext;
        #endregion

        #region Methods
        /// <summary>
        /// Starts a new transaction
        /// </summary>
        /// <returns></returns>
        public TransactionScope BeginTransaction()
        {
            _transaction = new(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.Serializable,
                Timeout = TimeSpan.FromMinutes(_defaultTimeOut)
            }, TransactionScopeAsyncFlowOption.Enabled);

            return _transaction;
        }

        /// <summary>
        /// Starts a new transaction
        /// </summary>
        /// <returns></returns>
        public TransactionScope BeginTransaction(TransactionScopeOption? option, IsolationLevel? isolation, int? defaultTimeOut)
        {
            _transaction = new(option ?? TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = isolation ?? IsolationLevel.Serializable,
                Timeout = TimeSpan.FromMinutes(defaultTimeOut ?? _defaultTimeOut)
            }, TransactionScopeAsyncFlowOption.Enabled);

            return _transaction;
        }

        /// <summary>
        /// Commit pending changes
        /// </summary>
        /// <param name="completeTransaction"></param>
        /// <returns></returns>
        public async Task CommitAsync(bool completeTransaction = false)
        {
            await Context.SaveChangesAsync();

            if (_transaction != null && completeTransaction) _transaction.Complete();
        }

        /// <summary>
        /// Rollback pending changes
        /// </summary>
        public void Rollback()
        {
            DismissChanges();

            if (_transaction != null) _transaction.Dispose();
        }
        #endregion

        #region Privates
        /// <summary>
        /// Dismiss Changes
        /// </summary>
        /// <returns></returns>
        private int DismissChanges()
        {
            int i = 0;

            foreach (var entry in Context.ChangeTracker.Entries())
            {
                i++;
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }

            return i;
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
                    Context.Dispose();
                    _transaction?.Dispose();
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
