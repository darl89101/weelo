using Microsoft.AspNetCore.Mvc.Filters;
using System.Transactions;
using Weelo.Domain.Interfaces.Base;

namespace Weelo.Domain.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class UseTransactionAttribute : ActionFilterAttribute
{
    #region Properties
    /// <summary>
    /// Transaction
    /// </summary>
    private TransactionScope _scope;
    /// <summary>
    /// TimeLife transaction
    /// </summary>
    public int DefaultTimeOut { get; set; }
    /// <summary>
    /// Isolation level
    /// </summary>
    public IsolationLevel IsolationLevel { get; set; }
    /// <summary>
    /// Scope option
    /// </summary>
    public TransactionScopeOption TransactionScopeOption { get; set; }
    #endregion

    #region Builder
    /// <summary>
    /// Builder
    /// </summary>
    public UseTransactionAttribute()
    {
    }
    #endregion

    #region Implemetation
    /// <summary>
    /// Ejecuta el atributo e inicia la transacción
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        try
        {
            var _uow = context.HttpContext.RequestServices.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            _scope = _uow?.BeginTransaction(TransactionScopeOption, IsolationLevel, DefaultTimeOut);

            base.OnActionExecuting(context);
        }
        catch
        {
            _scope?.Dispose();
            throw;
        }
    }

    /// <summary>
    /// al finalizar el método destruye la transacción si ha generado error
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
        {
            _scope?.Dispose();
        }

        base.OnActionExecuted(context);
    }

    /// <summary>
    /// Se dispara al terminar de consumir el servicio y completa la transacción
    /// </summary>
    /// <param name="context"></param>
    public override void OnResultExecuted(ResultExecutedContext context)
    {
        base.OnResultExecuted(context);
        _scope.Complete();
        _scope.Dispose();
    }
    #endregion
}
