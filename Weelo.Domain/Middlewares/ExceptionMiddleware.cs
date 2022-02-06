using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Weelo.Domain.Abstract;
using Weelo.Domain.Commons;
using Weelo.Domain.Exceptions;

namespace Weelo.Domain.Middlewares;

/// <summary>
/// Exception Middleware to dev environment
/// </summary>
public class ExceptionMiddleware
{
    #region Properties    
    private readonly RequestDelegate _next;
    #endregion

    #region Builder
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="next"></param>
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    #endregion

    #region Implementation
    /// <summary>
    /// invoke
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (BaseException ex)
        {
            SetResponse(context, ex.CodeError, ex.Message, ex.Errors?.ToArray(), ex);
        }
        catch (Exception ex)
        {
            SetResponse(context, 500, ex.Message, null, ex);
        }
    }
    #endregion

    #region Privates
    /// <summary>
    /// set a response
    /// </summary>
    /// <param name="context"></param>
    /// <param name="code"></param>
    /// <param name="description"></param>
    /// <param name="errors"></param>
    /// <param name="ex"></param>
    private static async void SetResponse(HttpContext context, int code, string description, string[]? errors, Exception ex)
    {
        context.Response.Clear();
        context.Response.StatusCode = code;
        context.Response.ContentType = Const.APPLICATION_JSON;

        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
        {
            Code = code,
            Description = description,
            Errors = errors,
            Trace = ex.ToDetailedString()
        }));
    }
    #endregion
}
