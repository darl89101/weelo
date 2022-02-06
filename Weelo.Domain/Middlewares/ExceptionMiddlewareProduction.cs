using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Weelo.Domain.Commons;
using Weelo.Domain.Exceptions;

namespace Weelo.Domain.Middlewares;

/// <summary>
/// Exception middleware to production environment
/// </summary>
public class ExceptionMiddlewareProduction
{
    #region Properties
    private readonly RequestDelegate _next;
    #endregion

    #region Builder
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="next"></param>
    public ExceptionMiddlewareProduction(RequestDelegate next)
    {
        _next = next;
    }
    #endregion

    #region Implementation
    /// <summary>
    /// Invoke
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
            SetResponse(context, ex.CodeError, ex.Message, ex.Errors?.ToArray());
        }
        catch (Exception ex)
        {
            SetResponse(context, 500, "Unknown error, please contact administrator", new string[] { ex.Message });
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
    /// <param name="_"></param>
    private static async void SetResponse(HttpContext context, int code, string description, string[]? errors)
    {
        context.Response.Clear();
        context.Response.StatusCode = code;
        context.Response.ContentType = Const.APPLICATION_JSON;

        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
        {
            Code = code,
            Description = description,
            Errors = errors,
            Trace = string.Empty
        }));
    }
    #endregion
}

