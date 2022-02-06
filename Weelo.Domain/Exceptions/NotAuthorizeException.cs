namespace Weelo.Domain.Exceptions;

public class NotAuthorizeException : BaseException
{
    public NotAuthorizeException(string message, IEnumerable<string>? errors = null)
        : base(401, message, errors)
    {
    }
}