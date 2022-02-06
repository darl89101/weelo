using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Weelo.Domain.Interfaces.Base
{
    /// <summary>
    /// Unit of work
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Context
        /// </summary>
        DbContext Context { get; }
        /// <summary>
        /// Starts a new transaction
        /// </summary>
        /// <returns></returns>
        TransactionScope BeginTransaction();
        /// <summary>
        /// Starts a new transaction
        /// </summary>
        /// <param name="option"></param>
        /// <param name="isolation"></param>
        /// <param name="defaultTimeOut"></param>
        /// <returns></returns>
        TransactionScope BeginTransaction(
            TransactionScopeOption? option,
            IsolationLevel? isolation,
            int? defaultTimeOut
        );
        /// <summary>
        /// Commit pending changes
        /// </summary>
        /// <param name="completeTransaction"></param>
        /// <returns></returns>
        Task CommitAsync(bool completeTransaction = false);
        /// <summary>
        /// Rollback pending changes
        /// </summary>
        void Rollback();
    }
}
