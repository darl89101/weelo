namespace Weelo.Domain.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string message, IEnumerable<string>? errors = null)
        : base(400, message, errors)
    {
    }
}
