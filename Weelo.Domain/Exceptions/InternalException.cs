namespace Weelo.Domain.Exceptions;

public class InternalException : BaseException
{
    public InternalException(string message, IEnumerable<string>? errors = null)
        : base(500, message, errors)
    {
    }
}
